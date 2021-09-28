using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace RhythmGameStarter
{
    public class LoseContinue : MonoBehaviour
    {
        public float m_Time;
        public TextMeshProUGUI txt_Time;
        public CanvasGroup m_CanvasGroup;
        public Button btn_Continue;
        public Image img_ClockWise;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_Continue, Continue);
        }

        private void OnEnable()
        {
            FadeIn();
            m_Time = 3f;
            txt_Time.text = ((int)m_Time).ToString();
            btn_Continue.interactable = true;

            AddListener();
        }

        private void OnDisable()
        {
            FadeOut();
            RemoveListener();
        }

        private void OnDestroy()
        {
            RemoveListener();
        }

        public void AddListener()
        {
            EventManager.AddListener(GameEvent.CONTINUE, ContinueAds);
        }

        public void RemoveListener()
        {
            EventManager.RemoveListener(GameEvent.CONTINUE, ContinueAds);
        }

        private void Update()
        {
            if (!AdsManager.Instance.openRwdAds2)
            {
                m_Time -= Time.deltaTime;
            }

            img_ClockWise.fillAmount = m_Time / 3f;
            txt_Time.text = ((int)m_Time).ToString();

            if (btn_Continue.interactable)
            {
                if (m_Time <= 0.5f)
                {
                    btn_Continue.interactable = false;
                }
            }

            if (m_Time <= 0f)
            {
                m_Time = 0f;
                txt_Time.text = ((int)m_Time).ToString();
                FadeOut();
                UIManager.Instance.OpenLosePopup(true);
            }
        }

        public void Continue()
        {
            AdsManager.Instance.WatchRewardVideo(RewardType.CONTINUE);
        }

        public void ContinueAds()
        {
            AnalysticsManager.LogRevive();
            FadeOut();
            GameManager.Instance.m_Continue = false;
            GameManager.Instance.m_knot = 0;
            GameManager.Instance.m_Knot = 0;
            // StatsSystem.Instance.missed = 0;
            // StatsSystem.Instance.combo = 0;
            GameManager.Instance.ResetVsBar();
            GameManager.Instance.ResumeSong();
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