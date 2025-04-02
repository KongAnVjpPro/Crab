using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDash : MyMonobehaviour
{
    bool used;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject canvasUI;
    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player") && (!used))
        {
            used = true;

            StartCoroutine(ShowUI());
            PlayerController.Instance.unlockedDash = true;
            // Destroy(gameObject);
        }
    }

    void Start()
    {
        if (PlayerController.Instance.unlockedDash)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator ShowUI()
    {
        GameObject _particles = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(_particles, 0.5f);
        yield return new WaitForSeconds(0.5f);

        canvasUI.SetActive(true);

        yield return new WaitForSeconds(4f);
        PlayerController.Instance.unlockedDash = true;
        SaveData.Instance.SavePlayerData();
        canvasUI.SetActive(false);
        Destroy(gameObject);
    }
}
