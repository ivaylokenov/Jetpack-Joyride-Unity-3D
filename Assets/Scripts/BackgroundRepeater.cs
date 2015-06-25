using UnityEngine;
using System.Collections.Generic;

public class BackgroundRepeater : MonoBehaviour
{
    public List<GameObject> availableRooms;
    public List<GameObject> currentRooms;

    private float screenWidth;

    public void Start()
    {
        var height = Camera.main.orthographicSize * 2;
        this.screenWidth = Camera.main.aspect * height;
    }

    public void Update()
    {
        var playerPosition = this.transform.position;

        var maxWidthRoomIndexX = playerPosition.x + this.screenWidth;
        var minWidthRoomIndexX = playerPosition.x - this.screenWidth;

        float farthestDistanceX = 0.0f;

        var roomsToRemove = new List<GameObject>();

        foreach (var room in this.currentRooms)
        {
            var currentRoomX = room.transform.position.x + this.GetRoomWidth(room) / 2;
            farthestDistanceX = Mathf.Max(currentRoomX, farthestDistanceX);

            if (currentRoomX < minWidthRoomIndexX)
            {
                roomsToRemove.Add(room);
            }
        }

        foreach (var room in roomsToRemove)
        {
            this.currentRooms.Remove(room);
            Destroy(room.gameObject);
        }

        if (farthestDistanceX < maxWidthRoomIndexX)
        {
            this.AddRoom(farthestDistanceX);
        }
    }

    public void AddRoom(float farthestDistanceX)
    {
        var randomRoomIndex = Random.Range(0, this.availableRooms.Count);
        var room = Instantiate(this.availableRooms[randomRoomIndex]);
        var roomWidth = this.GetRoomWidth(room);
        room.transform.position = new Vector3(roomWidth / 2 + farthestDistanceX, 0, 0);

        this.currentRooms.Add(room);
    }

    private float GetRoomWidth(GameObject room)
    {
        var roomWidth = room.GetComponentInChildren<BoxCollider2D>().transform.localScale.x;
        return roomWidth;
    }
}
