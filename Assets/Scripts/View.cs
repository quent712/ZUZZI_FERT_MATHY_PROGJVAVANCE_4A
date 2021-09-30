using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible to sync the unity environment to the Model class
public class View
{
    // The dictionaries and lists contains the unity gameObjects
    private GameObject playerObject;
    private Dictionary<int,GameObject> playerObjectDict;

    private GameObject player2Object;
    
    private GameObject AIeasyObject;
    private Dictionary<int,GameObject> AIeasyObjectDict;

    private GameObject AIhardObject;
    private Dictionary<int,GameObject> AIhardObjectDict;
    
    private GameObject bombObject;
    private Dictionary<int,GameObject> bombObjectDict;
    
    private GameObject wallObject;
    private List<GameObject> wallObjectList;
    
    private GameObject breakableObject;
    private Dictionary<Vector2, GameObject> breakableObjectDict;

    private GameObject fireObject;

    private string Difficulty;
    // TEMPORARY VARIABLES
    private Dictionary<int, Bomb> tempDict;
    private List<int> idList;
    private Map tempMap;
    private List<Vector2> tempVectorList;

    // View constructor
    public View(Dictionary<string, object> gameState, GameObject player, GameObject player2, GameObject aieasy, GameObject aihard, string difficulty, GameObject bomb, GameObject wall, GameObject breakable, GameObject fire)
    {
        // We add the prefab so it can be generated
        playerObject = player;
        playerObjectDict = new Dictionary<int, GameObject>();

        player2Object = player2;
        
        AIeasyObject = aieasy;
        AIeasyObjectDict = new Dictionary<int, GameObject>();
        
        AIhardObject = aihard;
        AIhardObjectDict = new Dictionary<int, GameObject>();

        bombObject = bomb;
        bombObjectDict = new Dictionary<int, GameObject>();
        
        wallObject = wall;
        wallObjectList = new List<GameObject>();
        
        breakableObject = breakable;
        breakableObjectDict = new Dictionary<Vector2, GameObject>();

        fireObject = fire;

        Difficulty = difficulty;

        // For each player from Model we instantiate a new Player model
        
        foreach (Player playerInfo in (IEnumerable) gameState["PlayersInfo"])
        {
            GameObject tempmodel = null;
            if (playerInfo.playerID == 0)
            {
                tempmodel = playerObject;
            }
            else if(playerInfo.playerID == 1 && Difficulty == "Multiplayer")
            {
                tempmodel = player2Object;
            }
            else if (Difficulty == "Easy")
            {
                tempmodel = AIeasyObject;
            }
            else if (Difficulty == "Hard")
            {
                tempmodel = AIhardObject;
            }
            
            GameObject newPlayer = GameObject.Instantiate(tempmodel);
            newPlayer.transform.position = new Vector3(playerInfo.position.x, 0, playerInfo.position.y);
            newPlayer.name = playerInfo.playerID.ToString();
            playerObjectDict.Add(playerInfo.playerID, newPlayer);

        }
        
        // Visual Map generation
        tempMap = (Map) gameState["MapInfo"];
        for (int i=0;  i < tempMap.mapSizeX; i++)
        {
            for (int j = 0;  j<tempMap.mapSizeY; j++)
            {
                if (tempMap.myMapLayout[i,j] == MapEnvironment.Wall)
                {
                    wallObjectList.Add(GameObject.Instantiate(wallObject, new Vector3(i, 0, j), Quaternion.identity));
                }
                else if (tempMap.myMapLayout[i,j] == MapEnvironment.Breakable)
                {
                    breakableObjectDict.Add(new Vector2(i,j),GameObject.Instantiate(breakableObject,new Vector3(i,0,j),Quaternion.identity));
                }
            }
        }
    }
    
    // Update every model with the positions from Model
    public void UpdateView(Dictionary<string, object> gameState)
    {
        
        // Update Players Position
        foreach (Player playerInfo in (IEnumerable) gameState["PlayersInfo"])
        {
            playerObjectDict[playerInfo.playerID].transform.position =
                new Vector3(playerInfo.position.x, 0, playerInfo.position.y);
        }
        
        // Update the bombs state and display an explosion if exploding
        foreach (KeyValuePair<int,Bomb> bombItem in (IEnumerable) gameState["BombsInfo"])
        {
            // if new bomb add
            if (!bombObjectDict.ContainsKey(bombItem.Key))
            {
                GameObject newBomb = GameObject.Instantiate(bombObject);
                newBomb.transform.position = new Vector3(bombItem.Value.position.x, 0, bombItem.Value.position.y);
                bombObjectDict.Add(bombItem.Key,newBomb);
            }
            
            // Create an explosion
            if (bombItem.Value.exploding)
            {
                foreach (Vector2 explosionPosition in bombItem.Value.explosionSquares)
                {
                    GameObject.Instantiate(fireObject, new Vector3(explosionPosition.x, 0, explosionPosition.y),
                        Quaternion.identity);
                }
                GameObject.Destroy(bombObjectDict[bombItem.Key]);
                bombObjectDict.Remove(bombItem.Key);
                
                SoundManager.Instance.PlaySoundEffectBoom();
            }
        }
        
        // Update breakable environment
        tempMap = (Map) gameState["MapInfo"];
        tempVectorList = new List<Vector2>(breakableObjectDict.Keys);
        foreach (Vector2 breakPos in tempVectorList)
        {
            if (tempMap.myMapLayout[(int) breakPos.x, (int) breakPos.y] == MapEnvironment.Empty)
            {
                GameObject.Destroy(breakableObjectDict[breakPos]);
                breakableObjectDict.Remove(breakPos);
            }
        }

    }
}
