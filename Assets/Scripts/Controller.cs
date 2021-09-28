    using UnityEngine;

// Conceptually the Controller is the interface with which the players will interact
// It will transmit instructions to the model on what actions to take
public class Controller
{
    
    public Model activeModel;
    public Randomer randomer;
    
    
    public Controller(Randomer randomer)
    {
        this.randomer = randomer;
    }
    
    
    // Listens to Player action
    public void UpdateController(bool randomIA)
    {
        
        // FOR PLAYER VS PLAYER TAKE INTO ACCOUNT ALTERNATE CONTROL SCHEME
        // PLAYERID IS MANUALLY INPUTTED, NOT GOOD?
        
        // PLAYER 1 INPUTS
        if (Input.GetKey(KeyCode.Z))
        {
            activeModel.movementAction(MovementDirection.Up, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            activeModel.movementAction(MovementDirection.Down, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            activeModel.movementAction(MovementDirection.Left, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            activeModel.movementAction(MovementDirection.Right, 0);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            activeModel.dropBombAction(0);
        }
        
        // PLAYER 2 INPUTS

        if (!randomIA)
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                activeModel.movementAction(MovementDirection.Up, 1);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                activeModel.movementAction(MovementDirection.Down, 1);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                activeModel.movementAction(MovementDirection.Left, 1);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                activeModel.movementAction(MovementDirection.Right, 1);
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                activeModel.dropBombAction(1);
            }
        }

        else
        {
             activeModel.movementAction((MovementDirection)Random.Range(0, 4),1);
        }
    }
}
