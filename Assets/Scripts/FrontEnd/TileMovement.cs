using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class TileMovement : MonoBehaviour
{
    private int tileX = 0;
    private int tileY = 0;

    public float height = 1f;
    public float speed = 1f;
    public float returnSpeed = 1f;
    public float rotationSpeed = 10f;

    public static bool canRot = true;

    public void StartTileRotation(int angleRot, int dir)
    {
        StartCoroutine(TileRotation(angleRot, dir));
    }

    /* FUNZIONI WRAPPER PER RETROCOMPATIBILITA' CON ALTRI SCRIPT */
    /* TILE X E TILE Y SONO STATI SPOSTATI DENTRO LO SCRIPT TileCoords */
    public int GetTileX()
    {
        return TileCoords.GetX(gameObject);
    }
    public int GetTileY()
    {
        return TileCoords.GetY(gameObject);
    }
    public void SetTileX(int x)
    {
        TileCoords.SetX(gameObject, x);
    }
    public void SetTileY(int y)
    {
        TileCoords.SetY(gameObject, y);
    }

    //Animazione Tile
    IEnumerator TileRotation(int angleRot, int dir) 
    {
        canRot = false;

        //gameObject.GetComponent<FloatAnimation>().enabled = false;

        GameManager.instance.eventoRisalitaTileIniziata.Raise();
        //SoundManager.instance.Play("tilerotation");

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.up * height;

        GameObject playerPrefab = GameManager.playerPrefabInstance;
        Vector3 playerPosition = playerPrefab.transform.position;
        float playerTileGapY = playerPosition.y - transform.position.y;

        bool playerIsOnTile = (GameManager.playerInstance.getX() == TileCoords.GetX(gameObject) && GameManager.playerInstance.getY() == TileCoords.GetY(gameObject));

        MapTile backEnd = GameManager.mapInstance.getTile(GetTileX(), GetTileY());

        if (backEnd.HasCaramella())
        {
            backEnd.GetCaramellaFrontEnd().GetComponent<PickupAnimation>().Despawn();
        }

        if (backEnd.HasPetardo())
        {
            backEnd.GetPetardoFrontEnd().GetComponent<PickupAnimation>().Despawn();
        }

        if (backEnd.HasScarpetta())
        {
            backEnd.GetScarpettaFrontEnd().GetComponent<PickupAnimation>().Despawn();
        }

        if (backEnd.HasKey())
        {
            backEnd.GetChiaveFrontEnd().GetComponent<KeyAnimation>().DespawnNoDestroy();
        }

        float counter = 0f;

        // la tile sale

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, endPosition, Mathf.Abs(Mathf.Sin(counter)));

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.5f, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, endPosition) <= 0.01f)
            {
                transform.position = endPosition;
            }

            if (playerIsOnTile)
                playerPrefab.transform.position = new Vector3(playerPosition.x, transform.position.y + playerTileGapY, playerPosition.z);

            yield return new WaitForFixedUpdate();
        }

        counter = 0f;

        yield return new WaitForSeconds(0.1f);

        GameManager.instance.eventoRisalitaTileFinita.Raise();
        //Debug.LogError("eventorisalitatilefinita");
        //SoundManager.instance.Play("whoosh");

        for (float i = 0; i < rotationSpeed; i++)
        {
            transform.Rotate(0f, angleRot / rotationSpeed, 0f);
            if(playerIsOnTile)
                playerPrefab.transform.Rotate(0f, angleRot / rotationSpeed, 0f);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.1f);

        counter = 0f;

        GameManager.instance.eventoDiscesaTileIniziata.Raise();
        //SoundManager.instance.Play("tilerotation");

        // Torna in posizione
        while (transform.position != startPosition)
        {
            counter += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, startPosition, Mathf.Abs(Mathf.Sin(counter)));

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Mathf.Abs(Mathf.Sin(counter)));

            if (Vector3.Distance(transform.position, startPosition) <= 0.01f)
            {
                transform.position = startPosition;
            }

            if (playerIsOnTile)
                playerPrefab.transform.position = new Vector3(playerPosition.x, transform.position.y + playerTileGapY, playerPosition.z);

            yield return new WaitForFixedUpdate();
        }

        if (backEnd.HasCaramella())
        {
            backEnd.GetCaramellaFrontEnd().GetComponent<PickupAnimation>().Spawn();
        }

        if (backEnd.HasScarpetta())
        {
            backEnd.GetScarpettaFrontEnd().GetComponent<PickupAnimation>().Spawn();
        }

        if (backEnd.HasKey())
        {
            backEnd.GetChiaveFrontEnd().GetComponent<KeyAnimation>().Spawn();
        }

        if (backEnd.HasPetardo())
        {
            backEnd.GetPetardoFrontEnd().GetComponent<PickupAnimation>().Despawn();
        }

        //gameObject.GetComponent<FloatAnimation>().enabled = true;

        canRot = true;

        GameManager.instance.eventoDiscesaTileFinita.Raise();
        GameManager.instance.eventoFineAnimazioneGiocatore.Raise();
    }
}