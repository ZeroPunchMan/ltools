// See https://aka.ms/new-console-template for more information
using CsvHelper;
using System.Diagnostics;
using System.Globalization;


public class AssetInfo
{
    public string? ID { get; set; }
    public string? Chinese { get; set; }
    public string? English { get; set; }
}


public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string? dir = Path.GetDirectoryName(args[0]);
        string? name = Path.GetFileNameWithoutExtension(args[0]);
        StreamWriter fId = File.CreateText(dir+$"\\StringAssetId_{name}.cs");
        StreamWriter fChi = File.CreateText(dir+$"\\StringAssetChi_{name}.cs");
        StreamWriter fEng = File.CreateText(dir+$"\\StringAssetEng_{name}.cs");

        fId.WriteLine($"public enum StrAssetId_{name}");
        fId.WriteLine("{");

        fChi.WriteLine($"public static partial class StringAsset_{name}");
        fChi.WriteLine("{\r\nstatic string[] chinese =\r\n    {");

        fEng.WriteLine($"public static partial class StringAsset_{name}");
        fEng.WriteLine("{\r\nstatic string[] english =\r\n    {");

        //读取CSV文件数据
        FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var reader = new StreamReader(fs);
        var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        int index = 0;
        while (csvReader.Read())
        {
            var record = csvReader.GetRecord<AssetInfo>();
            fId.WriteLine($"    {record.ID} = {index},");
            fChi.WriteLine($"        @\"{record.Chinese}\",");
            fEng.WriteLine($"        @\"{record.English}\",");

            index++;
        }

        fId.WriteLine("}");
        fChi.WriteLine("    };\r\n}");
        fEng.WriteLine("    };\r\n}");


        fId.Close();
        fChi.Close();
        fEng.Close();
    }
}
