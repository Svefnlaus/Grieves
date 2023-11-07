using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statuseffect : MonoBehaviour
{
    public GameObject petrified;
    public GameObject leech;

    // Start is called before the first frame update
    void Start()
    {
        petrified.SetActive(false);
        leech.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Activate());
        }

        else
        {
            leech.SetActive(false);
        }
    }

   IEnumerator Activate()
    {
        petrified.SetActive(true);

        yield return new WaitForSeconds(1f);

        petrified.SetActive(false);
    }

    
}
