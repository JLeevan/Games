using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AppleRotate : MonoBehaviour
{
    Vector3 tF;
    Vector3 fD;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        tF = this.transform.up; // facing up at Y axis
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        fD = player.transform.position - this.transform.position; //get the vector from spaceship to player
        float angle = Vector2.Angle(tF, fD);//use Vector2 to avoid Z in the calculation

        Debug.DrawRay(this.transform.position, tF * 2, Color.green);
        Debug.DrawRay(this.transform.position, fD, Color.red);

        Vector3 crossP = Vector3.Cross(tF, fD);
        int clockwise = 1;
        if (crossP.z < 0)
            clockwise = -1;
        this.transform.rotation = Quaternion.Euler(0, 0, angle * clockwise);
        this.transform.Translate(this.transform.up * Time.deltaTime);
    }
}
