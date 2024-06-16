using RoadToAAA.ProjectClock.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public void ReturnButton()
    {
        EventManager.Instance.Publish(EEventType.OnShopReturnButtonPressed);
    }
}
