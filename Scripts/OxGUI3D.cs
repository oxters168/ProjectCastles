using UnityEngine;
using System.Collections;

public class OxGUI3D : MonoBehaviour {

    private GameObject movementHelper;
    private Vector3 cameraDifference, originalScale;

	// Use this for initialization
	protected virtual void Start ()
    {
        movementHelper = new GameObject("MovementHelp");
        movementHelper.transform.position = transform.position;
        movementHelper.transform.forward = Camera.main.transform.up;
        transform.parent = movementHelper.transform;

        cameraDifference = Camera.main.transform.position - transform.position;
        originalScale = transform.localScale;
        
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        UpdatePosition();
        UpdateScale();
	}

    protected virtual void UpdatePosition()
    {
        Vector3 cameraOffset = Camera.main.transform.position + (Camera.main.transform.forward * cameraDifference.magnitude);

        movementHelper.transform.position = cameraOffset;
        movementHelper.transform.forward = Camera.main.transform.up;
    }
    protected virtual void UpdateScale()
    {
        float originalDistance = cameraDifference.magnitude;
        float currentDistance = Vector3.Distance(Camera.main.transform.position, transform.position);

        transform.localScale = Vector3.one * ((currentDistance - originalDistance) / originalDistance) + originalScale;
    }
}
