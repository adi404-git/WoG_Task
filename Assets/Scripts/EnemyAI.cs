using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
   
    public float speed = 3f;
    public float checkRadius = 5f;
    public float attackRadius = 1f;
    public int health = 10; 
    
   
    public int damage = 1;
    public SpriteRenderer spriteRenderer;
    
    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        
       
      
        bool shouldMove = distance <= checkRadius && distance > attackRadius;
        
        if (shouldMove)
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            rb.MovePosition(temp);
            
            Vector3 dir = target.position - transform.position;
            dir.Normalize();
            movement = dir;
            
            if (anim != null)
            {
                anim.SetFloat("moveX", movement.x);
                anim.SetFloat("moveY", movement.y);
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
           
            if (anim != null) anim.SetBool("isMoving", false);
        }
    }
    
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCombat player = collision.gameObject.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(damage);
                
            }
        }
    }
    
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(spriteRenderer != null) StartCoroutine(FlashColor());
        
        if(health <= 0) Destroy(gameObject);
    }

    IEnumerator FlashColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}