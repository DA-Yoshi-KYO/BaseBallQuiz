using System;
using UnityEngine;


[Serializable]
public enum QuestionKind
{
    Central,
    Pacific,

    Max
}

[Serializable]
public struct Question
{
    public int difficult;
    public string Sentence;
    public string CorrectChoice;
    public string[] InCorrectChoice;
}

[Serializable]
public struct QuestionData
{
    [Header("–â‘è‚ÌƒWƒƒƒ“ƒ‹")]
    public QuestionKind questionKind;
    [Header("–â‘è")]
    public Question question;
}


[CreateAssetMenu(fileName = "QuestionDatabase", menuName = "Scriptable Objects/QuestionDatabase")]
public class QuestionDatabase : ScriptableObject
{
    public QuestionData[] m_QuestionDatas;
}
