using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILook
{
    void LookDir(Vector2 dir);
    void LookSpeedMultiplier(float mult);
    public void PlayStateAnimation(StateEnum state);
}
