using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{

    public float healAmountPercentage = 25;
    public static float healMultiplier = 1.0f;
    private Player player;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("TestPlayer").GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player")) //player layer
        {
            player.Heal(healAmountPercentage * healMultiplier);
            Destroy(gameObject);
        }
    }
}
