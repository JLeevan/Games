using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySpawn : MonoBehaviour
{
    public float spawnTime = 1; //Spawn Time
    public GameObject apple; //Apple prefab
    public GameObject bomb;
    // Start is called before the first frame update
    void Start()
    {
        //Start the spawn update
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Spawn()
    {
        //Wait spawnTime
        yield return new WaitForSeconds(spawnTime);
        //Spawn prefab is apple
        GameObject prefab = apple;
        //If random is over 30
        if (Random.Range(0, 100) < 30)
        {
            //Spawn prefab is bomb
            //you code here later in task 4
            prefab = bomb;
        }

        Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.Euler(0, 0, Random.Range(-90F, 90F)));

        StartCoroutine("Spawn");
    }
}