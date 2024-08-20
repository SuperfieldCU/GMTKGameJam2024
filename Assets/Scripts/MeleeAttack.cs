using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D attackCollider;

    [SerializeField]
    private int damage;

    [SerializeField, Range(0, 1.0f)]
    private float stunChance;

    [SerializeField]
    private float knockbackForce;

    [SerializeField]
    private string animationString;

    [SerializeField]
    private Animator animator;

    bool isFacingRight = true;

    public virtual void StartAttack()
    {
        animator.SetBool(animationString, true);
    }

    public virtual void Attack(bool isAttackingRight)
    {
        if (isFacingRight != isAttackingRight)
        {
            isFacingRight = isAttackingRight;
            Vector3 prevLoc = transform.localPosition;
            transform.localPosition = new Vector3(-1 * prevLoc.x, prevLoc.y, prevLoc.z);
        }
        attackCollider.enabled = true;
    }

    public virtual void StopAttack()
    {
        attackCollider.enabled = false;
        animator.SetBool(animationString, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health)
        {
            health.TakeDamage(damage);
            float randVal = UnityEngine.Random.Range(0, 1.0f);
            if (randVal <= stunChance)
            {
                Vector2 dir = new Vector2(knockbackForce, 0);
                if (!isFacingRight)
                {
                    dir.x *= -1;
                }
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir);
            }
        }
    }
}
