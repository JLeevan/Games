using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterStrike : MonoBehaviour
{
    [Header("KeyCodes")]
    public KeyCode strikingKey;
    public KeyCode counterKey;

    [Header("Attacking")]
    public Animator anime;

    public GameObject fullCounter, counterFull; //link to the fullcounter prefab
    public AudioClip throwSound; //throw sound
    public float throwSpeed;
    public Transform counterTip;


    void Start()
    {
        anime = GetComponent<Animator>();throwSpeed = 50;//animator component
        strikingKey = KeyCode.Mouse0;counterKey = KeyCode.Mouse1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(strikingKey)) { StartCoroutine(FullStrike()); }

        else if (Input.GetKeyDown(counterKey)) StartCoroutine(FullCounter());
    }
    private IEnumerator FullCounter()//replacement for deltaTime..
    {
        anime.SetBool("counter", true);// plays the animation that runs on the condition of "counter"
        yield return new WaitForSeconds(0.5f);//doesn't continue the loop function until a certain number of seconds has passed
        anime.SetBool("counter", false);
        Vector3 instantPos = counterTip.position;
        counterFull = Instantiate(fullCounter, instantPos, counterTip.rotation) as GameObject;
        counterFull.GetComponent<Rigidbody>().velocity = counterTip.forward * throwSpeed;
    }
    private IEnumerator FullStrike()
    {
        anime.SetBool("attack", true);// plays animation that runs on the condition of "attack"
        yield return new WaitForSeconds(0.5f);
        anime.SetBool("attack", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
