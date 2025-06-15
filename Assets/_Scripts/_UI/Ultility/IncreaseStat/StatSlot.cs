using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
public class StatSlot : MyMonobehaviour
{
    [SerializeField] UIIncreaseStatController controller;
    public StatComponent.StatType statType;
    [SerializeField] CanvasGroup clickedCanvas;
    public float amount = 1f;
    public int coinCost = 0;
    public int costScale = 2;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (controller == null)
        {
            controller = UIEntity.Instance.uiStatUpgrade;
        }
    }
    public void GetCoinCost()
    {
        switch (statType)
        {
            case StatComponent.StatType.Health:
                coinCost = Mathf.RoundToInt(PlayerEntity.Instance.playerStat.TotalHealth + amount) * costScale;
                break;
            case StatComponent.StatType.Mana:
                coinCost = Mathf.RoundToInt(PlayerEntity.Instance.playerStat.TotalMana + amount) * costScale;
                break;
            case StatComponent.StatType.Stamina:
                coinCost = Mathf.RoundToInt(PlayerEntity.Instance.playerStat.TotalStamina + amount) * costScale;
                break;
        }
    }
    public void Upgrade()
    {
        float prevStat = 0;
        float newStat = 0;

        switch (statType)
        {
            case StatComponent.StatType.Health:
                // coinCost = Mathf.RoundToInt(PlayerEntity.Instance.playerStat.TotalHealth + amount) * costScale;
                prevStat = PlayerEntity.Instance.playerStat.TotalHealth;
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Health, amount);
                newStat = PlayerEntity.Instance.playerStat.TotalHealth; ;
                break;
            case StatComponent.StatType.Mana:
                // coinCost = Mathf.RoundToInt(PlayerEntity.Instance.playerStat.TotalMana + amount) * costScale;
                prevStat = PlayerEntity.Instance.playerStat.TotalMana;
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Mana, amount);
                newStat = PlayerEntity.Instance.playerStat.TotalMana;
                break;
            case StatComponent.StatType.Stamina:
                prevStat = PlayerEntity.Instance.playerStat.TotalStamina;
                PlayerEntity.Instance.playerStat.ChangeTotalStats(StatComponent.StatType.Stamina, amount);
                newStat = PlayerEntity.Instance.playerStat.TotalStamina;
                // coinCost = Mathf.RoundToInt(PlayerEntity.Instance.playerStat.TotalStamina + amount) * costScale;
                break;
        }
        if (!PlayerEntity.Instance.playerInventory.CanUseCoin(coinCost))
        {
            UIEntity.Instance.uiNotification.NoticeSomething(4f, "Not Enough Shell", "It cost " + coinCost.ToString());
            return;
        }
        if (prevStat == newStat)
        {
            UIEntity.Instance.uiNotification.NoticeSomething(4f, "Reach Maximum Stat", "");
            return;
        }
        PlayerEntity.Instance.playerInventory.UseCoin(coinCost);
        UIEntity.Instance.uiNotification.NoticeSomething(4f, "Training Success", "");

    }
    public void OnClickHandle()
    {
        controller.preSlot?.EndClickHandle();
        controller.preSlot = this;
        controller.currentSlot = this;
        GetCoinCost();
        //set xong thi show
        controller.ShowCost();
        clickedCanvas.DOFade(1f, 0.5f);
    }
    public void EndClickHandle()
    {
        clickedCanvas.DOFade(0, 0.5f);
    }
}