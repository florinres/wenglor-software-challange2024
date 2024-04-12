using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;


class Barcodes
{
    private string prototextPath = @"C:\\Users\\rares\\Downloads\\opencv_3rdparty-wechat_qrcode\\sr.prototxt",
        modelPath = @"C:\\Users\\rares\\Downloads\\opencv_3rdparty-wechat_qrcode\\sr.caffemodel";
    private string[] caleFisiere = Array.Empty<string>();
    private string codBare = "";
    private BarcodeDetector barcodeUtil;
    private VectorOfPoint puncteIesire = new VectorOfPoint();
    private IInputArray puncteIntrare;


    //public void FindMyBarCode(Bitmap imagine,int coltStangaSus, int coltDreaptaJos)
    //{
    //    int gridLines = 0, pozitieGrid;
    //    //va trebui sa facem mai multe treceri pentru a identifica codul de bare perfect
    //    //poate ne trebuie un char array de pixeli pentru prima trecere dupa care mai trecem o data din directia inversa
    //    //metoda cu 
    //    //facem in urmatorul mod ne imaginam ca impartem imaginea in ghidaje mici, avem nevoie de
    //    //trebuie sa determinam o frecventa cu inaltimea imaginii

    //    //numarul de ghidaje
    //    switch (imagine.Height)
    //    {
    //        case int w when (w< 200):
    //            gridLines = 2;
    //            break;
    //        case int w when (w < 800):
    //            gridLines = 4;
    //            break;
    //        case int w when (w < 1600):
    //            gridLines = 6;
    //            break;
    //        case int w when (w < 2500):
    //            gridLines = 10;
    //            break;

    //        default:
    //            gridLines = 8;
    //            break;
    //    }

    //    //setam grid-urile
    //    pozitieGrid = imagine.Height / gridLines;

    //    //crestem pozitia grid cat timp nu am ajuns la dimensiunea finala
    //    //pozitie grid determina un punct de start pentru detectare
    //    //pot exista cazuri ca in prima poza unde avem o 
    //    while (pozitieGrid < imagine.Height)
    //    {
    //        //inaltimea
    //        for (int i = pozitieGrid; i < imagine.Height; i++)
    //        {
    //            //latimea
    //            for (int j = 0; j < imagine.Width; j++)
    //            {

    //            }
    //        }
    //    }
    //}

    public void DetectBarcodes()
    {

        Console.WriteLine("Detectam barcodes");

        // Utilizăm Parallel.ForEach pentru a paraleliza procesarea imaginilor
        Parallel.ForEach(caleFisiere, item =>
        {
            Console.WriteLine(item);

            try
            {
                using (Image<Bgr, Byte> image = new Image<Bgr, byte>(item))
                {
                    this.puncteIntrare = image;

                    if (barcodeUtil.Detect(image, puncteIesire))
                    {
                        // Pentru a evita problemele de concurență, folosim un obiect blocat pentru a accesa variabila codBare
                        lock (this)
                        {
                            codBare = barcodeUtil.Decode(image, puncteIntrare);
                            Console.WriteLine(codBare);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Poți trata excepțiile aici sau le poți înregistra pentru a fi tratate ulterior
                throw;
            }
        });
    }

    public void CitesteFisiereIntrare()
    {
        barcodeUtil = new BarcodeDetector(prototextPath, modelPath);
        Console.WriteLine("Scanam pentru fisiere");
    //D:\Repouri\wenglor soft\wenglor software challange\input\

        string path = Environment.CurrentDirectory;

        path = path.Substring(0, path.IndexOf("bin")-1);
        path += "\\input";

        string[] fisiereJpg = Directory.GetFiles(path, "*.jpg");

        this.caleFisiere = fisiereJpg;
    }

    public void WriteOnDrive(Bitmap bitmap)
    {
        int count = 1;
        string caleFisier;

        caleFisier = "C:\\Users\\rares\\source\\repos\\wenglor software challange\\wenglor software challange\\output\\";

        caleFisier += "out" + count++ + ".png";

        bitmap.Save(caleFisier, System.Drawing.Imaging.ImageFormat.Png);

    }

    static void Main(String[] args)
    {
        Barcodes barcodes = new Barcodes();

        Console.WriteLine("Program de detectare a codului de bare");

        barcodes.CitesteFisiereIntrare();

        barcodes.DetectBarcodes();

    }
}