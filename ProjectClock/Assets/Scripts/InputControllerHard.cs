using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputControllerHard : MonoBehaviour
{
    public List<GameObject> hands = new List<GameObject>();
    private int index = 0;
    public float precision = -0.5f;
    private float dotProduct = 0;
    private float oldProduct = float.NegativeInfinity;
    private bool canLose = false;

    private void Awake()
    {
        hands[0].GetComponent<Clock>().enabled = true;
    }

    private void Start()
    {
        EventManagerOneParam<GameObject>.Instance.TriggerEvent("onNewClock", hands[index]);
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
                    canLose = false;
                    hands[index].GetComponent<Clock>().enabled = false;

                    index++;
                    oldProduct = float.NegativeInfinity;
                    if (index >= hands.Count - 1)
                    {
                        index = 0;
                        hands.Reverse();
                    }

                    hands[index].GetComponent<Clock>().enabled = true;
                    EventManagerOneParam<GameObject>.Instance.TriggerEvent("onNewClock", hands[index]);
                }
            }
        }

        if (dotProduct < 0 && oldProduct > dotProduct)
        {
            canLose = true;
        }

        if (canLose && oldProduct < dotProduct)
        {
            GameOver();
        }

        oldProduct = dotProduct;
    }

    private bool Check()
    {
        if (dotProduct <= precision && oldProduct > dotProduct)
        {
            return true;
        }

        GameOver();
        return false;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
