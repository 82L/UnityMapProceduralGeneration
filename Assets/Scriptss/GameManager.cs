using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Range(2, 500)]
    private int AmountOfRooms = 5;

    public static GameManager Instance;
    private int Room;
    private List<GameObject> Rooms;
    private GameObject CurrentRoom;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        GenerateRooms();
        SetRoom(0, PlayerSpawner.CardinalDirection.NORTH);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoom(int room, PlayerSpawner.CardinalDirection direction)
    {
        Room = room;
        GameObject RoomObject = this.Rooms[Room];
        RoomObject.SetActive(true);

        if (CurrentRoom != null)
            CurrentRoom.SetActive(false);

        CurrentRoom = RoomObject;

        GameObject Player = PlayerFactory.GetOrCreatePlayer();
    }

    public void GenerateRooms()
    {
        // Add here code to generate rooms

        this.Rooms = new List<GameObject>();

        for (int i = 0; i < AmountOfRooms; i++)
        {
            string RoomName = "Room " + i;
            GameObject RoomObject = GameObject.Find(RoomName);
            RoomObject.SetActive(false);
            this.Rooms.Add(RoomObject);
        }
    }
}