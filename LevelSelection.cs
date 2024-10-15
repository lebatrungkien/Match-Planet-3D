using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [System.Serializable]
    public class StarState
    {
        public GameObject starFull;
        public GameObject starEmpty;
    }

    [System.Serializable]
    public class LevelState
    {
        public GameObject levelFinished;
        public GameObject levelBeingFinished;
        public GameObject levelUnfinished;
        public StarState[] stars;
    }

    public Button[] levelButtons;
    public LevelState[] levelStates;

    private int currentLevel;
    private int highestLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 1);
        highestLevel = PlayerPrefs.GetInt("highestLevel", 1);
        UpdateLevelStates();
        Debug.Log("currentLevel: " + currentLevel);
        Debug.Log("highestLevel: " + highestLevel);
    }

    public void UpdateLevelStates()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 < highestLevel)
            {
                SetActiveLevelState(i, true, false, false);
            }
            else if (i + 1 == highestLevel)
            {
                SetActiveLevelState(i, false, true, false);
            }
            else
            {
                SetActiveLevelState(i, false, false, true);
            }
            UpdateStars(i);
        }
    }

    private void SetActiveLevelState(int index, bool isFinished, bool isBeingFinished, bool isUnfinished)
    {
        levelStates[index].levelFinished.SetActive(isFinished);
        levelStates[index].levelBeingFinished.SetActive(isBeingFinished);
        levelStates[index].levelUnfinished.SetActive(isUnfinished);
    }

    private void UpdateStars(int index)
    {
        int stars = PlayerPrefs.GetInt("Level" + (index + 1) + "_Stars", 0);
        for (int i = 0; i < levelStates[index].stars.Length; i++)
        {
            bool isFull = i < stars && stars > 0;
            levelStates[index].stars[i].starFull.SetActive(isFull);
            levelStates[index].stars[i].starEmpty.SetActive(!isFull);
        }
    }

    public void OnClickLevel(int levelIndex)
    {
        SoundManager.instance.OnPlaySound(SoundManager.SoundType.sfx_tap);
        
        if (levelIndex + 1 <= highestLevel)
        {
            switch (levelIndex + 1)
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
            PlayerPrefs.SetInt("level", levelIndex + 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("gameplay");
        }
    }
}




