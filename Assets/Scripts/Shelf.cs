using System.Collections.Generic;

public class Shelf
{
    public float YPosition { get; private set; }
    public float Height { get; private set; }
    public float SheetWidth { get; private set; }

    public List<RectZone> FreeZones { get; private set; } = new List<RectZone>();

    public Shelf(float yPosition, float height, float sheetWidth)
    {
        YPosition = yPosition;
        Height = height;
        SheetWidth = sheetWidth;

        FreeZones.Add(new RectZone(0, yPosition, sheetWidth, height));
    }

    public class RectZone
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public RectZone(float x, float y, float width, float height)
        {
            X = x; 
            Y = y; 
            Width = width; 
            Height = height;
        }
    }
}