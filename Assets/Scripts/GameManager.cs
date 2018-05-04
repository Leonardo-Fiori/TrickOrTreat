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
    public int dimensioneMappa = 7;
    public Tileset tileset;

    public Giocatore giocatore;
    public GameObject prefabGiocatore;

    public float distanzaTraTile;
    public float scalaTiles;

    public static bool debugMode;
    public static bool cheatMode;

    public GameObject prefabStrega;
    public Strega strega;

    public SOEvent eventoMovimento;

    public CameraManagerIsometric cameraManager;

    // Prefabs per generazione mappa
    public GameObject prefabQuadrivia;
    public GameObject prefabCorridoio;
    public GameObject prefabTrivia;
    public GameObject prefabAngolo;

    // Lista dei prefab front end delle tile
    private static GameObject[] frontEndTileInstances;

    void Start () {

        instance = this;

        cameraManagerInstance = cameraManager;

        tileDistance = distanzaTraTile;

        playerMovementEvent = eventoMovimento;

        // Inizializzo il player
        playerInstance = giocatore;
        playerPrefabInstance = prefabGiocatore;
        playerInstance.SetFrontEndPrefab(playerPrefabInstance);
        playerPrefabInstance.SetActive(false);

        // Inizializzo la strega
        witchInstance = strega;
        witchPrefabInstance = prefabStrega;
        witchInstance.SetFrontEndPrefab(witchPrefabInstance);
        witchPrefabInstance.SetActive(false);

        // Inizializzo la mappa
        mapInstance = new Mappa(dimensioneMappa);

        // Inizializzo il movement manager
        movementManagerInstance = new MovementManager();

        // Imposto la mappa
        mapInstance.setTileSet(tileset);
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

    private void ReloadScene()
    {
        playerInstance.ResetMosseFatte();
        witchInstance.ResetMosseFatte();
        turno = Turno.giocatore;
        Giocatore.chiaviRaccolte = 0;
        MovePlayer.moving = false;
        MoveWitch.moving = false;
        TileMovement.canRot = true;
        MoveWarpTiles.animating = false;

        foreach (GameObject tile in frontEndTileInstances)
        {
            Destroy(tile);
        }

        SceneManager.LoadScene("Main");
    }

    public GameObject GetFrontEndTilePrefab(TileType tileType)
    {
        GameObject prefab = null;

        if (tileType == TileType.quadrivio)
            prefab = prefabQuadrivia;
        if (tileType == TileType.trivio)
            prefab = prefabTrivia;
        if (tileType == TileType.angolo)
            prefab = prefabAngolo;
        if (tileType == TileType.corridoio)
            prefab = prefabCorridoio;

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

        strega.Spawn();
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
        if (Input.GetKeyDown(KeyCode.D)) debugMode = !debugMode;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }

        if (debugMode)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                TileFog[] tfs = FindObjectsOfType<TileFog>();
                foreach(TileFog tf in tfs)
                {
                    tf.SetFog(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                TileFog[] tfs = FindObjectsOfType<TileFog>();
                foreach (TileFog tf in tfs)
                {
                    tf.SetFog(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                cheatMode = !cheatMode;
                print("Cheatmode: " + cheatMode);
            }

            

            if (Input.GetKeyDown(KeyCode.W))
            {
                witchPrefabInstance.GetComponent<MoveWitch>().InvokeMovement(1f);
            }
        }

        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movementManagerInstance.movePlayer(Direction.nord);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movementManagerInstance.movePlayer(Direction.ovest);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movementManagerInstance.movePlayer(Direction.est);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movementManagerInstance.movePlayer(Direction.sud);
            }
        }
    }
}
