using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDestruction : MonoBehaviour
{
    private float lifetime = 0.1f;
    void Start()
    {
        StartCoroutine(LifeTime());
    }

    // Update is called once per frame
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifetime);
        
        GameObject.Destroy(gameObject);
    }
    
    
}
