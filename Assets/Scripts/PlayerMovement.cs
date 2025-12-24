using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Animator animator;
    void Start()
    {
           animator= GetComponent<Animator>();
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

        bool IsMoving = moveInput.sqrMagnitude > 0.01;
        animator.SetBool("IsMoving",IsMoving);

        if (IsMoving){
            animator.SetFloat("x",moveInput.x);
            animator.SetFloat("y",moveInput.y);
        }
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    // Update is called once per frame
   
}
