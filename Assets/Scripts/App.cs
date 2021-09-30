using System;
using UnityEngine;


public enum P2Input
{
    Multi,
    RandomAI,
    MCTSAI
}
// General class containing the main variables and responsible of initiating the different classes
public class App : MonoBehaviour
{
    private View myView;
    private Controller myController;
    private Model myModel;

    public int numberOfPlayer = 2;
    public int mapSizeX = 13;
    public int mapSizeY = 13;
    
    public GameObject player;
    public GameObject ennPlayer;
    public GameObject AIeasy;
    public GameObject AIhard;
    public GameObject bomb;
    public GameObject wall;
    public GameObject breakable;
    public GameObject fire;



    public P2Input currentMode;
    // TO BE ADDED
    
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    //public GameObject floorModel;

    //public GameObject destructibleEnvModel;

    // Initialize the different parts of the MVC model
    private void Start()
    {
        Time.timeScale = 1.0f;
        myModel = new Model(mapSizeX,mapSizeY,numberOfPlayer);
        CharacterRender charrender = new CharacterRender();
        MCTS1 mcts = new MCTS1(myModel);

        switch (AIandSound.Instance.Difficulty)
        {
            case "Easy":
                currentMode = P2Input.RandomAI;
                break;
            case "Hard":
                currentMode = P2Input.MCTSAI;
                break;
            case "Multiplayer":
                currentMode = P2Input.Multi;
                break;
            default:
                currentMode = P2Input.Multi;
                break;
        }
        
        
        myController = new Controller(mcts);
       
        myController.activeModel = myModel;
        
        myView = new View(myModel.getGameState(),player, ennPlayer, AIeasy,  AIhard ,currentMode, bomb, wall, breakable, fire);
        
        
        
    }

    private void Update()
    {
        myModel.inGameDeltaTime = Time.deltaTime;
        myController.UpdateController(currentMode);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Time.deltaTime);
        
        myModel.UpdateModel(Time.deltaTime);
        
        //////////////// TO BE CHANGED FOR PROPER SOLUTION ////////////
        if (!myModel.isBothPlayerAlive)
        {
            Player winner = myModel.getWinner();
            Debug.Log("Game Ended and I got a Winner: Player "+winner.playerID);

            if (winner.playerID == 0)
            {
                winPanel.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                losePanel.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
        /////////////////////////////////////////////////////////////
        
        myView.UpdateView(myModel.getGameState());
        
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
