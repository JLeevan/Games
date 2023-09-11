using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superball : MonoBehaviour
{
    [Header("Tripod")]
    GameObject tripod;
    AlienTripod at;
    public GameObject player, superBallDestination, agentPos;
    private bool canHitEnemy;

    [Header("GameObject")]
    Rigidbody rb;
    public Vector3 force;
    public GameObject fullCounter; //throw sound

    void Start()
    {
        superBallDestination = GameObject.Find("SuperballLoc");
        agentPos = GameObject.Find("AgentPos");
        canHitEnemy = false;
        player = GameObject.Find("Player");tripod = GameObject.Find("tripod");
        rb = GetComponent<Rigidbody>();at = tripod.GetComponent<AlienTripod>();
        force = (superBallDestination.transform.position - agentPos.transform.position) * 3 * Time.deltaTime;

        rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Enemy") && !canHitEnemy) || other.gameObject.CompareTag("Ceiling")) { return; }
        if (other.gameObject.CompareTag("FullCounter") || other.gameObject.CompareTag("CounterStrike")) 
        {
            canHitEnemy = true;
            GameObject counterSound = Instantiate(fullCounter, other.gameObject.transform.position, other.gameObject.transform.rotation) as GameObject;
            Destroy(counterSound, 2f);
            Destroy(gameObject, 6f);
            return; 
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerProperties>().ChangeHealth(-5f);
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Enemy")) { Destroy(tripod); }
        if (other.gameObject.CompareTag("Terrain")) Destroy(other.gameObject);
        Destroy(other.gameObject);
    }
}
