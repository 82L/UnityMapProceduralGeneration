using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class MapGeneratorComponent : MonoBehaviour
    {
        [SerializeField]private GameObject prefab;
        [SerializeField]private MapGenerationData data;

        private void Start()
        {
            // MapGenerator mapGenerator = new MapGenerator();
            List<MapGenerator.RoomData> l_roomPositions = MapGenerator.GenerateMap(data);
            foreach (var roomData in l_roomPositions)
            {
                GameObject go = Instantiate(roomData.Room.data, transform);
                go.transform.position = new Vector3(roomData.Coordinates.x, 0, roomData.Coordinates.y);
            }
        }
        
        
    }
}