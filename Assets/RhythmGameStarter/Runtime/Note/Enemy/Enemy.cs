using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmGameStarter;

public class Enemy : MonoBehaviour
{
    public Animator anim_Owner;
    public E_State m_State;
    public Material face;

    public Texture[] faces;

    private void OnEnable()
    {
        EventManager1<E_State>.AddListener(GameEvent.ENEMY_STATE, SetState);
        EventManager1<E_State>.AddListener(GameEvent.ENEMY_STATE_VARIABLE, SetStateVariable);

        m_State = E_State.IDLE;
        SetFace(0);
    }

    private void OnDisable()
    {
        EventManager1<E_State>.RemoveListener(GameEvent.ENEMY_STATE, SetState);
        EventManager1<E_State>.RemoveListener(GameEvent.ENEMY_STATE_VARIABLE, SetStateVariable);
    }

    private void OnDestroy()
    {
        EventManager1<E_State>.RemoveListener(GameEvent.ENEMY_STATE, SetState);
        EventManager1<E_State>.RemoveListener(GameEvent.ENEMY_STATE_VARIABLE, SetStateVariable);
    }

    public virtual void SetFace(int _face)
    {
        face.mainTexture = faces[_face];
    }

    public void SetStateVariable(E_State _state)
    {
        m_State = _state;
    }

    public virtual void SetState(E_State _state)
    {
        // if (_state != m_State)
        // {
        switch (_state)
        {
            case E_State.IDLE:
                if (m_State == E_State.SING)
                {
                    SetAnimTrigger("Idle");
                }
                break;
            case E_State.SING:
                if (m_State == E_State.IDLE)
                {
                    SetAnimTrigger("Sing");
                }
                break;
        }
        // }
    }

    public virtual void SetAnimTrigger(string _anim)
    {
        anim_Owner.SetTrigger(_anim);
    }
}

public enum E_State
{
    IDLE,
    SING,
}