using UnityEngine;
public class BossPhaseTwo : EnemyState
{
    [SerializeField] ParticleSystem phaseTwoParticle;
    [SerializeField] float changeStateTime = 5f;
    [SerializeField] float changeStateTimer = 0;
    [SerializeField] float boostScale = 2f;
    public override void Init(EnemyStateMachine machine)
    {
        base.Init(machine);
        stateID = EnemyStateID.SeaWeedPhaseTwo;
    }
    void Awake()
    {
        phaseTwoParticle.Stop();
    }
    public string bossSummonText = "Riseeeeee, my Children";
    public string warningText = "";
    public override void Enter()
    {
        base.Enter();
        UIEntity.Instance.uiNotification.NoticeSomething(4f, bossSummonText, warningText);
        SetTalkLayer();
        stateMachine.isOnPhase2 = true;
        phaseTwoParticle.Play();
        stateMachine.collie.enabled = false;
        stateMachine.bossBoost *= boostScale;
    }
    public override void Do()
    {
        base.Do();
        changeStateTimer += Time.deltaTime;
        if (changeStateTimer < changeStateTime) return;
        phaseTwoParticle.Stop();
        isComplete = true;
    }
    public override void Exit()
    {
        base.Exit();
        SetAttackLayer();
        stateMachine.collie.enabled = true;
    }
    public override EnemyStateID? CheckNextState()
    {
        // return base.CheckNextState();
        if (stateMachine.IsDead()) return EnemyStateID.SeaWeedPrepareDeath;
        else return EnemyStateID.SeaWeedSummon;
    }
}