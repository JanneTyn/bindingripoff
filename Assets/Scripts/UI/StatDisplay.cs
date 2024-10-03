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
    private TMP_Text killCounter;
    private Weapon currentWeapon;
    private Player player;
    private Image WeaponImage;

    public TMP_Text CurrentLevelText;
    private PlayerLeveling Stat;
    private Player weapon;
    private int roomsCleared = 0;
    [SerializeField] private GameObject roomText;
    private int enemiesKilled;

    void Start()
    {
        Stat = GameObject.FindWithTag("Player").GetComponent<PlayerLeveling>();
        weapon = GameObject.FindWithTag("Player").GetComponent<Player>();
        WeaponImage = GameObject.Find("currentWeapon").GetComponent<Image>();
    }

    void Update()
    {
        CurrentLevelText.text = "Player level: " + playerLeveling.currentPlayerLevel;
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
        StatDisplayFinder("Max Health", "maxHealth");
        StatDisplayFinder("Damage", "damage");
        StatDisplayFinder("Fire Rate", "reloadSpeed");
        StatDisplayFinder("Defence", "defense");
        StatDisplayFinder("Evasion", "evasion");
        StatDisplayFinder("Movement Speed", "movementSpeed");
    }

    private void StatDisplayFinder(string stringStatName, string floatStatName)
    {
        /**float value = (float)playerStats.GetType().GetField(floatStatName).GetValue(playerStats);

        GameObject.Find(stringStatName + " stat").GetComponent<TMP_Text>().text = stringStatName + ": " + value;**/
        
        TMP_Text textComponent = GameObject.Find(stringStatName + " stat").GetComponent<TMP_Text>();

        var field = playerStats.GetType().GetField(floatStatName);
        if (field != null)
        {
            float value = (float)field.GetValue(playerStats);
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
