using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    [SerializeField] private float hSpeed  = 10f;
    [SerializeField] private float vSpeed = 6f;
    private Rigidbody2D rb2D;
    [SerializeField] private bool canMove;

    private bool facingRight = true;

    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move(float hMove, float vMove, bool jump)
    {
        if(canMove)
        {
            Vector3 targetVelocity = new Vector2(hMove * hSpeed, vMove * vSpeed);

            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmooth);

            //rotate character if we're facing the wrong way
            if(hMove > 0 && !facingRight)
            {
                flip();
            }
            else if (hMove< 0 && facingRight)
            {
                flip();
            }

        }
    }

    private void flip()
    {
        facingRight = !facingRight;
       transform.Rotate(0, 180, 0);
    }
    
    
}
