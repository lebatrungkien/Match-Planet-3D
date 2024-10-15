using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    Nhac_bai_1_1_loop = 0,
    Nhac_bai_2_1_loop = 1,
    Nhac_bai_3_1_loop = 2,
    Nhac_bai_4_1_loop = 3,
    Nhac_bai_5_1_loop = 4,
    Nhac_ket_bai_1_1_loop_common = 5,
    Nhac_log_in = 6,
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;
    public AudioSource audioFx;
    private bool isBGMEnabled = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Not Destroy...");

        }
        else
        {
            Debug.Log("Destroying...");
            Destroy(this);
        }
    }

    // Đảm bảo AudioSource được khởi tạo đúng cách
    private void OnValidate()
    {
        if (audioFx == null)
        {
            audioFx = gameObject.AddComponent<AudioSource>();
            audioFx.playOnAwake = false;
        }
    }

    // Hàm để phát nhạc
    public void OnPlayMusic(MusicType musicType)
    {
        SmoothSound(audioFx, 1, musicType);
        // Kiểm tra nếu đang phát nhạc Nhac_log_in thì không cho lặp lại
        if (musicType == MusicType.Nhac_log_in)
        {
            audioFx.loop = false; // Không lặp lại nhạc Nhac_log_in
        }
        else
        {
            audioFx.loop = true; // Các loại nhạc khác sẽ lặp lại
        }
    }

    // private void LoadBGM()
    // {
    //     isBGMEnabled = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
    //     audioFx.mute = !isBGMEnabled;
    //     Debug.Log("BGMEnabled" + isBGMEnabled);
    // }
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("BGMEnabled", isBGMEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void ToggleBGM()
    {
        isBGMEnabled = !isBGMEnabled;
        audioFx.mute = !isBGMEnabled;
        SaveSettings();
    }

    public void SmoothSound(AudioSource audioSource, float fadeTime, MusicType musicType)
    {
        StartCoroutine(FadeSoundOn(audioSource, fadeTime, musicType));
    }

    IEnumerator FadeSoundOn(AudioSource audioSource, float fadeTime, MusicType musicType)
    {
        yield return FadeSoundOff(audioSource, fadeTime);
        var audio = Resources.Load<AudioClip>($"Music/{musicType.ToString()}");
        // Debug.Log("Audio: " + audio);
        if (audio != null)
        {
            if (audioFx != null)
            {
                audioFx.clip = audio; // Set clip để quản lý âm thanh đang phát
                audioFx.loop = true;  // Set loop nếu muốn phát lặp lại
                audioFx.Play();
                Debug.Log($"Playing music: {musicType.ToString()}");
            }
            else
            {
                Debug.Log("Music is already playing and it's the same clip.");
            }

            // Gán nhạc mới nếu chưa phát hoặc nhạc hiện tại khác
            // if (audioFx.clip != audio || !audioFx.isPlaying)
            // {
            //     audioFx.clip = audio; // Set clip để quản lý âm thanh đang phát
            //     audioFx.loop = true;  // Set loop nếu muốn phát lặp lại
            //     audioFx.Play();
            //     Debug.Log($"Playing music: {musicType.ToString()}");
            // }
            // else
            // {
            //     Debug.Log("Music is already playing and it's the same clip.");
            // }
        }
        else
        {
            Debug.LogError($"AudioClip {musicType.ToString()} not found in Resources/Music/");
        }

        var t = 0f;
        while (t < 1)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            if (audioSource != null) audioSource.volume = t;
        }
    }

    IEnumerator FadeSoundOff(AudioSource audioSource, float fadeTime)
    {
        var t = fadeTime;
        while (t > 0)
        {
            yield return new WaitForEndOfFrame();
            t -= Time.deltaTime;
            if (audioSource != null) audioSource.volume = t;
        }
    }
}