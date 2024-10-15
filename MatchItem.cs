using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MatchItem : MonoBehaviour
{
    // Start is called before the first frame update
    public MatchChecking MatchChecking;

    private Vector3 _normalVec;
    private Vector3 _hitPos;
    private Vector3 _scale;
    private Vector3 _checkingPos;

    public List<GameObject> spawnedObjs = new List<GameObject>();
    public Dictionary<string, int> QuestDic = new Dictionary<string, int>();


    void Start()
    {

        MatchChecking = GameObject.Find("MatchChecking").GetComponent<MatchChecking>();

        _checkingPos = new Vector3(0, 0, 0);


    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        // SoundManager.instance.PlaySelectSFX();
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_select);

        if (_checkingPos == new Vector3(0, 0, 0))
        {
            MoveToMatchPoint();
        }
        else
        {
            Debug.Log("back to earth...");
            BackToEarthTransform();
            _checkingPos = new Vector3(0, 0, 0);
        }

    }

    void MoveToMatchPoint()
    {
        _checkingPos = MatchChecking.GetEmptyPos();




        transform.DOJump(_checkingPos, 2f, 1, 0.5f);
        transform.DOScale(new Vector3(0.22f, 0.22f, 0.22f), 0.5f);
        // transform.DOScale(new Vector3(newScale, newScale, newScale), 0.5f);
        transform.DORotate(new Vector3(0f, 180f, 0f), 0.5f);
        transform.SetParent(MatchChecking.gameObject.transform);


        Debug.Log("clicked!");


        // Invoke("DestroyGO", 1.0f);
    }

    public void SetHitPos(Vector3 hitPos)
    {
        _hitPos = hitPos;
        transform.position = hitPos;
    }

    public void SetNormalVec(Vector3 normalVec)
    {
        _normalVec = normalVec;
        transform.up = normalVec;

    }
    public void SetScale(Vector3 scale)
    {
        _scale = scale;
        transform.localScale = scale;

    }

    public void BackToEarthTransform()
    {
        _checkingPos = new Vector3(0, 0, 0);

        transform.DOJump(_hitPos, 2f, 1, 0.5f);
        transform.DOScale(_scale, 0.5f);

        // transform.DORotate(ConvertNormalToEuler(_normalVec), 0.5f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, _normalVec);
        transform.DORotateQuaternion(rotation, 0.5f);

        transform.SetParent(GameObject.Find("MatchObjects").transform);



        Debug.Log("clicked!");
    }


    private Vector3 ConvertNormalToEuler(Vector3 up)
    {
        Quaternion rotation = Quaternion.LookRotation(up);
        Vector3 eulerAngles = rotation.eulerAngles;

        return eulerAngles;
    }

    // void OnCollisionEnter(Collision other){
    //     ContactPoint contact = other.contacts[0];
    //     Vector3 position = contact.point;
    // }
}

