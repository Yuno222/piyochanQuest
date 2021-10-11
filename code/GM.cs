using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public GameObject piyochan;
    public Button StopButton;
    public GameObject StopPanel;
    public Text jaga;
    private float jagaStock;

    private bool isAlive = true;

    public float JagaStock
    {
        get { return jagaStock; }
    }

    public bool IsAlive
    {
        set { isAlive = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        jagaStock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == false)
        {
            Destroy(piyochan);
            Invoke("GameOver", 1);
        }
    }

    public void DecreaseJaga()
    {
        jagaStock -= 1;
        jaga.text = "" + jagaStock;
        if (jagaStock == 0)
        {
            StopButton.interactable = false;
        }
    }

    public void IncreaseJaga()
    {
        jagaStock += 1;
        jaga.text = "" + jagaStock;
        if (jagaStock > 0)
        {
            StopButton.interactable = true;
        }
    }

    public void DisplayStopPanel()
    {
        StopPanel.SetActive(true);
    }

    public void HideStopPanel()
    {
        StopPanel.SetActive(false);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}
