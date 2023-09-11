using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
    [Header("Player Properties")]
    public float health, stamina, maxStamina, maxHealth;
    public Slider greenWheel, redWheel, healthbar;

    [Header("Player")]
    Rigidbody rb;PlayerMovement pm;GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        greenWheel.gameObject.SetActive(false); redWheel.gameObject.SetActive(false);// stamina wheel is hidden by default
        stamina = maxStamina;maxHealth = 5; health = maxHealth;
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = health / maxHealth;//assigns the value of the healthbar to the value of the health as a fraction of it's full health
        if (health < 1) { Destroy(GameObject.Find("PlayerController")); }
        if (stamina < 1) pm.sprintSpeed = 1; else pm.sprintSpeed = 2;
        if(stamina > maxStamina) 
        { 
            greenWheel.gameObject.SetActive(false); // if stamina is at max, hide the stamina wheel
            redWheel.gameObject.SetActive(false); 
        }
    }
    public void ChangeHealth(float change)
    {
        health = health + change;
    }
    public void DecreaseStamina(float change, float multiplier)
    {
        greenWheel.gameObject.SetActive(true); redWheel.gameObject.SetActive(true);
        if (stamina < 0) { return; }
        stamina = stamina + change * multiplier * -1;
        greenWheel.value = stamina / maxStamina;
        redWheel.value = stamina / maxStamina + 0.05f;
    }
    public void IncreaseStamina(float change, float multiplier)
    {
        greenWheel.gameObject.SetActive(true); redWheel.gameObject.SetActive(true);
        if (stamina > maxStamina) { return; }
        stamina = stamina + change * multiplier;
        greenWheel.value = stamina / maxStamina;
        redWheel.value = stamina / maxStamina;
    }
    public IEnumerator Stagger(float propertyMulti, Transform direction)
    {
        rb.AddForce(direction.forward * propertyMulti, ForceMode.Impulse);
        yield return new WaitForSeconds(2);
    }
}
