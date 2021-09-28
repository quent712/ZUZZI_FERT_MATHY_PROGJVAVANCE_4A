using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain {

    static public GameObject playerPart, trainerPart;
    static public double born = 0;
    private Team adversaire, me;

    public Terrain(Trainer adversaire, PlayerTeam me)
    {
        this.adversaire = adversaire;
        this.me = me;
        callPokemon();
    }
    public void callPokemon()
    {
        Pokemon pokemonAdv = adversaire.getFirstAlivePokemon();
        Pokemon pokemonMe = me.getFirstAlivePokemon();
        
        if(pokemonAdv != null)
        {
            PokemonBattleRender.makeSprite(trainerPart, pokemonAdv, false);
        }
        if(pokemonMe != null)
        {
            PokemonBattleRender.makeSprite(playerPart, pokemonMe, true);
        }
    }
    public void update(float t)
    {
        adversaire.updateBattle(trainerPart, me);
        me.updateBattle(playerPart, adversaire);

        System.Random rand = new System.Random();
        
        Pokemon pokemonAdv = adversaire.getFirstAlivePokemon();
        Pokemon pokemonMe = me.getFirstAlivePokemon();

        int i = me.agent.interact(pokemonMe, pokemonAdv);

        i += adversaire.agent.interact(pokemonAdv, pokemonMe);
        if(i != 0) callPokemon();


        if(pokemonMe != null)
        {
            pokemonMe.charge(t);
            PokemonBattleRender.update(playerPart, pokemonMe, true);
        }
        if(pokemonAdv != null){
            pokemonAdv.charge(t);
        }
    }
}
