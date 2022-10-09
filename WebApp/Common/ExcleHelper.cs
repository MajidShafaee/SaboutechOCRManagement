



namespace WebApp.Common
{
    public static class ExcleHelper
    {
       public static DataSet ReadExcleFile(string path)
        {
            var stream=new FileStream(path, FileMode.Open, FileAccess.Read);
            using IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dsexcelRecords = reader.AsDataSet();
            reader.Close();
            stream.Close();
            return dsexcelRecords;
        }
    }
}
