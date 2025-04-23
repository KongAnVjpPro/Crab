using UnityEngine;
public class MovementComponent : EntityComponent
{
    // [SerializeField] protected float moveSpeed = 5f;

    [SerializeField] protected bool canMoveHorizontal = true;
    [SerializeField] protected bool canMoveVertical = true;

    public virtual void MoveHorizontal(Vector2 direction, float xAxis, float moveSpeed)
    {
        if (canMoveHorizontal)
        {
            entityController.rb.velocity = new Vector2(direction.x * moveSpeed * xAxis, entityController.rb.velocity.y);
        }
    }
    public virtual void MoveVertical(Vector2 direction, float yAxis, float moveSpeed)
    {
        if (canMoveVertical)
        {
            entityController.rb.velocity = new Vector2(entityController.rb.velocity.x, direction.y * moveSpeed * yAxis);
        }
    }
}