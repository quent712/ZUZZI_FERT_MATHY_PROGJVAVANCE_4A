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
    public GameObject bomb;
    public GameObject wall;
    public Randomer randomer;
    

    public bool randomIA = false;
    // TO BE ADDED
    
    public GameObject pauseCanvas;
    //public GameObject floorModel;

    //public GameObject destructibleEnvModel;

    // Initialize the different parts of the MVC model
    private void Start()
    {
        
        myModel = new Model(mapSizeX,mapSizeY,numberOfPlayer);
        CharacterRender charrender = new CharacterRender();
        Randomer rand = new Randomer(charrender);
        
        
        
        myController = new Controller(rand);
       
        
        myController.activeModel = myModel;
        
        myView = new View(myModel.getGameState(),player,bomb, wall);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        myController.UpdateController(randomIA);
        myModel.UpdateModel();
        myView.UpdateView(myModel.getGameState());
        
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
