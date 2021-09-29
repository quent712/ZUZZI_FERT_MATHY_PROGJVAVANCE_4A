using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Action
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    SetBomb
};

// Possible Environment on the map
public enum MapEnvironment
{
    Wall,
    Breakable,
    Empty
    
}

// The map is an array containing the possible element of environment for each square 
public struct Map
{
    // Map constructor
    public Map(int mapX,int mapY) : this()
    {
        mapSizeX = mapX+2;
        mapSizeY = mapY+2;
        myMapLayout = CreateRandomMap();
    }
    
    public MapEnvironment[,] myMapLayout;
    public int mapSizeX;
    public int mapSizeY;

    // Creation of a map with only border walls, not random
    private MapEnvironment[,] CreateRandomMap()
    {
        MapEnvironment[,] newMap = new MapEnvironment[mapSizeX,mapSizeY];
        for (int j = 0; j < mapSizeY; j++)
        {
            for (int i = 0; i < mapSizeX; i++)
            {
                if (j == 0 || j == mapSizeY-1)
                {
                    newMap[i,j] = MapEnvironment.Wall;
                }
                else if (i == 0 || i == mapSizeX-1)
                {
                    newMap[i,j] = MapEnvironment.Wall;
                }
                else if (i % 2 != 0 && j % 2 != 0)
                {
                    newMap[i,j] = MapEnvironment.Wall;
                }
                else
                {
                    if (Random.Range(0, 100) >= 80)
                    {
                        newMap[i,j] = MapEnvironment.Breakable;
                    }
                    else
                    {
                        newMap[i,j] = MapEnvironment.Empty;
                    }
                }
            }
        }
        return newMap;
    }

}

// A player is a position and health pool
public struct Player
{
    private static int nbPlayer = 0; // increments to give each player a unique ID
    public static float movementStep = 10.0f; // the step by which a Player move on each key pressed
    
    public int playerID;
    public Vector2 position;
    public int health;
    public float timeuntilbomb;
    
    // TO BE CONTINUED: HANDLE POSITION RANDOMIZATION
    // Player Constructor
    public Player(Vector2 pos)
    {
        playerID = nbPlayer;
        nbPlayer++;
        position = pos;
        health = 1;
        timeuntilbomb = 0f;
    }
}

// A bomb is a position that sets an explosion after some time 
public struct Bomb
{
    private static float bombTimer = 3.0f;
    private static int nbBomb = 0;
    
    public int bombID;
    public bool exploding;
    public Vector2 position;
    public float explosionTime;
    public float explosionRadius;
    public List<Vector2> explosionSquares;
    
    // Bomb constructor
    public Bomb(Vector2 setPosition,float time,float radius = 5)
    {
        bombID = nbBomb;
        nbBomb++;
        exploding = false;
        explosionSquares = new List<Vector2>();
        position = setPosition;
        explosionTime = time + bombTimer;
        explosionRadius = radius;

    }

    public void explode()
    {
        exploding = true;
    }
}


// Model is gonna do most of calculation and handle collisions
public class Model
{
    private Map currentMap;
    private Player[] playerList;
    private Dictionary<int,Bomb> bombList;
    private Dictionary<string, object> myGameState;

    //////////////// TO BE CHANGED FOR PROPER SOLUTION ////////////
    public bool isBothPlayerAlive;

    // This code is kinda bad
    public Player getWinner()
    {
        foreach (Player player in playerList)
        {
            if (player.health > 0) return player;
        }
        return playerList[0];
    }
    ////////////////////////////////////////////////////////////
    
    private float inGameTimer;
    
    // TEMPORARY VARIABLES
    private List<int> idList;
    private Vector2 tempPosition;
    private Rect tempRect;
    
    // Init the different lists and had new players
    public Model(int mapX,int mapY, int numberOfPlayer)
    {
        inGameTimer = 0.0f;
        playerList = new Player[numberOfPlayer];
        bombList = new Dictionary<int, Bomb>();
        currentMap = new Map(mapX,mapY);
        isBothPlayerAlive = true;
        myGameState = new Dictionary<string, object>();
        for (int i = 0; i < numberOfPlayer; i++)
        {
            Vector2 pos = new Vector2(0,0);
            if (i == 0)
            {
                pos = new Vector2(2, 11);
            }
            else
            {
                pos = new Vector2(11, 2);
            }
            playerList[i] = (new Player(pos));
        }
    }

    // Check the nearest points to see if it is a wall
    private bool checkPossibleMovement(Vector2 posToCheck)
    {
        Vector2 closePoint = new Vector2((int) Math.Round(posToCheck.x, 0), (int) Math.Round(posToCheck.y, 0));
        
        if (closePoint.x >= 0 && closePoint.x < currentMap.mapSizeX && closePoint.y >= 0 &&
            closePoint.y < currentMap.mapSizeY)
        {
            if (currentMap.myMapLayout[(int) closePoint.y, (int) closePoint.x] != MapEnvironment.Empty)
            {
                tempRect = new Rect(closePoint.x - 0.5f, closePoint.y - 0.5f, 1, 1);
                if (tempRect.Contains(posToCheck)) return false;
            }
            return true;
        }
        return false;
    }
    
    private int checkPossibleExplosion(Vector2 explToCheck)
    {
        Vector2 closePoint = new Vector2((int) Math.Round(explToCheck.x, 0), (int) Math.Round(explToCheck.y, 0));
        
        if (closePoint.x >= 0 && closePoint.x < currentMap.mapSizeX && closePoint.y >= 0 &&
            closePoint.y < currentMap.mapSizeY)
        {
            if (currentMap.myMapLayout[(int) closePoint.y, (int) closePoint.x] == MapEnvironment.Wall)
            {
                tempRect = new Rect(closePoint.x - 0.5f, closePoint.y - 0.5f, 1, 1);
                if (tempRect.Contains(explToCheck)) return 0;
            }
            else if (currentMap.myMapLayout[(int) closePoint.y, (int) closePoint.x] == MapEnvironment.Breakable)
            {
                currentMap.myMapLayout[(int) closePoint.y, (int) closePoint.x] = MapEnvironment.Empty;
                return 2;
            }
            return 1;
        }
        return 0;
    }
    
    // Handle all possible action
    public void actionHandler(Action action, int playerID)
    {
        
        if (action != Action.SetBomb)
        {
            tempPosition = playerList[playerID].position;
            switch (action)
            {
                case Action.MoveUp:
                    tempPosition.y += Player.movementStep*Time.deltaTime;
                    break;
            
                case Action.MoveDown:
                    tempPosition.y -= Player.movementStep*Time.deltaTime;
                    break;
            
                case Action.MoveRight:
                    tempPosition.x += Player.movementStep*Time.deltaTime;
                    break;
            
                case Action.MoveLeft:
                    tempPosition.x -= Player.movementStep*Time.deltaTime;
                    break;
            
                default:
                    break;
            }
            // check if move is possible
            
            if(checkPossibleMovement(tempPosition)) playerList[playerID].position = tempPosition;
            
        }
        else dropBombAction(playerID);
    }
    
    // Drop a bomb at the position of a given player
    private void dropBombAction(int playerID)
    {
        //Debug.Log("Ingametimer" + inGameTimer);
        //Debug.Log("timeuntilbomb" + playerList[playerID].timeuntilbomb);
        if (inGameTimer > playerList[playerID].timeuntilbomb)
        {
            Bomb newBomb = new Bomb(playerList[playerID].position, inGameTimer);
            bombList.Add(newBomb.bombID, newBomb);
            playerList[playerID].timeuntilbomb = inGameTimer + 1f;
        }
    }
    
    // Returns the current state of the game in a dictionary
    public Dictionary<string, object> getGameState()
    {
        myGameState["MapInfo"] = currentMap;
        myGameState["InGameTime"] = inGameTimer;
        myGameState["PlayersInfo"] = playerList;
        myGameState["BombsInfo"] = bombList;
        
        return myGameState;
    }
    
    // Handles explosion collisions
    private void explosionCollision(Bomb explodingBomb)
    {
        int collisionResult;
        foreach (Vector2 direction in new Vector2[]{Vector2.up,Vector2.right, Vector2.down,Vector2.right})
        {
            for (int i = 1; i <= explodingBomb.explosionRadius; i++)
            {
                collisionResult = checkPossibleExplosion(explodingBomb.position + direction * i);
                
                if (collisionResult!=0)
                {
                    explodingBomb.explosionSquares.Add(explodingBomb.position + direction * i);
                    
                }
                if(collisionResult==2) break;
            }
        }

        foreach (Vector2 explSquare in explodingBomb.explosionSquares)
        {
            tempRect = new Rect(explSquare.x - 0.5f, explSquare.y - 0.5f, 1, 1);
            foreach (Player player in playerList)
            {
                if (tempRect.Contains(player.position))
                {
                    playerList[player.playerID].health = 0;
                
                    //////////////// TO BE CHANGED AS WELL ////////////////
                    isBothPlayerAlive = false;

                }
            }
            
        }
        
    }

    // On each update, check if a bomb should explode and launch explosion detection
    public void UpdateModel()
    {
        
        inGameTimer += Time.deltaTime;
        
        // Bomb suppression if timer expired
        idList = new List<int>(bombList.Keys);
        foreach (int bombKey in idList)
        {
            if (inGameTimer > bombList[bombKey].explosionTime)
            {
                if (!bombList[bombKey].exploding)
                {
                    explosionCollision(bombList[bombKey]);
                    bombList[bombKey].explode();
                }
                else bombList.Remove(bombKey);
            }
        }
        
        
    }
}
