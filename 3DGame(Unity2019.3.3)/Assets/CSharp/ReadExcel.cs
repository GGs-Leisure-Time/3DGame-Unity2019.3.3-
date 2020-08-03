using System.Collections;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using System.Data;
using Excel;
using OfficeOpenXml;
using System.Collections.Generic;
using UnityEngine;

public class ReadExcel : MonoBehaviour
{
    public string ExcelName;
    public List<string> ReadData;
    private void Start()
    {
        Read();
    }
    void Read()
    {
        //讀取Excel所有表格內容
        string path = Application.streamingAssetsPath + "/" + ExcelName + ".xlsx";
        //要抓取的Excel工作表名稱
        string set = "Data";
        //讀取Excel檔案
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        //創建並讀取Excel檔案
        IExcelDataReader excelRead = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //將讀取到Excel檔案暫存在內存
        DataSet result = excelRead.AsDataSet();
        //抓取Excel行數與列數
        int colums = result.Tables[set].Columns.Count;
        int rows = result.Tables[set].Rows.Count;

        for (int i = 0;i<rows;i++)
        {
            for (int j = 0; j < colums; j++)
            {
                String data = result.Tables[set].Rows[i][j].ToString();
                ReadData.Add(data);
            }
        }
    }

}
