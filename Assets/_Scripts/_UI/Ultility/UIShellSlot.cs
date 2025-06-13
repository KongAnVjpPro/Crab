using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIShellSlot : MyMonobehaviour
{
    [SerializeField] CombinedShellData shellData;
    [SerializeField] Image shellIcon;
    [SerializeField] Image shellDecorImage;
    [SerializeField] Color shellMainColor;
    [SerializeField] Color shellDecorColor;


    [SerializeField] CanvasGroup clickedImage;

    public CombinedShellData ShellData => shellData;
    public void SetShellData(CombinedShellData data)
    {
        shellData = data;
        if (shellData == null) return;
        shellIcon.color = new Color(1f, 1f, 1f, 1f);
        shellDecorImage.color = new Color(1f, 1f, 1f, 1f);

        shellMainColor = shellData.mainColor;
        shellDecorColor = shellData.decorColor;

        shellIcon.color = shellMainColor;
        shellDecorImage.color = shellDecorColor;
    }
    public void ClearShellData()
    {
        shellData = null;
        shellIcon.color = new Color(1f, 1f, 1f, 0f);
        shellDecorImage.color = new Color(1f, 1f, 1f, 0f);
    }

    #region UI Interaction
    public void OnClickHandle()
    {

        clickedImage.DOFade(1f, 0.5f);
        UIEntity.Instance.uiShellStation.previousSlot?.EndClickHandle();
        UIEntity.Instance.uiShellStation.currentSlot = this;
        UIEntity.Instance.uiShellStation.previousSlot = this;


        // uiInventory.ShowItemDescription(currentItem);
        UIEntity.Instance.uiShellStation.ShowDescription(shellData);
    }

    public void EndClickHandle()
    {
        clickedImage.DOFade(0, 0.5f);
        // uiInventory.HideItemDescription();
    }
    public void OnClickRemoveForge()
    {
        if (shellData == null) return;
        // PlayerEntity.Instance.playerShell.RemoveForge(shellData);

        EndClickHandle();
        // UIEntity.Instance.uiShellStation.UpdateShellSlots();
        if (UIEntity.Instance.uiShellStation.shell1 == this)
        {
            UIEntity.Instance.uiShellStation.RemoveForgeSlot(this);
        }
        else if (UIEntity.Instance.uiShellStation.shell2 == this)
        {
            UIEntity.Instance.uiShellStation.RemoveForgeSlot(this);
        }
        else
        {
            // clickedImage.DOFade(1f, 0.5f);
        }
    }
    // public void OnClick
    #endregion
}