using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private TextMeshProUGUI _selectButtonText;
    [SerializeField] private GameObject _palettePanel;
    [SerializeField] private PaletteElement _paletteElementPrefab;

    private List<PaletteElement> _paletteElements;

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(ReturnButtonClicked);
        _selectButton.onClick.AddListener(SelectNewPalette);

        EventManager<int>.Instance.Subscribe(EEventType.OnNewPaletteBought, UpdateShopVisual);
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();

        EventManager<int>.Instance.Unsubscribe(EEventType.OnNewPaletteBought, UpdateShopVisual);
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
        _paletteElements = new();
        for (int i = 0; i < palettes.Length; i++)
        {
            PaletteElement paletteElement = Instantiate(_paletteElementPrefab, _palettePanel.transform);
            _paletteElements.Add(paletteElement);
            paletteElement.Initialize(palettes[i].ShopIcon, palettes[i].Cost, i, this, PlayerDataManager.Instance.IsPaletteUnlocked(i));
        }
    }

    public void SelectNewPalette()
    {
        PlayerDataManager.Instance.SetSelectedPalette();
    }

    public void PreviewPalette(int index)
    {
        PlayerDataManager.Instance.PreviewPaletteIndex = index;

        if (PlayerDataManager.Instance.IsPaletteUnlocked(index))
            _selectButtonText.text = "Select";
        else
            _selectButtonText.text = "Buy";
    }

    private void UpdateShopVisual(int index)
    {
        _selectButtonText.text = "Select";
        _paletteElements[index].UnlockPanel();
    }
}
