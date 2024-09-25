using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Transform Follow;
    private Slider Slider;
    private Camera MainCamera;
    private TestEnemy testEnemy;
    private Player player;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 0, 0);

    void Start()
    {
        MainCamera = Camera.main;
        Slider = GetComponent<Slider>();
        Slider.maxValue = 100f;

        testEnemy = GetComponentInParent<TestEnemy>();
        player = GetComponentInParent<Player>();

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
        
        if (testEnemy != null)
        {
            Slider.value = testEnemy.PublicCurrentHealth();
        }
        else
        {
            Slider.value = player.PublicCurrentHealth();
        }
        
    }
}