using UnityEngine;
public class AttackableComponent : EntityComponent
{
    [SerializeField] protected float damage = 1;
    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        EntityController opponent = _collision.GetComponent<EntityController>();
        if (opponent == null)
        {
            return;
        }
        Attack(opponent);
    }
    protected virtual void Attack(EntityController opponent)
    {

    }
    protected virtual void ChangeDamage(float amount)
    {
        damage += amount;
    }
}