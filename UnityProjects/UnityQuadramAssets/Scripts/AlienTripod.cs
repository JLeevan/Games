using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AlienTripod : MonoBehaviour
{

    [Header("Properties")]
    float health, power, height;
    public Slider healthbar;

    [Header("Searching")]
    GameObject cursor;
    bool itsTime;

    [Header("Players")]
    NavMeshAgent agent;
    public Transform player, pcamera;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject counterStrike, agentPosition, superballDestination, platforms;
    public LineRenderer line;

    [Header("Patrolling")]
    private Vector3 displacement;

    [Header("Attacking")]
    private Vector3 shootRange;
    public GameObject fireball, ball, superball, super, cellShot, grappleGun;
    

    // Start is called before the first frame update
    void Start()
    {
        healthbar.value = 1;
        line = GetComponent<LineRenderer>();
        itsTime = false;
        height = transform.localScale.y; health = 1f;
        cursor = GameObject.Find("ForwardCursor");
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.transform.rotation = Quaternion.LookRotation(pcamera.position - healthbar.gameObject.transform.position);//ui stuff...not important
        Awake();AttackPlayer();

        if (itsTime)//this is the event that occurs when the counterStrike (the sword) is active in the game
        {// the agent is the tripod
            agent.transform.position = agentPosition.transform.position;//the player goes to a certain position in the sky
            agent.transform.rotation = Quaternion.LookRotation(new Vector3(superballDestination.transform.position.x, //Quarternion.LookRotation sets the object to look in the direction of the given vector
                0f, superballDestination.transform.position.z));// the y value is 0 so that the agent does not look down at the player, just faces the player                                                      
            FinalAttack();
        }
        else
        {
            Search();
        }
    }
    private void FixedUpdate()
    {

    }

    private void Awake()
    {
        player = GameObject.Find("Camera").transform;
        agent = GetComponent<NavMeshAgent>();// tripod uses a navmesh component to scan the terrain and follow the player
    }
    private void AttackPlayer()
    {
        if (GameObject.Find("FireBall(Clone)") != null) { return; }// will not shoot a fireball if there is already one in the game

        else if ((GameObject.Find("FireBall(Clone)") == null) && displacement.magnitude < 20f)// if the tripod is close to the player and no fireball is found...
        {
            ball = Instantiate(fireball, cursor.transform.position, cursor.transform.rotation) as GameObject;// ...then instantiate a fireball
        }
    }
    private void FinalAttack()// occurs when the counterstrike (the sword) is active in the hierarchy
    {
        if(platforms.transform.position.y > 120) { platforms.transform.Translate(0f, -10 * Time.deltaTime, 0f); }// move the sky platforms down until they reach a certain height
        
        //displacement = player.transform.position - agent.transform.position;
        if (GameObject.Find("SuperBall(Clone)") != null || player.position.y < 100) { return; }//copy script of the fireball

        else if (GameObject.Find("SuperBall(Clone)") == null)
        {
            super = Instantiate(superball, new Vector3(cursor.transform.position.x, cursor.transform.position.y, 
                cursor.transform.position.z + 20f), cursor.transform.rotation) as GameObject;
        }
    }
    public Vector3 GetShootRange()
    {
        return shootRange;
    }
    private void Search()
    {
        shootRange = player.transform.position - cursor.transform.position;
        displacement = player.transform.position - agent.transform.position;
        /*if (cellShot.activeInHierarchy)
        {
            agent.speed = 30;agent.acceleration = 30;
        }*/

        agent.transform.rotation = Quaternion.LookRotation(new Vector3(displacement.x, 0f, displacement.z));// makes the tripod look at the player without facing downwards
        if (counterStrike.activeInHierarchy)//                                                               if the tripod looks downwards, the whole thing will rotate, not just the head
        {
            itsTime = true;
        }
        agent.SetDestination(player.position);// a navigation function that tells the tripod to follow the player
    }
    public void Stagger()
    {
        GetComponent<Rigidbody>().AddForce(-cursor.transform.forward * 50f, ForceMode.Impulse);//pushes the tripod directly backwards by a force of 50
    }
}
