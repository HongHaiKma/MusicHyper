using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class TargetBoundary : MonoBehaviour
    {
        private TrackManager trackManager;

        private void Awake()
        {
            trackManager = GetComponentInParent<TrackManager>();
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag == "Note")
            {
                Note note = col.GetComponent<Note>();
                // Note note = col.GetComponent<Note>();

                // if (noteSetup != null && noteSetup.m_Miss)
                if (note != null)
                {
                    if (note.m_NoteSetup.m_PlayerNote && note.m_NoteSetup.m_Miss)
                    {
                        StatsSystem.Instance.AddMissed(1);
                        GameManager.Instance.txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();
                        GameManager.Instance.UpdatePlayerNotes(true);
                        GameObject go = GameManager.Instance.Miss();
                        NoteLevel noteLevel = go.GetComponent<NoteLevel>();
                        noteLevel.SetPosition(note);
                    }
                    else
                    {
                        GameManager.Instance.UpdatePlayerNotes(false);
                    }
                    if (GameManager.Instance.m_NoteInGame.Count != 0)
                    {
                        // GameManager.Instance.m_NoteInGame.Remove(note);
                        // GameManager.Instance.CheckEnemyTurn();
                        EventManager.CallEvent(GameEvent.CHECK_ENEMY_TURN);
                    }
                }

                // if (GameManager.Instance.m_Enemy.m_State == E_State.IDLE)
                // {
                //     GameManager.Instance.m_Enemy.SetState(E_State.SING);
                //     // note.gameObject.SetActive(false);
                //     GameManager.Instance.l_LightEnemy.intensity = 4.5f;
                //     GameManager.Instance.l_LightPlayer.intensity = 2.5f;
                // }

                if (trackManager.useNotePool)
                {
                    trackManager.ResetNoteToPool(col.gameObject);
                }
                else
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}