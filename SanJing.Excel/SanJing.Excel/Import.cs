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
    /// 快速导入XLS文件
    /// </summary>
    public class Import
    {
        /// <summary>
        /// 从XLSX文件读取数据
        /// </summary>
        /// <param name="filename">完整文件名XLSX</param>
        /// <param name="cellnum">读取数据的最大列序数（从0开始算）</param>
        /// <returns>（0,0）到（最大行序数,cellnum）</returns>
        public static IEnumerable<string[]> ReadAs2007(string filename, int cellnum)
        {
            if (!(filename ?? string.Empty).ToLower().EndsWith(".xlsx"))
                throw new ArgumentException("必须是.xlsx后缀文件", filename);

            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new XSSFWorkbook(fileStream);  //xls数据读入workbook
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表
            IRow row = null;            //新建当前工作表行数据

            var result = new List<string[]>();
            for (int i = 0; i <= sheet.LastRowNum; i++)  //对工作表每一行
            {
                var val = new string[cellnum + 1];
                row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j <= cellnum; j++)  //对工作表每一列
                    {
                        var cell = row.GetCell(j); //获取i行j列数据
                        val[j] = cell == null ? string.Empty : cell.ToString();
                    }
                }
                result.Add(val);
            }
            workbook.Close();
            fileStream.Close();
            return result;
        }

        /// <summary>
        /// 从XLS文件读取数据
        /// </summary>
        /// <param name="filename">完整文件名XLS</param>
        /// <param name="cellnum">读取数据的最大列序数（从0开始算）</param>
        /// <returns>（0,0）到（最大行序数,cellnum）</returns>
        public static IEnumerable<string[]> ReadAs2003(string filename, int cellnum)
        {
            if (!(filename ?? string.Empty).ToLower().EndsWith(".xls"))
                throw new ArgumentException("必须是.xls后缀文件", filename);

            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表
            IRow row = null;            //新建当前工作表行数据

            var result = new List<string[]>();

            for (int i = 0; i <= sheet.LastRowNum; i++)  //对工作表每一行
            {
                var val = new string[cellnum + 1];
                row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        var cell = row.GetCell(j); //获取i行j列数据
                        val[j] = cell == null ? string.Empty : cell.ToString();
                    }
                }
                result.Add(val);
            }
            workbook.Close();
            fileStream.Close();
            return result;
        }
    }
}
