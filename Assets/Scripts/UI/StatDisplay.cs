using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Animations;

public class StatDisplay : MonoBehaviour
{

    #region Singleton
    public static StatDisplay instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Instance of StatDisplay already exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion


    private PlayerLeveling playerLeveling;

    private PlayerStats playerStats;

    private Weapon currentWeapon;
    private Player player;
    private Image WeaponImage;

    [SerializeField] GameObject playerPrefab;

    [SerializeField]private GameObject CurrentLevelText;

    private int roomsCleared = 0;
    [SerializeField]private GameObject roomText;

    private int enemiesKilled = 0;
    [SerializeField]private GameObject killCounter;
    [SerializeField]private GameObject currentWeaponPrefab;
    void Start()
    {
        playerLeveling = playerPrefab.GetComponent<PlayerLeveling>();

        playerStats = playerPrefab.GetComponent<PlayerStats>();

        player = playerPrefab.GetComponent<Player>();
        WeaponImage = currentWeaponPrefab.GetComponent<Image>();

    }

    void Update()
    {
        CurrentLevelText.GetComponent<TMP_Text>().text = "Player level: " + playerLeveling.currentPlayerLevel;

        SetCurrentWeapon();



    }

    public void RoomCleared()
    {
        roomsCleared++;
        roomText.GetComponent<TMP_Text>().text = "Rooms cleared: " + roomsCleared;
    }

    public void KillCount()
    {
        enemiesKilled++;
        killCounter.GetComponent<TMP_Text>().text = "Kill counter: " + enemiesKilled;
    }

    private void SetCurrentWeapon()
    {
        currentWeapon = player.CurrentWeaponSprite();
        WeaponImage.sprite = currentWeapon.sprite;

        switch (currentWeapon.rarity)
        {
            case Weapon.Rarity.Uncommon:
                WeaponImage.color = Color.green;
                break;
            case Weapon.Rarity.Rare:
                WeaponImage.color = Color.blue;
                break;
            default:
                WeaponImage.color = Color.gray;
                break;
        }
    }

    public void DisplayCurrentStats()
    {
        StatDisplayFinder("Max Health", "maxHealthIncrease");
        StatDisplayFinder("Damage", "damageIncrease");
        StatDisplayFinder("Fire Rate", "fireRate");
        StatDisplayFinder("Defence", "armor");
        StatDisplayFinder("Evasion", "dodge");
        StatDisplayFinder("Movement Speed", "moveSpeedIncrease");
    }

    private void StatDisplayFinder(string stringStatName, string floatStatName)
    {
        GameObject StatObject = GameObject.Find(stringStatName + " stat");
        
        if (StatObject != gameObject.activeSelf) {
            Debug.Log(stringStatName + " not found");
            return; 
        }
        
        //Debug.Log(stringStatName + " found");

        TMP_Text textComponent = StatObject.GetComponent<TMP_Text>();
        if (textComponent == null) { return; }

        if (player == null) { player = playerPrefab.GetComponent<Player>(); }
        var field = player.GetType().GetField(floatStatName);
        
        if (field != null)
        {
            float value = (float)field.GetValue(player);
            textComponent.text = stringStatName + ": " + value;
        }
        else
        {
            textComponent.text = stringStatName + ": Field not found";
        }
    }

    public void EnableGameobject(string objectName)
    {
        GameObject parentObject = this.gameObject; //for Canvas
        if (parentObject != null)
        {
            Transform[] allChildren = parentObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.gameObject.name == objectName)
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Parent object not found!");
        }
    }
}
