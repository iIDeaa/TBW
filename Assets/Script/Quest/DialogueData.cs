using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Game/Dialogue")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class Line
    {
        public string speaker;
        [TextArea] public string text;
        public Sprite portrait;
        public Sprite tellimage; 
    }

    public Line[] lines;
}
