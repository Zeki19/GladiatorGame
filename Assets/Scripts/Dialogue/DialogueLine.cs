using System;
using UnityEngine;

[Serializable]
public class DialogueLine //BEWARE: when modifying this, there is a script in assets/editor that has a reference to it --> Talk with Zeki.
{
    public string speakerName; //this should be an EnumCharacters or something like that.
    public Sprite speakerImage;
    
    [TextArea(3,10)]
    public string sentence;
    
    public bool cameraShake;
    public bool shakeAtTheStart;
    
    public AudioClip soundToPlay;
    public bool playAtTheStart;
}