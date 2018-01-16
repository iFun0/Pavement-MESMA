using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PavementDetection
{
    /// <summary>
    /// 文件名:MESMAForm.cs
    /// 功能描述:该模块可用于端元的收集、查看和优化，用于MESMA模块的输入端元光谱
    /// 创建人:潘一凡
    /// 单位:北京大学遥感所
    /// 联系方式:yfpan@pku.edu.cn
    /// 创建日期：2016-07-15
    /// 修改人：
    /// 修改日期：
    /// 修改备注：
    /// 版本：1.0
    /// </summary>
    /// 
    public partial class EMCollection : Form
    {
        private DataTable spec_grid_dt = new DataTable();//创建全局ＤataTable用于存储光谱元数据 
        private DataSet spec_chart_ds = new DataSet();//创建全局数据集存储DataTable集存储光谱数据
       
        //窗体构造函数
        public EMCollection()
        {
            InitializeComponent();

        }

        //窗体Load事件
        private void EMCollection_Load(object sender, EventArgs e)
        {
            //初始化全局DataTable并与dataGridView1连接，增加所需列名
            spec_grid_dt.Columns.Add("序号", typeof(string));
            spec_grid_dt.Columns.Add("地物名称", typeof(string));
            spec_grid_dt.Columns.Add("地物类型", typeof(string));
            spec_grid_dt.Columns.Add("光谱名称", typeof(string));
            spec_grid_dt.Columns.Add("波段数", typeof(string));
            spec_grid_dt.Columns.Add("FWHM", typeof(string));
            spec_grid_dt.Columns.Add("照片文件", typeof(string));
            spec_grid_dt.Columns.Add("测量日期", typeof(string));
            spec_grid_dt.Columns.Add("测量时间", typeof(string));
            spec_grid_dt.Columns.Add("是否路面", typeof(string));
            spec_grid_dt.Columns.Add("公路类型", typeof(string));
            spec_grid_dt.Columns.Add("公路等级", typeof(string));
            spec_grid_dt.Columns.Add("铺设日期", typeof(string));
            spec_grid_dt.Columns.Add("使用年数", typeof(string));
            spec_grid_dt.Columns.Add("健康状况", typeof(string));
            spec_grid_dt.Columns.Add("病害类型", typeof(string));
            spec_grid_dt.Columns.Add("色卡号", typeof(string));
            spec_grid_dt.Columns.Add("经度", typeof(string));
            spec_grid_dt.Columns.Add("纬度", typeof(string));
            spec_grid_dt.Columns.Add("高程", typeof(string));
            spec_grid_dt.Columns.Add("光谱源", typeof(string));
            spec_grid_dt.Columns.Add("备注", typeof(string));
            spec_grid_dt.Columns.Add("EAR指数", typeof(string));
            dataGridView1.DataSource = spec_grid_dt;

            //初始化Chart控件样式
            InitializeChartControl();

            this.EMCProgressBar.Minimum = 0;

        }

        //初始化Chart控件样式函数
        private void InitializeChartControl()
        {
            # region 设置图表的属性
            //图表的背景色
            EMColeChart.BackColor = Color.FromArgb(211, 223, 240);
            //图表背景色的渐变方式
            EMColeChart.BackGradientStyle = GradientStyle.TopBottom;
            //图表的边框颜色
            EMColeChart.BorderlineColor = Color.FromArgb(26, 59, 105);
            //图表的边框线条样式
            EMColeChart.BorderlineDashStyle = ChartDashStyle.Solid;
            //图表边框线条的宽度
            EMColeChart.BorderlineWidth = 2;
            //图表边框的皮肤
            EMColeChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            # endregion

            # region 设置图表区属性
            //背景色
            EMColeChart.ChartAreas[0].BackColor = Color.FromArgb(211, 223, 240); //Color.FromArgb(64, 165, 191, 228);
            //背景渐变方式
            EMColeChart.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            //渐变和阴影的辅助背景色
            EMColeChart.ChartAreas[0].BackSecondaryColor = Color.White;
            //边框颜色
            EMColeChart.ChartAreas[0].BorderColor = Color.FromArgb(64, 64, 64, 64);
            //阴影颜色
            EMColeChart.ChartAreas[0].ShadowColor = Color.Transparent;
            //设置X轴和Y轴线条的颜色和宽度
            EMColeChart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            EMColeChart.ChartAreas[0].AxisX.LineWidth = 1;
            EMColeChart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            EMColeChart.ChartAreas[0].AxisY.LineWidth = 1;
            //EMColeChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
            //设置图表区网格横纵线条的颜色和宽度
            EMColeChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            EMColeChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            EMColeChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            EMColeChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            //设置图标区坐标轴标题及间隔
            EMColeChart.ChartAreas[0].AxisX.Title = "波长(um)";
            EMColeChart.ChartAreas[0].AxisX.Interval = 100;
            EMColeChart.ChartAreas[0].AxisY.Title = "反射率(%)";
            # endregion


        }

        /// 通过TXT文件添加端元集，判断文件结构是否符合要求没做
        private void FileAddbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog Spec_file = new OpenFileDialog();//打开文件对话框，选择TXT文件
            Spec_file.Multiselect = true;//允许选择多个文件
            Spec_file.Filter = "标准光谱文件|*.txt";//打开文件对话框筛选器
            //string spec_Path;//文件完整的路径名
            if (Spec_file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int num_spec = dataGridView1.RowCount + 1;//统计光谱记录个数,赋值给序号列
                    foreach (string spec_Path in Spec_file.FileNames)//遍历每个光谱库文件的路径
                    {
                        DataRow spec_grid_dr = spec_grid_dt.NewRow();//创建行存储光谱元数据

                        string[] lines = System.IO.File.ReadAllLines(spec_Path, Encoding.Default).Where(s => !string.IsNullOrEmpty(s)).ToArray();//利用ReadAllline读取光谱TXT文件所有行存入字符串数组                      
                        if (lines[0] != "**********************元数据**********************")
                        {
                            MessageBox.Show(this, "该文件不是标准光谱文件，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        string spec_name = lines[3].Trim().Split(':')[1]; //获取光谱文件中的文件名作为系列名

                        //创建DataTable存储反射率值，并用光谱文件名命名
                        DataTable spec_chart_dt = new DataTable(spec_name);
                        spec_chart_dt.Columns.Add("波长", typeof(string));//添加列名
                        spec_chart_dt.Columns.Add("反射率", typeof(string));//添加列名

                        //赋值给序号列值
                        spec_grid_dr[0] = num_spec;
                        num_spec++;

                        //遍历光谱文件，提取元数据和反射率值
                        int j = 1;
                        for (int i = 0; i < lines.Length; i++)
                        {

                            if (lines[i].Contains(":") && i < 23)//遍历光谱文件中的元数据行，获取光谱属性值
                            {
                                string[] spec_meta = lines[i].Trim().Split(':');
                                spec_grid_dr[j] = spec_meta[1];
                                j++;
                            }
                            else if (lines[i].Contains(",") && i >= 23)//遍历光谱文件中的光谱数据行，获取光谱反射率值
                            {
                                DataRow spec_chart_dr = spec_chart_dt.NewRow();//创建DataTable的行读取波长值和反射率值
                                string[] spectrumdata = lines[i].Trim().Split(',');
                                //MessageBox.Show(spectrumdata[1]);
                                spec_chart_dr["波长"] = spectrumdata[0];
                                spec_chart_dr["反射率"] = spectrumdata[1];
                                spec_chart_dt.Rows.Add(spec_chart_dr);
                            }
                        }
                        //将每一条光谱记录的反射率DataTable添加到Dataset
                        spec_chart_ds.Tables.Add(spec_chart_dt);
                        //spec_grid_dr[spec_grid_dt.Columns.Count - 1] = spec_Path;//将文件的路径赋值给最后一列用于显示
                        spec_grid_dt.Rows.Add(spec_grid_dr);//向元数据表中添加写入行
                        dataGridView1.DataSource = spec_grid_dt;//datagridview连接DataTable spec_grid_dt用于显示
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//捕捉异常
                }
            }


        }

        //保存筛选出的文件
        private void SaveFileBt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要输出的光谱记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //设置进度条值
                this.EMCProgressBar.Minimum = 0;
                this.EMCProgressBar.Maximum = dataGridView1.SelectedRows.Count;
                this.EMCProgressBar.Value = 0;

                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)//遍历选中光谱记录，写入新文件中
                    {
                        string[] lines = new string[23];//构建元数据字符串数组

                        //基本信息
                        lines[0] = "**********************元数据**********************";
                        lines[1] = "地物名称:" + dataGridView1.SelectedRows[i].Cells[1].Value.ToString().Trim();
                        lines[2] = "地物类型:" + dataGridView1.SelectedRows[i].Cells[2].Value.ToString().Trim();
                        lines[3] = "光谱名称:" + dataGridView1.SelectedRows[i].Cells[3].Value.ToString().Trim();
                        lines[4] = "波段数:" + dataGridView1.SelectedRows[i].Cells[4].Value.ToString().Trim();
                        lines[5] = "FWHM:" + dataGridView1.SelectedRows[i].Cells[5].Value.ToString().Trim();
                        lines[6] = "照片文件:" + dataGridView1.SelectedRows[i].Cells[6].Value.ToString().Trim();
                        lines[7] = "测量日期:" + dataGridView1.SelectedRows[i].Cells[7].Value.ToString().Trim();
                        lines[8] = "测量时间:" + dataGridView1.SelectedRows[i].Cells[8].Value.ToString().Trim();
                        lines[9] = "是否路面:" + dataGridView1.SelectedRows[i].Cells[9].Value.ToString().Trim();
                        lines[10] = "公路类型:" + dataGridView1.SelectedRows[i].Cells[10].Value.ToString().Trim();
                        lines[11] = "公路等级:" + dataGridView1.SelectedRows[i].Cells[11].Value.ToString().Trim();
                        lines[12] = "铺设日期:" + dataGridView1.SelectedRows[i].Cells[12].Value.ToString().Trim();
                        lines[13] = "使用年数:" + dataGridView1.SelectedRows[i].Cells[13].Value.ToString().Trim();
                        lines[14] = "健康状况:" + dataGridView1.SelectedRows[i].Cells[14].Value.ToString().Trim();
                        lines[15] = "病害类型:" + dataGridView1.SelectedRows[i].Cells[15].Value.ToString().Trim();
                        lines[16] = "色卡号:" + dataGridView1.SelectedRows[i].Cells[16].Value.ToString().Trim();
                        lines[17] = "经度:" + dataGridView1.SelectedRows[i].Cells[17].Value.ToString().Trim();
                        lines[18] = "纬度:" + dataGridView1.SelectedRows[i].Cells[18].Value.ToString().Trim();
                        lines[19] = "高程:" + dataGridView1.SelectedRows[i].Cells[19].Value.ToString().Trim();
                        lines[20] = "光谱源:" + dataGridView1.SelectedRows[i].Cells[20].Value.ToString().Trim();
                        lines[21] = "备注:" + dataGridView1.SelectedRows[i].Cells[21].Value.ToString().Trim();
                        lines[22] = "**********************光谱数据**********************";

                        //提取反射率数据
                        string DataTable_name = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
                        int ref_bands = spec_chart_ds.Tables[DataTable_name].Rows.Count;
                        string[] ref_data = new string[ref_bands];
                        for (int iRows = 0; iRows < spec_chart_ds.Tables[DataTable_name].Rows.Count; iRows++)
                        {
                            ref_data[iRows] = spec_chart_ds.Tables[DataTable_name].Rows[iRows][0].ToString().Trim() + "," + spec_chart_ds.Tables[DataTable_name].Rows[iRows][1].ToString().Trim();
                            //MessageBox.Show(ref_data[iRows]);
                        }

                        //光谱文件命名
                        string SpecFilename = folderBrowserDialog.SelectedPath.ToString() + "\\" + dataGridView1.SelectedRows[i].Cells[3].Value.ToString().Trim() + ".txt";
                        //MessageBox.Show(folderBrowserDialog.SelectedPath);
                        //MessageBox.Show(SpecFilename);

                        //输出TXT文件
                        StreamWriter spec_file = new StreamWriter(SpecFilename, false, Encoding.UTF8);
                        foreach (string line in lines)//遍历存储元数据
                        {
                            spec_file.WriteLine(line);//直接追加文件末尾，换行
                        }
                        foreach (string data_line in ref_data)//遍历存储反射率数据
                        {
                            spec_file.WriteLine(data_line);
                        }
                        spec_file.Close();//关闭文件      

                        this.EMCProgressBar.Value = i+1;//更新进度条
                    }

                    MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }

        }

        //选择全部光谱记录
        private void SelecteallEMbt_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.SelectAll();//dataGridView里的行全部选中
                //更新光谱曲线绘图区域
                EMColeChart.Series.Clear();//清空Chart绘图控件
                for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)//遍历所有选中记录
                {
                    string DataTable_name = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
                    string series_name = dataGridView1.SelectedRows[i].Cells[0].Value.ToString() + dataGridView1.SelectedRows[i].Cells[2].Value.ToString();
                    DisplaySpec(DataTable_name, series_name);
                }
            }
            else
                MessageBox.Show(this, "列表中暂无光谱记录，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //反向选择剩余光谱记录
        private void selectinversebt_Click(object sender, EventArgs e)
        {

            if (this.dataGridView1.Rows.Count > 0)//判断是否有记录
            {
                for (int iSelect = 0; iSelect < this.dataGridView1.Rows.Count; iSelect++)//有记录
                {
                    if (this.dataGridView1.Rows[iSelect].Selected)//已被选择
                        this.dataGridView1.Rows[iSelect].Selected = false;//则不选
                    else
                        this.dataGridView1.Rows[iSelect].Selected = true;//否则选中
                    //更新光谱曲线绘图区域
                    EMColeChart.Series.Clear();//清空Chart绘图控件
                    for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)//遍历所有选中记录
                    {
                        string DataTable_name = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
                        string series_name = dataGridView1.SelectedRows[i].Cells[0].Value.ToString() + dataGridView1.SelectedRows[i].Cells[2].Value.ToString();
                        DisplaySpec(DataTable_name, series_name);
                    }
                }
            }
            else
                MessageBox.Show(this, "列表中暂无光谱记录，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //删除光谱记录
        private void DeleteEMbt_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count > 0)
                {
                    if (this.dataGridView1.SelectedRows.Count > 0)//判断是否有选中记录
                    {
                        DialogResult RSS = MessageBox.Show(this, "确定要删除选中的端元光谱吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        switch (RSS)
                        {
                            case DialogResult.Yes:
                                for (int i = this.dataGridView1.SelectedRows.Count; i > 0; i--)
                                {
                                    //删除光谱记录对应的DataTable表
                                    string DataTable_name = dataGridView1.SelectedRows[i - 1].Cells[3].Value.ToString();
                                    spec_chart_ds.Tables.Remove(DataTable_name);

                                    //删除光谱曲线
                                    string series_name = dataGridView1.SelectedRows[i - 1].Cells[0].Value.ToString() + dataGridView1.SelectedRows[i - 1].Cells[2].Value.ToString();
                                    //this.EMColeChart.Series[series_name].Points.Clear();                                  
                                    this.EMColeChart.Series.Remove(EMColeChart.Series.FindByName(series_name));//判断是否已画出该记录的光谱曲线

                                    //删除光谱记录
                                    spec_grid_dt.Rows.RemoveAt(dataGridView1.SelectedRows[i - 1].Index);
                                }
                                //序号列重新排序生成序号
                                for (int j = 0; j < spec_grid_dt.Rows.Count; j++)
                                {
                                    spec_grid_dt.Rows[j][0] = j + 1;//序号列重新排序生成序号
                                }
                                break;
                            case DialogResult.No:
                                break;
                        }
                    }
                    else
                        MessageBox.Show(this, "请至少选择一条光谱记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(this, "列表中暂无光谱记录，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }
        }


        //选择某一行立即显示光谱曲线
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)//多选光谱记录
                {
                    for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)//遍历所有选中记录
                    {
                        string DataTable_name = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
                        string series_name = dataGridView1.SelectedRows[i].Cells[0].Value.ToString() + dataGridView1.SelectedRows[i].Cells[2].Value.ToString();
                        DisplaySpec(DataTable_name, series_name);
                    }
                }
                else//单选光谱记录
                {
                    EMColeChart.Series.Clear();//清空Chart绘图控件
                    string DataTable_name = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    string series_name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    DisplaySpec(DataTable_name, series_name);
                }

            }

        }
       
        //显示光谱曲线公共函数
        private void DisplaySpec(string tmp_DataTable_name, string tmp_series_name)
        {
            if (EMColeChart.Series.IsUniqueName(tmp_series_name))
            {
                //创建系列值
                Series spec_series = new Series(tmp_series_name);
                spec_series.Points.DataBind(spec_chart_ds.Tables[tmp_DataTable_name].AsEnumerable(), "波长", "反射率", "");
                spec_series.ChartType = SeriesChartType.Line;
                //spec_series.BorderColor = Color.FromArgb(180, 26, 59, 105);
                spec_series.BorderWidth = 2;
                //spec_series.ShadowOffset = 1;
                //spec_series.ShadowColor = Color.Black;
                spec_series.XValueMember = "波长";
                spec_series.XValueMember = "反射率";
                EMColeChart.Series.Add(spec_series);
                EMColeChart.ChartAreas[0].RecalculateAxesScale();
            }
        }

        //利用chart控件显示选中光谱数据，并用不同颜色显示
        private void DataShowbt_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count > 0)
                {
                    if (this.dataGridView1.SelectedRows.Count > 0)//判断是否有选中记录
                    {
                        EMColeChart.Series.Clear();//清空Chart绘图控件
                        for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)//遍历所有选中记录
                        {

                            string DataTable_name = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
                            string series_name = dataGridView1.SelectedRows[i].Cells[0].Value.ToString() + dataGridView1.SelectedRows[i].Cells[2].Value.ToString();
                            //创建系列值
                            Series spec_series = new Series(series_name);
                            spec_series.Points.DataBind(spec_chart_ds.Tables[DataTable_name].AsEnumerable(), "波长", "反射率", "");
                            spec_series.ChartType = SeriesChartType.Line;
                            //spec_series.BorderColor = Color.FromArgb(180, 26, 59, 105);
                            spec_series.BorderWidth = 2;
                            //spec_series.ShadowOffset = 1;
                            //spec_series.ShadowColor = Color.Black;
                            spec_series.XValueMember = "波长";
                            spec_series.XValueMember = "反射率";
                            EMColeChart.Series.Add(spec_series);
                        }
                    }
                    else
                        MessageBox.Show(this, "请至少选择一条光谱记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(this, "列表中暂无光谱记录，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }

        }


        //EAR优化界面启动按钮事件
        private void EARoptbt_Click(object sender, EventArgs e)
        {
            switch (dataGridView1.RowCount)
            {
                case 0:
                    MessageBox.Show(this, "列表中暂无光谱记录，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                case 1:
                    MessageBox.Show(this, "请至少添加两条光谱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                default:
                    {                       
                        EAROpt earoptform1 = new EAROpt(this, spec_chart_ds);
                        //EAROpt earoptform1 = new EAROpt(spec_chart_ds);
                        earoptform1.ShowDialog();
                        return;
                    }
            }
        }

        //MESMA界面启动事件
        private void collectMESMAbt_Click(object sender, EventArgs e)
        {
            MESMAForm mesmaform2 = new MESMAForm();
            mesmaform2.ShowDialog();
        }

        //定义函数，添加EAR窗口计算出来的EAR值
        public void Add_EAR(double[] EAR_array)
        {
            for (int i = 0; i < spec_grid_dt.Rows.Count; i++)
                spec_grid_dt.Rows[i][spec_grid_dt.Columns.Count - 1] = EAR_array[i];

        }

        //更新进度条
        public void UpdateProgess(int tmp_provalue)
        {           
            this.EMCProgressBar.Maximum =spec_chart_ds.Tables.Count*spec_chart_ds.Tables.Count;
            this.EMCProgressBar.Value = tmp_provalue;
        }

        //根据EAR对列表进行排序
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Ascending);//顺序
        }

        

    }
}
