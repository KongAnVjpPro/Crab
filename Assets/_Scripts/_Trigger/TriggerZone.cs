using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider2D))]
public class TriggerZone : MyMonobehaviour
{
    public bool oneShot = false;
    [SerializeField] private bool alreadyEntered = false;
    [SerializeField] private bool alreadyExited = false;

    public string collisionTag;
    // public LayerMask playerLayer;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public void ResetTrigger()
    {
        alreadyEntered = false;
        alreadyExited = false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyEntered)
        {
            return;
        }
        if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag))
        {
            return;
        }
        onTriggerEnter?.Invoke();
        if (oneShot)
        {
            alreadyEntered = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (alreadyExited)
        {
            return;
        }
        if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag))
        {
            return;
        }
        onTriggerExit?.Invoke();
        if (oneShot)
        {
            alreadyExited = true;
        }
    }
}