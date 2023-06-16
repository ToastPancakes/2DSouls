using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement staminaSource;
    public Image staminaBar;
    public Text coolDownText;
    // Start is called before the first frame update
    void Start()
    {
        staminaSource = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.fillAmount = staminaSource.stamina / staminaSource.maxStamina;
        if (staminaSource.onCoolDown)
        {
            coolDownText.text = "Stamina On Cooldown";
        }
        else {
            coolDownText.text = "";
        }
    }
}
