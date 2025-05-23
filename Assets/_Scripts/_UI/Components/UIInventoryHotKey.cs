using TMPro;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class UIInventoryHotKey : UIComponent
{
    [Header("HotKey1")]
    [SerializeField] float coolDownHotKey = 5f;
    [SerializeField] TextMeshProUGUI hotKey1Count;
    [SerializeField] Image fillerHotKey1;
    [SerializeField] float hotKey1Timer = 5;
    [SerializeField] ItemSO hotKey1Item; //healPotion
    [SerializeField] bool isUsedHealPotion = false;


    protected override void Awake()
    {
        base.Awake();
        // hotKey1Item? = PlayerEntity.Instance.playerInventory.he
        StartCoroutine(WaitForPlayer());
    }
    IEnumerator WaitForPlayer()
    {
        while (PlayerEntity.Instance == null || PlayerEntity.Instance.playerInventory == null)
        {
            yield return null;
        }
        coolDownHotKey = PlayerEntity.Instance.playerInventory.healPotionCoolDown;
        hotKey1Item = PlayerEntity.Instance.playerInventory.healPotionPreb;
        UpdateFillerHotKey1();
        int cnttemp = PlayerEntity.Instance.playerInventory.ItemCount(new ItemData { itemSO = hotKey1Item, amount = 1 });
        if (cnttemp == -1)
        {
            Debug.Log("null");
            hotKey1Count.text = "0";
        }
        else
        {
            hotKey1Count.text = cnttemp.ToString();
        }
        // UpdateHotKey1Text();
        //    int cnttemp = PlayerEntity.Instance.playerInventory.IsContainItem(new ItemData{itemSO = hotKey1Item,amount = 1})
    }
    #region  Update
    public void Reload()
    {
        UpdateHotKey1Text();
        // hotKey1Count.text =
    }
    void Update()
    {
        hotKey1Timer += Time.deltaTime;
        if (PlayerEntity.Instance.playerInput.hotkey1)
        {
            Debug.Log("HK1");
            UseHealPotion();
        }
        if (isUsedHealPotion)
        {
            UpdateFillerHotKey1();
        }
    }
    #endregion



    #region HK1
    void UseHealPotion()
    {
        if (hotKey1Timer >= coolDownHotKey)
        {
            PlayerEntity.Instance.playerInventory.HotKey_UseHealPotion();
            hotKey1Timer = 0;
            isUsedHealPotion = true;
            UpdateHotKey1Text();
        }

    }
    void UpdateFillerHotKey1()
    {
        if (hotKey1Timer >= coolDownHotKey)
        {
            isUsedHealPotion = false;
            return;
        }
        fillerHotKey1.fillAmount = Mathf.Clamp(Mathf.Abs((hotKey1Timer - coolDownHotKey) / coolDownHotKey), 0, 1);
    }


    //  [SerializeField] TextMeshProUGUI hotKey1Count;
    int previousHotKey1Count = 0;
    Sequence s;
    void UpdateHotKey1Text()
    {

        if (!PlayerEntity.Instance.playerInventory.IsContainItem(new ItemData { itemSO = hotKey1Item, amount = 1 }))
        {
            hotKey1Count.text = "0";
            return;
        }
        int currentHK1Count = PlayerEntity.Instance.playerInventory.inventory.FindItem(hotKey1Item).amount;
        if (previousHotKey1Count == -1)
        {
            previousHotKey1Count = currentHK1Count;
            hotKey1Count.text = currentHK1Count.ToString();
            return;
        }

        bool isIncrease = currentHK1Count > previousHotKey1Count;

        if (currentHK1Count == previousHotKey1Count)
        {
            hotKey1Count.text = currentHK1Count.ToString();
            return;
        }
        Color originalColor = Color.white;
        Color targetColor = isIncrease ? Color.green : Color.red;


        Vector3 originalScale = new Vector3(1, 1, 1);
        Vector3 punchScale = originalScale * 1.2f;


        hotKey1Count.text = currentHK1Count.ToString();

        if (s != null && s.IsActive())
        {
            hotKey1Count.color = Color.white;
            hotKey1Count.transform.localScale = originalScale;
            s.Kill();
        }
        s = DOTween.Sequence();
        // Sequence s = DOTween.Sequence();
        // s.Kill();
        s.Append(hotKey1Count.DOColor(targetColor, 0.2f))
         .Join(hotKey1Count.transform.DOPunchScale(punchScale, 0.3f, 10, 1))
         .AppendInterval(0.5f)
         .Append(hotKey1Count.DOColor(originalColor, 0.2f));

        previousHotKey1Count = currentHK1Count;
    }
    #endregion
}