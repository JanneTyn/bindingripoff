using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    private PlayerLeveling Stat;
    private Player Weapon;
    private Image WeaponImage;
    private GameObject CurrentLevelText;
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
        WeaponImage.sprite = Weapon.CurrentWeaponSprite();
    }
}
