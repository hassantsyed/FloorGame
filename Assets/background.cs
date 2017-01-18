using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

    Camera c;
    public System.Random r;
    bool changing = false;
    Color[] colors;
    //public bool start = false;
    GameObject p;
    
    public bool activate = false;

    //float t = 1;
    // Use this for initialization
    void Start () {
        c = gameObject.GetComponent<Camera>();
        r = new System.Random();
        colors = new Color[2];
        colors[0] = Color.gray;
        colors[1] = getColors();
        c.backgroundColor = colors[0];
        p = GameObject.Find("Player");
        p.SetActive(false);
        
    }

    // Update is called once per frame
    void Update() {
        while (!changing)
        {
            colors[0] = colors[1];
            colors[1] = getColors();
            StartCoroutine(changeColor());
        }
        //c.backgroundColor = Color.Lerp(colors[0], colors[1], Mathf.PingPong(Time.time*(float)t,(float)1.0));
        
		
	}

 
    IEnumerator changeColor()
    {
        float t = 1;
        changing = true;
        while (t > 0)
        {
            t -= Time.deltaTime;
            c.backgroundColor = Color.Lerp(colors[0], colors[1], 1 - t);
            yield return new WaitForSeconds(.02f);
            yield return null;
        }
        changing = false;


    }
    public void setStart(bool x)
    {
        GameObject.Find("Button").SetActive(false);
        p.SetActive(true);
        activate = true;
    }

    

    public Color getColors()
    {
        return new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
    }


}
