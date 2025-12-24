using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; 

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    
 
    public float dashDistance = 3f; 
    public float dashDuration = 0.2f; 
    public LayerMask wallLayer; 
    private bool isDashing;
    private float dashCooldown = 1f;
    private float lastDashTime;

    private Vector2 moveInput;
    private bool isSprinting;
    private Vector2 lastMoveDirection; 

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if(moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed && Time.time > lastDashTime + dashCooldown && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        
        Vector2 dashDir = moveInput != Vector2.zero ? moveInput : lastMoveDirection;
        if(dashDir == Vector2.zero) dashDir = Vector2.down;

        Vector2 startPos = transform.position;
      
        RaycastHit2D hit = Physics2D.Raycast(startPos, dashDir, dashDistance, wallLayer);

        Vector2 targetPos;
        if (hit.collider != null)
        {
         
            targetPos = hit.point - (dashDir * 0.5f); 
        }
        else
        {
           
            targetPos = startPos + (dashDir * dashDistance);
        }

       
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
          
            rb.MovePosition(Vector2.Lerp(startPos, targetPos, elapsedTime / dashDuration));
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        isDashing = false;
    }

    void FixedUpdate()
    {
        
        if (isDashing) return;

        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        rb.linearVelocity = moveInput * currentSpeed;
    }
    void Update()
    {
        if (isDashing) return; 

            bool IsMoving = moveInput.sqrMagnitude > 0.01f;
            animator.SetBool("IsMoving", IsMoving);

        if (IsMoving)
        {
            animator.SetFloat("x", moveInput.x);
            animator.SetFloat("y", moveInput.y);
        }
    }
}