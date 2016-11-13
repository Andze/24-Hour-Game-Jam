using UnityEngine;
using System.Collections;

public class BalloonScript : MonoBehaviour
{
    public GameObject brushContainer;
    public Camera canvasCamera;
    public int targetLayerMask;

    private float inflationRate;
    private float maxScale;
    private Color brushColor;

    void Start()
    {
        inflationRate = Random.Range(0.005f, 0.05f);
        maxScale = Random.Range(4.0f, 7.5f);
        brushColor = GetComponent<MeshRenderer>().material.color;
    }

    void Update()
    {
        transform.localScale += FloatToVec(inflationRate);

        if (transform.localScale.x > maxScale)
            Pop();
    }

    private void Pop()
    {
        // Paint
        Vector3 uvWorldPosition = Vector3.zero;
        if (GetUVWorldPosition(ref uvWorldPosition))
        {
            GameObject brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity"));
            brushObj.GetComponent<SpriteRenderer>().color = brushColor;
            brushObj.transform.parent = brushContainer.transform;
            brushObj.transform.localPosition = uvWorldPosition;
            brushObj.transform.localScale = Vector3.one * (maxScale / 50.0f);
            brushObj.transform.localRotation = Quaternion.Euler(0f, 0, Random.Range(0f, 180f));
        }

        // Destroy this
        DestroyImmediate(gameObject);
    }

    private Vector3 FloatToVec(float value)
    {
        return new Vector3(value, value, value);
    }

    private bool GetUVWorldPosition(ref Vector3 uvWorldPosition)
    {
        Vector3 yOffsetPosition = transform.position + new Vector3(0.0f, transform.localScale.y, 0.0f);

        RaycastHit hit;
        if (Physics.Raycast(yOffsetPosition, Vector3.down, out hit, 20.0f, targetLayerMask))
        {
            MeshCollider mC = hit.collider as MeshCollider;
            if (mC == null || mC.sharedMesh == null)
                return false;

            Vector2 pixelUV = new Vector3(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - canvasCamera.orthographicSize;
            uvWorldPosition.y = pixelUV.y - canvasCamera.orthographicSize;
            uvWorldPosition.z = 0.0f;
            return true;
        }
        else return false;
    }
}
