using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RhythmGameStarter;
using EnhancedUI.EnhancedScroller;

// namespace RhythmGameStarter
// {
public class SongPlaylist : EnhancedScrollerCellView
{
    public int id;
    public int songId;
    public Button btn_PlaySong;
    public Button btn_TrySong;
    public TextMeshProUGUI txt_Name;
    public Button btn_BuySongGold;
    public TextMeshProUGUI txt_BuySongGold;
    public TextMeshProUGUI txt_OutOfGold;
    public TextMeshProUGUI txt_HighScore;
    public GameObject g_Content;
    public GameObject g_Lock;
    public GameObject g_OutOfGold;
    public Image img_EnemyAva;
    public Image BG;

    private void Awake()
    {
        GUIManager.Instance.AddClickEvent2(btn_PlaySong, PlaySong);
        GUIManager.Instance.AddClickEvent2(btn_BuySongGold, BuySong);
        GUIManager.Instance.AddClickEvent2(btn_TrySong, TrySong);
    }

    public void OnEnable()
    {
        btn_PlaySong.gameObject.SetActive(false);
        SongConfig configs = GameData.Instance.GetSongConfig(songId);
        // g_OutOfGold.SetActive(!ProfileManager.IsEnoughGold(configs.m_Price));
        DisplaySongInfo();
        AddListener();
    }

    public void OnDisable()
    {
        RemoveListener();
    }

    public void OnDestroy()
    {
        RemoveListener();
    }

    public void AddListener()
    {
        EventManager.AddListener(GameEvent.DISPLAY_SONG_INFO, DisplaySongInfo);
    }

    public void RemoveListener()
    {
        EventManager.RemoveListener(GameEvent.DISPLAY_SONG_INFO, DisplaySongInfo);
    }

    public void SetData(SongData data)
    {
        songId = data.m_ID;
        DisplaySongInfo();
    }

    public void DisplaySongInfo()
    {
        // songId = 4 * GameManager.Instance.m_FreePlayListId + id;
        BG.sprite = SpriteManager.Instance.m_SongPlaylistBG[SpriteManager.Instance.m_SongPlaylistBGIndex[songId - 1]];
        int count = GameData.Instance.GetSongCount();

        SongProfile song = ProfileManager.GetSongProfiles(songId);
        SongConfig configs = GameData.Instance.GetSongConfig(songId);

        img_EnemyAva.sprite = SpriteManager.Instance.m_EnemyAvas[configs.m_EnemyNo - 1];
        img_EnemyAva.SetNativeSize();

        if (song != null)
        {
            btn_PlaySong.gameObject.SetActive(false);
            g_Lock.SetActive(true);
            img_EnemyAva.color = GameManager.Instance.m_SongPLayFade;

            btn_BuySongGold.gameObject.SetActive(false);

            // g_Lock.SetActive(ProfileManager.GetSongProfiles(songId) != null ? false : true);

            if (songId <= count)
            {
                btn_PlaySong.gameObject.SetActive(true);
                g_Lock.SetActive(false);
                img_EnemyAva.color = GameManager.Instance.m_SongPLayFadeNormal;
                btn_TrySong.gameObject.SetActive(false);
            }
            else
            {
                btn_PlaySong.gameObject.SetActive(false);
                btn_TrySong.gameObject.SetActive(true);
                g_Lock.SetActive(true);
                img_EnemyAva.color = GameManager.Instance.m_SongPLayFade;
            }

            txt_HighScore.gameObject.SetActive(true);
            txt_HighScore.text = "score: " + song.m_HighScore.ToString();

            g_OutOfGold.SetActive(false);
        }
        else
        {
            btn_PlaySong.gameObject.SetActive(true);
            g_Lock.SetActive(false);
            img_EnemyAva.color = GameManager.Instance.m_SongPLayFadeNormal;
            // g_Block.SetActive(true);
            btn_PlaySong.gameObject.SetActive(false);
            g_Lock.SetActive(true);
            img_EnemyAva.color = GameManager.Instance.m_SongPLayFade;
            btn_PlaySong.interactable = false;

            // if (ProfileManager.IsEnoughGold(configs.m_Price))
            // {
            btn_BuySongGold.gameObject.SetActive(ProfileManager.IsEnoughGold(configs.m_Price));
            g_OutOfGold.SetActive(!ProfileManager.IsEnoughGold(configs.m_Price));
            // }
            // else
            // {
            //     btn_BuySongGold.gameObject.SetActive(false);
            //     g_OutOfGold.SetActive(true);
            // }

            btn_TrySong.gameObject.SetActive(true);
            if (songId <= count)
            {
                // SongConfig config = GameData.Instance.GetSongConfig(songId);
                txt_BuySongGold.text = configs.m_Price.ToString2();
                txt_OutOfGold.text = configs.m_Price.ToString2();
            }

            txt_HighScore.gameObject.SetActive(false);
        }

        if (songId <= count)
        {
            g_Content.SetActive(true);
            btn_PlaySong.interactable = true;
            txt_Name.gameObject.SetActive(true);

            SongConfig songConfig = GameData.Instance.GetSongConfig(songId);
            txt_Name.text = songConfig.m_Name;
        }
        else
        {
            g_Content.SetActive(false);
            btn_PlaySong.interactable = false;
            txt_Name.gameObject.SetActive(false);
            txt_HighScore.gameObject.SetActive(false);
        }
    }

    public void PlaySong()
    {
        GUIManager.Instance.SetBlockPopup(true);
        TrackManager.Instance.beatSize = 1.5f;

        GameManager.Instance.txt_Time.gameObject.SetActive(false);

        GUIManager.Instance.SetBlockPopup(true);
        SoundManager.Instance.PauseBGM();
        GameManager.Instance.PlaySong();
        GameManager.Instance.m_DefaultSong = songId;

        SongConfig song = GameData.Instance.GetSongConfig(songId);

        if (ProfileManager.GetSongProfiles(songId) != null)
        {
            GameManager.Instance.m_TrySong = false;
        }
        else
        {
            GameManager.Instance.m_TrySong = true;
        }

        if (GameManager.Instance.m_Enemy != null)
        {
            Destroy(GameManager.Instance.m_Enemy.gameObject);
        }

        Helper.DebugLog("Enemy name: " + song.m_EnemyName);

        GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
        enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);
        enemy.transform.localScale = new Vector3(1f, 1f, 1f);
        GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

        StartCoroutine(IEPlaySong());
    }

    public void BuySong()
    {
        SongConfig config = GameData.Instance.GetSongConfig(songId);
        BigNumber gold = ProfileManager.GetGold2();
        if (gold >= config.m_Price)
        {
            ProfileManager.ConsumeGold(config.m_Price);
            ProfileManager.UnlockSong(songId);
            EventManager.CallEvent(GameEvent.DISPLAY_SONG_INFO);
            GameManager.Instance.txt_TotalGold.text = ProfileManager.GetGold();
        }
    }

    public void TrySong()
    {
        PlaySong();
    }

    IEnumerator IEPlaySong()
    {
        yield return Yielders.Get(0.5f);
        SongManager.Instance.defaultSong = GameManager.Instance.m_Songs[songId - 1];
        // GUIManager.Instance.SetBlockPopup(false);
    }
}
// }