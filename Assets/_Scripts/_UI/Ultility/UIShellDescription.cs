using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIShellDescription : MyMonobehaviour
{
    [SerializeField] CanvasGroup shellDescriptionCanvasGroup;
    [SerializeField] TextMeshProUGUI shellNameText;
    [SerializeField] TextMeshProUGUI shellDescriptionText;
    [SerializeField] CombinedShellData shellData;
    [SerializeField] Image shellIcon;
    [SerializeField] Image shellDecorImage;
    [SerializeField] Color shellMainColor;
    [SerializeField] Color shellDecorColor;
    [SerializeField] CanvasGroup buttonCanvasGroup;

    public void SetShellData(CombinedShellData data)
    {
        shellData = data;
        if (shellData == null) return;

        shellNameText.text = shellData.shellName;
        shellDescriptionText.text = shellData.shellDescription;

        shellMainColor = shellData.mainColor;
        shellDecorColor = shellData.decorColor;

        shellIcon.color = shellMainColor;
        shellDecorImage.color = shellDecorColor;
    }
    public void ShowShellDescription()
    {
        if (shellData == null) return;
        buttonCanvasGroup.alpha = 1f;
        buttonCanvasGroup.interactable = true;
        buttonCanvasGroup.blocksRaycasts = true;

        shellDescriptionCanvasGroup.alpha = 1f;
        shellDescriptionCanvasGroup.interactable = true;
        shellDescriptionCanvasGroup.blocksRaycasts = true;
    }
    public void HideShellDescription()
    {
        shellDescriptionCanvasGroup.alpha = 0f;

        buttonCanvasGroup.alpha = 0f;
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;
        shellDescriptionCanvasGroup.interactable = false;
        shellDescriptionCanvasGroup.blocksRaycasts = false;
    }

    #region UI Interaction
    public void EquipShell()
    {
        Debug.Log("Equip Shell: " + shellData.shellName);
        if (shellData == null) return;

        PlayerEntity.Instance.playerShell.EquipShell(shellData);
        HideShellDescription();
    }
    public void UnEquipShell()
    {
        if (shellData == null) return;

        PlayerEntity.Instance.playerShell.UnEquippedShell();
        HideShellDescription();
    }

    #endregion
}