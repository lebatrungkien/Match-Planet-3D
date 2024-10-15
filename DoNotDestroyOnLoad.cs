using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class donotdestroyonload : MonoBehaviour
{
    // Start is called before the first frame update
    public static donotdestroyonload d = null;

    void Awake()
    {
        if (d == null)
        {
            d = this.gameObject.GetComponent<donotdestroyonload>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //	gameObject.transform.GetComponent<admobads> ().OnDestroybanner ();
            if (this != d)
            {
                print("Occur");
                Destroy(this.gameObject);
            }
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
