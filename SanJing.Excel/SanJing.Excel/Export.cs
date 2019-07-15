using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
        /// <param name="filename">完整文件名</param>
        /// <param name="source">数据源</param>
        /// <param name="titles">列标题</param>
        public static void SaveAs(string filename, IEnumerable<string[]> source, params string[] titles)
        {
            //创建工作薄
            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个名称为mySheet的表
            ISheet tb = wk.CreateSheet("Sheet1");
            int index = 0;
            if (titles.Length > 0)
            {
                IRow row = tb.CreateRow(0);
                foreach (var item in titles)
                {
                    var cell = row.CreateCell(index++);  
                    cell.SetCellValue(item);
                }
            }
            index = titles.Length > 0 ? 1 : 0;
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
        }
    }
}
