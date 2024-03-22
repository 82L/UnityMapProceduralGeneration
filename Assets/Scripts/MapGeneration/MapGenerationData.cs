using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "Map Generation Data", menuName = "Map Generation Data", order = 0)]
public class MapGenerationData : ScriptableObject
{
    public int numberOfTiles;
    public int seed;
    public enum RoomType
    {
        Door1,
        Door2I,
        Door2L,
        Door3,
        Door4
    }

    public List<Room> rooms1Doors = new ();
    public List<Room> rooms2DoorsI = new ();
    public List<Room> rooms2DoorsL = new ();
    public List<Room> rooms3Doors = new ();
    public List<Room> rooms4Doors = new ();

    public Room GetRoom(RoomType p_roomType)
    {
        List<Room> l_selectedRoomCollection = p_roomType switch
        {
            RoomType.Door1 => rooms1Doors,
            RoomType.Door2I => rooms2DoorsI,
            RoomType.Door2L => rooms2DoorsL,
            RoomType.Door3 => rooms3Doors,
            RoomType.Door4 => rooms4Doors,
            _ => rooms4Doors
        };
        return RetrieveRoom(l_selectedRoomCollection);
    }

    /// <summary>
    /// Get the room type of a room
    /// </summary>
    /// <param name="doorNeighbors">neighbors of room</param>
    /// <returns>return the room type</returns>
    /// <exception cref="NotImplementedException">Shouldn't  have more than 4 neighbor, it's a grid !</exception>
    public static RoomType GetRoomType(IReadOnlyCollection<int2> doorNeighbors)
    {
        return doorNeighbors.Count switch
        {
            1 => RoomType.Door1,
            2 when doorNeighbors.First().Equals(doorNeighbors.Last() * -1) => RoomType.Door2I,
            2 => RoomType.Door2L,
            3 => RoomType.Door3,
            4 => RoomType.Door4,
            _ => throw new NotImplementedException()
        };
    }

    private Room RetrieveRoom(List<Room> p_roomCollection)
    {
        int l_index = 0;
        int iterationCount = 0;
        while (true)
        {
            l_index = Random.Range(0, p_roomCollection.Count);
            float l_appearanceTest = Random.Range(0f, 1f);
            if (l_appearanceTest <= p_roomCollection[l_index].apparitionFrequency)
                break;
            if(++iterationCount>100)
                break;
        }

        return p_roomCollection[l_index];
    }

}
    
[Serializable]
public class Room
{
    public GameObject data;
    [Range(0, 1f)]
    public float apparitionFrequency = 1f;
}