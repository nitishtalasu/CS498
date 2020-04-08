using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public int minTargets;
    public int maxTargets;

    // Start is called before the first frame update
    void Start()
    {
        // maxTargets = Mathf.Max(maxTargets, minTargets + 2);
        if(maxTargets == 0)
        {
            maxTargets = 10;
        }
        int targetsToBeSpawned = Random.Range(minTargets, maxTargets);
        Debug.Log("Max targets are: " + maxTargets);
        Debug.Log("Targets spawned are: " + targetsToBeSpawned);
        Vector3 camPosition = Camera.main.transform.position;
        for (int i = 0; i < maxTargets; i++)
        {
            float x = Random.Range(camPosition.x - 100, camPosition.x + 1000);
            float z = Random.Range(camPosition.z - 100, camPosition.z + 1000);
            float minY = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
            float y = Random.Range(minY + 100, minY + 300);
            Instantiate(targetPrefab, new Vector3(x, y, z), Quaternion.identity, this.transform);
        }
    }
}
