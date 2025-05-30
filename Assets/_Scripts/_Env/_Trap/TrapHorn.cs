using UnityEngine;
public class TrapHorn : MyMonobehaviour
{
    [SerializeField] float trapDamage = 0.5f;
    [SerializeField] float timer = 0;
    [SerializeField] float timeBetweenDeal = 2f;

    [SerializeField] bool playerInZone = false;
    Vector2 knockedBackDir = new Vector2(0, 1);
    protected virtual void TrapDebuff()
    {
        Debug.Log("debuff");

        //deal dmg
        PlayerEntity.Instance.playerStat.ReceiveDamage(knockedBackDir, trapDamage);
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