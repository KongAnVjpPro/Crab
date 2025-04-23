using Unity.VisualScripting;
using UnityEngine;
public class EnemyRotator : EnemyComponent
{
    public enum FlipDirection
    {
        Right = 0,
        Left = 1,
        Up = 2,
        Down = 3
    }
    public void Flip(FlipDirection dir)
    {
        switch (dir)
        {
            case FlipDirection.Right:
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case FlipDirection.Left:
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case FlipDirection.Up:
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                break;
            case FlipDirection.Down:
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
                break;
        }
    }
    public void RotateZ(Vector2 vectorDir)
    {
        float angle = Mathf.Atan2(vectorDir.y, vectorDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}