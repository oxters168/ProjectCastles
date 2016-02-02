using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    public float speed = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 movement = Vector3.zero;

	    if(Input.GetKey(KeyCode.W))
        {
            movement += Vector3.Cross(Vector3.up, Vector3.Cross(Camera.main.transform.forward, Camera.main.transform.up)) * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.Cross(Camera.main.transform.forward, Camera.main.transform.up) * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.Cross(Vector3.Cross(Camera.main.transform.forward, Camera.main.transform.up), Vector3.up) * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.Cross(Camera.main.transform.up, Camera.main.transform.forward) * speed;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            movement += Vector3.up * speed;
        }

        transform.position += movement;
	}
}
