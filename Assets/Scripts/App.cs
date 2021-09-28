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
    
    
    // TO ADDED
    //public GameObject bombModel;

    //public GameObject wallModel;

    //public GameObject floorModel;

    //public GameObject destructibleEnvModel;

    // Initialize the different parts of the MVC model
    private void Start()
    {
        
        myModel = new Model(mapSizeX,mapSizeY,numberOfPlayer);
        
        myController = new Controller();
        myController.activeModel = myModel;
        
        myView = new View(myModel.getGameState(),player);
        
    }

    // Update is called once per frame
    void Update()
    {
        myController.UpdateController();
        myModel.UpdateModel();
        myView.UpdateView(myModel.getGameState());
    }
}
