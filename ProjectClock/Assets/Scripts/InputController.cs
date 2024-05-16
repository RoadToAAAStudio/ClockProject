using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>();
    private int index = 0;
    public float precision = -0.5f;
    float dotProduct = 0;
    private float oldProduct = float.NegativeInfinity;

    private void Awake()
    {
        hands[0].GetComponent<Clock>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        dotProduct = Vector2.Dot(hands[index].transform.GetChild(0).up, (hands[index].transform.position - hands[index + 1].transform.position));
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Check())
                {
                    hands[index].GetComponent<Clock>().enabled = false;
                    
                    index++;
                    oldProduct = float.NegativeInfinity;
                    if (index >= hands.Count - 1)
                    {
                        index = 0;
                        hands.Reverse();
                    }
                    
                    hands[index].GetComponent<Clock>().enabled = true;
                }
            }
        }
        oldProduct = dotProduct;
    }

    //private bool Check()
    //{
    //    float dotProduct = Vector2.Dot(hands[index].transform.GetChild(0).up, hands[index + 1].transform.GetChild(0).up);
        
    //    if (dotProduct <= precision)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    private bool Check()
    {
        if (dotProduct <= precision && oldProduct > dotProduct)
        {
            return true;
        }
        
        return false;
    }

}
