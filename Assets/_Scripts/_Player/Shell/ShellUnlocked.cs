using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShellUnlocked : MyMonobehaviour
{
    [SerializeField] List<ShellSO> shellBase;
    [SerializeField] CombinedShellData shellProvided;
    [Header("Visualize: ")]
    [SerializeField] ParticleSystem unlockedParticle;
    [SerializeField] Animator shellAnim;
    [SerializeField] float timeUI = 8f;
    protected override void Awake()
    {
        base.Awake();
        LoadShellProvided();
        unlockedParticle.Stop();
    }
    protected virtual void LoadShellProvided()
    {
        // if (shellProvided != null) return;
        shellProvided = new CombinedShellData(shellBase);
    }

    public bool CanReceiveShell()
    {
        return true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTrigger = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTrigger = false;
    }
    void InteractShell()
    {
        if (!isTrigger) return;
        if (!PlayerEntity.Instance.playerInput.interact) return;
        if (!received)
        {
            GivePlayerAbility();
        }
        else
        {
            HandleShellStation();
        }

    }
    [SerializeField] bool isTrigger;
    [SerializeField] bool received = false;
    void Update()
    {
        InteractShell();
    }
    #region unlock ability
    void GivePlayerAbility()
    {
        if (received) return;

        StartCoroutine(UnlockedSkill());


    }
    IEnumerator UnlockedSkill()
    {
        GameController.Instance.isBlockPlayerControl = true;
        PlayerEntity.Instance.playerShell.ownedShellList.Add(shellProvided);
        received = true;
        unlockedParticle.Play();
        UIEntity.Instance.uiUnlockedShell.UnlockedShellUI(timeUI, shellProvided.shellName);
        shellAnim.SetBool("Activate", true);
        yield return new WaitForSeconds(timeUI);
        if (unlockedParticle.IsAlive())
        {
            unlockedParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        shellAnim.SetBool("Activate", false);
        GameController.Instance.isBlockPlayerControl = false;
    }
    #endregion
    public void HandleShellStation()
    {

    }
}