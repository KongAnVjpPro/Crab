using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MyMonobehaviour

{
    // [SerializeField] GameObject platform;

    // public Transform point1, point2;
    public Material elvMat;
    public Material normalMat;
    public SpriteRenderer platform;
    public bool canMove = true;
    public List<Transform> path;
    public float moveSpeed = 2f;
    int nextId = 0;
    // public float offSetDistanceCheck = 0.05f;
    Vector3 nextPos;
    void Start()
    {
        // nextPos = point2.position;
        nextPos = path[1].position;
        nextId = 1;
    }
    public void ActivatePlatform(bool val)
    {
        canMove = val;
        platform.material = val ? elvMat : normalMat;
    }
    void Update()
    {
        if (!canMove)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
        if (transform.position == nextPos)
        {
            if (nextId >= path.Count - 1)
            {
                nextId = 0;
            }
            else
            {
                nextId++;
            }
            // nextPos = (nextPos == point1.position) ? point2.position : point1.position;
            nextPos = path[nextId].position;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.gameObject.name);
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        // collision.gameObject.transform.parent = platform.transform;
        PlayerEntity.Instance.gameObject.transform.parent = transform;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        // collision.gameObject.transform.parent = null;
        PlayerEntity.Instance.gameObject.transform.parent = null;
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log(collision.gameObject.name);
    //     if (!collision.gameObject.CompareTag("Player"))
    //     {
    //         return;
    //     }
    //     // collision.gameObject.transform.parent = platform.transform;
    //     PlayerEntity.Instance.gameObject.transform.parent = transform;
    // }
    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (!collision.gameObject.CompareTag("Player"))
    //     {
    //         return;
    //     }
    //     // collision.gameObject.transform.parent = null;
    //     PlayerEntity.Instance.gameObject.transform.parent = null;
    // }

}