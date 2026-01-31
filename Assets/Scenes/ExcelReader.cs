using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ExcelReader : MonoBehaviour
{
    public struct ExcelData
    {
        public string content;
        public string backgroundFileName;
    }

    public static List<ExcelData> ReadExcel(string filePath)
    {
        List<ExcelData> excelDatas = new List<ExcelData>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        ExcelData data = new ExcelData();
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        data.content = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1).ToString();
                        data.backgroundFileName = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3).ToString();
                        // 第0列作为标识
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        excelDatas.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelDatas;
    }
}
