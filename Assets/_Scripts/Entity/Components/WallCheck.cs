using UnityEngine;
public class WallCheck : EntityComponent
{
    public Transform wallCheck;
    [SerializeField] protected LayerMask wallLayer;

    public bool Walled()
    {
        return (Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer));
    }
}