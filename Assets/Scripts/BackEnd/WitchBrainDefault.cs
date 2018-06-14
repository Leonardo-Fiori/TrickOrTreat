using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Witch Brains/Default Witch Brain")]
public class WitchBrainDefault : WitchBrain {
    public override void Think(Strega subject)
    {
        // Mossa random
        if (Random.Range(0, 100) < (subject.chanceMossaRandomAttuale - (subject.mosseFatte * 10f)) && subject.mosseFatte != subject.mossePerTurno - 1 && !subject.PlayerOnNextTile())
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

            subject.chanceMossaRandomAttuale /= 2f;
        }
        // Mossa intelligente
        else
        {
            //Debug.Log("mossa intelligente" + chanceMossaRandomAttuale);

            subject.chanceMossaRandomAttuale = subject.chanceMossaRandom;

            MapTile nord = GameManager.movementManagerInstance.getNextTile(subject.x, subject.y, Direction.nord);
            MapTile sud = GameManager.movementManagerInstance.getNextTile(subject.x, subject.y, Direction.sud);
            MapTile ovest = GameManager.movementManagerInstance.getNextTile(subject.x, subject.y, Direction.ovest);
            MapTile est = GameManager.movementManagerInstance.getNextTile(subject.x, subject.y, Direction.est);

            int distanzaX = 1000; // what are you doing domi
            int distanzaY = 1000;
            int bestTileX = 0;
            int bestTileY = 0;

            int playerX = GameManager.playerInstance.getX();
            int playerY = GameManager.playerInstance.getY();

            if (((Mathf.Abs(nord.getX() - playerX)) <= distanzaX) && (Mathf.Abs(nord.getY() - playerY)) <= distanzaY)
            {
                bestTileX = nord.getX();
                bestTileY = nord.getY();
                distanzaX = Mathf.Abs(nord.getX() - playerX);
                distanzaY = Mathf.Abs(nord.getY() - playerY);
            }

            if (((Mathf.Abs(sud.getX() - playerX)) <= distanzaX) && (Mathf.Abs(sud.getY() - playerY) <= distanzaY))
            {
                bestTileX = sud.getX();
                bestTileY = sud.getY();
                distanzaX = Mathf.Abs(sud.getX() - playerX);
                distanzaY = Mathf.Abs(sud.getY() - playerY);
            }

            if (((Mathf.Abs(ovest.getX() - playerX)) <= distanzaX) && (Mathf.Abs(ovest.getY() - playerY) <= distanzaY))
            {
                bestTileX = ovest.getX();
                bestTileY = ovest.getY();
                distanzaX = Mathf.Abs(ovest.getX() - playerX);
                distanzaY = Mathf.Abs(ovest.getY() - playerY);
            }


            if (((Mathf.Abs(est.getX() - playerX)) <= distanzaX) && (Mathf.Abs(est.getY() - playerY)) <= distanzaY)
            {
                bestTileX = est.getX();
                bestTileY = est.getY();
                distanzaX = Mathf.Abs(est.getX() - playerX);
                distanzaY = Mathf.Abs(est.getY() - playerY);
            }

            subject.x = bestTileX;
            subject.y = bestTileY;
        }
    }
}
