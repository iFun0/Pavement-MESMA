using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PavementDetection
{
    public partial class AgingReport : Form
    {
        int[] PAModelNumSet;//创建老化初期模型号数组
        int[] MAModelNumSet;//创建老化中期模型号数组
        int[] HAModelNumSet;//创建老化后期模型号数组
        MESMAForm tmp_MESMAForm;

        public AgingReport()
        {
            InitializeComponent();
        }

        public AgingReport(MESMAForm tmp_fatherform, double tmp_pixelXSize, double tmp_pixelYSize, int tmp_TypeofModel, DataTable tmp_modelinfo)
        {
            InitializeComponent();

            tmp_MESMAForm = tmp_fatherform;

            //根据模型类型填充丰度影像的所有波段
            for (int i = 1; i <= tmp_TypeofModel; i++)
            {
                PavementBandsComboBox.Items.Add(i.ToString());
            }
            //PavementBandsComboBox.Items.Add(tmp_TypeofModel.ToString());

            //获取影像分辨率
            PixelSizeXTextBox.Text = tmp_pixelXSize.ToString();
            PixelSizeYTextBox.Text = tmp_pixelYSize.ToString();

            //获取具体的模型信息
            ModelInfodataGridView.DataSource = tmp_modelinfo;


        }

        private void AgingReport_Load(object sender, EventArgs e)
        {
            //公路属性信息初始化
            ReportRoadTypeComboBox.SelectedIndex = 0;
            ReportRoadLevelcomboBox.SelectedIndex = 0;
            ReportPaveTypecomboBox.SelectedIndex = 0;
            ReportPaveDatedateTimePicker.Checked = false;
            ReportYanghudateTimePicker.Checked = false;
            ReportUselencheckBox.Checked = false;
            ReportUseLennumericUpDown.Enabled = false;
            //初始选中第一个波段
            PavementBandsComboBox.SelectedIndex = 0;
            //设置DataGridView样式
            ModelInfodataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ModelInfodataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //ModelInfodataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            for (int i = 0; i < ModelInfodataGridView.ColumnCount; i++)//设置列不可排序
            {
                ModelInfodataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }


        /*---------------------------功能函数---------------------------------*/
        //如果使用年数不知，则不使用
        private void ReportUselencheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ReportUselencheckBox.Checked == true)
            {
                ReportUseLennumericUpDown.Enabled = true;
            }
            else
                ReportUseLennumericUpDown.Enabled = false;
        }

        //将含有老化初期端元的模型号选中添加到list中
        private void PAModelAddBt_Click(object sender, EventArgs e)
        {
            ModelAddList(PAModellistBox, MAModellistBox, HAModellistBox);
        }

        //将含有老化中期端元的模型号选中添加到list中
        private void MAModelAddBt_Click(object sender, EventArgs e)
        {
            ModelAddList(MAModellistBox, PAModellistBox, HAModellistBox);
        }

        //将含有老化后期端元的模型号选中添加到list中
        private void HAModelAddBt_Click(object sender, EventArgs e)
        {
            ModelAddList(HAModellistBox, PAModellistBox, MAModellistBox);
        }

        //双击删除错误老化初期模型号
        private void PAModellistBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PAModellistBox.Items.Remove(PAModellistBox.SelectedItem);

        }

        //双击删除错误老化中期模型号
        private void MAModellistBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MAModellistBox.Items.Remove(MAModellistBox.SelectedItem);
        }

        //双击删除错误老化后期模型号
        private void HAModellistBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HAModellistBox.Items.Remove(HAModellistBox.SelectedItem);
        }

        //参数设置确认
        private void ModelParamOkbt_Click(object sender, EventArgs e)
        {
            //检查是否输入了公路属性信息
            if (ReportRoadNameTextBox.Text.ToString() == "")
            {
                MessageBox.Show("请输入公路名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //检查是否已经选择了不同老化路面的模型号
            if (PAModellistBox.Items.Count == 0)
            {
                MessageBox.Show("请选择老化初期路面的模型号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MAModellistBox.Items.Count == 0)
            {
                MessageBox.Show("请选择老化中期路面的模型号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (HAModellistBox.Items.Count == 0)
            {
                MessageBox.Show("请选择老化后期路面的模型号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //获取公路属性信息
            string ReportRoadName = ReportRoadNameTextBox.Text.ToString();
            string ReportRoadType = ReportRoadTypeComboBox.SelectedItem.ToString();
            string ReportRoadLevel = ReportRoadLevelcomboBox.SelectedItem.ToString();
            string ReportPaveType = ReportPaveTypecomboBox.SelectedItem.ToString();
            string ReportPaveDate = "未知";
            if (ReportPaveDatedateTimePicker.Checked == true)
            {
                ReportPaveDate = ReportPaveDatedateTimePicker.Text.ToString().Trim();
            }
            string ReportYanghuDate = "未知";
            if (ReportYanghudateTimePicker.Checked == true)
            {
                ReportYanghuDate = ReportYanghudateTimePicker.Text.ToString().Trim();
            }
            string ReportUseLength = "未知";
            if (ReportUselencheckBox.Checked == true)
            {
                ReportUseLength = ReportUseLennumericUpDown.Value.ToString();
            }

            //获取路面丰度影像的波段
            int PavementFracBands = PavementBandsComboBox.SelectedIndex;
            //根据listbox项数实例化数组
            PAModelNumSet = new int[PAModellistBox.Items.Count];
            MAModelNumSet = new int[MAModellistBox.Items.Count];
            HAModelNumSet = new int[HAModellistBox.Items.Count];
            //向老化初期模型号数组赋值
            for (int i = 0; i < PAModellistBox.Items.Count; i++)
            {
                PAModelNumSet[i] = int.Parse(PAModellistBox.Items[i].ToString());
            }
            //向老化中期模型号数组赋值
            for (int i = 0; i < MAModellistBox.Items.Count; i++)
            {
                MAModelNumSet[i] = int.Parse(MAModellistBox.Items[i].ToString());
            }
            //向老化中期模型号数组赋值
            for (int i = 0; i < HAModellistBox.Items.Count; i++)
            {
                HAModelNumSet[i] = int.Parse(HAModellistBox.Items[i].ToString());
            }
            //将每个老化阶段的模型号传入MESMA模型中
            tmp_MESMAForm.ExtractReportParameter(PAModelNumSet, MAModelNumSet, HAModelNumSet, PavementFracBands,
                ReportRoadName, ReportRoadType, ReportRoadLevel, ReportPaveType, ReportPaveDate, ReportYanghuDate, ReportUseLength);

            this.Close();

        }

        /*---------------------------公共函数---------------------------------*/

        //将模型信息表中选中的模型添加到相应的listbox中
        private void ModelAddList(ListBox tmp_ListBox_Self, ListBox tmp_ListBox_other1, ListBox tmp_ListBox_other2)
        {
            int NumofSelectedRows = ModelInfodataGridView.SelectedRows.Count;
            if (NumofSelectedRows > 0)
            {
                for (int i = NumofSelectedRows; i > 0; i--)
                {
                    //遍历listbox所有项检查要添加的序号是否已经存在
                    int SameCheck = 0;
                    foreach (object j1 in tmp_ListBox_Self.Items)
                    {
                        if (j1.ToString() == ModelInfodataGridView.SelectedRows[i - 1].Cells[0].Value.ToString())
                        {
                            SameCheck = 1;//如果存在相同的序号则赋值为1
                        }
                    }
                    foreach (object j2 in tmp_ListBox_other1.Items)
                    {
                        if (j2.ToString() == ModelInfodataGridView.SelectedRows[i - 1].Cells[0].Value.ToString())
                        {
                            SameCheck = 2;//如果存在相同的序号则赋值为1
                        }
                    }
                    foreach (object j2 in tmp_ListBox_other2.Items)
                    {
                        if (j2.ToString() == ModelInfodataGridView.SelectedRows[i - 1].Cells[0].Value.ToString())
                        {
                            SameCheck = 2;//如果存在相同的序号则赋值为1
                        }
                    }
                    switch (SameCheck)
                    {
                        case 0://如果SameCheck为0则进行添加，如果为1则不添加
                            tmp_ListBox_Self.Items.Add(ModelInfodataGridView.SelectedRows[i - 1].Cells[0].Value.ToString());
                            break;
                        case 1://如果SameCheck为1则与自身Items重复
                            MessageBox.Show("该模型号" + ModelInfodataGridView.SelectedRows[i - 1].Cells[0].Value.ToString() + "已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case 2://如果SameCheck为2则与其他Items重复
                            MessageBox.Show("该模型号" + ModelInfodataGridView.SelectedRows[i - 1].Cells[0].Value.ToString() + "与其他老化阶段路面模型重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }
                }
            }
            else
                MessageBox.Show("请至少选择一个模型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }







    }
}
