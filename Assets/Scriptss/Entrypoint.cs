using System;
using System.Collections.Generic;
using UnityEngine;

public class GameApp
{
    [RuntimeInitializeOnLoadMethod]
    static void Bootstrap()
    {
        var MyGameManager = new GameObject("Game");
        MyGameManager.AddComponent<GameManager>();
    }
}