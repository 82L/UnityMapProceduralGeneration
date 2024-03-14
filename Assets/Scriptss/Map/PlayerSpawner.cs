using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public enum CardinalDirection
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public CardinalDirection Direction;
    public int TargetRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.parent.transform.parent.name);
        if (other.transform.parent.transform.parent.name == "Player(Clone)")
        {
            GameManager.Instance.SetRoom(TargetRoom, Direction);
        }
    }
}
