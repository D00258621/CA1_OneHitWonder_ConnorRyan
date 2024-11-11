using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameOver : MonoBehaviour //supposed to end code when all heads have been collected
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (player.GetComponent<PlayerMovement>().heads.Equals(6))
        {
            Time.timeScale = 0;
        }

    }

}
