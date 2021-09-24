using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class Character : MonoBehaviour
    {
        public Animator anim_Owner;
        public SkinnedMeshRenderer m_Skin;
        public Material mat_Idle;

        public Material face;

        public Texture[] faces;

        public void SetFace(int _face)
        {
            face.mainTexture = faces[_face];
        }

        public void SingAnim(Note _note)
        {
            // if (!GameManager.Instance.m_EnemyTurn)
            // {
            NoteSetup noteSetup = _note.m_NoteSetup;
            switch (noteSetup.m_TrackTypes)
            {
                case TrackTypes.LEFT:
                    anim_Owner.SetTrigger("1");
                    // m_Skin.materials[1] = GameManager.Instance.mat_Sing1;
                    break;
                case TrackTypes.RIGHT:
                    anim_Owner.SetTrigger("2");
                    // m_Skin.materials[1] = GameManager.Instance.mat_Sing2;
                    break;
                case TrackTypes.UP:
                    anim_Owner.SetTrigger("3");
                    // m_Skin.materials[1] = GameManager.Instance.mat_Sing3;
                    break;
                case TrackTypes.DOWN:
                    anim_Owner.SetTrigger("4");
                    // m_Skin.materials[1] = GameManager.Instance.mat_Sing4;
                    break;
                    // }
            }
        }
    }
}