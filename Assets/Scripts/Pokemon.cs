using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon {
    protected string name;
    protected Stats stats;
    protected Stats baseStats;
    protected Stats IV; // todo eventually
    protected float nature;
    protected int level;
    protected int life;

    public float charged;

    public Capacity[] capacities;
    public EfficientRule resistance;

    protected Sprite frontSprite, backSprite;

    public Pokemon(string spriteName, int level)
    {
        this.level = level;
        this.nature = 1.0f;
        this.charged = 100f;
        this.setIV();
        this.capacities = new Capacity[4];
        frontSprite = Resources.Load("graphics/pokemon/" + spriteName + "/front", typeof(Sprite)) as Sprite;
        backSprite = Resources.Load("graphics/pokemon/" + spriteName + "/back", typeof(Sprite)) as Sprite;
    }

    public string getName()
    {
        return name;
    }
    public void kill()
    {
        this.life = 0;
    }
    public int getLevel()
    {
        return level;
    }
    public int getPv()
    {
        return life;
    }
    public void setPv(int life)
    {
        if (life < 0) life = 0;
        this.life = life;
    }

    public Stats getStats()
    {
        return stats;
    }
    protected void setStats()
    {
        stats = new Stats();
        stats.Attack = (int)( ( (2 * baseStats.Attack + IV.Attack + level)/100 + 5 ) * nature );
        stats.Defense = (int)(((2 * baseStats.Defense + IV.Defense + level) / 100 + 5) * nature);
        stats.SpeAttack = (int)(((2 * baseStats.SpeAttack + IV.SpeAttack + level) / 100 + 5) * nature);
        stats.SpeDefense = (int)(((2 * baseStats.SpeDefense + IV.SpeDefense + level) / 100 + 5) * nature);
        stats.Vitess = (int)(((2 * baseStats.Vitess + IV.Vitess + level) / 100 + 5) * nature);
        stats.Pv = (int)((2 * baseStats.Vitess + IV.Vitess + level) / 100 + level + 10);

        stats.Attack = (int)(((IV.Attack + baseStats.Attack)*2*level)/100+5);
        stats.Defense = (int)(((IV.Defense + baseStats.Defense) * 2 * level) / 100 + 5);
        stats.SpeAttack = (int)(((IV.SpeAttack + baseStats.SpeAttack) * 2 * level) / 100 + 5);
        stats.SpeDefense = (int)(((IV.SpeDefense + baseStats.SpeDefense) * 2 * level) / 100 + 5);
        stats.Vitess = (int)(((IV.Vitess + baseStats.Vitess) * 2 * level) / 100 + 5);
        stats.Pv = (int)(((IV.Pv + baseStats.Pv) * 2 * level) / 100) + level+10;

        life = stats.Pv;
    }


    public Sprite getFrontSprite()
    {
        return frontSprite;
    }
    public Sprite getBackSprite()
    {
        return backSprite;
    }

    public int simulCapacity(int i, Pokemon target, ref float charge, ref int[] pp){
        if (i == -1)
            i = getRandomAttack();

        if (i != -1)
        {
            if (canUseAttack(i)){
                float chargeApplied = charge/100f;
                charge = 0;
                pp[i]--;
                return capacities[i].getScoreCapacity(this, target, chargeApplied);
            }
        }
        return 0;
    }

    public int useCapacity(int i, Pokemon target, PokemonRender render)
    {
        if (i == -1)
            i = getRandomAttack();

        if (i != -1)
        {
            if (canUseAttack(i)){
                if(capacities[i] is SpecialAttack){
                    ((SpecialAttack) capacities[i]).setRenderTarget(render);
                }
                capacities[i].use(this, target);
                this.charged = 0;
                return 1;
            }
        }
        return 0;
       
    }
    public int getRandomAttack()
    {
        int i = -1;
        if (canAttack())
        {
            while (i == -1)
            {
                i = Random.Range(0, 3);
                if (canUseAttack(i))
                {
                    return i;
                }
                i = -1;
            }
        }
        return i;
    }
    public bool canUseAttack(int i)
    {
        try
        {
            if (capacities[i] != null && capacities[i].getPP() > 0)
            {
                return true;
            }
            return false;
        }catch(System.IndexOutOfRangeException ex)
        {
            Debug.Log(capacities[i]);
        }
        return false;
    }
    public bool canAttack()
    {
        for(int i = 0; i < 4; i++)
        {
            if(canUseAttack(i))
            {
                return true;
            }
        }
        return false;
    }    
    protected void setIV()
    {
        IV = new Stats();
        IV.Attack = Random.Range(0, 15);
        IV.SpeAttack = Random.Range(0, 15);
        IV.Defense = Random.Range(0, 15);
        IV.SpeDefense = IV.SpeAttack;
        IV.Vitess = Random.Range(0, 15);
        IV.Pv = 8 * (IV.Attack % 2 == 0 ? 0 : 1) + 
            4*(IV.Defense  % 2 == 0 ? 0 : 1) +
            2*(IV.Vitess % 2 == 0 ? 0 : 1) +
            (IV.SpeAttack % 2 == 0 ? 0 : 1);
    }
    public void charge(float time){
        this.charged += (float)(time * this.stats.Vitess);
        if(this.charged >= 100f){
            this.charged -= 100f;
        }
        if(this.charged < 0){
            this.charged = 0;
        }
    }
}
