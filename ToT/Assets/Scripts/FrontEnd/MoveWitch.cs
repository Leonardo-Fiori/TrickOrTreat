using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWitch : MonoBehaviour {

    public float factor = 1f;
    public float offsetY = 1f;

    public void Move(int x, int z)
    {
        transform.position = new Vector3(x * factor, offsetY, z * factor);
        return;
    }

}
