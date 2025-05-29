using DG.Tweening;
using UnityEngine;
public class Coin : MyMonobehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] bool isGrab = false;
    [SerializeField] bool startCountDown = false;
    [SerializeField] float startTime = 0;
    [SerializeField] float existTime = 5f;
    [SerializeField] ItemSpawner itemSpawner;
    public void SetSpawner(ItemSpawner itemSpawner)
    {
        this.itemSpawner = itemSpawner;
    }
    public void SelfDestroy()
    {
        sr.DOFade(0, 1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        isGrab = false;
        startCountDown = false;
        startTime = 0;
    }
    void OnEnable()
    {
        ResetValue();
        startCountDown = true;
    }
    void Update()
    {
        if (startCountDown)
        {
            startTime += Time.deltaTime;
            if (startTime >= existTime)
            {
                SelfDestroy();
                startCountDown = false;
                startTime = 0f;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (isGrab) return;
        isGrab = true;
        collision.GetComponent<PlayerInventory>()?.AddCoin(1);
        SoundManager.Instance.PlaySound3D("CoinPickup", transform.position);
        SelfDestroy();
    }
}