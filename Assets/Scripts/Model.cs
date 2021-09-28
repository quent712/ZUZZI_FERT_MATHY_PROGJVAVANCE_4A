using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Possible Movement direction for any given Player or IA
public enum MovementDirection
{
    Up,
    Down,
    Left,
    Right
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
    public Map(int mapx,int mapY) : this()
    {
        mapSizeX = mapx;
        mapSizeY = mapY;
        myMapLayout = CreateRandomMap();
    }
    
    private MapEnvironment[,] myMapLayout;
    public int mapSizeX;
    public int mapSizeY;

    // Creation of a map with only border walls, not random
    private MapEnvironment[,] CreateRandomMap()
    {
        MapEnvironment[,] newMap = new MapEnvironment[mapSizeX+2,mapSizeY+2];
        for (int j = 0; j < mapSizeY+2; j++)
        {
            for (int i = 0; i < mapSizeX+2; i++)
            {
                if (j == 0 || j == +1)
                {
                    newMap[i,j] = MapEnvironment.Wall;
                }
                else if (i == 0 || i == +1)
                {
                    newMap[i,j] = MapEnvironment.Wall;
                }
                else
                {
                    newMap[i, j] = MapEnvironment.Empty;
                }
            }
        }
        return newMap;
    }

}

// A player is a position and health pool
public struct Player
{
    // TO BE CONTINUED: HANDLE POSITION RANDOMIZATION
    // In a struct, a constructor needs to have a parameter and be called using this parameter
    public Player(int hp = 1)
    {
        playerID = nbPlayer;
        nbPlayer++;
        position = new Vector2(1, 1);
        health = hp;

    }
    
    
    private static int nbPlayer = 0; // increments to give each player a unique ID
    public static float movementStep = 0.1f; // the step by which a Player move on each key pressed
    
    public int playerID;
    public Vector2 position;
    public int health;

    // TO BE MOVED TO MODEL CLASS TO HANDLE COLLISIONS
    public void makeAMove(MovementDirection direction)
    {   
        
        // Currently with direct incrementation
        switch (direction)
        {
            case MovementDirection.Up:
                position.y += movementStep;
                break;
            
            case MovementDirection.Down:
                position.y -= movementStep;
                break;
            
            case MovementDirection.Right:
                position.x += movementStep;
                break;
            
            case MovementDirection.Left:
                position.x -= movementStep;
                break;
            
            default:
                break;
        }
    }
}

// A bomb is a position that sets an explosion after some time 
public struct Bomb
{
    private static float bombTimer = 3.0f;
    
    public Vector2 position;
    public float explosionTime;
    
    public Bomb(Vector2 setPosition)
    {
        position = setPosition;
        explosionTime = Time.time + bombTimer;

    }
}


// Model is gonna do most of calculation and handle collisions
public class Model
{
    private Map currentMap;
    private Player[] playerList;
    private Bomb[] bombList;
    private Dictionary<string, object> myGameState;
    
    // Init the different lists and had new players
    public Model(int mapX,int mapY, int numberOfPlayer)
    {
        playerList = new Player[numberOfPlayer];
        bombList = new Bomb[10];
        currentMap = new Map(mapX,mapY);
        
        myGameState = new Dictionary<string, object>();
        for (int i = 0; i < numberOfPlayer; i++)
        {
            playerList[i] = (new Player(1));
        }
        
    }
    
    // TO BE CONTINUED: HANDLE BORDER DETECTION
    public void movementAction(MovementDirection chosenDirection, int playerID)
    {
        playerList[playerID].makeAMove(chosenDirection);
    }
    
    // Returns the current state of the game in a dictionary
    public Dictionary<string, object> getGameState()
    {
        myGameState["MapInfo"] = currentMap;
        myGameState["PlayersInfo"] = playerList;
        myGameState["BombsInfo"] = bombList;
        
        return myGameState;
    }
    
    // TO BE DONE: HANDLE BOMB EXPLOSION DETECTION
    public void UpdateModel()
    {
        
    }
}
