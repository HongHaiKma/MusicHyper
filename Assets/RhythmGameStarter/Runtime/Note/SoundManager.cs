using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using MoreMountains.NiceVibrations;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource m_IngameShootingFx;
    public AudioSource m_BGM;

    public AudioClip m_ButtonClick;
    public AudioClip m_ButtonArrow;
    public AudioClip m_ButtonConfirm;
    public AudioClip m_SoundGetGold;

    public AudioClip m_SoundCrack;

    public AudioClip m_WaterSpray;
    public AudioClip[] m_BGMTheme;

    public AudioClip m_SoundBigPrize;

    public AudioClip m_One;
    public AudioClip m_Two;
    public AudioClip m_Three;
    public AudioClip m_Go;

    public AudioClip m_BtnSound;

    // private void Awake()
    // {
    //     m_BGM.Pause();
    // }

    // private void Start()
    // {
    //     m_BGM.Pause();
    // }

    public override void OnEnable()
    {
        base.OnEnable();
        OnSoundChange();
        OnMusicChange();
        PlayBGM(0);


        StartListenToEvent();
    }

    public void StartListenToEvent()
    {
        // EventManager.AddListener(GameEvent.SOUND_CHANGE, OnSoundChange);
        // EventManager.AddListener(GameEvent.MUSIC_CHANGE, OnMusicChange);
        // EventManager.AddListener(GameEvent.CHAR_WIN, OnSoundWin);
        // EventManager.AddListener(GameEvent.CHAR_SPOTTED, OnSoundLose);
    }

    public void StopListenToEvent()
    {
        // EventManager.RemoveListener(GameEvent.SOUND_CHANGE, OnSoundChange);
        // EventManager.RemoveListener(GameEvent.MUSIC_CHANGE, OnMusicChange);
        // EventManager.RemoveListener(GameEvent.CHAR_WIN, OnSoundWin);
        // EventManager.RemoveListener(GameEvent.CHAR_SPOTTED, OnSoundLose);
    }

    public void OnSoundChange()
    {
        m_IngameShootingFx.volume = 0.3f;
    }

    public void OnMusicChange()
    {
        m_BGM.volume = 0.6f;
    }

    public void PlayBGM(int index)
    {
        m_BGM.clip = m_BGMTheme[index];
        m_BGM.Play();
    }

    public void PauseBGM()
    {
        m_BGM.Pause();
    }

    public void PlayWaterSpray()
    {
        m_BGM.clip = m_WaterSpray;
        m_BGM.Play();
    }

    public void PlayButtonClick()
    {
        m_IngameShootingFx.PlayOneShot(m_ButtonClick, 1);
    }

    public void PlayButtonClickArrow()
    {
        m_IngameShootingFx.PlayOneShot(m_ButtonArrow, 2);
    }

    public void PlayButtonClickConfirm()
    {
        m_IngameShootingFx.PlayOneShot(m_ButtonConfirm, 2);
    }

    public void PlaySoundCrack()
    {
        m_IngameShootingFx.PlayOneShot(m_SoundCrack, 1.4f);
    }

    public void PlaySoundOne()
    {
        m_IngameShootingFx.PlayOneShot(m_One, 2);
    }

    public void PlaySoundTwo()
    {
        m_IngameShootingFx.PlayOneShot(m_Two, 2);
    }

    public void PlaySoundThree()
    {
        m_IngameShootingFx.PlayOneShot(m_Three, 2);
    }

    public void PlaySoundGo()
    {
        m_IngameShootingFx.PlayOneShot(m_Go, 2);
    }

    public void PlayBtnSound()
    {
        m_IngameShootingFx.PlayOneShot(m_BtnSound, 2);
    }

    public void OnSoundWin()
    {
        m_BGM.Pause();
    }

    public void OnSoundLose()
    {
        m_BGM.Pause();
    }

    public void SetSoundState(int value)
    {
        PlayerPrefs.SetInt("Sound", value);
        // EventManager.CallEvent("MusicChange");
        OnSoundChange();
    }
    public void SetMusicState(int value)
    {
        PlayerPrefs.SetInt("Music", value);
        // EventManager.CallEvent("MusicChange");
        OnMusicChange();
    }
}
