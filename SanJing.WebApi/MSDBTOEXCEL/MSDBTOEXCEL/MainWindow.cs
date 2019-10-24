using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSDBTOEXCEL
{
    public partial class MainWindow : MaterialSkin.Controls.MaterialForm
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            EXCEL_TEXT.Text = $@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).ToUpper()}\MSDBTOEXCEL.XLSX";
            if (File.Exists("conn.ini"))
            {
                DB_TEXT.Items.AddRange(File.ReadAllLines("conn.ini", Encoding.Default));
            }
        }

        private void EXPORT_BUTTON_Click(object sender, EventArgs e)
        {
            ERROR_LABLE.Text = "";
            if (string.IsNullOrWhiteSpace(DB_TEXT.Text))
            {
                ERROR_LABLE.Text = "DB CONNECTIONSTRING IS NULL";
                return;
            }
            if (string.IsNullOrWhiteSpace(EXCEL_TEXT.Text))
            {
                ERROR_LABLE.Text = "EXCEL SAVEFULLFILENAME IS NULL";
                return;
            }
            try
            {
                //TABLE
                var sql = @"
SELECT 
    TABLE_NAME       = case when a.colorder=1 then d.name else '' end,
    TABLE_DESC     = case when a.colorder=1 then isnull(f.value,'') else '' end,
    COLUMN_INDEX   = a.colorder,
    COLUMN_NAME     = a.name,
    COLUMN_ISIDENTITY       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
    COLUMN_ISPK       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '√' else '' end,
    COLUMN_TYPE       = b.name,
    COLUMN_BITY = a.length,
    COLUMN_LENGTH       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    COLUMN_POINT   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    COLUMN_ISNULL     = case when a.isnullable=1 then '√'else '' end,
    COLUMN_DEFAULT     = isnull(e.text,''),
    COLUMN_DESC   = isnull(g.[value],'')
FROM 
    syscolumns a
left join 
    systypes b 
on 
    a.xusertype=b.xusertype
inner join 
    sysobjects d 
on 
    a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join 
    syscomments e 
on 
    a.cdefault=e.id
left join 
sys.extended_properties   g 
on 
    a.id=G.major_id and a.colid=g.minor_id  
left join
sys.extended_properties f
on 
    d.id=f.major_id and f.minor_id=0
where 
   d.name in (select TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and ORDINAL_POSITION = '1')   
order by 
    a.id,a.colorder";
                var db = new PetaPoco.Database(DB_TEXT.Text, string.Empty);
                var query = db.QueryMultiple(sql);
                var tables = query.Read<DB_MODEL>();
                var excel = new List<string[]>();
                excel.Add(new[] { "表名", "表说明", "字段序号", "字段名", "标识", "主键", "类型", "占用字节数", "长度", "小数位数", "允许空", "默认值", "字段说明", });
                excel.AddRange(tables.Select(q => new[] {
                    q.TABLE_NAME, q.TABLE_DESC, q.COLUMN_INDEX.ToString(), q.COLUMN_NAME, q.COLUMN_ISIDENTITY, q.COLUMN_ISPK, q.COLUMN_TYPE,
                    $"{q.COLUMN_BITY }", $"{q.COLUMN_LENGTH}", $"{q.COLUMN_POINT }", q.COLUMN_ISNULL, q.COLUMN_DEFAULT, q.COLUMN_DESC }));
                SanJing.Excel.Export.SaveAs2007(EXCEL_TEXT.Text, excel);
                if (File.Exists("conn.ini"))
                {
                    if (File.ReadAllLines("conn.ini", Encoding.Default).Contains(DB_TEXT.Text.Trim()))
                    {
                        ERROR_LABLE.Text = "SUCCESS";
                        return;
                    }
                }
                File.AppendAllLines("conn.ini", new[] { DB_TEXT.Text.Trim() });
                ERROR_LABLE.Text = "SUCCESS";

            }
            catch (Exception ex)
            {
                ERROR_LABLE.Text = ex.Message.ToUpper();
            }
        }
    }
    public class DB_MODEL
    {
        public string TABLE_NAME { get; set; }
        public string TABLE_DESC { get; set; }
        public int COLUMN_INDEX { get; set; }
        public string COLUMN_NAME { get; set; }
        public string COLUMN_ISIDENTITY { get; set; }
        public string COLUMN_ISPK { get; set; }
        public string COLUMN_TYPE { get; set; }
        public int COLUMN_BITY { get; set; }
        public int COLUMN_LENGTH { get; set; }
        public int COLUMN_POINT { get; set; }
        public string COLUMN_ISNULL { get; set; }
        public string COLUMN_DEFAULT { get; set; }
        public string COLUMN_DESC { get; set; }
    }
}
