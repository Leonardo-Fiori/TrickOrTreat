using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    public void Think()
    {
        GetComponent<Animator>().SetTrigger("death");
    }
}
