using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(Rigidbody2D)))]
[RequireComponent(typeof(BoxCollider2D))]

public class Paralax : MonoBehaviour
{
    public BoxCollider2D imgCollider;

    [SerializeField] private float speed;
    private Rigidbody2D imageRB;
    private float imageLength;

    private void Start()
    {
        //Set background velocity
        imageRB = GetComponent<Rigidbody2D>();
        imageRB.velocity = new Vector2(-speed, 0);

        //Get background length
        imageLength = imgCollider.size.x;
    }

    private void Update()
    {
        //Create infinite scrolling background
        if (transform.position.x < -imageLength)
            RepositionBackground();
    }

    private void RepositionBackground()
    {
        Vector2 Offset = new Vector2(imageLength * 2f, 0);
        transform.position = (Vector2)transform.position + Offset;
    }
}