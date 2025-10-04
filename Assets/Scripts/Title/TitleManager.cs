using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_OptionMenu;

    void Start()
    {
        m_OptionMenu.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ShowOption()
    {
        GameObject.Find("OptionShowButton").GetComponent<Button>().enabled = false;
        GameObject.Find("StartButton").GetComponent<Button>().enabled = false;
        m_OptionMenu.SetActive(true);
        GameObject.Find("OptionHideButton").GetComponent<Button>().enabled = true;
    }

    public void HideOption()
    {
        GameObject.Find("OptionShowButton").GetComponent<Button>().enabled = true;
        GameObject.Find("StartButton").GetComponent<Button>().enabled = true;
        GameObject.Find("OptionHideButton").GetComponent<Button>().enabled = false;
        m_OptionMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SceneGame");
    }
}
