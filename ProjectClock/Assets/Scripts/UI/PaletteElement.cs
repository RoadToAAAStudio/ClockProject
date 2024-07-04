using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaletteElement : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _paletteImage;
    [SerializeField] private TextMeshProUGUI _paletteText;
    [SerializeField] private GameObject _lockPanel;

    private ShopPanel _shopPanel;
    private int _paletteCost;
    private int _paletteIndex;

    private void OnEnable()
    {
        _button.onClick.AddListener(PaletteButtonClicked);
    }

    private void OnDisable()
    {
        _button?.onClick.RemoveAllListeners();
    }

    public void Initialize(Sprite paletteImage, int paletteCost, int paletteIndex, ShopPanel shopPanel, bool isUnlocked)
    {
        _paletteImage.sprite = paletteImage;
        _paletteCost = paletteCost;
        _paletteText.text = paletteCost.ToString();
        _paletteIndex = paletteIndex;
        _shopPanel = shopPanel;

        if (isUnlocked)
        {
            UnlockPanel();
        }
    }

    private void PaletteButtonClicked()
    {
        // Set this as the currently selected palette
        _shopPanel.PreviewPalette(_paletteIndex);
    }

    public void UnlockPanel()
    {
        _lockPanel.SetActive(false);
    }
}
