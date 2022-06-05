using System.Text;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace ITextReadPdf;

/// <summary>
///     The top to bottom text extraction strategy.
/// </summary>
public class TopToBottomTextExtractionStrategy : ITextExtractionStrategy
{
    private readonly SortedList<PdfTextBlocks, PdfTextBlocks> _textBlocks = new(new PdfTextBlocksComparer());
    private PdfTextBlocks _currentTextBlock = new();
    private int secondaryOrderCount = 1;

    private Vector? _lastEnd;
    private Vector? _lastStart;
    private readonly bool _debug = false;
    private bool firstRender = true;

    /// <inheritdoc />
    public void EventOccurred(IEventData data, EventType type)
    {
        if (type.Equals(EventType.RENDER_TEXT))
        {
            var renderInfo = (TextRenderInfo)data;
            var segment = renderInfo.GetBaseline();
            var start = segment.GetStartPoint();
            var end = segment.GetEndPoint();

            var sameLine = false;
            if (!firstRender)
            {
                var x0 = start;
                var x1 = _lastStart;
                var x2 = _lastEnd;

                var dist = x2!.Subtract(x1).Cross(x1!.Subtract(x0)).LengthSquared() / x2.Subtract(x1).LengthSquared();

                var sameLineThreshold = 2f;
                // If we've detected that we're still on the same
                if (dist <= sameLineThreshold)
                    sameLine = true;

                if ((int) Math.Floor(x0.Get(1)) > (int) Math.Floor(x1!.Get(1)))
                {
                    if (_debug)
                        _currentTextBlock.Content.Append($"\n == Order Change [{x0.Get(1)} > {x1.Get(1)}] == \n");
                    // There seems to be change in the order make it as a new block
                    _textBlocks.Add(_currentTextBlock, _currentTextBlock);
                    secondaryOrderCount++;
                    _currentTextBlock = new PdfTextBlocks
                    {
                        StartX = (int) Math.Floor(x0.Get(0)),
                        StartY = (int) Math.Floor(x0.Get(1)),
                        SecondaryOrder = secondaryOrderCount
                    };
                }
                // Check for sequential content
                // Content that changes in the distance by 2 * sameLineThreshold
                else if (dist > 400)
                {
                    if (_debug)
                        _currentTextBlock.Content.Append($"\n == Large Gaps [{dist} >= {2 * sameLineThreshold}] == \n");
                    _textBlocks.Add(_currentTextBlock, _currentTextBlock);
                    _currentTextBlock = new PdfTextBlocks
                    {
                        StartX = (int) Math.Floor(x0.Get(0)),
                        StartY = (int) Math.Floor(x0.Get(1)),
                        SecondaryOrder = secondaryOrderCount
                    };
                }
            }


            if (firstRender)
            {
                _currentTextBlock.StartX = (int) Math.Floor(start.Get(0));
                _currentTextBlock.StartY = (int) Math.Floor(start.Get(1));
                _currentTextBlock.SecondaryOrder = secondaryOrderCount;
                firstRender = false;
            }
            else
            {
                // Don't append if the new next is empty
                if (renderInfo.GetText().Length > 0 && !renderInfo.GetText().StartsWith(" "))
                {
                    //Don't append if the new text starts with a space
                    //Calculate the distance between the two blocks
                    var spacing = _lastEnd!.Subtract(start).Length();
                    //If it "looks" like it should be a space
                    if (spacing > renderInfo.GetSingleSpaceWidth() / 2f) //Add a space
                        _currentTextBlock.Content.Append(" ");
                }
            }

            _currentTextBlock.Content.Append((sameLine ? string.Empty : "\n") + renderInfo.GetText());

            _lastStart = start;
            _lastEnd = end;
        }
    }

    /// <inheritdoc />
    public ICollection<EventType> GetSupportedEvents()
    {
        return null!;
    }

    /// <inheritdoc />
    public virtual string GetResultantText()
    {
        var buf = new StringBuilder();
        if (_currentTextBlock.Content.Length > 0) _textBlocks.Add(_currentTextBlock, _currentTextBlock);
        foreach (var sortedBlock in _textBlocks) buf.AppendLine(sortedBlock.Value.ToString());
        Reset();
        return buf.ToString();
    }

    /// <summary>
    ///     Reset current page buffer
    /// </summary>
    public void Reset()
    {
        _currentTextBlock = new PdfTextBlocks();
        firstRender = true;
        _textBlocks.Clear();
        secondaryOrderCount = 1;
    }
}