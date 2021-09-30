using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS1
{
    private Node tree;
    public static float FREQUENCY = 0.4f; // Fréquence des actions du MCTS
    private int[] a; // matrice des touches directionnelles
    private float born;
    private CharacterRender render;
    private Model model;
    private int playerID = 1;

    public MCTS1(Model model) 
    {
        tree = new Node(new Register(0, 0));
        born = 0.0f;
        a = new int[4];
        this.model = model;
    }

    public bool thrust()
    {
        foreach (Node n in tree.getPossibleAction())
        {
            if (n.data.b > 20)
            {
                // au moins un des noeuds doit être fiable (>20)
                return true;
            }
        }

        return false;
    }

    public int interact()
    {
        Player[] listplayer = model.getGameState()["PlayersInfo"] as Player[];

        if (listplayer.Length != 0)
        {
            // initialise les données du simulateur PROB HERE
            
            listplayer[1].health = 1;
            listplayer[0].health = 1;
            
           compute(tree); //compute(tree,pokemonMe, pokemonAdv);
        }


        // Appel horloge
        if (thrust())
        {
            born = FREQUENCY;

            float max = float.MinValue;
            Action currentAction = Action.Undertermined;
            Node n = null;
            bool priorityMove = false;
           

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

            
           
            //On résou la meilleur action conduisant à une victoire
            int i = 0;
            
            
                model.actionHandler(currentAction,1); //On lance l'action select

                // IMPORTANT ! On définie le nouveau noeud de base sur le noeud choisi
            if (n != null)
                tree = n;

            return i;
        }

        return 0;
    }




    void compute(Node action)
    {
        Debug.Log("In COMPUTE");
        Model simmodel = (Model)model.Clone();
        GameSimul.model = simmodel;
        //Debug.Log(action.data.a + "/" + action.data.b);
        //Tant que la simulation n'est pas achevée
        while (!GameSimul.isFinished)
        {
            
            System.Array actions = GameSimul.GetNextPossibleAction(action);

            // Choisi une action au piff
            Action choice = (Action) GameSimul.GetRandomAction(actions);

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

            // Lance la simulation 
            GameSimul.PlayAction(action);
            //Debug.Log(GameSimul.lifeAdv + " | " + GameSimul.lifeMe);
            //if(i++ > 10000) break;
        }

        // Applique des valeurs sur la feuille finale
        action.data.b = 1;
        if (GameSimul.finalSituation == 0) //gameover
            action.data.a = 0;
        else //win
            action.data.a = 1;

        // Retroprograpagation de l'action
        Node.Retropropagation(action);
        // Prépare le simulateur à une prochaine simulation
        GameSimul.Reset();

    }
}
