using Cinemachine.Utility;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
public class PatrolState : EnemyState
{
    private float timer;
    public float patrolDuration = 3f;
    public float speed = 2f;
    [SerializeField] Vector3 patrolCenter;
    [SerializeField] Vector2 currentDir = new Vector2(1, 0);
    [SerializeField] float patrolRadius;
    [SerializeField] float obstacleCheckDistance = 0.5f;
    [SerializeField] float obstacleOffsetCheck = 0.5f;
    [SerializeField] private int moveDirection = 1;
    // [SerializeField] Vector3 right;

    [SerializeField] private bool returningToCenter = false;

    [Header("For swimming: ")]
    [SerializeField] float rangeRandomDirection = 45f;


    // public bool isSwimming = true;
    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        // stateMachine.HPBarFadeOut();
        float distanceToCenter = Vector3.Distance(transform.position, patrolCenter);
        returningToCenter = distanceToCenter > patrolRadius;
    }
    void Awake()
    {
        this.patrolCenter = transform.position;
    }
    public override void Do()
    {
        // Debug.DrawRay(stateMachine.transform.position, stateMachine.transform.right);

        timer += Time.deltaTime;
        if (timer >= patrolDuration)
        {
            isComplete = true;
            // return;
        }
        // stateMachine.RotateZ(new Vector2(moveDirection, 0));
        if (returningToCenter)
        {
            Vector2 dirToCenter = (patrolCenter - transform.position).normalized;

            if (!IsBlocked(dirToCenter))
            {
                MoveInDirection(dirToCenter);
            }
            else
            {
                Debug.Log("Block", gameObject);

            }

            if (Vector2.Distance(transform.position, patrolCenter) <= patrolRadius)
            {
                returningToCenter = false;
            }

            return;
        }

        Vector2 moveVec = new Vector2(moveDirection, 0);


        if (!stateMachine.isSwimming)
        {
            moveVec.y = stateMachine.rb.velocity.y;
            currentDir = moveVec;
        }
        else
        {
            moveVec = currentDir;
        }


        float distToCenter = Vector2.Distance(transform.position, patrolCenter);
        // float
        // right = moveDirection * stateMachine.transform.right;
        if (distToCenter > patrolRadius || IsBlocked(stateMachine.transform.right))
        {
            // Debug.Log("Obstacle");
            if (stateMachine.isSwimming)
            {
                RandomPatrolDirection();
                // Debug.Log("swim");
                // MoveInDirection(moveVec);
                return;
            }
            else
            {
                FlipDirection();
                moveVec = new Vector2(moveDirection, stateMachine.isSwimming ? 0 : stateMachine.rb.velocity.y);
            }

        }

        MoveInDirection(moveVec);

        // t(moveDirection * stateMachine.transform.right);
        // Debug.Log("Patrol");
    }
    void OnDrawGizmos()
    {

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);
    }

    private void FlipDirection()
    {
        // Debug.Log("Flip", gameObject);
        moveDirection *= -1;
    }
    private void RandomPatrolDirection()//for swimming
    {

        float minAngle = -rangeRandomDirection;
        float maxAngle = rangeRandomDirection;
        FlipDirection();

        float angle = Random.Range(minAngle, maxAngle);
        Vector2 baseDir = new Vector2(moveDirection, 0);
        Vector2 newDir = Quaternion.Euler(0, 0, angle) * baseDir;
        newDir.Normalize();
        // Debug.Log(newDir);
        currentDir = newDir;

        MoveInDirection(newDir);

    }

    void MoveInDirection(Vector2 dir)
    {
        // dir = new Vector2(dir.x, Random.Range(0.1f, 0.9f));
        stateMachine.rb.velocity = dir * speed;
        // stateMachine.RotateZ(dir);
        // stateMachine.Flip(dir.x >= 0 ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);

        if (stateMachine.isSwimming)
        {
            stateMachine.RotateZ(dir);
            stateMachine.Flip(dir.x >= 0 ? EnemyRotator.FlipDirection.Up : EnemyRotator.FlipDirection.Down);
        }
        else
        {
            stateMachine.Flip(dir.x >= 0 ? EnemyRotator.FlipDirection.Right : EnemyRotator.FlipDirection.Left);
        }

    }
    void t(Vector2 direction)
    {
        Vector2 origin = stateMachine.transform.position;

        Vector2[] rayOrigins = new Vector2[]
        {
            origin + (Vector2) stateMachine.transform.up * obstacleOffsetCheck,
            origin ,
            origin + (Vector2) stateMachine.transform.up * -1 * obstacleOffsetCheck
        };
        foreach (var t in rayOrigins)
        {
            Debug.DrawRay(t, direction.normalized);
        }
    }
    private bool IsBlocked(Vector2 direction)
    {
        // Debug.Log("block");
        if (!stateMachine.isSwimming)
        {
            direction *= moveDirection;
        }
        Vector2 origin = stateMachine.transform.position;

        Vector2[] rayOrigins = new Vector2[]
        {
            origin + (Vector2) stateMachine.transform.up * obstacleOffsetCheck,
            origin ,
            origin + (Vector2) stateMachine.transform.up * -1 * obstacleOffsetCheck
        };

        foreach (Vector2 rayOrigin in rayOrigins)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction.normalized, obstacleCheckDistance, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
                return true;
        }

        return false;
    }

    public override EnemyStateID? CheckNextState()
    {
        if (!PlayerEntity.Instance.pState.alive)
        {
            return EnemyStateID.Patrolling;
        }
        float dist = Vector2.Distance(transform.position, stateMachine.player.position);
        if (dist <= stateMachine.rangeAttackDistanceCheck && stateMachine.isRangeAttack)
        {
            return EnemyStateID.RangeAttack;
        }
        if (dist <= 2.5f)
        {
            // if (stateMachine.isRangeAttack) return EnemyStateID.RangeAttack;
            return EnemyStateID.Attacking;
        }
        if (dist <= 6f) return EnemyStateID.Chasing;
        return EnemyStateID.Patrolling;
    }
    public AIPath aiPath;
}
