
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSimul{
/// <summary>
/// Le script a pour but de définir les différentes actions possible dans le jeu.
/// </summary>
    public static bool isFinished = false;

    public static int finalSituation = -1; // 0 = gameover; 1 = win
    
    public static int TouchAdv = -1; // 0 = pastouché; 1 = touché
    
    public static int TouchME = -1; // 0 = pastouché; 1 = touché   

    public static int[] ppMe = new int[4], ppAdv = new int[4];

    public static Model copymodel = null;

    public static void Reset(){
        TouchAdv = 0;
        TouchME = 0;
        isFinished = false;
        finalSituation = -1;
      
    }

    public static void PlayAction(Node action){
 
        // Il va simuler l'action choisi pour l'IA
       copymodel.actionHandler(action.state,1);
       
       //On va simuler une action random du player
       Action actiona = (Action)Random.Range(0, 5);
       
       copymodel.actionHandler( actiona,0); //Action aléatoire du player
       copymodel.inGameTimer += copymodel.inGameDeltaTime * 20; //Raccourcir temps de posage de bombe et de déplacements
       copymodel.UpdateModel(copymodel.inGameDeltaTime); //Pour calcul explos
       
        //On a une fin de partie ?
       Player[] listplayer = copymodel.getGameState()["PlayersInfo"] as Player[];
        if(listplayer[1].health <=0){   //Si Adversaire mort
            finalSituation = 0;
            isFinished = true;
        }
        else if(listplayer[0].health <=0){ //Si Player mort
            finalSituation = 1;
            isFinished = true;
        }
        
    }

    public static System.Array GetNextPossibleAction(Node n){ //?????
        return Action.GetValues(typeof(Action));
    }

    public static object GetRandomAction(System.Array actions){
        System.Random rand = new System.Random();
        int i = 0;
        if(i >= 1){
            return Action.Wait;
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
