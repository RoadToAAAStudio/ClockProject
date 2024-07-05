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

    [SerializeField] private ScrollRect _scrollRect;

    private List<PaletteElement> _paletteElements = new();

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(ReturnButtonClicked);
        _selectButton.onClick.AddListener(SelectNewPalette);

        EventManager<int>.Instance.Subscribe(EEventType.OnNewPaletteBought, UpdateShopVisual);

        // Set the current palette element at the center of the scroll view
        if (_paletteElements.Count > 0)
            SnapScrollView(_paletteElements[PlayerDataManager.Instance.CurrentPaletteIndex]);
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();

        EventManager<int>.Instance.Unsubscribe(EEventType.OnNewPaletteBought, UpdateShopVisual);

        // Resets the position of all the elements in the scroll view
        if (_paletteElements.Count > 0)
            _scrollRect.content.anchoredPosition = Vector2.zero;
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
            PaletteElement paletteElement = Instantiate(_paletteElementPrefab, _palettePanel.transform);
            _paletteElements.Add(paletteElement);
            paletteElement.Initialize(palettes[i].ShopIcon, palettes[i].Cost, i, this, PlayerDataManager.Instance.IsPaletteUnlocked(i));
        }

        SnapScrollView(_paletteElements[PlayerDataManager.Instance.CurrentPaletteIndex]);
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

    private void SnapScrollView(PaletteElement selectedElement)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 contentPos = _scrollRect.transform.InverseTransformPoint(_scrollRect.transform.position);
        Vector2 elementPos = _scrollRect.transform.InverseTransformPoint(selectedElement.transform.position);
        Vector2 endPos = contentPos - elementPos;
        endPos.y = contentPos.y;
        _scrollRect.content.anchoredPosition = endPos;
    }
}
