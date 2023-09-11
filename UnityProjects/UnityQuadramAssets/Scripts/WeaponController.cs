using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Utilities")]
    public GameObject grappleGun, cellShot, counterStrike, player;

    [Header("KeyCodes")]
    public KeyCode counterKey;
    
    void Start()
    {
        //cellShot.SetActive(false); grappleGun.SetActive(false);
        counterKey = KeyCode.C;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(counterKey)) { pressCounter(); }
    }
    private void FixedUpdate()
    {
        
    }
    private void LateUpdate()
    {
        
    }
    public void collectGrappleGun()
    {
        if (grappleGun.activeInHierarchy) { grappleGun.SetActive(false); }
        else { grappleGun.SetActive(true); }
    }
    public void collectCellShot()
    {
        if (cellShot.activeInHierarchy) { cellShot.SetActive(false); }
        else { cellShot.SetActive(true); }
    }
    public void pressCounter()
    {
        if (counterStrike.activeInHierarchy) { counterStrike.SetActive(false); collectGrappleGun(); collectCellShot(); }
        else { counterStrike.SetActive(true); collectGrappleGun(); collectCellShot(); }
    }
}
