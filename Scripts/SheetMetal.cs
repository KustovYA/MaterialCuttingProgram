using TMPro;
using UnityEngine;

public class SheetMetal : MonoBehaviour
{
    [SerializeField] private TMP_InputField _width;
    [SerializeField] private TMP_InputField _lenght;
        
    public float Width => float.TryParse(_width.text, out float width) ? width : 0;    
    public float Length => float.TryParse(_lenght.text, out float lenght) ? (lenght * 100f) : 0;   
}
