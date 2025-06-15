using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class TrapHorn : MyMonobehaviour
{
    [SerializeField] float trapDamage = 0.5f;
    [SerializeField] float timer = 0;
    [SerializeField] float timeBetweenDeal = 2f;

    [SerializeField] bool playerInZone = false;
    Vector2 knockedBackDir = new Vector2(0, 1);
    public UnityEvent CallBackHorn;
    // Vector2 trapRespawnPoint;
    protected virtual void TrapDebuff()
    {
        Debug.Log("debuff");

        //deal dmg
        PlayerEntity.Instance.playerStat.ReceiveDamage(knockedBackDir, trapDamage);
        CallBackHorn?.Invoke();
        // SpawnInSaveZone();
    }
    protected override void Awake()
    {
        base.Awake();
        // CallBackHorn += SpawnInSaveZone();
    }
    #region callbacks
    public void SpawnInSaveZone()
    {
        if (!PlayerEntity.Instance.pState.alive) return;
        StartCoroutine(Respawn());

    }
    [Header("Callback Slow Down: ")]
    [SerializeField] float slowDownAmount = 0.5f;
    [SerializeField] float slowDownDuration = 2f;
    public void SlowDown()
    {
        StartCoroutine(ApplySlowDown());

    }
    IEnumerator ApplySlowDown()
    {
        if (!PlayerEntity.Instance.pState.alive) yield break;
        if (PlayerEntity.Instance.playerMovement.isOnBuffMove)
        {
            yield break;
        }
        PlayerEntity.Instance.playerMovement.BoostSpeedAndJump(slowDownAmount, slowDownAmount);

        yield return new WaitForSeconds(slowDownDuration);
        PlayerEntity.Instance.playerMovement.ResetBoost();
        // PlayerEntity.Instance.playerMovement.BoostSpeedAndJump(restoreAmount, restoreAmount);
    }


    #endregion
    IEnumerator Respawn()
    {
        PlayerEntity.Instance.pState.invincible = true;
        GameController.Instance.isBlockPlayerControl = true;
        yield return StartCoroutine(UIEntity.Instance.uISaveScreen.EnterSaveScreen());
        PlayerEntity.Instance.transform.position = GameController.Instance.trapRespawnPoint;
        GameController.Instance.isBlockPlayerControl = false;
        PlayerEntity.Instance.pState.invincible = false;
        yield return StartCoroutine(UIEntity.Instance.uISaveScreen.ExitScreen());

    }




    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInZone = true;

        timer = timeBetweenDeal;

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInZone = false;

        timer = 0f;

    }
    void Update()
    {
        if (!playerInZone) return;

        timer += Time.deltaTime;
        if (timer >= timeBetweenDeal)
        {
            TrapDebuff();
            timer = 0f;
        }
    }
    // void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (!collision.CompareTag("Player")) return;

    // }
}