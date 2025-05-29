using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BossDeath : EnemyState//reuseable
{
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.Dead;
        ParticleSystem[] parts = stateMachine.GetComponentsInChildren<ParticleSystem>();
        if (parts.Length == 0) return;
        foreach (var part in parts)
        {
            particles.Add(part);
        }
    }
    [SerializeField] float deathTime = 5f;
    [SerializeField] float deathTimer = 0;
    [SerializeField] List<ParticleSystem> particles;

    void Awake()
    {

    }
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Death());
        GameController.Instance.isBlockPlayerControl = true;
    }
    IEnumerator Death()
    {
        foreach (var part in particles)
        {
            part.Play();
        }
        yield return new WaitForSeconds(deathTime);
        foreach (var part in particles)
        {
            part.Stop();
        }
        yield return new WaitForSeconds(deathTime);
        isComplete = true;
    }
    public string bossDeathNotification = "Boss defeated";
    public string subBossDeathNotification = "A part of the ocean has been cleaned";
    public override void Exit()
    {
        base.Exit();

        // SaveSystem.Instance.isSeawWeedDefeated = true;
        // SaveSystem.Instance.SaveBossDefeated();
        // stateMachine.gameObject.SetActive(false);
        BossController boss = stateMachine.GetComponent<BossController>();
        if (boss != null)
        {
            boss.Defeated();
        }
        UIEntity.Instance.uiNotification.NoticeSomething(4f, bossDeathNotification, subBossDeathNotification);
        GameController.Instance.isBlockPlayerControl = true;
    }
    public override EnemyStateID? CheckNextState()
    {
        return EnemyStateID.Patrolling;
    }
}