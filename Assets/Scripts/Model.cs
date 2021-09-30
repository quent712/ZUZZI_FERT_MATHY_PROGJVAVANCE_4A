using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

// Possible Actions Available for any player
public enum Action
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    SetBomb,
    Wait,
    Undertermined
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

    public Map(Map mapToCopy)
    {
        mapSizeX = mapToCopy.mapSizeX;
        mapSizeY = mapToCopy.mapSizeY;
        myMapLayout = new MapEnvironment[mapSizeX,mapSizeY];
        for (int j = 0; j < mapSizeY; j++)
        {
            for (int i = 0; i < mapSizeX; i++)
            {
                myMapLayout[i, j] = mapToCopy.myMapLayout[i, j];
            }
        }
    }
    
    public MapEnvironment[,] myMapLayout;
    public int mapSizeX;
    public int mapSizeY;

    // Creation of a map with breakables
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
    
    // Player Constructor
    public Player(Vector2 pos)
    {
        playerID = nbPlayer;
        nbPlayer++;
        position = pos;
        health = 1;
        timeuntilbomb = 0f;
    }

    public Player(Player playerToCopy)
    {
        playerID = playerToCopy.playerID;
        position = playerToCopy.position;
        health = playerToCopy.health;
        timeuntilbomb = playerToCopy.timeuntilbomb;
    }

    public static void setnbPlayer(int i)
    {
        nbPlayer = i;
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
    public List<Vector2> explosionSquares; // contains the squares on which the explosion will propagate
    
    // Bomb constructor
    public Bomb(Vector2 setPosition,float time,float radius = 3)
    {
        bombID = nbBomb;
        nbBomb++;
        exploding = false;
        explosionSquares = new List<Vector2>();
        position = setPosition;
        explosionTime = time + bombTimer;
        explosionRadius = radius;

    }

    public Bomb(Bomb bombToCopy)
    {
        bombID = bombToCopy.bombID;
        exploding = bombToCopy.exploding;
        explosionSquares = bombToCopy.explosionSquares.ConvertAll(vect => new Vector2(vect.x,vect.y));

        position = bombToCopy.position;
        explosionTime = bombToCopy.explosionTime;
        explosionRadius = bombToCopy.explosionRadius;
    }
}


// Model is gonna do most of calculation and handle collisions
public class Model
{
    private Map currentMap;
    private Player[] playerList;
    private Dictionary<int,Bomb> bombList;
    private Dictionary<string, object> myGameState;
    public float inGameTimer;
    public float inGameDeltaTime;

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
    
    
    
    // TEMPORARY VARIABLES
    private List<int> idList;
    private Vector2 tempPosition;
    private Rect tempRect;
    private Bomb tempBomb;
    private int tempInt;
    
    // Init the different lists and add new players
    public Model(int mapX,int mapY, int numberOfPlayer)
    {
        Player.setnbPlayer(0);
        inGameTimer = 0.0f;
        playerList = new Player[numberOfPlayer];
        bombList = new Dictionary<int, Bomb>();
        currentMap = new Map(mapX,mapY);
        isBothPlayerAlive = true;
        myGameState = new Dictionary<string, object>();
        
        
        // TO DO: HANDLE NOT SPAWNING ON BREAKABLE
        for (int i = 0; i < numberOfPlayer; i++)
        {

            tempPosition = new Vector2(Random.Range(0, currentMap.mapSizeX), Random.Range(0, currentMap.mapSizeX));
            while (currentMap.myMapLayout[(int) tempPosition.x, (int) tempPosition.y] != MapEnvironment.Empty)
            {
                tempPosition = new Vector2(Random.Range(0, currentMap.mapSizeX), Random.Range(0, currentMap.mapSizeX));
            }
            playerList[i] = new Player(tempPosition);
        }
    }

    public Model(Model modelToCopy)
    {
        inGameTimer = 0.0f;
        playerList = new Player[modelToCopy.playerList.Length];
        foreach (Player player in modelToCopy.playerList)
        {
            playerList[player.playerID] = new Player(player);
        }
        bombList = new Dictionary<int, Bomb>();
        foreach (KeyValuePair<int,Bomb> bombItem in modelToCopy.bombList)
        {
            bombList.Add(bombItem.Key,new Bomb(bombItem.Value));
        }
        currentMap = new Map(modelToCopy.currentMap);
        isBothPlayerAlive = true;
        myGameState = new Dictionary<string, object>();
    }

    // Check if a given position is in a wall or breakable
    private int checkPossiblePosition(Vector2 posToCheck,bool isExplosion = false)
    {
        Vector2 closePoint = new Vector2((int) Math.Round(posToCheck.x, 0), (int) Math.Round(posToCheck.y, 0));
        
        if (closePoint.x >= 0 && closePoint.x < currentMap.mapSizeX && closePoint.y >= 0 &&
            closePoint.y < currentMap.mapSizeY)
        {
            if (currentMap.myMapLayout[(int) closePoint.x, (int) closePoint.y] == MapEnvironment.Wall || currentMap.myMapLayout[(int) closePoint.x, (int) closePoint.y] == MapEnvironment.Breakable && !isExplosion)
            {
                tempRect = new Rect(closePoint.x - 0.5f, closePoint.y - 0.5f, 1, 1);
                if (tempRect.Contains(posToCheck)) return 0;
            }
            else if (currentMap.myMapLayout[(int) closePoint.x, (int) closePoint.y] == MapEnvironment.Breakable && isExplosion)
            {
                currentMap.myMapLayout[(int) closePoint.x, (int) closePoint.y] = MapEnvironment.Empty;
                return 2;
            }
            return 1;
        }
        return 0;
    }
    
    
    // Handler for all possible actions
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
            // for a move action check if it is possible
            if(checkPossiblePosition(tempPosition)==1) playerList[playerID].position = tempPosition;
            
        }
        else dropBombAction(playerID);
    }
    
    // Drop a bomb at the position of a given player
    private void dropBombAction(int playerID)
    {
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
        explodingBomb.explosionSquares.Add(explodingBomb.position);
        // for each direction check if explosion is gonna be stopped by a wall
        foreach (Vector2 direction in new Vector2[]{Vector2.up,Vector2.right, Vector2.down,Vector2.left})
        {
            for (int i = 1; i < explodingBomb.explosionRadius; i++)
            {
                tempInt = checkPossiblePosition(explodingBomb.position + direction * i, true);
                if (tempInt!=0)
                {
                    explodingBomb.explosionSquares.Add(explodingBomb.position + direction * i);
                }
                if(tempInt!=1) break;
            }
        }
        
        // check if a player is in the explosion
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
    public void UpdateModel(float deltaTime)
    {
        // Update the in game timer for computation
        inGameDeltaTime = deltaTime;
        inGameTimer += inGameDeltaTime;
        
        // Create an explosion if bomb timer has expired
        idList = new List<int>(bombList.Keys);
        foreach (int bombKey in idList)
        {
            if (inGameTimer > bombList[bombKey].explosionTime)
            {
                if (!bombList[bombKey].exploding)
                {
                    tempBomb = bombList[bombKey];
                    explosionCollision(tempBomb);
                    tempBomb.exploding = true;
                    bombList[bombKey] = tempBomb;
                }
                else bombList.Remove(bombKey);
            }
        }
    }
}
