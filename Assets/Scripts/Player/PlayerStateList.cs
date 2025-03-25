using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateList : MyMonobehaviour
{
    public bool jumping = false;
    public bool dashing = false;
    public bool recoilingX, recoilingY;
    public bool lookingRight;
    public bool invincible;
    public bool healing;
    public bool casting;
    public bool cutscene = false;
    public bool alive = true;
}
