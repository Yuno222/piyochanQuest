using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public GameObject fadepanel;
    float red, green, blue;
    float alfa;

    bool isfadeout = false;
    bool isfadein = true;
    float fadespeed;

    public bool Isfadeout
    {
        set { isfadeout = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        red = fadepanel.GetComponent<Image>().color.r;
        green = fadepanel.GetComponent<Image>().color.g;
        blue = fadepanel.GetComponent<Image>().color.b;
        alfa = fadepanel.GetComponent<Image>().color.a;

        fadespeed = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        //フェードアウト処理
        if (isfadeout == true)
        {
            fadepanel.SetActive(true);
            fadepanel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += fadespeed * Time.deltaTime;
            if (alfa > 0.8)
            {
                isfadeout = false;
                isfadein = true;
                //遷移するシーン先
                switch (SceneManager.GetActiveScene().name)
                {

                    case "TitleScene":
                        SceneManager.LoadScene("GameScene1");
                        break;

                    case "GameScene1":
                        SceneManager.LoadScene("GameScene2");
                        break;

                    case "GameScene2":
                        SceneManager.LoadScene("VsPom");
                        break;

                    case "VsPom":
                        SceneManager.LoadScene("ClearScene");
                        break;

                    case "ClearScene":
                        SceneManager.LoadScene("TitleScene");
                        break;

                    case "GameOver":
                        SceneManager.LoadScene("TitleScene");
                        break;
                }
            }
        }

        //フェードインの処理
        if (isfadein == true)
        {
            if (alfa >= 0)
            {
                fadepanel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
                alfa -= fadespeed * Time.deltaTime;
            }

            else
            {
                isfadein = false;
                fadepanel.SetActive(false);
            }
        }
    }
}
