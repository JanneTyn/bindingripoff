using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Door logic
/// </summary>
public class Door : MonoBehaviour
{
    private bool locked = true;
    public Vector2Int doorDirection;
    private Room currentRoom;

    public void Unlock()
    {
        currentRoom = transform.root.GetComponent<Room>();
        locked = false;

        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.CompareTag("Player")) return;

        if(!locked)
        {
            Level.instance.RoomTransition(currentRoom, doorDirection);
        }
    }
}