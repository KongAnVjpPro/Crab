using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossPreDeath : EnemyState
{

    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedPrepareDeath;
    }

    public void SetIscomplete(bool val)
    {
        isComplete = val;
    }
    public override EnemyStateID? CheckNextState()
    {
        return EnemyStateID.SeaWeedTalk;
    }
    public override void Enter()
    {
        base.Enter();
        SetTalkLayer();
        // stateMachine.collie.enabled = true;
        isComplete = true;
        stateMachine.shield.SetActive(false);
        StartCoroutine(LastTalk());
    }
    [SerializeField] float deathTime = 5f;
    [SerializeField] float timer = 0;
    IEnumerator LastTalk()
    {
        foreach (var minion in stateMachine.minions)
        {
            if (!minion.gameObject.activeSelf) continue;
            minion.state.ChangeState(EnemyStateID.Dead);
        }
        while (timer < deathTime)
        {
            timer += Time.deltaTime;
            stateMachine.impulseSource.GenerateImpulse();
            yield return null;
        }
        DialogueTrigger dialogue = stateMachine.GetComponent<DialogueTrigger>();
        dialogue.TriggerDialogue();

    }
}