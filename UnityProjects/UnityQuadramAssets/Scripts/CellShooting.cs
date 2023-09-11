using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellShooting : MonoBehaviour
{
    public GameObject powercell; //link to the powerCell prefab
    public int no_rocks;
    public AudioClip throwSound; //throw sound
    public float throwSpeed;//throw speed
                                 // public bool shooted = false;
    GameObject player;
    public GameObject cell;
    public Transform shotTip;
    void Start()
    {
        throwSpeed = 100f;
        player = GameObject.Find("Player");
        shotTip = GameObject.Find("ShotTip").transform;
        //starts the game with 2 rocks
        no_rocks = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 instantPos = shotTip.position;
            AudioSource.PlayClipAtPoint(throwSound, shotTip.position);
            //instantaite the power cel as game object
            cell = Instantiate(powercell, instantPos, shotTip.rotation) as GameObject;
            cell.GetComponent<Rigidbody>().velocity = shotTip.forward * 100;
            Destroy(cell, 3.0f);
        }
    }
    private void FixedUpdate()
    {

    }

    public void addRock()
    {
        no_rocks++;
    }



}
