using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    int m_nCorrectIndex = 0;    // ����I�����̃C���f�b�N�X

    [SerializeField]
    [Header("��肪�i�[����Ă���f�[�^�x�[�X")]
    private QuestionDatabase dataBase;  // ���̃f�[�^�x�[�X
    private QuestionData m_QuestionData;    // �o�肷����̃f�[�^
    private QuestionKind m_eQuestionKind;   // �o�肷��W������

    private GameObject m_Sentence;  // ��蕶
    private GameObject[] m_Choices = new GameObject[4]; // �I����
    private GameObject[] m_KindSelectButton = new GameObject[4]; // �I����

    private GameObject m_KindSelect;
    private GameObject m_Question;
    private GameObject m_Ball;
    private GameObject m_Timer;

    [SerializeField]
    [Header("������������")]
    private float m_fLimitTime = 10.0f;
    private float m_fTime = 0.0f;
    private bool m_bBallIn = true;
    
    // �Q�[���t�F�[�Y
    enum GamePhase
    {
        KindSelect, // �W�������I��
        ToQuiz,     // �N�C�Y�Ɉڍs
        Quiz,   // �N�C�Y
        ToKindSelect    // �W�������I���Ɉڍs
    };
    GamePhase m_ePhase = GamePhase.KindSelect;

    // �W��������
    string[] m_sKindName =
    {
        "�Z�E���[�O",
        "�p�E���[�O"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �C���X�^���X�̎擾
        m_KindSelect = GameObject.Find("KindSelect");
        m_Question = GameObject.Find("Question");
        m_Sentence = GameObject.Find("Sentence");
        m_Ball = GameObject.Find("Ball");
        m_Timer = GameObject.Find("Timer");
        m_Timer.GetComponent<Slider>().interactable = false;
        for (int i = 0; i < 4; i++)
        {
            int index = i + 1;
            m_Choices[i] = GameObject.Find("Button" + index);
            m_KindSelectButton[i] = GameObject.Find("SelectButton" + index);
        }

        // �W�������ʖ�萔 �̃f�o�b�O�\��
        int[] questionNum = new int[(int)QuestionKind.Max];
        for (int i = 0; i < dataBase.m_QuestionDatas.Length; i++)
        {
            questionNum[(int)dataBase.m_QuestionDatas[i].questionKind]++;
        }
        Debug.Log("�W�������ʖ�萔");
        for (int i = 0; i < (int)QuestionKind.Max; i++)
        {
            Debug.Log((QuestionKind)i + "�F" + questionNum[i]);
        }

        // �g�����X�t�H�[���̏�����
        m_Ball.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        m_KindSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        m_Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(1200, -390);

        // �W�������I������X�^�[�g
        m_ePhase = GamePhase.KindSelect;
        
        // �^�C�}�[�̏�����
        m_fTime = 0.0f;

        // �{�[����Ԃ̏�����
        m_bBallIn = true;

        // �W�������̒��I
        SelectKindRandomChoice();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(m_ePhase)
        {
            case GamePhase.KindSelect:
                m_Timer.GetComponent<Slider>().value = 1;
                break;
            case GamePhase.ToQuiz:
                const float EaseTime = 1f;
                const float MaxSize = 1800f;
                float size = 0.0f;
                if (m_bBallIn)
                {
                    size = MaxSize * Easing.EaseOutQuint(m_fTime, EaseTime);
                    m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, m_fTime * 360);
                    if (m_fTime >= EaseTime)
                    {
                        m_KindSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(1200, 0);
                        m_Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -390);
                        m_fTime = 0.0f;
                        m_bBallIn = false;
                    }
                }
                else
                {
                    size = MaxSize - MaxSize * Easing.EaseOutQuint(m_fTime, EaseTime);
                    m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, (m_fTime + EaseTime) * 360);
                    if (m_fTime >= EaseTime)
                    {
                        m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                        m_ePhase = GamePhase.Quiz;
                        m_fTime = 0.0f;
                        m_bBallIn = true;

                        for (int i = 0; i < 4; i++)
                        {
                            m_Choices[i].GetComponent<Button>().interactable = true;
                        }
                        return;
                    }
                }
                m_Ball.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                break;
            case GamePhase.Quiz:
                if (m_fTime >= m_fLimitTime)
                {
                    m_ePhase = GamePhase.ToKindSelect;
                    m_fTime = 0.0f;
                }
                m_Timer.GetComponent<Slider>().value = 1 - m_fTime / m_fLimitTime;
                break;

            case GamePhase.ToKindSelect:
                if (m_bBallIn)
                {
                    size = MaxSize * Easing.EaseOutQuint(m_fTime, EaseTime);
                    m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, m_fTime * 360);
                    if (m_fTime >= EaseTime)
                    {
                        m_KindSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                        m_Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(1200, -390);
                        m_fTime = 0.0f;
                        m_bBallIn = false;
                    }
                }
                else
                {
                    size = MaxSize - MaxSize * Easing.EaseOutQuint(m_fTime, EaseTime);
                    m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, (m_fTime + EaseTime) * 360);
                    if (m_fTime >= EaseTime)
                    {
                        m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                        m_ePhase = GamePhase.KindSelect;
                        m_fTime = 0.0f;
                        m_bBallIn = true;

                        for (int i = 0; i < 4; i++)
                        {
                            m_KindSelectButton[i].GetComponent<Button>().interactable = true;
                        }

                        // �W�������̒��I
                        SelectKindRandomChoice();
                        return;
                    }
                }
                m_Ball.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                break;
        }

        m_fTime += Time.deltaTime;
    }

    public void Select(int index)
    {

        if (m_nCorrectIndex != index)
        {
            Debug.Log("�s����");
        }
        else
        {
            Debug.Log("����");
        }

        for (int i = 0; i < 4; i++)
        {
            m_Choices[i].GetComponent<Button>().interactable = false;
        }


        
        m_ePhase = GamePhase.ToKindSelect;
        m_fTime = 0.0f;
    }

    public void SelectKind(int selectIndex)
    {
        // �����̃{�^���̑I����������
        m_nCorrectIndex = Random.Range(0, 3);
        Debug.Log("����I�����̃C���f�b�N�X" + m_nCorrectIndex);

        // �I�΂ꂽ�W���������璊�I������f�[�^�𒊏o
        List<QuestionData> list = new();
        switch (m_KindSelectButton[selectIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
        {
            case "�Z�E���[�O": m_eQuestionKind = QuestionKind.Central; break;
            case "�p�E���[�O": m_eQuestionKind = QuestionKind.Pacific; break;          
        }

        for (int i =0; i < dataBase.m_QuestionDatas.Length; i++)
        {
            if (dataBase.m_QuestionDatas[i].questionKind == m_eQuestionKind)
                list.Add(dataBase.m_QuestionDatas[i]);
        }

        // �N�C�Y�f�[�^�̒��I
        int QuestionIndex = Random.Range(0, list.Count);
        m_QuestionData = list[QuestionIndex];

        // ��蕶�̃e�L�X�g�擾
        m_Sentence.GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.Sentence;

        // �I�����������_���ɔz�u
        List<int> indices = new List<int> { 0, 1, 2 };  // �s����I�����̃C���f�b�N�X
        for (int i = 0; i < 4; i++)
        {
            if (i != m_nCorrectIndex)
            {
                // i == �s�����̑I�����Ȃ�s����I�����̃C���f�b�N�X���烉���_���ɒ��I���e�L�X�g���擾
                int index = indices[Random.Range(0, indices.Count - 1)];
                m_Choices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.InCorrectChoice[index];

                // �I�΂ꂽ�C���f�b�N�X�͍폜����
                indices.Remove(index);
            }
            else
            {
                // i == �����̑I�����Ȃ炻�̂܂܃e�L�X�g���擾
                m_Choices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.CorrectChoice;
            }
        }


        for (int i = 0; i < 4; i++)
        {
            m_KindSelectButton[i].GetComponent<Button>().interactable = false;
        }
        // �N�C�Y�ւ̑J�ڂɈڍs
        m_ePhase = GamePhase.ToQuiz;
        m_fTime = 0.0f;
    }

    void SelectKindRandomChoice()
    {
        // �v���C���[�̐ݒ�ɉ����ďo�肷����𐧌�����
        // if ()

        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, m_sKindName.Length);
            m_KindSelectButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_sKindName[random];
        }
    }
}
