



namespace OCR
{
    public class TesseractPersianOCR
    {
        public delegate void LoggerCallback(string log);
        private LoggerCallback _loggerCallback;
        public delegate void DbCallback(string ocredText);
        private DbCallback _dbCallback;

        private string filePath;

        public TesseractPersianOCR(string filePath, LoggerCallback loggerCallback,
            DbCallback dbCallback)
        {
            MagickNET.SetTempDirectory(@"E:\imageMagicTempOCRManagement");
            MagickNET.Initialize();
            _loggerCallback = loggerCallback;
            _dbCallback = dbCallback;
            this.filePath = filePath;
        }

        public void DoOCR()
        {
            try
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}. starting file:{filePath}");
                var ocrText = string.Empty;                
                var path = @$"{filePath}";
                var imagesCount = ConvertPdfToPng(path);
                Console.WriteLine($"Image Count:{imagesCount}");
                for (int i = 1; i <= imagesCount; i++)
                {
                    Console.WriteLine($"Starting OCR :{Thread.CurrentThread.ManagedThreadId}fromPdf.Page{i}.png");
                    var (per, text) = TesseractOCR($"{Thread.CurrentThread.ManagedThreadId}fromPdf.Page{i}.png");
                    ocrText += text;                    
                }                
                Console.WriteLine($"Success ocr. file:{filePath}");
               _dbCallback(ocrText);

            }

            catch (Exception ex)
            {
                if (_loggerCallback != null)
                    _loggerCallback($"{Thread.CurrentThread.ManagedThreadId},{ex.Message},{ex.InnerException?.Message}");

            }
            finally
            {
                if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Interrupt();
            }
        }

        private int ConvertPdfToPng(string pdfPath)
        {

            try
            {
                var settings = new MagickReadSettings();

                // Settings the density to 300 dpi will create an image with a better quality
                settings.Density = new Density(300, 300);

                using (var images = new MagickImageCollection())
                {

                    // Add all the pages of the pdf file to the collection
                    images.Read(pdfPath, settings);

                    var page = 1;

                    foreach (var image in images)
                    {
                        // Write page to file that contains the page number
                        image.Write(Thread.CurrentThread.ManagedThreadId.ToString() + "fromPdf.Page" + page + ".png");
                        // Writing to a specific format works the same as for a single image
                        //image.Format = MagickFormat.Ptif;
                        //image.Write("Snakeware.Page" + page + ".tif");
                        page++;
                    }
                    return images.Count;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception on ConvertPdfToPng.\r\n{pdfPath}.\r\n.{ex.Message}");
            }
        }

        private (string, string) TesseractOCR(string imagePath)
        {
            try
            {
                using (var engine = new TesseractEngine(@"tessdata", "fas+eng", EngineMode.Default))
                {
                    using (var image = new System.Drawing.Bitmap(@$"{imagePath}"))
                    {
                        using (var pix = PixConverter.ToPix(image))
                        {
                            using (var page = engine.Process(pix))
                            {
                                var meanConfidenceLabel = String.Format("{0:P}", page.GetMeanConfidence());
                                var resultText = page.GetText();
                                return (meanConfidenceLabel, resultText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception on OCR.\r\n{imagePath}.\r\n.{(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}{ex.Message}\r\n{ex.StackTrace}\r\n{ex.Source}");
            }
        }

    }
}
