using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Enumeratori di:
 *  Tipo tile
 *  Rotazione tile
 *  Direzione cardinale
 *  
 *  Usati dalle altre classi / oggetti.
*/

public enum Turno
{
    giocatore,
    strega
}

public enum TileType
{
    corridoio = 0,
    angolo = 1,
    trivio = 2,
    quadrivio = 3
}

public enum Rotation
{
    su = 0,
    giu = 1,
    destra = 2,
    sinistra = 3
}

public enum Direction
{
    nord = 0,
    sud = 1,
    est = 2,
    ovest = 3
}