using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : Singleton<InputController>
{
    public List<GameObject> hands = new List<GameObject>();
    private int index = 0;
    private float dotProduct = 0;
    private float oldProduct = float.NegativeInfinity;
    private bool canLose = false;
    private int tries = 0;

    public bool canCheck;
    public bool gameover = false;

    private void Start()
    {
        EventManagerTwoParams<GameObject, GameObject>.Instance.TriggerEvent("onNewClock", hands[index], null);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canCheck) return;
        dotProduct = Vector2.Dot(hands[index].transform.GetChild(0).right, (hands[index].transform.position - hands[index + 1].transform.position).normalized);
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (gameover) ReloadScene();


                CheckState checkState = Check();
                if (checkState != CheckState.UNSUCCESS)
                {
                    canLose = false;
                    tries = 0;
                    hands[index].GetComponent<Clock>().enabled = false;

                    index++;
                    oldProduct = float.NegativeInfinity;

                    hands[index].GetComponent<Clock>().enabled = true;
                    EventManagerTwoParams<GameObject, GameObject>.Instance.TriggerEvent("onNewClock", hands[index], index > 0 ? hands[(index - 1)] : hands[1]);
                    EventManager.Instance.TriggerEvent("onNewClock");

                    EventManagerOneParam<CheckState>.Instance.TriggerEvent("onNewClock", checkState);
                }
            }
        }

        if (dotProduct < 0 && oldProduct > dotProduct)
        {
            canLose = true;
        }

        if (canLose && oldProduct < dotProduct && dotProduct >= hands[index].GetComponent<Clock>().MaxDotProductAllowed())
        {
            tries++;
            if (tries == 2)
            {
                GameOver();
            }
            canLose = false;
        }

        oldProduct = dotProduct;
    }

    private CheckState Check()
    {
        if (dotProduct <= hands[index].GetComponent<Clock>().MaxDotProductAllowed())
        {
            if (dotProduct <= hands[index].GetComponent<Clock>().PerfectMaxDotProduct())
            {
                return CheckState.PERFECT;
            }
            return CheckState.SUCCESS;
        }

        GameOver();
        return CheckState.UNSUCCESS;
    }

    private void GameOver()
    {
        gameover = true;
        EventManager.Instance.TriggerEvent("onGameover");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

enum CheckState
{
    UNSUCCESS,
    SUCCESS,
    PERFECT
}
