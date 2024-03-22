using System;
using System.Collections.Generic;
using Unity.Mathematics;

public static class GameConstants
{
    public struct EntriesRotations
    {
        public List<int2> Entries;
        public List<int> Rotations;
    }
    
    public static readonly List<int2> CoordinatesPossibleNeighbors = new List<int2>
    {
        new (0, 1),
        new (1, 0),
        new (-1, 0),
        new (0, -1),
    };

    public static readonly List<EntriesRotations> RoomOrientationDoors1 = new()
    {
        new EntriesRotations
        {
            Entries = new List<int2> { new(0, 1) },
            Rotations = new List<int> { 0 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(1, 0) },
            Rotations = new List<int> { 90 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(0, -1) },
            Rotations = new List<int> { 180 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(-1, 0) },
            Rotations = new List<int> { -90 }
        }, 
    };
    public static readonly List<EntriesRotations> RoomOrientationDoors2I = new()
    {
        new EntriesRotations
        {
            Entries = new List<int2> { new(0, 1),new(0, -1) },
            Rotations = new List<int> { 0,180 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(1, 0), new(-1, 0) },
            Rotations = new List<int> { 90, -90 }
        }, 
    };
    public static readonly List<EntriesRotations> RoomOrientationDoors2L = new()
    {
        new EntriesRotations
        {
            Entries = new List<int2> { new(0, 1),new(1, 0) },
            Rotations = new List<int> { 0 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(1, 0),new(0, -1) },
            Rotations = new List<int> { 90 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(0, -1),new(-1, 0) },
            Rotations = new List<int> { 180 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(-1, 0),new(0, 1) },
            Rotations = new List<int> { -90 }
        }, 
    };
    public static readonly List<EntriesRotations> RoomOrientationDoors3 = new()
    {
        new EntriesRotations
        {
            Entries = new List<int2> {new(-1, 0), new(0, 1),new(1, 0) },
            Rotations = new List<int> { 0 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(0, 1),new(1, 0),new(0, -1), },
            Rotations = new List<int> { 90 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> { new(1, 0),new(0, -1),new(-1, 0) },
            Rotations = new List<int> { 180 }
        }, 
        new EntriesRotations
        {
            Entries = new List<int2> {new(0, -1), new(-1, 0),new(0, 1) },
            Rotations = new List<int> { -90 }
        }, 
    };
    public static readonly List<EntriesRotations> RoomOrientationDoors4 = new()
    {
        new EntriesRotations
        {
            Entries = new List<int2> {new(-1, 0), new(0, 1),new(1, 0),new(1, 0) },
            Rotations = new List<int> { 0,90,180,-90 }
        }, 
    };
    
    public static List<EntriesRotations> GetRoomPossibleRotations(MapGenerationData.RoomType p_roomType)
    {
        List<EntriesRotations> l_selectedListOfOrientations= p_roomType switch
        {
            MapGenerationData.RoomType.Door1 => RoomOrientationDoors1,
            MapGenerationData.RoomType.Door2I => RoomOrientationDoors2I,
            MapGenerationData.RoomType.Door2L => RoomOrientationDoors2L,
            MapGenerationData.RoomType.Door3 => RoomOrientationDoors3,
            MapGenerationData.RoomType.Door4 => RoomOrientationDoors4,
            _ => RoomOrientationDoors4
        };
        return l_selectedListOfOrientations;
    }
}