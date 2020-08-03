using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using System.Data;
using Excel;
using UnityEditor;

public class ExcelWritter : MonoBehaviour
{
    public static List<string> ansList = new List<string>();
    //新增一行資料需要花費的時間
    public static string timer;

    //將資料寫入EXCEL(Excel名稱，表單名稱)
    public static void WriteExcel(string ExcelName, string SheetName)
    {
        //自定義Excel位置
        string Path = Application.streamingAssetsPath + "/" + ExcelName + ".xlsx";
        FileInfo newFile = new FileInfo(Path);
        //透過ExcelPackage開啟Excel表單
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //讀取Excel中的Sheet表單
            ExcelWorksheet worksheet;
            worksheet = package.Workbook.Worksheets[SheetName];
            //讀取Excel表格內那些地區已有資料，接續從該筆資料下方繼續寫入新資料
            float collect = ExcelWritter.ReadExcel(Path, SheetName).Count;
            //新增一行資料需要花費的時間
            worksheet.Cells["A" + (collect + 1)].Value = timer;
            //抓取要填入資料的表單代號[A...]，例如要寫入的資料只有三個欄位，就代表Excel裡的[A、B、C]，可自行增加與修改要寫入的資料欄位
            //假設此案只儲存LevelID和Total Score
            for (int i = 0; i < 2; i++)
            {
                //表格代號
                string letter = "";
                switch (i) 
                {
                    case 0:
                        letter = "A";
                        break;
                    case 1:
                        letter = "B";
                        break;
                }
                //將資料帶入表格內，如果是第一個位置資料
                //worksheet.Cells[letter+"1"].Value=要帶入的資料=worksheet.Cells["A1"].Value=要帶入的資料
                worksheet.Cells[letter + (collect + 1)].Value = ExcelWritter.ansList[i];
            }
            //儲存Excel;
            package.Save();
            //將帶入Excel內的ansList List資料清除，以免資料堆砌
            ExcelWritter.ansList.Clear();
        }

    }
    //讀取Row目前在Excel表單中有多少行數
    static DataRowCollection ReadExcel(string ExcelPath, string SheetName)
    {
        //將Excel開啟
        FileStream stream = File.Open(ExcelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excekReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excekReader.AsDataSet();
        return result.Tables[SheetName].Rows;
    }

}
