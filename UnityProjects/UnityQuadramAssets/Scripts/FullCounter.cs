using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCounter : MonoBehaviour
{
    public AudioClip soundEffect; //throw sound

    void Start()
    {
        Destroy(gameObject, 6f);//destroy the object after 5 seconds
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FullCounter")) { return; }
        if (other.gameObject.CompareTag("CounterStrike")) { return; }
        if (other.gameObject.CompareTag("Superball"))
        {
            GameObject superball = other.gameObject;
            superball.GetComponent<Rigidbody>().AddForce(-4 * superball.GetComponent<Superball>().force, ForceMode.Impulse);// pushes superball back in opposite direction it came from
            AudioSource.PlayClipAtPoint(soundEffect, other.gameObject.transform.position);// play clip when hits
        }
    }
}
