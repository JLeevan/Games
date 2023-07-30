using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private bool canJump = false;
    public float jumpForce = 2.8f;
    public Vector3 jump;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal*6 , 0.0f, speed/5);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Grounded")
        {
            canJump = true;
        }
    }
    void Update()
    {
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        if (Input.GetKeyDown(KeyCode.Space)&&canJump)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
        
    }

}