using UnityEngine;
using System.Collections;

/* Classe istanziata dalla Map rappresentante un tile che compone la mappa.
 * Mantiene rotazione e tipo, mette a disposizione funzioni per controllare il tipo, la rotazione
 * e se è presente una via di uscita in una determinata direzione.
*/

public class MapTile
{
    private int x;
    private int y;

    private bool uscita = false;
    private bool key = false;
    private bool caramella = false;
    private bool scarpetta = false;

    private Rotation rotation;
    private TileType type;

    private bool fog = true;

    private GameObject frontEndPrefab;
    public GameObject scarpettaFrontEnd;
    public GameObject chiaveFrontEnd;
    public GameObject caramellaFrontEnd;
    public GameObject petardoFrontEnd;

    public bool petardo = false;
    public bool petardoAttivo = false;

    public void SetPetardoFrontEnd(GameObject obj)
    {
        petardoFrontEnd = obj;
    }

    public GameObject GetPetardoFrontEnd()
    {
        return petardoFrontEnd;
    }

    public bool HasPetardo()
    {
        return petardo;
    }

    public void SetPetardo(bool hasPetardo)
    {
        petardo = hasPetardo;
    }

    public bool IsPetardoAttivo()
    {
        return petardoAttivo;
    }

    public void AttivaPetardo()
    {
        Debug.Log("MapTile "+x+" "+y+" petardo attivo");

        GameObject.Instantiate(GameManager.instance.particlePetardoPiazzato, frontEndPrefab.transform);

        petardoAttivo = true;
    }

    public void ScoppiaPetardo()
    {
        if (!petardoAttivo)
            return;

        GameObject.Instantiate(GameManager.instance.particlePetardoEsploso, frontEndPrefab.transform);

        petardoAttivo = false;
        //SoundManager.instance.Play("bangpetardo");
        GameManager.instance.eventoPetardoScoppiato.Raise();
    }

    public void SetScarpettaFrontEnd(GameObject obj)
    {
        scarpettaFrontEnd = obj;
    }

    public GameObject GetScarpettaFrontEnd()
    {
        return scarpettaFrontEnd;
    }

    public void SetCaramellaFrontEnd(GameObject obj)
    {
        caramellaFrontEnd = obj;
    }

    public GameObject GetCaramellaFrontEnd()
    {
        return caramellaFrontEnd;
    }

    public void SetChiaveFrontEnd(GameObject obj)
    {
        chiaveFrontEnd = obj;
    }

    public GameObject GetChiaveFrontEnd()
    {
        return chiaveFrontEnd; ;
    }

    public void SetCaramella(bool hasCaramella)
    {
        caramella = hasCaramella;
    }

    public bool HasCaramella()
    {
        return caramella;
    }

    public void SetScarpetta(bool hasScarpetta)
    {
        scarpetta = hasScarpetta;
    }

    public void PrendiScarpetta()
    {
        scarpetta = false;
        //Debug.Log("inserisco " + this.x + " " + this.y);
        GameManager.mapInstance.scarpetteDaRespawnare.Add(this);
        //Debug.Log("ho inserito " + this.x + " " + this.y + " la lista contiene " + GameManager.mapInstance.scarpetteDaRespawnare.Count + " tiles");
    }

    public bool HasScarpetta()
    {
        return scarpetta;
    }

    public void SetKey(bool hasKey)
    {
        key = hasKey;
    }

    public bool HasKey()
    {
        return key;
    }

    public void SetUscita(bool isUscita)
    {
        uscita = isUscita;
    }

    public bool IsUscita()
    {
        return uscita;
    }

    public void setPrefab(GameObject prefab)
    {
        frontEndPrefab = prefab;
    }

    public GameObject getPrefab()
    {
        return frontEndPrefab;
    }

    public void setFog(bool active)
    {
        fog = active;
        frontEndPrefab.GetComponent<TileFog>().SetFog(fog);
    }
    
    public bool Fog()
    {
        return fog;
    }

    public MapTile(int x, int y, TileType type, Rotation rotation)
    {
        this.x = x;
        this.y = y;

        this.type = type;
        this.rotation = rotation;
    }

    // Restituisce true se c'è una via di uscita nella direzione specificata in dir,
    // tiene conto del tipo di tile e della rotazione attuale del tile.

    public bool getDirection(Direction dir)
    {
        bool nord = false;
        bool sud = false;
        bool est = false;
        bool ovest = false;

        if (type == TileType.angolo)
        {
            if (rotation == Rotation.su) // E' come una L
            {
                nord = true;
                sud = false;
                est = true;
                ovest = false;
            }
            else if (rotation == Rotation.giu) // è una L sottosopra (girando in senso orario)
            {
                sud = true;
                nord = false;
                ovest = true;
                est = false;
            }
            else if (rotation == Rotation.destra) // è una L ruotata di 90° in senso orario
            {
                est = true;
                ovest = false;
                sud = true;
                nord = false;
            }
            else if (rotation == Rotation.sinistra) // è una L ruotata di 180° in senso orario
            {
                ovest = true;
                nord = true;
                sud = false;
                est = false;
            }
        }
        else if (type == TileType.corridoio)
        {
            if (rotation == Rotation.su) // E' come una I
            {
                nord = true;
                sud = true;
                est = false;
                ovest = false;
            }
            else if (rotation == Rotation.giu)
            {
                nord = true;
                sud = true;
                est = false;
                ovest = false;
            }
            else if (rotation == Rotation.destra)
            {
                nord = false;
                sud = false;
                est = true;
                ovest = true;
            }
            else if (rotation == Rotation.sinistra)
            {
                nord = false;
                sud = false;
                est = true;
                ovest = true;
            }
        }
        else if (type == TileType.trivio)
        {
            if (rotation == Rotation.su) // è come una T rovesciata
            {
                est = true;
                ovest = true;
                sud = false;
                nord = true;
            }
            else if (rotation == Rotation.giu) // è come una T
            {
                est = true;
                ovest = true;
                sud = true;
                nord = false;
            }
            else if (rotation == Rotation.destra) // il gambo della T punta a dx
            {
                est = true;
                ovest = false;
                nord = true;
                sud = true;
            }
            else if (rotation == Rotation.sinistra) // il gambo della T punta a sx
            {
                nord = true;
                sud = true;
                ovest = true;
                est = false;
            }
        }
        else if (type == TileType.quadrivio)
        {
            nord = true;
            sud = true;
            est = true;
            ovest = true;
        }

        if (dir == Direction.est)
            return est;
        else if (dir == Direction.ovest)
            return ovest;
        else if (dir == Direction.nord)
            return nord;
        else if (dir == Direction.sud)
            return sud;
        else
        {
            throw new System.Exception("Errore! Direzione specificata non esistente");
        }
    }
    
    // Funzioni getter e settter

    public TileType getTileType()
    {
        return type;
    }

    public Rotation getTileRotation()
    {
        return rotation;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public int[] getXY()
    {
        int[] xy = new int[2];
        xy[0] = x;
        xy[1] = y;
        return xy;
    }

    // Ruota il tile, parametro clockwise su true ruota in senso orario, su false in antiorario

    public void rotate(bool clockwise)
    {
        if (type == TileType.quadrivio) return;

        //GameManager.playerMovementEvent.Raise();

        if (clockwise)
        {
            if (rotation == Rotation.su)
                rotation = Rotation.destra;
            else if (rotation == Rotation.destra)
                rotation = Rotation.giu;
            else if (rotation == Rotation.giu)
                rotation = Rotation.sinistra;
            else if (rotation == Rotation.sinistra)
                rotation = Rotation.su;
        }
        else
        {
            if (rotation == Rotation.su)
                rotation = Rotation.sinistra;
            else if (rotation == Rotation.sinistra)
                rotation = Rotation.giu;
            else if (rotation == Rotation.giu)
                rotation = Rotation.destra;
            else if (rotation == Rotation.destra)
                rotation = Rotation.su;
        }

        //Debug.Log(rotation);
    }
}
