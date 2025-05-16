using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PlayerShellController : PlayerComponent
{
    // [SerializeField] protected List<SpriteRenderer> shellModel;
    [Header("Shell shape configuration: ")]
    [SerializeField] SpriteRenderer triangle1;//main
    [SerializeField] SpriteRenderer triangle2;
    [SerializeField] SpriteRenderer triangle3;
    [SerializeField] SpriteRenderer triangle4;//main
    [SerializeField] Light2D lightLevel;
    [SerializeField] float maxIntensity = 10f;

    [Header("Shell data: ")]
    [SerializeField] CombinedShellData currentShell;
    public List<CombinedShellData> ownedShellList = new List<CombinedShellData>();
    [Header("Save Load data: ")]
    public List<string> shellSaveKey;//user for save load
    public string lastShellEquippedKey;


    [SerializeField] List<ShellSO> defaultShell;


    public Dictionary<ShellID, ShellSO> shellDict = new Dictionary<ShellID, ShellSO>();
    public Dictionary<string, CombinedShellData> combinedShellDict = new Dictionary<string, CombinedShellData>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDict();


        this.LoadOwnedShells();
        this.LoadCurrentShell();
        EquipShell(ownedShellList[0]);
    }

    void LoadOwnedShells()//load data
    {
        ownedShellList.Clear();

        foreach (var key in shellSaveKey)
        {
            List<ShellSO> baseShells = new List<ShellSO>();
            string[] parts = key.Split('_');

            foreach (var part in parts)
            {
                if (System.Enum.TryParse(part, out ShellID id) && shellDict.TryGetValue(id, out var shell))
                {
                    baseShells.Add(shell);
                }
                else
                {
                    Debug.LogWarning($"Invalid ShellID: {part}");
                }
            }

            if (baseShells.Count > 0)
            {
                var combined = new CombinedShellData(baseShells);
                ownedShellList.Add(combined);
                combinedShellDict[combined.GetKey()] = combined;
            }
        }
    }
    public bool TryCombineShell(CombinedShellData shellA, CombinedShellData shellB, out CombinedShellData result)
    {
        if (shellA == shellB)
        {
            result = null;
            return false;
        }
        var baseShells = shellA.baseShellIDs.Concat(shellB.baseShellIDs)
                                            .Distinct()
                                            .OrderBy(id => id)
                                            .ToList();

        string key = string.Join("_", baseShells);

        if (combinedShellDict.TryGetValue(key, out result))
        {
            return false;
        }


        List<ShellSO> baseShellSOs = baseShells
            .Where(id => shellDict.ContainsKey(id))
            .Select(id => shellDict[id])
            .ToList();

        if (baseShellSOs.Count != baseShells.Count)
        {
            Debug.LogWarning("thieu shellID");
            result = null;
            return false;
        }

        result = new CombinedShellData(baseShellSOs);
        combinedShellDict[key] = result;


        ownedShellList.Remove(shellA);
        ownedShellList.Remove(shellB);
        shellSaveKey.Remove(shellA.GetKey());
        shellSaveKey.Remove(shellB.GetKey());


        ownedShellList.Add(result);
        shellSaveKey.Add(result.GetKey());

        return true;
    }



    protected virtual void LoadCurrentShell()
    {
        EquipShell(currentShell);
    }
    protected virtual void LoadDict()
    {
        ShellSO[] sOs = Resources.LoadAll<ShellSO>("Shell");
        foreach (var shell in sOs)
        {
            shellDict[shell.id] = shell;
            // Debug.Log(shell.name);
        }
    }





    public void UnEquippedShell()
    {
        playerController.playerAbility.RemoveAbility(currentShell);
        currentShell = null;
        ActiveShell(false);
        //remove ability
        // playerController.playerAbility.UnlockAbilit
    }
    public void EquipShell(CombinedShellData shell)
    {
        if (shell == null) return;
        currentShell = shell;
        LoadShellVisual(shell);
        playerController.playerAbility.UnlockAbility(currentShell);
        //add ability
    }
    #region Graphic handle
    void ActiveShell(bool activeValue)
    {
        triangle1.gameObject.SetActive(activeValue);
        triangle2.gameObject.SetActive(activeValue);
        triangle3.gameObject.SetActive(activeValue);
        triangle4.gameObject.SetActive(activeValue);
        lightLevel.enabled = activeValue;
    }
    void LoadShellVisual(CombinedShellData shellEquipped)
    {
        if (shellEquipped == null) return;
        ActiveShell(true);
        triangle1.color = shellEquipped.mainColor;
        triangle2.color = shellEquipped.decorColor;
        triangle3.color = shellEquipped.decorColor;
        triangle4.color = shellEquipped.mainColor;
        lightLevel.intensity = Mathf.Clamp(shellEquipped.baseShellIDs.Count(), 1, maxIntensity);
        lightLevel.color = Color.Lerp(shellEquipped.mainColor, shellEquipped.decorColor, 0.5f);

        triangle1.transform.localScale = shellEquipped.triangle1Scale;
        triangle2.transform.localScale = shellEquipped.triangle2Scale;
        triangle3.transform.localScale = shellEquipped.triangle3Scale;
        triangle4.transform.localScale = shellEquipped.triangle4Scale;
    }
    #endregion
}