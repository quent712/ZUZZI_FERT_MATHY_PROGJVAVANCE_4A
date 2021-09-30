using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MCTS1
{
    private Node tree;
    
    private float born;
    private CharacterRender render;
    private Model model;
    private int playerID = 1;

    public MCTS1(Model model) 
    {
        tree = new Node(new Register(0, 0));
        born = 0.0f;
   
        this.model = model;
    }

    public bool trust()
    {
        foreach (Node n in tree.getPossibleAction())
        {
            if (n.data.b > 150)
            {
                // au moins un des noeuds doit être fiable (>20)
                return true;
            }
        }

        return false;
    }

    public Action interact() //SELECT BEST ACTION IN THREE
    {
        for (int i =0;i<50;i++)
        {
            compute(tree); //compute(tree,pokemonMe, pokemonAdv);
        }
        


        // Appel horloge
        if (trust())
        {
            float max = float.MinValue;
            Action currentAction = Action.Undertermined;
            Node n = null;
            
            // Cherche la meilleure action conduisant à une victoire
            foreach (Node child in tree.getPossibleAction())
            {
                if (child.state != Action.Undertermined)
                {
                    if ((float) child.data.a / (float) child.data.b > max)
                    {
                        currentAction = child.state;
                        max = (float) child.data.a / (float) child.data.b;
                        n = child;
                    }
                }
            }
            Debug.Log("MAX" +"=" + max);
            if (n != null)
                tree = n;

            return currentAction; //On retourne l'action select

                // IMPORTANT ! On définie le nouveau noeud de base sur le noeud choisi
            
        }

        return Action.Undertermined;
    }




    void compute(Node action) //Simulation
    {
        
        Model simumodel = new Model(model);  //On copie le model actuel
        simumodel.inGameDeltaTime = 0.02f; //Les déplacements seront similaire à la réalité dans la simu
        GameSimul.copymodel = simumodel;
        
        
        //Tant que la simulation n'est pas achevée
        while (!GameSimul.isFinished)
        {
            System.Array actions = GameSimul.GetNextPossibleAction(action);

            // Choisi une action au piff
            Action choice = (Action) GameSimul.GetRandomAction(actions);
            //Action choice = (Action)Random.Range(0, 6);
            // Crée un node (donc une action) si elle n'existe pas encore
            // ou sinon prend celle trouvée
            Node exitanteNode = action.Exist(choice);
            if (exitanteNode == null)
            {
                Node selectedAction = action.AddChild(new Register(0, 0));
                selectedAction.parent = action;
                selectedAction.setState(choice);

                action = selectedAction; //La nouvelle action devient la current action
            }
            else
            {
                action = exitanteNode;   //la current action est l'action
            }

            // Lance l'action
            GameSimul.PlayAction(action);
           
            
        }

        // Applique des valeurs sur la feuille finale
        action.data.b = 1;
        if (GameSimul.finalSituation == 0) //gameover
        {
            action.data.a = 0;
           
        }
        else if (GameSimul.finalSituation == 1)//win
        {
           
            action.data.a = 1;
            
        }

        // Retroprograpagation de l'action
        Node.Retropropagation(action);
        // Prépare le simulateur à une prochaine simulation
        GameSimul.Reset();

    }
}
