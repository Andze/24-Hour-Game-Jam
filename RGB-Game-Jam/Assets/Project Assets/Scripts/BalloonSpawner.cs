using UnityEngine;
using System.Collections;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject brushContainer;
    public Camera canvasCamera;
    public Object balloonPrefab;
    public float minSpawnWait = 5.0f, maxSpawnWait = 15.0f;

    private int targetLayerMask;
    private float timeUntilNextSpawn;

    void Start()
    {
        targetLayerMask = LayerMask.GetMask("Floor");
        timeUntilNextSpawn = Random.Range(minSpawnWait, maxSpawnWait);
    }

    void Update()
    {
        timeUntilNextSpawn -= Time.deltaTime;

        if (timeUntilNextSpawn <= 0.0f)
        {
            if (balloonPrefab)
            {
                GameObject tmpBaloon = (GameObject)Instantiate(balloonPrefab, GetNextSpawnPoint(), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                BalloonScript bScript = tmpBaloon.GetComponent<BalloonScript>();
                bScript.brushContainer = brushContainer;
                bScript.canvasCamera = canvasCamera;
                bScript.targetLayerMask = targetLayerMask;
            }

            timeUntilNextSpawn = Random.Range(minSpawnWait, maxSpawnWait);
        }
    }

    private Vector3 GetNextSpawnPoint()
    {
        Vector3 randomXY = new Vector3(Random.Range(-10.0f, 10.0f), 10.0f, Random.Range(-10.0f, 10.0f));

        RaycastHit hit;
        if (Physics.Raycast(randomXY, Vector3.down, out hit, 10.0f, targetLayerMask))
        {
            return hit.point;
        }
        return GetNextSpawnPoint();
    }
}
