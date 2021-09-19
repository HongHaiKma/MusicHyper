using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace RhythmGameStarter
{
    public class Track : MonoBehaviour
    {
        public TrackTypes m_TrackTypes;
        public Transform lineArea;
        public NoteArea noteArea;

        [HideInInspector]
        public Transform notesParent;

        [HideInInspector]
        public IEnumerable<SongItem.MidiNote> allNotes;

        [HideInInspector]
        public AudioSource trackHitAudio;

        [HideInInspector]
        public List<Note> runtimeNote;

        public Transform tf_NoteImage;
        public SpriteRenderer m_SpriteRenderer;
        public Sprite m_OnNote;
        public Sprite m_OffNote;

        void Awake()
        {
            trackHitAudio = GetComponent<AudioSource>();
            notesParent = new GameObject("Notes").transform;
            notesParent.SetParent(transform);
            ResetTrackPosition();
        }

        private void ResetTrackPosition()
        {
            notesParent.transform.position = lineArea.position;
            notesParent.transform.localEulerAngles = Vector3.zero;
        }

        public GameObject CreateNote(GameObject prefab)
        {
            var note = Instantiate(prefab);
            note.transform.SetParent(notesParent);
            note.transform.localEulerAngles = Vector3.zero;

            var noteScript = note.GetComponent<Note>();
            noteScript.inUse = true;
            runtimeNote.Add(noteScript);
            return note;
        }

        public void AttachNote(GameObject noteInstance)
        {
            noteInstance.transform.SetParent(notesParent);
            noteInstance.transform.localEulerAngles = Vector3.zero;

            var note = noteInstance.GetComponent<Note>();
            var noteSetup = noteInstance.GetComponent<NoteSetup>();
            noteSetup.m_TrackTypes = m_TrackTypes;
            note.parentTrack = this;
            runtimeNote.Add(note);
        }

        public void DestoryAllNotes()
        {
            runtimeNote.Clear();
            foreach (Transform child in notesParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void RecycleAllNotes(TrackManager manager)
        {
            runtimeNote.Clear();

            var currentNotes = new List<Transform>();
            foreach (Transform child in notesParent)
            {
                currentNotes.Add(child);
            }
            currentNotes.ForEach(x =>
            {
                manager.ResetNoteToPool(x.gameObject);
            });
        }

        public void ResetTrack()
        {
            ResetTrackPosition();

            runtimeNote.Clear();

            noteArea.ResetNoteArea();
        }

        public void AnimateNote()
        {
            tf_NoteImage.DOScale(1f, 0.1f).OnComplete(
               () => tf_NoteImage.DOScale(0.5f, 0.1f)
            );

            float glow = 0.09f;
            DOTween.To(() => glow, x => glow = x, 0.09f, 0.1f).
            OnStart(
                () => m_SpriteRenderer.sprite = m_OnNote
            ).
            OnUpdate(
                () => m_SpriteRenderer.material.SetFloat("_HitEffectBlend", glow)
            ).
            OnComplete(
                () => DOTween.To(() => glow, x => glow = x, 0f, 0.1f).
                OnStart(
                () => m_SpriteRenderer.sprite = m_OffNote
                ).
                OnUpdate(
                    () => m_SpriteRenderer.material.SetFloat("_HitEffectBlend", glow)
                )
            );
        }
    }
}