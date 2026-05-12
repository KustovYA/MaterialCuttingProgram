using UnityEngine;

public class ProductAdder : MonoBehaviour
{    
    [SerializeField] private GameObject _productFields;
    [SerializeField] private RectTransform _contentParent;

    private int _productFieldsCount  = 0;
    private readonly float _shiftCoefficient = -40f;

    public void AddButtons()
    {
        if (_productFields != null && _contentParent != null)
        {        
        GameObject productFields = Instantiate(_productFields, _contentParent);
        productFields.transform.localScale = Vector2.one;

        _productFieldsCount += 1;

        float productFieldsShift = _productFieldsCount * _shiftCoefficient;
        productFields.transform.position += new Vector3(0, productFieldsShift, 0);  
        }              
    }   
}
