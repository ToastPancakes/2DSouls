using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Animator anim;
    public float damage;
    public float meleeCoolDown;
    public float timer;
    public static bool offBuffer;
    public PlayerMovement staminaSource;
    public GameObject sword;

    private void Start()
    {
        PlayerMovement staminaSource = new PlayerMovement();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && staminaSource.stamina != 0 && staminaSource.stamina != 0.01f) {
            if (Input.GetMouseButtonDown(0)) {
                anim.SetTrigger("Attack");
                offBuffer = false;
                timer = meleeCoolDown;
                if ((staminaSource.stamina - 10) < 0)
                {
                    staminaSource.stamina = 0;
                }
                else
                {
                    staminaSource.stamina -= 15;
                }
            } 
        }
        if (timer <= (meleeCoolDown / 2)) {
            offBuffer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) {
            //col.GetComponent<EnemyStats>().TakeDamage();
            Debug.Log("Enemy Hit!");
        }
    }
}

