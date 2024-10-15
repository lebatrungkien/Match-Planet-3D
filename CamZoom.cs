using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamZoom : MonoBehaviour
{
    public Camera cam;
    public float zoomMinScale = 20f;
    public float zoomMaxScale = 40f;
    public float zoomDuration = 0.5f;
    public float zoomSensitivity = 10f;

    private float targetFov;
    private float currentVelocity = 0f;

    void Start()
    {

        targetFov = cam.fieldOfView;
    }

    void Update()
    {

        if (Input.touchCount == 2)
        {

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);


            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;


            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            targetFov += deltaMagnitudeDiff * zoomDuration;
            targetFov = Mathf.Clamp(targetFov, zoomMinScale, zoomMaxScale);
        }


        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFov, ref currentVelocity, zoomDuration);

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {

            targetFov -= scrollInput * zoomSensitivity;
            targetFov = Mathf.Clamp(targetFov, zoomMinScale, zoomMaxScale);
        }


        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFov, ref currentVelocity, zoomDuration);
    }

}