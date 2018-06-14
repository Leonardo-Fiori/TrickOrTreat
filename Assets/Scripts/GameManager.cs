using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Riferimenti statici pubblici ai componenti & altri valori interni
    public static GameManager instance;
    public static Mappa mapInstance;
    public static MovementManager movementManagerInstance;
    public static Giocatore playerInstance;
    public static GameObject playerPrefabInstance;
    public static GameObject witchPrefabInstance;
    public static Strega witchInstance;
    public static Turno turno;
    public static float tileDistance;
    public static bool debugMode;
    public static bool cheatMode;
    public static CameraManagerIsometric cameraManagerInstance;
    private static GameObject[] frontEndTileInstances;
    private int dimensioneMappa = 0;

    // Valori pubblici di configurazione
    [Header("Moduli di Gioco in ordine di Difficoltà")]
    public SDifficulty difficulty;
    public int[] dimensioniMappa;
    public Tileset[] tilesets;
    public Giocatore[] moduliGiocatore;
    public Strega[] moduliStrega;

    [Header("Configurazioni Mappa")]
    public float distanzaTraTile;
    public float scalaTiles;
    public float witchDelay;

    [Header("Reference a Componenti in Scena")]
    public CameraManagerIsometric cameraManager;
    public GameObject prefabGiocatore;
    public GameObject prefabStrega;

    // Prefabs per generazione mappa
    [Header("Prefabs Mappa")]
    public List<GameObject> prefabsQuadrivia;
    public List<GameObject> prefabsCorridoio;
    public List<GameObject> prefabsTrivia;
    public List<GameObject> prefabsAngolo;

    // Roba petardi
    private int turniPassati = 0;
    private List<MapTile> petardiDaDespawnare;
    [Header("Particles Petardo")]
    public GameObject particlePetardoPiazzato;
    public GameObject particlePetardoEsploso;

    // Centralizzazione comandi
    [Header("Centralizzazione Comandi")]
    public SOControls controlli;
    public static SOControls controls;

    // Eventi
    [Header("Eventi di Gioco")]
    public SOEvent eventoMorteGiocatore;
    public SOEvent eventoVittoriaGiocatore;
    public SOEvent eventoFineAnimazioneGiocatore;
    public SOEvent eventoMovimentoGiocatore;
    public SOEvent eventoMovimentoStrega;
    public SOEvent eventoScarpettaPresa;
    public SOEvent eventoCaramellaPresa;
    public SOEvent eventoChiavePresa;
    public SOEvent eventoPetardoPreso;
    public SOEvent eventoPetardoScoppiato;
    public SOEvent eventoPetardoUsato;
    public SOEvent eventoScomparsaNebbia;
    public SOEvent eventoFineAnimazioneStrega;
    public SOEvent eventoTilePericoloseAggiornate;
    public SOEvent eventoTurnoGiocatore;
    public SOEvent eventoTurnoStrega;
    public SOEvent eventoHoveringTile;
    public SOEvent eventoRisalitaTileIniziata;
    public SOEvent eventoRisalitaTileFinita;
    public SOEvent eventoDiscesaTileIniziata;
    public SOEvent eventoDiscesaTileFinita;
    public SOEvent eventoHoveringStrega;
    public SOEvent eventoWitchUntoggled;
    public SOEvent eventoWitchToggled;
    public SOEvent eventoPortaSbloccata;
    public SOEvent eventoWarpTile;
    public SOEvent eventoModalitaPetardoUntoggled;
    public SOEvent eventoModalitaPetardoToggled;

    // Inizializzazioni varie
    void OnEnable ()
    {
        // Inizializzo alcuni riferimenti pubbllici statici:
        instance = this;
        controls = controlli;
        petardiDaDespawnare = new List<MapTile>();
        cameraManagerInstance = cameraManager;
        tileDistance = distanzaTraTile;

        // Inizializzo il Player:
        playerInstance = moduliGiocatore[difficulty.value];
        playerPrefabInstance = prefabGiocatore;
        playerInstance.SetFrontEndPrefab(playerPrefabInstance);
        playerPrefabInstance.SetActive(false);

        // Inizializzo la Strega:
        witchInstance = moduliStrega[difficulty.value];
        witchPrefabInstance = prefabStrega;
        witchInstance.SetFrontEndPrefab(witchPrefabInstance);
        witchPrefabInstance.SetActive(false);

        // Inizializzo la Mappa:
        dimensioneMappa = dimensioniMappa[difficulty.value];
        mapInstance = new Mappa(dimensioneMappa);

        // Inizializzo il Movement Manager:
        movementManagerInstance = new MovementManager();

        // Finisco di inizializzare la Mappa:
        mapInstance.setTileSet(tilesets[difficulty.value]);
        mapInstance.randomize();

        // Spawno il Front End:
        spawnFrontEnd();
    }

    // Invoca il ReloadScene in 1 secondo
    public void Restart()
    {
        Invoke("ReloadScene", 1f);
    }

    // Ricarica la scena dopo averla distrutta e chiama la resetcomponents
    private void ReloadScene()
    {
        ResetComponents();

        foreach (GameObject tile in frontEndTileInstances)
        {
            Destroy(tile);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Resetta i componenti che persistono al reload
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

    // Chiude il gioco
    public void Quit()  
    {
        #if UNITY_EDITOR       

        UnityEditor.EditorApplication.isPlaying = false;        // levo la playermode se fossi da editor
        
        #else
     
        Application.Quit();

        #endif
    }

    // Cambia il turno e calcoli annessi
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

            //SoundManager.instance.Play("playerturn");
            eventoTurnoGiocatore.Raise();

            turno = Turno.giocatore;

            witchInstance.ResetMosseFatte();

            witchInstance.ResetPetardo();

            //cameraManagerInstance.SwitchSubject(playerPrefabInstance);
        }
        else
        {
            //SoundManager.instance.Play("witchturn");
            eventoTurnoStrega.Raise();

            turno = Turno.strega;

            playerInstance.ResetMosseFatte();

            witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement();

            //cameraManagerInstance.SwitchSubject(witchPrefabInstance);
        }

        cameraManager.SwitchSubject();

    }

    // Utilità: restituisce un gameobject random da una lista di gameobject passata
    private GameObject RandomFromList(List<GameObject> list)
    {
        int random = Random.Range(0, list.Count);
        return list[random];
    }

    // Restituisce un prefab da mettere nel front end in base al tiletype
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

    // Restituisce un quaternione da assegnare al tile appena istanziato in base alla tilerotation
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

    // Disattiva le quattro nuvolette di fog da subito
    private void deactivateSpawnFog()
    {
        mapInstance.getTile(playerInstance.getX(), playerInstance.getY()).setFog(false);
        mapInstance.getTile(playerInstance.getX() + 1, playerInstance.getY()).setFog(false);
        mapInstance.getTile(playerInstance.getX() - 1, playerInstance.getY()).setFog(false);
        mapInstance.getTile(playerInstance.getX(), playerInstance.getY() + 1).setFog(false);
        mapInstance.getTile(playerInstance.getX(), playerInstance.getY() - 1).setFog(false);
    }

    // Restituisce l'istanza di un tile front end date due coordinate
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

    // Controlla la pressione dei tasti
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

    // Despawna i petardi piazzati facendoli scoppiare
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

    // Aggiunge un petardo alla lista di quelli che scoppieranno
    public void AggiungiDespawnPetardo(MapTile tile)
    {
        petardiDaDespawnare.Add(tile);
    }

    // Resetta i componenti quando si disabilita
    private void OnDisable()
    {
        ResetComponents();
    }
}
