using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace RhythmGameStarter
{
    public class PopupLose : MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        public Button btn_Replay;
        public Button btn_Home;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_Replay, Replay);
            GUIManager.Instance.AddClickEvent(btn_Home, Home);
        }

        private void OnEnable()
        {
            FadeIn();
        }

        public void Replay()
        {
            FadeOut();
            StatsSystem.Instance.missed = 0;
            StatsSystem.Instance.combo = 0;
            GUIManager.Instance.SetBlockPopup(true);
            GameManager.Instance.ResetSong();
        }

        public void Home()
        {
            FadeOut();
            Note.m_ReturnHome = true;
            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                GUIManager.Instance.LoadPlayScene(() => UIManager.Instance.OpenStoryMenu());
                // UIManager.Instance.OpenStoryMenu();
                // GameManager.Instance.StopSong();
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                GUIManager.Instance.LoadPlayScene(() => UIManager.Instance.OpenFreeplayMenu());
                // UIManager.Instance.OpenFreeplayMenu();
                // GameManager.Instance.StopSong();
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
}