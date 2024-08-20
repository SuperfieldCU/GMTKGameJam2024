using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using FMOD.Studio;

//ensure gameobject has required component
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    Vector2 moveDir;

    [SerializeField] private float moveSpeed;

    private bool bCanMove = true;

    private bool bFacingRight = true;

    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private MeleeAttack meleeAttack;

    private bool isAttackQueued = false;
    private bool isAttacking = false;

    private Vector3 velocity = Vector3.zero;

    private bool isGrounded = true;
    private float jumpLoc;

    private EventInstance playerFootsteps;
    // Start is called before the first frame update

    private void Start()
    {
        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.playerFootsteps);
    }

    void Awake()
    {
        //setup rigidbody and sprite renderer
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        //enable movement controls and connect to the Move function
        controls = new PlayerControls();
        controls.Movement.Movement.performed += Move;
        controls.Movement.Movement.canceled += StopMoving;
        controls.Movement.Movement.Enable();
        controls.Movement.Jump.started += Jump;
        controls.Movement.Jump.Enable();
        controls.Combat.MeleeAttack.started += BeginAttack;
        controls.Combat.MeleeAttack.Enable();

        Health health = GetComponent<Health>();
        health.OnHealthChanged += TakeDamage;
    }

    void Move(CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<Vector2>();
        animator.SetBool("isMoving", true);
        PLAYBACK_STATE playbackState;
        playerFootsteps.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            playerFootsteps.start();
        }
    }

    void StopMoving(CallbackContext ctx)
    {
        moveDir = Vector2.zero;
        animator.SetBool("isMoving", false);
        playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);

    }

    void Jump(CallbackContext ctx)
    {
        if (isGrounded)
        {
            isGrounded = false;
            jumpLoc = transform.position.y;
            rb.AddForce(new Vector2(0, jumpForce));
            rb.gravityScale = 1;
            animator.SetBool("isGrounded", false);
        }
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
        if (!isGrounded)
        {
            if (transform.position.y < jumpLoc)
            {
                isGrounded = true;
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                animator.SetBool("isGrounded", true);
            }
        }
    }

    void BeginAttack(CallbackContext ctx)
    {
        if (isAttacking)
        {
            isAttackQueued = true;
        }
        else
        {
            isAttacking = true;
            meleeAttack.StartAttack();
        }
    }

    public void Attack()
    {
        meleeAttack.Attack(bFacingRight);
    }

    public void StopAttack()
    {
        isAttacking = false;
        meleeAttack.StopAttack();
        if (isAttackQueued)
        {
            isAttackQueued = false;
            isAttacking = true;
            meleeAttack.StartAttack();
        }
    }

    private void flip()
    {
        //flip sprite
       bFacingRight = !bFacingRight;
       spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void TakeDamage(int newHealth)
    {
        if (newHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        controls.Disable();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDie, this.transform.position);
    }
}
