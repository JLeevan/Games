using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTip : MonoBehaviour
{
    public GameObject alienTripod;
    public AudioClip soundEffect;
    void Start()
    {
        alienTripod = GameObject.Find("tripod");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Superball"))
        {
            GameObject superball = other.gameObject;
            Vector3 displacement = alienTripod.GetComponent<AlienTripod>().GetShootRange();
            superball.GetComponent<Rigidbody>().AddForce(-4 * superball.GetComponent<Superball>().force, ForceMode.Impulse);
            AudioSource.PlayClipAtPoint(soundEffect, other.gameObject.transform.position);
        }
    }
}
