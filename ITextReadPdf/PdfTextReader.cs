using System.ComponentModel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace ITextReadPdf;

public partial class PdfTextReader : Form
{
    public PdfTextReader()
    {
        InitializeComponent();

        comboBox1.Items.Add("Simple");
        comboBox1.Items.Add("Custom");
        comboBox1.Items.Add("xPdf");
        comboBox1.SelectedIndex = 0;
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
        textBox1.Text = openFileDialog1.FileName;
        Process(textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        openFileDialog1.ShowDialog();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(textBox1.Text))
            Process(textBox1.Text);
    }

    public void Process(string filePath)
    {
        richTextBox1.Text = "";

        if (comboBox1.Text == "Simple")
        {
            var reader = new PdfReader(openFileDialog1.FileName);

            var pdfDocument = new PdfDocument(reader);
            var strategy = new SimpleTextExtractionStrategy();
            var pagesCount = pdfDocument.GetNumberOfPages();

            for (var i = 1; i <= pagesCount; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
                richTextBox1.Text += text;
            }
        }
        else if (comboBox1.Text == "xPdf")
        {
            var pdfHelper = new XpdfHelper();
            richTextBox1.Text = pdfHelper.ToText(openFileDialog1.FileName);
        }
        else
        {
            var reader = new PdfReader(openFileDialog1.FileName);
            var pdfDocument = new PdfDocument(reader);

            var strategy = new TopToBottomTextExtractionStrategy();

            var pagesCount = pdfDocument.GetNumberOfPages();

            for (var i = 1; i <= pagesCount; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
                richTextBox1.Text += text;
            }
        }
    }
}