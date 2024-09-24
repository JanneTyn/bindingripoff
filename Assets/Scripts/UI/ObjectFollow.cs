using UnityEngine;

public class ObjectFollow : MonoBehaviour
{

    public Transform Follow;

    private Camera MainCamera;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    // Update is called once per frame
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