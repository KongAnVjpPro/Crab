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
    [SerializeField] private AnimationInOut[] transitions;
    // private SceneTransition
    // public Slider progressBar;
    public CanvasGroup blockCanvasGroup;

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
        transitions = transitionContainer.GetComponentsInChildren<AnimationInOut>();
    }
    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        blockCanvasGroup.blocksRaycasts = true;
        AnimationInOut transition = transitions.First(t => t.name == transitionName);
        // PlayerEntity.Instance.rb.gravityScale = 0f;
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        GameController.Instance.isBlockPlayerControl = true;
        // yield return UIEntity.Instance.uISaveScreen.EnterSaveScreen();
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
        GameController.Instance.isBlockPlayerControl = false;
        PlayerEntity.Instance.rb.gravityScale = 3f;
        blockCanvasGroup.blocksRaycasts = false;
        // yield return UIEntity.Instance.uISaveScreen.ExitScreen();
    }

}