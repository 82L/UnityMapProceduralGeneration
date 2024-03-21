using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerFactory
{
    public static GameObject Player;

    public static GameObject GetOrCreatePlayer()
    {
        if (Player == null)
        {
            Vector3 StartLocation = Vector3.zero;
            StartLocation.y = 2f;

            GameObject PlayerPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Models/Prefabs/Player/Player.prefab").WaitForCompletion();
            Player = GameObject.Instantiate(PlayerPrefab);
            Player.transform.position = StartLocation;
        }

        return Player;
    }

    public static void ResetPlayer()
    {
        if (Player != null)
        {
            GameObject.Destroy(Player.gameObject);
            Player = null;
        }
    }
}