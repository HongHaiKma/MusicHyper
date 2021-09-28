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
        FadeIn();
    }

    public void Submit()
    {
        FadeOut();
        ProfileManager.MyProfile.m_RateUs = 1;
        if (GameManager.Instance.m_RateStar == 5)
        {
            Helper.DebugLog("AAAAAAAAAAAAAAA");
            Application.OpenURL("market://details?id=" + Application.identifier);
        }
    }

    public virtual void FadeOut()
    {
        m_CanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        transform.DOScale(1.05f, 0.2f).SetEase(Ease.Flash).OnComplete(() => { gameObject.SetActive(false); }).SetUpdate(UpdateType.Late, true);
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
