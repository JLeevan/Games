using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Vector3 acceleration; //the new velocity is equal to the old velocity plus the acceleration
    private Vector3 gravityForce;
    private Vector3 velocity; //the new position is equal to the old position plus the velocity
    private Vector3 secondForce; //random force
    private float mass = 10f;
    // Start is called before the first frame update
    void Start()
    {
        gravityForce = new Vector3(0f, -1f, 0f);
        if (Random.Range(0, 10) < 5) // using a randomiser to decide which direction the fruit objects will spawn in
        {
            secondForce = new Vector3(-25f, 200f, 0f); //spawn left
        } 
        else
        {
            secondForce = new Vector3(25f, 200f, 0f); // or spawn right
        }
        useForce(secondForce);
    }

    // Update is called once per frame
    void Update()
    {
        useForce(gravityForce);
        changePos();
    }

    private void useForce(Vector3 force)
    {
        Vector3 accel = force / mass;
        acceleration += accel;
    }
    
    private void changePos()
    {
        velocity = velocity + acceleration;
        transform.position += velocity * Time.deltaTime;
        acceleration = new Vector3(0f, 0f, 0f);

    }
}
