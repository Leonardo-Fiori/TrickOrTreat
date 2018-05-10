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
}
