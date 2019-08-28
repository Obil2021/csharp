using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing.Excel
{
    /// <summary>
    /// 快速导出XLS文件
    /// </summary>
    public class Export
    {
        /// <summary>
        /// 从XLSX文件读取数据
        /// </summary>
        /// <param name="filename">完整文件名XLSX</param>
        /// <returns></returns>
        public static IEnumerable<string[]> ReadAs2007(string filename)
        {
            if (!(filename ?? string.Empty).ToLower().EndsWith(".xlsx"))
                throw new ArgumentException("必须是.xlsx后缀文件", filename);

            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new XSSFWorkbook(fileStream);  //xls数据读入workbook
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表
            IRow row = null;            //新建当前工作表行数据

            var result = new List<string[]>();

            for (int i = sheet.FirstRowNum; i < sheet.LastRowNum; i++)  //对工作表每一行
            {
                row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    var val = new string[row.LastCellNum];
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        var cell = row.GetCell(j); //获取i行j列数据
                        val[i] = cell == null ? string.Empty : cell.ToString();
                    }
                    result.Add(val);
                }
            }
            workbook.Close();
            fileStream.Close();
            return result;
        }

        /// <summary>
        /// 从XLS文件读取数据
        /// </summary>
        /// <param name="filename">完整文件名XLS</param>
        /// <returns></returns>
        public static IEnumerable<string[]> ReadAs2003(string filename)
        {
            if (!(filename ?? string.Empty).ToLower().EndsWith(".xls"))
                throw new ArgumentException("必须是.xls后缀文件", filename);

            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表
            IRow row = null;            //新建当前工作表行数据

            var result = new List<string[]>();

            for (int i = sheet.FirstRowNum; i < sheet.LastRowNum; i++)  //对工作表每一行
            {
                row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    var val = new string[row.LastCellNum];
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        var cell = row.GetCell(j); //获取i行j列数据
                        val[i] = cell == null ? string.Empty : cell.ToString();
                    }
                    result.Add(val);
                }
            }
            workbook.Close();
            fileStream.Close();
            return result;
        }
        /// <summary>
        /// 保存数据至XLS文件
        /// </summary>
        /// <param name="filename">完整文件名XLS</param>
        /// <param name="source">数据源</param>
        public static void SaveAs2003(string filename, IEnumerable<string[]> source)
        {
            if (!(filename ?? string.Empty).ToLower().EndsWith(".xls"))
                throw new ArgumentException("必须使用.xls文件后缀", filename);
            //创建工作薄
            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个名称为mySheet的表
            ISheet tb = wk.CreateSheet("Sheet1");
            int index = 0;
            foreach (var item in source)
            {
                IRow row = tb.CreateRow(index++);
                int index2 = 0;
                foreach (var item1 in item)
                {
                    var cell = row.CreateCell(index2++);
                    cell.SetCellValue(item1);
                }
            }
            using (FileStream fs = File.Create(filename))
            {
                wk.Write(fs);
            }
            wk.Close();
        }
        /// <summary>
        /// 保存数据至XLSX文件
        /// </summary>
        /// <param name="filename">完整文件名XLSX</param>
        /// <param name="source">数据源</param>
        public static void SaveAs2007(string filename, IEnumerable<string[]> source)
        {
            if (!(filename ?? string.Empty).ToLower().EndsWith(".xlsx"))
                throw new ArgumentException("必须使用.xlsx文件后缀", filename);
            //创建工作薄
            XSSFWorkbook wk = new XSSFWorkbook();
            //创建一个名称为mySheet的表
            ISheet tb = wk.CreateSheet("Sheet1");
            int index = 0;
            foreach (var item in source)
            {
                IRow row = tb.CreateRow(index++);
                int index2 = 0;
                foreach (var item1 in item)
                {
                    var cell = row.CreateCell(index2++);
                    cell.SetCellValue(item1);
                }
            }
            using (FileStream fs = File.Create(filename))
            {
                wk.Write(fs);
                wk.Close();
            }
            wk.Close();
        }
    }
}
