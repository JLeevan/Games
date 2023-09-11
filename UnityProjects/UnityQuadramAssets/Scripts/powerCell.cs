using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerCell : MonoBehaviour
{
    public GameObject playerCamera, player;
    GameObject cellShot;
    public GameObject explode;
    private GameObject tripod;
    private bool tripodHit;
    private bool appQuit = false;
    public GameObject cylinder, explosion;
    // Use this for initialization

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Camera");
        cylinder = GameObject.Find("Cylinder");
        tripodHit = false;
        Destroy(gameObject, 3.0f);
        player = GameObject.Find("Player");
        tripod = GameObject.Find("tripod");//find the tripod
    }
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            tripodHit = true;
            other.gameObject.GetComponent<AlienTripod>().Stagger();
            //instantiate the explosion
            Instantiate(explode, transform.position, transform.rotation);
            Destroy(gameObject);//destory self
        }
        else if (other.gameObject.CompareTag("Player")) { return; }
        else if (other.gameObject.CompareTag("Explosion")) { return; }
        else if (other.gameObject.CompareTag("Cell")) { }
        else if (other.gameObject.CompareTag("Cylinder")) 
        {
            other.gameObject.GetComponent<Cylinder>().health--;
            Destroy(gameObject); 
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Floor")) { return; }
        else
        {
            //instantiate the explosion
            Instantiate(explode, transform.position, transform.rotation);
            //reduce the tripod's health
            Destroy(gameObject);//destroy self
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }
    private void OnApplicationQuit()
    {
        appQuit = true;
    }

    void OnDestroy()
    {
        if (appQuit) return;
        if (!tripodHit) Instantiate(explode, transform.position, transform.rotation);
    }
}
