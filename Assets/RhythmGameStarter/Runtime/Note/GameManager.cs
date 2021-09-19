using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RhythmGameStarter
{
    [DefaultExecutionOrder(-90)]
    public class GameManager : Singleton<GameManager>
    {
        public ModePlay m_ModePlay;
        public StoryLevel m_StoryLevel;

        public Character m_Player;
        public Enemy m_Enemy;

        public int m_PlayerNotes;
        public TextMeshProUGUI txt_PlayerNotes;

        public int m_EnemyNotes;
        public TextMeshProUGUI txt_EnemyNotes;

        public int m_Notes;
        public int m_NotesMax = 5;

        public bool m_PlayerTurn = true;

        public GameObject g_RhythymCore;

        public int m_DefaultSong;

        public bool m_TrySong;

        [Header("VS Bar")]
        public Transform tf_Icon;
        public Image img_PlayerBar;
        public int m_knot;
        public int m_Knot
        {
            get { return m_knot; }
            set
            {
                float fill = img_PlayerBar.fillAmount;
                DOTween.To(() => fill, x => fill = x, tf_VsFillPos[m_knot], 0.5f).OnUpdate(
                    () => img_PlayerBar.fillAmount = fill
                );
            }
        }
        public Transform[] tf_Knots;
        public float[] tf_VsFillPos;
        public Light l_LightPlayer;
        public Light l_LightEnemy;

        [Header("UI")]
        public TextMeshProUGUI txt_Time;
        public Button btn_Play;
        public Button btn_LeftSong;
        public Button btn_RightSong;
        public Button btn_Pause;
        public Animator anim_UI;
        public Transform parent_UI;
        public GameObject g_Ready;
        public GameObject g_Set;
        public GameObject g_Go;
        public GameObject g_Go2;
        public TextMeshProUGUI txt_Page;
        public int m_FreePlayListId;
        public TextMeshProUGUI txt_TotalGold;
        public TextMeshProUGUI txt_Miss;

        // [Header("Song")]
        public List<SongItem> m_Songs;

        [Header("Player")]
        public Material mat_Sing1;
        public Material mat_Sing2;
        public Material mat_Sing3;
        public Material mat_Sing4;
        public Material mat_Hit;
        public Material mat_Idle;
        public Transform tf_Start;
        public Transform tf_End;
        public EnemyTime m_EnemyTime;
        public bool m_Continue = true;
        public Transform tf_EnemyHolder;
        public bool m_EnemyTurn;

        [Header("Story")]
        public int m_StorysongNo;
        public int m_StorySongID;
        public List<WeekConfig> m_WeekConfigs = new List<WeekConfig>();
        public List<SongItem> m_WeekSongs = new List<SongItem>();
        public int m_WeekNo = 1;

        public List<Note> m_NoteInGame = new List<Note>();

        public Transform tf_Left;
        public Transform tf_Down;
        public Transform tf_Up;
        public Transform tf_Right;

        public bool m_1stSong;

        public Color m_SongPLayFade;
        public Color m_SongPLayFadeNormal;

        public bool m_ReturnHome;

        public BigNumber m_Golg;
        public BigNumber m_Test;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            if (!PlayerPrefs.HasKey("Vibration"))
            {
                PlayerPrefs.SetInt("Vibration", 1);
            }

            // GUIManager.Instance.AddClickEvent(btn_Play, PlaySong);
            // GUIManager.Instance.AddClickEvent(btn_LeftSong, OnPrevSong);
            // GUIManager.Instance.AddClickEvent(btn_RightSong, OnNextSong);
            // GUIManager.Instance.AddClickEvent(btn_Pause, PauseSongPopup);
        }

        private void Update()
        {
            // if (g_RhythymCore.activeInHierarchy)
            // {
            //     txt_Time.text = ((int)SongManager.Instance.songPosition).ToString();
            // }

            if (Input.GetKeyDown(KeyCode.C))
            {
                m_Golg = ProfileManager.GetGold2();
                m_Test = GameData.Instance.GetSongConfig(19).m_Price + 0;
            }

            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     // m_StorysongNo++;
            //     // SongManager.Instance.defaultSong = m_WeekSongs[m_StorysongNo];
            //     // SoundManager.Instance.PauseBGM();
            //     // m_TrySong = false;

            //     // WeekConfig song = m_WeekConfigs[m_StorysongNo];

            //     // if (m_Enemy.gameObject != null)
            //     // {
            //     //     PrefabManager.Instance.DespawnPool(m_Enemy.gameObject);
            //     // }

            //     // GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
            //     // enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
            //     // enemy.transform.localPosition = Vector3.zero;
            //     // enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);
            //     // enemy.transform.localScale = new Vector3(1f, 1f, 1f);
            //     // GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

            //     // ResetSong();
            //     NextWeekSong();
            // }

            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     List<WeekConfig> weekConfigs = GameManager.Instance.m_WeekConfigs;
            //     for (int i = 0; i < GameManager.Instance.m_WeekConfigs.Count; i++)
            //     {
            //         Helper.DebugLog("Name: " + GameManager.Instance.m_WeekConfigs[i].m_Name);
            //     }
            // }

            // if (Input.GetKeyDown(KeyCode.D))
            // {
            //     Time.timeScale = 10f;
            // }

            // if (SongManager.Instance.songHasStarted)
            // {
            //     List<TimeInfo> timeInfo = m_EnemyTime.m_TimeInfo;

            //     for (int i = 0; i < timeInfo.Count; i++)
            //     {
            //         if (timeInfo[i].IsInRange(SongManager.Instance.songPosition))
            //         {
            //             if (m_Enemy.m_State == E_State.IDLE)
            //             {
            //                 m_Enemy.SetState(E_State.SING);
            //                 l_LightEnemy.intensity = 4.5f;
            //                 l_LightPlayer.intensity = 2.5f;
            //             }
            //             return;
            //         }
            //     }

            //     if (m_Enemy.m_State == E_State.SING)
            //     {
            //         m_Enemy.SetState(E_State.IDLE);
            //         l_LightEnemy.intensity = 2f;
            //         l_LightPlayer.intensity = 6.05f;
            //     }
            // }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            m_ReturnHome = false;

            txt_Miss.text = "MISS:" + "0".ToString();

            m_NoteInGame.Clear();

            m_EnemyTurn = false;

            m_Continue = true;
            m_TrySong = false;

            m_FreePlayListId = 0;
            txt_Page.text = (m_FreePlayListId + 1).ToString();

            m_knot = 10;
            m_Knot = 10;

            m_PlayerNotes = 0;
            txt_PlayerNotes.text = m_PlayerNotes.ToString();

            m_EnemyNotes = 0;
            txt_EnemyNotes.text = m_EnemyNotes.ToString();

            txt_TotalGold.text = ProfileManager.GetGold();

            m_StorysongNo = 0;
            m_WeekNo = 1;

            // GUIManager.Instance.AddClickEvent(btn_Play, PlaySong);
            // GUIManager.Instance.AddClickEvent(btn_LeftSong, OnPrevSong);
            // GUIManager.Instance.AddClickEvent(btn_RightSong, OnNextSong);
            // GUIManager.Instance.AddClickEvent(btn_Pause, PauseSongPopup);
        }

        public void AddClickEvent()
        {
            // GUIManager.Instance.AddClickEvent(btn_Play, PlaySong);
            // GUIManager.Instance.AddClickEvent(btn_LeftSong, OnPrevSong);
            // GUIManager.Instance.AddClickEvent(btn_RightSong, OnNextSong);
            // GUIManager.Instance.AddClickEvent(btn_Pause, PauseSongPopup);
        }

        public void DecideWin()
        {
            if (m_TrySong)
            {
                Home();
            }
            else
            {
                if (m_knot < 20)
                {
                    if (m_ModePlay == ModePlay.STORY)
                    {
                        // if (IsStoryWeekEnd())
                        // {
                        //     UIManager.Instance.OpenWinPopup(true);
                        // }
                        // else
                        // {
                        //     // m_StorysongNo++;
                        //     // SongManager.Instance.defaultSong = m_WeekSongs[m_StorysongNo];
                        //     // SoundManager.Instance.PauseBGM();
                        //     // m_TrySong = false;

                        //     // WeekConfig song = m_WeekConfigs[m_StorysongNo];

                        //     // if (m_Enemy.gameObject != null)
                        //     // {
                        //     //     PrefabManager.Instance.DespawnPool(m_Enemy.gameObject);
                        //     // }

                        //     // GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
                        //     // enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
                        //     // enemy.transform.localPosition = Vector3.zero;
                        //     // enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);
                        //     // enemy.transform.localScale = new Vector3(1f, 1f, 1f);

                        //     // GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

                        //     // ResetSong();
                        //     UIManager.Instance.OpenWinPopup(true);
                        // }

                        UIManager.Instance.OpenWinPopup(true);
                    }
                    else if (m_ModePlay == ModePlay.FREEPLAY)
                    {
                        UIManager.Instance.OpenWinPopup(true);
                    }
                }
                else
                {
                    UIManager.Instance.OpenLosePopup(true);
                }
            }
        }

        public void NextWeekSong()
        {
            m_StorysongNo++;
            m_StorySongID = m_WeekConfigs[m_StorysongNo].m_Id;
            SongManager.Instance.defaultSong = m_WeekSongs[m_StorysongNo];
            SoundManager.Instance.PauseBGM();
            m_TrySong = false;

            WeekConfig song = m_WeekConfigs[m_StorysongNo];

            if (m_Enemy.gameObject != null)
            {
                PrefabManager.Instance.DespawnPool(m_Enemy.gameObject);
            }

            GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
            enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
            enemy.transform.localPosition = Vector3.zero;
            enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);
            enemy.transform.localScale = new Vector3(1f, 1f, 1f);

            GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

            UIManager.Instance.OpenDialoguePopup(false);

            // ResetSong();
        }

        public void PlaySong()
        {
            anim_UI.SetTrigger("Play");
            StartCoroutine(IEPlaySong());
        }

        public void PlaySongStory()
        {
            GUIManager.Instance.SetBlockPopup(true);
            SoundManager.Instance.PauseBGM();
            m_TrySong = false;

            WeekConfig song = m_WeekConfigs[m_StorysongNo];

            if (GameManager.Instance.m_Enemy != null)
            {
                Destroy(GameManager.Instance.m_Enemy.gameObject);
            }

            GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
            enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
            enemy.transform.localPosition = Vector3.zero;
            enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);
            enemy.transform.localScale = new Vector3(1f, 1f, 1f);
            GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

            PlaySong();
            // SongManager.Instance.PlaySong();
            StartCoroutine(IEDelaySong());
        }

        IEnumerator IEDelaySong()
        {
            yield return Yielders.Get(0.5f);
            SongManager.Instance.defaultSong = m_WeekSongs[m_StorysongNo];
            // yield return Yielders.Get(0.1f);
            // SongManager.Instance.PlaySong();
        }

        // public void PlaySong()
        // {
        //     SoundManager.Instance.PauseBGM();
        //     GameManager.Instance.PlaySong();
        //     GameManager.Instance.m_DefaultSong = songId;

        //     SongConfig song = GameData.Instance.GetSongConfig(songId);

        //     if (ProfileManager.GetSongProfiles(songId) != null)
        //     {
        //         GameManager.Instance.m_TrySong = false;
        //     }
        //     else
        //     {
        //         GameManager.Instance.m_TrySong = true;
        //     }

        //     GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
        //     enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
        //     enemy.transform.localPosition = Vector3.zero;
        //     enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);
        //     enemy.transform.localScale = new Vector3(1f, 1f, 1f);
        //     GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

        //     StartCoroutine(IEPlaySong());
        // }

        public void OnNextSong()
        {
            int songCount = GameData.Instance.GetSongCount();

            if (songCount > ((m_FreePlayListId + 1) * 4))
            {
                m_FreePlayListId++;
                txt_Page.text = (m_FreePlayListId + 1).ToString();
                EventManager.CallEvent(GameEvent.DISPLAY_SONG_INFO);
            }
        }

        public void OnPrevSong()
        {
            if (m_FreePlayListId > 0)
            {
                m_FreePlayListId--;
                txt_Page.text = (m_FreePlayListId + 1).ToString();
                EventManager.CallEvent(GameEvent.DISPLAY_SONG_INFO);
            }
        }

        IEnumerator IEPlaySong()
        {
            // SongManager.Instance.DisplayTime();
            // Time.timeScale = 1.5f;
            g_Ready.SetActive(false);
            g_Set.SetActive(false);
            g_Go.SetActive(false);
            yield return Yielders.Get(1.5f);
            SoundManager.Instance.PlaySoundOne();
            g_Ready.SetActive(true);
            yield return Yielders.Get(1.5f);
            SoundManager.Instance.PlaySoundTwo();
            g_Ready.SetActive(false);
            g_Set.SetActive(true);
            yield return Yielders.Get(1.5f);
            SoundManager.Instance.PlaySoundThree();
            g_Set.SetActive(false);
            g_Go.SetActive(true);
            yield return Yielders.Get(1.5f);
            // Time.timeScale = 1f;
            SoundManager.Instance.PlaySoundGo();
            g_Go.SetActive(false);
            g_Go2.SetActive(true);
            GUIManager.Instance.SetBlockPopup(false);
            yield return Yielders.Get(0.5f);
            g_Go2.SetActive(false);
            GameManager.Instance.txt_Time.gameObject.SetActive(true);
            SongManager.Instance.DisplayTime();
        }

        public void PauseSong()
        {
            // Time.timeScale = 0;
            SongManager.Instance.PauseSong();
        }

        public void PauseSongPopup()
        {
            SongManager.Instance.PauseSong();
            UIManager.Instance.OpenPausePopup(true);
        }

        public void ResetSong()
        {
            GameManager.Instance.m_Continue = true;
            txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();
            ComboSystem.Instance.UpdateComboDisplay();
            m_Enemy.SetAnimTrigger("Idle");
            ResetVsBar();
            StartCoroutine(IEResetSong());
            SongManager.Instance.delay = 4f;
            SongManager.Instance.StopSong(true);
            SongManager.Instance.PlaySong();
        }

        public void StopSong()
        {
            SoundManager.Instance.PlayBGM();
            GameManager.Instance.m_Continue = true;
            m_Enemy.SetAnimTrigger("Idle");
            ResetVsBar();
            SongManager.Instance.delay = 4f;
            SongManager.Instance.StopSong(true);
        }

        IEnumerator IEResetSong()
        {
            // SongManager.Instance.DisplayTime();

            g_Ready.SetActive(true);
            g_Set.SetActive(false);
            g_Go.SetActive(false);

            SoundManager.Instance.PlaySoundOne();

            yield return Yielders.Get(1.5f);

            SoundManager.Instance.PlaySoundTwo();
            g_Ready.SetActive(false);
            g_Set.SetActive(true);

            yield return Yielders.Get(1.5f);

            g_Set.SetActive(false);
            g_Go.SetActive(true);
            SoundManager.Instance.PlaySoundThree();

            yield return Yielders.Get(1.5f);

            g_Set.SetActive(false);
            g_Go.SetActive(true);
            SoundManager.Instance.PlaySoundGo();
            yield return Yielders.Get(0.2f);

            g_Go.SetActive(false);
            g_Go2.SetActive(true);

            GUIManager.Instance.SetBlockPopup(false);

            yield return Yielders.Get(0.2f);

            g_Go2.SetActive(false);

            GameManager.Instance.txt_Time.gameObject.SetActive(true);
            SongManager.Instance.DisplayTime();
        }

        public void ResumeSong()
        {
            // Time.timeScale = 1;
            GUIManager.Instance.SetBlockPopup(true);
            txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();
            ComboSystem.Instance.UpdateComboDisplay();
            StartCoroutine(IEResumeSong());
        }

        IEnumerator IEResumeSong()
        {
            g_Ready.SetActive(true);
            g_Set.SetActive(false);
            g_Go.SetActive(false);
            SoundManager.Instance.PlaySoundOne();
            yield return Yielders.Get(1f);
            g_Ready.SetActive(false);
            g_Set.SetActive(true);
            SoundManager.Instance.PlaySoundTwo();
            yield return Yielders.Get(1f);
            g_Set.SetActive(false);
            g_Go.SetActive(true);
            SoundManager.Instance.PlaySoundThree();
            yield return Yielders.Get(1f);
            SoundManager.Instance.PlaySoundGo();
            g_Go.SetActive(false);
            g_Go2.SetActive(true);
            SongManager.Instance.ResumeSong();
            GUIManager.Instance.SetBlockPopup(false);
            yield return Yielders.Get(0.5f);
            g_Go2.SetActive(false);
        }

        public void UpdatePlayerNotes(Note _note)
        {
            _note.m_NoteSetup.m_Miss = false;
            m_Player.SingAnim(_note);
            m_PlayerNotes++;
            // GameManager.Instance.m_Enemy.SetState(E_State.IDLE);
            txt_PlayerNotes.text = m_PlayerNotes.ToString();
            UpdateVsBar(true);
        }

        public void UpdateVsBar(bool _claim)
        {
            if (_claim)
            {
                if (m_knot > 0)
                {
                    m_knot--;
                    m_Knot--;
                    tf_Icon.DOMoveX(tf_Knots[m_knot].position.x, 0.5f);
                }
            }
            else
            {
                if (m_knot < 20)
                {
                    m_knot++;
                    m_Knot++;
                    tf_Icon.DOMoveX(tf_Knots[m_knot].position.x, 0.5f);
                }

                if (!m_TrySong)
                {
                    if (m_knot >= 20)
                    {
                        GameManager.Instance.PauseSong();
                        if (GameManager.Instance.m_Continue)
                        {
                            UIManager.Instance.OpenLoseContinuePopup(true);
                        }
                        else
                        {
                            UIManager.Instance.OpenLosePopup(true);
                        }
                    }
                }
            }
        }

        public void ResetVsBar()
        {
            m_knot = 10;
            m_Knot = 10;
            tf_Icon.DOMoveX(tf_Knots[m_knot].position.x, 0.5f);
        }

        public void UpdatePlayerNotes(bool _IsPlayer)
        {
            if (_IsPlayer)
            {
                m_Player.anim_Owner.SetTrigger("Hit");
                m_Player.m_Skin.materials[1] = mat_Hit;
                if (m_PlayerNotes > 0)
                {
                    m_PlayerNotes--;
                }
                UpdateVsBar(false);
                txt_PlayerNotes.text = m_PlayerNotes.ToString();
            }
            else
            {
                // m_Enemy.SetAnimTrigger("Sing");
                m_EnemyNotes++;
                txt_EnemyNotes.text = m_EnemyNotes.ToString();
            }
        }

        public void UpdateMaxNote()
        {
            m_Notes++;
            if (m_Notes >= m_NotesMax)
            {
                m_Notes = 0;
                m_PlayerTurn = !m_PlayerTurn;
            }
        }

        public void Home()
        {
            // SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
            GUIManager.Instance.LoadPlayScene();
        }

        public GameObject Sick()
        {
            return PrefabManager.Instance.SpawnNoteLevelPool("Sick", Vector3.zero);
        }

        public GameObject Good()
        {
            return PrefabManager.Instance.SpawnNoteLevelPool("Good", Vector3.zero);
        }

        public GameObject Bad()
        {
            return PrefabManager.Instance.SpawnNoteLevelPool("Bad", Vector3.zero);
        }

        public GameObject Shit()
        {
            return PrefabManager.Instance.SpawnNoteLevelPool("Shit", Vector3.zero);
        }

        public GameObject Miss()
        {
            EventManager.CallEvent(GameEvent.VIBRATE_HEAVY);
            return PrefabManager.Instance.SpawnNoteLevelPool("Miss", Vector3.zero);
        }

        #region STORY

        public bool IsStoryWeekEnd()
        {
            if (m_StorysongNo >= (m_WeekSongs.Count - 1))
            {
                return true;
            }
            return false;
        }

        #endregion

        public void CheckEnemyTurn()
        {
            if (!m_NoteInGame[0].m_NoteSetup.m_PlayerNote)
            {
                if (GameManager.Instance.m_Enemy.m_State == E_State.IDLE)
                {
                    GameManager.Instance.m_Enemy.SetState(E_State.SING);
                }
                GameManager.Instance.l_LightEnemy.intensity = 4.5f;
                GameManager.Instance.l_LightPlayer.intensity = 2.5f;
            }
            else
            {
                if (GameManager.Instance.m_Enemy.m_State == E_State.SING)
                {
                    GameManager.Instance.m_Enemy.SetState(E_State.IDLE);
                }
                GameManager.Instance.l_LightEnemy.intensity = 2f;
                GameManager.Instance.l_LightPlayer.intensity = 6.05f;
            }
        }
    }

    public enum ModePlay
    {
        STORY = 0,
        FREEPLAY = 1,
    }

    public enum StoryLevel
    {
        EASY = 0,
        NORMAL = 1,
        HARD = 2,
    }
}