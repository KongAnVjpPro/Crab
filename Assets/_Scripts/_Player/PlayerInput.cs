using System.Linq.Expressions;
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
     cast,
     block,
     inventory;
    void GetInput()
    {
        inventory = Input.GetKeyDown(KeyCode.I);


        if (GameController.Instance.isBlockPlayerControl)
        {
            // xAxis = 0;
            // yAxis = 0;
            // attack = false;
            // block = false;
            // jumpStart = false;
            // jumpPress = false;
            // jumpEnd = false;
            // dash = false;
            // interact = false;
            // heal = false;
            // cast = false;
            BlockInput();
            return;
        }
        if (playerController.pState.alive == false)
        {
            BlockInput();
            return;
        }
        //Player controls
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        attack = Input.GetButtonDown("Attack");
        block = Input.GetButton("Block");


        jumpStart = Input.GetButtonDown("Jump");
        jumpPress = Input.GetButton("Jump");
        jumpEnd = Input.GetButtonUp("Jump");

        dash = Input.GetButtonDown("Dash");
        interact = Input.GetButtonDown("Interact");
        heal = Input.GetButton("Healing");
        cast = Input.GetButtonDown("CastSpell");


    }
    void BlockInput()
    {
        {
            xAxis = 0;
            yAxis = 0;
            attack = false;
            block = false;
            jumpStart = false;
            jumpPress = false;
            jumpEnd = false;
            dash = false;
            interact = false;
            heal = false;
            cast = false;


        }
    }
    void Update()
    {
        GetInput();
    }
}