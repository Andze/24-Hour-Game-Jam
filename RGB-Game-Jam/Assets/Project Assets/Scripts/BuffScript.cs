using UnityEngine;
using System;
using System.Collections;

public class BuffScript : MonoBehaviour
{
    public float cooldownTime = 30.0f;
    protected bool active = false;

    void Start()
    {
        if (!active)
            GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!active && other.gameObject.tag == "Player")
        {
            Instance(other.gameObject).Activate(cooldownTime);
            Destroy(this.gameObject);
        }
    }

    protected virtual void Activate(float cooldownTime)
    {
        active = true;
        this.cooldownTime = cooldownTime;
    }

    public virtual void Update()
    {
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
