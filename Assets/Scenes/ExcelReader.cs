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
        public string type;
        public string content;
        public string backgroundFileName;
        public string choice1content;
        public int choice1FileName;
        public string choice2content;
        public int choice2FileName;
    }

    public struct SceneData
    {
        public string name;
        public string backgroundFileName;
        public string storyFileName;
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
                        data.type = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0).ToString();
                        data.content = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1).ToString();
                        data.backgroundFileName = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3).ToString();
                        data.choice1content = reader.IsDBNull(4) ? string.Empty : reader.GetValue(4).ToString();
                        //data.choice1FileName = reader.IsDBNull (5) ? -1 : reader.GetInt32(5);
                        data.choice1FileName = reader.IsDBNull(5) ? -1 : (int.TryParse(reader.GetValue(5)?.ToString(), out int val1) ? val1 : -1) - 1;
                        data.choice2content = reader.IsDBNull(6) ? string.Empty : reader.GetValue(6).ToString();
                        //data.choice2FileName = reader.IsDBNull(7) ? -1 : reader.GetInt32(7);
                        data.choice2FileName = reader.IsDBNull(7) ? -1 : (int.TryParse(reader.GetValue(5)?.ToString(), out int val2) ? val2 : -1) - 1;
                        // 注意行数需要减一之后才是真实值
                        // 第0列作为标识
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        excelDatas.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelDatas;
    }

    public static List<SceneData> ReadSceneData()
    {
        string filePath = "Assets/Resources/sceneData.xlsx";
        List<SceneData> sceneDatas = new List<SceneData>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        SceneData data = new SceneData();
                        data.name = reader.GetValue(0).ToString();
                        data.backgroundFileName = reader.GetValue(1).ToString();
                        data.storyFileName = reader.GetValue(2).ToString();
                        sceneDatas.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return sceneDatas;
    }
}
