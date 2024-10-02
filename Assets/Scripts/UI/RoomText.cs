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

    private void ObjectInstantiator(string prefabName, string parentObjectName) //for unity assets/resources/ui prefabs
    {
        GameObject prefab = Resources.Load<GameObject>("UI/" + prefabName); 
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab);

            GameObject parentObject = GameObject.Find(parentObjectName);

            if (parentObject != null)
            {
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

