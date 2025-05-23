using System.Collections;
using UnityEngine;
using DG.Tweening;
public class SavePoint : MyMonobehaviour
{
    public bool isTrigger;
    public Animator anim;

    public void ResetTrigger()
    {
        anim.ResetTrigger("In");
        anim.ResetTrigger("Out");
    }
    public void AnimationIn()
    {
        ResetTrigger();
        anim.SetTrigger("In");
    }
    public void AnimationOut()
    {
        ResetTrigger();
        anim.SetTrigger("Out");
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
        AnimationIn();
        // });
        // AnimationIn();
        // UIManager.Instance.ActivateDeathScreen
        yield return UIEntity.Instance.uISaveScreen.EnterSaveScreen();
        Restore();
        SaveData();
        yield return UIEntity.Instance.uISaveScreen.ExitScreen();
        AnimationOut();
        GameController.Instance.isBlockPlayerControl = false;
    }
    protected virtual void SaveData()
    {
        Debug.Log("Save here", gameObject);
    }
    protected virtual void Restore()
    {
        PlayerEntity.Instance.playerStat.ChangeCurrentStats(StatComponent.StatType.Health, 200);
        PlayerEntity.Instance.playerBlocking.isBlocking = false;
    }
}