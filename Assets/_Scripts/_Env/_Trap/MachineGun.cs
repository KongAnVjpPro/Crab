using System.Collections.Generic;
using UnityEngine;
public class MachineGun : MyMonobehaviour
{
    [SerializeField] RangeAttackSpawner bulletSpawner;
    [SerializeField] float bulletPerSecond = 2f;
    [SerializeField] Animator machineGunAnim;
    [SerializeField] Transform spawnPos;
    [SerializeField] bool canFire = true;
    [SerializeField] List<SpriteRenderer> wires;
    [SerializeField] Material defaultMat;
    [SerializeField] Material wireMat;
    // [SerializeField] Animator levelAnim;
    void Fired()//embeded to animator
    {
        bulletSpawner.Spawn(spawnPos.position, transform.right);
    }
    void Update()
    {
        if (canFire)
        {
            machineGunAnim.speed = bulletPerSecond;
        }
    }
    public void DeactiveMachine()
    {
        canFire = false;
        machineGunAnim.SetBool("Fire", false);
        // levelAnim?.SetBool("IsOpened", true);
        foreach (var wire in wires)
        {
            wire.material = defaultMat;
        }
    }
    public void ActivateMachine()
    {
        canFire = true;
        machineGunAnim.SetBool("Fire", true);
        // levelAnim?.SetBool("IsOpened", false);
        foreach (var wire in wires)
        {
            wire.material = wireMat;
        }
    }
    public void LevelSwitch()
    {
        if (canFire)
        {
            DeactiveMachine();
        }
        else
        {
            ActivateMachine();
        }
    }
}