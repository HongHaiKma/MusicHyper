using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class P_Face : StateMachineBehaviour
    {
        Character m_Char;
        public int face;

        private void Awake()
        {
            m_Char = GameManager.Instance.m_Player;
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // if (m_Char != null)
            // {
            //     m_Char.m_Skin.materials[1] = m_Char.mat_Idle;
            // }
            // else
            // {
            //     if (GameManager.Instance.m_Player != null)
            //     {
            //         m_Char = GameManager.Instance.m_Player;
            //         m_Char.m_Skin.materials[1] = m_Char.mat_Idle;
            //     }
            // }

            if (m_Char == null)
            {
                m_Char = GameManager.Instance.m_Player;
            }
            m_Char.SetFace(face);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}