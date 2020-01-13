using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerObject
{
    public static GameObject GetPlayerObject()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null)
        {
            foreach (GameObject player in players)
            {
                if (player.activeSelf == true)
                {
                    return player;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

}
