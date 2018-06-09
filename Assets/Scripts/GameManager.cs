using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Richiede un Tileset per generare la mappa ed un Giocatore (scriptable object)
 * Prepara la partita generando la mappa e posizionando il giocatore
 * Mantiene delle reference statiche pubbliche per accedere a mappa, movement manager e giocatore.
*/

public class GameManager : MonoBehaviour {

    // Riferimenti statici pubblici ai componenti
    public static GameManager instance;

    public static Mappa mapInstance;
    public static MovementManager movementManagerInstance;

    public static Giocatore playerInstance;
    public static GameObject playerPrefabInstance;

    public static GameObject witchPrefabInstance;
    public static Strega witchInstance;

    public static Turno turno;

    public static SOEvent playerMovementEvent;

    public static float tileDistance;

    public static CameraManagerIsometric cameraManagerInstance;

    // Valori pubblici di configurazione
    public SDifficulty difficulty;
    public int[] dimensioniMappa;
    public Tileset[] tilesets;
    public Giocatore[] moduliGiocatore;
    public Strega[] moduliStrega;

    private int dimensioneMappa = 7;     // diff
    private Tileset tileset;             // diff

    private Giocatore giocatore;         // diff
    public GameObject prefabGiocatore;

    public float distanzaTraTile;
    public float scalaTiles;

    public float witchDelay;

    public static bool debugMode;
    public static bool cheatMode;

    public GameObject prefabStrega;
    private Strega strega;               // diff

    public SOEvent eventoMovimento;

    public CameraManagerIsometric cameraManager;

    // Prefabs per generazione mappa
    public List<GameObject> prefabsQuadrivia;
    public List<GameObject> prefabsCorridoio;
    public List<GameObject> prefabsTrivia;
    public List<GameObject> prefabsAngolo;

    // Lista dei prefab front end delle tile
    private static GameObject[] frontEndTileInstances;

    private int turniPassati = 0;
    private List<MapTile> petardiDaDespawnare;
    public SOEvent eventoScoppioPetardo;
    public GameObject particlePetardoPiazzato;
    public GameObject particlePetardoEsploso;


    // Centralizzazione comandi
    public SOControls controlli;
    public static SOControls controls;

    void OnEnable () {

        instance = this;

        controls = controlli;

        petardiDaDespawnare = new List<MapTile>();

        cameraManagerInstance = cameraManager;

        tileDistance = distanzaTraTile;

        playerMovementEvent = eventoMovimento;

        // Inizializzo il player
        playerInstance = moduliGiocatore[difficulty.value];
        playerPrefabInstance = prefabGiocatore;
        playerInstance.SetFrontEndPrefab(playerPrefabInstance);
        playerPrefabInstance.SetActive(false);

        // Inizializzo la strega 
        witchInstance = moduliStrega[difficulty.value];
        witchPrefabInstance = prefabStrega;
        witchInstance.SetFrontEndPrefab(witchPrefabInstance);
        witchPrefabInstance.SetActive(false);

        // Inizializzo la mappa
        mapInstance = new Mappa(dimensioniMappa[difficulty.value]);

        // Inizializzo il movement manager
        movementManagerInstance = new MovementManager();

        // Imposto la mappa
        mapInstance.setTileSet(tilesets[difficulty.value]);
        mapInstance.randomize();

        // Spawno il front end della mappa
        spawnFrontEnd();
    }

    public void Restart()
    {
        Invoke("ReloadScene", 1f);
    }

    public void Quit()  
    {
        #if UNITY_EDITOR       

        UnityEditor.EditorApplication.isPlaying = false;        // levo la playermode se fossi da editor
        
        #else
     
        Application.Quit();

        #endif
    }

    private void ResetComponents()
    {
        witchInstance.ResetMosseFatte();
        witchInstance.ResetPetardo();
        turno = Turno.giocatore;
        playerInstance.Reset();
        Giocatore.morto = false;
        MovePlayer.moving = false;
        MoveWitch.moving = false;
        TileMovement.canRot = true;
        MoveWarpTiles.animating = false;
    }

    private void ReloadScene()
    {
        ResetComponents();

        foreach (GameObject tile in frontEndTileInstances)
        {
            Destroy(tile);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SwitchTurn()
    {
        // Respawna scarpette
        turniPassati++;

        List<MapTile> toRemove = new List<MapTile>();

        for (int i = 0; i < mapInstance.scarpetteDaRespawnare.Count; i++)
        {
            MapTile tile = mapInstance.scarpetteDaRespawnare[i];

            bool sameX = (tile.getX() == playerInstance.getX());
            bool sameY = (tile.getY() == playerInstance.getY());
            bool samePos = (sameX && sameY);

            if (!samePos)
            {
                tile.SetScarpetta(true);
                tile.getPrefab().GetComponent<PickupSpawner>().SpawnScarpetta();
                toRemove.Add(tile);
            }
        }

        foreach (MapTile tile in toRemove)
        {
            mapInstance.scarpetteDaRespawnare.Remove(tile);
        }

        if (turno == Turno.strega)
        {
            DespawnPetardi();

            SoundManager.instance.Play("playerturn");

            turno = Turno.giocatore;

            witchInstance.ResetMosseFatte();

            witchInstance.ResetPetardo();

            //cameraManagerInstance.SwitchSubject(playerPrefabInstance);
        }
        else
        {
            SoundManager.instance.Play("witchturn");

            turno = Turno.strega;

            playerInstance.ResetMosseFatte();

            witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement();

            //cameraManagerInstance.SwitchSubject(witchPrefabInstance);
        }

        cameraManager.SwitchSubject();

    }

    private GameObject RandomFromList(List<GameObject> list)
    {
        int random = Random.Range(0, list.Count);
        return list[random];
    }

    public GameObject GetFrontEndTilePrefab(TileType tileType)
    {
        GameObject prefab = null;

        if (tileType == TileType.quadrivio)
            prefab = RandomFromList(prefabsQuadrivia);
        if (tileType == TileType.trivio)
            prefab = RandomFromList(prefabsTrivia);
        if (tileType == TileType.angolo)
            prefab = RandomFromList(prefabsAngolo);
        if (tileType == TileType.corridoio)
            prefab = RandomFromList(prefabsCorridoio);

        return prefab;
    }

    public Quaternion GetFrontEndTileRotation(GameObject prefab, Rotation tileRotation)
    {
        Quaternion rotazione = prefab.transform.rotation;

        float rotazioneY = Mathf.Floor(prefab.transform.rotation.eulerAngles.y / 10f) * 10f;
        if (tileRotation == Rotation.su)
            rotazione = prefab.transform.rotation;
        if (tileRotation == Rotation.giu)
            rotazione = Quaternion.Euler(0, rotazioneY + 180, 0);
        if (tileRotation == Rotation.destra)
            rotazione = Quaternion.Euler(0, rotazioneY + 90, 0);
        if (tileRotation == Rotation.sinistra)
            rotazione = Quaternion.Euler(0, rotazioneY - 90, 0);

        return rotazione;
    }

    // Spawna il frontend dopo aver preparato il back end
    private void spawnFrontEnd()
    {
        for (int i = 0; i < dimensioneMappa; i++)
        {
            for (int j = 0; j < dimensioneMappa; j++)
            {
                TileType tileType = mapInstance.getTile(i, j).getTileType();
                Rotation tileRotation = mapInstance.getTile(i, j).getTileRotation();

                Vector3 posizione = new Vector3(i * distanzaTraTile, 0f, j * distanzaTraTile);

                GameObject prefab = GetFrontEndTilePrefab(tileType);

                Quaternion rotazione = GetFrontEndTileRotation(prefab, tileRotation);

                //print(prefab + " " + rotazione + " " + posizione);
                GameObject tile = Instantiate(prefab, posizione, rotazione);
                mapInstance.getTile(i, j).setPrefab(tile);
                //print(tile);

                TileCoords.SetX(tile, i);
                TileCoords.SetY(tile, j);

                //print(tm.GetTileX() + " " + tm.GetTileY());

                mapInstance.getTile(i, j).setFog(true);
            }
        }

        frontEndTileInstances = GameObject.FindGameObjectsWithTag("Tile");

        playerInstance.move((mapInstance.dim / 2), (mapInstance.dim / 2), Movement.teleport);
        playerInstance.ResetMosseFatte();
        playerPrefabInstance.SetActive(true);

        witchInstance.Spawn();
        witchPrefabInstance.SetActive(true);

        /* Se disattivo una singola nebbia all'interno di questa funzione 
         * o prima che questa finisca (chiamata interna ad un altra funzione), 
         * tutte le nebbie vengono disattivate.
         * Dunque uso la invoke... mah! DA RIVEDERE, BUG
         */

        Invoke("deactivateSpawnFog", 1f);
    }

    private void deactivateSpawnFog()
    {
        mapInstance.getTile(playerInstance.getX(), playerInstance.getY()).setFog(false);
        mapInstance.getTile(playerInstance.getX() + 1, playerInstance.getY()).setFog(false);
        mapInstance.getTile(playerInstance.getX() - 1, playerInstance.getY()).setFog(false);
        mapInstance.getTile(playerInstance.getX(), playerInstance.getY() + 1).setFog(false);
        mapInstance.getTile(playerInstance.getX(), playerInstance.getY() - 1).setFog(false);
    }

    public static GameObject GetFrontEndTile(int x, int y)
    {
        foreach (GameObject tile in frontEndTileInstances)
        {
            if (TileCoords.GetX(tile) == x && TileCoords.GetY(tile) == y)
            {
                //print("trovato " + x + " " + y);
                return tile;
            }
        }

        Debug.Log("Front end tile manager: tile cercato non trovato");
        return null;
    }

    private void Update()
    {
        if (Input.GetKey(controls.debugHold) && Input.GetKeyDown(controls.debugPress))
        {
            debugMode = !debugMode;
            print("Debug Mode: " + debugMode);
        }

        if (Input.GetKeyDown(controls.restart))
        {
            Restart();
            return;
        }

        if (Input.GetKeyDown(controls.quit))
        {
            Quit();
        }

        if (Input.GetKeyDown(controls.passaTurno))
        {
            if(turno == Turno.giocatore)
                SwitchTurn();
        }

        if (debugMode)
        {
            if(Input.GetKeyDown(controls.debugFogOff))
            {
                TileFog[] tfs = FindObjectsOfType<TileFog>();
                foreach(TileFog tf in tfs)
                {
                    tf.SetFog(false);
                }
            }

            if (Input.GetKeyDown(controls.debugCheatMode))
            {
                cheatMode = !cheatMode;
                print("Cheatmode: " + cheatMode);
            }

            if (Input.GetKeyDown(controls.debugIncreaseDifficulty))
            {
                difficulty.value++;
                if (difficulty.value >= dimensioniMappa.Length) difficulty.value = dimensioniMappa.Length - 1;
                SoundManager.instance.Play("playermove");
                print("Selected difficulty level: " + difficulty.value);
            }

            if (Input.GetKeyDown(controls.debugDecreaseDifficulty))
            {
                difficulty.value--;
                if (difficulty.value < 0) difficulty.value = 0;
                SoundManager.instance.Play("playermove");
                print("Selected difficulty level: " + difficulty.value);
            }

            if(Input.GetKeyDown(KeyCode.F12) && Input.GetKeyDown(KeyCode.F11))
            {
                foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
                {
                    tile.GetComponent<FloatAnimation>().enabled = true;
                    SoundManager.instance.Play("win");
                }
            }
        }

        if (debugMode)
        {
            if (Input.GetKeyDown(controls.debugUp))
            {
                movementManagerInstance.movePlayer(Direction.nord);
            }
            if (Input.GetKeyDown(controls.debugLeft))
            {
                movementManagerInstance.movePlayer(Direction.ovest);
            }
            if (Input.GetKeyDown(controls.debugRight))
            {
                movementManagerInstance.movePlayer(Direction.est);
            }
            if (Input.GetKeyDown(controls.debugDown))
            {
                movementManagerInstance.movePlayer(Direction.sud);
            }
        }
    }

    private void DespawnPetardi()
    {
        if(turniPassati > tilesets[difficulty.value].despawnPetardi)
        {
            turniPassati = 0;
            foreach(MapTile tile in petardiDaDespawnare)
            {
                tile.ScoppiaPetardo();
            }
            petardiDaDespawnare.Clear();
        }
    }

    public void AggiungiDespawnPetardo(MapTile tile)
    {
        petardiDaDespawnare.Add(tile);
    }

    private void OnDisable()
    {
        ResetComponents();
    }
}
