using TMPro;
using UnityEngine;

public class SheetMetalSizeChanger : MonoBehaviour
{
    [SerializeField] private RectTransform _targetImage;   
    [SerializeField] private TMP_InputField _widthInput; 
    [SerializeField] private TMP_InputField _lenghtInput;
    [SerializeField] private SheetMetal _sheetMetal;

    void Start()
    {
        _widthInput.onValueChanged.AddListener(delegate { UpdateSize(); });
        _lenghtInput.onValueChanged.AddListener(delegate { UpdateSize(); });    
        
        UpdateSize();
    }

    public void UpdateSize()
    {      
        float finalWidth = _sheetMetal.Width;
        float finalLength = _sheetMetal.Length;

        _targetImage.sizeDelta = new Vector2(finalWidth, finalLength);
    }
}
