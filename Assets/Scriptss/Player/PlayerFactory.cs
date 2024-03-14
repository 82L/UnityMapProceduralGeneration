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
            PlayerSpawner[] playerStarts = GameObject.FindObjectsOfType<PlayerSpawner>();
            Vector3 startLocation = Vector3.zero;

            if (playerStarts.Length >= 0)
                startLocation = playerStarts[0].transform.position;

            startLocation.y += 1f;

            GameObject PlayerPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Models/Prefabs/Player/Player.prefab").WaitForCompletion();
            Player = GameObject.Instantiate(PlayerPrefab);
            Player.transform.position = startLocation;
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