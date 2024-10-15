using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManger : MonoBehaviour
{
    public GameObject loadingGO;
    public GameObject settingPanel;
    public TextMeshProUGUI welcomeTMP;

    public GameObject selectLevelPanel;

    public GameObject mainMenuCanvas;
    public GameObject planetsHolder;

    public GameObject bgMainMenu;

    public Toggle bgmToggle;
    public Toggle sfxToggle;

    // public Leaderboard leaderboard;

    // private int currentPlanet = PlayerPrefs.GetInt("Index_current_planet");

    void Start()
    {
        // bgmToggle.isOn = PlayerPrefs.GetInt("BGM", 1) == 1;
        // sfxToggle.isOn = PlayerPrefs.GetInt("SFX", 1) == 1;

        // bgmToggle.onValueChanged.AddListener(delegate { BGMToggle(); });
        // sfxToggle.onValueChanged.AddListener(delegate { SFXToggle(); });
        welcomeTMP.text = "Highest score: " + PlayerPrefs.GetInt("highestScore", 0) + " " + PlayerPrefs.GetString("highestScorePlayer", "NoName");
        // MusicManager.instance.OnPlayMusic(MusicType.Nhac_log_in);
        // SoundManager.instance.LoadAudioSetting();
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayClick()
    {
        // loadingGO.SetActive(true);
        // SceneManager.LoadScene("gameplay");
        // PlayerPrefs.SetInt("level", 1);

        // levelvalue = PlayerPrefs.GetInt("level", 1);
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);

        // Lấy màn chơi hiện tại từ PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("level", 1);

        //Phát nhạc tương ứng với màn chơi
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

        // Hiển thị màn hình loading và load màn chơi hiện tại
        loadingGO.SetActive(true);
        SceneManager.LoadScene("gameplay");

    }

    public void OnSettingClick()
    {
        // UIAnimation.instance.ShowSettingPanel();
        settingPanel.SetActive(true);
    }

    public void ExitSettingClick()
    {
        // UIAnimation.instance.HideSettingPanel();
        settingPanel.SetActive(false);
        // SoundManager.instance.LoadAudioSetting();
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

    public void OnSelectLevelClick()
    {
        selectLevelPanel.SetActive(true);
        mainMenuCanvas.SetActive(false);
        planetsHolder.SetActive(false);
        bgMainMenu.SetActive(false);
    }

    public void ReturnHomeMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }

}
