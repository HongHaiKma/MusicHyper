using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace RhythmGameStarter
{
    public class NoteLevel : MonoBehaviour
    {
        private void OnEnable()
        {
            // transform.SetParent(GameManager.Instance.parent_UI);
            // // transform.localPosition = GameManager.Instance.tf_End.localPosition;
            // transform.localScale = new Vector3(1f, 1f, 1f);
            // transform.DOScale(1.3f, 0.3f).OnComplete(
            //     () => transform.DOScale(1f, 0.1f).OnComplete(
            //         () => PrefabManager.Instance.DespawnPool(gameObject)
            //     )
            // );
        }

        public void SetPosition(Note _note)
        {
            transform.SetParent(GameManager.Instance.parent_UI);
            if (_note.m_NoteSetup.m_TrackTypes == TrackTypes.LEFT)
            {

                transform.localPosition = GameManager.Instance.tf_Left.localPosition;
            }
            else if (_note.m_NoteSetup.m_TrackTypes == TrackTypes.RIGHT)
            {
                transform.localPosition = GameManager.Instance.tf_Right.localPosition;
            }
            else if (_note.m_NoteSetup.m_TrackTypes == TrackTypes.UP)
            {

                transform.localPosition = GameManager.Instance.tf_Up.localPosition;
            }
            else if (_note.m_NoteSetup.m_TrackTypes == TrackTypes.DOWN)
            {
                transform.localPosition = GameManager.Instance.tf_Down.localPosition;
            }

            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.DOScale(1.3f, 0.3f).OnComplete(
                () => transform.DOScale(1f, 0.1f).OnComplete(
                    () => PrefabManager.Instance.DespawnPool(gameObject)
                )
            );
        }
    }
}