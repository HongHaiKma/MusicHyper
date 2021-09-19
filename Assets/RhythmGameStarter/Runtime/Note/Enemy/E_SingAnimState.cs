using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    public class E_SingAnimState : StateMachineBehaviour
    {
        Enemy m_Enemy;

        void Awake()
        {
            m_Enemy = GameManager.Instance.m_Enemy;
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // GameManager.Instance.l_LightEnemy.intensity = 4.5f;
            // GameManager.Instance.l_LightPlayer.intensity = 2.5f;
            if (m_Enemy == null)
            {
                m_Enemy = GameManager.Instance.m_Enemy;
            }
            m_Enemy.m_State = E_State.SING;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // GameManager.Instance.l_LightEnemy.intensity = 2f;
            // GameManager.Instance.l_LightPlayer.intensity = 6.05f;
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