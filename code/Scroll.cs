using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    float ScrollSpeed = 1f;
    Vector3 nowpos;

    // Start is called before the first frame update
    void Start()
    {
        nowpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-ScrollSpeed*Time.deltaTime, 0, 0);
        if(transform.position.x <= -28.5f)
        {
            transform.position = new Vector3(10.13f,nowpos.y,nowpos.z);
        }
    }
}
