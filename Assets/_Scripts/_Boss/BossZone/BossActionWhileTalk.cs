using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
public class BossActionWhileTalk : NPCAction//can be use for action while talk and trigger zone so on
{
    [SerializeField] DialogueTrigger dialogue;
    [SerializeField] BossController boss;
    [SerializeField] List<BossLock> locks;
    protected override void Awake()
    {
        base.Awake();
        boss = FindAnyObjectByType<BossController>();
        // dialogue = FindAnyObjectByType<BossController>().GetComponent<DialogueTrigger>();
        dialogue = boss.GetComponent<DialogueTrigger>();
    }
    void Start()
    {
        if (boss != null)
        {
            if (boss.isDefeated)
            {
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
    public CinemachineVirtualCamera cinematicCam;
    public void DisablePlayerMovement(bool val)
    {
        GameController.Instance.isBlockPlayerControl = val;
    }
    public void NoticeSomething(string content)
    {
        UIEntity.Instance.uiNotification.NoticeSomething(4f, content, "");
    }
    #region bossLock
    public void Lock()
    {
        foreach (var bossLock in locks)
        {
            bossLock.Appear();
        }
    }
    public void UnLock()
    {
        foreach (var bossLock in locks)
        {
            bossLock.Disappear();
        }
    }
    #endregion
    #region cinemachine
    public void SetLookAt(BossController boss)
    {
        if (boss.isDefeated) return;
        cinematicCam.LookAt = boss.transform;
    }
    public void SetFollowAt(Transform trans)
    {
        cinematicCam.Follow = trans;
    }
    public void FollowPlayer()
    {
        cinematicCam.Follow = PlayerEntity.Instance.transform;
    }
    public void RemoveLookAt()
    {
        cinematicCam.LookAt = null;
    }
    #endregion
    #region  dialogue
    public override void ChangeDialogue(int id)
    {
        List<Dialogue> dl = dialogue.alterDialogue;
        if (id < 0) return;
        if (dl.Count == 0 || id > dl.Count - 1)
        {
            return;
        }
        dialogue.currentDialogue = dl[id];
    }
    public override void ChangeDialogue(string key)
    {
        foreach (var dl in dialogue.alterDialogue)
        {
            if (dl.dialogueKey == key)
            {
                dialogue.currentDialogue = dl;
                return;
            }
        }
    }
    #endregion
    #region  music
    public void ChangeMusic(string musicName)
    {
        MusicManager.Instance.PlayMusic(musicName);
    }
    #endregion
    #region call back UI
    public void RegisterBoss()
    {
        UIEntity.Instance.uiBoss.SetCurrentBoss(boss);
    }
    public void Fade(bool val)
    {
        if (val)
        {
            UIEntity.Instance.uiBoss.FadeIn(1f);
        }
        else
        {
            UIEntity.Instance.uiBoss.FadeOut(1f);
        }

    }
    public void ExitCinematic(float time)
    {
        UIEntity.Instance.uiCinematic.ExitCinematic(time);
    }
    public void EnterCinematic(float time)
    {
        UIEntity.Instance.uiCinematic.EnterCinematic(time);
    }
    #endregion
}