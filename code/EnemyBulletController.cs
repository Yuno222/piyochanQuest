using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    GameObject GM;
    GameObject piyochan;
    private float BulletSpeed = 0.01f;
    Vector3 piyoPos;
    Vector3 dir;

    public enum MoveState
    {
        Move,
        Stop,
    }

    MoveState nowstate;

    
    // Start is called before the first frame update
    void Start()
    {
        piyochan = GameObject.Find("piyochan");
        GM = GameObject.Find("GameManager");
        piyoPos = piyochan.transform.position;
        dir = piyoPos - this.transform.position;
        nowstate = MoveState.Move;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (nowstate == MoveState.Move)
        {
            this.transform.position += dir * BulletSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //判定を行わないオブジェクト
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Ebullet")|| collision.gameObject.CompareTag("Jaga"))
        {
            return;
        }

        //Move状態の時のみ衝突判定(弾のみ)
        if (nowstate == MoveState.Move)
        {
            //アタッチしているオブジェクトがEbulletの場合
            if (this.gameObject.CompareTag("Ebullet"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    GM.GetComponent<GM>().IsAlive = false;
                }
            }
            Destroy(this.gameObject);
        }

        //アタッチしているオブジェクトがJagaの場合
        if (this.gameObject.CompareTag("Jaga"))
        {
            GM.GetComponent<GM>().IncreaseJaga();
            Destroy(this.gameObject);
        }

        

    }

    public void StopBullet()
    {
        nowstate = MoveState.Stop;
    }

    public void StartBullet ()
    {
        nowstate = MoveState.Move;
    }

}
