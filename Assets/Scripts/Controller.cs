using UnityEngine;

// Conceptually the Controller is the interface with which the players will interact
// It will transmit instructions to the model on what actions to take
public class Controller
{
    
    public Model activeModel;
    public MCTS1 mcts;
    
    public Controller(MCTS1 mcts)
    {
       
        this.mcts = mcts;
    }

    private void listenMultiplayer()
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

    private void listenRandomAI()
    {
        Action action = (Action)Random.Range(0, 5);
        activeModel.actionHandler(action, 1);
    }

    private void listenMCTSAI()
    {
        activeModel.actionHandler(mcts.interact(),1);
    }

    // Listens to Player action
    public void UpdateController(P2Input currentMode)
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
            Debug.Log("want bomb");
            activeModel.actionHandler(Action.SetBomb,0);
        }
        
        // PLAYER 2 INPUTS
        switch (currentMode)
        {
            case P2Input.Multi:
                listenMultiplayer();
                break;
            
            case P2Input.RandomAI:
                listenRandomAI();
                break;
            
            case P2Input.MCTSAI:
                listenMCTSAI();
                break;
        }
        
    }
}
