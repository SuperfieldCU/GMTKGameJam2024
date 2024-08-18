using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum AttackDirections { MeleeR, MeleeL, RangeR, RangeL };

    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private float stopDistance;

    [Header("Attack Positions")]
    [SerializeField]
    private AttackDirections[] attackDirections;

    [SerializeField]
    private GameObject[] attackPositions;

    private GameObject target;

    private float targetDistance;

    private SpriteRenderer spriteRenderer;


    private Dictionary<AttackDirections, GameObject> attackPoints = new Dictionary<AttackDirections, GameObject>();

    private AttackDirections attackDirection;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = FindObjectOfType<PlayerMovement>().gameObject;

        for (int i = 0; i < attackDirections.Length && i < attackPositions.Length; i++)
        {
            attackPoints.Add(attackDirections[i], attackPositions[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            targetDistance = Vector2.Distance(attackPoints[attackDirection].transform.position, target.transform.position);         
        }

        if (targetDistance < chaseDistance)
        {
            ChaseTarget();
        }
        else
        {
            StopChaseTarget();
        }
    }

    void ChaseTarget()
    {
        spriteRenderer.flipX = attackPoints[attackDirection].transform.position.x > target.transform.position.x;

        Vector3 newPointPos = Vector2.MoveTowards(attackPoints[attackDirection].transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position += newPointPos - attackPoints[attackDirection].transform.position;
    }

    void StopChaseTarget()
    {

    }
}
