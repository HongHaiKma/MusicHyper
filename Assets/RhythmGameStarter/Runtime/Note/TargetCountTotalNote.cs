using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class TargetCountTotalNote : MonoBehaviour
    {
        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Note")
            {
                GameManager.Instance.UpdateMaxNote();

                Note note = col.GetComponent<Note>();

                // note.m_NoteSetup.m_PlayerNote = false;

                if (!note.m_NoteSetup.m_IsAdded)
                {
                    GameManager.Instance.m_NoteInGame.Add(note);
                    note.m_NoteSetup.m_IsAdded = true;
                }

                GameManager.Instance.CheckEnemyTurn();

                // if (note.action == Note.NoteAction.Swipe)
                // {
                //     // if (GameManager.Instance.m_EnemyTurn)
                //     // {
                //     //     GameManager.Instance.m_EnemyTurn = true;
                //     //     GameManager.Instance.m_Enemy.SetState(E_State.SING);
                //     //     note.gameObject.SetActive(false);
                //     //     GameManager.Instance.l_LightEnemy.intensity = 4.5f;
                //     //     GameManager.Instance.l_LightPlayer.intensity = 2.5f;
                //     // }

                //     GameManager.Instance.m_EnemyTurn = !GameManager.Instance.m_EnemyTurn;
                //     // GameManager.Instance.m_EnemyTurn = true;
                //     if (GameManager.Instance.m_EnemyTurn)
                //     {
                //         if (GameManager.Instance.m_Enemy.m_State == E_State.IDLE)
                //         {
                //             Helper.DebugLog("SINGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
                //             GameManager.Instance.m_Enemy.SetState(E_State.SING);
                //             note.gameObject.SetActive(false);
                //             GameManager.Instance.l_LightEnemy.intensity = 4.5f;
                //             GameManager.Instance.l_LightPlayer.intensity = 2.5f;
                //         }
                //     }
                //     else
                //     {
                //         if (GameManager.Instance.m_Enemy.m_State == E_State.SING)
                //         {
                //             GameManager.Instance.m_Enemy.SetState(E_State.IDLE);
                //             note.gameObject.SetActive(false);
                //             GameManager.Instance.l_LightEnemy.intensity = 2f;
                //             GameManager.Instance.l_LightPlayer.intensity = 6.05f;
                //         }
                //     }
                // }
                // else
                // {
                //     // GameManager.Instance.m_EnemyTurn = false;
                //     // if (!GameManager.Instance.m_EnemyTurn)
                //     // {
                //     if (GameManager.Instance.m_Enemy.m_State == E_State.SING)
                //     {
                //         GameManager.Instance.m_EnemyTurn = false;
                //         GameManager.Instance.m_Enemy.SetState(E_State.IDLE);
                //         // note.gameObject.SetActive(false);
                //         GameManager.Instance.l_LightEnemy.intensity = 2f;
                //         GameManager.Instance.l_LightPlayer.intensity = 6.05f;
                //         Helper.DebugLog("Player turnnnnnnnnnnnn");
                //     }
                //     // }
                // }

                NoteSetup noteSetup = col.GetComponent<NoteSetup>();
                if (noteSetup != null)
                {
                    noteSetup.ClassifyAllNote(note);
                }
            }
        }
    }
}