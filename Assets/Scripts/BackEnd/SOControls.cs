using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[CreateAssetMenu(menuName = "Controls Set")]
public class SOControls : ScriptableObject {
    public int rotazioneOraria;
    public int rotazioneAntioraria;
    public int standardClick;
    public int movimento;
    public int rotazioneCamera;
    public int zoomCamera;
    public KeyCode petardo;
    public KeyCode restart;
    public KeyCode quit;
    public KeyCode passaTurno;
    public KeyCode incrementaGrafica;
    public KeyCode decrementaGrafica;
    public KeyCode debugHold;
    public KeyCode debugPress;
    public KeyCode debugFogOff;
    public KeyCode debugCheatMode;
    public KeyCode debugUp;
    public KeyCode debugDown;
    public KeyCode debugRight;
    public KeyCode debugLeft;
}
