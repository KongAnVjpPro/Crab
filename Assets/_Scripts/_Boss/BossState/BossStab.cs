using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class BossStab : EnemyState
{
    public Transform limitRight, limitLeft;
    // [SerializeField] float appearTimer = 0f;
    [SerializeField] float appearTime = 5f;
    [SerializeField] float noticeTimer = 0f;
    [SerializeField] float noticeTime = 3f;
    //spike
    public float heightAppear = 5f;
    public List<TrapAppear> spikes;
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedStab;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(limitLeft.position, limitRight.position);
    }
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(SpawnRandomSpike());
        SetTalkLayer();
        noticeTimer = 0;
        stateMachine.collie.enabled = false;
    }
    IEnumerator SpawnRandomSpike()
    {
        //ease.outquad

        foreach (var trap in spikes)
        {
            float randomX = Random.Range(limitLeft.position.x, limitRight.position.x);
            trap.transform.position = new Vector2(randomX, trap.transform.position.y);
            trap.gameObject.SetActive(true);
        }
        foreach (var trap in spikes)
        {
            trap.dustEffect.Play();
        }
        while (noticeTimer < noticeTime / stateMachine.bossBoost)
        {
            noticeTimer += Time.deltaTime;


            yield return null;
        }
        Sequence s = DOTween.Sequence();
        foreach (var trap in spikes)
        {
            s.Join(trap.transform.DOMove(new Vector2(trap.transform.position.x, trap.transform.position.y + heightAppear * stateMachine.bossBoost), appearTime / stateMachine.bossBoost).SetEase(Ease.OutQuad));
            trap.dustEffect.Stop();
        }
        yield return s.WaitForCompletion();
        s = DOTween.Sequence();
        foreach (var trap in spikes)
        {
            s.Join(trap.transform.DOMove(new Vector2(trap.transform.position.x, trap.transform.position.y - heightAppear * stateMachine.bossBoost), appearTime / stateMachine.bossBoost).SetEase(Ease.OutQuad));
        }
        yield return s.WaitForCompletion();
        foreach (var trap in spikes)
        {

            trap.gameObject.SetActive(false);
        }
        isComplete = true;


    }
    public override void Exit()
    {
        base.Exit();
        // appearTimer = 0;
        SetAttackLayer();
        noticeTimer = 0;
        stateMachine.collie.enabled = true;
    }
    public override EnemyStateID? CheckNextState()
    {
        if (stateMachine.IsDead())
        {
            return EnemyStateID.SeaWeedPrepareDeath;
        }
        return EnemyStateID.Patrolling;
    }
}