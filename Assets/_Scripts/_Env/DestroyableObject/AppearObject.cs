using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
public class AppearObject : MyMonobehaviour
{
    [SerializeField] GameObject startPos;
    [SerializeField] GameObject endPos;
    // [SerializeField] DamagedAbleObject obj;
    // [SerializeField] float hittedCD = 1f;
    // public float hitTimer = 0.5f;
    // public bool canHit = false;

    void OnEnable()
    {
        transform.position = startPos.transform.position;
        transform.DOMove(endPos.transform.position, 1f);
    }
    void OnDisable()
    {
        transform.position = startPos.transform.position;
    }

    // void Update()
    // {
    //     if (!canHit) return;
    //     hitTimer += Time.deltaTime;
    //     if (hitTimer < hittedCD)
    //     {
    //         return;
    //     }
    //     hitTimer = 0;
    //     obj.HitObject();
    // }
}