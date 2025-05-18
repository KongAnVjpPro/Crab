using UnityEngine;
public class PlayerState : MyMonobehaviour
{
    public bool jumping = false;
    public bool dashing = false;
    public bool lookingUp = false;
    public bool lookingDown = false;
    public bool blocking = false;
    public bool lookingRight;
    public bool alive = true;
    public bool attacking = false;
    // public bool walkIntoNewScene = false;
    // public bool onGround;
    public bool invincible = false;
    [Header("Ability Unlocked: ")]
    public bool unlockedDash = false;
    public bool unlockedDoubleJump = false;
    public bool unlockedParry = false;

}