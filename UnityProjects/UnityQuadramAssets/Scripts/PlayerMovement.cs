using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float airSpeed, moveDrag, airDrag, iceDrag, groundSpeed, sprintSpeed, 
        crouchYScale, crouchSpeed, startYScale, speedMultiplier;
    public bool canSprint;

    [Header("Ground Check")]
    public float playerHeight, groundDrag;
    public LayerMask theGround, iceFloor;
    bool grounded;

    [Header("Jumping")]
    public float jumpForce, jumpCooldown, airMulti;
    //public float sprintJump;
    //public float groundJump;
    bool readyToJump = true;

    [Header("Slope Standing")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("protected variables")]
    public Transform orientation, floorCursor;
    float horiInput, vertInput;
    Vector3 moveDirection;
    public Rigidbody rb;
    public MovementState state;
    private bool frozen;
    public bool onIceFloor;
    private bool grappleEngaged;
    private Vector3 velocityToSet;
    public PlayerProperties playerProps;
    public GameObject pcamera; public WeaponController control;


    public enum MovementState //enumerators are classes that implement variables as a bunch of sets...their indexing is the same arrays , going down in the list (wallking=index 0)
    {
        walking,
        sprintFloating,
        sprintWalking,
        crawling,
        floating,
        frozen,
        grappling,
        penguinSliding,
        sprintSliding
    }
    void Start()
    {
        state = MovementState.walking;
        airDrag = 1;iceDrag = 2;
        control = pcamera.GetComponent<WeaponController>();
        playerProps = GetComponent<PlayerProperties>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        crouchYScale = startYScale / 2;
        speedMultiplier = 2f;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (rb.transform.position.y < -5)
        {
            rb.transform.position = new Vector3(rb.transform.position.x, 50f, rb.transform.position.z);
        }
        MyInput();
        StateHandler();
        MoveHandler();

        //the raycast is half the player's height + a bit more to hit deep enough into the terrain
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, iceFloor))// raycast to hit the icefloor
        { 
            grounded = false; onIceFloor = true; 
        } 
        else if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, theGround))// raycast to hit anything masked with "theGround"
        { 
            onIceFloor = false; grounded = true; 
        }
        else 
        { 
            onIceFloor = false; grounded = false; 
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();// because the movement can be done whilst holding buttons, the MovePlayer function can be put here
                     // so that these inputs are processed at a certain rate
    }
    private void MyInput()
    {
        horiInput = Input.GetAxisRaw("Horizontal");//left and right input buttons
        vertInput = Input.GetAxisRaw("Vertical");// up and down input buttons...these are used in the MovePlayer function

        if (Input.GetKey(jumpKey) && readyToJump && (grounded || onIceFloor))
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);// basically, as long as the key is pressed the player will keep jumping
                                                    // the function will be invoked every 'jumpCooldown' amount of time
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);// changes the y length of the player
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
    }
    private void StateHandler()// I feel like this is easy to understand; to make it easier, start reading from the bottom
    {
        if (onIceFloor)
        {
            state = MovementState.penguinSliding;
        }
        else if (Input.GetKey(sprintKey) && !grounded)
        {
            state = MovementState.sprintFloating;
        }
        else if (Input.GetKey(sprintKey) && grounded)
        {
            state = MovementState.sprintWalking;
        }
        else if (grounded)
        {
            state = MovementState.walking;
        }
        else if (!grounded)
        {
            state = MovementState.floating;
        }
    }

    private void MoveHandler()
        //PlayerProps is the PlayerProperties script
        //the moveDrag variable changes based on these movement scenarios, and the rigidbody's drag is set in MovePlayer
        //Time.deltaTime is the amount of time that has passed between frames...basically converts framerate to time
    {
        if (Input.GetKey(sprintKey) && onIceFloor)
        {
            state = MovementState.sprintSliding;
            moveDrag = iceDrag;
            moveSpeed = 10;
            playerProps.DecreaseStamina(5f, Time.deltaTime);
        }
        else if (state == MovementState.penguinSliding)
        {
            moveDrag = iceDrag;
            moveSpeed = groundSpeed;
            playerProps.IncreaseStamina(10f, Time.deltaTime);
        }
        else if (state == MovementState.sprintFloating)
        {
            moveSpeed = sprintSpeed;moveDrag = airDrag;
            playerProps.DecreaseStamina(5f, Time.deltaTime);
        }
        else if (state==MovementState.sprintWalking)
        {
            moveSpeed = sprintSpeed;
            moveDrag = groundDrag;
            playerProps.DecreaseStamina(5f, Time.deltaTime);
        }
        else if (state==MovementState.walking)
        {
            moveDrag = groundDrag;
            moveSpeed = groundSpeed;
            playerProps.IncreaseStamina(10f, Time.deltaTime);
        }
        else if (state==MovementState.floating)
        {
            moveDrag = airDrag;
            moveSpeed = airSpeed;
            playerProps.IncreaseStamina(10f, Time.deltaTime);
        }
        /*else if (onSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 2f, ForceMode.Impulse);
        }*/
    }

    
    private void MovePlayer()
    {
        rb.drag = moveDrag;
        moveDirection = orientation.forward * vertInput + orientation.right * horiInput;
        if (Input.GetKey(sprintKey) && !grounded)
        {
            rb.AddForce(-moveDirection.normalized * moveSpeed * 1.7f, ForceMode.Impulse);// when sprinting, apply a force in the opposite direction, so the player doesn't ZOOM
        }

        rb.AddForce(moveDirection.normalized * moveSpeed * 2f, ForceMode.Impulse);
    }
    
    private void Jump()
    {
        if (state == MovementState.sprintWalking)
        {
            rb.velocity = new Vector3(rb.velocity.x/2, 10f, rb.velocity.z/2);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }
        rb.velocity = new Vector3(rb.velocity.x, 10f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    /*
    private bool onSlope()
    {
        if(Physics.Raycast(floorCursor.position, Vector3.forward, out slopeHit, 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }*/

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;// Vector3.ProjectOnPlane returns the location where the vector hits the plane
    }

    public void Frozen(bool value)
    {
        frozen = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ceiling")) 
        { 
            rb.velocity = new Vector3(0f, -10f, 0f); 
        }

        if (other.gameObject.CompareTag("CounterCrate")) 
        { 
            control.pressCounter(); Destroy(other.gameObject, 0.5f); 
        }

        if (other.gameObject.CompareTag("CellShot")) 
        { 
            control.collectCellShot(); Destroy(other.gameObject, 0.5f); 
        }

        if (other.gameObject.CompareTag("GrappleGun")) 
        { 
            control.collectGrappleGun(); Destroy(other.gameObject, 0.5f); 
        }

        if (other.gameObject.CompareTag("Lava")) 
        { 
            playerProps.ChangeHealth(-2f * Time.deltaTime); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("VelocityWall")) //there are walls in the ice part to prevent the the player from smashing through the terrain
        { 
            rb.velocity = new Vector3(-10 * moveDirection.x, 0f, -10 * moveDirection.y); 
        }
    }
}