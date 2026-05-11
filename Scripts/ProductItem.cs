public class ProductItem 
{    
    public float Width { get; private set; }
    public float Length { get; private set; }
    public float Square { get; private set; }
    public float Count { get; private set; }       
    public ProductData Data { get; private set; }

   public ProductItem(float width, float lenght, float square, int count, ProductData parent)
    {
        Width = width;
        Length = lenght;
        Square = square;
        Count = count;
        Data = parent;
    }

    public void Rotate()
    {
        float temp = Width;
        Width = Length;
        Length = temp;
    }    
}
