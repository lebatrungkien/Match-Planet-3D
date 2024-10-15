using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MatchChecking : MonoBehaviour
{
    public LayerMask newLayer; 
    private Dictionary<GameObject, LayerMask> originalLayers = new Dictionary<GameObject, LayerMask>(); 

    [Header("Match Checking Position")]
    public Transform matchPointA;
    public Transform matchPointB;
    public Transform centerPoint;
    public Transform disappearPoint;

    public List<GameObject> pickedObjs = new List<GameObject>();
    public GameObject matchingObj1;
    public GameObject matchingObj2;

    public int collectedObjNum;

    private float _originalScale;

    void Start()
    {
        collectedObjNum = 0;
        _originalScale = GameManager.instance._constScale * GameManager.instance._scale;
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name);

        if (other.gameObject.layer != LayerMask.NameToLayer("NewLayer") && !pickedObjs.Contains(other.gameObject))
        {
            
            LayerMask originalLayer = other.gameObject.layer;
            if (!originalLayers.ContainsKey(other.gameObject))
            {
                originalLayers.Add(other.gameObject, originalLayer);
            }
            other.gameObject.layer = LayerMask.NameToLayer("NewLayer");

            pickedObjs.Add(other.gameObject);
        }

        if (pickedObjs.Count == 2)
        {
            if (pickedObjs[0].name == pickedObjs[1].name)
            {
                pickedObjs[0].GetComponent<BoxCollider>().enabled = false;
                pickedObjs[1].GetComponent<BoxCollider>().enabled = false;

                matchingObj1 = pickedObjs[0];
                matchingObj2 = pickedObjs[1];
                pickedObjs.Clear();

                // SoundManager.instance.PlayMatchSFX();
                SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_match);

                Invoke("mergingObjsAnim", 0.25f);
                Invoke("disappearAnim", 0.5f);
                Invoke("collectMatchedObjs", 1.0f);
            }
            else
            {
               
                foreach (var obj in pickedObjs)
                {
                    obj.GetComponent<MatchItem>().BackToEarthTransform();
                }
                pickedObjs.Clear();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: " + other.gameObject.name);

        if (pickedObjs.Contains(other.gameObject))
        {
            pickedObjs.Remove(other.gameObject);
        }

        if (originalLayers.ContainsKey(other.gameObject))
        {
            
            other.gameObject.layer = originalLayers[other.gameObject];
            originalLayers.Remove(other.gameObject);
        }
    }

    void collectMatchedObjs()
    {
        Destroy(matchingObj1);
        Destroy(matchingObj2);

        collectedObjNum++;
        GameManager.instance.CheckLevelCompleted(collectedObjNum);
    }

    void mergingObjsAnim()
    {
        matchingObj1.transform.DOScale(new Vector3(0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale), 0.25f);
        matchingObj2.transform.DOScale(new Vector3(0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale), 0.25f);

        matchingObj1.transform.DOMove(centerPoint.position, 0.25f);
        matchingObj2.transform.DOMove(centerPoint.position, 0.25f);
    }

    void disappearAnim()
    {
        matchingObj1.transform.DOScale(new Vector3(0f, 0f, 0f), 0.25f);
        matchingObj2.transform.DOScale(new Vector3(0f, 0f, 0f), 0.25f);

        matchingObj1.transform.DOMove(disappearPoint.position, 0.25f);
        matchingObj2.transform.DOMove(disappearPoint.position, 0.25f);
    }

    public Vector3 GetEmptyPos()
    {
        if (pickedObjs.Count == 0)
        {
            return matchPointA.position;
        }
        else
        {
            return matchPointB.position;
        }
    }
}

// public class MatchChecking : MonoBehaviour
// {
//     // Start is called before the first frame update
//     // public static MatchChecking instance = null;
//     public LayerMask newLayer; // Lớp mới sẽ gán cho đối tượng khi vào vùng kiểm tra
//     private Dictionary<GameObject, LayerMask> originalLayers = new Dictionary<GameObject, LayerMask>(); // Lưu lớp gốc của các đối tượng
//     [Header("Match Checking Position")]
//     public Transform matchPointA;
//     public Transform matchPointB;

//     public Transform centerPoint;

//     public Transform disappearPoint;

//     public List<GameObject> pickedObjs = new List<GameObject>();
//     public GameObject matchingObj1;
//     public GameObject matchingObj2;

//     // public float spawnScale = 0.2f;

//     // void Awake() {
//     //     if (instance == null) {
//     //         instance = this;
//     //     } else {
//     //         Destroy(gameObject);
//     //     }
//     // }
//     public int collectedObjNum;

//     private float _originalScale;

//     void Start()
//     {
//         collectedObjNum = 0;
//         _originalScale = GameManager.instance._constScale * GameManager.instance._scale;

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }


//     void OnTriggerEnter(Collider other)
//     {
//         Debug.Log("I'm in...");
//         if (pickedObjs.Count == 0)
//         {
//             // other.gameObject.tag = "check1";
//             // pickedObjs.Add(other.gameObject);
//             if (!pickedObjs.Contains(other.gameObject))
//             {
//                 // Lưu lớp gốc và gán lớp mới cho đối tượng
//                 LayerMask originalLayer = other.gameObject.layer;
//                 if (!originalLayers.ContainsKey(other.gameObject))
//                 {
//                     originalLayers.Add(other.gameObject, originalLayer);
//                 }
//                 other.gameObject.layer = LayerMask.NameToLayer("NewLayer");
//                 other.gameObject.tag = "check1";
//                 pickedObjs.Add(other.gameObject);
//             }


//         }
//         else if (other.gameObject.name.Contains(pickedObjs[0].name) == true)
//         {

//             pickedObjs.Add(other.gameObject);
//             pickedObjs[0].gameObject.GetComponent<BoxCollider>().enabled = false;
//             pickedObjs[1].gameObject.GetComponent<BoxCollider>().enabled = false;


//             matchingObj1 = pickedObjs[0].gameObject;
//             matchingObj2 = pickedObjs[1].gameObject;
//             pickedObjs.Clear();



//             // // Đặt kích thước mới cho các đối tượng
//             // matchingObj1.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
//             // matchingObj2.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);

//             // GameManager.instance._scoreValue += 10;
//             // GameManager.instance.SetTMPScore(); 

//             SoundManager.instance.PlayMatchSFX();

//             Invoke("mergingObjsAnim", 0.25f);
//             Invoke("disappearAnim", 0.5f);
//             Invoke("collectMatchedObjs", 1.0f);



//         }
//         else
//         {

//             other.gameObject.GetComponent<MatchItem>().BackToEarthTransform();
//         }
//     }

//     void OnTriggerExit(Collider other)
//     {
//         // Destroy(pickedObjs[0].gameObject);

//         if (other.gameObject.CompareTag("check1"))
//         {
//             other.gameObject.tag = "Untagged";
//             pickedObjs.Clear();

//         }

//         if (originalLayers.ContainsKey(other.gameObject))
//         {
//             // Khôi phục lớp gốc của đối tượng
//             other.gameObject.layer = originalLayers[other.gameObject];
//             originalLayers.Remove(other.gameObject);
//         }
//     }

//     void collectMatchedObjs()
//     {
//         Destroy(matchingObj1.gameObject);
//         Destroy(matchingObj2.gameObject);

//         // pickedObjs.Clear();
//         collectedObjNum++;
//         GameManager.instance.CheckLevelCompleted(collectedObjNum);
//     }

//     void mergingObjsAnim()
//     { // Total time = 0.25f

//         // Scale up
//         matchingObj1.gameObject.transform.DOScale(new Vector3(0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale), 0.25f);
//         matchingObj2.gameObject.transform.DOScale(new Vector3(0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale, 0.3f * GameManager.instance._scale), 0.25f);

//         // Merging
//         matchingObj1.gameObject.transform.DOMove(centerPoint.position, 0.25f);
//         matchingObj2.gameObject.transform.DOMove(centerPoint.position, 0.25f);
//     }

//     void disappearAnim()
//     { // Total time = 0.25f
//       // Scale down
//         matchingObj1.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.25f);
//         matchingObj2.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.25f);

//         // Merging
//         matchingObj1.gameObject.transform.DOMove(disappearPoint.position, 0.25f);
//         matchingObj2.gameObject.transform.DOMove(disappearPoint.position, 0.25f);
//     }

//     public Vector3 GetEmptyPos()
//     {
//         if (pickedObjs.Count == 0)
//         {
//             return matchPointA.position;
//         }
//         else
//         {
//             return matchPointB.position;
//         }
//     }
// }

