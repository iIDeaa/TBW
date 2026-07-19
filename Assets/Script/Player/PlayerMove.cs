using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public bool canMove = true;
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    [Header("Footstep")]
    [SerializeField] private float footstepDelay = 0.3f;
    private float footstepTimer;
    
    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (canMove)
            rb.velocity = moveInput * moveSpeed;
        else
            rb.velocity = Vector2.zero;

        Footstep();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>();
    }


    private void Footstep()
    {
        // ถ้าไม่ได้เดิน ไม่ต้องทำอะไร
        if (moveInput == Vector2.zero)
        {
            footstepTimer = 0;
            return;
        }


        footstepTimer -= Time.fixedDeltaTime;


        if (footstepTimer <= 0)
        {
            AudioManager.Instance.PlaySFX("Footstep");

            footstepTimer = footstepDelay;
        }
    }
    
}