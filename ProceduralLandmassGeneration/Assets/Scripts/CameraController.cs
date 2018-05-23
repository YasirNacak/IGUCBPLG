using UnityEngine;

public class CameraController : MonoBehaviour {
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 150f;

	void Update () {
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        if (Input.GetKey("w") /*|| Input.mousePosition.y >= Screen.height - panBorderThickness*/){
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") /*|| Input.mousePosition.y <= panBorderThickness*/){
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") /*|| Input.mousePosition.x >= Screen.width - panBorderThickness*/){
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") /*|| Input.mousePosition.x <= panBorderThickness*/){
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        pos.y -= scroll * scrollSpeed * Time.deltaTime;

        int lowerThreshold = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().meshHeightMultiplier * 10;
        if(GameObject.Find("MapGenerator").GetComponent<MapGenerator>().isCreating == false)
            pos.y = Mathf.Clamp(pos.y, lowerThreshold + lowerThreshold * 0.25f, lowerThreshold * 2);
        transform.position = pos;
        transform.eulerAngles = rot;
	}
}
