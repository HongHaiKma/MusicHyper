using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using RhythmGameStarter;

public class PopupRate : MonoBehaviour
{
    public CanvasGroup m_CanvasGroup;
    public Button btn_Close;
    public Button btn_Submit;

    public GameObject g_Content;
    public GameObject g_Thanks;

    private void Awake()
    {
        GUIManager.Instance.AddClickEvent(btn_Close, Close);
        GUIManager.Instance.AddClickEvent(btn_Submit, Submit);
    }

    void Close()
    {
        FadeOut();
    }

    private void OnEnable()
    {
        ProfileManager.MyProfile.m_OpenRateUs++;

        ProfileManager.MyProfile.m_FreeRate = 0;
        ProfileManager.MyProfile.m_ChallengeRate = 0;

        if (ProfileManager.MyProfile.m_OpenRateUs >= 2)
        {
            ProfileManager.MyProfile.m_RateUs = 1;
        }
        ProfileManager.Instance.SaveData();

        FadeIn();
        g_Content.SetActive(true);
        g_Thanks.SetActive(false);
    }

    public void Submit()
    {
        ProfileManager.MyProfile.m_RateUs = 1;
        ProfileManager.Instance.SaveData();

        if (GameManager.Instance.m_RateStar == 5)
        {
            FadeOut();
            Helper.DebugLog("AAAAAAAAAAAAAAA");
            Application.OpenURL("market://details?id=" + Application.identifier);
        }
        else
        {
            Helper.DebugLog("GGGGGGGGGGGG");
            g_Content.SetActive(false);
            g_Thanks.SetActive(true);
            StartCoroutine(IEFadeOut());
        }
    }

    IEnumerator IEFadeOut()
    {
        yield return Yielders.Get(1.5f);
        FadeOut();
    }

    public virtual void FadeOut()
    {
        // GameManager.Instance.ResumeSong();

        m_CanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        transform.DOScale(1.05f, 0.2f).SetEase(Ease.Flash).OnComplete(() => { gameObject.SetActive(false); }).SetUpdate(UpdateType.Late, true);

        if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
        {
            if (GameManager.Instance.IsStoryWeekEnd())
            {
                EventManager1<InterType>.CallEvent(GameEvent.WATCH_INTER, InterType.END_STORY);
            }
            else
            {
                EventManager1<InterType>.CallEvent(GameEvent.WATCH_INTER, InterType.STORY);
            }
        }
        else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
        {
            EventManager1<InterType>.CallEvent(GameEvent.WATCH_INTER, InterType.FREE);
        }
    }
    public void FadeIn()
    {
        if (m_CanvasGroup != null)
        {
            m_CanvasGroup.alpha = 0;
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            m_CanvasGroup.DOFade(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
            transform.DOScale(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        }
    }
}
