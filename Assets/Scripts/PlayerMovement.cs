using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f; 
    private Vector2 moveInput;
    private bool isSprinting; 
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    void Update()
    {
       
        float Speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        
        transform.position += (Vector3)moveInput * Speed * Time.deltaTime;

        bool IsMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", IsMoving);

        if (IsMoving)
        {
            animator.SetFloat("x", moveInput.x);
            animator.SetFloat("y", moveInput.y);
        }
    }
}