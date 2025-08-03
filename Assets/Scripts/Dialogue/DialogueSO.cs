using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "New dialogue/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public EnumDialogues dialogueName; 
    public List<DialogueLine> lines;
}
