using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCanvas : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
