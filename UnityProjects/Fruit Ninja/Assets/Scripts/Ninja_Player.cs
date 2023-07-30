using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja_Player : MonoBehaviour
{
    public int score = 0; //Score
    private Vector3 pos; //Position
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;//Set screen orientation to landscape
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//Set sleep timeout to never
    }

    // Update is called once per frame
    void Update()
    {
        //If the game is running on an iPhone device
        if (Application.platform == RuntimePlatform.Android)
        {
            //If we are hitting the screen
            if (Input.touchCount == 1)
            {
                //Find screen touch position, by
                //transforming position from screen space into game world space.
                pos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 3);
                //Set position of the player object
                transform.position = Camera.main.ScreenToWorldPoint(pos);
                //Set collider to true
                GetComponent<Collider2D>().enabled = true;
                return;
            }
            //Set collider to false
            GetComponent<Collider2D>().enabled = false;
        }

        else //If the game is not running on an Android device
        {
            //Find mouse position
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9));//Player 'z' position == ScreenToWorldPoint position + position of 'Main Camera' element
            transform.position = pos;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fruit")
        {
            //write your code here
            other.GetComponent<Fruit2D>().Hit();    
            score++;
        } else if (other.tag == "Bomb")
        {
            other.GetComponent<Fruit2D>().Hit();
            score=score-2;
        }
    }

}
