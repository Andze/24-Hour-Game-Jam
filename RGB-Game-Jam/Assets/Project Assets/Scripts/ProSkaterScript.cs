using UnityEngine;
using System.Collections;

public class ProSkaterScript : MonoBehaviour
{
    public float variance;
    public float floatSpeed;
    public Vector3 rotation;
    public float inactiveRamp = 0.5f;

    private float t = 0.5f;
    private Vector3 lowerBound;
    private Vector3 upperBound;
    private BoxCollider bC;

    void Start()
    {
        lowerBound = transform.position - new Vector3(0, variance, 0);
        upperBound = transform.position + new Vector3(0, variance, 0);
        
        bC = GetComponent<BoxCollider>();
    }

    void Update()
    {
        t += floatSpeed;

        transform.position = new Vector3(transform.position.x,
            Mathf.Lerp(lowerBound.y, upperBound.y, t), transform.position.z);

        if (t >= upperBound.y || t <= lowerBound.y)
            floatSpeed *= -1;

        transform.Rotate(rotation);
    }
}

