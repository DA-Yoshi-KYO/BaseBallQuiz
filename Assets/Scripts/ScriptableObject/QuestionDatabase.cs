using System;
using UnityEngine;

[Serializable]
public struct QuestionKind
{
    public int difficult;
    public bool isCentral;
    public bool isPacific;
}

[Serializable]
public struct Question
{
    public string Sentence;
    public string CorrectChoice;
    public string[] InCorrectChoice;
}

[Serializable]
public struct QuestionData
{
    [Header("���̃W������")]
    public QuestionKind questionKind;
    [Header("���")]
    public Question question;
}


[CreateAssetMenu(fileName = "QuestionDatabase", menuName = "Scriptable Objects/QuestionDatabase")]
public class QuestionDatabase : ScriptableObject
{
    public QuestionData[] m_QuestionDatas;
}
