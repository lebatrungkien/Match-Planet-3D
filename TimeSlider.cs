using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeSlider : MonoBehaviour
{
    public static TimeSlider instance = null;
    public Slider timeSlider;
    public float totalTime;
    private float currentTime;

    public GameObject[] stars;
    private float segmentValue;

    private bool isTimeRunning = true;


    void Awake()
    {
        // PlayerPrefs.SetInt("level", 1);
        // levelvalue = PlayerPrefs.GetInt("level", 1);

        if (instance == null)
        {
            instance = this;
            Debug.Log("Not Destroy...");

        }
        else
        {
            Debug.Log("Destroying...");
            Destroy(this);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            totalTime = GameManager.instance.GetLevelTime();
        }
        else
        {
            Debug.LogError("GameManager instance is not available.");
            return;
        }

        Debug.Log("Total time " + totalTime);
        Debug.Log("Total time " + totalTime);
        currentTime = totalTime;
        timeSlider.maxValue = totalTime;
        timeSlider.value = totalTime;

        segmentValue = totalTime / stars.Length;

        foreach (GameObject star in stars)
        {
            SetStarFull(star);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // currentTime -= Time.deltaTime;
        // timeSlider.value = currentTime;

        // UpdateStars();

        // if (currentTime <= 0)
        // {
        //     currentTime = 0;

        // }
        if (isTimeRunning) 
        {
            currentTime -= Time.deltaTime;
            timeSlider.value = currentTime;

            UpdateStars();

            if (currentTime <= 0)
            {
                currentTime = 0;
                StopTime(); 
            }
        }
    }

    void UpdateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
           
            float starThreshold = totalTime * (1 - (i + 1) / (float)stars.Length);

            if (currentTime <= starThreshold)
            {
                SetStarEmpty(stars[i]);
            }
            else
            {
                SetStarFull(stars[i]);
            }
        }
    }

    void SetStarFull(GameObject star)
    {
        star.transform.GetChild(0).gameObject.SetActive(true);
        star.transform.GetChild(1).gameObject.SetActive(false);
    }

    void SetStarEmpty(GameObject star)
    {
        star.transform.GetChild(0).gameObject.SetActive(false);
        star.transform.GetChild(1).gameObject.SetActive(true);
    }

   
    public void StopTime()
    {
        isTimeRunning = false;
    }

    
    public void ResumeTime()
    {
        isTimeRunning = true;
    }

}
