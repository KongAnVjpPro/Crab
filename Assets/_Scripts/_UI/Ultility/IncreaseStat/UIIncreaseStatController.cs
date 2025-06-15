using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIIncreaseStatController : UIComponent
{
    // public List<StatSlot> statSlots;
    public CanvasGroup mainCanvas;
    public StatSlot currentSlot, preSlot;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Image coinIcon;
    // [SerializeField] float maxStat = 25f;
    public bool isShowing = false;
    public void ShowSlot()
    {
        if (isShowing)
        {
            return;
        }
        if (UIController.isSomethingOpened)
        {
            return;
        }
        isShowing = true;
        UIController.isSomethingOpened = true;
        GameController.Instance.isBlockPlayerControl = true;
        mainCanvas.DOFade(1f, 1f).OnComplete(() =>
        {
            mainCanvas.blocksRaycasts = true;
            mainCanvas.interactable = true;
        });
    }
    public void HideSlot()
    {
        if (!isShowing) return;
        isShowing = false;
        UIController.isSomethingOpened = false;

        costText.text = "";
        coinIcon.enabled = false;

        GameController.Instance.isBlockPlayerControl = false;
        mainCanvas.blocksRaycasts = false;
        mainCanvas.interactable = false;
        mainCanvas.DOFade(0f, 1f).OnComplete(() =>
      {

      });
    }
    public void OnClickUpgrade()
    {
        if (!isShowing)
        {
            return;
        }
        if (currentSlot == null) return;
        // int cost = currentSlot.coinCost;

        currentSlot.Upgrade();
    }
    public void OnClickClose()
    {
        currentSlot?.EndClickHandle();
        preSlot?.EndClickHandle();
        currentSlot = null;
        preSlot = null;
        HideSlot();
    }
    public void ShowCost()
    {
        if (this.currentSlot == null) return;
        costText.text = "Cost: " + currentSlot.coinCost.ToString();
        coinIcon.enabled = true;
    }
    void Update()
    {
        //test
        // if (Input.GetKey(KeyCode.U))
        // {
        //     ShowSlot();
        // }

        //test
        if (!isShowing)
            return;
        if (Input.GetKey(KeyCode.Escape))
        {
            OnClickClose();
        }
    }
}