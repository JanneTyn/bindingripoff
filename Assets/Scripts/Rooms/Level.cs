using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level logic
/// </summary>
public class Level : MonoBehaviour
{
    #region Singleton
    public static Level instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than 1 instance of Level exists!");
            enabled = false;
        }
        else instance = this;
    }
    #endregion

    private List<Vector2Int> roomLayout = new();
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform.root;
        roomLayout.Add(new Vector2Int(0, 0));
    }

    /// <summary>
    /// TODO this shit sucks remake it
    /// </summary>
    /// <param name="currentRoom"></param>
    /// <param name="roomDirection"></param>
    public void RoomTransition(Room currentRoom, Vector2Int roomDirection)
    {
        Debug.Log(currentRoom.coordinate);
        Debug.Log(roomDirection);
        int nextRoomDistance = roomDirection.y == 0 ? 32 : 20;

        if (!RoomExists(currentRoom.coordinate + roomDirection))
        {
            roomLayout.Add(currentRoom.coordinate + roomDirection);
            var room = Instantiate(Resources.Load("Room") as GameObject, currentRoom.transform.position + (Vector3)(Vector2)roomDirection * nextRoomDistance, Quaternion.identity);
            room.GetComponent<Room>().coordinate = currentRoom.coordinate + roomDirection;
        }

        CameraController.instance.MoveCamera(roomDirection * nextRoomDistance);
        player.Translate((Vector2)roomDirection * 6f);

    }

    public bool RoomExists(Vector2Int roomPos)
    {
        return roomLayout.Contains(roomPos);
    }
}