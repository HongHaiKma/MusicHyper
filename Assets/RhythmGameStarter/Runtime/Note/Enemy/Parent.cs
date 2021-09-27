using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : Enemy
{
    public Animator anim_Owner2;
    public Material face2;
    public Texture[] faces2;

    public override void SetFace(int _face)
    {
        face.mainTexture = faces[_face];
        face2.mainTexture = faces2[_face];
    }

    public override void SetAnimTrigger(string _anim)
    {
        base.SetAnimTrigger(_anim);
        anim_Owner2.SetTrigger(_anim);
    }
}
