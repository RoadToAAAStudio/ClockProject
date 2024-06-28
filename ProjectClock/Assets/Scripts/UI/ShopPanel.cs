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

    private int _previewPaletteIndex = 0;

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(ReturnButtonClicked);
        _selectButton.onClick.AddListener(SelectNewPalette);

        PopulateShop();
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();
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
        PlayerData.Instance.SelectedPaletteIndex = _previewPaletteIndex;
        Debug.Log("Selecting palette n. " + _previewPaletteIndex);
    }

    public void PreviewPalette(int index)
    {
        _previewPaletteIndex = index;
        EventManager<int>.Instance.Publish(EEventType.OnPalettePreviewChanged, _previewPaletteIndex);
        Debug.Log("Preview palette n. " + _previewPaletteIndex);
    }
}
