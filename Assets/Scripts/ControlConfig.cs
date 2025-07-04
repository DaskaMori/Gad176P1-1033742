using UnityEngine;

public struct ControlsBinding
{
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Interact;
    public KeyCode Attack;

    public ControlsBinding(KeyCode up, KeyCode down, KeyCode left, KeyCode right, KeyCode interact, KeyCode attack)
    {
        Up = up;
        Down = down;
        Left = left;
        Right = right;
        Interact = interact;
        Attack = attack;
    }
}

public static class ControlsConfig
{
    public static ControlsBinding GetBinding(PlayerID player)
    {
        switch (player)
        {
            case PlayerID.One:
                return new ControlsBinding(
                    KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.G, KeyCode.H
                );
            case PlayerID.Two:
                return new ControlsBinding(
                    KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Slash, KeyCode.Period
                );
            default:
                throw new System.ArgumentException("Unsupported player ID");
        }
    }
}