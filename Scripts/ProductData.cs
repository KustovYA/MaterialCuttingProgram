using TMPro;
using UnityEngine;

public class ProductData : MonoBehaviour
{
    [SerializeField] private TMP_InputField _width;
    [SerializeField] private TMP_InputField _lenght;
    [SerializeField] private TMP_InputField _count;
    [SerializeField] private TMP_Text _possibleCount;
      
    private int _currentPlaced;

    public float Width => float.TryParse(_width.text, out float width) ? width : 0;
    public float Length => float.TryParse(_lenght.text, out float lenght) ? lenght : 0;
        public int Count => int.TryParse(_count.text, out int count) ? count : 0;
        
    public void UpdateDisplay(int count)
    {
        _currentPlaced = count;
        _possibleCount.text = _currentPlaced.ToString();
    }

    public void AddOne()
    {
        _currentPlaced++;
        _possibleCount.text = _currentPlaced.ToString();
    }
}
