using UnityEngine;

// Conceptually the Controller is the interface with which the players will interact
// It will transmit instructions to the model on what actions to take
public class Controller
{

    public Model activeModel;
    
    // Listens to Player action
    public void UpdateController()
    {
        
        // FOR PLAYER VS PLAYER TAKE INTO ACCOUNT ALTERNATE CONTROL SCHEME
        // PLAYERID IS MANUALLY INPUTTED, NOT GOOD?
        
        // PLAYER 1 INPUTS
        if (Input.GetKey("z"))
        {
            activeModel.movementAction(MovementDirection.Up, 0);
        }
        if (Input.GetKey("s"))
        {
            activeModel.movementAction(MovementDirection.Down, 0);
        }
        if (Input.GetKey("q"))
        {
            activeModel.movementAction(MovementDirection.Left, 0);
        }
        if (Input.GetKey("d"))
        {
            activeModel.movementAction(MovementDirection.Right, 0);
        }
        
        // PLAYER 2 INPUTS
        if (Input.GetKey("up"))
        {
            activeModel.movementAction(MovementDirection.Up, 1);
        }
        if (Input.GetKey("down"))
        {
            activeModel.movementAction(MovementDirection.Down, 1);
        }
        if (Input.GetKey("left"))
        {
            activeModel.movementAction(MovementDirection.Left, 1);
        }
        if (Input.GetKey("right"))
        {
            activeModel.movementAction(MovementDirection.Right, 1);
        }
    }
}
