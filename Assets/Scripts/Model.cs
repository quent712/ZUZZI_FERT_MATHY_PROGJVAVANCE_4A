using System;using System.Collections.Generic;
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
        myMapLayout = createRandomMap();
    }
    private MapEnvironment[,] myMapLayout;
    public int mapSizeX;
    public int mapSizeY;

    // TO BE CONTINUED
    private MapEnvironment[,] createRandomMap()
    {
        MapEnvironment[,] newMap = new MapEnvironment[mapSizeX+2,mapSizeY+2];
        return newMap;
    }

}

// A player is a position and health pool
public struct Player
{
    // CONSTRUCTOR TO BE ADDED
    public int playerID;
    public Vector2 position;
    public int health;
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
    
    // TO BE CONTINUED
    public Model(int mapX,int mapY, int numberOfPlayer)
    {
        myGameState = new Dictionary<string, object>();
        currentMap = new Map();
    }
    
    // TO BE CONTINUED
    public void movementAction(MovementDirection chosenDirection,int playerID){}

    public Dictionary<string, object> getGameState()
    {
        myGameState["MapInfo"] = currentMap;
        myGameState["PlayersInfo"] = playerList;
        myGameState["BombsInfo"] = bombList;
        
        return myGameState;
    }
}
