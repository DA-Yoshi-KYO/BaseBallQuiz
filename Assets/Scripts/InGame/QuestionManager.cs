using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    int m_nCorrectIndex = 0;    // ����I�����̃C���f�b�N�X
    int m_nSelectIndex = 0;    // �������I�񂾑I�����̃C���f�b�N�X

    [SerializeField]
    [Header("��肪�i�[����Ă���f�[�^�x�[�X")]
    private QuestionDatabase dataBase;  // ���̃f�[�^�x�[�X
    private QuestionData m_QuestionData;    // �o�肷����̃f�[�^
    private QuestionKind m_eQuestionKind;   // �o�肷��W������
    private Vector2[] m_QuestionInitSize;

    private GameObject m_Sentence;  // ��蕶
    private GameObject[] m_Choices = new GameObject[4]; // �I����
    private GameObject[] m_KindSelectButton = new GameObject[4]; // �I����

    private GameObject m_KindSelect;
    private GameObject m_Question;
    private GameObject m_Ball;
    private GameObject m_Bat;
    private GameObject m_Timer;

    private GameObject m_SelectObject;

    [SerializeField]
    [Header("������������")]
    private float m_fLimitTime = 10.0f;
    private float m_fTime = 0.0f;

    private int m_nTextCount = 0;
    
    // �Q�[���t�F�[�Y
    enum GamePhase
    {
        KindSelect, // �W�������I��
        ToQuiz,     // �N�C�Y�Ɉڍs
        Quiz,   // �N�C�Y
        ToKindSelect    // �W�������I���Ɉڍs
    };
    GamePhase m_ePhase = GamePhase.KindSelect;

    enum ToQuizPhase
    {
        DownKindSelect, // �W�������I����ʂ����Ɉړ�
        ThrowBall,  // ����
        QuestionActive  // ��蕶�̕\��
    };
    ToQuizPhase m_eToQuizPhase = ToQuizPhase.DownKindSelect;

    enum ToKindSelectPhase
    {
        QuestionDisActive,
        BatMove,
        BatSwing,

    };
    ToKindSelectPhase m_eToKindSelectPhase = ToKindSelectPhase.QuestionDisActive;

    // �W��������
    string[] m_sKindName =
    {
        "�Z�E���[�O",
        "�p�E���[�O",
        "�ǔ��W���C�A���c"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �C���X�^���X�̎擾
        m_KindSelect = GameObject.Find("KindSelect");
        m_Question = GameObject.Find("Question");
        m_Sentence = GameObject.Find("Sentence");
        m_Ball = GameObject.Find("Ball");
        m_Bat = GameObject.Find("Bat");
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
        m_eToQuizPhase = ToQuizPhase.DownKindSelect;
        m_eToKindSelectPhase = ToKindSelectPhase.QuestionDisActive;

        // �^�C�}�[�̏�����
        m_fTime = 0.0f;

        // �W�������̒��I
        SelectKindRandomChoice();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (m_ePhase)
        {
            case GamePhase.KindSelect:
                m_Timer.GetComponent<Slider>().value = 1;
                break;
            case GamePhase.ToQuiz:
                ToQuizUpdate();
                break;
            case GamePhase.Quiz:
                const float nextTextTime = 0.1f;

                if (m_fTime >= nextTextTime * m_nTextCount && m_nTextCount < m_QuestionData.question.Sentence.Length)
                {
                    // ��蕶�̃e�L�X�g�擾�A�ꕶ�����\��
                    m_Sentence.GetComponent<TextMeshProUGUI>().text += m_QuestionData.question.Sentence[m_nTextCount];
                    m_nTextCount++;
                }

                if (m_fTime >= m_fLimitTime)
                {
                    m_ePhase = GamePhase.ToKindSelect;
                    m_fTime = 0.0f;
                }
                m_Timer.GetComponent<Slider>().value = 1 - m_fTime / m_fLimitTime;
                break;

            case GamePhase.ToKindSelect:
                ToKindSelectUpdate();
                break;
        }

        m_fTime += Time.deltaTime;
    }
    void SelectKindRandomChoice()
    {
        // �v���C���[�̐ݒ�ɉ����ďo�肷����𐧌�����
        // if ()

        for (int i = 0; i < 4; i++)
        {
            int random = UnityEngine.Random.Range(0, m_sKindName.Length);
            m_KindSelectButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_sKindName[random];
        }
    }

    void ToQuizUpdate()
    {
        const float EaseTime = 1f;

        switch (m_eToQuizPhase)
        {
            case ToQuizPhase.DownKindSelect:
                m_KindSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -800.0f * Easing.EaseInBack(m_fTime, 0.5f));
                if (m_fTime >= EaseTime)
                {
                    m_fTime = 0.0f;
                    m_eToQuizPhase = ToQuizPhase.ThrowBall;
                }
                break;
            case ToQuizPhase.ThrowBall:
                m_Ball.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                m_Ball.GetComponent<RectTransform>().anchoredPosition = Easing.Helmite(
                    m_fTime,
                    new Vector2(-71.0f, 277.0f), new Vector2(0.0f, -175.0f),
                    new Vector2(-1000.0f, -1000.0f), new Vector2(0.0f, 0.0f),
                    EaseTime);
                m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -m_fTime * 720);
                if (m_fTime >= EaseTime)
                {
                    
                    m_fTime = 0.0f;
                    m_eToQuizPhase = ToQuizPhase.QuestionActive;

                    GameObject.Find("Choice1").GetComponent<TextMeshProUGUI>().enabled = false;
                    GameObject.Find("Choice2").GetComponent<TextMeshProUGUI>().enabled = false;
                    GameObject.Find("Choice3").GetComponent<TextMeshProUGUI>().enabled = false;
                    GameObject.Find("Choice4").GetComponent<TextMeshProUGUI>().enabled = false;
                    GameObject.Find("Sentence").GetComponent<TextMeshProUGUI>().enabled = false;

                    RectTransform[] ChildCnt = m_Question.transform.GetComponentsInChildren<RectTransform>();
                    m_QuestionInitSize = new Vector2[ChildCnt.Length];
                    int childIndexSet = 0;
                    foreach (var item in ChildCnt)
                    {
                        m_QuestionInitSize.SetValue(item.sizeDelta, childIndexSet);
                        Debug.Log(item.gameObject.name);
                        childIndexSet++;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        m_Choices[i].GetComponent<Button>().interactable = true;
                    }
                }
                break;
            case ToQuizPhase.QuestionActive:
                int childIndex = 0;
                m_Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -390);
                m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -m_fTime * 720);
                m_Ball.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100) * (1 - Easing.EaseInQuint(m_fTime, 0.5f));

                foreach (var itr in m_Question.transform.GetComponentsInChildren<Transform>())
                {
                    itr.gameObject.GetComponent<RectTransform>().sizeDelta = m_QuestionInitSize[childIndex]* Easing.EaseInQuint(m_fTime, EaseTime);
                    childIndex++;
                }

                if (m_fTime >= EaseTime)
                {
                    GameObject.Find("Choice1").GetComponent<TextMeshProUGUI>().enabled = true;
                    GameObject.Find("Choice2").GetComponent<TextMeshProUGUI>().enabled = true;
                    GameObject.Find("Choice3").GetComponent<TextMeshProUGUI>().enabled = true;
                    GameObject.Find("Choice4").GetComponent<TextMeshProUGUI>().enabled = true;
                    GameObject.Find("Sentence").GetComponent<TextMeshProUGUI>().enabled = true;

                    m_Ball.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);

                    m_fTime = 0.0f;
                    m_eToQuizPhase = ToQuizPhase.DownKindSelect;
                    m_ePhase = GamePhase.Quiz;
                }
                    break;
        };
    }

    void ToKindSelectUpdate()
    {
        float EaseTime = 1f;

        switch (m_eToKindSelectPhase)
        {
            case ToKindSelectPhase.QuestionDisActive:
                GameObject.Find("Choice1").GetComponent<TextMeshProUGUI>().enabled = false;
                GameObject.Find("Choice2").GetComponent<TextMeshProUGUI>().enabled = false;
                GameObject.Find("Choice3").GetComponent<TextMeshProUGUI>().enabled = false;
                GameObject.Find("Choice4").GetComponent<TextMeshProUGUI>().enabled = false;
                GameObject.Find("Sentence").GetComponent<TextMeshProUGUI>().enabled = false;


                int childIndex = 0;
                foreach (var itr in m_Question.transform.GetComponentsInChildren<Transform>())
                {
                    if (itr.gameObject != m_SelectObject)
                    {
                        itr.gameObject.GetComponent<RectTransform>().sizeDelta = m_QuestionInitSize[childIndex] * (1.0f - Easing.EaseInQuint(m_fTime, EaseTime));
                    }
                    else
                    {
                        RectTransform selectTransform = itr.GetComponent<RectTransform>();
                        selectTransform.anchoredPosition = new Vector2(0, selectTransform.anchoredPosition.y) + new Vector2(1100, 0.0f) * Easing.EaseInQuint(m_fTime, 0.5f);
                    }

                    childIndex++;
                }

                m_Ball.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100) * Easing.EaseInQuint(m_fTime, 1.5f);
                if (m_fTime > 1.5f)
                {
                    m_eToKindSelectPhase = ToKindSelectPhase.BatMove;
                    m_fTime = 0.0f;
                }
                break;
            case ToKindSelectPhase.BatMove:
                m_Bat.GetComponent<RectTransform>().anchoredPosition = Easing.Helmite(
                    m_fTime, 
                    new Vector2(1200.0f, -650.0f), new Vector2(-235.0f, -81.0f),
                    new Vector2(1000.0f, 200.0f), new Vector2(0.0f, 300.0f), 
                    EaseTime);
                m_Bat.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.0f);
                m_Bat.transform.rotation = Quaternion.Euler(0, 0, m_fTime * 720.0f + 90.0f );

                if (m_fTime > EaseTime)
                {
                    m_eToKindSelectPhase = ToKindSelectPhase.BatSwing;
                    m_fTime = -0.5f;
                }
                break;
            case ToKindSelectPhase.BatSwing:
                EaseTime = 1.0f;
                m_Bat.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.0f);
                m_Bat.transform.rotation = Quaternion.Euler(0, 0, Easing.Linear(m_fTime, 0.2f) * 300.0f + 90.0f);

                if (m_Bat.transform.rotation.z >= 310.0f)
                {
                    m_Ball.GetComponent<RectTransform>().anchoredPosition = Easing.Helmite(
                    m_fTime - 0.2f,
                    new Vector2(0.0f, -175.0f), new Vector2(0.0f, 520.0f),
                    new Vector2(1000.0f, -1000.0f), new Vector2(0.0f, 0.0f),
                    EaseTime);
                }
                if (m_fTime > EaseTime + 0.2f)
                {
                    m_eToKindSelectPhase = ToKindSelectPhase.QuestionDisActive;
                    m_fTime = 0.0f;
                }
                break;
        }
    }

    public void Select(int index)
    {
        m_nSelectIndex = index;
        m_SelectObject = GameObject.Find("Button" + (m_nSelectIndex + 1).ToString());
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


        m_Sentence.GetComponent<TextMeshProUGUI>().text= m_QuestionData.question.Sentence;
        m_ePhase = GamePhase.ToKindSelect;
        m_fTime = 0.0f;
    }

    public void SelectKind(int selectIndex)
    {
        // �����̃{�^���̑I����������
        m_nCorrectIndex = UnityEngine.Random.Range(0, 3);
        Debug.Log("����I�����̃C���f�b�N�X" + m_nCorrectIndex);

        // �I�΂ꂽ�W���������璊�I������f�[�^�𒊏o
        List<QuestionData> list = new();
        switch (m_KindSelectButton[selectIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
        {
            case "�Z�E���[�O": m_eQuestionKind = QuestionKind.Central; break;
            case "�p�E���[�O": m_eQuestionKind = QuestionKind.Pacific; break;          
            case "�ǔ��W���C�A���c": m_eQuestionKind = QuestionKind.Giants; break;
            default: Debug.Log("�{�^�����o�^����Ă��܂���"); break;
        }

        for (int i = 0; i < dataBase.m_QuestionDatas.Length; i++)
        {
            if (dataBase.m_QuestionDatas[i].questionKind == m_eQuestionKind)
                list.Add(dataBase.m_QuestionDatas[i]);
        }

        // �N�C�Y�f�[�^�̒��I
        int QuestionIndex = UnityEngine.Random.Range(0, list.Count);
        m_QuestionData = list[QuestionIndex];

        m_Sentence.GetComponent<TextMeshProUGUI>().text = "";

        // �I�����������_���ɔz�u
        List<int> indices = new List<int> { 0, 1, 2 };  // �s����I�����̃C���f�b�N�X
        for (int i = 0; i < 4; i++)
        {
            if (i != m_nCorrectIndex)
            {
                // i == �s�����̑I�����Ȃ�s����I�����̃C���f�b�N�X���烉���_���ɒ��I���e�L�X�g���擾
                int index = indices[UnityEngine.Random.Range(0, indices.Count - 1)];
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

}
