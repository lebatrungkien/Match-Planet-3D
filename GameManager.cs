using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Threading.Tasks;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Level Editor")]
    public leveleditor levels;
    int levelvalue;
    int highestLevel;

    public int levelScore, totalScore;
    public Dictionary<int, int> highScores;

    public TextMeshProUGUI levelTMP;

    // public te levelValuetxt;
    [Header("Spawning Settings")]
    // public GameObject[] resourcePrefabs;
    public float spawnChance;
    public float _scale;
    public float _constScale;

    public Transform planetSpawningPoint;

    [Header("Timer Settings")]
    public float _countdownTime;
    public float _elapsedTime;
    private bool _isFinished;
    public TextMeshProUGUI timerText;
    public GameObject timeIcon;

    [Header("Raycast Settings")]
    public LayerMask layerMask;
    public int matchObjNum;
    private int _clickableItemLayer;
    public float sphereRadius = 0.05f; // Bán kính của khối cầu

    [Header("Set UI")]
    public GameObject wonPanel;
    public GameObject lossPanel;
    public GameObject pausePanel;
    public GameObject earthGO;
    public GameObject leaderboardPanel;
    // public TextMeshProUGUI scoreTMP;
    // public int _scoreValue;

    [Header("Music Sound")]
    public UnityEngine.UI.Toggle bgmToggle;  // Tham chiếu tới toggle cho BGM trong UI
    public UnityEngine.UI.Toggle sfxToggle;
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
        _constScale = 0.2f;
        _clickableItemLayer = LayerMask.NameToLayer("Match Item");
        timerText = GameObject.Find("TimerTMP").GetComponent<TextMeshProUGUI>();
        levelTMP = GameObject.Find("LevelTMP").GetComponent<TextMeshProUGUI>();


        // if (levels != null) {
        //     Debug.Log("levels " + levels.LevelData[0]);
        // }

        // public static int GetInt(string key, int defaultValue);
        levelvalue = PlayerPrefs.GetInt("level", 1);
        highestLevel = PlayerPrefs.GetInt("highestLevel", 1);
        if (levelvalue == 1)
        {
            Debug.Log("Hi Newbie");
        }
        _scale = levels.LevelData[levelvalue - 1].scale;
        _countdownTime = levels.LevelData[levelvalue - 1].minute * 60 + levels.LevelData[levelvalue - 1].seconds;
        _isFinished = false;
        _elapsedTime = 0;

        // bgmToggle.isOn = PlayerPrefs.GetInt("BGM", 1) == 1;
        // sfxToggle.isOn = PlayerPrefs.GetInt("SFX", 1) == 1;
        // // SoundManager.instance.LoadAudioSetting();

        // bgmToggle.onValueChanged.AddListener(delegate { BGMToggle(); });
        // sfxToggle.onValueChanged.AddListener(delegate { SFXToggle(); });

        Debug.Log("Current Level = " + levelvalue);
        levelTMP.text = "Level " + levelvalue;

        earthGO.GetComponent<Rigidbody>().mass = (0 - 4 / 9) * levelvalue + (5 + 4 / 9);

        // _scoreValue = PlayerPrefs.GetInt("score", 0);
        // SetTMPScore();

        CreateLevel();
        SpawnResources();
    }

    // Update is called once per frame
    void Update()
    {
        /* ***** Raycast Testcase *****
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Tạo một hướng ngẫu nhiên trên bề mặt của hình cầu đơn vị
        //    Vector3 rayPosition = Random.onUnitSphere * sphereRadius;

        //    // C1 Tạo ra ray từ camera
        //    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    //RaycastHit hit;

        //    // C2 Tạo một ray từ điểm ngẫu nhiên tới (0, 0, 0)
        //    Ray ray = new Ray(rayPosition, Vector3.zero - rayPosition);
        //    RaycastHit hit;

        //    // In ra gốc và hướng của ray
        //    Debug.Log($"Gốc của ray: {ray.origin}");
        //    Debug.Log($"Hướng của ray: {ray.direction}");

        //    if (Physics.Raycast(ray, out hit, 100))
        //    {
        //        Debug.Log(hit.transform.name);
        //        Debug.Log("hit");

        //        var newObj = Instantiate(resourcePrefab, hit.point, Quaternion.identity);
        //        newObj.transform.parent = GameObject.Find("EmptyGameObject").transform;


        //        Vector3 normal = hit.transform.position; // Lấy pháp tuyến của bề mặt
        //        newObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        //        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        //    }
        //    else
        //    {
        //        Debug.Log("Not hit");
        //        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        //    }
        //    //var newObj = Instantiate(resourcePrefab, rayPosition, Quaternion.identity);
        //    //newObj.transform.parent = GameObject.Find("EmptyGameObject").transform;
        //    //Vector3 normal = hit.transform.position; // Lấy pháp tuyến của bề mặt
        //    //newObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        //}
        // ************* */

        if (_countdownTime > 0 && _isFinished == false)
        {
            _elapsedTime += Time.deltaTime;
            _countdownTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_countdownTime / 60);
            int seconds = Mathf.FloorToInt(_countdownTime % 60);
            try
            {
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Update the TMP text with formatted time
            }
            catch (NullReferenceException)
            {
                Debug.Log("Catch Bug" + timerText);
            }
        }
        else if (_countdownTime <= 0 && !_isFinished)
        {
            _isFinished = true;
            timerText.text = "Time's up!";
            timeIcon.SetActive(false);
            TimeSlider.instance.StopTime();
            UIAnimation.instance.ShowLoserPanel();
            lossPanel.SetActive(true);
            SetElapsedTime();
        }

    }


    void CreateLevel()
    {
        matchObjNum = levels.LevelData[levelvalue - 1].TotalObjects.Length;
        Debug.Log("matchObjNum" + matchObjNum);
        timerText.text = levels.LevelData[levelvalue - 1].minute + ":" + levels.LevelData[levelvalue - 1].seconds + "Min";
    }

    // Init an match object
    void InitMatchObj(GameObject randomPref)
    {
        // Tạo một hướng ngẫu nhiên trên bề mặt của hình cầu đơn vị
        Vector3 rayPosition = Random.onUnitSphere * sphereRadius;

        // C2 Tạo một ray từ điểm ngẫu nhiên tới (0, 0, 0)
        Ray ray = new Ray(rayPosition, planetSpawningPoint.position - rayPosition);

        RaycastHit hit;



        // In ra gốc và hướng của ray
        // Debug.Log($"Gốc của ray: {ray.origin}");
        // Debug.Log($"Hướng của ray: {ray.direction}");

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            // Debug.Log("hit" + hit.transform.name);

            // Instantiate new pref
            var newObj = Instantiate(randomPref, hit.point, Quaternion.identity);

            // Add component
            newObj.AddComponent<BoxCollider>();
            newObj.AddComponent<MatchItem>();

            // Add rigidbody for trigerring match point
            newObj.AddComponent<Rigidbody>();
            newObj.GetComponent<Rigidbody>().useGravity = false;
            newObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            // Set layer for new instance
            newObj.layer = _clickableItemLayer;

            // Set pos for new instance
            newObj.GetComponent<MatchItem>().SetHitPos(hit.point);

            // Set rotation for new instance
            // newObj.transform.up = hit.normal;
            newObj.GetComponent<MatchItem>().SetNormalVec(hit.normal);

            // Set scale for new instance
            // newObj.transform.localScale = new Vector3(_constScale, _constScale, _constScale);
            newObj.GetComponent<MatchItem>().SetScale(new Vector3(_constScale * _scale, _constScale * _scale, _constScale * _scale));

            // Set parent for instance
            newObj.transform.parent = GameObject.Find("MatchObjects").transform;

            // Draw Ray for testing
            Debug.DrawRay(ray.origin, ray.direction, Color.red);

        }
        else
        {
            Debug.Log("Not hit");
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
        }
    }

    // Init all match object in each level
    void SpawnResources()
    {
        Debug.Log("Num of MatchObjNum" + matchObjNum);
        for (int i = 0; i < matchObjNum; i++)
        {
            // Spawn double object
            // Select random in prefabs arr
            // var randomNum = Random.Range(0, resourcePrefabs.Length);
            // var randomPref = resourcePrefabs[randomNum];
            var matchObj = levels.LevelData[levelvalue - 1].TotalObjects[i];
            InitMatchObj(matchObj);
            InitMatchObj(matchObj);
        }
        Debug.Log("spawned!");

    }


    public void CheckLevelCompleted(int collectedObjNum)
    {
        if (collectedObjNum == matchObjNum)
        {
            _isFinished = true;
            Debug.Log("collectedObjNum == matchObjNum " + collectedObjNum);
            earthGO.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            int score = PlayerPrefs.GetInt("totalScore") + collectedObjNum * 10 + (int)_countdownTime * 10;
            PlayerPrefs.SetInt("totalScore", score);
            Debug.Log("totalScore " + score);

            TimeSlider.instance.StopTime();
            UIAnimation.instance.ShowWonPanel();
            wonPanel.SetActive(true);
            SetElapsedTime();

            PlayerPrefs.SetInt("level", levelvalue);
            int nextLevel = levelvalue + 1;
            if (nextLevel > PlayerPrefs.GetInt("highestLevel", 1))
            {
                PlayerPrefs.SetInt("highestLevel", nextLevel);
            }
            PlayerPrefs.Save();

        }
    }



    public void OnNextClick()
    {
        // levelvalue++;
        // PlayerPrefs.SetInt("level", levelvalue);
        // // PlayerPrefs.Save();
        // SceneManager.LoadScene("gameplay");
        // Debug.Log("OnNextClick***" + PlayerPrefs.GetInt("level", levelvalue));

        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        levelvalue++;
        PlayerPrefs.SetInt("level", levelvalue);
        // Lấy màn chơi hiện tại từ PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("level", 1);

        // Phát nhạc tương ứng với màn chơi
        switch (currentLevel)
        {
            case 1:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_1_1_loop);
                break;
            case 2:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_2_1_loop);
                break;
            case 3:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_3_1_loop);
                break;
            case 4:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_4_1_loop);
                break;
            case 5:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_5_1_loop);
                break;
            default:
                Debug.LogError("Không có nhạc cho màn chơi này.");
                break;
        }
        // PlayerPrefs.Save();
        SceneManager.LoadScene("gameplay");
        Debug.Log("OnNextClick***" + PlayerPrefs.GetInt("level", levelvalue));
    }


    public async void OnReplay()
    {
        // await UIAnimation.instance.HidePausePanel();
        // PlayerPrefs.SetInt("level", levelvalue);
        // // PlayerPrefs.Save();
        // SceneManager.LoadScene("gameplay");
        // Time.timeScale = 1;
        // Debug.Log("OnReplayClick***" + PlayerPrefs.GetInt("level", levelvalue));

        // Phát âm thanh khi bấm nút replay
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        await UIAnimation.instance.HidePausePanel();

        // Đảm bảo AudioSource không bị mute trước khi phát nhạc
        if (MusicManager.instance.audioFx.mute)
        {
            MusicManager.instance.audioFx.mute = false;
        }

        // Lưu lại màn chơi hiện tại
        PlayerPrefs.SetInt("level", levelvalue);

        // Lấy màn chơi hiện tại từ PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("level", 1);

        // Phát nhạc tương ứng với màn chơi
        switch (currentLevel)
        {
            case 1:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_1_1_loop);
                break;
            case 2:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_2_1_loop);
                break;
            case 3:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_3_1_loop);
                break;
            case 4:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_4_1_loop);
                break;
            case 5:
                MusicManager.instance.OnPlayMusic(MusicType.Nhac_bai_5_1_loop);
                break;
            default:
                Debug.LogError("Không có nhạc cho màn chơi này.");
                break;
        }

        // Tải lại scene gameplay
        SceneManager.LoadScene("gameplay");

        // Đặt lại Time.timeScale
        Time.timeScale = 1;

        Debug.Log("OnReplayClick***" + PlayerPrefs.GetInt("level", levelvalue));
    }

    public async void OnHome()
    {
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        MusicManager.instance.OnPlayMusic(MusicType.Nhac_log_in);
        // await UIAnimation.instance.HidePausePanel();
        PlayerPrefs.SetInt("level", levelvalue);
        Debug.Log("On home click");
        // PlayerPrefs.Save();
        SceneManager.LoadScene("mainmenu");
        Time.timeScale = 1;
    }

    public void OnRestartLV1()
    {

        if (levelvalue > 1)
        {
            PlayerPrefs.SetInt("level", levelvalue - 1);
        }
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("highestLevel", 1);
        SceneManager.LoadScene("gameplay");
        Debug.Log("OnRestartLv1***" + PlayerPrefs.GetInt("level", levelvalue));
    }

    public void SetElapsedTime()
    {
        var elapsedTimeTMP = GameObject.Find("ElapsedTimeTMP").GetComponent<Text>();
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        elapsedTimeTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // public void SetTMPScore()
    // {
    //     scoreTMP.text = _scoreValue.ToString();
    // }

    public void ToggleBGM()
    {
        // SoundManager.instance.ToggleBGM();
        MusicManager.instance.ToggleBGM();
    }

    public void OnPauseClick()
    {
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        // UIAnimation.instance.ShowPausePanel();
    }

    public async void ExitPauseClick()
    {
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        // SoundManager.instance.LoadAudioSetting();
        await UIAnimation.instance.HidePausePanel();
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public async void OnResumeClick()
    {
        // SoundManager.instance.LoadAudioSetting();
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        await UIAnimation.instance.HidePausePanel();
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void BGMToggle()
    {
        // SoundManager.instance.ToggleBGM();
        MusicManager.instance.ToggleBGM();
    }

    public void SFXToggle()
    {
        SoundManager.instance.ToggleSFX();
    }



    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel", levelvalue);
    }

    public float GetLevelTime()
    {
        return _countdownTime;
    }
}
