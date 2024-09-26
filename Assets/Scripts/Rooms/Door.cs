using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool locked = true;
    public Vector2 doorDirection;

    public void Unlock()
    {
        locked = false;

        var green = Color.green;
        green.a = 0.5f;
        GetComponent<SpriteRenderer>().color = green;

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.CompareTag("Player")) return;

        if(!locked)
        {
            //Todo make this better lol
            int nextRoomDistance = doorDirection.y == 0 ? 32 : 16;
            Instantiate(Resources.Load("Room") as GameObject, transform.root.position + (Vector3)doorDirection * nextRoomDistance, Quaternion.identity);
            collider.transform.position = collider.transform.position + (Vector3)doorDirection * 6f;
            CameraController.instance.MoveCamera(doorDirection * nextRoomDistance);
        }
    }
}
