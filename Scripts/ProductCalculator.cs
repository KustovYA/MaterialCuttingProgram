using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductCalculator : MonoBehaviour
{   
    [SerializeField] private GameObject _productPrefab;
    [SerializeField] private RectTransform _sheetMetal;
    [SerializeField] private RectTransform _contentParent;
    [SerializeField] private Window _window;
    [SerializeField] private ScrollViewFocuser _scrollView;     

    private List<ProductItem> allProducts = new List<ProductItem>();

    public void Calculte()
    {        
        CreateProductsList();

        if (allProducts.Count == 0)
        {
            string message = "—писок изделий пуст";
            _window.Show(message);
            return;
        }

        SetupSheetMetal();
      

        TryPlaceProducts();

        _scrollView.FocusOnSheetMetal();
    }

    private void TryPlaceProducts()
    {     
        float sheetMetalWidth = _sheetMetal.rect.width;
        float sheetMetalLenght = _sheetMetal.rect.height;
        List<Shelf> shelves = new List<Shelf>();               
        float totalUsedHeight = 0;

        foreach (var product in allProducts.GroupBy(p => p.Data)) //
        {
            product.Key.UpdateDisplay(0);
        }

        foreach (ProductItem product in allProducts)
        {
            Shelf bestShelf = null;
            float minRemainingSpace = float.MaxValue;

            if (product.Width < product.Length) product.Rotate();
            
            bool fitsNormal = product.Width <= sheetMetalWidth && product.Length <= sheetMetalLenght;
            bool fitsRotated = product.Length <= sheetMetalWidth && product.Width <= sheetMetalLenght;
                      
            if (!fitsNormal && !fitsRotated)
            {
                string message = $"»зделие ({product.Width}x{product.Length}) больше размеров метеллического листа";
                _window.Show(message);
                return;
            }

            if (product.Width > sheetMetalWidth) product.Rotate();            

            foreach (Shelf shelf in shelves)
            {
                if (product.Width <= shelf.RemainingWidth && product.Length <= shelf.Height)
                {
                    float spaceAfter = shelf.RemainingWidth - product.Width;

                    if (spaceAfter < minRemainingSpace)
                    {
                        minRemainingSpace = spaceAfter;
                        bestShelf = shelf;
                    }
                }
            }

            if (bestShelf != null)
            {
                CreateVisualProduct(bestShelf.CurrentX, bestShelf.YPosition, product);

                bestShelf.CurrentXChange(product);               
                bestShelf.RemainingWidthChange(product);   

                product.Data.AddOne();                
            }
            else
            {                
                if (totalUsedHeight + product.Length > sheetMetalLenght)
                {
                    _window.Show($"Ќе удалось разместить изделие ({product.Width}x{product.Length}), лист заполнен");
                    return;
                }

                Shelf newShelf = new Shelf(totalUsedHeight, product.Length, sheetMetalWidth);

                CreateVisualProduct(newShelf.CurrentX, newShelf.YPosition, product);

                newShelf.CurrentXChange(product);                
                newShelf.RemainingWidthChange(product);                

                shelves.Add(newShelf);
                totalUsedHeight += product.Length;

                product.Data.AddOne();                
            }
        }
    }

    private void CreateVisualProduct(float currentX, float currentY, ProductItem product)
    {
            GameObject visualProduct = Instantiate(_productPrefab, _sheetMetal);
            RectTransform rect = visualProduct.GetComponent<RectTransform>();

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = Vector2.zero;

            rect.anchoredPosition = new Vector2(currentX, currentY);
            rect.sizeDelta = new Vector2(product.Width, product.Length);            
    }

    private void SetupSheetMetal()
    {
        _sheetMetal.anchorMin = Vector2.zero;
        _sheetMetal.anchorMax = Vector2.zero;
        _sheetMetal.pivot = Vector2.zero;
        _sheetMetal.anchoredPosition = Vector2.zero;
    }

    private void CreateProductsList()
    {
        foreach (Transform child in _sheetMetal)
        {
            Destroy(child.gameObject);
        }

        allProducts.Clear();

        foreach (Transform child in _contentParent)
        {            
            if (child.TryGetComponent(out ProductData data))
            {
                
                ProductItem newItem = new ProductItem(data.Width, data.Length, data.Square, data.Count, data);

                for (int i = 0; i < newItem.Count; i++)
                {
                    allProducts.Add(newItem);
                }               
            }
        }

        allProducts = allProducts.OrderByDescending(product => product.Square).ToList();
    }    
}

