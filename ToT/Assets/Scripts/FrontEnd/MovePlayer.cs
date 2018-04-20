using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script che fa muovere il prefab del giocatore in una data direzione quando viene chiamata la move()
 * NON FA CONTROLLI SE LO SPOSTAMENTO E' LECITO O MENO. I controlli vengono fatti nel back-end.
 * Questo è uno script del front-end.
*/

public class MovePlayer : MonoBehaviour {

    // Di quante coordinate si deve spostare?
    public float factor = 1f;
    public float offsetY = 1f;
    public float raiseSpeed = 1f;
    public float raiseHeight = 3f;

    // Sposta il prefab del giocatore di tot factor in direzione dir

    IEnumerator moveTowards(Vector3 finalPos)
    {
        float counter = 0f;

        Vector3 originalPos = transform.position;

        GameManager.moving = true;
        // Solleva il giocatore se necessario
        if (Mathf.Abs(originalPos.x - finalPos.x) > 2 * factor || Mathf.Abs(originalPos.z - finalPos.z) > 2 * factor)
        {
            while (transform.position.y < offsetY + raiseHeight)
            {
                transform.position += Vector3.up * Time.deltaTime * raiseSpeed;
                yield return null;
            }

            finalPos.y = transform.position.y;
        }
        // Spostalo a destinazione
        while (transform.position != finalPos)
        {
            counter += Time.deltaTime;

            float x = Mathf.Lerp(transform.position.x, finalPos.x, Mathf.Abs(Mathf.Sin(counter)));//Mathf.MoveTowards(transform.position.x, finalPos.x, speed);
            float z = Mathf.Lerp(transform.position.z, finalPos.z, Mathf.Abs(Mathf.Sin(counter)));//Mathf.MoveTowards(transform.position.z, finalPos.z, speed);

            Vector3 newTransform = new Vector3(x, transform.position.y, z);
            transform.position = newTransform;

            yield return null;
        }
        // Abbassalo
        if (Mathf.Abs(originalPos.x - finalPos.x) > 2 * factor || Mathf.Abs(originalPos.z - finalPos.z) > 2 * factor)
        {
            while (transform.position.y > offsetY)
            {
                transform.position -= Vector3.up * Time.deltaTime * raiseSpeed;
                yield return null;
            }
        }
        GameManager.moving = false;
    }

    public void move(int x, int z, bool teleport)
    {
        if (teleport)
        {
            transform.position = new Vector3(x * factor, offsetY, z * factor);
            return;
        }

        StartCoroutine(moveTowards(new Vector3(x * factor, transform.position.y, z * factor)));
    }
}
