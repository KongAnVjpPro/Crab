using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Shell", menuName = "Shell")]
public class ShellSO : ScriptableObject
{
    public ShellID id;
    public string shellName;
    public string shellDescription;
    public List<ShellAbilityID> abilityID;
    public Sprite spriteBase;
    [Header("Shell shape configure: ")]
    public Color decorColor;
    public Color mainColor;
    public Vector2 triangle1Scale;
    public Vector2 triangle2Scale;
    public Vector2 triangle3Scale;
    public Vector2 triangle4Scale;
}
public enum ShellID
{
    None = 0,
    DoubleJumpShell = 1,
    ParryShell = 2,
    DashShell = 3,

    UltimateShell = 100,
}
public enum ShellAbilityID
{
    None = 0,
    DoubleJump = 1,
    Parry = 2,
    Dash = 3,


}