public class Shelf
{
    public float CurrentX { get; private set; }
    public float YPosition { get; private set; }
    public float Height { get; private set; }
    public float RemainingWidth { get; private set; }

    public Shelf(float yPosition, float height, float totalWidth)
    {
        YPosition = yPosition;
        Height = height;
        CurrentX = 0;
        RemainingWidth = totalWidth;
    }

    public void CurrentXChange(ProductItem product)
    {
        CurrentX += product.Width;
    }

    public void RemainingWidthChange(ProductItem product)
    {
        RemainingWidth -= product.Width;
    }
}