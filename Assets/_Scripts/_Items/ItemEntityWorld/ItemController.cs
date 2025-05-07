using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class ItemController : MyMonobehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;
    [SerializeField] private ItemSpawner itemSpawner;
    [SerializeField] private float existTime = 5f;
    [SerializeField] private float startTime = 0f;
    [SerializeField] public bool startCountDown = false;
    [SerializeField] TriggerZone triggerZone;
    [SerializeField] private Rigidbody2D rg;
    // [SerializeField] Light2D light2D;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTriggerZone();
        LoadRigidbody2D();
    }
    protected virtual void LoadRigidbody2D()
    {
        if (rg != null) return;
        rg = GetComponent<Rigidbody2D>();
    }
    protected virtual void LoadTriggerZone()
    {
        if (triggerZone != null) return;
        triggerZone = GetComponent<TriggerZone>();

    }
    public void Init()
    {
        startTime = 0f;
        startCountDown = true;
        triggerZone.ResetTrigger();
        rg.gravityScale = 1;
        // sr.DOFade(1, 0.5f);
        sr.color = Color.white;
    }
    public void SetSpawner(ItemSpawner spawner)
    {
        itemSpawner = spawner;
    }
    public void SetItemData(ItemSO itemSO, int amount)
    {
        itemData = new ItemData { itemSO = itemSO, amount = amount };
        if (itemData.itemSO != null)
        {
            sr.sprite = itemData.itemSO.itemSprite;
        }

    }
    public void GrabItem()
    {
        // if(!PlayerEntity.Instance.playerinve)
        if (!PlayerEntity.Instance.playerInventory.CanAddItem(itemData)) return;

        PlayerEntity.Instance.playerInventory.AddItem(itemData);
        rg.gravityScale = 0;
        Sequence seq = DOTween.Sequence();
        seq.Join(sr.DOFade(0, 1f));
        seq.Join(transform.DOMove(transform.position + new Vector3(0, 1, 0), 1f));
        seq.OnComplete(() =>
        {

            DestroyItem();
        });

    }
    public void DropItem()
    {

    }
    public void DestroyItem()
    {
        sr.DOFade(0, 1f).OnComplete(() =>
        {
            itemData = null;
            sr.sprite = null;
            gameObject.SetActive(false);
        });

    }
    void Update()
    {
        if (startCountDown)
        {
            startTime += Time.deltaTime;
            if (startTime >= existTime)
            {
                DestroyItem();
                startCountDown = false;
                startTime = 0f;
            }
        }
    }

    //quyet dinh logic item o day
}