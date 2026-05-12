using UnityEngine;
using UnityEngine.UI;

public class ScrollViewFocuser : MonoBehaviour 
{
    [SerializeField] private ScrollRect _scrollRect;  

    public void FocusOnSheetMetal()
    {
        _scrollRect.normalizedPosition = new Vector2(0, 0);        
    }
}
