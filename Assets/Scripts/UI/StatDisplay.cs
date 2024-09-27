using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    public PlayerLeveling Stat;
    public Player Weapon;
    public Image WeaponImage;
    public GameObject CurrentLevelText;
    void Start()
    {
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
