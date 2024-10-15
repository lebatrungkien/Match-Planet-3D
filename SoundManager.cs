using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        sfx_match = 0,
        sfx_select = 1,
        sfx_tap = 2,
    }


    public static SoundManager instance;
    public AudioSource audioFx;
    private bool isSFXEnabled = true;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            Debug.Log("Not Destroy SFXManager"); //dam bao object khong bi huy khi chuyen scene
        }
        else
        {
            Debug.Log("Destroying SFXManager");
            Destroy(this); //dam bao chi ton tai 1 instance cua SoundManager
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnValidate()
    {
        if (audioFx == null)
        {
            audioFx = gameObject.AddComponent<AudioSource>();
            audioFx.playOnAwake = false;
        }
    }
    public void OnPlaySound(SoundType soundType)
    {
        var audio = Resources.Load<AudioClip>($"Sound/{soundType.ToString()}");
        audioFx.PlayOneShot(audio);
    }
    public void ToggleSFX()
    {
        isSFXEnabled = !isSFXEnabled;
        audioFx.mute = !isSFXEnabled;
        SaveSettings();
    }
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("SFXEnabled", isSFXEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

}
// public static SoundManager instance = null;

// public AudioSource MusicSrc;
// public AudioSource SFXSrc;
// public AudioClip MatchClip;
// public AudioClip TapClip;
// public AudioClip SelectClip;
// public AudioSource bgm;

// void Awake()
// {

//     if (instance == null)
//     {
//         instance = this;
//         DontDestroyOnLoad(this);
//         Debug.Log("Not Destroy SFXManager"); //dam bao object khong bi huy khi chuyen scene
//     }
//     else
//     {
//         Debug.Log("Destroying SFXManager");
//         Destroy(this); //dam bao chi ton tai 1 instance cua SoundManager
//     }
//     LoadAudioSetting();
// }
// // Start is called before the first frame update
// void Start()
// {

// }

// // Update is called once per frame
// void Update()
// {

// }

// public void LoadAudioSetting(){
//     if(PlayerPrefs.HasKey("BGM")){
//         bool bgmEnabled = PlayerPrefs.GetInt("BGM") == 1;
//         bgm.mute = !bgmEnabled;
//     }
//     if(PlayerPrefs.HasKey("SFX")){
//         bool sfxEnabled = PlayerPrefs.GetInt("SFX") == 1;
//         SFXSrc.mute = !sfxEnabled;
//     }
// }

// public void PlayMatchSFX() {
//     SFXSrc.PlayOneShot(MatchClip);
// }
// public void PlaySelectSFX() {
//     SFXSrc.PlayOneShot(SelectClip);
// }

// public void ToggleBGM() {
//     bool isMuted = bgm.mute;
//     bgm.mute = !isMuted;
//     PlayerPrefs.SetInt("BGM", isMuted ? 0 : 1);
//     // bgm.mute = !bgm.mute;
// }
// public void ToggleSFX(){
//     bool isMuted = SFXSrc.mute;
//     SFXSrc.mute = !isMuted;
//     PlayerPrefs.SetInt("SFX", isMuted ? 0 : 1);
//     // SFXSrc.mute = !SFXSrc.mute;
// }

