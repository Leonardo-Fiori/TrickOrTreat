using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Gestisce la mappa e la sua generazione.
 * Contiene una matrice di tile e una reference ad un tileset con cui generare randomicamente la mappa
 * si può regolare la dimensione della mappa nella chiamata al costruttore dal gamemanager.
 * E' istanziata dal gameManager, non toccare.
*/

namespace UnityEngine
{
    public class Mappa
    {
        private MapTile[,] tiles;
        private Tileset tileSet;
        public int dim;
        private const int DEF_DIM = 7;
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

        public Mappa(int size)
        {
            dim = size;
            //Debug.Log(dim);
            if ((dim % 2) == 0) throw new Exception("Errore! Lato mappa non dispari!");
            tiles = new MapTile[dim, dim];
            keys = new bool[dim, dim];
            caramelle = new bool[dim, dim];
            scarpette = new bool[dim, dim];
            scarpetteDaRespawnare = new List<MapTile>();
            petardi = new bool[dim, dim];
        }

        public Mappa() : this(DEF_DIM) { }

        public void setTileSet(Tileset set)
        {
            tileSet = (Tileset)ScriptableObject.CreateInstance("Tileset");
            tileSet.quantiAngoli = set.quantiAngoli;
            tileSet.quantiCorridoi = set.quantiCorridoi;
            tileSet.quantiTrivia = set.quantiTrivia;
            tileSet.quantiQuadrivia = set.quantiQuadrivia;
            quanteChiavi = set.quanteChiavi;
            quanteCaramelle = set.quanteCaramelle;
            quanteScarpette = set.quanteScarpette;
            quantiPetardi = set.quantiPetardi;
        }

        public string show()
        {
            string res = "";

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
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
                int y = Random.Range(0, dim - 1);
                int x = Random.Range(0, dim - 1);
                int safetyCounter = 0;
                while (!LocationIsOk(x, y))
                {
                    y = Random.Range(0, dim - 1);
                    x = Random.Range(0, dim - 1);
                    if (safetyCounter++ > dim * dim * 1000)
                        throw new Exception("Non ho trovato un posto libero per piazzare un petardo!");
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
                int y = Random.Range(0, dim - 1);
                int x = Random.Range(0, dim - 1);
                int safetyCounter = 0;
                while (!LocationIsOk(x, y) || keys[x, y] || caramelle[x, y])
                {
                    y = Random.Range(0, dim - 1);
                    x = Random.Range(0, dim - 1);
                    if (safetyCounter++ > dim * dim * 1000)
                        throw new Exception("Non ho trovato un posto libero per piazzare una scarpetta!");
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
                int y = Random.Range(0, dim - 1);
                int x = Random.Range(0, dim - 1);
                int safetyCounter = 0;
                while (!LocationIsOk(x, y) || keys[x,y] || scarpette[x,y])
                {
                    y = Random.Range(0, dim - 1);
                    x = Random.Range(0, dim - 1);
                    if (safetyCounter++ > dim * dim * 1000)
                        throw new Exception("Non ho trovato un posto libero per piazzare una caramella!");
                }
                caramelle[x, y] = true;
                tiles[x, y].SetCaramella(true);
            }

            return;
        }

        private void SpawnMappa()
        {
            int center = (dim / 2);

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
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
                int y = Random.Range(0, dim - 1);
                int x = Random.Range(0, dim - 1);
                while (!LocationIsOk(x, y) || scarpette[x,y] || caramelle[x,y] )
                {
                    y = Random.Range(0, dim - 1);
                    x = Random.Range(0, dim - 1);
                }
                keys[x, y] = true;
                tiles[x, y].SetKey(true);
            }

            return;
        }

        private void SpawnUscita()
        {
            int y = Random.Range(0, dim - 1);
            int x = Random.Range(0, dim - 1);

            while (!LocationIsOk(x, y) || scarpette[x, y] || caramelle[x, y] || keys[x,y])
            {
                y = Random.Range(0, dim - 1);
                x = Random.Range(0, dim - 1);
            }

            uscitaX = x;
            uscitaY = y;
            tiles[x, y].SetUscita(true);
        }

        private void AntiLockdown()
        {
            int center = (dim / 2);

            //Debug.Log(dim+" "+center);

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

            for(int i = 0; i < dim; i++)
            {
                for(int j = 0; j < dim; j++)
                {
                    int counter = 0;

                    while(counter < 4 && isLocked(i, j))
                    {
                        tiles[i, j].rotate(true);
                        counter++;
                    }
                }
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
                y = (y + 1) % dim;
            }
            else if (dir == Direction.sud)
            {
                y = (y - 1) % dim;
            }
            else if (dir == Direction.est)
            {
                x = (x + 1) % dim;
            }
            else if (dir == Direction.ovest)
            {
                x = (x - 1) % dim;
            }

            if (y == -1) y = dim - 1;
            if (x == -1) x = dim - 1;

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
            if ((x > ((dim / 2) - 2) && x < ((dim / 2) + 2)) && (y > ((dim / 2) - 2) && y < ((dim / 2) + 2)))
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

        private bool aviableTiles()
        {
            return tileSet.quantiCorridoi > 0 || tileSet.quantiAngoli > 0 || tileSet.quantiTrivia > 0 || tileSet.quantiQuadrivia > 0;
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

                if (type == 0 && tileSet.quantiCorridoi > 0)
                {
                    tileSet.quantiCorridoi--;
                    return (TileType)type;
                }
                else if (type == 1 && tileSet.quantiAngoli > 0)
                {
                    tileSet.quantiAngoli--;
                    return (TileType)type;
                }
                else if (type == 2 && tileSet.quantiTrivia > 0)
                {
                    tileSet.quantiTrivia--;
                    return (TileType)type;
                }
                else if (type == 3 && tileSet.quantiQuadrivia > 0)
                {
                    tileSet.quantiQuadrivia--;
                    return (TileType)type;
                }
            }
            throw new Exception("Errore! Tiles nel TileSet esauriti.");
        }

        public MapTile getTile(int x, int y)
        {
            //Debug.Log(x + " " + y);
            if (x >= dim || y >= dim) throw new Exception("Errore: è stato richiesto un tile fuori dalla mappa.");
            return tiles[x, y];
        }
    }
}