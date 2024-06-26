using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button _returnButton;
    [SerializeField] private GameObject _palettePanel;
    [SerializeField] private PaletteElement _paletteElementPrefab;

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(ReturnButtonClicked);

        PopulateShop();
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveAllListeners();
    }

    public void ReturnButtonClicked()
    {
        EventManager.Instance.Publish(EEventType.OnReturnButtonPressed);
    }

    private void PopulateShop()
    {
        PaletteAsset[] palettes = ConfigurationManager.Instance.PaletteAssets;
        for (int i = 0; i < palettes.Length; i++)
        {
            Instantiate(_paletteElementPrefab, _palettePanel.transform);
        }
    }
}
