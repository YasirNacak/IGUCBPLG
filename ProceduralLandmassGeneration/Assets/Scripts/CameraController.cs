using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float scrollSpeed = 150f;

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        if (Input.GetKey("w"))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        pos.y -= scroll * scrollSpeed * Time.deltaTime;

        int lowerThreshold = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().meshHeightMultiplier * 10;
        if (GameObject.Find("MapGenerator").GetComponent<MapGenerator>().isCreating == false)
            pos.y = Mathf.Clamp(pos.y, lowerThreshold + lowerThreshold * 0.25f, lowerThreshold * 1.50f);
        transform.position = pos;
        transform.eulerAngles = rot;
    }
}
