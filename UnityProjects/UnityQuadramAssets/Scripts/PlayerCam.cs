using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public Transform orientation;//an empty object used to tell the forward direction that the camera is facing
    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;// Locks the cursor in the middle of the screen
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;// takes the mouse x movement input as a float value
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;// takes the mouse y movement input as a float value

        // !!To remember: yRotation is the rotation around the y axis, which means the physical rotation is happening on the x axis
        //                and vice versa

        // !! Quarternion is a class that handles rotations using angles, Euler is a quartenion function

        yRotation += mouseX; //when the mouse moves left or right, the yRotation value is increased
        xRotation -= mouseY; //when the mouse moves up or down, the xRotation value is increased
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);// clamps the xRotation between looking directly up or down; prevents the cam from looking backwards
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);// links the rotation of the camera to the rotation values
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);// makes the orientation object turn left and right with the camera, to keep following the direction it's facing
    }
}
