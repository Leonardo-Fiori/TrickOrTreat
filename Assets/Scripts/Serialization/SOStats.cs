using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Stats")]
public class SOStats : ScriptableObject {
    // Numero di vittorie nelle rispettive modalità
    public int easy;
    public int normal;
    public int hard;
}
