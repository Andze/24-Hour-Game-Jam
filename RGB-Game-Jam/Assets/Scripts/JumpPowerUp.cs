using UnityEngine;
using System.Collections;

public class JumpPowerUp : BuffScript
{
    public float boostedJumpMultiplier = 1.75f;
    private float originalJump = 0.0f;

    protected override void Activate(float cooldownTime)
    {
        base.Activate(cooldownTime);

        originalJump = GetComponent<PlayerMove>().jumpForce;
        GetComponent<PlayerMove>().jumpForce *= boostedJumpMultiplier;
    }

    protected override void Destruct()
    {
        GetComponent<PlayerMove>().jumpForce = originalJump;

        base.Destruct();
    }

    protected override BuffScript Instance(GameObject other)
    {
        return other.AddComponent<JumpPowerUp>();
    }
}
