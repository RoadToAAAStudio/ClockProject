using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaletteElement : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _paletteImage;
    [SerializeField] private TextMeshProUGUI _paletteText;

    private ShopPanel _shopPanel;
    private int _paletteIndex;
    public int PaletteIndex {  get { return _paletteIndex; } }

    private void OnEnable()
    {
        _button.onClick.AddListener(PaletteButtonClicked);
    }

    private void OnDisable()
    {
        _button?.onClick.RemoveAllListeners();
    }

    public void Initialize(Sprite paletteImage, string paletteCost, int paletteIndex, ShopPanel shopPanel)
    {
        _paletteImage.sprite = paletteImage;
        _paletteText.text = paletteCost;
        _paletteIndex = paletteIndex;
        _shopPanel = shopPanel;
    }

    private void PaletteButtonClicked()
    {
        // Set this as the currently selected palette
    }
}
