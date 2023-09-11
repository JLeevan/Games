using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaWheel : MonoBehaviour
{
    public float stamina, maxStamina;
    public Slider greenWheel, redWheel;
    public bool shouldDecrease;

    void Start()
    {
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldDecrease)
        {
            if (stamina > 0)
            {
                stamina -= 5 * Time.deltaTime;
            }
            redWheel.value = stamina / maxStamina + 0.05f;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += 10 * Time.deltaTime;
            }
            redWheel.value = stamina / maxStamina;
        }
        greenWheel.value = stamina / maxStamina;
    }
}
