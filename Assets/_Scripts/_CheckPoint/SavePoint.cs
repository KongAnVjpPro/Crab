using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class SavePoint : MyMonobehaviour
{
    public bool isTrigger;
    public Animator anim;
    bool inSaveProcess = false;
    public void ResetTrigger()
    {
        anim.ResetTrigger("In");
        anim.ResetTrigger("Out");
    }
    public void AnimationIn()
    {
        // ResetTrigger();
        anim.SetTrigger("In");
    }
    public void AnimationOut()
    {
        // ResetTrigger();
        anim.SetTrigger("Out");
        // Debug.Log("out");
    }
    //save

    //restore
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTrigger = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTrigger = false;
    }
    void EnterSavePoint()
    {
        if (!isTrigger) return;
        if (!PlayerEntity.Instance.playerInput.interact) return;
        // SaveData();
        if (inSaveProcess) return;
        GameController.Instance.isBlockPlayerControl = true;
        StartCoroutine(AnimationHandle());

    }
    void Update()
    {
        EnterSavePoint();
    }
    IEnumerator AnimationHandle()
    {
        // PlayerController.Instance.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
        // {
        inSaveProcess = true;
        AnimationIn();
        // });
        // AnimationIn();
        // UIManager.Instance.ActivateDeathScreen
        GameController.Instance.respawnPoint = transform.position;
        yield return UIEntity.Instance.uISaveScreen.EnterSaveScreen();
        Restore();
        SaveData();
        yield return UIEntity.Instance.uISaveScreen.ExitScreen();
        AnimationOut();
        GameController.Instance.isBlockPlayerControl = false;
        inSaveProcess = false;
        // ResetTrigger();
    }
    protected virtual void SaveData()
    {
        Debug.Log("Save here", gameObject);
        SaveSystem.Instance.shellSceneName = SceneManager.GetActiveScene().name;
        SaveSystem.Instance.shellStationPos = transform.position;
        // SaveSystem.Instance.SaveShellStation();
        // SaveSystem.Instance.SavePlayerData();
        // SaveSystem.Instance.SaveNPCAppear();
        // SaveSystem.Instance.SaveDoorData();
        SaveSystem.Instance.SaveGlobalData();
    }
    protected virtual void Restore()
    {
        // PlayerEntity.Instance.playerStat.ChangeCurrentStats(StatComponent.StatType.Health, 200);
        PlayerEntity.Instance.playerStat.RestoreStat();

        PlayerEntity.Instance.playerBlocking.isBlocking = false;

    }
}