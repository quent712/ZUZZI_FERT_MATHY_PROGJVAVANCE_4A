using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomer : Agent
{
    private double born;
    private System.Random random;

    double[] a;
    public Randomer(PokemonRender render) : base(render) {
        born = 0;
        random = new System.Random();
        a = new double[4];
    }

    public override int interact(Pokemon pokemonMe, Pokemon pokemonAdv){
        
        int k = 0;
        if(internalHorloge > born)
        {
            born = random.NextDouble() * 2.0d + 1d;
            internalHorloge = 0;
            int i = 0;
            if(pokemonMe != null
               && pokemonAdv != null)
            {
                i = pokemonMe.useCapacity(-1, pokemonAdv, render);
            }
            
            for(k = 0; k < 4; k++)
                a[k] = random.NextDouble();


            return i;
        }
        if(pokemonMe != null){
            if(a[0] >= 0.8f){
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
            }
        }
        return 0;
    }
}
