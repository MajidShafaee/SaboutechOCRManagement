



namespace OCR
{
    public class TesseractPersianOCR
    {
        public delegate void LoggerCallback(string log, int fileId);
        private LoggerCallback _loggerCallback;

        public delegate void DbCallback(int fileId,string ocredText);
        private DbCallback _dbCallback;

        public delegate void RunNewOCRCallback();
        private RunNewOCRCallback _runNewOCRCallback;

        private string _filePath;
        private string _fileName= string.Empty;
        private int _fileId = 0;

        //static TesseractPersianOCR()
        //{
        //    MagickNET.SetTempDirectory(@"E:\imageMagicTempOCRManagement");
        //    MagickNET.SetGhostscriptDirectory(@"E:\gs");
        //    MagickNET.Initialize();
        //}

        public  TesseractPersianOCR(string filePath, string fileName, int fileId, LoggerCallback loggerCallback, DbCallback dbCallback, RunNewOCRCallback runNewOCRCallback)
        {
            MagickNET.SetTempDirectory(@"E:\imageMagicTempOCRManagement");
            MagickNET.SetGhostscriptDirectory(@"E:\gs");
            MagickNET.Initialize();
            _loggerCallback = loggerCallback;
            _dbCallback = dbCallback;
            _runNewOCRCallback = runNewOCRCallback;
            _filePath = filePath;
            _fileName = fileName;
            _fileId = fileId;
        }        

        public void DoOCR()
        {
            try
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}. Starting OCR fileId:{_fileId}");
                var ocrText = string.Empty;                
                var path = @$"{_filePath}";                
                var imagesCount = ConvertPdfToPng(path, _fileName,_fileId);
                Console.WriteLine($"Image Count:{imagesCount}");
                for (int i = 1; i <= imagesCount; i++)
                {                   
                    var (per, text) = TesseractOCR($"{Thread.CurrentThread.ManagedThreadId}_{_fileId}.Page{i}.png");
                    ocrText += text;
                    File.Delete($"{Thread.CurrentThread.ManagedThreadId}_{_fileId}.Page{i}.png");
                }                
                Console.WriteLine($"Success ocr. fileId:{_fileId}");
                _dbCallback(_fileId, ocrText);
                //_runNewOCRCallback();
            }

            catch (Exception ex)
            {
                if (_loggerCallback != null)
                    _loggerCallback($"{Thread.CurrentThread.ManagedThreadId},{ex.Message},{ex.InnerException?.Message}", _fileId);

                //_runNewOCRCallback();
            }
            finally
            {
                if (Thread.CurrentThread.IsAlive)
                    Thread.CurrentThread.Interrupt();

            }
        }

      
        private int ConvertPdfToPng(string pdfPath, string fileName, int fileId)
        {

            try
            {
               
                var settings = new MagickReadSettings();

                // Settings the density to 300 dpi will create an image with a better quality
                settings.Density = new Density(300, 300, DensityUnit.PixelsPerInch);
;
                using (var images = new MagickImageCollection())
                {

                    // Add all the pages of the pdf file to the collection
                    images.Read(pdfPath, settings);

                    var page = 1;

                    foreach (var image in images)
                    {                        
                        // Write page to file that contains the page number
                        image.Write(Thread.CurrentThread.ManagedThreadId.ToString() + $"_{fileId}.Page" + page + ".png");
                        // Writing to a specific format works the same as for a single image
                        //image.Format = MagickFormat.Ptif;
                        //image.Write("Snakeware.Page" + page + ".tif");
                        page++;
                        image.Dispose();
                        
                    }
                    return images.Count;
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception($"Exception on ConvertPdfToPng __ {fileId} __ {ex.Message}");
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
                throw new Exception($"Exception on OCR:  {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}{ex.Message}\r\n{ex.StackTrace}\r\n{ex.Source}");
            }
        }

    }
}
