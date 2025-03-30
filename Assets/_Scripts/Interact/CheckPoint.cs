using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MyMonobehaviour
{
    public bool interacted;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay2D(Collider2D _collision)
    {
        // Debug.Log("En");
        if (_collision.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            interacted = true;
            SaveData.Instance.checkPointName = SceneManager.GetActiveScene().name;
            SaveData.Instance.checkPointPosition = new Vector2(transform.position.x, transform.position.y);
            SaveData.Instance.SaveCheckPoint();
            SaveData.Instance.SavePlayerData();
        }
    }
    void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            interacted = false;
        }
    }
}
