using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Scriptable object contenente il numero di tile per ciascun
 * tipo, usato dal gameManager per istanziare una Map.
 * Modificabile da editor!
*/

[CreateAssetMenu(fileName = "Tileset", menuName = "Tileset", order = 1)]
public class Tileset : ScriptableObject
{
    public int quantiCorridoi = 12;
    public int quantiAngoli = 12;
    public int quantiTrivia = 25;
    public int quanteChiavi = 2;
    public int quanteCaramelle = 5;
    public int quanteScarpette = 5;
    public int quantiQuadrivia = 0;
    public int quantiPetardi = 0;
    public int despawnPetardi = 2;
}
