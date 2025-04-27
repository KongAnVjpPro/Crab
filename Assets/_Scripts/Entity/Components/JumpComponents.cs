using UnityEngine;
public class JumpComponent : EntityComponent
{
    [SerializeField] protected float jumpForce = 5f;
    protected virtual void Jump() // dinh nghia lai o enemy va player
    {

    }
}