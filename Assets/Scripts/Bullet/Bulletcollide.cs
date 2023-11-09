using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletcollide : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource bold;

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bold.Play();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);

        }
      

    }

}