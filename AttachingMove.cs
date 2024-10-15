using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachingMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject planet;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = planet.transform.position;
        gameObject.transform.rotation = planet.transform.rotation;
    }
}
