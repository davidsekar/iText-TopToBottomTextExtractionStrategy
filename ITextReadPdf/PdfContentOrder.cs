namespace ITextReadPdf;

/// <summary>
///     The pdf content order.
/// </summary>
public class PdfContentOrder
{
    /// <summary>
    ///     Gets or Sets the start x.
    /// </summary>
    public float StartX { get; set; }

    /// <summary>
    ///     Gets or Sets the start y.
    /// </summary>
    public float StartY { get; set; }

    /// <summary>
    ///     Gets or Sets the end x.
    /// </summary>
    public float EndX { get; set; }

    /// <summary>
    ///     Gets or Sets the end y.
    /// </summary>
    public float EndY { get; set; }

    /// <summary>
    ///     Gets or Sets the content.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    ///     Tos the string.
    /// </summary>
    /// <returns>A string.</returns>
    public override string ToString()
    {
        return $"[{StartX},{StartY}] - [{EndX},{EndY}] => {Content}";
    }
}