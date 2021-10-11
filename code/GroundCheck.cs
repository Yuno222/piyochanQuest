using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //接地判定のゲッター
    public bool IsGround
    {
        get { return isGround;}
    }

    private void Update()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }

        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            isGroundEnter = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
            isGroundStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            isGroundExit = true;
    }


}
