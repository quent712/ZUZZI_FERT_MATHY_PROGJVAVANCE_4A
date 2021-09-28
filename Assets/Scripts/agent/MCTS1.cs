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

    public MCTS1() : base()
    {
        tree = new Node(new Register(0, 0));
        born = 0.0f;
        a = new int[4];
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

    public int interact(Character characAdv, Pokemon characMe)
    {


        if (characAdv != null && characMe != null)
        {

            // initialise les données du simulateur
            GameSimul.TouchAdv = 0;
            GameSimul.TouchME = 0;

            compute(tree); //compute(tree,pokemonMe, pokemonAdv);
        }


        // Appel horloge
        if (thrust())
        {
            born = FREQUENCY;

            float max = float.MinValue;
            PossibleAction currentAction = PossibleAction.UNDETERMINED;
            Node n = null;
            bool priorityMove = false;
            // Si une bombe est proche de l'objet on gère une priorité
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("bomb"))
            {
                if (Vector2.Distance(o.transform.position, render.transform.position) < 2f)
                {
                    priorityMove = true;
                    ;
                }
            }

            // Cherche la meilleure action conduisant à une victoire
            foreach (Node child in tree.getPossibleAction())
            {
                if (child.state != PossibleAction.UNDETERMINED)
                {
                    if ((float) child.data.a / (float) child.data.b > max)
                    {
                        currentAction = child.state;
                        max = (float) child.data.a / (float) child.data.b;
                        n = child;
                    }
                }
            }

            // Si priorité de mouvement ou action ESQUIVE
            // On se déplace
            a = new int[4];
            if (priorityMove || currentAction == PossibleAction.WALK)
            {
                if (render.v.x >= 0)
                    a[2] = 1;
                else
                    a[3] = 1;

                if (render.v.y >= 0)
                    a[1] = 1;
                else
                    a[0] = 1;
            }


            int i = 0;
            if (characMe != null
                && characAdv != null)
            {
                switch (currentAction)
                {
                    case PossibleAction.UNDETERMINED:
                    case PossibleAction.WAIT:
                        break;

                    // Utilise les capacités si le MCTS le demande
                    case PossibleAction.SETBOMBE:
                        // i = pokemonMe.useCapacity(0, pokemonAdv, render);
                        break;

                }
            }

            // IMPORTANT ! On définie le nouveau noeud de base sur le noeud choisi
            if (n != null)
                tree = n;

            return i;
        }

        // Applique le mouvement
        if (characMe != null)
        {
            /* if(a[0] >= 0.8f){
                 render.v.y += Time.deltaTime * SENSIBILITY;
                 pokemonMe.charge(COST_MOVE);
             }
             if(a[1] >= 0.8f){
                 render.v.y -= Time.deltaTime * SENSIBILITY;
                 pokemonMe.charge(COST_MOVE);
             }
             if(a[2] >= 0.8f){
                 render.v.x -= Time.deltaTime * SENSIBILITY;
                 pokemonMe.charge(COST_MOVE);
             }
             if(a[3] >= 0.8f){
                 render.v.x += Time.deltaTime * SENSIBILITY;
                 pokemonMe.charge(COST_MOVE);
             }   */
        }

        return 0;
    }




    void compute(Node action)
    {
        //Debug.Log(action.data.a + "/" + action.data.b);
        //Tant que la simulation n'est pas achevée
        while (!GameSimul.isFinished)
        {
            System.Array actions = GameSimul.GetNextPossibleAction(action);

            // Choisi une action au piff
            PossibleAction choice = (PossibleAction) GameSimul.GetRandomAction(actions);

            // Crée un node (donc une action) si elle n'existe pas encore
            // ou sinon prend celle trouvée
            Node exitanteNode = action.Exist(choice);
            if (exitanteNode == null)
            {
                Node selectedAction = action.AddChild(new Register(0, 0));
                selectedAction.parent = action;
                selectedAction.setState(choice);

                action = selectedAction;
            }
            else
            {
                action = exitanteNode;
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
