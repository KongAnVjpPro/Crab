using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWallJump : MyMonobehaviour
{
    bool used;
    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player") && (!used))
        {
            used = true;
            PlayerController.Instance.unlockedWallJump = true;
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (PlayerController.Instance.unlockedWallJump)
        {
            Destroy(gameObject);
        }
    }

}