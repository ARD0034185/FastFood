using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject[] objectsToInstantiate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int random =  Random.Range(0, objectsToInstantiate.Length);
        int randomSpawn =  Random.Range(0, 11);

        
            Instantiate(objectsToInstantiate[random], transform.position, objectsToInstantiate[random].transform.rotation);
        


    }
}
