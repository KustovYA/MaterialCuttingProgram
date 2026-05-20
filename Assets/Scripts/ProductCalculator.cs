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
            string message = "Список изделий пуст";
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
        float minZoneSize = 0.01f;
        float totalUsedHeight = 0;

        foreach (var product in allProducts.GroupBy(p => p.Data))
        {
            product.Key.UpdateDisplay(0);
        }

        foreach (ProductItem product in allProducts)
        {
            Shelf.RectZone bestZone = null;
            Shelf bestShelf = null;
            float minRemainingWidth = float.MaxValue;

            if (product.Length > product.Width) product.Rotate();

            bool fitsNormal = product.Width <= sheetMetalWidth && product.Length <= sheetMetalLenght;
            bool fitsRotated = product.Length <= sheetMetalWidth && product.Width <= sheetMetalLenght;

            if (!fitsNormal && !fitsRotated)
            {
                string message = $"Изделие ({product.Width}x{product.Length}) больше размеров металлического листа";
                _window.Show(message);
                return;
            }

            if (product.Width > sheetMetalWidth) product.Rotate();

            foreach (Shelf shelf in shelves)
            {
                foreach (var zone in shelf.FreeZones)
                {
                    if (product.Width <= zone.Width && product.Length <= zone.Height)
                    {
                        float spaceAfterWidth = zone.Width - product.Width;

                        if (spaceAfterWidth < minRemainingWidth)
                        {
                            minRemainingWidth = spaceAfterWidth;
                            bestZone = zone;
                            bestShelf = shelf;
                        }
                    }
                }
            }

            if (bestZone != null && bestShelf != null)
            {
                CreateVisualProduct(bestZone.X, bestZone.Y, product);

                float originalX = bestZone.X;
                float originalY = bestZone.Y;
                float originalWidth = bestZone.Width;
                float originalHeight = bestZone.Height;

                bestShelf.FreeZones.Remove(bestZone);

                if (originalWidth - product.Width > minZoneSize)
                {
                    bestShelf.FreeZones.Add(new Shelf.RectZone(originalX + product.Width, originalY, originalWidth - product.Width, product.Length));
                }

                if (originalHeight - product.Length > minZoneSize)
                {
                    bestShelf.FreeZones.Add(new Shelf.RectZone(originalX, originalY + product.Length, originalWidth, originalHeight - product.Length));
                }

                product.Data.AddOne();
            }
            else
            {                
                if (totalUsedHeight + product.Length > sheetMetalLenght)
                {
                    Debug.Log($"Крупное изделие ({product.Width}x{product.Length}) не поместилось, ищем место для следующего");
                    continue;
                }

                Shelf newShelf = new Shelf(totalUsedHeight, product.Length, sheetMetalWidth);
                Shelf.RectZone targetZone = newShelf.FreeZones[0];

                CreateVisualProduct(targetZone.X, targetZone.Y, product);

                newShelf.FreeZones.Remove(targetZone);

                if (sheetMetalWidth - product.Width > minZoneSize)
                {
                    newShelf.FreeZones.Add(new Shelf.RectZone(product.Width, totalUsedHeight, sheetMetalWidth - product.Width, product.Length));
                }
                
                if (newShelf.Height - product.Length > minZoneSize)
                {
                    newShelf.FreeZones.Add(new Shelf.RectZone(0, totalUsedHeight + product.Length, sheetMetalWidth, newShelf.Height - product.Length));
                }

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
                
                ProductItem newItem = new ProductItem(data.Width, data.Length, data.Count, data);

                for (int i = 0; i < newItem.Count; i++)
                {
                    allProducts.Add(newItem);
                }               
            }
        }
       
        allProducts = allProducts.OrderByDescending(product => product.Length).ThenByDescending(p => p.Width).ToList();
    }    
}

