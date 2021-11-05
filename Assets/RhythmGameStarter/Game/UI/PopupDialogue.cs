using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using RhythmGameStarter;

namespace RhythmGameStarter
{
    public class PopupDialogue : MonoBehaviour
    {
        public GameObject g_Char;
        public Image img_Enemy;
        public TextMeshProUGUI txt_Dialogue;
        public int m_DialogueID;
        public List<Dialogue> m_Dialogues = new List<Dialogue>();
        public TextMeshProUGUI txt_EnemyName;
        public Button btn_Next;
        public Button btn_Skip;

        // IEnumerator m_Cou;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_Next, NextDialogue);
            GUIManager.Instance.AddClickEvent(btn_Skip, Skip);
        }

        private void OnEnable()
        {
            // m_Cou = DisplayDialogue();
            m_DialogueID = 0;
            m_Dialogues.Clear();

            if (GameManager.Instance.m_WeekNo < 4)
            {
                m_Dialogues = GameData.Instance.GetDialogueBySongID(GameManager.Instance.m_StorySongID);
            }
            else
            {
                m_Dialogues = GameData.Instance.GetDialogueBySongID(GameManager.Instance.m_StorySongID - 1);
            }

            StartCoroutine(DisplayDialogue());
            // StartCoroutine(m_Cou);
            txt_EnemyName.text = GameData.Instance.GetSongConfig(GameManager.Instance.m_StorySongID).m_EnemyName;

            if (GameManager.Instance.m_WeekNo == 4)
            {
                img_Enemy.sprite = SpriteManager.Instance.m_Enemies[4];
            }
            else if (GameManager.Instance.m_WeekNo == 3)
            {
                img_Enemy.sprite = SpriteManager.Instance.m_Enemies[3];
            }
            else
            {
                img_Enemy.sprite = SpriteManager.Instance.m_Enemies[m_Dialogues[m_DialogueID].m_EnemyTurn];
            }

            // img_Enemy.SetNativeSize();
            Helper.DebugLog("Enemy Turn: " + m_Dialogues[m_DialogueID].m_EnemyTurn);
            if (m_Dialogues[m_DialogueID].m_EnemyTurn != 0)
            {
                img_Enemy.gameObject.SetActive(true);
                g_Char.SetActive(false);
            }
            else
            {
                img_Enemy.gameObject.SetActive(false);
                g_Char.SetActive(true);
            }

            SongConfig songs = GameData.Instance.GetSongConfig(GameManager.Instance.m_StorySongID);

            WeekConfig song = GameManager.Instance.m_WeekConfigs[GameManager.Instance.m_StorysongNo];

            if (GameManager.Instance.m_StorysongNo == 0)
            {
                if (GameManager.Instance.m_Enemy != null)
                {
                    Destroy(GameManager.Instance.m_Enemy.gameObject);
                    Helper.DebugLog("= NULLL");
                }

                if (GameManager.Instance.m_WeekNo != 1)
                {
                    GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
                    enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
                    enemy.transform.localPosition = Vector3.zero;

                    enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);

                    enemy.transform.localScale = new Vector3(1f, 1f, 1f);
                    GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();

                    Helper.DebugLog("SPQNWWWWWWWW");
                }
            }
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.C))
        //     {
        //         if (m_Dialogues[m_DialogueID].m_EnemyTurn != 1)
        //         {
        //             img_Enemy.gameObject.SetActive(true);
        //             g_Char.SetActive(false);
        //         }
        //         else
        //         {
        //             img_Enemy.gameObject.SetActive(false);
        //             g_Char.SetActive(true);
        //         }
        //     }
        // }

        public void Skip()
        {
            UIManager.Instance.CloseDialoguePopup();
            if (GameManager.Instance.m_1stSong)
            {
                GameManager.Instance.PlaySongStory();
                // GameManager.Instance.PlaySongStoryNext();
            }
            else
            {
                GameManager.Instance.ResetSong();
            }
        }

        public void NextDialogue()
        {
            // StopCoroutine(m_Cou);
            if (m_DialogueID < m_Dialogues.Count - 1)
            {
                m_DialogueID++;
                // txt_Dialogue.text = m_Dialogues[m_DialogueID].m_Dialogue;
                StartCoroutine(DisplayDialogue());
                // StartCoroutine(m_Cou);
                if (m_Dialogues[m_DialogueID].m_EnemyTurn != 0)
                {
                    // img_Enemy.sprite = SpriteManager.Instance.m_Enemies[m_Dialogues[m_DialogueID].m_EnemyTurn];
                    if (GameManager.Instance.m_WeekNo == 4)
                    {
                        img_Enemy.sprite = SpriteManager.Instance.m_Enemies[4];
                    }
                    else if (GameManager.Instance.m_WeekNo == 3)
                    {
                        img_Enemy.sprite = SpriteManager.Instance.m_Enemies[3];
                    }
                    else
                    {
                        img_Enemy.sprite = SpriteManager.Instance.m_Enemies[m_Dialogues[m_DialogueID].m_EnemyTurn];
                    }
                    img_Enemy.gameObject.SetActive(true);
                    g_Char.SetActive(false);
                }
                else
                {
                    img_Enemy.gameObject.SetActive(false);
                    g_Char.SetActive(true);
                }
                // img_Enemy.SetNativeSize();
            }
            else
            {
                UIManager.Instance.CloseDialoguePopup();
                if (GameManager.Instance.m_1stSong)
                {
                    GameManager.Instance.PlaySongStory();
                    // GameManager.Instance.PlaySongStoryNext();
                }
                else
                {
                    GameManager.Instance.ResetSong();
                }
            }
        }

        IEnumerator DisplayDialogue()
        {
            btn_Next.interactable = false;
            txt_Dialogue.text = "";
            string dialogue = m_Dialogues[m_DialogueID].m_Dialogue;
            Helper.DebugLog(dialogue);
            int count = dialogue.Length;
            int i = 0;
            while (i < count)
            {
                txt_Dialogue.text += dialogue[i];
                i++;
                yield return Yielders.Get(Time.deltaTime / 4);
            }
            btn_Next.interactable = true;
        }
    }
}