using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
public class WelcomeController : MonoBehaviour
{
    public TMP_InputField nameInput;
    public GameObject loadingGO;

    void Start()
    {
        MusicManager.instance.OnPlayMusic(MusicType.Nhac_log_in);
    }

    public void onPlayClick()
    {
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        PlayerPrefs.SetString("playerName", nameInput.text);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadScene("mainmenu");
        loadingGO.SetActive(true);
    }
}