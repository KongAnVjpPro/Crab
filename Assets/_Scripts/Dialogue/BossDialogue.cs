using UnityEngine;
public class BossDialogue : DialogueTrigger
{
    public BossType bossType;
    public bool isBossDefeated = false;


    void GetBossState()
    {

        isBossDefeated = SaveSystem.Instance.GetBossDefeated(bossType);

    }
    void Start()
    {
        GetBossState();
    }
    void Awake()
    {
        currentDialogue = mainDialogue;
    }
    public override void TriggerDialogue()
    {
        if (isBossDefeated)
        {
            return;
        }
        base.TriggerDialogue();
        Debug.Log("dialogue");
    }
    protected override void Update()
    {
        // base.Update();

    }
}