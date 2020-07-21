using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that syncs audio with color

[RequireComponent(typeof(Image))]
public class AudioSyncColor : AudioSyncer
{
    public Color[] beatColors;  //Desired colors to display when the music is beating
    public Color restColor;  //Desired color to display when the music is not beating

    Image img;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        img = GetComponent<Image>();
    }

    private IEnumerator MoveToColor(Color target)
    {
        Color currentColor = img.color;
        Color initialColor = currentColor;

        float timer = 0f;

        while (currentColor != target)
        {
            //Lerp into the beat scale
            currentColor = Color.Lerp(initialColor, target, timer / beatTime);
            timer += Time.deltaTime;

            img.color = currentColor;
            yield return null;
        }

        isBeating = false;
    }

    private Color getRandomColor()
    {
        if (beatColors == null || beatColors.Length == 0) return Color.white;
        int index = Random.Range(0, beatColors.Length);
        return beatColors[index];
    }


    public override void onUpdate()
    {
        base.onUpdate();

        //If we are not beating we lerp into the rest scale
        if (!isBeating)
            img.color = Color.Lerp(img.color, restColor, restTime * Time.deltaTime);
    }

    public override void onBeat()
    {
        base.onBeat();

        //Stop a scaling corroutine if it is already running and start a new one
        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", getRandomColor());

    }
}
