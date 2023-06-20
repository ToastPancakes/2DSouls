using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public Rigidbody2D enemyRB;
    public BoxCollider2D enemyBox;
    public GameObject player;
    public GameObject sword;
    public Animator anim;
    public float minDistance = 2.75f;
    public float speed = 1.5f;
    public float lungeSpeed = 3f;
    public float noticeRange = 7f;
    public float offset;
    public float cooldown;
    public bool willMove;
    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemyBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (transform.position - player.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3( 0, 0, angle + offset));
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= noticeRange) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
            if (distance >= minDistance)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else if (distance < minDistance)
            {
                    Attack();
            }
        }
        cooldown -= Time.deltaTime;
    }

    void Attack() {
        if (cooldown <= 0) {
            if (Random.Range(0, 3) > 0)
            {
                anim.SetTrigger("fastAttack");
                cooldown = Random.Range(1.5f, 2f);
            }
            else
            {
                anim.SetTrigger("heavyAttack");
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, lungeSpeed * Time.deltaTime);
                cooldown = Random.Range(2f,  2.5f);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<PlayerStats>().TakeDamage();
        }
    }
}
