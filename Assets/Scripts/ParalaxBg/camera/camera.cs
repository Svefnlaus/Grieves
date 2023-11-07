using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    public Transform target;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);   
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }
}
