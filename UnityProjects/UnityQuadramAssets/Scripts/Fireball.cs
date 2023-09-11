using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Tripod")]
    GameObject tripod;
    AlienTripod at;
    public GameObject player, cursor;

    [Header("GameObject")]
    Rigidbody rb;
    
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        tripod = GameObject.Find("tripod");
        at = tripod.GetComponent<AlienTripod>();

        rb.AddForce(at.GetShootRange(), ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(player.GetComponent<PlayerProperties>().Stagger(60f, gameObject.transform));
            player.GetComponent<PlayerProperties>().ChangeHealth(-1f);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Terrain")) Destroy(gameObject);
    }
}
