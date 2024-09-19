using UnityEngine;

public class ObjectFollow : MonoBehaviour
{

    public Transform Follow;

    private Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Follow == null)
        {
            Destroy(this.gameObject);
            return;
        }

        var screenPos = MainCamera.WorldToScreenPoint(Follow.position);

        transform.position = screenPos;

    }
}