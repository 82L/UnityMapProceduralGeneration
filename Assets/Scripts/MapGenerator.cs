using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class MapGenerator
{
   public struct RoomData
   {
      public int2 Coordinates;
      public Dictionary<int2,int2> DoorNeighbors;
      public Room Room;
      public MapGenerationData.RoomType RoomType;
   }

   // private GameObject m_prefab;

   // private GameObject m_parent;
   private static readonly List<int2> m_possibleNeighbors = new List<int2>
   {
      new (0, 1),
      new (1, 0),
      new (-1, 0),
      new (0, -1),
   };
   

   public static List<RoomData> GenerateMap(MapGenerationData p_mapData)
   {
      Random.InitState(p_mapData.seed);
      List<int2> layout = LayoutCreator(p_mapData.numberOfTiles);
      List<RoomData> map = GetMapData(layout, p_mapData);
      return map;
   }

   private static List<RoomData> GetMapData(List<int2> layout, MapGenerationData p_mapData)
   {
      List<RoomData> l_map = new List<RoomData>();
      foreach (var l_coordinates in layout)
      {
         RoomData l_newRoomData = new RoomData();
         l_newRoomData.Coordinates = l_coordinates;
         l_newRoomData.DoorNeighbors = new Dictionary<int2, int2>();
         foreach (var l_neighbor in m_possibleNeighbors)
         {
            int2 l_coordinateToTest = l_coordinates + l_neighbor;
            if (layout.Contains(l_coordinateToTest))
            {
               l_newRoomData.DoorNeighbors.Add(l_neighbor, l_coordinateToTest);
            }
         }
         l_newRoomData.RoomType = GetRoomType(l_newRoomData.DoorNeighbors);
         l_newRoomData.Room = p_mapData.GetRoom(l_newRoomData.RoomType);
         l_map.Add(l_newRoomData);
      }
      return l_map;
   }

   private static MapGenerationData.RoomType GetRoomType(Dictionary<int2, int2> doorNeighbors)
   {
      return doorNeighbors.Count switch
      {
         1 => MapGenerationData.RoomType.Door1,
         2 when doorNeighbors.Keys.First().Equals(doorNeighbors.Keys.Last() * -1) => MapGenerationData.RoomType.Door2I,
         2 => MapGenerationData.RoomType.Door2L,
         3 => MapGenerationData.RoomType.Door3,
         4 => MapGenerationData.RoomType.Door4,
         _ => throw new NotImplementedException()
      };
   }
   


   private static List<int2> LayoutCreator(int p_numberOfRooms)
   {
      List<int2> l_roomPositions = new ();
      List<int2> l_possibleCoordinates = new () {new int2(0,0)};
      var max = new int2(0, 0);
      
      // Adding p_number of room
      for (int i = 0; i < p_numberOfRooms; i++)
      {
         int index;
         //get index of room
         do
         {
            index = Random.Range(0, l_possibleCoordinates.Count);

         } while (!TestCoordinateApparitionChances(l_possibleCoordinates[index], l_roomPositions));
         
         int2 l_newRoomCoordinates = l_possibleCoordinates[index];
         if (l_roomPositions.Contains(l_newRoomCoordinates))
         {
            i--;
            l_possibleCoordinates.Remove(l_newRoomCoordinates);
            continue;
         }
         l_roomPositions.Add(l_newRoomCoordinates);
         l_possibleCoordinates.Remove(l_newRoomCoordinates);
         AddNeighbors(ref l_possibleCoordinates,l_newRoomCoordinates, l_roomPositions);
         max.x = Math.Max(max.x, l_newRoomCoordinates.x);
         max.y = Math.Max(max.y, l_newRoomCoordinates.y);
      }
      
      return l_roomPositions;
   }

   private static void AddNeighbors(ref List<int2> p_possibleCoordinates,int2 p_coordinates, ICollection<int2> p_roomPositions)
   {
      foreach (var l_neighbor in m_possibleNeighbors)
      {
         int2 l_coord = p_coordinates + l_neighbor;
         if (!p_roomPositions.Contains(l_coord))
         {
            p_possibleCoordinates.Add(l_coord);
         }
      }
      p_possibleCoordinates = p_possibleCoordinates.Distinct().ToList();
   }

   /// <summary>
   ///  Test If coordinate can appear
   /// </summary>
   /// <param name="p_coordinateToTest"></param>
   /// <param name="p_currentRooms"></param>
   /// <returns></returns>
   private static bool TestCoordinateApparitionChances(int2 p_coordinateToTest, List<int2> p_currentRooms)
   {
      int l_numberOfNeighbors = 0;
      foreach (var l_neighbor in m_possibleNeighbors)
      {
         int2 l_coord = p_coordinateToTest + l_neighbor;
         if (p_currentRooms.Contains(l_coord))
         {
            l_numberOfNeighbors++;
         }
      }
      float l_valueLinear = math.remap(1f, m_possibleNeighbors.Count, 1f, 0f, l_numberOfNeighbors);
      float l_vFinal = math.pow(l_valueLinear, 3);
      float value = Random.Range(0f, 1f);
      return value <= l_vFinal;
   }
   

 
}
