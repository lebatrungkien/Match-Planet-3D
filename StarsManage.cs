// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UI;

// public class StarsManage : MonoBehaviour
// {
//     public GameObject star1Full;
//     public GameObject star1Empty;
//     public GameObject star2Full;
//     public GameObject star2Empty;
//     public GameObject star3Full;
//     public GameObject star3Empty;


//     private float totalTime;
//     private float timeRemaining;
//     void Start()
//     {
//         totalTime = GameManager.instance.GetLevelTime();
//         timeRemaining = totalTime;
//         Debug.Log("Total time: " + totalTime);
//         Debug.Log("Time Remaining: " + timeRemaining);
//         UpdateStars();
//     }

//     void Update()
//     {

//         timeRemaining -= Time.deltaTime;
//         if (timeRemaining <= 0)
//         {
//             timeRemaining = 0;
//         }

//         UpdateStars();
//     }

//     void UpdateStars()
//     {
//         float ratio = timeRemaining / totalTime;
//         int stars = 0;
//         if (ratio > 0.66f)
//         {

//             star1Full.SetActive(true);
//             star1Empty.SetActive(false);

//             star2Full.SetActive(true);
//             star2Empty.SetActive(false);

//             star3Full.SetActive(true);
//             star3Empty.SetActive(false);

//             stars = 3;
//         }
//         else if (ratio > 0.33f)
//         {

//             star1Full.SetActive(true);
//             star1Empty.SetActive(false);

//             star2Full.SetActive(true);
//             star2Empty.SetActive(false);

//             star3Full.SetActive(false);
//             star3Empty.SetActive(true);

//             stars = 2;
//         }
//         else
//         {
//             star1Full.SetActive(false);
//             star1Empty.SetActive(true);

//             star2Full.SetActive(true);
//             star2Empty.SetActive(false);

//             star3Full.SetActive(false);
//             star3Empty.SetActive(true);

//             stars = 1;
//         }
//         string levelKey = "Level" + GameManager.instance.GetCurrentLevel();
//         PlayerPrefs.SetInt(levelKey + "_Stars", stars);
//         PlayerPrefs.Save();
//     }
// }
using System;
using UnityEngine;

public class StarsManage : MonoBehaviour
{
    public GameObject[] fullStars;
    public GameObject[] emptyStars;

    private float totalTime;
    private float timeRemaining;

    void Start()
    {
        totalTime = GameManager.instance.GetLevelTime();
        timeRemaining = totalTime;
        Debug.Log("Total time: " + totalTime);
        Debug.Log("Time Remaining: " + timeRemaining);
        UpdateStars();
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }

        UpdateStars();
    }

    void UpdateStars()
    {
        float ratio = timeRemaining / totalTime;
        int stars = GetStarsFromRatio(ratio);

       
        for (int i = 0; i < 3; i++)
        {
            if (i < stars)
            {
                fullStars[i].SetActive(true);
                emptyStars[i].SetActive(false);
            }
            else
            {
                fullStars[i].SetActive(false);
                emptyStars[i].SetActive(true);
            }
        }

       
        string levelKey = "Level" + GameManager.instance.GetCurrentLevel();
        PlayerPrefs.SetInt(levelKey + "_Stars", stars);
        PlayerPrefs.Save();
    }

    int GetStarsFromRatio(float ratio)
    {
        if (ratio > 0.66f)
            return 3;
        else if (ratio > 0.33f)
            return 2;
        else
            return 1;
    }
}
