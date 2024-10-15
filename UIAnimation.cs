using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;



public class UIAnimation : MonoBehaviour
{
    public static UIAnimation instance = null;
    
     void Awake()
    {
        // PlayerPrefs.SetInt("level", 1);
        // levelvalue = PlayerPrefs.GetInt("level", 1);

        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(this);
        }

    }

    //pause panel
    public GameObject pausePanel;  
    public RectTransform pausePanelRect;
    public GameObject BG;
    public float topPosY, middlePosY;
    public float tweenDuration;

    // //setting panel
    // public CanvasGroup settingPanel;
    // public RectTransform settingPanelRect;
   


    //won panel
    public float fadeTime = 1f;
    public CanvasGroup wonPanelCG;
    public RectTransform wonPanelRect;

    //loser panel
    public CanvasGroup loserPanelCG;
    public RectTransform loserPanelRect;

    public void ShowLoserPanel(){
        loserPanelCG.alpha = 0f;
        loserPanelRect.transform.localPosition = new Vector3(0f, 500f, 0f);
        loserPanelRect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutBack);
        loserPanelCG.DOFade(1, fadeTime);
    }

    public void HideLoserPanel(){
        loserPanelCG.alpha = 1f;
        loserPanelRect.transform.localPosition = new Vector3(0f, 0f, 0f);
        loserPanelRect.DOAnchorPos(new Vector2(0f, -500f), fadeTime, false).SetEase(Ease.InOutBack);
    }

    public void ShowWonPanel(){
        wonPanelCG.alpha = 0f;
        wonPanelRect.transform.localPosition = new Vector3(0f, -500f, 0f);
        wonPanelRect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        wonPanelCG.DOFade(1, fadeTime);
    }

    public void HideWonPanel(){
        wonPanelCG.alpha = 1f;
        wonPanelRect.transform.localPosition = new Vector3(0f, 0f, 0f);
        wonPanelRect.DOAnchorPos(new Vector2(0f, -500f), fadeTime, false).SetEase(Ease.InOutElastic);
        wonPanelCG.DOFade(0, fadeTime);
    }

    public void ShowPausePanel(){
        // BG.SetActive(true);
        pausePanelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
        BG.SetActive(true);
    }  

    public async Task HidePausePanel(){
        await pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    // public void ShowSettingPanel(){
    //     settingPanel.alpha = 0f;
    //     settingPanelRect.transform.localPosition = new Vector3(0f, -500f, 0f);
    //     settingPanelRect.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
    //     settingPanel.DOFade(1, fadeTime);
    // }
    // public void HideSettingPanel(){
    //     settingPanel.alpha = 1f;
    //     settingPanelRect.transform.localPosition = new Vector3(0f, 0f, 0f);
    //     settingPanelRect.DOAnchorPos(new Vector2(0f, -500f), fadeTime, false).SetEase(Ease.InOutElastic);
    //     settingPanel.DOFade(0, fadeTime);
    // }

}
