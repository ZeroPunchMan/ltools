// See https://aka.ms/new-console-template for more information
using CsvHelper;
using System.Diagnostics;
using System.Globalization;
using System.Text;

//csvhelper 33.1.0
public class AssetInfo
{
    public string? ID { get; set; }
    public string? Chinese { get; set; }
    public string? English { get; set; }
}


public static class Program
{
    public static string? Escape(string? input)
    {
        if (input == null)
            return null;

        var _builder = new StringBuilder();

        foreach (char c in input)
        {
            switch (c)
            {
                case '\\': _builder.Append("\\\\"); break;
                case '\"': _builder.Append("\\\""); break;
                case '\'': _builder.Append("\\\'"); break;
                case '\0': _builder.Append("\\0"); break;
                case '\a': _builder.Append("\\a"); break;
                case '\b': _builder.Append("\\b"); break;
                case '\f': _builder.Append("\\f"); break;
                case '\n': _builder.Append("\\n"); break;
                case '\r': _builder.Append("\\r"); break;
                case '\t': _builder.Append("\\t"); break;
                case '\v': _builder.Append("\\v"); break;
                default:
                    _builder.Append(c);
                    break;
            }
        }

        return _builder.ToString();
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string? dir = Path.GetDirectoryName(args[0]);
        string? name = Path.GetFileNameWithoutExtension(args[0]);
        StreamWriter fId = File.CreateText(dir+$"\\StringAssetId_{name}.cs");
        StreamWriter fChi = File.CreateText(dir+$"\\StringAssetChi_{name}.cs");
        StreamWriter fEng = File.CreateText(dir+$"\\StringAssetEng_{name}.cs");

        fId.WriteLine($"//generated form {name}.csv");
        fId.WriteLine($"public enum StrAssetId_{name}");
        fId.WriteLine("{");

        fChi.WriteLine($"//generated form {name}.csv");
        fChi.WriteLine($"public static partial class StringAsset_{name}");
        fChi.WriteLine("{\r\nstatic string[] chinese =\r\n    {");

        fEng.WriteLine($"//generated form {name}.csv");
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
            fId.WriteLine($"    {record.ID}= {index},");
            fChi.WriteLine($"        \"{Escape(record.Chinese)}\", // {record.ID}");
            fEng.WriteLine($"        \"{Escape(record.English)}\", // {record.ID}");

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
