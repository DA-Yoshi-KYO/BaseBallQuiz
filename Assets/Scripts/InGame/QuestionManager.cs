using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    int m_nCorrectIndex = 0;    // 正解選択肢のインデックス

    [SerializeField]
    [Header("問題が格納されているデータベース")]
    private QuestionDatabase dataBase;  // 問題のデータベース
    private QuestionData m_QuestionData;    // 出題する問題のデータ
    private QuestionKind m_eQuestionKind;   // 出題するジャンル

    private GameObject m_Sentence;  // 問題文
    private GameObject[] m_Choices = new GameObject[4]; // 選択肢
    private GameObject[] m_KindSelectButton = new GameObject[4]; // 選択肢

    private GameObject m_KindSelect;
    private GameObject m_Question;
    private GameObject m_Ball;
    private GameObject m_Timer;

    [SerializeField]
    [Header("問題を解く時間")]
    private float m_fLimitTime = 10.0f;
    private float m_fTime = 0.0f;
    private bool m_bBallIn = true;
    
    // ゲームフェーズ
    enum GamePhase
    {
        KindSelect, // ジャンル選択
        ToQuiz,     // クイズに移行
        Quiz,   // クイズ
        ToKindSelect    // ジャンル選択に移行
    };
    GamePhase m_ePhase = GamePhase.KindSelect;

    // ジャンル名
    string[] m_sKindName =
    {
        "セ・リーグ",
        "パ・リーグ"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // インスタンスの取得
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

        // ジャンル別問題数 のデバッグ表示
        int[] questionNum = new int[(int)QuestionKind.Max];
        for (int i = 0; i < dataBase.m_QuestionDatas.Length; i++)
        {
            questionNum[(int)dataBase.m_QuestionDatas[i].questionKind]++;
        }
        Debug.Log("ジャンル別問題数");
        for (int i = 0; i < (int)QuestionKind.Max; i++)
        {
            Debug.Log((QuestionKind)i + "：" + questionNum[i]);
        }

        // トランスフォームの初期化
        m_Ball.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        m_KindSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        m_Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(1200, -390);

        // ジャンル選択からスタート
        m_ePhase = GamePhase.KindSelect;
        
        // タイマーの初期化
        m_fTime = 0.0f;

        // ボール状態の初期化
        m_bBallIn = true;

        // ジャンルの抽選
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

                        // ジャンルの抽選
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
            Debug.Log("不正解");
        }
        else
        {
            Debug.Log("正解");
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
        // 正解のボタンの選択肢を決定
        m_nCorrectIndex = Random.Range(0, 3);
        Debug.Log("正解選択肢のインデックス" + m_nCorrectIndex);

        // 選ばれたジャンルから抽選しうるデータを抽出
        List<QuestionData> list = new();
        switch (m_KindSelectButton[selectIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
        {
            case "セ・リーグ": m_eQuestionKind = QuestionKind.Central; break;
            case "パ・リーグ": m_eQuestionKind = QuestionKind.Pacific; break;          
        }

        for (int i =0; i < dataBase.m_QuestionDatas.Length; i++)
        {
            if (dataBase.m_QuestionDatas[i].questionKind == m_eQuestionKind)
                list.Add(dataBase.m_QuestionDatas[i]);
        }

        // クイズデータの抽選
        int QuestionIndex = Random.Range(0, list.Count);
        m_QuestionData = list[QuestionIndex];

        // 問題文のテキスト取得
        m_Sentence.GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.Sentence;

        // 選択肢をランダムに配置
        List<int> indices = new List<int> { 0, 1, 2 };  // 不正解選択肢のインデックス
        for (int i = 0; i < 4; i++)
        {
            if (i != m_nCorrectIndex)
            {
                // i == 不正解の選択肢なら不正解選択肢のインデックスからランダムに抽選しテキストを取得
                int index = indices[Random.Range(0, indices.Count - 1)];
                m_Choices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.InCorrectChoice[index];

                // 選ばれたインデックスは削除する
                indices.Remove(index);
            }
            else
            {
                // i == 正解の選択肢ならそのままテキストを取得
                m_Choices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_QuestionData.question.CorrectChoice;
            }
        }


        for (int i = 0; i < 4; i++)
        {
            m_KindSelectButton[i].GetComponent<Button>().interactable = false;
        }
        // クイズへの遷移に移行
        m_ePhase = GamePhase.ToQuiz;
        m_fTime = 0.0f;
    }

    void SelectKindRandomChoice()
    {
        // プレイヤーの設定に応じて出題する問題を制限する
        // if ()

        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, m_sKindName.Length);
            m_KindSelectButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_sKindName[random];
        }
    }
}
