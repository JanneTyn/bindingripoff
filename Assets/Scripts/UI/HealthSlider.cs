using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Transform Follow;
    private Slider Slider;
    private Camera MainCamera;
    private Enemy testEnemy;
    private Player player;
    private string CharacterType;
    private Vector3 hpBarOffset = new Vector3(0, -1.2f, 0);
    private RectTransform rectTransform;

    void Start()
    {
        if (GetComponentInParent<Enemy>() == null)
        {
            player = GetComponentInParent<Player>();
            CharacterType = "Player";
        }
        else
        {
            testEnemy = GetComponentInParent<Enemy>();
            CharacterType = "TestEnemy";
        }

        MainCamera = Camera.main;
        Slider = GetComponent<Slider>();
        Slider.maxValue = 100f;


        if (Follow == null)
        {
            Follow = transform.parent;
        }

        rectTransform = GetComponent<RectTransform>();
        transform.SetParent(GameObject.Find("Canvas").transform);
        this.gameObject.transform.localScale = new Vector3(1.3f, 0.8f, 1.3f);
    }

    void Update()
    {
        if (Follow == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 screenPos = MainCamera.WorldToScreenPoint(Follow.position + hpBarOffset);
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            screenPos,
            MainCamera,
            out canvasPos);
        rectTransform.localPosition = canvasPos;

        switch (CharacterType)
        {
            case "Player":
                Slider.value = player.currentHealth;
                break;
            default:
                Slider.value = testEnemy.currentHealth;
                break;

        }
        
    }
}