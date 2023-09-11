using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    private PlayerMovement playerMove;
    private Rigidbody rigidPlayer;
    public Transform cam;
    public Transform gunTip;
    public LayerMask grappleable;
    public LineRenderer line;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    private Vector3 grapplePoint;
    private bool grappling;
    public float grappleForce;

    [Header("Cooldown")]
    public float grappleCooldown;
    private float grapplingCdTimer;

    [Header("KeyCodes")]
    public KeyCode grappleKey = KeyCode.Mouse1;


    // Start is called before the first frame update
    void Start()
    {
        playerMove = player.GetComponent<PlayerMovement>();
        rigidPlayer = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();//starts grappling when you right click
        }

        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        if (grappling)
        {
            line.SetPosition(0, gunTip.position);//the linerenderer will shoot the line FROM the position of grapple tip object
          //line.SetPosition(0, anything.at.all) means it sets where the line will START from
          //line.SetPosition(1, anything.at.all) means it sets where the line will END at
        }
    }
    public void StartGrapple()
    {
        if (grapplingCdTimer > 0)//if the cooldown timer is still in effect, grappling is not possible...prevents multiple grapples from happening at once
        {
            return;
        }
        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, grappleable))// shoots a raycast
        {
            rigidPlayer.velocity = Vector3.zero;//because you don't want the player to miss the target, so make the player not move will shooting the grapple hook
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
            line.enabled = true;
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
            line.enabled = false;
        }
        line.SetPosition(1, grapplePoint);
    }
    public void ExecuteGrapple()
    {
        Vector3 newVelocity = new Vector3(grapplePoint.x - rigidPlayer.transform.position.x, 
            grapplePoint.y - rigidPlayer.transform.position.y, grapplePoint.z - rigidPlayer.transform.position.z);
        if (grapplePoint.y < rigidPlayer.transform.position.y)// makes the player move quicker, direcly towards the grapple point...when moving downwards
        {
            rigidPlayer.velocity = 2 * newVelocity;
        }
        else if(newVelocity.y < 0)// when the y movement speed is negative, makes sure the player doesn't move upwards when supposed to go downwards and vice versa
        {
            rigidPlayer.velocity = new Vector3(1.5f * newVelocity.x, 0f, 1.5f * newVelocity.z);// 
        }
        else
        {
            rigidPlayer.velocity = new Vector3(1.5f * newVelocity.x, 3.3f * newVelocity.y, 1.5f * newVelocity.z);// normal grappling movement upwards
        }

        rigidPlayer.rotation = Quaternion.identity;//keeps the orientation of the player constant, prevents it from rotating
        Invoke(nameof(StopGrapple), 1f);
    }
    public void StopGrapple()
    {
        grappling = false;
        grapplingCdTimer = grappleCooldown;//starts the cooldown countdown when reaching the end of the grappling
        line.enabled = false;//turns the line off
    }
}
