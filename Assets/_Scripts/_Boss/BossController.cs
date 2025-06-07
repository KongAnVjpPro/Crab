using System.Collections.Generic;
using UnityEngine;
public class BossController : EnemyEntity
{
    public BossType bossType;
    public bool isDefeated = false;
    // public LayerMask enemyMask; // thay mask khi noi chuyen xong
    // public LayerMask defaultMask;
    // public List<EnemyEntity> minions;//disable, enable khi goi skill, dung 1 lan
    public string bossName;
    protected override void Awake()
    {
        base.Awake();

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCanAppear();
    }
    protected virtual void LoadCanAppear()
    {
        // if (bossType == BossType.SeaWeed)
        // {
        //     isDefeated = SaveSystem.Instance.isSeawWeedDefeated;
        // }
        // else if (bossType == BossType.JellyFish)
        // {
        //     isDefeated = SaveSystem.Instance.isJellyFishDefeated;
        // }
        // else if (bossType == BossType.Crab)
        // {
        //     isDefeated = SaveSystem.Instance.isCrabDefeated;
        // }
        // else if (bossType == BossType.Final)
        // {
        //     isDefeated = SaveSystem.Instance.isFinalBossDefeated;
        // }
        isDefeated = SaveSystem.Instance.GetBossDefeated(bossType);
    }
    void DisableBoss()
    {
        //do something like set anim or disable collider,...
        gameObject.SetActive(false);
    }
    public void Defeated()
    {
        isDefeated = true;
        SaveSystem.Instance.SetBossDefeated(bossType, isDefeated);
        // SaveSystem.Instance.SaveBossDefeated();
        // SaveSystem.Instance.SavePlayerData();
        SaveSystem.Instance.SaveGlobalData();
        DisableBoss();
    }


}
public enum BossType
{
    SeaWeed = 0,
    JellyFish = 1,
    Crab = 2,
    Final = 3
}