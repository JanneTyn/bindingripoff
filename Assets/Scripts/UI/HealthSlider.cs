using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Transform Follow;
    private Slider Slider;
    private Camera MainCamera;
    private TestEnemy testEnemy;
    private Player player;
    private string CharacterType;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, -0.8f, 0);

    void Start()
    {
        if (GetComponentInParent<TestEnemy>() == null)
        {
            player = GetComponentInParent<Player>();
            CharacterType = "Player";
        }
        else
        {
            testEnemy = GetComponentInParent<TestEnemy>();
            CharacterType = "TestEnemy";
        }

        MainCamera = Camera.main;
        Slider = GetComponent<Slider>();
        Slider.maxValue = 100f;


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
        
        switch (CharacterType)
        {
            case "Player":
                Slider.value = player.PublicCurrentHealth();
                break;
            default:
                Slider.value = testEnemy.PublicCurrentHealth();
                break;

        }
        
    }
}