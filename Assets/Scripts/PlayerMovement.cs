using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

//ensure gameobject has required component
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    Vector2 moveDir;

    [SerializeField] private float moveSpeed;

    private bool bCanMove = true;

    private bool bFacingRight = true;

    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Awake()
    {
        //setup rigidbody and sprite renderer
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //enable movement controls and connect to the Move function
        controls = new PlayerControls();
        controls.Movement.Movement.performed += Move;
        controls.Movement.Movement.canceled += ctx => moveDir = Vector2.zero;
        controls.Movement.Movement.Enable();
    }

    void Move(CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player can move
        if (bCanMove)
        {
            //determine target velocity based on move vector
            Vector3 targetVelocity = new Vector3(moveDir.x * moveSpeed, moveDir.y * moveSpeed, 0);

            //smoothly speed up/slow down
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmooth);

            if (moveDir.x > 0 && !bFacingRight)
            {
                flip();
            }
            else if (moveDir.x < 0 && bFacingRight)
            {
                flip();
            }
        }
    }

    private void flip()
    {
        //flip sprite
       bFacingRight = !bFacingRight;
       spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
