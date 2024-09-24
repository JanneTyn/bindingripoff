using UnityEngine;
using UnityEngine.UI;

public class ObjectFollow : MonoBehaviour
{
    public Transform Follow;
    //private Slider Slider;
    private Camera MainCamera;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 0, 0);

    void Start()
    {
        MainCamera = Camera.main;
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    void Update()
    {
        if (Follow == null)
        {
            Destroy(this.gameObject);
            return;
        } 

        var screenPos = MainCamera.WorldToScreenPoint(Follow.position + hpBarOffset);
        
        transform.position = screenPos;

    }
}