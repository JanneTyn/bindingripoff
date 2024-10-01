using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomText : MonoBehaviour
{
    private GameObject Canvas;
    private void Start()
    {
        ObjectInstantiator("pause", "Canvas");
        ObjectInstantiator("gameplayui", "Canvas");
        GameObject.Find("Canvas").AddComponent<MenuController>();
        Destroy(gameObject);

    }

    private void ObjectInstantiator(string prefabName, string parentObjectName)
    {
        GameObject prefab = Resources.Load<GameObject>("UI/" + prefabName);
        if (prefab != null)
        {
            // Instantiate the prefab
            GameObject instance = Instantiate(prefab);

            // Find the parent object in the scene
            GameObject parentObject = GameObject.Find(parentObjectName);

            if (parentObject != null)
            {
                // Set the parent of the instantiated prefab
                instance.transform.SetParent(parentObject.transform, false);
            }
            else
            {
                Debug.LogError("Parent object not found in the scene.");
            }
        }
        else
        {
            Debug.LogError("Prefab not found in Resources folder.");
        }
    }
}

