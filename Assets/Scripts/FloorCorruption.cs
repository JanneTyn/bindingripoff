using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorCorruption : MonoBehaviour
{
    Color lerpedColor = Color.white;
    public float corruptionTimer = 120;
    public bool corruptionActive = false;
    public float corruptionTimeActive = 0;
    public static float corruptPercentage = 0;
    TilemapRenderer renderer;

    void Start()
    {
        StartCoroutine(CorruptionActivationTimer());
        renderer = GetComponent<TilemapRenderer>();
    }

    void Update()
    {
        if (corruptionActive)
        {
            if (lerpedColor.g > 0.05f)
            {
                corruptionTimeActive += Time.deltaTime;
                lerpedColor = Color.Lerp(Color.white, Color.red, Mathf.PingPong(corruptionTimeActive / 120, 1));
                corruptPercentage = 1 - lerpedColor.g;
                renderer.material.color = lerpedColor;
            }
        }
    }

    private IEnumerator CorruptionActivationTimer()
    {
        yield return new WaitForSeconds(corruptionTimer);
        corruptionActive = true;
    }


}
