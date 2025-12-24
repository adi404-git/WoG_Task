using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public int health = 5;
    public SpriteRenderer spriteRenderer;
    
    public float attackRange = 2f;
    public int attackDamage = 3;
    public float cooldownTime = 2f;
    private float nextAttackTime = 0f;
    
    public LayerMask enemyLayer;

    public void OnJump(InputValue value) 
    {
        if(value.isPressed && Time.time >= nextAttackTime)
        {
            SpecialAttack();
            nextAttackTime = Time.time + cooldownTime;
        }
    }

    void SpecialAttack()
    {
   
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
            if(enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
        Debug.Log("Special Attack Fired");
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(spriteRenderer != null) StartCoroutine(FlashColor());
        if(health <= 0) Debug.Log("Player Died");
    }

    IEnumerator FlashColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}