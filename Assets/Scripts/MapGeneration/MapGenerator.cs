using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MapGenerator
{
   public struct RoomData
   {
      public int2 Coordinates;
      public Dictionary<int2,int2> DoorNeighbors;
      public Room Room;
      public MapGenerationData.RoomType RoomType;
      public int Rotation;
   }
   

   /// <summary>
   /// Generate a map based on map data
   /// </summary>
   /// <param name="p_mapData">Map data to generate map</param>
   /// <returns>List of room data to instantiate the map</returns>
   public static List<RoomData> GenerateMap(MapGenerationData p_mapData)
   {
      Random.InitState(p_mapData.seed);
      List<int2> layout = LayoutCreator(p_mapData.numberOfTiles);
      List<RoomData> map = GetMapData(layout, p_mapData);
      return map;
   }

   /// <summary>
   /// Transform list of coordinates to a map. Coordinates must me continous
   /// </summary>
   /// <param name="layout">The lsit of room coordinates</param>
   /// <param name="p_mapData">The mapdata, to get the rooms from</param>
   /// <returns>A list of room data representing a map</returns>
   private static List<RoomData> GetMapData(List<int2> layout, MapGenerationData p_mapData)
   {
      List<RoomData> l_map = new List<RoomData>();
      foreach (var l_coordinates in layout)
      {
         RoomData l_newRoomData = new RoomData();
         l_newRoomData.Coordinates = l_coordinates;
         l_newRoomData.DoorNeighbors = new Dictionary<int2, int2>();
         // Counting neighbors of coordinate
         foreach (var l_neighbor in GameConstants.CoordinatesPossibleNeighbors)
         {
            int2 l_coordinateToTest = l_coordinates + l_neighbor;
            if (layout.Contains(l_coordinateToTest))
            {
               l_newRoomData.DoorNeighbors.Add(l_neighbor, l_coordinateToTest);
            }
         }
         
         l_newRoomData.RoomType = MapGenerationData.GetRoomType(l_newRoomData.DoorNeighbors.Keys);
         l_newRoomData.Rotation = GetRoomRotation(l_newRoomData.RoomType, l_newRoomData.DoorNeighbors.Keys);
         l_newRoomData.Room = p_mapData.GetRoom(l_newRoomData.RoomType);
         l_map.Add(l_newRoomData);
      }
      return l_map;
   }

   /// <summary>
   /// Get the rotation of a room based on its neighbors
   /// </summary>
   /// <param name="roomType">Type of the room</param>
   /// <param name="doorNeighbors">neighbors of the room</param>
   /// <returns></returns>
   private static int GetRoomRotation(MapGenerationData.RoomType roomType, ICollection<int2> doorNeighbors)
   {
      List<GameConstants.EntriesRotations> rotationsList = GameConstants.GetRoomPossibleRotations(roomType);
      List<int> possibleRotations = null;
      foreach (var rotations in rotationsList)
      {
         if (rotations.Entries.All(doorNeighbors.Contains))
         {
            possibleRotations = rotations.Rotations;
            break;
         }
      }

      if (possibleRotations is null) return 0;
      
      int index = Random.Range(0, possibleRotations.Count);
      return possibleRotations[index];
   }
   

   /// <summary>
   ///  Create the layout of the map
   /// </summary>
   /// <param name="p_numberOfRooms"> number of rooms for the map</param>
   /// <returns>A list containing the coordinates of every rooms</returns>
   private static List<int2> LayoutCreator(int p_numberOfRooms)
   {
      List<int2> l_roomPositions = new ();
      List<int2> l_possibleCoordinates = new () {new int2(0,0)};
      
      // Adding p_number of room
      for (int i = 0; i < p_numberOfRooms; i++)
      {
         int index;
         //get index of room
         int iterationCount = 0;
         while(true)
         {
            index = Random.Range(0, l_possibleCoordinates.Count);
            if (TestCoordinateApparitionChances(l_possibleCoordinates[index], l_roomPositions)) 
               break;
            Debug.Log(iterationCount);
            if (++iterationCount > 100)  
               break;
         }
         
         int2 l_newRoomCoordinates = l_possibleCoordinates[index];
         //If it is already a spawned room, we loop back, don't want 2 rooms at the same coordinates
         if (l_roomPositions.Contains(l_newRoomCoordinates))
         {
            i--;
            l_possibleCoordinates.Remove(l_newRoomCoordinates);
            continue;
         }
         //Adds the room
         l_roomPositions.Add(l_newRoomCoordinates);
         l_possibleCoordinates.Remove(l_newRoomCoordinates);
         //Adding neighbors of new room to the possible coordinates
         AddNeighbors(ref l_possibleCoordinates,l_newRoomCoordinates, l_roomPositions);
        
      }
      
      return l_roomPositions;
   }

   /// <summary>
   /// Adds neighbors of coordinate to list of potential new rooms
   /// </summary>
   /// <param name="p_possibleCoordinates">List of possible coordinates to add neighbors to</param>
   /// <param name="p_coordinates">coordinate to get neighbors from</param>
   /// <param name="p_roomPositions">current coordinates used</param>
   private static void AddNeighbors(ref List<int2> p_possibleCoordinates,int2 p_coordinates, ICollection<int2> p_roomPositions)
   {
      foreach (var l_neighbor in GameConstants.CoordinatesPossibleNeighbors)
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
   ///  Test to see if room can appear at given coordinates
   /// </summary>
   /// <param name="p_coordinateToTest"> the coordinate to test room spawn</param>
   /// <param name="p_currentRooms">List of given room</param>
   /// <returns>If the room can appear here</returns>
   private static bool TestCoordinateApparitionChances(int2 p_coordinateToTest, List<int2> p_currentRooms)
   {
      int l_numberOfNeighbors = 0;
      // Count neighbors
      foreach (var l_neighbor in GameConstants.CoordinatesPossibleNeighbors)
      {
         int2 l_coord = p_coordinateToTest + l_neighbor;
         if (p_currentRooms.Contains(l_coord))
         {
            l_numberOfNeighbors++;
         }
      }

      if (l_numberOfNeighbors == 0) l_numberOfNeighbors++;
      //Remap neighbors on scale going from 1 to 0, the higher we are, the closer to 0
      float l_numberOfNeighborsScale = math.remap(1f, GameConstants.CoordinatesPossibleNeighbors.Count, 1f, 0f, l_numberOfNeighbors);
      
      // Pow to get a non linear curve, less neighbors, more chances of apparition
      float l_chancesOfApparition = math.pow(l_numberOfNeighborsScale, 3);
      
      //Like a 1d100 throw
      float value = Random.Range(0f, 1f);
      return value <= l_chancesOfApparition;
   }
   
}
