// using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlanetItem : MonoBehaviour
{
    public float rotationSpeed = 50f;
    bool _isDragging;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("touch supported" + Input.touchSupported);
    }

    void OnMouseDrag()
    {
        _isDragging = true;
        Debug.Log("draginggg");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            Debug.Log("not drag");
        }
    }

    void FixedUpdate()
    {
        if (_isDragging)
        {
            float x = Input.GetAxis("Mouse X") * rotationSpeed;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Touch touch = Input.GetTouch(0);
            // float touchDeltaX = touch.deltaPosition.x;
            // float touchDeltaY = touch.deltaPosition.y;


            // float x = touchDeltaX * rotationSpeed * Time.fixedDeltaTime;
            // float y = touchDeltaY * rotationSpeed * Time.fixedDeltaTime;

            rb.AddTorque(Vector3.down * x);
            rb.AddTorque(Vector3.right * y);
        }

    }

    // void FixedUpdate()
    // {
    //     Debug.Log("input rb" + Input.touchCount + rb);

    //     if (Input.touchSupported)
    //     {
    //         Debug.Log("TOUCH IS SUPPORTED");
    //         if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
    //         {
    //             print("touched screen");
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("TOUCH IS NOT SUPPORTED");
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             print("clicked screen");
    //         }
    //     }

    //     if (Input.touchCount > 0 && _isDragging)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         float touchDeltaX = touch.deltaPosition.x;
    //         float touchDeltaY = touch.deltaPosition.y;


    //         float x = touchDeltaX * rotationSpeed * Time.fixedDeltaTime;
    //         float y = touchDeltaY * rotationSpeed * Time.fixedDeltaTime;

    //         rb.AddTorque(Vector3.down * x);
    //         rb.AddTorque(Vector3.right * y);

    //         Debug.Log("rb" + rb);
    //     }
    // }
}

