using System;
using System.Collections.Generic;
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

    private Room RetrieveRoom(List<Room> p_roomCollection)
    {
        bool l_isSelected = false;
        int l_index = 0;
        do
        {
            l_index = Random.Range(0, p_roomCollection.Count);
            float l_appearanceTest = Random.Range(0f, 1f);
            if (l_appearanceTest <= p_roomCollection[l_index].apparitionFrequency)
            {
                l_isSelected = true;
            }

        } while (!l_isSelected);

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