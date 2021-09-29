using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// Possible Movement direction for any given Player or IA
public enum MovementDirection
{
    Up,
    Down,
    Left,
    Right
};

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
                pos = new Vector2(11, 3);
            }
            playerList[i] = (new Player(pos));
        }
        
    }

    // Check the nearest points to see if it is a wall
    private bool canMakeMove(Vector2 posToCheck)
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
            if(canMakeMove(tempPosition)) playerList[playerID].position = tempPosition;
            
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
    private void explosionCollision(Vector2 bombPosition, float bombRadius)
    {
        Rect horizontal = new Rect(bombPosition.x - bombRadius, bombPosition.y - 0.5f, bombRadius * 2, 1.0f);
        Rect vertical = new Rect(bombPosition.x-0.5f, bombPosition.y - bombRadius, 1.0f, bombRadius*2);
        Debug.Log(horizontal.center+" vs "+bombPosition +" vs "+vertical.center);
        
        foreach (Player player in playerList)
        {
            if (horizontal.Contains(player.position) || vertical.Contains(player.position))
            {
                //Debug.Log("Exploded a player");
                playerList[player.playerID].health = 0;
                
                //////////////// TO BE CHANGED AS WELL ////////////////
                isBothPlayerAlive = false;

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
                // HANDLE EXPLOSION COLLISION HERE
                explosionCollision(bombList[bombKey].position,bombList[bombKey].explosionRadius);
                bombList.Remove(bombKey);
            }
        }
        
        
    }
}
