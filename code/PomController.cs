using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PomController : MonoBehaviour
{
    public FadeScript fs;
    public GameObject PomBullet;
    public GameObject jaga;
    SpriteRenderer sr;
    Rigidbody2D rig;
    
    float MoveSpeed=0.07f;
    float blinkspan = 0;
    float currenthp = 50;
    bool isBlink;

    public enum MOVEDIR
    {
        RIGHT,
        LEFT,
    }

    public enum MOVESTATE
    {
        MOVE,
        STOP,
    }

    MOVEDIR dir = MOVEDIR.LEFT;
    MOVESTATE state = MOVESTATE.MOVE;

    //stateのセッター
    public void StopMove(MOVESTATE State)
    {
        state = State;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        rig = this.GetComponent<Rigidbody2D>();
        InvokeRepeating("CreateBullet", 5, 5f);
        isBlink = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= 7.8)
        {
            dir = MOVEDIR.LEFT;
        }

        if (transform.position.x <= -7.6)
        {
            dir = MOVEDIR.RIGHT;
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

        //死亡処理
        if(currenthp <= 0)
        {
            state = MOVESTATE.STOP;
            rig.constraints = RigidbodyConstraints2D.None;
            CancelInvoke();
            StartCoroutine("WaitDestroy");
        }
    }


    private void FixedUpdate()
    {
        if (state == MOVESTATE.MOVE)
        {
            if (dir == MOVEDIR.LEFT)
            {
                transform.Translate(-MoveSpeed, 0, 0);
            }

            else if (dir == MOVEDIR.RIGHT)
            {
                transform.Translate(MoveSpeed, 0, 0);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pbullet"))
        {
            currenthp -= 10;
            sr.enabled = false;
            isBlink = true;
        }
    }


    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(2.3f);
        Destroy(this.gameObject);
        fs.Isfadeout = true;
    }

    public void CreateBullet()
    {
        //1-10までのランダムな数字を生成
        int bullettype = Random.Range(1, 11);

        //1/20でじゃがりこ生成
        if (bullettype == 10)
        {
            Instantiate(jaga, this.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(PomBullet, this.transform.position, Quaternion.identity);
        }
    }

    public void StopCreateBullet()
    {
        CancelInvoke();
    }

    public void StartCreateBullet()
    {
        InvokeRepeating("CreateBullet", 5, 5f);
    }
}
