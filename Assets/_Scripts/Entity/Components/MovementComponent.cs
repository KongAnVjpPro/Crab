using UnityEngine;
public class MovementComponent : EntityComponent
{
    [SerializeField] protected float moveSpeed = 5f;

    bool canMove = true;

    public virtual void MoveHorizontal(Vector2 direction, float xAxis)
    {
        if (canMove)
        {
            entityController.rb.velocity = new Vector2(direction.x * moveSpeed * xAxis, entityController.rb.velocity.y);
        }
    }
}