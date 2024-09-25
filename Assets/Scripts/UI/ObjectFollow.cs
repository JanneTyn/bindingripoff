using UnityEngine;
using UnityEngine.UI;

public class ObjectFollow : MonoBehaviour
{
    public Transform Follow;
    private Slider Slider;
    private Camera MainCamera;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 0, 0);

    void Start()
    {
        MainCamera = Camera.main;
        Slider = GetComponent<Slider>();
        
        if (Follow == null)
        {
            Follow = transform.parent;
        }
        
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        this.gameObject.transform.localScale = new Vector3(1, 0.6f, 1);
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