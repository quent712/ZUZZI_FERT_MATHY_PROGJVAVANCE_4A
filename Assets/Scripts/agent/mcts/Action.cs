
using System;
using System.Collections.Generic;
public class GameSimul{
/// <summary>
/// Le script a pour but de définir les différentes actions possible dans le jeu.
/// </summary>
    public static bool isFinished = false;

    public static float chargeAdv=0, chargeMe=0;

    public static int finalSituation = -1; // 0 = gameover; 1 = win

    public static int lifeAdv=100,lifeMe=100;

    public static int[] ppMe = new int[4], ppAdv = new int[4]; 

    public static void Reset(){
        lifeAdv = 100;
        lifeMe = 100;
        isFinished = false;
        finalSituation = -1;
        chargeAdv = 0;
        chargeMe = 0;
    }

    public static void PlayAction(Node action){
        
        /// Gestion du niveau chargement des pokémons
        
        //chargeMe += MCTS.FREQUENCY * pokemonMe.getStats().Vitess;
        //chargeAdv += MCTS.FREQUENCY * pokemonAdv.getStats().Vitess;

       
        if(chargeMe > 100) chargeMe = 0;
        if(chargeAdv > 100) chargeAdv = 0;

        if(chargeMe < 0) chargeMe = 0;
        if(chargeAdv < 0) chargeAdv = 0;

        // Gestion de la vie
        // Notre pokémon prend une capacité aléatoire de l'ennemi
        int lifeToLose = pokemonAdv.simulCapacity(-1, pokemonMe, ref chargeAdv, ref ppAdv);
        // Il va aussi simuler l'action choisi et voir les changements qu'ils impliquent
        switch(action.state){
            case PossibleAction.CAPACITY0:
                lifeAdv -= pokemonMe.simulCapacity(0,pokemonAdv, ref chargeMe, ref ppMe);
                break;
            case PossibleAction.CAPACITY1:
                lifeAdv -= pokemonMe.simulCapacity(1,pokemonAdv, ref chargeMe, ref ppMe);
                break;
            case PossibleAction.CAPACITY2:
                lifeAdv -= pokemonMe.simulCapacity(2,pokemonAdv, ref chargeMe, ref ppMe);
                break;
            case PossibleAction.CAPACITY3:
                lifeAdv -= pokemonMe.simulCapacity(3,pokemonAdv, ref chargeMe, ref ppMe);
                break;
            case PossibleAction.ESQUIVE:
                lifeToLose = (int)(UnityEngine.Random.Range(0,3) * lifeToLose / 3f);
                chargeMe -= pokemonMe.getStats().Vitess * MCTS.FREQUENCY;
                break;
            case PossibleAction.WAIT:
               // chargeMe += pokemonMe.getStats().Vitess * 0.5f;
                break;
        }
        //Condition de fin de partie
        lifeMe-=lifeToLose;
        if(lifeMe <= 0){
            finalSituation = 0;
            isFinished = true;
        }
        else if(lifeAdv<= 0){
            finalSituation = 1;
            isFinished = true;
        }
        
    }

    public static System.Array GetNextPossibleAction(Node n){ //?????
        return PossibleAction.GetValues(typeof(PossibleAction));
    }

    public static object GetRandomAction(System.Array actions){
        System.Random rand = new System.Random();
        int i = 0;
        if(i >= 1){
            return PossibleAction.WAIT;
        }else{
            return actions.GetValue(rand.Next(actions.Length-1));
        }
    }
}

public struct Register{
    //Feed forward
    public int a;
    public int b;

    //Life

    public Register(int a,int b){
        this.a = a;
        this.b = b;
    }
}
public enum PossibleAction{
    UNDETERMINED,
    WAIT,
    ESQUIVE,
    CAPACITY0,
    CAPACITY1,
    CAPACITY2,
    CAPACITY3
}