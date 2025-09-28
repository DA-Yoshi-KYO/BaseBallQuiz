using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    int m_nCorrectIndex = 0;
    [SerializeField] 
    private QuestionDatabase dataBase;
    private QuestionData m_QuestionData;

    private GameObject m_Sentence;
    private GameObject[] m_Choices = new GameObject[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_nCorrectIndex = Random.Range(0, 3);
        Debug.Log("正解選択肢のインデックス" + m_nCorrectIndex);

        List<QuestionData> list = new();
        for (int i = 0; i < dataBase.m_QuestionDatas.Length; i++)
        {
            list.Add(dataBase.m_QuestionDatas[i]);
        }
        int QuestionIndex = Random.Range(0,list.Count);
        m_QuestionData = list[QuestionIndex];

        m_Sentence = GameObject.Find("Sentence");
        m_Sentence.GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.Sentence;
        
        List<int> indices = new List<int> { 0, 1, 2 };
        
        for (int i = 0; i < 4; i++)
        {
            int ChoiceNo = i + 1;
            m_Choices[i] = GameObject.Find("Choice" + ChoiceNo);
            
            if (i != m_nCorrectIndex)
            {
                int index = indices[Random.Range(0, indices.Count - 1)];
                m_Choices[i].GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.InCorrectChoice[index];
                indices.Remove(index);
            }
            else
            {
                m_Choices[i].GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.CorrectChoice;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(int index)
    {
        if (m_nCorrectIndex != index)
        {
            Debug.Log("不正解");
        }
        else
        {
            Debug.Log("正解");
        }
    }
}
