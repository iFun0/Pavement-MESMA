using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PavementDetection
{
    /// <summary>
    /// 文件名:TXTFileBuild.cs
    /// 功能描述:该模块可用于标准光谱文件的创建，标准光谱文件可用于光谱库上传、光谱计算处理和MESMA模块使用
    /// 创建人:潘一凡
    /// 单位:北京大学遥感所
    /// 联系方式:yfpan@pku.edu.cn
    /// 创建日期：2016-07-15
    /// 修改人：
    /// 修改日期：
    /// 修改备注：
    /// 版本：1.0
    /// </summary>

    public partial class TXTFileBuild : Form
    {
        int uselength;//使用年数
        int error_var;//容错标识

        DateTimePicker tmp_measure_picker = new DateTimePicker();
        DateTimePicker tmp_roadpave_picker = new DateTimePicker();

        //窗体构造函数
        public TXTFileBuild()
        {
            InitializeComponent();

        }

        //窗口初始化
        private void TXTFileBuild_Load(object sender, EventArgs e)
        {
            //uselength = MeasuredatePicker.Value.Year - RoadConsdateTimePicker.Value.Year;
            //UseLengthtextBox.Text = uselength.ToString().Trim();

            //初始化基本信息
            materialclasscomboBox.SelectedIndex = 0;

            //初始化光谱信息
            SpecSourcecomboBox.SelectedIndex = 0;

            //初始化测量信息
            MeasuredatePicker.Checked = false;
            MeasuretimePicker.Checked = false;

            //初始化位置信息
            GPScheckBox.Checked = false;
            longitudecomboBox.SelectedIndex = 0;
            latitudecomboBox.SelectedIndex = 0;
            longitudecomboBox.Enabled = false;
            longitudenumericUp.Enabled = false;
            latitudecomboBox.Enabled = false;
            latitudenumericUp.Enabled = false;
            AltitudeNumericUp.Enabled = false;

            //初始化路面信息
            IsPavecomboBox.SelectedIndex = 1;
            RoadClasscomboBox.SelectedIndex = 0;
            RoadRankcomboBox.SelectedIndex = 0;
            RoadConsdateTimePicker.Checked = false;
            HealthConditioncomboBox.SelectedIndex = 0;
            DistresscomboBox.SelectedIndex = 0;
            ColorValuecombox.SelectedIndex = 0;

        }

        //测量日期改变，动态更改使用年数
        private void MeasuredatePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsPavecomboBox.SelectedIndex == 0 && RoadConsdateTimePicker.Checked == true)
                {
                    int check = CompareDate(MeasuredatePicker, RoadConsdateTimePicker);
                    if (check == 1)
                    {
                        tmp_measure_picker.Value = MeasuredatePicker.Value;
                        uselength = MeasuredatePicker.Value.Year - RoadConsdateTimePicker.Value.Year;
                        UseLengthtextBox.Text = uselength.ToString().Trim();
                    }
                    else
                    {
                        MessageBox.Show("测量日期小于铺设日期，请修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        MeasuredatePicker.Value = tmp_roadpave_picker.Value;
                    }
                }
                else
                    tmp_measure_picker.Value = MeasuredatePicker.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }
        }

        //铺设日期改变，动态更改使用年数
        private void RoadConsdateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsPavecomboBox.SelectedIndex == 0 && MeasuredatePicker.Checked == true)
                {
                    int check = CompareDate(MeasuredatePicker, RoadConsdateTimePicker);
                    if (check == 1)
                    {
                        //tmp_roadpave_picker.Value = RoadConsdateTimePicker.Value;
                        uselength = MeasuredatePicker.Value.Year - RoadConsdateTimePicker.Value.Year;
                        UseLengthtextBox.Text = uselength.ToString().Trim();
                    }
                    else
                    {
                        MessageBox.Show("铺设日期大于测量日期，请修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //RoadConsdateTimePicker.Value = tmp_roadpave_picker.Value;
                        RoadConsdateTimePicker.Value = tmp_measure_picker.Value;
                    }
                }
                else
                    tmp_roadpave_picker.Value = RoadConsdateTimePicker.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }
        }

        //比较两个日期的大小
        private int CompareDate(DateTimePicker timepicker1, DateTimePicker timepicker2)
        {
            try
            {
                int tag_check = 1;
                int year1 = timepicker1.Value.Year;
                int year2 = timepicker2.Value.Year;
                int month1 = timepicker1.Value.Month;
                int month2 = timepicker2.Value.Month;
                int day1 = timepicker1.Value.Day;
                int day2 = timepicker2.Value.Day;
                if (year1 > year2)//如果年1大于年2，则日期1大于日期2，返回1
                {
                    return tag_check;
                }
                else if (year1 < year2)//如果年1小于年2，则日期1小于日期2，返回-1
                {
                    tag_check = -1;
                    return tag_check;
                }
                else//如果年1等于年2，则需要比较月份
                {
                    if (month1 > month2)//如果月1大于月2，则日期1大于日期2，返回1
                    {
                        return tag_check;
                    }
                    else if (month1 < month2)//如果月1小于月2，则日期1小于日期2，返回-1
                    {
                        tag_check = -1;
                        return tag_check;
                    }
                    else//如果月1等于月2，则需要比较天数
                    {

                        if (day1 > day2)//如果天1大于天2，则日期1大于日期2，返回1
                        {
                            return tag_check;
                        }
                        else if (day1 < day2)//如果天1小于天2，则日期1小于日期2，返回-1
                        {
                            tag_check = -1;
                            return tag_check;
                        }
                        else
                        {
                            return tag_check;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;//捕捉异常
            }
        }

        //是否是路面光谱
        private void IsPavecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsPavecomboBox.SelectedIndex == 1)//如果不是路面
                {
                    //初始化路面信息
                    IsPavecomboBox.SelectedIndex = 1;
                    RoadClasscomboBox.SelectedIndex = 0;
                    RoadRankcomboBox.SelectedIndex = 0;
                    RoadConsdateTimePicker.Checked = false;
                    HealthConditioncomboBox.SelectedIndex = 0;
                    DistresscomboBox.SelectedIndex = 0;
                    ColorValuecombox.SelectedIndex = 0;
                    //将控件改为不可用
                    RoadClasscomboBox.Enabled = false;
                    RoadRankcomboBox.Enabled = false;
                    RoadConsdateTimePicker.Enabled = false;
                    HealthConditioncomboBox.Enabled = false;
                    DistresscomboBox.Enabled = false;
                    UseLengthtextBox.Enabled = false;
                    ColorValuecombox.Enabled = false;
                }
                else//如果是路面
                {
                    //将控件改为可用
                    RoadClasscomboBox.Enabled = true;
                    RoadRankcomboBox.Enabled = true;
                    RoadConsdateTimePicker.Enabled = true;
                    HealthConditioncomboBox.Enabled = true;
                    DistresscomboBox.Enabled = true;
                    UseLengthtextBox.Enabled = true;
                    ColorValuecombox.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }

        }

        //获取波段数
        private void specdatarichTextBox_TextChanged(object sender, EventArgs e)
        {
            int NumofBands = specdatarichTextBox.Lines.Length;
            NumofbandstextBox.Text = NumofBands.ToString();
        }

        //打开图片文件，选取文件名
        private void OpenPhotobutton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog Photo_file = new OpenFileDialog();
                Photo_file.Filter = "JPEG|*.jpg";//打开文件对话框筛选器
                string photo_path;
                if (Photo_file.ShowDialog() == DialogResult.OK)
                {
                    photo_path = Photo_file.FileName;
                    PhotofilepathtextBox.Text = photo_path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }

        }

        //是否有GPS位置信息
        private void GPScheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GPScheckBox.Checked == true)
            {
                longitudecomboBox.Enabled = true;
                longitudenumericUp.Enabled = true;
                latitudecomboBox.Enabled = true;
                latitudenumericUp.Enabled = true;
                AltitudeNumericUp.Enabled = true;

            }
            else
            {
                longitudecomboBox.Enabled = false;
                longitudenumericUp.Enabled = false;
                latitudecomboBox.Enabled = false;
                latitudenumericUp.Enabled = false;
                AltitudeNumericUp.Enabled = false;
            }

        }

        //生成光谱TXT文件
        private void TXTbuildbt_Click(object sender, EventArgs e)
        {
            try
            {
                //检查是否有错误
                error_var = -1;//没有错误
                if (materialnametextBox.Text.ToString() == "")//如果地物名称为空
                    error_var = 0;
                else if (specdatarichTextBox.Text.ToString() == "")//如果光谱数据为空
                    error_var = 1;
                else if (savefilepathtextBox.Text.ToString() == "")//如果没有选择存储路径
                    error_var = 2;
                else if (!Directory.Exists(Path.GetDirectoryName(savefilepathtextBox.Text.ToString().Trim())))//如果存储路径不存在                 
                    error_var = 3;

                switch (error_var)
                {
                    case -1:
                        break;
                    case 0:
                        MessageBox.Show("请输入地物名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case 1:
                        MessageBox.Show("请输入光谱数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case 2:
                        MessageBox.Show("请选择输出路径及文件名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    case 3:
                        MessageBox.Show("保存路径不存在，请选择正确的输出路径及文件名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                //根据表单内容向字符串数组赋值

                string[] lines = new string[23];//构建元数据字符串数组

                //基本信息
                lines[0] = "**********************元数据**********************";
                lines[1] = "地物名称:" + materialnametextBox.Text.ToString().Trim();
                lines[2] = "地物类型:" + materialclasscomboBox.SelectedItem.ToString().Trim();

                //光谱信息
                lines[3] = "光谱名称:" + Path.GetFileNameWithoutExtension(savefilepathtextBox.Text.ToString().Trim());
                lines[4] = "波段数:" + NumofbandstextBox.Text.ToString().Trim();
                lines[5] = "FWHM:" + FWHMtextBox7.Text.ToString().Trim();
                lines[20] = "光谱源:" + SpecSourcecomboBox.SelectedItem.ToString().Trim();

                //测量信息
                if (PhotofilepathtextBox.Text.ToString() == "")//判断是否有照片文件
                    lines[6] = "照片文件:无";
                else
                    lines[6] = "照片文件:" + Path.GetFileName(PhotofilepathtextBox.Text.ToString().Trim());
                if (MeasuredatePicker.Checked == true)//判断测量日期是否可知
                    lines[7] = "测量日期:" + MeasuredatePicker.Text.ToString().Trim();
                else
                    lines[7] = "测量日期:未知";
                if (MeasuretimePicker.Checked == true)//判断测量时间是否可知
                    lines[8] = "测量时间:" + MeasuretimePicker.Text.ToString().Trim();
                else
                    lines[8] = "测量时间:未知";


                //路面信息
                lines[9] = "是否路面:" + IsPavecomboBox.SelectedItem.ToString().Trim();
                if (IsPavecomboBox.SelectedIndex == 0)//如果是路面,则输出相应的路面属性信息
                {
                    lines[10] = "公路类型:" + RoadClasscomboBox.SelectedItem.ToString().Trim();
                    lines[11] = "公路等级:" + RoadRankcomboBox.SelectedItem.ToString().Trim();
                    if (RoadConsdateTimePicker.Checked == true)//如果铺设日期可知则选中，输出具体日期
                    {
                        lines[12] = "铺设日期:" + RoadConsdateTimePicker.Text.ToString().Trim();
                        //lines[12] = "铺设日期:" + RoadConsdateTimePicker.Value.Year.ToString().Trim() + RoadConsdateTimePicker.Value.Month.ToString().Trim() + RoadConsdateTimePicker.Value.Day.ToString().Trim();
                        lines[13] = "使用年数:" + uselength.ToString().Trim();
                    }
                    else//如果铺设日期未知则不选中，不输出具体日期
                    {
                        lines[12] = "铺设日期:未知";
                        lines[13] = "使用年数:未知";
                    }
                    lines[14] = "健康状况:" + HealthConditioncomboBox.SelectedItem.ToString().Trim();
                    lines[15] = "病害类型:" + DistresscomboBox.SelectedItem.ToString().Trim();
                    lines[16] = "色卡号:" + ColorValuecombox.SelectedItem.ToString().Trim();
                }
                else//如果不是路面，则不输出任何路面属性信息
                {
                    lines[10] = "公路类型:";
                    lines[11] = "公路等级:";
                    lines[12] = "铺设日期:";
                    lines[13] = "使用年数:";
                    lines[14] = "健康状况:";
                    lines[15] = "病害类型:";
                    lines[16] = "色卡号:";
                }

                //位置信息
                if (GPScheckBox.Checked == true)//如果有GPS位置信息，则设置GPS信息
                {

                    lines[17] = "经度:" + longitudecomboBox.SelectedItem.ToString().Trim() + longitudenumericUp.Value.ToString().Trim();
                    lines[18] = "纬度:" + latitudecomboBox.SelectedItem.ToString().Trim() + latitudenumericUp.Value.ToString().Trim();
                    lines[19] = "高程:" + AltitudeNumericUp.Value.ToString().Trim();
                }
                else
                {
                    lines[17] = "经度:未知";
                    lines[18] = "纬度:未知";
                    lines[19] = "高程:未知";
                }

                //备注信息
                lines[21] = "备注:" + DescriptionrichTextBox.Text.ToString().Trim();

                lines[22] = "**********************光谱数据**********************";

                //输出TXT文件
                StreamWriter spec_file = new StreamWriter(savefilepathtextBox.Text.ToString().Trim(), false, Encoding.UTF8);
                foreach (string line in lines)//遍历存储元数据
                {
                    spec_file.WriteLine(line);//直接追加文件末尾，换行
                }
                foreach (string data_line in specdatarichTextBox.Lines)//遍历存储反射率数据
                {
                    spec_file.WriteLine(data_line);
                }
                spec_file.Close();//关闭文件

                MessageBox.Show("光谱文件创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }
        }

        //存储光谱文件
        private void Savespecbutton_Click(object sender, EventArgs e)
        {
            SaveFileDialog spec_save = new SaveFileDialog();
            spec_save.Filter = "文本文件|*.txt";
            if (spec_save.ShowDialog() == DialogResult.OK)
            {
                savefilepathtextBox.Text = spec_save.FileName;
            }
        }

        //取消
        private void cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        //路面健康状况对应不同的病害类型
        private void HealthConditioncomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (HealthConditioncomboBox.SelectedIndex)
            {
                case 0://选中未知
                    this.DistresscomboBox.Items.Clear();
                    string distressitems0 = "未知";
                    this.DistresscomboBox.Items.Add(distressitems0);
                    DistresscomboBox.SelectedIndex = 0;
                    break;
                case 1://选中老化
                    this.DistresscomboBox.Items.Clear();
                    string[] distressitems1 = new string[] { "未知", "初期", "中期", "后期" };
                    foreach (string distressitem in distressitems1)
                    {
                        this.DistresscomboBox.Items.Add(distressitem);
                    }
                    DistresscomboBox.SelectedIndex = 0;
                    break;
                case 2://选中病害，又分为沥青道路病害和水泥道路病害
                    this.DistresscomboBox.Items.Clear();
                    if (materialclasscomboBox.SelectedIndex == 5)//如果类型是水泥路面
                    {
                        string[] distressitems20 = new string[] { "未知","斜向裂缝","交叉裂缝","横裂","纵裂","纹裂","网裂","角隅断裂","断裂板",
                        "沉陷","胀起","唧泥","错台","接缝碎裂","拱起","纵缝张开","填缝料损坏","起皮","磨损","露骨","坑洞","活性集料反应","修补损坏" };
                        foreach (string distressitem in distressitems20)
                        {
                            this.DistresscomboBox.Items.Add(distressitem);
                        }
                    }
                    else//如果不是水泥路面
                    {
                        string[] distressitems21 = new string[] { "未知","龟裂","不规则裂缝","横裂","纵裂","坑槽",
                        "麻面","脱皮","啃边","搓板","松散","沉陷","车辙","波浪","拥包","泛油","磨光","冻胀","翻浆","修补损坏" };
                        foreach (string distressitem in distressitems21)
                        {
                            this.DistresscomboBox.Items.Add(distressitem);
                        }

                    }
                    DistresscomboBox.SelectedIndex = 0;
                    break;
                default://其他情况
                    break;
            }
        }
    }



}
