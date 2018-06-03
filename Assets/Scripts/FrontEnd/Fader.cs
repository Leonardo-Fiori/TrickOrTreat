using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

    public GameObject panel;
    private Image img;
    private Color destination;
    public float speed = 1f;

    public IEnumerator Animation()
    {
        while(img.color != destination)
        {
            destination = Color.Lerp(img.color, destination, Time.deltaTime * speed);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Dissolve()
    {
        destination = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Animation());
    }

    public void ToBlack()
    {
        destination = new Color(0f, 0f, 0f, 1f);
        StartCoroutine(Animation());
    }

	// Use this for initialization
	void Start () {
        img = panel.GetComponent<Image>();
        Dissolve();
	}
}
