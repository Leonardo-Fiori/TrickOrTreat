using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Witch Brains/Stupid Witch Brain")]
public class WitchBrainStupid : WitchBrain {
    public override void Think(Strega subject)
    {
        float randomDirChanche = Random.Range(0, 100);
        Direction randomDir = 0;

        if (randomDirChanche >= 0f && randomDirChanche <= 25f)
        {
            randomDir = Direction.nord;
        }
        else if (randomDirChanche > 25f && randomDirChanche <= 50f)
        {
            randomDir = Direction.sud;
        }
        else if (randomDirChanche > 50f && randomDirChanche <= 75f)
        {
           randomDir = Direction.est;
        }
        else if (randomDirChanche > 75f && randomDirChanche <= 100f)
        {
         randomDir = Direction.ovest;
        }

        MapTile nextTile = GameManager.movementManagerInstance.getNextTile(subject.x, subject.y, randomDir);
        subject.x = nextTile.getX();
        subject.y = nextTile.getY();
    }
}
