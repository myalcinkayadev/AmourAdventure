using UnityEngine;

public class PlayerAnimations : CharacterAnimations
{
    public void ResetAnimation()
    {
        SetMoveAnimation(Vector2.down);
        SetReviveAnimation();
    }
}
