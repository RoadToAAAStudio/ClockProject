using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    [SerializeField] private float MovingSpeed = 0.5f;


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + MovingSpeed * Time.deltaTime);
    }
}
