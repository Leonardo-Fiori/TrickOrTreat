using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Richiede un Tileset per generare la mappa ed un Giocatore (scriptable object)
 * Prepara la partita generando la mappa e posizionando il giocatore
 * Mantiene delle reference statiche pubbliche per accedere a mappa, movement manager e giocatore.
*/

public class GameManager : MonoBehaviour {

    // Riferimenti statici pubblici ai componenti
    public static Mappa mapInstance;
    public static MovementManager movementManagerInstance;
    public static Giocatore playerInstance;
    public static GameObject playerPrefabInstance;
    public static bool moving = false;
    public static FrontEndTileManager frontEndTileManagerInstance;
    public static GameObject witchPrefabInstance;
    public static Strega witchInstance;
    public static Turno turno;

    // Valori pubblici di configurazione
    public int dimensioneMappa = 7;
    public Tileset tileset;
    public Giocatore statGiocatore;
    public GameObject prefabGiocatore;
    public float distanzaTraTile;
    public float scalaTiles;
    public static bool debugMode;
    public GameObject prefabStrega;
    public Strega strega;

    // Prefabs per generazione mappa
    public GameObject prefabQuadrivia;
    public GameObject prefabCorridoio;
    public GameObject prefabTrivia;
    public GameObject prefabAngolo;

    void Start () {
        playerInstance = statGiocatore;
        playerPrefabInstance = prefabGiocatore;
        playerPrefabInstance.SetActive(false);

        witchInstance = strega;
        witchPrefabInstance = prefabStrega;
        witchInstance.SetFrontEndPrefab(witchPrefabInstance);
        witchPrefabInstance.SetActive(false);

        mapInstance = new Mappa(dimensioneMappa);

        movementManagerInstance = new MovementManager();

        mapInstance.setTileSet(tileset);
        mapInstance.randomize();

        spawnFrontEnd();

        frontEndTileManagerInstance = gameObject.AddComponent<FrontEndTileManager>();
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

                GameObject prefab = null;
                if (tileType == TileType.quadrivio)
                    prefab = prefabQuadrivia;
                if (tileType == TileType.trivio)
                    prefab = prefabTrivia;
                if (tileType == TileType.angolo)
                    prefab = prefabAngolo;
                if (tileType == TileType.corridoio)
                    prefab = prefabCorridoio;

                Quaternion rotazione = prefab.transform.rotation;
                float rotazioneY = Mathf.Floor(prefab.transform.rotation.eulerAngles.y/10f)*10f;
                if (tileRotation == Rotation.su)
                    rotazione = prefab.transform.rotation;
                if (tileRotation == Rotation.giu)
                    rotazione = Quaternion.Euler(0, rotazioneY + 180, 0);
                if (tileRotation == Rotation.destra)
                    rotazione = Quaternion.Euler(0, rotazioneY + 90, 0);
                if (tileRotation == Rotation.sinistra)
                    rotazione = Quaternion.Euler(0, rotazioneY - 90, 0);

                GameObject tile = Instantiate(prefab, posizione, rotazione);
                mapInstance.getTile(i, j).setPrefab(tile);

                TileMovement tm = tile.GetComponent<TileMovement>();
                tm.SetTileX(i);
                tm.SetTileY(j);

                mapInstance.getTile(i, j).setFog(true);
            }
        }

        playerPrefabInstance.GetComponent<MovePlayer>().move(playerInstance.getX(), playerInstance.getY(), true);
        playerPrefabInstance.SetActive(true);

        strega.Spawn();
        witchPrefabInstance.SetActive(true);

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) debugMode = !debugMode;

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
