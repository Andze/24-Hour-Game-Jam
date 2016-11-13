using UnityEngine;
using System.Collections;

public class SpeedBoost : BuffScript
{
    public float boostedSpeed = 15.0f;
    private float originalSpeed = 0.0f;
    
    protected override void Activate(float cooldownTime)
    {
        base.Activate(cooldownTime);

        originalSpeed = GetComponent<PlayerMove>().moveSpeed;
        GetComponent<PlayerMove>().moveSpeed = boostedSpeed;
    }
    
    protected override void Destruct()
    {
        GetComponent<PlayerMove>().moveSpeed = originalSpeed;

        base.Destruct();
    }

    protected override BuffScript Instance(GameObject other)
    {
        return other.AddComponent<SpeedBoost>();
    }
}
