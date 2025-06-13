using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
public class UIShellStation : UIComponent

{
    [SerializeField] CanvasGroup mainCanvasGroup;
    [SerializeField] UIShellDescription shellDescription;
    public List<CombinedShellData> ownedShellList = new List<CombinedShellData>();
    public List<UIShellSlot> shellSlots = new List<UIShellSlot>();

    public bool isShellStationOpen = false;
    protected override void Awake()
    {
        base.Awake();
        UIShellSlot[] slots = mainCanvasGroup.GetComponentsInChildren<UIShellSlot>();
        shellSlots.Clear();
        foreach (UIShellSlot slot in slots)
        {
            shellSlots.Add(slot);
        }
    }

    public void GetShellList()
    {
        ownedShellList = PlayerEntity.Instance.playerShell.ownedShellList;
    }

    public void HideShellStation()
    {


        // if (!UIController.isSomethingOpened) return;

        isShellStationOpen = false;
        GameController.Instance.isBlockPlayerControl = false;

        UIController.isSomethingOpened = false;

        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        mainCanvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            mainCanvasGroup.alpha = 0f;
        });

        HideDescription();
    }


    public void ShowShellStation()
    {


        if (UIController.isSomethingOpened) return;
        GameController.Instance.isBlockPlayerControl = true;
        isShellStationOpen = true;

        UIController.isSomethingOpened = true;



        mainCanvasGroup.DOFade(1f, 1f).OnComplete(() =>
        {
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;
        });


        GetShellList();


        UpdateShellSlots();

    }

    public void UpdateShellSlots()
    {
        for (int i = 0; i < shellSlots.Count; i++)
        {
            if (i < ownedShellList.Count)
            {
                shellSlots[i].SetShellData(ownedShellList[i]);
            }
            else
            {
                shellSlots[i].ClearShellData();
            }
        }
    }

    #region  description
    public void ShowDescription(CombinedShellData shellData)
    {
        if (shellData == null) return;
        shellDescription.SetShellData(shellData);
        shellDescription.ShowShellDescription();
    }
    public void HideDescription()
    {
        shellDescription.HideShellDescription();
        // GameController.Instance.isBlockPlayerControl = false;
    }
    #endregion
    #region UI Interaction
    public UIShellSlot currentSlot;
    public UIShellSlot previousSlot;


    #endregion
    void Update()
    {
        if (!isShellStationOpen) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            HideShellStation();
        }

    }

    #region Forge
    [Header("Forge Settings")]
    [SerializeField] public UIShellSlot shell1;
    [SerializeField] public UIShellSlot shell2;
    [SerializeField] UIShellSlot shellForgeResult;

    [SerializeField] TextMeshProUGUI forgeCostText;
    [SerializeField] int costBase = 30;
    [SerializeField] int currentCost = 0;
    [SerializeField] public UIShellSlot slot1Chosen;
    [SerializeField] public UIShellSlot slot2Chosen;

    void UpdateCostText()
    {
        int cost = 0;
        if (shell1.ShellData == null || shell2.ShellData == null)
        {
            forgeCostText.text = "";
            return;
        }
        List<ShellAbilityID> abilities1 = shell1.ShellData.combinedAbilities;
        List<ShellAbilityID> abilities2 = shell2.ShellData.combinedAbilities;
        cost = costBase * abilities1.Count;
        foreach (ShellAbilityID ability in abilities2)
        {
            if (abilities1.Contains(ability))
            {
                continue;
            }
            cost += costBase;
        }
        currentCost = cost;
        forgeCostText.text = "Price: " + cost.ToString();
    }
    #region OnClick Forge
    public void OnClickForge()
    {
        if (shell1.ShellData == null || shell2.ShellData == null)
        {
            UIEntity.Instance.uiNotification.NoticeSomething(2f, "Forge slots are empty!", "add shells to forge");
            return;
        }
        if (!PlayerEntity.Instance.playerInventory.CanUseCoin(currentCost))
        {
            UIEntity.Instance.uiNotification.NoticeSomething(2f, "Not enough coins!", "need " + currentCost + " coins");
            return;
        }
        // CombinedShellData result = PlayerEntity.Instance.playerShell.TryCombineShell(shell1.ShellData, shell2.ShellData,out result);
        CombinedShellData res;
        if (!PlayerEntity.Instance.playerShell.TryCombineShell(shell1.ShellData, shell2.ShellData, out res))
        {
            // ForgeResult(res);
            UIEntity.Instance.uiNotification.NoticeSomething(2f, "Forge failed!", "try again");
            return;
        }
        if (res == null)
        {
            UIEntity.Instance.uiNotification.NoticeSomething(2f, "Forge failed!", "try again");
            return;
        }
        shellForgeResult.SetShellData(res);
        GetShellList();
        UpdateShellSlots();
        // PlayerEntity.Instance.playerShell.RemoveForge(shell1.ShellData);
        // PlayerEntity.Instance.playerShell.RemoveForge(shell2.ShellData);
        PlayerEntity.Instance.playerInventory.UseCoin(currentCost);
        // UIEntity.
        ClearAll();
    }
    #endregion
    public void ClearAll()
    {
        shell1.ClearShellData();
        shell2.ClearShellData();
        shellForgeResult.ClearShellData();
        forgeCostText.text = "";
        slot1Chosen = null;
        slot2Chosen = null;
        currentCost = 0;
    }
    public void AddToForge()
    {
        if (currentSlot == slot1Chosen || currentSlot == slot2Chosen)
        {
            UIEntity.Instance.uiNotification.NoticeSomething(2f, "Already added to forge!", "remove first");
            return;
        }
        if (shell1.ShellData == null)
        {
            shell1.SetShellData(currentSlot.ShellData);
            slot1Chosen = currentSlot;
            // slot.ClearShellData();
            // UpdateCostText(costBase);
        }
        else if (shell2.ShellData == null)
        {
            shell2.SetShellData(currentSlot.ShellData);
            slot2Chosen = currentSlot;
            // slot.ClearShellData();
            // UpdateCostText(costBase * 2);

        }
        else
        {
            // Debug.Log("Forge slots are full!");
            UIEntity.Instance.uiNotification.NoticeSomething(2f, "Forge slots are full!", "remove one shell first");
        }
        UpdateCostText();
    }
    public void RemoveForgeSlot(UIShellSlot slot)
    {
        if (slot == shell1)
        {
            shell1.ClearShellData();
            slot1Chosen = null;
        }
        else if (slot == shell2)
        {
            shell2.ClearShellData();
            slot2Chosen = null;
        }
        else
        {
            return;
        }
        UpdateCostText();
    }
    #endregion
}