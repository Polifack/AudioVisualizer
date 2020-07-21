using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarVisualizerGenerator : MonoBehaviour
{
    public int audioRange;  //Range of audio to map
    public float timeStep;  //Minimum time interval between beats
    public float beatTime;  //Time of beat effect
    public float restTime;  //Time of rest effect

    public GameObject bar;    //Bar

    public float beatHeight;  //Desired object height when the music is beating
    public float restHeight;  //Desired object height when the music is not beating

    public Color restColor; //Desired rest color
    public Color initialBeatColor; //Desired beat color of the first bar
    public Color finalBeatColor;  //Desired beat color of the last bar

    public int nBars; //Desired number of bars

    private float barWidth = 0f;
    private float audioRangeDelta = 0f;
    private void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        float h = canvas.GetComponent<RectTransform>().rect.height;
        float w = canvas.GetComponent<RectTransform>().rect.width;

        barWidth = w/nBars;
        audioRangeDelta = audioRange / nBars;

        for (int i = 0; i < nBars; i++)
        {
            createBar(i);
        }
    }

    private void createBar(int index)
    {
        GameObject currentBar = Instantiate(bar);
        currentBar.transform.SetParent(transform);
        RectTransform rt = currentBar.GetComponent<RectTransform>();

        //Set bar width
        Vector2 barSize = rt.sizeDelta;
        barSize.x = barWidth;
        rt.sizeDelta = barSize;

        //Set bar position
        Vector3 barPosition = currentBar.transform.position;
        barPosition.x = index * barWidth;
        barPosition.y = 0;
        currentBar.transform.position = barPosition;

        //Manage bar AudioSync 
        AudioSyncBar asb = currentBar.GetComponent<AudioSyncBar>();
        asb.bias = (index + 1) * audioRangeDelta;
        asb.timeStep = timeStep;
        asb.beatTime = beatTime;
        asb.restTime = restTime;
        asb.beatHeight = beatHeight;
        asb.restHeight = restHeight;

        asb.Setup();


        //Manage color AudioSync
        Color[] c = new Color[1];
        c[0] = computeIntermediateColor(initialBeatColor, finalBeatColor, index);
        
        AudioSyncColor asc = currentBar.GetComponent<AudioSyncColor>();
        asc.bias = (index+1) * audioRangeDelta;
        asc.timeStep = timeStep;
        asc.beatTime = beatTime;
        asc.restTime = restTime;
        asc.restColor = restColor;
        asc.beatColors = c;

        asc.Setup();

    }

    private Color computeIntermediateColor(Color a, Color b, float decision)
    {
        float ra = a.r;
        float ga = a.g;
        float ba = a.b;

        float rb = b.r;
        float gb = b.g;
        float bb = b.b;

        float diffr = (ra - rb)/nBars;
        float diffg = (ga - gb)/nBars;
        float diffb = (ba - bb)/nBars;

        return new Color(
            ra + diffr * decision,
            ga + diffg * decision,
            bb + diffb * decision);
    }


}
