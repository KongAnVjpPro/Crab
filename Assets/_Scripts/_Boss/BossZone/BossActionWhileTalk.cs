using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
public class BossActionWhileTalk : NPCAction//can be use for action while talk and trigger zone so on
{
    [SerializeField] DialogueTrigger dialogue;
    [SerializeField] BossController boss;
    protected override void Awake()
    {
        base.Awake();
        boss = FindAnyObjectByType<BossController>();
        // dialogue = FindAnyObjectByType<BossController>().GetComponent<DialogueTrigger>();
        dialogue = boss.GetComponent<DialogueTrigger>();
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
    public void ChangeMusic(string musicName)
    {
        MusicManager.Instance.PlayMusic(musicName);
    }
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
}