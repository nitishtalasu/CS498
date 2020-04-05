using UnityEngine;

public class Fly : MonoBehaviour
{

    public float acceleration = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = transform.position - Vector3.back * 10.0f - Vector3.left * 10.0f;
        Camera.main.transform.LookAt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newCamPosition = transform.position - transform.forward * 50.0f + Vector3.up * 5.0f;
        float bias = 0.96f;
        //Debug.Log("cam pos: " + Camera.main.transform.position);
        Camera.main.transform.position = Camera.main.transform.position * bias + newCamPosition * (1.0f - bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);
        transform.position += transform.forward * Time.deltaTime * acceleration;
        acceleration -= transform.forward.y * Time.deltaTime * 50.0f;

        if (acceleration < 50.0f)
        {
            acceleration = 50.0f;
        }
        transform.Rotate(-Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        if (terrainHeight > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, terrainHeight + 100.0f, transform.position.z);
        }
    }
}
