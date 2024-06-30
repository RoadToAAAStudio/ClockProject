using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private GameObject _palettePanel;
    [SerializeField] private PaletteElement _paletteElementPrefab;

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(ReturnButtonClicked);
        _selectButton.onClick.AddListener(SelectNewPalette);
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        PopulateShop();
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
            Instantiate(_paletteElementPrefab, _palettePanel.transform).Initialize(palettes[i].ShopIcon, palettes[i].Cost, i, this);
        }
    }

    public void SelectNewPalette()
    {
        // TODO: check the price and the currency the player currently holds
        PlayerData.Instance.SetSelectedPalette();
    }

    public void PreviewPalette(int index)
    {
        //EventManager<int>.Instance.Publish(EEventType.OnPalettePreviewChanged, _previewPaletteIndex);
        PlayerData.Instance.PreviewPaletteIndex = index;
    }
}
