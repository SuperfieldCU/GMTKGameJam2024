using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    public enum EAttackDirections { MeleeR, MeleeL, RangeR, RangeL, Null };

    public enum EActionMode { Wander, Pursue, Attack };

    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private float stopDistance;
    [SerializeField]
    private float wanderDistance = 3.0f;

    [Header("Attack Positions")]
    [SerializeField]
    private EAttackDirections[] attackDirections;

    [SerializeField]
    private GameObject[] attackPositions;

    [SerializeField]
    private MeleeAttack meleeAttack;

    [SerializeField]
    private RangedAttack rangeAttack;

    private GameObject target;

    private float targetDistance;

    private SpriteRenderer spriteRenderer;
    private Animator animator;


    private Dictionary<EAttackDirections, GameObject> attackPoints = new Dictionary<EAttackDirections, GameObject>();

    private EAttackDirections attackDirection = EAttackDirections.Null;
    private EActionMode actionMode = EActionMode.Wander;
    private Vector3 startPos;

    private Vector2 targetPos = Vector2.zero;

    private void Awake()
    {
        target = FindObjectOfType<PlayerMovement>().gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        for (int i = 0; i < attackDirections.Length && i < attackPositions.Length; i++)
        {
            attackPoints.Add(attackDirections[i], attackPositions[i]);
        }
    }

    private void OnEnable()
    {
        EnemyManager.Instance.AddEnemy(this);
    }

    // Start is called before the first frame update
    void Start()
    { 
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (actionMode == EActionMode.Pursue)
        {
            if (target)
            {
                targetDistance = Vector2.Distance(attackPoints[attackDirection].transform.position, target.transform.position);
            }

            if (targetDistance < chaseDistance && targetDistance > stopDistance)
            {
                ChaseTarget();
            }
            else if (targetDistance <= stopDistance)
            {
                StartAttack();
            }
            else
            {
                StopChaseTarget();
            }
        }
        else if (actionMode == EActionMode.Wander)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    void ChaseTarget()
    {
        spriteRenderer.flipX = attackPoints[attackDirection].transform.position.x > target.transform.position.x;

        Vector3 newPointPos = Vector2.MoveTowards(attackPoints[attackDirection].transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position += newPointPos - attackPoints[attackDirection].transform.position;
    }

    void StartAttack()
    {
        animator.SetBool("isMoving", false);
        actionMode = EActionMode.Attack;
        BeginAttack();
    }

    void BeginAttack()
    {
        meleeAttack.StartAttack();
    }

    public void Attack()
    {
        switch (attackDirection)
        {
            case EAttackDirections.MeleeL:
                meleeAttack.Attack(false);
                break;
            case EAttackDirections.MeleeR:
                meleeAttack.Attack(true);
                break;
            default:
                animator.SetBool("rangeAttack", true);
                break;
        }
    }

    void StopChaseTarget()
    {
        animator.SetBool("isMoving", false);
    }

    public void AssignPosition(EAttackDirections dir)
    {
        attackDirection = dir;
        if (attackDirection == EAttackDirections.Null)
        {
            animator.SetBool("isMoving", true);
            actionMode = EActionMode.Wander;
            float angle = UnityEngine.Random.Range(0, 360.0f);
            float distance = UnityEngine.Random.Range(0, wanderDistance);
            targetPos = transform.position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
        }
        else
        {
            animator.SetBool("isMoving", true);
            actionMode = EActionMode.Pursue;
        }
    }

    public bool IsRightOfTarget()
    {
        return transform.position.x >= target.transform.position.x;
    }

    public float GetRangedDistance()
    {
        EAttackDirections rangeDir = EAttackDirections.RangeR;
        if (IsRightOfTarget())
        {
            rangeDir = EAttackDirections.RangeL;
        }
        return Vector2.Distance(attackPoints[rangeDir].transform.position, target.transform.position);
    }

    public float GetMeleeDistance()
    {
        EAttackDirections meleeDir = EAttackDirections.MeleeR;
        if (IsRightOfTarget())
        {
            meleeDir = EAttackDirections.MeleeL;
        }
        return Vector2.Distance(attackPoints[meleeDir].transform.position, target.transform.position);
    }

    public float GetChaseDistance()
    {
        return chaseDistance;
    }

    public EAttackDirections GetAttackDirection()
    {
        return attackDirection;
    }

    public bool IsWithinRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) <= GetChaseDistance();
    }
}
