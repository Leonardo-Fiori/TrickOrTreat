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

        public Mappa(int size)
        {
            dim = size;
            //Debug.Log(dim);
            if ((dim % 2) == 0) throw new Exception("Errore! Lato mappa non dispari!");
            tiles = new MapTile[dim, dim];
        }

        public Mappa() : this(DEF_DIM) { }

        public void setTileSet(Tileset set)
        {
            tileSet = (Tileset)ScriptableObject.CreateInstance("Tileset");
            tileSet.quantiAngoli = set.quantiAngoli;
            tileSet.quantiCorridoi = set.quantiCorridoi;
            tileSet.quantiTrivia = set.quantiTrivia;
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

        private bool aviableTiles()
        {
            return tileSet.quantiCorridoi > 0 || tileSet.quantiAngoli > 0 || tileSet.quantiTrivia > 0;
        }

        // Basandosi sul tileset restituisce un tile random

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
                else if(type == 1 && tileSet.quantiAngoli > 0)
                {
                    tileSet.quantiAngoli--;
                    return (TileType)type;
                }
                else if (type == 2 && tileSet.quantiTrivia > 0)
                {
                    tileSet.quantiTrivia--;
                    return (TileType)type;
                }
            }
            throw new Exception("Errore! Tiles nel TileSet esauriti.");
        }
        
        // Restituisce una rotazione random tra su giu destra sinistra

        private Rotation randomRotation()
        {
            Rotation rot = (Rotation)Random.Range(0, 4); // da zero a tre, quattro rotazioni
            //Debug.Log(rot);
            return rot;
        }

        // Genera la mappa randomicamente basandosi sul tileset

        public void randomize()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    tiles[i, j] = new MapTile(i, j, randomTileType(), randomRotation());
                }
            }

            int center = (dim / 2);
            tiles[center, center] = new MapTile(center, center, TileType.quadrivio, Rotation.su);

            //Debug.Log(show());
        }

        // Restituisce uno specifico tile

        public MapTile getTile(int x, int y)
        {
            //Debug.Log(x + " " + y);
            if (x >= dim || y >= dim) throw new Exception("Errore: è stato richiesto un tile fuori dalla mappa.");
            return tiles[x, y];
        }
    }
}