using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty Level")]
public class SDifficulty : ScriptableObject {
    public int value;
    
    public void ChangeDifficulty(int valoreDifficoltà)
    {
        value = valoreDifficoltà;
    }
}


