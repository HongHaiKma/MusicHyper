using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmGameStarter;
using DG.Tweening;

public class NoteSetup : MonoBehaviour
{
    public Note m_Note;

    public bool m_Miss = true;

    public bool m_PlayerNote;
    public GameObject[] m_NoteSprites;

    public TrackTypes m_TrackTypes;
    public SpriteRenderer m_NotePlayer;
    public SpriteRenderer m_NoteEnemy;
    public GameObject g_NotePlayer;
    public GameObject g_NoteEnemy;
    public SpriteRenderer sr_LongLine;

    public bool m_IsAdded;

    private void OnEnable()
    {
        m_IsAdded = false;
        // m_NotePlayer.no
        g_NotePlayer.SetActive(true);
        g_NotePlayer.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        g_NoteEnemy.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        m_Miss = true;
    }

    public void ClassifyAllNote(Note _note)
    {
        // m_PlayerNote = GameManager.Instance.m_PlayerTurn;
        if (_note.action == Note.NoteAction.Swipe)
        {
            m_PlayerNote = false;
        }
        else
        {
            m_PlayerNote = true;
        }

        m_NotePlayer.color = Color.white;
        m_NoteEnemy.color = Color.white;
        m_NotePlayer.sprite = SpriteManager.Instance.m_PlayerArrows[(int)m_TrackTypes];
        m_NoteEnemy.sprite = SpriteManager.Instance.m_EnemyArrows[(int)m_TrackTypes];

        if (m_Note.action == Note.NoteAction.LongPress)
        {
            if (m_TrackTypes == TrackTypes.LEFT)
            {
                // sr_LongLine.material.SetColor("_GlowColor", Color.red);
                sr_LongLine.sprite = SpriteManager.Instance.m_LongNote[0];
            }
            else if (m_TrackTypes == TrackTypes.RIGHT)
            {
                // sr_LongLine.material.SetColor("_GlowColor", Color.cyan);
                sr_LongLine.sprite = SpriteManager.Instance.m_LongNote[1];
            }
            else if (m_TrackTypes == TrackTypes.UP)
            {
                // sr_LongLine.material.SetColor("_GlowColor", Color.blue);
                sr_LongLine.sprite = SpriteManager.Instance.m_LongNote[2];
            }
            else if (m_TrackTypes == TrackTypes.DOWN)
            {
                // sr_LongLine.material.SetColor("_GlowColor", Color.green);
                sr_LongLine.sprite = SpriteManager.Instance.m_LongNote[3];
            }
        }

        int count = m_NoteSprites.Length;
        for (int i = 0; i < count; i++)
        {
            ClassifySingleNote(i);
        }
    }

    public void ClassifySingleNote(int _index)
    {
        int count = m_NoteSprites.Length;
        if (m_PlayerNote)
        {
            if (_index == count - 1)
            {
                m_NoteSprites[_index].SetActive(false);
            }
            else
            {
                m_NoteSprites[_index].SetActive(true);
            }
        }
        else
        {
            if (_index == count - 1)
            {
                m_NoteSprites[_index].SetActive(false);
            }
            else
            {
                m_NoteSprites[_index].SetActive(false);
            }
        }
    }
}

public enum TrackTypes
{
    LEFT = 0,
    DOWN,
    UP,
    RIGHT
}