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
    [SerializeField] private TextMeshProUGUI _currencyText;
    [SerializeField] private GameObject _palettePanel;
    [SerializeField] private PaletteElement _paletteElementPrefab;
    [SerializeField] private ScrollRect _scrollRect;

    private List<PaletteElement> _paletteElements = new();

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(ReturnButtonClicked);
        _selectButton.onClick.AddListener(SelectNewPalette);

        EventManager<int>.Instance.Subscribe(EEventType.OnNewPaletteBought, UpdateShopVisual);
        EventManager<int>.Instance.Subscribe(EEventType.OnCurrencyChanged, UpdateCurrency);

        // Set the current palette element at the center of the scroll view
        if (_paletteElements.Count > 0)
            SnapScrollView(_paletteElements[PlayerDataManager.Instance.CurrentPaletteIndex]);


        UpdateShopVisual(false);
        UpdateCurrency(PlayerDataManager.Instance.Currency);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_currencyText.rectTransform);
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveAllListeners();
        _selectButton.onClick.RemoveAllListeners();

        EventManager<int>.Instance.Unsubscribe(EEventType.OnNewPaletteBought, UpdateShopVisual);
        EventManager<int>.Instance.Unsubscribe(EEventType.OnCurrencyChanged, UpdateCurrency);

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
        UpdateShopVisual(false);
    }

    public void PreviewPalette(int index)
    {
        if (PlayerDataManager.Instance.IsPaletteUnlocked(index))
        {
            _selectButtonText.text = "Select";
            UpdateShopVisual(PlayerDataManager.Instance.SelectedPaletteIndex != index);
        }
        else
        {
            _selectButtonText.text = "Buy";
            
            UpdateShopVisual(PlayerDataManager.Instance.Currency >= ConfigurationManager.Instance.PaletteAssets[index].Cost);
        }

        PlayerDataManager.Instance.PreviewPaletteIndex = index;
    }

    private void UpdateShopVisual(int index)
    {
        _selectButtonText.text = "Select";
        Color buttonColor = _selectButton.GetComponent<Image>().color;
        buttonColor.a = 0.3f;
        _selectButton.GetComponent<Image>().color = buttonColor;
        _paletteElements[index].UnlockPanel();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_currencyText.rectTransform);
    }

    private void UpdateShopVisual(bool isInteractable)
    {
        if (isInteractable)
        {
            _selectButton.enabled = true;
            Color buttonColor = _selectButton.GetComponent<Image>().color;
            buttonColor.a = 1f;
            _selectButton.GetComponent<Image>().color = buttonColor;
        }
        else
        {
            _selectButton.enabled = false;
            Color buttonColor = _selectButton.GetComponent<Image>().color;
            buttonColor.a = 0.3f;
            _selectButton.GetComponent<Image>().color = buttonColor;
        }
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

    private void UpdateCurrency(int currency)
    {
        _currencyText.text = currency.ToString();
    }
}
