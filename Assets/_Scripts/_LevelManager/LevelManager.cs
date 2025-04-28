using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
public class LevelManager : MyMonobehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;
    public GameObject transitionContainer;
    [SerializeField] private New_SceneTransition[] transitions;
    // private SceneTransition
    public Slider progressBar;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSingleton();
    }
    protected virtual void LoadSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        transitions = transitionContainer.GetComponentsInChildren<New_SceneTransition>();
    }
    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        New_SceneTransition transition = transitions.First(t => t.name == transitionName);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        yield return transition.AnimateTransitionIn();

        // progressBar.gameObject.SetActive(true);

        do
        {
            // progressBar.value = scene.progress;
            yield return null;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        // progressBar.gameObject.SetActive(false);

        yield return transition.AnimateTransitionOut();
    }

}