using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class E_Face : StateMachineBehaviour
    {
        public int face;
        Enemy m_Enemy;

        void Awake()
        {
            m_Enemy = GameManager.Instance.m_Enemy;
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (m_Enemy == null)
            {
                m_Enemy = GameManager.Instance.m_Enemy;
            }
            m_Enemy.SetFace(face);
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