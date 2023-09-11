using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        health = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1) { Destroy(gameObject); }
    }
}
