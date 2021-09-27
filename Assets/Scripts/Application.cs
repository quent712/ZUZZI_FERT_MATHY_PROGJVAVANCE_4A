using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// General class containing the main variables and responsible of initiating the different classes
public class Application : MonoBehaviour
{
    private View myView;
    private Controller myController;
    private Model myModel;

    public int numberOfPlayer = 2;
    public int mapSizeX = 13;
    public int mapSizeY = 13;

    
    private void Start()
    {
        
        myModel = new Model(mapSizeX,mapSizeY,numberOfPlayer);
        
        myController = new Controller();
        myController.activeModel = myModel;
        
        myView = new View(myModel.getGameState());
        
    }

    // Update is called once per frame
    void Update()
    {
        myController.UpdateController();
    }
}
