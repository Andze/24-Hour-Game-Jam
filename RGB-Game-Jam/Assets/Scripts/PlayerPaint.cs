using UnityEngine;
using System.Collections;

public class PlayerPaint : MonoBehaviour
{
    public float minPaintDistance = 0.1f;
    public Color brushColor = Color.white;
    public GameObject brushContainer;
    public Camera canvasCamera;

    private Vector3 positionLastPaint;
    private float brushSize = 0.05f;
    private int targetLayerMask = 0;

    void Start()
    {
        targetLayerMask = LayerMask.GetMask("Floor");
        Paint();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, positionLastPaint) > minPaintDistance)
            Paint();
    }

    private void Paint()
    {
        positionLastPaint = transform.position;

        Vector3 uvWorldPosition = Vector3.zero;
        if (GetUVWorldPosition(ref uvWorldPosition))
        {
            GameObject brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity"));
            brushObj.GetComponent<SpriteRenderer>().color = brushColor;
            brushObj.transform.parent = brushContainer.transform;
            brushObj.transform.localPosition = uvWorldPosition;
            brushObj.transform.localScale = Vector3.one * brushSize;
            brushObj.transform.localRotation = Quaternion.Euler(0f, 0, Random.Range(0f, 180f));
        }
    }

    private bool GetUVWorldPosition(ref Vector3 uvWorldPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.y * 2.0f, targetLayerMask))
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
