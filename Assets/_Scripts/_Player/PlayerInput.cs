using UnityEngine;
public class PlayerInput : PlayerComponent
{
    public float xAxis, yAxis;
    public bool attack,
     jumpPress,
     jumpStart,
     jumpEnd,
     dash,
     interact,
     heal,
     cast;
    void GetInput()
    {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        attack = Input.GetButtonDown("Attack");

        jumpStart = Input.GetButtonDown("Jump");
        jumpPress = Input.GetButton("Jump");
        jumpEnd = Input.GetButtonUp("Jump");

        dash = Input.GetButtonDown("Dash");
        interact = Input.GetButtonDown("Interact");
        heal = Input.GetButton("Healing");
        cast = Input.GetButtonDown("CastSpell");

    }
    void Update()
    {
        GetInput();
    }
}