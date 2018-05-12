using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class WitchDangerAlert : MonoBehaviour {
    public Strega strega;
    public Giocatore giocatore;
    public PostProcessingProfile pp;

    GrainModel.Settings grain;
    ChromaticAberrationModel.Settings aberr;
    VignetteModel.Settings vignette;

    private Color colorDest;
    private float vignetteDest;
    private float aberrDest;
    private float grainDest;

    public float grainDanger;
    public float vignetteDanger;
    public float aberrDanger;
    public Color colorDanger;

    public float grainStd = 0.001f;
    public float vignetteStd = 0.3f;
    public float aberrStd = 0.001f;
    public Color colorStd = Color.black;

    private void Start()
    {
        vignette = pp.vignette.settings;
        aberr = pp.chromaticAberration.settings;
        grain = pp.grain.settings;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Enable();
        }
        if (Input.GetKeyDown(KeyCode.AltGr))
        {
            Disable();
        }
    }

    public void Check () {
        int playerX = giocatore.getX();
        int playerY = giocatore.getY();

        int witchX = strega.GetX();
        int witchY = strega.GetY();

        MapTile nordTile = GameManager.movementManagerInstance.getNextTile(witchX, witchY, Direction.nord);
        MapTile sudTile = GameManager.movementManagerInstance.getNextTile(witchX, witchY, Direction.sud);
        MapTile estTile = GameManager.movementManagerInstance.getNextTile(witchX, witchY, Direction.est);
        MapTile ovestTile = GameManager.movementManagerInstance.getNextTile(witchX, witchY, Direction.ovest);

        bool nord = (nordTile.getX() == playerX && nordTile.getY() == playerY);
        bool sud = (sudTile.getX() == playerX && sudTile.getY() == playerY);
        bool est = (estTile.getX() == playerX && estTile.getY() == playerY);
        bool ovest = (ovestTile.getX() == playerX && ovestTile.getY() == playerY);

        if (nord || sud || est || ovest || (witchX == playerX && witchY == playerY))
        {
            print("on");
            Enable();
        }
        else
        {
            print("off");
            Disable();
        }
	}

    private IEnumerator Anim()
    {
        while (!Mathf.Approximately(grain.intensity,grainDest))
        {
            grain.intensity = Mathf.Lerp(grain.intensity, grainDest, Time.deltaTime);
            pp.grain.settings = grain;
            yield return new WaitForFixedUpdate();
        }
        while (!Mathf.Approximately(aberr.intensity,aberrDest))
        {
            aberr.intensity = Mathf.Lerp(aberr.intensity, aberrDest, Time.deltaTime);
            pp.chromaticAberration.settings = aberr;
            yield return new WaitForFixedUpdate();
        }
        while (!Mathf.Approximately(vignette.intensity,vignetteDest))
        {
            vignette.intensity = Mathf.Lerp(vignette.intensity, vignetteDest, Time.deltaTime);
            pp.vignette.settings = vignette;
            yield return new WaitForFixedUpdate();
        }
        while (!Mathf.Approximately(vignette.color.r,colorDest.r))
        {
            vignette.color = Color.Lerp(vignette.color, colorDest, Time.deltaTime);
            pp.vignette.settings = vignette;
            yield return new WaitForFixedUpdate();
        }
    }

    private void Disable()
    {
        colorDest = colorStd;
        vignetteDest = vignetteStd;
        aberrDest = aberrStd;
        grainDest = grainStd;

        StartCoroutine(Anim());
    }

    private void Enable()
    {
        colorDest = colorDanger;
        vignetteDest = vignetteDanger;
        aberrDest = aberrDanger;
        grainDest = grainDanger;

        StartCoroutine(Anim());
    }

    private void OnDisable()
    {
        vignette.intensity = vignetteStd;
        aberr.intensity = aberrStd;
        vignette.color = colorStd;
        grain.intensity = grainStd;

        pp.vignette.settings = vignette;
        pp.chromaticAberration.settings = aberr;
        pp.grain.settings = grain;
    }

    private void OnEnable()
    {
        vignette.intensity = vignetteStd;
        aberr.intensity = aberrStd;
        vignette.color = colorStd;
        grain.intensity = grainStd;

        pp.vignette.settings = vignette;
        pp.chromaticAberration.settings = aberr;
        pp.grain.settings = grain;
    }
}
