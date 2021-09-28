using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    public static void Factory(GameObject block, int x, int z)
    {
        Instantiate(block, new Vector3(x, 1, z), Quaternion.identity);
    }
}
