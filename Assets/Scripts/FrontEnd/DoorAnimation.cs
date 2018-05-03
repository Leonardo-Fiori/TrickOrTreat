using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : InflateAnimation {

    override public void Think()
    {
        int wX = GameManager.witchInstance.GetX();
        int wY = GameManager.witchInstance.GetY();

        if(x == wX && y == wY)
        {
            Despawn();
        }
        else
        {
            Spawn();
        }
    }

}
