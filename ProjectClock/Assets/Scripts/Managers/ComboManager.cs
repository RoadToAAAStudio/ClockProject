using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : Singleton<ComboManager>
{
    private ComboAsset[] comboAssets;
    private int comboIndex = 0;
    private int comboNumber = 0;

    private ComboAsset currentCombo;
    public ComboAsset CurrentCombo
    {
        get { return currentCombo; }
    }

    protected override void Awake()
    {
        base.Awake();
        comboAssets = Resources.LoadAll<ComboAsset>("ComboAssets");
        currentCombo = comboAssets[comboIndex];
    }

    private void OnEnable()
    {
        EventManagerOneParam<CheckState>.Instance.StartListening("onNewClock", ChangeCombo);
    }

    private void OnDisable()
    {
        EventManagerOneParam<CheckState>.Instance.StopListening("onNewClock", ChangeCombo);
    }

    private void ChangeCombo(CheckState state)
    {
        if (state != CheckState.COMBO)
        {
            comboIndex = 0;
            comboNumber = 0;
            currentCombo = comboAssets[comboIndex];
            return;
        }

        if (comboIndex >= comboAssets.Length - 1) return;
        
        comboNumber++;

        if (comboNumber >= currentCombo.numberOfComboToChangeAsset)
        {
            comboIndex++;
            comboNumber = 0;
            currentCombo = comboAssets[comboIndex];
        }
    }
}
