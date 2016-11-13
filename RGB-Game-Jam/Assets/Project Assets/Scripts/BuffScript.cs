using UnityEngine;
using System;
using System.Collections;

public class BuffScript : MonoBehaviour
{
    public float cooldownTime = 30.0f;
    protected bool active = false;
    private float respawnTime = 0.0f;
    private MeshRenderer renderer;

    void Start()
    {
        if (!active)
            GetComponent<Collider>().isTrigger = true;

        renderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (respawnTime == 0.0f && !active && other.gameObject.tag == "Player")
        {
            Instance(other.gameObject).Activate(cooldownTime);

            respawnTime = 10.0f;
            renderer.enabled = false;
        }
    }

    protected virtual void Activate(float cooldownTime)
    {
        active = true;
        this.cooldownTime = cooldownTime;
    }

    public virtual void Update()
    {
        if (respawnTime != 0.0f)
        {
            respawnTime -= Time.deltaTime;

            if (respawnTime <= 0.0f)
            {
                respawnTime = 0.0f;
                renderer.enabled = true;
            }
        }

        if (active)
        {
            cooldownTime -= Time.deltaTime;

            if (cooldownTime <= 0.0f)
                Destruct();
        }
    }

    protected virtual void Destruct()
    {
        DestroyImmediate(this);
    }

    protected virtual BuffScript Instance(GameObject other)
    {
        return other.AddComponent<BuffScript>();
    }
}
