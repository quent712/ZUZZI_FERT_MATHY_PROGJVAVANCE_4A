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
            activeModel.actionHandler(Action.MoveUp, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            activeModel.actionHandler(Action.MoveDown, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            activeModel.actionHandler(Action.MoveLeft, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            activeModel.actionHandler(Action.MoveRight, 0);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            activeModel.actionHandler(Action.SetBomb,0);
        }
        
        // PLAYER 2 INPUTS

        if (!randomIA)
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                activeModel.actionHandler(Action.MoveUp, 1);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                activeModel.actionHandler(Action.MoveDown, 1);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                activeModel.actionHandler(Action.MoveLeft, 1);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                activeModel.actionHandler(Action.MoveRight, 1);
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                activeModel.actionHandler(Action.SetBomb,1);
            }
        }

        else
        {
             Action action = (Action)Random.Range(0, 5);
             activeModel.actionHandler(action, 1);
        }
    }
}
