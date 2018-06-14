using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWarpTiles : EventListener {

    int dim;
    public int x;
    public int y;

    bool tileLatoDestro;
    bool tileLatoSinistro;
    bool tileLatoSopra;
    bool tileLatoSotto;

    GameObject fantoccio;

    Vector3 originalPosition;

    bool warped = false;
    Vector2 warpedTo;
    public static bool animating = false;

    Dictionary<MoveWarpTiles, Vector3> opposti;

    Color original;
    MeshRenderer mr;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        original = mr.material.GetColor("_Color");

        warpedTo = new Vector2();

        dim = GameManager.mapInstance.dim - 1;
        originalPosition = transform.position;

        x = TileCoords.GetX(gameObject);
        y = TileCoords.GetY(gameObject);

        tileLatoDestro = (x == dim && y >= 0 && y <= dim);
        tileLatoSinistro = (x == 0 && y >= 0 && y <= dim);
        tileLatoSopra = (x >= 0 && x <= dim && y == dim);
        tileLatoSotto = (x >= 0 && x <= dim && y == 0);

        opposti = new Dictionary<MoveWarpTiles, Vector3>();

        Invoke("InitializeOpposites", 2f);
    }

    private void InitializeOpposites()
    {
        if(tileLatoSotto || tileLatoSopra || tileLatoSinistro || tileLatoDestro)
            opposti = FindOppositeTiles();
        return;
    }

    private Dictionary<MoveWarpTiles, Vector3> FindOppositeTiles()
    {
        Dictionary<MoveWarpTiles, Vector3> res = new Dictionary<MoveWarpTiles, Vector3>();

        if(tileLatoSopra && tileLatoSinistro) // Angolo in alto a sinistra
        {
            MoveWarpTiles sopra = GameManager.GetFrontEndTile(0, 0).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSopra = transform.position + GameManager.tileDistance * Vector3.forward;
            res.Add(sopra, posizioneSopra);

            MoveWarpTiles sinistra = GameManager.GetFrontEndTile(dim, dim).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSinistra = transform.position + GameManager.tileDistance * Vector3.left;
            res.Add(sinistra, posizioneSinistra);

            return res;
        }
        else if(tileLatoSinistro && tileLatoSotto) // Angolo in basso a sinistra
        {
            MoveWarpTiles sotto = GameManager.GetFrontEndTile(0, dim).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSotto = transform.position + GameManager.tileDistance * Vector3.back;
            res.Add(sotto, posizioneSotto);

            MoveWarpTiles sinistra = GameManager.GetFrontEndTile(dim, 0).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSinistra = transform.position + GameManager.tileDistance * Vector3.left;
            res.Add(sinistra, posizioneSinistra);

            return res;
        }
        else if (tileLatoSotto && tileLatoDestro) // Angolo in basso a destra
        {
            MoveWarpTiles sotto = GameManager.GetFrontEndTile(dim, dim).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSotto = transform.position + GameManager.tileDistance * Vector3.back;
            res.Add(sotto, posizioneSotto);

            MoveWarpTiles destra = GameManager.GetFrontEndTile(0, 0).GetComponent<MoveWarpTiles>();
            Vector3 posizioneDestra = transform.position + GameManager.tileDistance * Vector3.right;
            res.Add(destra, posizioneDestra);

            return res;
        }
        else if(tileLatoDestro && tileLatoSopra) // Angolo in alto a destra
        {
            MoveWarpTiles sopra = GameManager.GetFrontEndTile(dim, 0).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSopra = transform.position + GameManager.tileDistance * Vector3.forward;
            res.Add(sopra, posizioneSopra);

            MoveWarpTiles destra = GameManager.GetFrontEndTile(0, dim).GetComponent<MoveWarpTiles>();
            Vector3 posizioneDestra = transform.position + GameManager.tileDistance * Vector3.right;
            res.Add(destra, posizioneDestra);

            return res;
        }
        else if (tileLatoDestro)
        {
            MoveWarpTiles destra = GameManager.GetFrontEndTile(0, y).GetComponent<MoveWarpTiles>();
            Vector3 posizioneDestra = transform.position + GameManager.tileDistance * Vector3.right;
            res.Add(destra, posizioneDestra);

            return res;
        }
        else if (tileLatoSinistro)
        {
            MoveWarpTiles sinistra = GameManager.GetFrontEndTile(dim, y).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSinitra = transform.position + GameManager.tileDistance * Vector3.left;
            res.Add(sinistra, posizioneSinitra);

            return res;
        }
        else if (tileLatoSopra)
        {
            MoveWarpTiles sopra = GameManager.GetFrontEndTile(x, 0).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSopra = transform.position + GameManager.tileDistance * Vector3.forward;
            res.Add(sopra, posizioneSopra);

            return res;
        }
        else if (tileLatoSotto)
        {
            MoveWarpTiles sotto = GameManager.GetFrontEndTile(x, dim).GetComponent<MoveWarpTiles>();
            Vector3 posizioneSotto = transform.position + GameManager.tileDistance * Vector3.back;
            res.Add(sotto, posizioneSotto);

            return res;
        }

        throw new System.Exception("Non è stato possibile trovare il tile o i tile opposti! " + x + " " + y);
    }

    private void MakeGhost()
    {
        Color transparent = new Color(original.r, original.g, original.b, 0.1f);
        GetComponent<MeshRenderer>().material.SetColor("_Color",transparent);
    }

    private void MoveMeTo(Vector3 destination)
    {
        //transform.position = destination;
        //StopCoroutine("TileWarpAnimation");
        TileType tileType = GameManager.mapInstance.getTile(x, y).getTileType();
        Rotation tileRotation = GameManager.mapInstance.getTile(x, y).getTileRotation();

        GameObject tile = GameManager.instance.GetFrontEndTilePrefab(tileType);
        Quaternion rotation = GameManager.instance.GetFrontEndTileRotation(tile, tileRotation);

        //fantoccio = Instantiate(tile, transform.position, rotation);

        // tile fantasma
        MakeGhost();

        StartCoroutine(TileWarpMoveAnimation(destination));
        warped = true;
    }

    private void MoveMeBack()
    {
        //transform.position = originalPosition;
        //StopCoroutine("TileWarpAnimation");
        StartCoroutine(TileWarpMoveAnimation(originalPosition));
        warped = false;
    }

    private void MoveMyOppositesToMe()
    {
        foreach (KeyValuePair<MoveWarpTiles, Vector3> coppia in opposti)
        {
            coppia.Key.MoveMeTo(coppia.Value);
            coppia.Key.warpedTo.x = x;
            coppia.Key.warpedTo.y = y;
        }
    }

    public void PlayerMoved()
    {
        int playerX = GameManager.playerInstance.getX();
        int playerY = GameManager.playerInstance.getY();

        bool latoDestro = (playerX == dim && playerY >= 0 && playerY <= dim);
        bool latoSinistro = (playerX == 0 && playerY >= 0 && playerY <= dim);
        bool latoSopra = (playerX >= 0 && playerX <= dim && playerY == dim);
        bool latoSotto = (playerX >= 0 && playerX <= dim && playerY == 0);

        if (warped && (playerX != warpedTo.x || playerY != warpedTo.y))
        {
            //Destroy(fantoccio);
            MoveMeBack();
        }

        if((latoDestro || latoSinistro || latoSopra || latoSotto) && (playerX == x && playerY == y))
        {
            GameManager.instance.eventoWarpTile.Raise();
            //SoundManager.instance.Play("tilewarping");
            MoveMyOppositesToMe();
        }
    }

    private IEnumerator TileWarpMoveAnimation(Vector3 destination)
    {
        animating = true;

        Vector3 endPosition = transform.position + (Vector3.up * 2f);

        float counter = 0f;

        MapTile backEnd = GameManager.mapInstance.getTile(x, y);
        /*
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
        }*/

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            float y = Mathf.Lerp(transform.position.y, endPosition.y, counter);

            Vector3 newTransform = new Vector3(transform.position.x, y, transform.position.z);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= 0.01f)
            {
                transform.position = endPosition;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        destination.y = endPosition.y;
        endPosition = destination;

        counter = 0f;

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            float y = Mathf.Lerp(transform.position.y, endPosition.y, counter);

            Vector3 newTransform = Vector3.Lerp(transform.position, endPosition, counter);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= 0.01f)
            {
                transform.position = endPosition;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        endPosition = transform.position + (Vector3.down * 2f);

        counter = 0f;

        while (transform.position != endPosition)
        {
            counter += Time.deltaTime;

            float y = Mathf.Lerp(transform.position.y, endPosition.y, counter);

            Vector3 newTransform = Vector3.Lerp(transform.position, endPosition, counter);

            if (Mathf.Abs(Vector3.Distance(newTransform, endPosition)) <= 0.01f)
            {
                transform.position = endPosition;
            }
            else
            {
                transform.position = newTransform;
            }

            yield return new WaitForFixedUpdate();
        }

        animating = false;
    }

    private IEnumerator TileWarpAnimation(Vector3 destination)
    {
        Vector3 originalScale = transform.localScale;

        animating = true;
        while(transform.localScale.x > 0.1f)
        {
            transform.localScale = transform.localScale * 0.9f;
            yield return null;
        }

        transform.position = destination;

        while(transform.localScale.x < originalScale.x)
        {
            transform.localScale = transform.localScale / 0.9f;
            yield return null;
        }
        animating = false;
    }
}
