using Unity.VisualScripting;
using UnityEngine;
public class GroundCheck : EntityComponent
{
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float groundCheckY = 0.2f;
    [SerializeField] Transform groundCheckX;
    Vector3 offset;
    Vector3 leftCheck;
    Vector3 rightCheck;
    void CalcValue()
    {
        offset = new Vector3(groundCheckX.position.x - groundCheckPoint.position.x, 0);
        leftCheck = groundCheckPoint.position - offset;
        rightCheck = groundCheckPoint.position + offset;
    }
    public bool Grounded()
    {
        CalcValue();
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) ||
        Physics2D.Raycast(rightCheck, Vector2.down, groundCheckY, whatIsGround) ||
        Physics2D.Raycast(leftCheck, Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}