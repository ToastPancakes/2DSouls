using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float currentSpeed;
    public float forceDampening = 1.2f;
    public float rotationSpeed;
    public float stamina;
    public float maxStamina = 100;
    public float regenRate = 0.25f;
    public float regenCoolDown = 0;
    public float dashSpeed = 7f;
    public float dashCounter;
    public float dashCooldown;
    public float dashLength = 0.5f;
    public bool canDash = true;
    public bool onCoolDown = false;
    public bool inLockIn = false;
    public TrailRenderer tr;
    public Rigidbody2D playerRB;
    public BoxCollider2D box;
    public Vector2 moveForce;
    public Vector2 outsideForce;
    public GameObject lockTarget = null;
    Vector2 playerInput;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        playerRB.GetComponent<Rigidbody2D>();
        box.GetComponent<Collider2D>();
        stamina = maxStamina;
        tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        playerRotate();
        playerDash();
        SetLockIn();
    }

    private void FixedUpdate()
    {
        regenStamina();
        regenCoolDown -= Time.deltaTime;
    }

    void onCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("KB")) {
            outsideForce += new Vector2(-20, 0);
            Destroy(collision.gameObject);
        }
    }

    void playerMove() 
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        moveForce = playerInput * currentSpeed;
        moveForce += outsideForce;
        outsideForce /= forceDampening;
        if (Mathf.Abs(outsideForce.x) <= 0.01 && Mathf.Abs(outsideForce.y) <= 0.01)
        {
            outsideForce = new Vector2(0, 0);
        }
        playerRB.velocity = moveForce;
    }

    void playerRotate() 
    {
        if (playerInput != Vector2.zero && !inLockIn) {
            Quaternion target = Quaternion.LookRotation(Vector3.forward, playerInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
            playerRB.MoveRotation(rotation);
        }
        if (inLockIn && lockTarget != null) {

            Vector3 dir = (transform.position - lockTarget.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        }
    }

    void playerDash() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && stamina != 0.01f && canDash && stamina != 0) {
            currentSpeed = dashSpeed;
            dashCounter = dashLength;
            tr.emitting = true;
            canDash = false;
            if ((stamina - 10) < 0)
            {
                stamina = 0;
            }
            else {
                stamina -= 10;
            }
        }

        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0) {
                tr.emitting = false;
                currentSpeed = speed;
                canDash = true;
            }
        }

    }

    void regenStamina() {
        
        if (stamina < maxStamina && canDash && !onCoolDown && PlayerMelee.offBuffer) {

            stamina += regenRate * Time.deltaTime;
        }
        if (stamina > maxStamina) {
            stamina = maxStamina;
        }
        if(stamina == 0 )
        {
            onCoolDown = true;
            regenCoolDown = 3;
            stamina += 0.01f;
            
        }
        if (regenCoolDown <= 0) {
            onCoolDown = false;
        }
        
    }

    void SetLockIn() {
        if (Input.GetMouseButtonDown(1) && (ClickManager.rayHit.CompareTag("Lockable") || ClickManager.rayHit.CompareTag("Enemy"))) {
            Debug.Log("finding lock target");
            inLockIn = true;
            lockTarget.SetActive(true);
        }
        float distance = Vector2.Distance(transform.position, lockTarget.transform.position);
        if (distance > 7f /*|| other code here*/) {
            inLockIn = false;
            lockTarget.SetActive(false);
        }
        if (inLockIn) {
            Debug.Log("lock in running");
            lockTarget.transform.position = new Vector3 (ClickManager.rayHit.transform.position.x, ClickManager.rayHit.transform.position.y, -1);
        }
    }
}
