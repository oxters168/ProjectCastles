using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public Transform target;
    //public Vector3 defaultPosition = new Vector3(0, 5000, 0);
    //public Quaternion defaultRotation = Quaternion.Euler(90, 0, 0);
    public float distance = 300f;
    public float rotationStrength = 0.25f, scrollStrength = 25f;
    public bool blockControl = true, invertX = false, invertY = false, invertZoom = false;
    private bool mouseDown = false;
    private Vector3 initialMouse = Vector3.zero, previousMouse = Vector3.zero;
    private int firstTouch = -1, secondTouch = -1;
    private float fingerDistance = 0;
    public float movementDeadZone = 25f, zoomDeadZone = 1f;
    public bool moved = false, zoomed = false;

	void Start ()
    {
	}
	
	void FixedUpdate ()
    {
        float horizontal = 0, vertical = 0, scroll = 0;

        if (!blockControl)
        {
            #region Touch Controls
            if (Input.touches.Length >= 2)
            {
                Vector2 firstPosition = Input.touches[0].position;
                Vector2 secondPosition = Input.touches[1].position;

                if (firstTouch < 0 || secondTouch < 0 || firstTouch != Input.touches[0].fingerId || secondTouch != Input.touches[1].fingerId)
                {
                    firstTouch = Input.touches[0].fingerId;
                    secondTouch = Input.touches[1].fingerId;
                    fingerDistance = (firstPosition - secondPosition).magnitude;
                    zoomed = false;
                }
                else
                {
                    if (zoomed || (firstPosition - secondPosition).magnitude - fingerDistance > zoomDeadZone)
                    {
                        scroll += (firstPosition - secondPosition).magnitude - fingerDistance;
                        fingerDistance = (firstPosition - secondPosition).magnitude;
                        zoomed = true;
                    }
                }
            }
            else if (Input.touches.Length >= 1)
            {
                if (firstTouch < 0)
                {
                    firstTouch = Input.touches[0].fingerId;
                    previousMouse = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0);
                    moved = false;
                }
                if (firstTouch > -1)
                {
                    if (moved || Mathf.Abs(Input.touches[0].position.x - previousMouse.x) > movementDeadZone || Mathf.Abs(Input.touches[0].position.y - previousMouse.y) > movementDeadZone)
                    {
                        horizontal = Input.touches[0].position.x - previousMouse.x;
                        vertical = previousMouse.y - Input.touches[0].position.y;
                        previousMouse = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0);
                        moved = true;
                    }
                }
            }
            else { firstTouch = -1; secondTouch = -1; }
            #endregion

            #region Mouse Controls
            if (Input.touches.Length <= 0)
            {
                if (!mouseDown && Input.GetMouseButton(0))
                {
                    moved = false;
                    mouseDown = true;
                    initialMouse = Input.mousePosition;
                    previousMouse = Input.mousePosition;
                }
                if (!Input.GetMouseButton(0))
                {
                    moved = false;
                    mouseDown = false;
                }
                if (mouseDown && (initialMouse - Input.mousePosition).magnitude > movementDeadZone) moved = true;
                if (mouseDown && moved)
                {
                    horizontal = Input.mousePosition.x - previousMouse.x;
                    vertical = previousMouse.y - Input.mousePosition.y;
                    previousMouse = Input.mousePosition;
                }
            }
            if (target == null) scroll += Input.mouseScrollDelta.y;
            else scroll += -Input.mouseScrollDelta.y;
            #endregion
        }
        #region Add Buffs
        if (invertZoom) scroll *= -1;
        if (invertX) horizontal *= -1;
        if (invertY) vertical *= -1;
        scroll *= scrollStrength;
        horizontal *= rotationStrength;
        vertical *= rotationStrength;
        #endregion

        if (target != null)
        {

            if (distance + scroll <= 0) distance = 5;
            else distance += scroll;

            #region Third Person View
            //if (distance > 0)
            //{
                Camera.main.transform.rotation *= Quaternion.AngleAxis(horizontal, Vector3.up) * Quaternion.AngleAxis(vertical, Vector3.right);
                Camera.main.transform.position = target.position - (Camera.main.transform.forward * distance);
                Camera.main.transform.rotation = Quaternion.LookRotation((target.position - Camera.main.transform.position).normalized);
            //}
            #endregion
        }
        else
        {
            Camera.main.transform.rotation = (Quaternion.AngleAxis(horizontal, Vector3.up) * Camera.main.transform.rotation) * Quaternion.AngleAxis(vertical, Vector3.right);
            Camera.main.transform.position += Camera.main.transform.forward * scroll;
        }
	}
}
