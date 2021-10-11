using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PiyoController : MonoBehaviour
{
    GameObject piyochan;
    public GroundCheck ground;
    public GM gm;
    Rigidbody2D rig;
    public GameObject bullet;
    public Button StopButton;
    public Button FlyButton;
    public Button ShotButton;

    GameObject[] StopObjB;
    GameObject[] StopObjE;
    GameObject[] StopObjW;
    bool isGround = false;
    bool nowStop=false;
    bool FlyTrigger = false;
    bool CanFly = true;
    float MoveSpeed = 0.02f;
    float currenthp;
    float flytime;
    float flyingtime;
    float gravity;
    float ypos;
    float ClickCount;

    Vector3 piyoscale;

    public enum MOVEDIR
    {
        STOP,
        RIGHT,
        LEFT,
    }

    public bool NowStop
    {
        get { return nowStop; }
    }

    MOVEDIR dir = MOVEDIR.STOP;
    // Start is called before the first frame update
    void Start()
    {
        piyochan = GameObject.Find("piyochan");
        StopButton.interactable = false;
        piyoscale = new Vector3(piyochan.transform.localScale.x, piyochan.transform.localScale.y, 0);
        rig = piyochan.GetComponent<Rigidbody2D>();
        gravity = rig.gravityScale;
        flytime = 2.0f;
        flyingtime = 0;
        ypos = 0;
        ClickCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //接地判定
        isGround = ground.IsGround;

        if (isGround == true)
        {
            ClickCount = 0;
        }

        //ジャンプボタン2回制限
        if(ClickCount >= 2)
        {
            FlyButton.interactable = false;
        }
        else
        {
            FlyButton.interactable = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Stop();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Stop();
        }
    }

    private void FixedUpdate()
    {
        if (dir == MOVEDIR.RIGHT)
        {
            piyochan.transform.Translate(MoveSpeed, 0, 0);
        }
        else if (dir == MOVEDIR.LEFT)
        {
            piyochan.transform.Translate(-MoveSpeed, 0, 0);
        }

        //飛んでいる時の処理
        if (FlyTrigger == true)
        {
            piyochan.transform.position = new Vector3(piyochan.transform.position.x, ypos + Mathf.PingPong(Time.time, 0.5f), piyochan.transform.position.z);
            rig.gravityScale = 0;
            flyingtime += Time.deltaTime;
            if (flyingtime >= flytime)
            {
                rig.gravityScale = gravity;
                flyingtime = 0;
                FlyTrigger = false;
                CanFly = false;
            }
        }
    }

    public void MoveRight()
    {
        dir = MOVEDIR.RIGHT;
        piyochan.transform.localScale = piyoscale;
    }

    public void MoveLeft()
    {
        dir = MOVEDIR.LEFT;
        piyochan.transform.localScale = new Vector3(piyoscale.x * -1, piyoscale.y, 0);
    }
    
    public void Stop()
    {
        dir = MOVEDIR.STOP;
    }

    public void Shot()
    {
        if (piyochan.transform.localScale.x >= 0)
        {
            Instantiate(bullet, piyochan.transform.position + new Vector3(1, 0.35f, 0), Quaternion.identity);
        }

        if (piyochan.transform.localScale.x <= 0)
        {
            Instantiate(bullet, piyochan.transform.position + new Vector3(-1, 0.35f, 0), Quaternion.Euler(0, 0, 180));
        }
    }

    public void Jump()
    {
        if (ClickCount < 2)
        {
            if (isGround == true)
            {
                CanFly = true;
                flyingtime = 0;
                rig.velocity = new Vector3(rig.velocity.x, 6.5f, 0);
            }
            else if (CanFly == true)
            {
                FlyTrigger = true;
                ypos = piyochan.transform.position.y;
                rig.velocity = new Vector3(0, 0, 0);
            }
        }
    }

    public void FalseFly()
    {
        if (ClickCount < 2)
        {
            ClickCount ++;
            rig.gravityScale = gravity;
            FlyTrigger = false;
        }
    }

    public void QuickMove()
    {
        if (gm.JagaStock > 0)
        {
            nowStop = true;
            gm.DisplayStopPanel();
            StopButton.interactable = false;
            ShotButton.interactable = false;
            gm.DecreaseJaga();
            StopObjB = GameObject.FindGameObjectsWithTag("Ebullet");
            StopObjE = GameObject.FindGameObjectsWithTag("Enemy");
            StopObjW = GameObject.FindGameObjectsWithTag("Water");

            //弾を止める
            foreach (GameObject soB in StopObjB)
            {
                soB.GetComponent<EnemyBulletController>().StopBullet();
            }

            //敵を止める
            foreach (GameObject soE in StopObjE)
            {
                if (soE.name == "pompom")
                {
                    soE.GetComponent<PomController>().StopCreateBullet();
                    soE.GetComponent<PomController>().StopMove(PomController.MOVESTATE.STOP);
                }

                if(soE.name == "Enemy")
                {
                    soE.GetComponent<EnemyController>().StopCreateBullet();
                }
            }

            //水を止める
            foreach (GameObject soW in StopObjW)
            {
                soW.GetComponent<Animator>().enabled = false;
            }

            Invoke("NormalMove", 5);
        }
        else return;
    }

    public void NormalMove()
    {
        nowStop = false;
        gm.HideStopPanel();
        ShotButton.interactable = true;

        //弾を動かす
        foreach (GameObject soB in StopObjB)
        {
            soB.GetComponent<EnemyBulletController>().StartBullet();
        }

        //敵キャラを動かす
        foreach (GameObject soE in StopObjE)
        {
            if (soE.name == "pompom")
            {
                soE.GetComponent<PomController>().StopMove(PomController.MOVESTATE.MOVE);
                soE.GetComponent<PomController>().StartCreateBullet();
            }

            if (soE.name == "Enemy")
            {
                soE.GetComponent<EnemyController>().StartCreateBullet();
            }
        }

        //水を動かす
        foreach (GameObject soW in StopObjW)
        {
            soW.GetComponent<Animator>().enabled = true;
        }

    }
}
