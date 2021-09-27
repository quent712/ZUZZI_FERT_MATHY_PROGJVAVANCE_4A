using UnityEngine;


public class Controller
{

    public Model activeModel;

    public void UpdateController()
    {
        
        // FOR PLAYER VS PLAYER TAKE INTO ACCOUNT ALTERNATE CONTROL SCHEME
        if (Input.GetButtonDown("Z"))
        {
            activeModel.movementAction(MovementDirection.Up, 0);
        }
        else if (Input.GetButtonDown("S"))
        {
            activeModel.movementAction(MovementDirection.Down, 0);
        }
        else if (Input.GetButtonDown("Q"))
        {
            activeModel.movementAction(MovementDirection.Left, 0);
        }
        else if (Input.GetButtonDown("D"))
        {
            activeModel.movementAction(MovementDirection.Right, 0);
        }
    }
}
