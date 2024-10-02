using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossRoomController : MonoBehaviour
{
    private Slider bossBar;
    [SerializeField] GameObject bossDoor;
    [SerializeField] GameObject bossBarPrefab;

    private bool bossAlive = true;
    private Enemy bossEntity;

    private void Start()
    {
        var bosses = Resources.LoadAll("Bosses/");
        var boss = bosses[Random.Range(0, bosses.Length)];
        bossEntity = ((GameObject)Instantiate(boss)).GetComponent<Enemy>();

        var bossBarGO = Instantiate(bossBarPrefab, GameObject.Find("Canvas").transform) as GameObject;
        bossBar = bossBarGO.GetComponent<Slider>();
        bossBarGO.GetComponentInChildren<TMP_Text>().text = boss.name;
    }

    //vitun paskaa koodia mutta ihan sama ei oo aikaa
    private void Update()
    {
        if (!bossAlive) return;

        if (!bossEntity)
        {
            bossAlive = false;
            bossDoor.SetActive(true);
            Destroy(bossBar.gameObject);
            Instantiate(Resources.Load("Pickups/HealthPickup") as GameObject, Vector3.zero, Quaternion.identity);
        }

        bossBar.value = bossEntity.currentHealth / bossEntity.maxHealth;

    }
}
