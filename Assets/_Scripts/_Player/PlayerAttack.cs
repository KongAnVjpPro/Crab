using UnityEngine;
public class PlayerAttack : PlayerComponent
{
    [SerializeField] float damage = 1f;
    [SerializeField] float attackCD = 1f;
    [SerializeField] float currentCD = 0;

    public void Attack()
    {

        if (currentCD >= attackCD)
        {
            Debug.Log("Attack");
            currentCD = 0;
        }

    }
    void Update()
    {
        currentCD += Time.deltaTime;
        if (playerController.playerInput.attack)
        {
            Attack();
        }
    }

}