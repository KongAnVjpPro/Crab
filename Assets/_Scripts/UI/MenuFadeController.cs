using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFadeController : MyMonobehaviour
{
    private FadeUI fadeUI;
    [SerializeField] float fadeTime;
    override protected void LoadComponents()
    {
        base.LoadComponents();
        this.LoadFadeUI();
    }
    protected virtual void LoadFadeUI()
    {
        if (this.fadeUI != null) return;
        this.fadeUI = GetComponent<FadeUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeUI.FadeUIOut(fadeTime);
    }
    IEnumerator FadeAndStartGame(string _sceneToLoad)
    {
        fadeUI.FadeUIIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(_sceneToLoad);
    }
    public void CallFadeAndStartGame(string _sceneToLoad)
    {
        StartCoroutine(FadeAndStartGame(_sceneToLoad));
    }
    // Update is called once per frame
    void Update()
    {

    }
}
