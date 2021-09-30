using UnityEngine;



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
    public GameObject AIeasy;
    public GameObject AIhard;
    public GameObject bomb;
    public GameObject wall;
    public GameObject breakable;
    public GameObject fire;
    public Randomer randomer;
    
    

    public bool randomIA = false;

    public bool MCTSIA = false;
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

        if (AIandSound.Instance.Difficulty == "Easy")
        {
            randomIA = true;
        }else if (AIandSound.Instance.Difficulty == "Hard")
        {
            randomIA = false;
        }
        
        
        myController = new Controller(mcts);
       
        myController.activeModel = myModel;
        
        myView = new View(myModel.getGameState(),player, AIeasy,  AIhard ,AIandSound.Instance.Difficulty, bomb, wall, breakable, fire);
        
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
        myController.UpdateController(randomIA,MCTSIA);
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
