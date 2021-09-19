using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim_Owner;
    public E_State m_State;

    private void OnEnable()
    {
        m_State = E_State.IDLE;
    }

    public void SetState(E_State _state)
    {
        // if (_state != m_State)
        // {
        switch (_state)
        {
            case E_State.IDLE:
                SetAnimTrigger("Idle");
                break;
            case E_State.SING:
                SetAnimTrigger("Sing");
                break;
        }
        // }
    }

    public void SetAnimTrigger(string _anim)
    {
        anim_Owner.SetTrigger(_anim);
    }
}

public enum E_State
{
    IDLE,
    SING,
}