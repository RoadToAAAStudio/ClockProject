using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>();
    private int index = 0;
    public float precision = -0.5f;

    private void Awake()
    {
        hands[0].GetComponent<Clock>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Check())
                {
                    hands[index].GetComponent<Clock>().enabled = false;
                    
                    if (index >= hands.Count)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    
                    hands[index].GetComponent<Clock>().enabled = true;
                }
            }
        }
    }

    //private bool Check()
    //{
    //    Vector2 handDirection = (hands[index].transform.endPosition - hands[index].transform.position).normalized;
    //    Vector2 secondHandDirection = (hands[index + 1].transform.endPosition - hands[index].transform.position).normalized;
    //    float dotProduct = Vector2.Dot(handDirection, secondHandDirection);

    //    if (dotProduct >= precision)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    private bool Check()
    {
        if (index + 1 >= hands.Count)
        {
            return false;
        }

        float dotProduct = Vector2.Dot(hands[index].transform.GetChild(0).up, hands[index + 1].transform.GetChild(0).up);
        
        if (dotProduct <= precision)
        {
            return true;
        }

        return false;
    }
}
