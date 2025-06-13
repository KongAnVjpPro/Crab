using DG.Tweening;
using TMPro;
using UnityEngine;
public class UIInventoryController : MyMonobehaviour
{
    [SerializeField] CanvasGroup statAndSkillCanvas;
    [SerializeField] CanvasGroup inventoryCanvas;
    [SerializeField] CanvasGroup mapCanvas;
    [SerializeField] RectTransform tabsContainer;
    [SerializeField] float offSetXMove = 1920f;

    public int prevTabIndex = 0;
    public int currentTabIndex = 0;
    public bool isTabSwitching = false;
    public float tabSwitchingTime = 0.5f;

    public bool isTabSwitchingEnabled = true;

    void DecideTabIndex()
    {
        if (statAndSkillCanvas.interactable)
        {
            currentTabIndex = 0;
            inventoryCanvas.interactable = false;
            mapCanvas.interactable = false;
            return;
        }
        else if (inventoryCanvas.interactable)
        {
            currentTabIndex = 1;
            statAndSkillCanvas.interactable = false;
            mapCanvas.interactable = false;
            return;
        }
        else if (mapCanvas.interactable)
        {
            currentTabIndex = 2;
            statAndSkillCanvas.interactable = false;
            inventoryCanvas.interactable = false;
            return;
        }
        else
        {
            currentTabIndex = -1;
        }
    }
    void SetInteractable()
    {
        switch (currentTabIndex)
        {
            case 0:
                statAndSkillCanvas.interactable = true;
                inventoryCanvas.interactable = false;
                mapCanvas.interactable = false;
                break;
            case 1:
                statAndSkillCanvas.interactable = false;
                inventoryCanvas.interactable = true;
                mapCanvas.interactable = false;
                break;
            case 2:
                statAndSkillCanvas.interactable = false;
                inventoryCanvas.interactable = false;
                mapCanvas.interactable = true;
                break;
            default:
                statAndSkillCanvas.interactable = false;
                inventoryCanvas.interactable = true;
                mapCanvas.interactable = false;
                break;
        }
    }
    public void AnimationHandle(int direction)
    {
        // if(isTabSwitching)
        isTabSwitching = true;
        Vector2 targetPosition = tabsContainer.anchoredPosition + new Vector2(offSetXMove * direction, 0);
        tabsContainer.DOAnchorPos(targetPosition, tabSwitchingTime).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            isTabSwitching = false;

        });
    }
    public void SwitchTab(int direction)
    {
        if (isTabSwitching)
        {
            return;
        }
        DecideTabIndex();//reload tab index
        prevTabIndex = currentTabIndex; // save pre tab index
        if (currentTabIndex == -1) return;
        int nextTabIndex = currentTabIndex + direction;
        if (nextTabIndex < 0)
        {
            nextTabIndex = 2;
        }
        else if (nextTabIndex > 2)
        {
            nextTabIndex = 0;
        }
        currentTabIndex = nextTabIndex; // update current tab index
        SetInteractable(); // set interactable canvas
        AnimationHandle(-currentTabIndex + prevTabIndex); // switch
    }
    void Update()
    {
        if (!isTabSwitchingEnabled) return;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            SwitchTab(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SwitchTab(-1);
        }


    }
    protected override void Awake()
    {
        base.Awake();
        SwitchTab(0);
    }
    #region  Stat and Skill
    [Header("Stat and Skill Canvas: ")]
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI mpText;
    [SerializeField] TextMeshProUGUI staText;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI spdText;
    public void UpdateStatAndSkillCanvas()
    {
        hpText.text = PlayerEntity.Instance.playerStat.TotalHealth.ToString();
        mpText.text = PlayerEntity.Instance.playerStat.TotalMana.ToString();
        staText.text = PlayerEntity.Instance.playerStat.TotalStamina.ToString();
        atkText.text = PlayerEntity.Instance.playerAttack.Damage.ToString();
        spdText.text = (PlayerEntity.Instance.speedBoost * 100).ToString() + "%";
    }

    #endregion

    #region Map

    public void UpdateMapCanvas()
    {

    }

    #endregion
}