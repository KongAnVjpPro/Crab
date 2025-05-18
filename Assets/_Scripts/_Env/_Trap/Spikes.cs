using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MyMonobehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            StartCoroutine(RespawnPoint());
        }
    }
    IEnumerator RespawnPoint()
    {   //thiếu kiểm tra nếu player chết
        PlayerController.Instance.PState.cutscene = true;
        PlayerController.Instance.PState.invincible = true;
        PlayerController.Instance.Rb.velocity = Vector2.zero;

        // Time.timeScale = 0;

        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        PlayerController.Instance.TakeDamage(1);
        yield return new WaitForSeconds(1);
        PlayerController.Instance.transform.position = GameManager.Instance.platformingRespawnPoint;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        yield return new WaitForSeconds(UIManager.Instance.sceneFader.fadeTime);
        PlayerController.Instance.PState.cutscene = false;
        PlayerController.Instance.PState.invincible = false;
        Time.timeScale = 1;
    }
}
