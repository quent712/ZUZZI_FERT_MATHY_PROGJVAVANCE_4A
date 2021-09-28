using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible to sync the unity environment to the Model class
public class View
{
    private Dictionary<string,GameObject> playerObjectList;

    //private GameObject bombModel;

    //private GameObject wallModel;

    //private GameObject floorModel;

    //private GameObject destructibleEnvModel;
    
    // TO BE CONTINUED: ADD VISUAL MAP GENERATION
    //public View(Dictionary<string, object> gameState, GameObject player, GameObject bomb, GameObject wall,GameObject floor, GameObject destructibleEnv)
    public View(Dictionary<string, object> gameState, GameObject player)
    {
        playerObjectList = new Dictionary<string, GameObject>();
        foreach (Player playerInfo in (IEnumerable<Player>) gameState["PlayersInfo"])
        {
            GameObject newPlayer = GameObject.Instantiate(player);
            newPlayer.transform.position = new Vector3(playerInfo.position.x,0,playerInfo.position.y);
            newPlayer.name = playerInfo.playerID.ToString();
            playerObjectList.Add(newPlayer.name,newPlayer);
        }
    }
    
    // Update every model with the positions from Model
    public void UpdateView(Dictionary<string, object> gameState)
    {
        foreach (Player playerInfo in (IEnumerable) gameState["PlayersInfo"])
        {
            playerObjectList[playerInfo.playerID.ToString()].transform.position =
                new Vector3(playerInfo.position.x, 0, playerInfo.position.y);
        }
    }
}
