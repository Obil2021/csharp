using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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

namespace EXCELTOWEBAPICODE
{
    public partial class MainWindow : MaterialSkin.Controls.MaterialForm
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void EXCEL_TEXT_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Multiselect = true;//该值确定是否可以选择多个文件
            // dialog.Title = "请选择文件夹";
            dialog.Filter = "EXCEL2007文件(*.XLSX)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    return;
                }
                EXCEL_TEXT.Text = dialog.FileName;
            }
        }

        private void CS_TEXT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    return;
                }
                CS_TEXT.Text = dialog.SelectedPath;
            }
        }

        private void EXPORT_BUTTON_Click(object sender, EventArgs e)
        {
            ERROR_LABLE.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(EXCEL_TEXT.Text))
            {
                ERROR_LABLE.Text = "EXCELFULLFILENAME IS NULL";
                return;
            }
            if (!File.Exists(EXCEL_TEXT.Text.Trim()))
            {
                ERROR_LABLE.Text = "EXCELFULLFILENAME IS NOT EXIST";
                return;
            }
            if (string.IsNullOrWhiteSpace(CS_TEXT.Text))
            {
                ERROR_LABLE.Text = "WEBAPICSSAVEPATH IS NULL";
                return;
            }
            if (!Directory.Exists(CS_TEXT.Text.Trim()))
            {
                ERROR_LABLE.Text = "WEBAPICSSAVEPATH IS NOT EXIST";
                return;
            }
            try
            {
                var excel = SanJing.Excel.Import.ReadAs2007(EXCEL_TEXT.Text.Trim(), 9).ToArray();
                if (excel.Length == 0)
                {
                    ERROR_LABLE.Text = $"EXCEL CONTENT IS NULL";
                    return;
                }
                if (excel[0].Length != 10)
                {
                    ERROR_LABLE.Text = $"EXCEL FORMAT IS ERROR";
                    return;
                }
                var namesapce = string.Empty;
                var apiname = string.Empty;
                var apidesc = string.Empty;
                var fullexcel = new List<string[]>();
                for (int i = 1; i < excel.Length; i++)
                {
                    var line = excel[i];
                    if (i == 1)
                    {
                        if (string.IsNullOrWhiteSpace(line[0]))
                        {
                            ERROR_LABLE.Text = $"{excel[0][0]}:必填，位于{i}行";
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(line[1]))
                        {
                            ERROR_LABLE.Text = $"{excel[0][1]}:必填，位于{i}行";
                            return;
                        }
                        namesapce = line[0];
                        apiname = line[1];
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(line[0]))
                        {
                            line[0] = namesapce;
                        }
                        else
                        {
                            namesapce = line[0];
                        }
                        if (string.IsNullOrWhiteSpace(line[1]))
                        {
                            line[1] = apiname;
                        }
                        else
                        {
                            apiname = line[1];
                            apidesc = string.Empty;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(apidesc))
                    {
                        if (string.IsNullOrWhiteSpace(line[2]))
                        {
                            apidesc = apiname;
                            line[2] = apiname;
                        }
                        else
                        {
                            apidesc = line[2];
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(line[2]))
                        {
                            line[2] = apidesc;
                        }
                        else
                        {
                            apidesc = line[2];
                        }
                    }
                    fullexcel.Add(line);
                }
                var namespaces = fullexcel.GroupBy(q => q[0]).ToArray();
                foreach (var item in namespaces)
                {
                    var apis = item.GroupBy(q => q[1]).ToArray();
                    foreach (var item1 in apis)
                    {
                        var request = item1.Where(q => !string.IsNullOrWhiteSpace(q[3])).ToArray();
                        var response = item1.Where(q => !string.IsNullOrWhiteSpace(q[8])).ToArray();
                        var code = Properties.Resources.String1.Replace("MvcApplication2", item.Key);
                        code = code.Replace("UserInfo", item1.Key);
                        code = code.Replace("userInfo", item1.Key.Substring(0, 1).ToLower() + item1.Key.Substring(1));
                        code = code.Replace("用户信息", item1.First()[2]);

                        foreach (var item2 in request)
                        {
                            var propmodel = @"/// <summary>
            /// @密码
            /// </summary>
            @[Required]
            @[StringLength(128, MinimumLength = 32)]
            public @string @Password { get; set; }@Value
            @RequestClass";
                            propmodel = propmodel.Replace("@密码", item2[7]);
                            propmodel = propmodel.Replace("@[Required]", string.IsNullOrWhiteSpace(item2[5]) ? string.Empty : $"[{item2[5]}]");
                            propmodel = propmodel.Replace("@[StringLength(128, MinimumLength = 32)]", string.IsNullOrWhiteSpace(item2[6]) ? string.Empty : $"[{item2[6]}]");
                            propmodel = propmodel.Replace("@string", item2[4]);
                            propmodel = propmodel.Replace("@Password", item2[3]);
                            propmodel = propmodel.Replace("@Value", item2[4].ToLower() == "string" ? " = string.Empty;" : string.Empty);
                            code = code.Replace("@RequestClass", propmodel);
                        }
                        code = code.Replace("@RequestClass", string.Empty);

                        foreach (var item2 in response.GroupBy(q => q[8].Split('.')[0]))
                        {
                            if (item2.Count() == 1)
                            {
                                var propmodel = @"/// <summary>
            /// @密码
            /// </summary>
            public string @Password { get; set; } = string.Empty;
            @ResponseClass";
                                propmodel = propmodel.Replace("@密码", item2.First()[9]);
                                propmodel = propmodel.Replace("@Password", item2.First()[8]);
                                code = code.Replace("@ResponseClass", propmodel);
                            }
                            else
                            {
                                try
                                {
                                    var cla = item2.Single(q => !q[8].Contains("."));
                                    var propmodel = @"/// <summary>
            /// @密码
            /// </summary>
            public @string[] @Password { get; set; } = new @string[0];
            @ResponseClass";
                                    propmodel = propmodel.Replace("@密码", cla[9]);
                                    propmodel = propmodel.Replace("@string", cla[8] + "Item");
                                    propmodel = propmodel.Replace("@Password", cla[8]);
                                    code = code.Replace("@ResponseClass", propmodel);

                                    propmodel = @"/// <summary>
            /// @密码
            /// </summary>
            public class @Password
            {
                @ResponseClassItem
            }
            @ResponseClass";
                                    propmodel = propmodel.Replace("@密码", cla[9]);
                                    propmodel = propmodel.Replace("@Password", cla[8] + "Item");
                                    code = code.Replace("@ResponseClass", propmodel);
                                    foreach (var item3 in item2)
                                    {
                                        if (item3 == cla)
                                            continue;
                                        propmodel = @"/// <summary>
                /// @密码
                /// </summary>
                public string @Password { get; set; } = string.Empty;
                @ResponseClassItem";
                                        propmodel = propmodel.Replace("@密码", item3[9]);
                                        propmodel = propmodel.Replace("@Password", item3[8].Replace(cla[8] + ".", string.Empty));
                                        code = code.Replace("@ResponseClassItem", propmodel);
                                    }
                                    code = code.Replace("@ResponseClassItem", string.Empty);
                                }
                                catch(Exception ex)
                                {
                                    ERROR_LABLE.Text = $"{item2.Key} FORMAT IS ERROR";
                                    return;
                                }
                            }
                        }
                        code = code.Replace("@ResponseClass", string.Empty);
                        File.WriteAllText($@"{CS_TEXT.Text}\{item1.Key}Controller.cs", code, Encoding.UTF8);
                    }
                }
                ERROR_LABLE.Text = "SUCCESS";
            }
            catch (Exception ex) { ERROR_LABLE.Text = ex.Message; }
        }
    }
}
