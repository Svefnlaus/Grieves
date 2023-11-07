using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dmgplayer : MonoBehaviour
{
    public Hpmanager hp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hp.TakeDamage(10);
        }
    }
}
