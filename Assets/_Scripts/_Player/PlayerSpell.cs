using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PlayerSpell : PlayerComponent
{



    public SpellType currentType;
    public float manaCost = 0;
    public bool isCasting = false;


    [SerializeField] float spellTimer = 6f;
    [SerializeField] float spellCoolDown = 5f;
    Vector2 spellDir = new Vector2(0, 0);

    [Header("Tide Burst (side)")]
    [SerializeField] float spellPrepareTime = 1f;
    [SerializeField] RangeAttackSpawner spellTideBurstSpawner;
    public float tideBurstCost = 3f;
    public float recoilForce = 5f;

    [Header("Crushing Wave (Down)")]
    [SerializeField] GameObject crushingWave;
    [SerializeField] float crushingWaveCost = 5f;
    [SerializeField] float downSpeed = 20f;
    [SerializeField] bool collideSomeThing = false;


    [Header("Abyssal Pulse (Up)")]
    [SerializeField] GameObject abyssalPulse;
    [SerializeField] float abyssalPusleCost = 5f;
    [SerializeField] float pulseDuration = 1f;

    public CinemachineImpulseSource impulseSource;
    void DecideSpellType()
    {
        if (!playerController.playerInput.cast)
        {
            currentType = SpellType.None;
            manaCost = 0;
            spellDir = Vector2.zero;
            return;
        }
        if ((playerController.playerInput.yAxis == 0) || ((playerController.playerInput.yAxis < 0) && (playerController.playerMovement.IsOnGround())))
        {
            currentType = SpellType.TideBurst;
            manaCost = tideBurstCost;
            spellDir = playerController.pState.lookingRight ? Vector2.right : Vector2.left;
            return;
        }
        else if (playerController.playerInput.yAxis > 0)
        {
            currentType = SpellType.AbyssalPulse;
            manaCost = abyssalPusleCost;
            spellDir = Vector2.up;
            return;
        }
        else if ((playerController.playerInput.yAxis < 0) && (!playerController.playerMovement.IsOnGround()))
        {
            currentType = SpellType.CrushingWave;
            manaCost = crushingWaveCost;
            spellDir = Vector2.down;
        }
    }

    void Update()
    {

        if (!playerController.pState.alive)
            return;
        spellTimer += Time.deltaTime;
        if (!playerController.playerInput.cast)
            return;

        DoSpell();

    }
    void DoSpell()
    {
        DecideSpellType();
        //check spell unlocked
        if (!CanDoSpell())
        {
            return;
        }
        //check mana
        if (playerController.playerStat.CurrentMana < manaCost)
        {
            return;
        }
        //check cd
        if (spellTimer < spellCoolDown)
        {
            return;
        }
        //check spell while cast
        if (isCasting)
        {
            return;
        }

        spellTimer = 0;
        //consume mana  
        playerController.playerStat.ChangeCurrentStats(StatComponent.StatType.Mana, -1 * manaCost);
        // //logic bullet

        // //logic player
        //  if (!hitTarget) return;
        // if (currentState == AttackState.forwardAttack)
        // {
        //     Vector2 recoilDir = (forwardAttackPos.position - playerController.transform.position).normalized;
        //     playerController.playerRecoil.RecoilBoth(recoilDir.x * attackRecoil, false);
        // }
        // else if (currentState == AttackState.downAttack)
        // {
        //     playerController.playerRecoil.RecoilVertical(true, attackRecoil);

        StartCoroutine(CastCoroutine());
    }
    IEnumerator CastCoroutine()
    {
        switch (currentType)
        {
            case SpellType.TideBurst:
                isCasting = true;
                // impulseSource.GenerateImpulse(spellDir);
                playerController.playerAnimator.SideCast(true);
                RangeAttack bullet = spellTideBurstSpawner.DelaySpawn(transform.position, spellDir);

                GameController.Instance.isBlockPlayerControl = true;
                playerController.rb.gravityScale = 0;
                playerController.rb.velocity = Vector2.zero;


                yield return new WaitForSeconds(spellPrepareTime);
                bullet.Fire(transform.position, spellDir);
                playerController.playerAnimator.SideCast(false);
                GameController.Instance.isBlockPlayerControl = false;
                playerController.rb.gravityScale = 3;
                playerController.playerRecoil.RecoilBoth(spellDir.x > 0 ? 1 : -1, true, 0);
                isCasting = false;
                break;
            case SpellType.CrushingWave:
                isCasting = true;
                playerController.playerAnimator.DownCast(true);
                GameController.Instance.isBlockPlayerControl = true;
                playerController.rb.velocity = Vector2.zero;
                crushingWave.SetActive(true);
                playerController.rb.velocity = spellDir * downSpeed;
                playerController.rb.gravityScale = 3f;
                while ((!playerController.playerMovement.IsOnGround()) && !collideSomeThing)
                {
                    yield return null;
                }
                collideSomeThing = false;
                impulseSource.GenerateImpulse();
                playerController.playerAnimator.DownCast(false);
                GameController.Instance.isBlockPlayerControl = false;
                isCasting = false;
                break;
            case SpellType.AbyssalPulse:
                isCasting = true;
                playerController.playerAnimator.Upcast(true);
                GameController.Instance.isBlockPlayerControl = true;
                playerController.rb.velocity = Vector2.zero;
                playerController.rb.gravityScale = 0;


                // yield return new WaitForSeconds(pulseDuration * 0.5f);

                yield return new WaitForSeconds(pulseDuration * 0.2f);
                abyssalPulse.SetActive(true);
                yield return new WaitForSeconds(pulseDuration * 0.3f);

                isCasting = false;
                playerController.rb.gravityScale = 3f;
                playerController.playerAnimator.Upcast(false);
                GameController.Instance.isBlockPlayerControl = false;
                break;
        }
    }
    public void SetCollideDownCast(bool val)
    {
        collideSomeThing = val;
    }
    bool CanDoSpell()
    {
        switch (currentType)
        {
            case SpellType.None:
                return false;
            case SpellType.TideBurst:
                return playerController.pState.unlockedTideBurst;
            case SpellType.CrushingWave:
                return playerController.pState.unlockedCurshingWave;
            case SpellType.AbyssalPulse:
                return playerController.pState.unlockedAbyssalPulse;
        }
        return false;
    }

}
public enum SpellType
{
    None,
    TideBurst,
    CrushingWave,
    AbyssalPulse
}