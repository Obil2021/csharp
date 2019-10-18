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
