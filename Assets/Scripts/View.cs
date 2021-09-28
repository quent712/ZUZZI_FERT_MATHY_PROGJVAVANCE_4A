using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible to sync the unity environment to the Model class
public class View
{
    private GameObject playerObject;
    private Dictionary<int,GameObject> playerObjectDict;

    private GameObject bombObject;
    private Dictionary<int,GameObject> bombObjectDict;

    private GameObject wallModel;
    private Dictionary<int, GameObject> wallModelDict;

    //private GameObject floorModel;

    //private GameObject destructibleEnvModel;
    
    // TO BE CONTINUED: ADD VISUAL MAP GENERATION
    //public View(Dictionary<string, object> gameState, GameObject player, GameObject bomb, GameObject wall,GameObject floor, GameObject destructibleEnv)
    public View(Dictionary<string, object> gameState, GameObject player, GameObject bomb, GameObject wall)
    {
        playerObject = player;
        playerObjectDict = new Dictionary<int, GameObject>();

        bombObject = bomb;
        bombObjectDict = new Dictionary<int, GameObject>();
        
        wallModel = wall;
        wallModelDict = new Dictionary<int, GameObject>();
        
        foreach (Player playerInfo in (IEnumerable) gameState["PlayersInfo"])
        {
            GameObject newPlayer = GameObject.Instantiate(playerObject);
            newPlayer.transform.position = new Vector3(playerInfo.position.x,0,playerInfo.position.y);
            newPlayer.name = playerInfo.playerID.ToString();
            playerObjectDict.Add(playerInfo.playerID,newPlayer);
            
        }

        Map temp = (Map) gameState["MapInfo"];
        
        for (int i=0;  i < 15; i++)
        {
            
            int padz = i;
            for (int j = 0;  j<15; j++)
            {
                int padx = j;
                
                if (temp.myMapLayout[i,j] == MapEnvironment.Wall)
                {
                    BlockFactory.Factory(wall, j+padx,i+padz);
                }
                else
                {
                    //Debug.Log("Nothing Here");
                }
            }
        }
    }
    
    // Update every model with the positions from Model
    public void UpdateView(Dictionary<string, object> gameState)
    {
        
        foreach (Player playerInfo in (IEnumerable) gameState["PlayersInfo"])
        {
            playerObjectDict[playerInfo.playerID].transform.position =
                new Vector3(playerInfo.position.x, 0, playerInfo.position.y);
        }
        
        foreach (KeyValuePair<int,GameObject> bombItem in bombObjectDict)
        {
            // TO BE CONTINUED
        }
        
        foreach (KeyValuePair<int,Bomb> bombItem in (IEnumerable) gameState["BombsInfo"])
        {
            if (!bombObjectDict.ContainsKey(bombItem.Key))
            {
                GameObject newBomb = GameObject.Instantiate(bombObject);
                newBomb.transform.position = new Vector3(bombItem.Value.position.x, 0, bombItem.Value.position.y);
                bombObjectDict.Add(bombItem.Key,newBomb);
            }
        }
        
        // Update dynamic environment here with gameState["MapInfo"]

    }
}
