using UnityEngine;
using System.Collections;

public class ParticlePlayer : MonoBehaviour
{
    public GameObject particleSystem;
    public Color color;
    
    private ParticleSystem particleInstance;

    void Start()
    {
        particleSystem.GetComponent<ParticleSystem>().startColor = color;
    }

    void Update()
    {
        if (particleInstance && particleInstance.isStopped)
        {
            Destroy(particleInstance.gameObject);
            particleInstance = null;
        }
    }

    public void Play(Color color)
    {
        GameObject temp = (GameObject)Instantiate(particleSystem, transform.position,
            Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        particleInstance = temp.GetComponent<ParticleSystem>();
        particleInstance.startColor = color;

        particleInstance.Play();
    }
}
