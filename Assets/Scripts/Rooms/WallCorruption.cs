using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallCorruption : MonoBehaviour
{
    TilemapRenderer renderer;
    CorruptTimer corruptTimer;

    void Start()
    {
        renderer = GetComponent<TilemapRenderer>();
        corruptTimer = GameObject.Find("CorruptTimer").GetComponent<CorruptTimer>();
    }

    void Update()
    {
        if (corruptTimer.corruptionActive)
        {
            renderer.material.color = corruptTimer.lerpedColorWalls;
        }
    }
}
