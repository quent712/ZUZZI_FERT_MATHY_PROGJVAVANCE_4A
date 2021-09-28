using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRender : MonoBehaviour
{

    public float minX, maxX;
    public float minY, maxY;

    public Vector2 v;

    public Vector2 originalPosition;

    public float sensibility = 0.5f;

    public GameObject target;

    public GameObject adversaire;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = gameObject.transform.localPosition;
    }

    public IEnumerator warn(Pokemon p, int degat){
        System.Random rand = new System.Random();
        Vector2 dangerzone = new Vector2(
            (float) rand.NextDouble() * (2) - 1 + adversaire.transform.position.x,
            (float) rand.NextDouble() * (2) - 1 + adversaire.transform.position.y
        );

        GameObject targetInstance = Instantiate(target,dangerzone, Quaternion.identity);
        
        yield return new WaitForSeconds(1.0f); //Temps avant explosion ?
        //Debug.Log(Vector2.Distance(adversaire.transform.position, targetInstance.transform.position));
        if(Vector2.Distance(adversaire.transform.position, targetInstance.transform.position) < 2f){
            p.setPv(p.getPv() - degat);
            yield return PokemonBattleRender.recoveryTime(adversaire.transform.parent.gameObject);
        }
        Destroy(targetInstance);
    }

    // Update is called once per frame
    void Update()
    {
       gameObject.transform.localPosition = originalPosition + v;

    }
}
