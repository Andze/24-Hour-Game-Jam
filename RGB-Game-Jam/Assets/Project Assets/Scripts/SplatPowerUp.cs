using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplatPowerUp : BuffScript
{
    public Image uiSplat;
    private Color currentColor;
    private float startCooldown = 0.0f;
    private float obscurityTime;
    
    protected override void Activate(float cooldownTime)
    {
        uiSplat.color = Color.white;
        currentColor = uiSplat.color;
        startCooldown = cooldownTime;
        obscurityTime = startCooldown * 0.8f;

        base.Activate(cooldownTime);
    }

    public override void Update()
    {
        base.Update();

        if (active)
        {
            currentColor.a = cooldownTime / (startCooldown - obscurityTime);
            uiSplat.color = currentColor;
        }
    }

    protected override void Destruct()
    {
        uiSplat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        base.Destruct();
    }

    protected override BuffScript Instance(GameObject other)
    {
        SplatPowerUp spu = other.AddComponent<SplatPowerUp>();
        spu.uiSplat = uiSplat;

        return spu;
    }
}
