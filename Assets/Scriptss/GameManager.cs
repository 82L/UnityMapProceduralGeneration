using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
            return;
        }

        GameObject Player = PlayerFactory.GetOrCreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcul position caméra selon position joueur
    }
}