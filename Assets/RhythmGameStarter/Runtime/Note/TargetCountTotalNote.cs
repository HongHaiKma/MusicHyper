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
                // EventManager.CallEvent(GameEvent.CHECK_ENEMY_TURN);

                NoteSetup noteSetup = col.GetComponent<NoteSetup>();
                if (noteSetup != null)
                {
                    noteSetup.ClassifyAllNote(note);
                }
            }
        }
    }
}