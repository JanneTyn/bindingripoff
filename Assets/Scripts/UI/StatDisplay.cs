using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private PlayerLeveling Stat;

    Weapon currentWeapon;
    private Player Weapon;
    private Image WeaponImage;

    private GameObject CurrentLevelText;
    
    private int roomsCleared = 0;
    [SerializeField] private GameObject roomText;
    void Start()
    {
        CurrentLevelText = GameObject.Find("currentLevel");
        Stat = GameObject.Find("TestPlayer").GetComponent<PlayerLeveling>();
        Weapon = GameObject.Find("TestPlayer").GetComponent<Player>();
        WeaponImage = GameObject.Find("currentWeapon").GetComponent<Image>();
    }

    void Update()
    {
        CurrentLevelText.GetComponent<TMP_Text>().text = "Player level: " + Stat.currentPlayerLevel;

        currentWeapon = Weapon.CurrentWeaponSprite();
        WeaponImage.sprite = currentWeapon.sprite;

    }

    public void RoomCleared()
    {
        roomsCleared++;
        roomText.GetComponent<TMP_Text>().text = "Rooms cleared: " + roomsCleared;
    }

    public void EnableGameobject(string objectName)
    {
        GameObject parentObject = this.gameObject;
        if (parentObject != null)
        {
            // Find the inactive child object by name
            Transform[] allChildren = parentObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.gameObject.name == objectName)
                {
                    // Enable the inactive object
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
