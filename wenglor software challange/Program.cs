using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;


class Barcodes
{
    private string prototextPath = @"C:\\Users\\rares\\Downloads\\opencv_3rdparty-wechat_qrcode\\sr.prototxt",
        modelPath = @"C:\\Users\\rares\\Downloads\\opencv_3rdparty-wechat_qrcode\\sr.caffemodel";
    private string[] caleFisiere = Array.Empty<string>();
    private string codBareCitit = "";
    private List<string> listaCodBare = new List<string>();
    private BarcodeDetector barcodeUtil;
    private VectorOfPoint puncteIesire = new VectorOfPoint();
    private IInputArray puncteIntrare;
    int counter = 0;

    public void CitesteCodDeBare()
    {
        Console.WriteLine("Detectam barcodes");
        
        foreach(string item in caleFisiere) { 
            Console.WriteLine(item);
            try
            {
                using (Image<Bgr, Byte> image = new Image<Bgr, byte>(item))
                {
                    this.puncteIntrare = image;
                    if (barcodeUtil.Detect(puncteIntrare, puncteIesire))
                    {
                        string codBare = barcodeUtil.Decode(puncteIntrare, puncteIesire);
                        Console.WriteLine(codBare);
                        listaCodBare.Add(codBare);
                    }
                    this.puncteIesire.Clear();
                    this.puncteIntrare.Dispose();
                }
            }
            catch (AccessViolationException e)
            {
                
            }
        };
        ScriePeDisc();

    }

    public void CitesteFisiereIntrare()
    {
        string path; string []caleFisiereJpg;

        barcodeUtil = new BarcodeDetector(prototextPath, modelPath);
        Console.WriteLine("Scanam pentru fisiere");

        path = System.IO.Directory.GetCurrentDirectory() + "\\Input Files";
        caleFisiereJpg = Directory.GetFiles(path, "*.jpg");
        
        counter = caleFisiereJpg.Length;

        Console.WriteLine("Am citit " + counter + " de fisiere");

        this.caleFisiere = caleFisiereJpg;
    }

    private void ScriePeDisc()
    {
        string filePath = "output.txt";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (string text in listaCodBare)
            {
                writer.WriteLine(text);
            }
        }

        Console.WriteLine("Textele au fost scrise în fișierul output.txt.");
    }



    static void Main(String[] args)
    {
        Barcodes barcodes = new Barcodes();

        Console.WriteLine("Program de detectare a codului de bare");

        barcodes.CitesteFisiereIntrare();

        barcodes.CitesteCodDeBare();

    }
}