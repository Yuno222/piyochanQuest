using System.Collections
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    PiyoController piyocon;
    GM gm;
    private float BulletSpeed=8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //飛んでいく方向
            if (transform.rotation.z == 0)
            {
                transform.position += new Vector3(BulletSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position -= new Vector3(BulletSpeed * Time.deltaTime, 0, 0);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Jaga") || collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
