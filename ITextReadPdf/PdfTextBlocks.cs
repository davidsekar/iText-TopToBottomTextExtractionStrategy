using System.Text;

namespace ITextReadPdf;

public class PdfTextBlocks
{
    public int StartX { get; set; }
    public int StartY { get; set; }
    public StringBuilder Content { get; } = new();
    public int SecondaryOrder = 0;

    public override string ToString()
    {
        // return $"[{StartX},{StartY}] at {SecondaryOrder}] => {Content}";
        return $"{Content}";
    }
}

public class PdfTextBlocksComparer : IComparer<PdfTextBlocks>
{
    public int Compare(PdfTextBlocks? x, PdfTextBlocks? y)
    {
        if (x == null || y == null) return 1;

        if (x.StartY > y.StartY) return -1;
        if (x.StartY == y.StartY && x.SecondaryOrder <= y.SecondaryOrder) return -1;
        if (x.StartX < y.StartX) return 1;

        return 1;
    }
}