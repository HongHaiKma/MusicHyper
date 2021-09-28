using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RhythmGameStarter;

public class RateStar : MonoBehaviour
{
    public int m_StarNo;
    public Button btn_Rate;
    public Image img_Star;

    private void Awake()
    {
        GUIManager.Instance.AddClickEvent(btn_Rate, () => EventManager1<int>.CallEvent(GameEvent.RATE, m_StarNo));
    }

    private void OnEnable()
    {
        EventManager1<int>.AddListener(GameEvent.RATE, SetRate);
    }

    private void OnDisable()
    {
        EventManager1<int>.RemoveListener(GameEvent.RATE, SetRate);
    }

    private void OnDestroy()
    {
        EventManager1<int>.RemoveListener(GameEvent.RATE, SetRate);
    }

    public void Click()
    {
        EventManager1<int>.CallEvent(GameEvent.RATE, m_StarNo);

        // if (m_StarNo == 5)
        // {
        //     ProfileManager.MyProfile.m_RateUs = 1;
        //     Application.OpenURL("market://details?id=" + Application.identifier);
        // }
    }

    public void SetRate(int _value)
    {
        GameManager.Instance.m_RateStar = _value;
        if (_value < m_StarNo)
        {
            img_Star.sprite = SpriteManager.Instance.m_RateStar[0];
        }
        else
        {
            transform.DOScale(new Vector3(1.2f, 1.2f, 1f), m_StarNo * 0.1f).
            OnComplete(
                () =>
                {
                    img_Star.sprite = SpriteManager.Instance.m_RateStar[1];
                    transform.DOScale(new Vector3(1f, 1f, 1f), m_StarNo * 0.2f);
                }
            );
            // img_Star.sprite = SpriteManager.Instance.m_RateStar[1];
            // UIManager.Instance.CloseRatePopup();
        }
    }
}
