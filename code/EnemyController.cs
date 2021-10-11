using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject EnemyBullet1;
    public GameObject piyochan;
    SpriteRenderer sr;
    public GM gm;

    bool isBlink;
    float blinkspan = 0;
    public float mylife;

    private Vector3 myScale;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        isBlink = false;
        InvokeRepeating("CreateBullet", 3, 3.5f);
        myScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Playerの位置によって、Enemyの左右の向きを変える
        if (piyochan.transform.position.x > this.transform.position.x)
        {
            this.transform.localScale = new Vector3(myScale.x*-1, myScale.y, 1);
        }

        else
        {
            this.transform.localScale = new Vector3(myScale.x , myScale.y, 1);
        }

        //攻撃を受けた際の点滅処理
        if (isBlink)
        {
            if (blinkspan >= 0.1)
            {
                sr.enabled = true;
                isBlink = false;
                blinkspan = 0;
            }
            blinkspan += Time.deltaTime;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //palyerが衝突したらゲームオーバー
        if (collision.gameObject.CompareTag("Player"))
        {
            gm.IsAlive = false;
        }

        //playerの弾が衝突した際の処理
        if (collision.gameObject.CompareTag("Pbullet"))
        {
            mylife -= 1;
            sr.enabled = false;
            isBlink = true;
            if (mylife <= 0)
            {
                CancelInvoke();
                Destroy(this.gameObject);
            }
        }
    }


    public void CreateBullet()
    {
        Instantiate(EnemyBullet1, this.transform.position, Quaternion.identity);
    }

    public void StopCreateBullet()
    {
        CancelInvoke();
    }

    public void StartCreateBullet()
    {
        InvokeRepeating("CreateBullet", 1, 2);
    }
}
