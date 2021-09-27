using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent
{
    protected const float SENSIBILITY = 0.5f;
    protected const float COST_MOVE = -0.005f;

    protected PokemonRender render;

    protected float internalHorloge;

    public Agent(PokemonRender render){
        this.render = render;
        internalHorloge = 0.0f;
    }

    public void addTime(float time){
        internalHorloge += time;
    }

    public abstract int interact(Pokemon pokemonMe, Pokemon pokemonAdv);
}
