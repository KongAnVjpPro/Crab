using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MyMonobehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string transitionTo;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Vector2 exitDirection;
    [SerializeField] private float exitTime;



    void OnTriggerEnter2D(Collider2D _other)//transition to new level
    {
        if (_other.CompareTag("Player"))
        {

            GameManager.Instance.transitionedFromScene = SceneManager.GetActiveScene().name;//store prev level
            PlayerController.Instance.PState.cutscene = true;
            PlayerController.Instance.PState.invincible = true;
            StartCoroutine(UIManager.Instance.sceneFader.FadeAndLoadScene(SceneFader.FadeDirection.In, transitionTo));//come to new level and play fade anim
        }

    }
    void Start()
    {
        if (transitionTo == GameManager.Instance.transitionedFromScene)//if the prev scene == where this port to
        {
            PlayerController.Instance.transform.position = startPoint.position;
            StartCoroutine(PlayerController.Instance.WalkIntoNewScene(exitDirection, exitTime));//flip or add force
        }
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));

    }
}
