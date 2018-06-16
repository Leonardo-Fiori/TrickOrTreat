using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mappa")]
public class Mappa : ScriptableObject
{
    // Valori pubblici di configurazione
    private Tileset internalTileset;
    public Tileset tileset;
    public int dimensione;
    private const int DEF_DIM = 7;

    // Prefabs della mappa
    public List<GameObject> prefabsQuadrivia;
    public List<GameObject> prefabsCorridoio;
    public List<GameObject> prefabsTrivia;
    public List<GameObject> prefabsAngolo;

    // Valori privati interni
    private MapTile[,] tiles;
    private int uscitaX = -1;
    private int uscitaY = -1;
    private bool[,] keys;
    private int quanteChiavi;
    private bool[,] caramelle;
    private int quanteCaramelle;
    private bool[,] scarpette;
    private int quanteScarpette;
    private bool[,] petardi;
    private int quantiPetardi;

    // Per il respawn dei pickup scarpetta
    // Pubblico per velocizzare accesso
    [HideInInspector]
    public List<MapTile> scarpetteDaRespawnare;

    #region"Getter e Setter"

    public int GetQuanteChiavi()
    {
        return quanteChiavi;
    }

    public int GetUscitaX()
    {
        return uscitaX;
    }

    public int GetUscitaY()
    {
        return uscitaY;
    }

    #endregion

    public void Initialize()
    {
        if ((dimensione % 2) == 0) throw new System.Exception("Errore! Lato mappa non dispari!");
        tiles = new MapTile[dimensione, dimensione];
        keys = new bool[dimensione, dimensione];
        caramelle = new bool[dimensione, dimensione];
        scarpette = new bool[dimensione, dimensione];
        scarpetteDaRespawnare = new List<MapTile>();
        petardi = new bool[dimensione, dimensione];

        setTileSet(tileset);
        randomize();
        spawnFrontEnd();
    }

    public void Reset()
    {
        tiles = null;
        keys = null;
        caramelle = null;
        scarpette = null;
        scarpetteDaRespawnare = null;
        petardi = null;
    }

    public void setTileSet(Tileset set)
    {
        internalTileset = (Tileset)ScriptableObject.CreateInstance("Tileset");
        internalTileset.quantiAngoli = set.quantiAngoli;
        internalTileset.quantiCorridoi = set.quantiCorridoi;
        internalTileset.quantiTrivia = set.quantiTrivia;
        internalTileset.quantiQuadrivia = set.quantiQuadrivia;
        quanteChiavi = set.quanteChiavi;
        quanteCaramelle = set.quanteCaramelle;
        quanteScarpette = set.quanteScarpette;
        quantiPetardi = set.quantiPetardi;
    }

    public string show()
    {
        string res = "";

        for (int i = 0; i < dimensione; i++)
        {
            for (int j = 0; j < dimensione; j++)
            {
                if (tiles[i, j].getTileType() == TileType.angolo)
                    res += "A";
                if (tiles[i, j].getTileType() == TileType.corridoio)
                    res += "C";
                if (tiles[i, j].getTileType() == TileType.quadrivio)
                    res += "Q";
                if (tiles[i, j].getTileType() == TileType.trivio)
                    res += "T";
            }
            res += "\n";
        }
        return res;
    }

    public void randomize()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        // Spawn mappa
        SpawnMappa();

        // Anti lockdown 
        AntiLockdown();

        // Posizionamento uscita
        SpawnUscita();

        // Spawna le chiavi
        SpawnChiavi();

        // Spawna caramelle
        SpawnCaramelle();

        // Spawna scarpette
        SpawnScarpette();

        // Spawna petardi
        SpawnPetardi();

    }

    private void SpawnPetardi()
    {
        for (int i = 0; i < quantiPetardi; i++)
        {
            int y = Random.Range(0, dimensione - 1);
            int x = Random.Range(0, dimensione - 1);
            int safetyCounter = 0;
            while (!LocationIsOk(x, y))
            {
                y = Random.Range(0, dimensione - 1);
                x = Random.Range(0, dimensione - 1);
                if (safetyCounter++ > dimensione * dimensione * 1000)
                    throw new System.Exception("Non ho trovato un posto libero per piazzare un petardo!");
            }
            petardi[x, y] = true;
            tiles[x, y].SetPetardo(true);
            //Debug.Log(x + " " + y + " petardo = " + tiles[x, y].HasPetardo());
        }

        return;
    }

    private void SpawnScarpette()
    {
        for (int i = 0; i < quanteScarpette; i++)
        {
            int y = Random.Range(0, dimensione - 1);
            int x = Random.Range(0, dimensione - 1);
            int safetyCounter = 0;
            while (!LocationIsOk(x, y) || keys[x, y] || caramelle[x, y])
            {
                y = Random.Range(0, dimensione - 1);
                x = Random.Range(0, dimensione - 1);
                if (safetyCounter++ > dimensione * dimensione * 1000)
                    throw new System.Exception("Non ho trovato un posto libero per piazzare una scarpetta!");
            }
            scarpette[x, y] = true;
            tiles[x, y].SetScarpetta(true);
        }

        return;
    }

    private void SpawnCaramelle()
    {
        for (int i = 0; i < quanteCaramelle; i++)
        {
            int y = Random.Range(0, dimensione - 1);
            int x = Random.Range(0, dimensione - 1);
            int safetyCounter = 0;
            while (!LocationIsOk(x, y) || keys[x, y] || scarpette[x, y])
            {
                y = Random.Range(0, dimensione - 1);
                x = Random.Range(0, dimensione - 1);
                if (safetyCounter++ > dimensione * dimensione * 1000)
                    throw new System.Exception("Non ho trovato un posto libero per piazzare una caramella!");
            }
            caramelle[x, y] = true;
            tiles[x, y].SetCaramella(true);
        }

        return;
    }

    private void SpawnMappa()
    {
        int center = (dimensione / 2);

        for (int i = 0; i < dimensione; i++)
        {
            for (int j = 0; j < dimensione; j++)
            {
                tiles[i, j] = new MapTile(i, j, randomTileType(), randomRotation());
            }
        }


        tiles[center, center] = new MapTile(center, center, TileType.quadrivio, Rotation.su);
    }

    private void SpawnChiavi()
    {
        for (int i = 0; i < quanteChiavi; i++)
        {
            int y = Random.Range(0, dimensione - 1);
            int x = Random.Range(0, dimensione - 1);
            bool keyNord = !getNextTile(x, y, Direction.nord).HasKey();
            bool keySud = !getNextTile(x, y, Direction.nord).HasKey();
            bool keyEst = !getNextTile(x, y, Direction.nord).HasKey();
            bool keyOvest = !getNextTile(x, y, Direction.nord).HasKey();
            while (!LocationIsOk(x, y) && (keyNord || keySud || keyEst || keyOvest))
            {
                y = Random.Range(0, dimensione - 1);
                x = Random.Range(0, dimensione - 1);
            }
            keys[x, y] = true;
            tiles[x, y].SetKey(true);
        }

        return;
    }

    private void SpawnUscita()
    {
        int y = Random.Range(0, dimensione - 1);
        int x = Random.Range(0, dimensione - 1);

        while (!LocationIsOk(x, y) || scarpette[x, y] || caramelle[x, y] || keys[x, y])
        {
            y = Random.Range(0, dimensione - 1);
            x = Random.Range(0, dimensione - 1);
        }

        uscitaX = x;
        uscitaY = y;
        tiles[x, y].SetUscita(true);
    }

    private void AntiLockdown()
    {
        int center = (dimensione / 2);

        //Debug.Log(dim+" "+center);

        for (int i = 0; i < dimensione; i++)
        {
            for (int j = 0; j < dimensione; j++)
            {
                int counter = 0;

                while (counter < 4 && isLocked(i, j))
                {
                    tiles[i, j].rotate(true);
                    counter++;
                }
            }
        }

        while (!tiles[center + 1, center].getDirection(Direction.ovest))
        {
            tiles[center + 1, center].rotate(true);
        }

        while (!tiles[center - 1, center].getDirection(Direction.est))
        {
            tiles[center - 1, center].rotate(true);
        }

        while (!tiles[center, center + 1].getDirection(Direction.sud))
        {
            tiles[center, center + 1].rotate(true);
        }

        while (!tiles[center, center - 1].getDirection(Direction.nord))
        {
            tiles[center, center - 1].rotate(true);
        }

        return;
    }

    public bool isLocked(int x, int y)
    {
        return !canMoveAt(Direction.est, x, y) && !canMoveAt(Direction.ovest, x, y) && !canMoveAt(Direction.sud, x, y) && !canMoveAt(Direction.nord, x, y);
    }

    public MapTile getNextTile(int x, int y, Direction dir)
    {
        if (dir == Direction.nord)
        {
            y = (y + 1) % dimensione;
        }
        else if (dir == Direction.sud)
        {
            y = (y - 1) % dimensione;
        }
        else if (dir == Direction.est)
        {
            x = (x + 1) % dimensione;
        }
        else if (dir == Direction.ovest)
        {
            x = (x - 1) % dimensione;
        }

        if (y == -1) y = dimensione - 1;
        if (x == -1) x = dimensione - 1;

        //Debug.Log(x + " " + y);
        return getTile(x, y);
    }

    public bool canMoveAt(Direction dir, int x, int y)
    {
        //Debug.Log("CONTROLLO SE PUO MUOVERSI IN " + dir);
        MapTile playerTile = getTile(x, y);
        //Debug.Log("Il player si trova su un " + playerTile.getTileType() + " " + playerTile.getTileRotation());
        MapTile nextTile = getNextTile(x, y, dir);
        //Debug.Log("E vuole andare su un " + nextTile.getTileType() + " " + nextTile.getTileRotation());

        if (nextTile.getX() == GameManager.witchInstance.GetX() && nextTile.getY() == GameManager.witchInstance.GetY())
            return false;

        // Se la strada nel tile attuale è sbarrata
        //if (!playerTile.getDirection(dir)) Debug.Log("Il playertile è chiuso a " + dir);
        if (!playerTile.getDirection(dir)) return false;

        // Controlla la strada nel tile di destinazione
        if (dir == Direction.nord)
        {
            if (!nextTile.getDirection(Direction.sud))
            {
                //Debug.Log("Il nextile è chiuso a sud");
                return false;
            }
        }
        else if (dir == Direction.sud)
        {
            if (!nextTile.getDirection(Direction.nord))
            {
                //Debug.Log("Il nextile è chiuso a nord");
                return false;
            }
        }
        else if (dir == Direction.est)
        {
            if (!nextTile.getDirection(Direction.ovest))
            {
                //Debug.Log("Il nextile è chiuso a ovest");
                return false;
            }
        }
        else if (dir == Direction.ovest)
        {
            if (!nextTile.getDirection(Direction.est))
            {
                //Debug.Log("Il nextile è chiuso a est");
                return false;
            }
        }

        // Se ha passato tutti i controlli allora può muovere!
        //Debug.Log("Può muoversi!");
        return true;
    }

    private bool LocationIsOk(int x, int y)
    {
        if ((x > ((dimensione / 2) - 2) && x < ((dimensione / 2) + 2)) && (y > ((dimensione / 2) - 2) && y < ((dimensione / 2) + 2)))
            return false;

        if (keys[x, y])
            return false;

        if (caramelle[x, y])
            return false;

        if (scarpette[x, y])
            return false;

        if (petardi[x, y])
            return false;

        if (uscitaX == x && uscitaY == y)
            return false;

        return true;
    }

    private bool IsLoctionNearToSpawn(int x, int y)
    {
        if ((x > ((dimensione / 2) - 2) && x < ((dimensione / 2) + 2)) && (y > ((dimensione / 2) - 2) && y < ((dimensione / 2) + 2)))
            return false;
        return true;
    }

    private bool aviableTiles()
    {
        return internalTileset.quantiCorridoi > 0 || internalTileset.quantiAngoli > 0 || internalTileset.quantiTrivia > 0 || internalTileset.quantiQuadrivia > 0;
    }

    private Rotation randomRotation()
    {
        Rotation rot = (Rotation)Random.Range(0, 4); // da zero a tre, quattro rotazioni
                                                     //Debug.Log(rot);
        return rot;
    }

    private TileType randomTileType()
    {
        while (aviableTiles())
        {
            int type = Random.Range(0, 4);
            //Debug.Log(type);

            if (type == 0 && internalTileset.quantiCorridoi > 0)
            {
                internalTileset.quantiCorridoi--;
                return (TileType)type;
            }
            else if (type == 1 && internalTileset.quantiAngoli > 0)
            {
                internalTileset.quantiAngoli--;
                return (TileType)type;
            }
            else if (type == 2 && internalTileset.quantiTrivia > 0)
            {
                internalTileset.quantiTrivia--;
                return (TileType)type;
            }
            else if (type == 3 && internalTileset.quantiQuadrivia > 0)
            {
                internalTileset.quantiQuadrivia--;
                return (TileType)type;
            }
        }
        throw new System.Exception("Errore! Tiles nel TileSet esauriti.");
    }

    public MapTile getTile(int x, int y)
    {
        //Debug.Log(x + " " + y);
        if (x >= dimensione || y >= dimensione) throw new System.Exception("Errore: è stato richiesto un tile fuori dalla mappa.");
        return tiles[x, y];
    }

    // Gestione front end

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
    public void spawnFrontEnd()
    {
        for (int i = 0; i < dimensione; i++)
        {
            for (int j = 0; j < dimensione; j++)
            {
                TileType tileType = getTile(i, j).getTileType();
                Rotation tileRotation = getTile(i, j).getTileRotation();

                Vector3 posizione = new Vector3(i * GameManager.instance.distanzaTraTile, 0f, j * GameManager.instance.distanzaTraTile);

                GameObject prefab = GetFrontEndTilePrefab(tileType);

                Quaternion rotazione = GetFrontEndTileRotation(prefab, tileRotation);

                //print(prefab + " " + rotazione + " " + posizione);
                GameObject tile = Instantiate(prefab, posizione, rotazione);
                getTile(i, j).setPrefab(tile);
                //print(tile);

                TileCoords.SetX(tile, i);
                TileCoords.SetY(tile, j);

                //print(tm.GetTileX() + " " + tm.GetTileY());

                if(!IsLoctionNearToSpawn(i,j))
                    getTile(i, j).setFog(true);
            }
        }

        GameManager.frontEndTileInstances = GameObject.FindGameObjectsWithTag("Tile");

        GameManager.playerInstance.move((dimensione / 2), (dimensione / 2), Movement.teleport);
        GameManager.playerInstance.ResetMosseFatte();
        GameManager.playerPrefabInstance.SetActive(true);

        GameManager.witchInstance.Spawn();
        GameManager.witchPrefabInstance.SetActive(true);

        /* Se disattivo una singola nebbia all'interno di questa funzione 
         * o prima che questa finisca (chiamata interna ad un altra funzione), 
         * tutte le nebbie vengono disattivate.
         * Dunque uso la invoke... mah! DA RIVEDERE, BUG
         */

        GameManager.instance.Invoke("deactivateSpawnFog", 1f);
    }
}