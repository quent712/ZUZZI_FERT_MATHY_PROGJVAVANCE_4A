
using System;
using System.Collections.Generic;
public class GameSimul{
/// <summary>
/// Le script a pour but de définir les différentes actions possible dans le jeu.
/// </summary>
    public static bool isFinished = false;

    public static int finalSituation = -1; // 0 = gameover; 1 = win
    
    public static int TouchAdv = -1; // 0 = pastouché; 1 = touché
    
    public static int TouchME = -1; // 0 = pastouché; 1 = touché   

    public static int[] ppMe = new int[4], ppAdv = new int[4]; 

    public static void Reset(){
        TouchAdv = 0;
        TouchME = 0;
        isFinished = false;
        finalSituation = -1;
      
    }

    public static void PlayAction(Node action){

        // Gestion de la vie
        
        // Il va simuler l'action choisi et voir les changements qu'ils impliquent
        switch(action.state){
            
            case PossibleAction.WALK:
                
                break;
            case PossibleAction.SETBOMBE:
                
                break;
            case PossibleAction.WAIT:
               // chargeMe += pokemonMe.getStats().Vitess * 0.5f;
                break;
        }
        //Condition de fin de partie
        
        if(TouchAdv == 0){
            finalSituation = 0;
            isFinished = true;
        }
        else if(TouchME == 0){
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
    WALK,
    SETBOMBE,
}