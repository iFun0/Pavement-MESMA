using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using OSGeo.GDAL;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Drawing2D;

namespace PavementDetection
{
    /// <summary>
    /// 文件名:MESMAForm.cs
    /// 功能描述:该模块可用于遥感影像的多端元混合像元分解，基于该方法也可以对路面的老化状况进行监测和评估，输出评估报告
    /// 创建人:潘一凡
    /// 单位:北京大学遥感所
    /// 联系方式:yfpan@pku.edu.cn
    /// 创建日期：2016-07-15
    /// 修改人：
    /// 修改日期：
    /// 修改备注：
    /// 版本：1.0
    /// </summary>

    public partial class MESMAForm : Form
    {
        /*---------------------------定义全局变量Begin---------------------------------*/
        int NumofBands;//定义影像的波段数
        double PixelSizeX;//定义影像的空间分辨率X
        double PixelSizeY;//定义影像的空间分辨率Y
        int NumofModel;//定义端元模型的数量（根据选择不同光谱库中选择端元的数量计算）
        int TypeofModel;//端元模型的类型，即二端元模型，三端元模型等
        DenseMatrix EMLibrary1;//定义第一个端元库
        DenseMatrix EMLibrary2;//定义第二个端元库
        DenseMatrix EMLibrary3;//定义第三个端元库       
        int NumofFile;//定义临时变量存储每个端元库的文件数，即光谱数
        int NumofEMLibrary1;//第1个端元光谱库端元的个数
        int NumofEMLibrary2;//第2个端元光谱库端元的个数
        int NumofEMLibrary3;//第3个端元光谱库端元的个数

        private DataTable ModelInfo_dt = new DataTable();//创建全局DataTable用于存储模型信息
        int[] MESMA_PAModelNumSet = new int[1];//创建老化初期模型号数组
        int[] MESMA_MAModelNumSet = new int[1];//创建老化中期模型号数组
        int[] MESMA_HAModelNumSet = new int[1];//创建老化后期模型号数组
        int MESMA_PavementFracBands;//路面丰度影像波段号

        //PDF统计报告所需变量
        string PDF_RoadName;//道路名称
        string PDF_RoadType;//道路类型
        string PDF_RoadLevel;//道路等级
        string PDF_PaveType;//路面材质类型
        string PDF_RoadPaveDate;//铺设日期
        string PDF_YanghuDate;//上一次路面养护日期
        string PDF_RoadUseLength;//使用年数             
        double Round_StaofAreaofPA;//老化初期统计面积
        double Round_StaofAreaofMA;//老化中期统计面积
        double Round_StaofAreaofHA;//老化后期统计面积
        double Round_StaofPercentofPA;//老化初期统计面积比例
        double Round_StaofPercentofMA;//老化中期统计面积比例
        double Round_StaofPercentofHA;//老化后期统计面积比例

        //定义结构体返回每个SMA运算的结果
        public struct SMAResult
        {
            public double[] frac;//端元丰度值
            public double shade;//阴影丰度值
            public double rmse;//RMSE
        }

        /*---------------------------定义全局变量End---------------------------------*/
        #region 界面设计代码

        //窗体类构造函数
        public MESMAForm()
        {
            InitializeComponent();
        }

        //窗体Load事件
        private void MESMAForm_Load(object sender, EventArgs e)
        {
            //初始化时，各按钮的状态
            //端元光谱库2不启用
            EMLibrary2listView.Enabled = false;
            EMLib2Addbt.Enabled = false;
            EMLib2Deletebt.Enabled = false;
            //端元光谱库3不启用
            EMLibrary3listView.Enabled = false;
            EMLib3Addbt.Enabled = false;
            EMLib3Deletebt.Enabled = false;
            //非阴影端元丰度最大值和最小值约束不启用
            FracMintrackBar.Enabled = false;
            FracMintextBox.Enabled = false;
            FracMaxtrackBar.Enabled = false;
            FracMaxtextBox.Enabled = false;
            //阴影端元丰度最大值和最小值约束不启用
            ShadeMaxtrackBar.Enabled = false;
            ShadeMaxtextBox.Enabled = false;
            //RMSE约束不启用
            RMSEMaxNumericUp.Enabled = false;
            //Residual约束不启用
            ResiMaxNumericUp.Enabled = false;
            ResiMaxtrackBar.Enabled = false;
            ResiStaBandtextBox.Enabled = false;
            //路面丰度、RMSE影像和模型影像默认不输出
            SavePaveFractextBox.Enabled = false;
            SavePaveFracbt.Enabled = false;
            SaveModeltextBox.Enabled = false;
            SaveModelbt.Enabled = false;
            SaveRMSEtextBox.Enabled = false;
            SaveRMSEbt.Enabled = false;

            //统计报告默认不输出
            StaReportSetBt.Enabled = false;
            StaReportSavebt.Enabled = false;
            SaveStaReporttextBox.Enabled = false;

            //初始化ModelInfoDataTable并与统计报告界面中的ModeldataGridView连接，增加所需列名
            ModelInfo_dt.Columns.Add("模型号", typeof(string));
            ModelInfo_dt.Columns.Add("端元1", typeof(string));
            ModelInfo_dt.Columns.Add("端元2", typeof(string));
            ModelInfo_dt.Columns.Add("端元3", typeof(string));
            ModelInfo_dt.Columns.Add("端元4", typeof(string));
        }

        //端元光谱库2的Check按钮事件
        private void EMLibrary2checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EMLibrary2checkBox.Checked == true)
            {
                EMLibrary2listView.Enabled = true;
                EMLib2Addbt.Enabled = true;
                EMLib2Deletebt.Enabled = true;
            }
            else
            {
                if (EMLibrary2listView.Items.Count != 0)//如果list中已有项目，则执行清除命令并还原矩阵
                {
                    EMLibrary2listView.Items.Clear();//清除所有项
                    EMLibrary2 = new DenseMatrix(NumofBands, 1);//将端元光谱库2矩阵还原为初始状态
                    TypeofModel = 2;//更新模型类型
                    NumofModel = NumofEMLibrary1;//更新模型数量                                    
                    TypeofModeltextBox.Text = TypeofModel.ToString();
                    NumofModeltxtBox.Text = NumofModel.ToString();
                }
                EMLibrary2listView.Enabled = false;
                EMLib2Addbt.Enabled = false;
                EMLib2Deletebt.Enabled = false;
                EMLibrary3checkBox.Checked = false;//同时停用端元光谱库3
            }

        }

        //端元光谱库3的Check按钮事件
        private void EMLibrary3checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EMLibrary3checkBox.Checked == true)
            {
                EMLibrary3listView.Enabled = true;
                EMLib3Addbt.Enabled = true;
                EMLib3Deletebt.Enabled = true;
            }
            else
            {
                if (EMLibrary3listView.Items.Count != 0)//如果list中已有项目，则执行清除命令并还原矩阵
                {
                    EMLibrary3listView.Items.Clear();//清除所有项
                    EMLibrary3 = new DenseMatrix(NumofBands, 1);//将端元光谱库2矩阵还原为初始状态
                    TypeofModel = 3;//更新模型类型
                    NumofModel = NumofEMLibrary1 * NumofEMLibrary2;//更新模型数量
                    TypeofModeltextBox.Text = TypeofModel.ToString();
                    NumofModeltxtBox.Text = NumofModel.ToString();
                }
                EMLibrary3listView.Enabled = false;
                EMLib3Addbt.Enabled = false;
                EMLib3Deletebt.Enabled = false;
            }
        }

        //最小丰度值约束是否启用事件
        private void FracMincheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FracMincheckBox.Checked == true)
            {
                FracMintrackBar.Enabled = true;
                FracMintextBox.Enabled = true;
            }
            else
            {
                FracMintrackBar.Enabled = false;
                FracMintextBox.Enabled = false;
            }
        }

        //最大丰度值约束是否启用事件
        private void FracMaxcheckBox_CheckedChanged(object sender, EventArgs e)
        {

            if (FracMaxcheckBox.Checked == true)
            {
                FracMaxtrackBar.Enabled = true;
                FracMaxtextBox.Enabled = true;
            }
            else
            {
                FracMaxtrackBar.Enabled = false;
                FracMaxtextBox.Enabled = false;
            }
        }

        //最大阴影丰度值约束是否启用事件
        private void ShadeMaxcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShadeMaxcheckBox.Checked == true)
            {
                ShadeMaxtrackBar.Enabled = true;
                ShadeMaxtextBox.Enabled = true;
            }
            else
            {
                ShadeMaxtrackBar.Enabled = false;
                ShadeMaxtextBox.Enabled = false;
            }

        }

        //最大RMSE约束是否启用事件
        private void RMSEMaxcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RMSEMaxcheckBox.Checked == true)
                RMSEMaxNumericUp.Enabled = true;
            else
                RMSEMaxNumericUp.Enabled = false;
        }

        //最大残差约束是否启用事件
        private void ResiMaxcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ResiMaxcheckBox.Checked == true)
            {
                ResiMaxNumericUp.Enabled = true;
                ResiMaxtrackBar.Enabled = true;
                ResiStaBandtextBox.Enabled = true;
            }
            else
            {
                ResiMaxNumericUp.Enabled = false;
                ResiMaxtrackBar.Enabled = false;
                ResiStaBandtextBox.Enabled = false;
            }

        }

        //最小丰度值滑动条滚动取值事件
        private void FracMintrackBar_Scroll(object sender, EventArgs e)
        {
            this.FracMintextBox.Text = (this.FracMintrackBar.Value / 100.0).ToString();
        }

        //最大丰度值滑动条滚动取值事件
        private void FracMaxtrackBar_Scroll(object sender, EventArgs e)
        {
            this.FracMaxtextBox.Text = (this.FracMaxtrackBar.Value / 100.0).ToString();
        }

        //最大阴影丰度值滑动条滚动取值事件
        private void ShadeMaxtrackBar_Scroll(object sender, EventArgs e)
        {
            this.ShadeMaxtextBox.Text = (this.ShadeMaxtrackBar.Value / 100.0).ToString();
        }

        //残差约束连续统计波段数
        private void ResiMaxtrackBar_Scroll(object sender, EventArgs e)
        {
            this.ResiStaBandtextBox.Text = this.ResiMaxtrackBar.Value.ToString();
        }

        //是否输出路面丰度影像
        private void SavePaveFraccheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SavePaveFraccheckBox.Checked == true)
            {
                StaReportSetBt.Enabled = true;
                SavePaveFracbt.Enabled = true;
                SavePaveFractextBox.Enabled = true;
            }
            else
            {
                StaReportSetBt.Enabled = false;
                SavePaveFractextBox.Clear();
                SavePaveFracbt.Enabled = false;
                SavePaveFractextBox.Enabled = false;

            }

        }

        //是否输出RMSE影像
        private void SaveRMSEcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SaveRMSEcheckBox.Checked == true)
            {
                SaveRMSEtextBox.Enabled = true;
                SaveRMSEbt.Enabled = true;
            }
            else
            {
                SaveRMSEtextBox.Clear();
                SaveRMSEtextBox.Enabled = false;
                SaveRMSEbt.Enabled = false;
            }

        }

        //是否输出模型影像
        private void SaveModelcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SaveModelcheckBox.Checked == true)
            {
                SaveModeltextBox.Enabled = true;
                SaveModelbt.Enabled = true;
            }
            else
            {
                SaveModeltextBox.Clear();
                SaveModeltextBox.Enabled = false;
                SaveModelbt.Enabled = false;
            }
        }

        //统计报告是否输出
        private void StaReportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StaReportCheckBox.Checked == true)
            {
                StaReportSavebt.Enabled = true;
                SaveStaReporttextBox.Enabled = true;
                StaReportSetBt.Enabled = true;
                //SaveModelcheckBox.Checked = true;//如果需要输出统计报告，必须要输出模型影像

            }
            else
            {
                SaveStaReporttextBox.Clear();
                StaReportSavebt.Enabled = false;
                SaveStaReporttextBox.Enabled = false;
                StaReportSetBt.Enabled = false;
            }

        }

        //取消按钮
        private void CancelBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #endregion


        #region 功能函数
        /*---------------------------功能函数---------------------------------*/

        //打开影像文件
        private void OpenImagebutton_Click(object sender, EventArgs e)
        {
            OpenImage(ImagePathtextBox);
        }


        //向端元光谱库1添加文件
        private void EMLib1Addbt_Click(object sender, EventArgs e)
        {
            if (ImagePathtextBox.Text.ToString() != "")
            {
                EMLibrary1 = AddEMSpec(EMLibrary1, EMLibrary1listView);
                if (EMLibrary1 != null)
                {
                    NumofEMLibrary1 = EMLibrary1.ColumnCount;
                    NumofModel = NumofEMLibrary1;//模型数量
                    NumofModeltxtBox.Text = NumofModel.ToString();
                    TypeofModel = 2;//模型种类
                    TypeofModeltextBox.Text = TypeofModel.ToString();
                    //MessageBox.Show(EMLibrary1.ToString());
                }
            }
            else
                MessageBox.Show("请先选择遥感影像！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        //端元光谱库1删除文件
        private void EMLib1Deletebt_Click(object sender, EventArgs e)
        {
            EMLibrary1 = DeleEMSpec(EMLibrary1, EMLibrary1listView);
            if (EMLibrary1 != null)
            {
                NumofEMLibrary1 = EMLibrary1.ColumnCount;
                NumofModel = NumofEMLibrary1;//模型数量
                NumofModeltxtBox.Text = NumofModel.ToString();
                TypeofModel = 2;//模型种类
                TypeofModeltextBox.Text = TypeofModel.ToString();
            }
        }

        //向端元光谱库2添加文件
        private void EMLib2Addbt_Click(object sender, EventArgs e)
        {

            if (EMLibrary1listView.Items.Count == 0)
            {
                MessageBox.Show("请先添加端元光谱库（一）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;//如果端元库1中没有光谱，直接向端元光谱库2添加光谱则退出
            }

            EMLibrary2 = AddEMSpec(EMLibrary2, EMLibrary2listView);
            if (EMLibrary2 != null)
            {
                NumofEMLibrary2 = EMLibrary2.ColumnCount;
                NumofModel = NumofEMLibrary1 * NumofEMLibrary2;//模型数量
                NumofModeltxtBox.Text = NumofModel.ToString();
                TypeofModel = 3;//模型种类
                TypeofModeltextBox.Text = TypeofModel.ToString();
                // MessageBox.Show(EMLibrary2.ToString());
            }
        }

        //端元光谱库2删除文件
        private void EMLib2Deletebt_Click(object sender, EventArgs e)
        {
            EMLibrary2 = DeleEMSpec(EMLibrary2, EMLibrary2listView);
            if (EMLibrary2 != null)
            {
                NumofEMLibrary2 = EMLibrary2.ColumnCount;
                NumofModel = NumofEMLibrary1 * NumofEMLibrary2; ;//模型数量
                NumofModeltxtBox.Text = NumofModel.ToString();
                TypeofModel = 3;//模型种类
                TypeofModeltextBox.Text = TypeofModel.ToString();
                //MessageBox.Show(EMLibrary2.ToString());
            }
        }

        //向端元光谱库3添加文件
        private void EMLib3Addbt_Click(object sender, EventArgs e)
        {

            if (EMLibrary1listView.Items.Count == 0)
            {
                MessageBox.Show("请先添加端元光谱库（一）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;//如果端元库1中没有光谱，直接向端元光谱库3添加光谱则退出
            }
            else if (EMLibrary2listView.Items.Count == 0)
            {
                MessageBox.Show("请先添加端元光谱库（二）!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;//如果端元库2中没有光谱，直接向端元光谱库3添加光谱则退出
            }

            EMLibrary3 = AddEMSpec(EMLibrary3, EMLibrary3listView);
            if (EMLibrary3 != null)
            {
                NumofEMLibrary3 = EMLibrary3.ColumnCount;
                NumofModel = NumofEMLibrary1 * NumofEMLibrary2 * NumofEMLibrary3;//模型数量
                NumofModeltxtBox.Text = NumofModel.ToString();
                TypeofModel = 4;//模型种类
                TypeofModeltextBox.Text = TypeofModel.ToString();
            }
        }

        //端元光谱库3删除文件
        private void EMLib3Deletebt_Click(object sender, EventArgs e)
        {
            EMLibrary3 = DeleEMSpec(EMLibrary3, EMLibrary3listView);
            if (EMLibrary3 != null)
            {
                NumofEMLibrary3 = EMLibrary3.ColumnCount;
                NumofModel = NumofEMLibrary1 * NumofEMLibrary2 * NumofEMLibrary3;//模型数量
                NumofModeltxtBox.Text = NumofModel.ToString();
                TypeofModel = 4;//模型种类
                TypeofModeltextBox.Text = TypeofModel.ToString();
            }
        }

        //选择地物丰度影像保存文件路径
        private void SaveFracfilebt_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "GeoTIFF(*.tif)|*.tif";
            if (save.ShowDialog() == DialogResult.OK)
                SaveFracPathtextBox.Text = save.FileName.ToString();
        }

        //选择路面丰度影像保存文件路径
        private void SavePaveFracbt_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "GeoTIFF(*.tif)|*.tif";
            if (save.ShowDialog() == DialogResult.OK)
                SavePaveFractextBox.Text = save.FileName.ToString();
        }

        //选择RMSE影像保存文件路径
        private void SaveRMSEbt_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "GeoTIFF(*.tif)|*.tif";
            if (save.ShowDialog() == DialogResult.OK)
                SaveRMSEtextBox.Text = save.FileName.ToString();

        }

        //选择模型影像保存文件路径
        private void SaveModelbt_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "GeoTIFF(*.tif)|*.tif";
            if (save.ShowDialog() == DialogResult.OK)
                SaveModeltextBox.Text = save.FileName.ToString();

        }

        //选择统计报告保存文件路径
        private void StaReportSavebt_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF文件(*.PDF)|*.PDF";
            //save.Filter = "GeoTIFF(*.tif)|*.tif";
            if (save.ShowDialog() == DialogResult.OK)
                SaveStaReporttextBox.Text = save.FileName.ToString();

        }

        //打开统计报告参数设置对话框
        private void StaReportBt_Click(object sender, EventArgs e)
        {
            //首先检查界面设置是否错误
            for (int iError = 0; iError < 9; iError++)
            {
                if (ErrorCheck(iError) == -1)
                    return;
            }

            ModelInfoTableReport();//将模型配对信息存储到临时表中用于显示
            //打开统计报告参数设置按钮
            AgingReport reportForm = new AgingReport(this, PixelSizeX, PixelSizeY, TypeofModel, ModelInfo_dt);
            reportForm.ShowDialog();
        }

        //运行MESMA
        private void MESMARunButton_Click(object sender, EventArgs e)
        {
            try
            {
                //首先检查界面设置是否错误
                for (int iError = 0; iError < 10; iError++)
                {
                    if (ErrorCheck(iError) == -1)
                        return;
                }

                //禁用运行和取消按钮
                this.MESMARunButton.Enabled = false;
                this.CancelBt.Enabled = false;

                //设置进度条状态
                ProgressLabel.Text = "正在处理......";
                ProgressLabel.Refresh();

                //获取影像文件路径
                string imagefilePath = ImagePathtextBox.Text.ToString();

                //GDAL库初始化
                OSGeo.GDAL.Gdal.AllRegister();//注册所有的格式驱动
                OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");//支持中文路径和名称
                OSGeo.GDAL.Gdal.SetConfigOption("GDAL_DATA", Application.StartupPath.ToString() + @"\GDAL_Compile_X64\data");//读取配置文件

                //打开影像文件
                OSGeo.GDAL.Dataset imagefile = Gdal.Open(imagefilePath, Access.GA_ReadOnly);
                if (imagefile == null)
                {
                    MessageBox.Show("影像数据损坏，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //获取影像的基本信息
                int imageWidth = imagefile.RasterXSize;//影像列数
                int imageHeight = imagefile.RasterYSize;//影像行数
                NumofBands = imagefile.RasterCount;//影像波段数
                string im_description = imagefile.GetDescription();//影像描述信息
                string[] im_metadata = imagefile.GetMetadata("");//影像元数据
                string im_projection = imagefile.GetProjection();//影像投影信息
                string im_projectionref = imagefile.GetProjectionRef();//影像投影信息

                GCP[] GCPs = imagefile.GetGCPs();//影像地面控制点
                double[] adfGeoTransform = new double[6];//影像仿射变换信息
                imagefile.GetGeoTransform(adfGeoTransform);//影像仿射变换信息是一个6维数组

                //进度条设置
                this.progressBar.Minimum = 0;
                this.progressBar.Maximum = imageWidth * imageHeight;

                //创建将要存储的数组或矩阵变量
                double[] PixelValue = new double[NumofBands];//存储每个像元的光谱值
                double[] SinglBandValue = new double[1];//临时存储每个像元的光谱值

                DenseMatrix output_frac = new DenseMatrix(TypeofModel - 1, imageWidth * imageHeight);//创建非阴影端元丰度值数组
                DenseVector output_shade = new DenseVector(imageWidth * imageHeight);//创建阴影端元丰度值数组

                double[] output_rmse = new double[imageWidth * imageHeight]; //创建RMSE数组
                double[] output_model = new double[imageWidth * imageHeight]; //创建模型影像数组

                double[] output_PA = new double[imageWidth * imageHeight];//创建老化初期路面影像数组
                double[] output_MA = new double[imageWidth * imageHeight];//创建老化中期路面影像数组
                double[] output_HA = new double[imageWidth * imageHeight];//创建老化后期路面影像数组
                //DenseMatrix output_Aging = new DenseMatrix(3, imageWidth * imageHeight);//第一行代表老化初期，第二行代表老化中期，第三行代表老化后期

                //遍历图像，获取每个像元的光谱值用于模型分解
                for (int i = 0; i < imageHeight; i++)
                {
                    for (int j = 0; j < imageWidth; j++)
                    {
                        //获取像元的序号
                        int PixelNum = i * imageWidth + j;

                        //获取每个像元的光谱向量
                        for (int iBand = 1; iBand <= NumofBands; iBand++)
                        {
                            Band band = imagefile.GetRasterBand(iBand);//获取第i个波段对象
                            band.ReadRaster(j, i, 1, 1, SinglBandValue, 1, 1, 0, 0);//读取单个像元在第iBand波段的值
                            PixelValue[iBand - 1] = SinglBandValue[0];
                        }
                        var Vector_Pixel = DenseVector.OfArray(PixelValue);

                        double best_rmse = 10000.0;//初始化RMSE值
                        SMAResult sma_result_mesma = new SMAResult();//实例化SMA对象
                        DenseMatrix Matrix_EM = new DenseMatrix(NumofBands, NumofModel);//定义端元矩阵                        

                        //开始遍历所有端元模型
                        switch (TypeofModel)
                        {
                            case 2://二端元模型
                                for (int iNumofEMLibrary1 = 0; iNumofEMLibrary1 < NumofEMLibrary1; iNumofEMLibrary1++)
                                {
                                    //创建端元矩阵
                                    Matrix_EM = DenseMatrix.OfColumnVectors(EMLibrary1.Column(iNumofEMLibrary1));

                                    //计算简单线性分解模型SMA
                                    sma_result_mesma = SMA_Frac_Cacul(Matrix_EM, Vector_Pixel);

                                    //如果现模型RMSE小于上一模型RMSE，则继续检查约束条件
                                    if (sma_result_mesma.rmse <= best_rmse)
                                    {
                                        //将模型RMSE赋值给临时变量RMSE，用于下个模型比较
                                        best_rmse = sma_result_mesma.rmse;
                                        //取对应的frac和shade值与选定的限制条件做比较
                                        double ValidCheck = ConstrainedOutput(sma_result_mesma);
                                        //如果符合约束条件（ValidCheck = 5）则输出
                                        if (ValidCheck == 5)
                                        {
                                            output_frac[0, i * imageWidth + j] = sma_result_mesma.frac[0];
                                            output_shade[i * imageWidth + j] = sma_result_mesma.shade;
                                            if (SaveRMSEcheckBox.Checked == true)
                                            {
                                                output_rmse[i * imageWidth + j] = sma_result_mesma.rmse;
                                            }
                                            if (SaveModelcheckBox.Checked == true || SavePaveFraccheckBox.Checked == true || StaReportCheckBox.Checked == true)
                                            {
                                                output_model[i * imageWidth + j] = iNumofEMLibrary1 + 1;
                                            }
                                        }
                                    }

                                }
                                //如果要输出路面影像或统计报告
                                if (StaReportCheckBox.Checked == true || SavePaveFraccheckBox.Checked == true)
                                {
                                    int OutputModelNum2 = (int)output_model[i * imageWidth + j];
                                    double OutputAgingFrac2 = output_frac[MESMA_PavementFracBands, i * imageWidth + j];
                                    FracReport(OutputModelNum2, PixelNum, output_PA, output_MA, output_HA, OutputAgingFrac2);
                                }

                                break;

                            case 3://三端元模型
                                for (int iNumofEMLibrary1 = 0; iNumofEMLibrary1 < NumofEMLibrary1; iNumofEMLibrary1++)
                                {
                                    for (int iNumofEMLibrary2 = 0; iNumofEMLibrary2 < NumofEMLibrary2; iNumofEMLibrary2++)
                                    {
                                        //创建端元矩阵
                                        DenseMatrix Matrix_EM1 = DenseMatrix.OfColumnVectors(EMLibrary1.Column(iNumofEMLibrary1));
                                        DenseMatrix Matrix_EM2 = DenseMatrix.OfColumnVectors(EMLibrary2.Column(iNumofEMLibrary2));
                                        Matrix_EM = (DenseMatrix)Matrix_EM1.Append(Matrix_EM2);
                                        //计算简单线性分解模型SMA
                                        sma_result_mesma = SMA_Frac_Cacul(Matrix_EM, Vector_Pixel);
                                        //如果现模型RMSE小于上一模型RMSE，则继续检查约束条件
                                        if (sma_result_mesma.rmse <= best_rmse)
                                        {
                                            best_rmse = sma_result_mesma.rmse;
                                            //取对应的frac和shade值与选定的限制条件做比较
                                            double ValidCheck = ConstrainedOutput(sma_result_mesma);
                                            //如果符合约束条件（ValidCheck = 5）则输出
                                            if (ValidCheck == 5)
                                            {
                                                output_frac[0, i * imageWidth + j] = sma_result_mesma.frac[0];
                                                output_frac[1, i * imageWidth + j] = sma_result_mesma.frac[1];
                                                output_shade[i * imageWidth + j] = sma_result_mesma.shade;
                                                if (SaveRMSEcheckBox.Checked == true)
                                                {
                                                    output_rmse[i * imageWidth + j] = sma_result_mesma.rmse;
                                                }
                                                if (SaveModelcheckBox.Checked == true || SavePaveFraccheckBox.Checked == true || StaReportCheckBox.Checked == true)
                                                {
                                                    output_model[i * imageWidth + j] = iNumofEMLibrary1 * NumofEMLibrary2 + iNumofEMLibrary2 + 1;
                                                }
                                            }
                                        }
                                    }
                                }
                                //如果要输出路面影像或统计报告
                                if (StaReportCheckBox.Checked == true || SavePaveFraccheckBox.Checked == true)
                                {
                                    int OutputModelNum3 = (int)output_model[i * imageWidth + j];
                                    double OutputAgingFrac3 = output_frac[MESMA_PavementFracBands, i * imageWidth + j];
                                    FracReport(OutputModelNum3, PixelNum, output_PA, output_MA, output_HA, OutputAgingFrac3);
                                }

                                break;
                            case 4://四端元模型
                                for (int iNumofEMLibrary1 = 0; iNumofEMLibrary1 < NumofEMLibrary1; iNumofEMLibrary1++)
                                {
                                    for (int iNumofEMLibrary2 = 0; iNumofEMLibrary2 < NumofEMLibrary2; iNumofEMLibrary2++)
                                    {
                                        for (int iNumofEMLibrary3 = 0; iNumofEMLibrary3 < NumofEMLibrary3; iNumofEMLibrary3++)
                                        {
                                            //创建端元矩阵
                                            DenseMatrix Matrix_EM1 = DenseMatrix.OfColumnVectors(EMLibrary1.Column(iNumofEMLibrary1));
                                            DenseMatrix Matrix_EM2 = DenseMatrix.OfColumnVectors(EMLibrary2.Column(iNumofEMLibrary2));
                                            DenseMatrix Matrix_EM3 = DenseMatrix.OfColumnVectors(EMLibrary3.Column(iNumofEMLibrary3));
                                            DenseMatrix Matrix_EM12 = (DenseMatrix)Matrix_EM1.Append(Matrix_EM2);
                                            Matrix_EM = (DenseMatrix)Matrix_EM12.Append(Matrix_EM3);
                                            //计算简单线性分解模型SMA
                                            sma_result_mesma = SMA_Frac_Cacul(Matrix_EM, Vector_Pixel);
                                            //如果现模型RMSE小于上一模型RMSE，则继续检查约束条件
                                            if (sma_result_mesma.rmse <= best_rmse)
                                            {
                                                best_rmse = sma_result_mesma.rmse;
                                                //取对应的frac和shade值与选定的限制条件做比较
                                                double ValidCheck = ConstrainedOutput(sma_result_mesma);
                                                if (ValidCheck == 5)
                                                {
                                                    output_frac[0, i * imageWidth + j] = sma_result_mesma.frac[0];
                                                    output_frac[1, i * imageWidth + j] = sma_result_mesma.frac[1];
                                                    output_frac[2, i * imageWidth + j] = sma_result_mesma.frac[2];
                                                    output_shade[i * imageWidth + j] = sma_result_mesma.shade;
                                                    if (SaveRMSEcheckBox.Checked == true)
                                                    {
                                                        output_rmse[i * imageWidth + j] = sma_result_mesma.rmse;
                                                    }
                                                    if (SaveModelcheckBox.Checked == true || SavePaveFraccheckBox.Checked == true || StaReportCheckBox.Checked == true)
                                                    {
                                                        output_model[i * imageWidth + j] = iNumofEMLibrary1 * NumofEMLibrary2 * NumofEMLibrary3 + iNumofEMLibrary2 * NumofEMLibrary3 + iNumofEMLibrary3 + 1;

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //如果要输出路面影像或统计报告
                                if (StaReportCheckBox.Checked == true || SavePaveFraccheckBox.Checked == true)
                                {
                                    int OutputModelNum4 = (int)output_model[i * imageWidth + j];
                                    double OutputAgingFrac4 = output_frac[MESMA_PavementFracBands, i * imageWidth + j];
                                    FracReport(OutputModelNum4, PixelNum, output_PA, output_MA, output_HA, OutputAgingFrac4);
                                }
                                break;
                        }//所有端元模型循环完毕
                        //刷新进度条
                        //ProgressLabel.Text ="处理进度: "+ (Math.Round(((decimal)(i * imageWidth + j) / (imageWidth * imageHeight)), 2) * 100).ToString() + "%";                    
                        progressBar.Value = i * imageWidth + j + 1;
                    }//一个像元分解完毕
                }//所有像元遍历完毕

                /*-------------------------------输出地物丰度影像-------------------------------------*/
                //将阴影丰度和非阴影端元丰度整合到一个矩阵中
                Matrix<double> Combine_Frac = output_frac.Stack(output_shade.ToRowMatrix());
                //获取丰度影像的存储路径
                string saveFracfilepath = SaveFracPathtextBox.Text.ToString();
                //指定影像格式驱动
                Driver frac_drv = Gdal.GetDriverByName("GTiff");
                //创建块大小
                int bXSize = imageWidth;
                int bYSize = 1;
                //创建图像
                string[] options = new string[] { "BLOCKXSIZE=" + bXSize, "BLOCKYSIZE=" + bYSize };
                Dataset frac_image = frac_drv.Create(saveFracfilepath, imageWidth, imageHeight, TypeofModel, DataType.GDT_Float64, options);
                //写入原始影像投影信息
                frac_image.SetDescription(im_description);
                frac_image.SetGeoTransform(adfGeoTransform);
                frac_image.SetGCPs(GCPs, "");
                frac_image.SetProjection(im_projection);
                //更新图像
                for (int iNewBand = 0; iNewBand < TypeofModel; iNewBand++)
                {
                    Band iFrac = frac_image.GetRasterBand(iNewBand + 1);
                    double[] iEM_frac = Combine_Frac.Row(iNewBand).ToArray();
                    iFrac.WriteRaster(0, 0, imageWidth, imageHeight, iEM_frac, imageWidth, imageHeight, 0, 0);
                    iFrac.FlushCache();
                }
                frac_image.FlushCache();

                /*-------------------------------输出路面丰度影像-------------------------------------*/
                if (SavePaveFraccheckBox.Checked == true)
                {
                    //获取路面丰度影像的存储路径
                    string SavePaveFracFilePath = SavePaveFractextBox.Text.ToString();

                    //输出不同老化路面的快视图并添加到PDF文件中
                    DenseMatrix MatrixPA = DenseMatrix.OfRowArrays(output_PA);
                    DenseMatrix MatrixMA = DenseMatrix.OfRowArrays(output_MA);
                    DenseMatrix MatrixHA = DenseMatrix.OfRowArrays(output_HA);

                    //将不同老化阶段的丰度影像整合到一个矩阵中，按照初期为绿色，中期为蓝色，后期为红色
                    Matrix<double> Combine_PaveFrac = (MatrixHA.Stack(MatrixPA)).Stack(MatrixMA);

                    //指定影像格式驱动
                    Driver savepavefrac_drv = Gdal.GetDriverByName("GTiff");
                    //创建图像
                    Dataset savepavefrac_image = savepavefrac_drv.Create(SavePaveFracFilePath, imageWidth, imageHeight, 3, DataType.GDT_Float64, options);

                    //写入原始影像投影信息
                    savepavefrac_image.SetDescription(im_description);
                    savepavefrac_image.SetGeoTransform(adfGeoTransform);
                    savepavefrac_image.SetGCPs(GCPs, "");
                    savepavefrac_image.SetProjection(im_projection);

                    //更新图像
                    for (int iPaveFracBand = 0; iPaveFracBand < 3; iPaveFracBand++)
                    {
                        Band iPaveFrac = savepavefrac_image.GetRasterBand(iPaveFracBand + 1);
                        double[] iPaveFracValue = Combine_PaveFrac.Row(iPaveFracBand).ToArray();
                        iPaveFrac.WriteRaster(0, 0, imageWidth, imageHeight, iPaveFracValue, imageWidth, imageHeight, 0, 0);
                        iPaveFrac.FlushCache();
                    }
                    savepavefrac_image.FlushCache();
                }


                /*-------------------------------输出RMSE影像-------------------------------------*/
                if (SaveRMSEcheckBox.Checked == true)
                {
                    //获取RMSE影像的存储路径
                    string SaveRmsefilepath = SaveRMSEtextBox.Text.ToString();
                    //指定影像格式驱动
                    Driver rmse_drv = Gdal.GetDriverByName("GTiff");
                    //创建图像
                    Dataset rmse_image = rmse_drv.Create(SaveRmsefilepath, imageWidth, imageHeight, 1, DataType.GDT_Float64, options);
                    //写入原始影像投影信息
                    rmse_image.SetDescription(im_description);
                    rmse_image.SetGeoTransform(adfGeoTransform);
                    rmse_image.SetGCPs(GCPs, "");
                    rmse_image.SetProjection(im_projection);
                    //更新图像
                    Band iRMSE = rmse_image.GetRasterBand(1);
                    iRMSE.WriteRaster(0, 0, imageWidth, imageHeight, output_rmse, imageWidth, imageHeight, 0, 0);
                    iRMSE.FlushCache();
                    rmse_image.FlushCache();
                }

                /*-------------------------------输出模型影像-------------------------------------*/
                if (SaveModelcheckBox.Checked == true)
                {
                    //获取Model影像的存储路径
                    string SaveModelfilepath = SaveModeltextBox.Text.ToString();
                    //指定影像格式驱动
                    Driver model_drv = Gdal.GetDriverByName("GTiff");
                    //创建图像
                    Dataset model_image = model_drv.Create(SaveModelfilepath, imageWidth, imageHeight, 1, DataType.GDT_Float64, options);
                    //写入原始影像投影信息
                    model_image.SetDescription(im_description);
                    model_image.SetGeoTransform(adfGeoTransform);
                    model_image.SetGCPs(GCPs, "");
                    model_image.SetProjection(im_projection);
                    //更新图像
                    Band iModel = model_image.GetRasterBand(1);
                    iModel.WriteRaster(0, 0, imageWidth, imageHeight, output_model, imageWidth, imageHeight, 0, 0);
                    iModel.FlushCache();
                    model_image.FlushCache();
                }

                /*-------------------------------输出统计报告-------------------------------------*/
                if (StaReportCheckBox.Checked == true)
                {
                    //获取统计报告的存储路径
                    string SaveReportfilepath = SaveStaReporttextBox.Text.ToString();

                    //统计不同老化阶段的路面面积
                    double AreaofPixel = PixelSizeX * PixelSizeY;
                    double StaofAreaofPA = DenseVector.OfArray(output_PA).Sum() * AreaofPixel;//老化初期路面面积
                    Round_StaofAreaofPA = Math.Round(StaofAreaofPA, 4);
                    double StaofAreaofMA = DenseVector.OfArray(output_MA).Sum() * AreaofPixel;//老化中期路面面积
                    Round_StaofAreaofMA = Math.Round(StaofAreaofMA, 4);
                    double StaofAreaofHA = Math.Round(DenseVector.OfArray(output_HA).Sum() * AreaofPixel, 4);//老化后期路面面积
                    Round_StaofAreaofHA = Math.Round(StaofAreaofHA, 4);
                    double StaofSumArea = StaofAreaofPA + StaofAreaofMA + StaofAreaofHA;//路面总面积
                    double StaofPercentofPA = StaofAreaofPA / StaofSumArea;//老化初期路面面积比例
                    Round_StaofPercentofPA = Math.Round(StaofPercentofPA, 4) * 100.0;
                    double StaofPercentofMA = StaofAreaofMA / StaofSumArea;//老化中期路面面积比例
                    Round_StaofPercentofMA = Math.Round(StaofPercentofMA, 4) * 100.0;
                    double StaofPercentofHA = StaofAreaofHA / StaofSumArea;//老化后期路面面积比例
                    Round_StaofPercentofHA = Math.Round(StaofPercentofHA, 4) * 100.0;

                    //--------------------------绘画条形统计图-------------------------//
                    string AgingFracBarChartPath = CreateBarChart(Round_StaofPercentofPA, Round_StaofPercentofMA, Round_StaofPercentofHA);

                    //--------------------------根据阈值输出JPEG分类图像-------------------------//

                    //设定阈值将丰度进行硬分类
                    for (int iPixel = 0; iPixel < imageWidth * imageHeight; iPixel++)//
                    {
                        if (output_PA[iPixel] != 0.0)
                            output_PA[iPixel] = 255.0;
                        if (output_MA[iPixel] != 0.0)
                            output_MA[iPixel] = 255.0;
                        if (output_HA[iPixel] != 0.0)
                            output_HA[iPixel] = 255.0;
                    }

                    //输出不同老化路面的快视图并添加到PDF文件中
                    DenseMatrix MatrixPA = DenseMatrix.OfRowArrays(output_PA);
                    DenseMatrix MatrixMA = DenseMatrix.OfRowArrays(output_MA);
                    DenseMatrix MatrixHA = DenseMatrix.OfRowArrays(output_HA);

                    //将不同老化阶段的丰度影像整合到一个矩阵中
                    Matrix<double> Combine_AgingFrac = (MatrixHA.Stack(MatrixPA)).Stack(MatrixMA);

                    //指定影像格式驱动
                    Driver SavePaveFracClass_drv = Gdal.GetDriverByName("MEM");
                    //创建图像
                    Dataset SavePaveFracClass_image = SavePaveFracClass_drv.Create("", imageWidth, imageHeight, 3, DataType.GDT_CInt32, options);
                    //写入原始影像投影信息
                    //SavePaveFracClass_image.SetDescription(im_description);
                    //SavePaveFracClass_image.SetGeoTransform(adfGeoTransform);
                    //SavePaveFracClass_image.SetGCPs(GCPs, "");
                    //SavePaveFracClass_image.SetProjection(im_projection);
                    //更新图像
                    for (int iPaveFracClassBand = 0; iPaveFracClassBand < 3; iPaveFracClassBand++)
                    {
                        Band iPaveFracClass = SavePaveFracClass_image.GetRasterBand(iPaveFracClassBand + 1);
                        double[] iPaveFracClassValue = Combine_AgingFrac.Row(iPaveFracClassBand).ToArray();
                        iPaveFracClass.WriteRaster(0, 0, imageWidth, imageHeight, iPaveFracClassValue, imageWidth, imageHeight, 0, 0);
                        iPaveFracClass.FlushCache();
                    }
                    SavePaveFracClass_image.FlushCache();

                    //创建JPEG图像
                    string AgingFracImageFilePath = Path.GetDirectoryName(SaveReportfilepath) + "\\" + Path.GetFileNameWithoutExtension(SaveReportfilepath) + ".jpg";
                    Driver ImagetoJPG = Gdal.GetDriverByName("JPEG");
                    string[] JPGoptions = new string[] { "QUALITY=100" };
                    ImagetoJPG.CreateCopy(AgingFracImageFilePath, SavePaveFracClass_image, 0, JPGoptions, null, null); //tif转换成jpg          

                    //调用函数生成统计报告
                    StaReportPDFSetting();

                    //处理完成后自动打开PDF统计报告
                    System.Diagnostics.Process.Start(SaveReportfilepath);

                }

                ProgressLabel.Text = "处理完成";

                //启用运行和取消按钮
                this.MESMARunButton.Enabled = true;
                this.CancelBt.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }
        }

        #endregion


        #region 公共函数

        /*---------------------------公共函数---------------------------------*/


        //打开影像选择对话框
        private void OpenImage(TextBox var_tbox)
        {
            try
            {
                OpenFileDialog openimagefile = new OpenFileDialog();
                openimagefile.Title = "打开遥感影像";
                openimagefile.Filter = "GeoTiff(*.tif)|*.tif";
                if (openimagefile.ShowDialog() == DialogResult.OK)
                {
                    var_tbox.Text = openimagefile.FileName;
                    //GDAL库初始化
                    OSGeo.GDAL.Gdal.AllRegister();//注册所有的格式驱动
                    OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");//支持中文路径和名称
                    //打开影像文件
                    OSGeo.GDAL.Dataset imagefile = Gdal.Open(var_tbox.Text.ToString(), Access.GA_ReadOnly);
                    //获取影像信息
                    NumofBands = imagefile.RasterCount;
                    double[] OpenImageGeoTransform = new double[6];
                    imagefile.GetGeoTransform(OpenImageGeoTransform);
                    PixelSizeX = OpenImageGeoTransform[1];
                    PixelSizeY = Math.Abs(OpenImageGeoTransform[5]);
                    imagefile.Dispose();

                    //残差约束获取波段信息                
                    ResiStaBandtextBox.Text = NumofBands.ToString();
                    ResiMaxtrackBar.Maximum = NumofBands;
                    ResiMaxtrackBar.Value = NumofBands;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }

        }

        //添加标准光谱库文件
        private DenseMatrix AddEMSpec(DenseMatrix SpecLibrary, ListView tmp_list)
        {
            try
            {
                OpenFileDialog AddSpecfile = new OpenFileDialog();
                AddSpecfile.Title = "添加端元光谱";
                AddSpecfile.Filter = "标准光谱文件(*.txt)|*.txt";
                AddSpecfile.Multiselect = true;
                if (AddSpecfile.ShowDialog() == DialogResult.OK)
                {
                    //统计选中文件个数
                    NumofFile = AddSpecfile.FileNames.Length;
                    //SpecLibrary = new DenseMatrix(NumofBands, NumofFile);//创建端元光谱库矩阵

                    int iColumn = 0;
                    //如果list中已有光谱文件，则对光谱库矩阵进行Append操作，否则进行Set操作
                    if (tmp_list.Items.Count == 0)
                    {
                        SpecLibrary = new DenseMatrix(NumofBands, 1);
                        foreach (string Singlefile_Path in AddSpecfile.FileNames)//遍历每个光谱库文件
                        {
                            //读取标准光谱文件的反射率数据并添加到光谱库矩阵中   
                            DenseVector one_lib = ReadSpecFile(Singlefile_Path);
                            if (one_lib == null)
                                return null;//如果输入的光谱文件不是标准的光谱文件则直接退出
                            if (iColumn == 0)
                            {
                                SpecLibrary.SetColumn(iColumn, one_lib);
                            }
                            else
                                SpecLibrary = (DenseMatrix)SpecLibrary.Append(one_lib.ToColumnMatrix());
                            iColumn++;
                            //将选中文件全路径添加到View中
                            tmp_list.BeginUpdate();
                            ListViewItem lvi = new ListViewItem();
                            lvi.SubItems.Add(Singlefile_Path);
                            tmp_list.Items.Add(lvi);
                            //--------------测试是否添加索引号Begin--------------//
                            lvi.Text = (lvi.Index + 1).ToString();
                            //--------------测试是否添加索引号End--------------//
                            tmp_list.EndUpdate();
                        }
                    }
                    else
                    {
                        foreach (string Singlefile_Path in AddSpecfile.FileNames)//遍历每个光谱库文件
                        {
                            //读取标准光谱文件的反射率数据并添加到光谱库矩阵中   
                            DenseVector one_lib = ReadSpecFile(Singlefile_Path);
                            if (one_lib == null)
                                return null;//如果输入的光谱文件不是标准的光谱文件则直接退出
                            SpecLibrary = (DenseMatrix)SpecLibrary.Append(one_lib.ToColumnMatrix());
                            //将选中文件全路径添加到View中
                            tmp_list.BeginUpdate();
                            ListViewItem lvi = new ListViewItem();
                            lvi.SubItems.Add(Singlefile_Path);
                            tmp_list.Items.Add(lvi);
                            //--------------测试是否添加索引号Begin--------------//
                            lvi.Text = (lvi.Index + 1).ToString();
                            //--------------测试是否添加索引号End--------------//
                            tmp_list.EndUpdate();
                        }
                    }
                    return SpecLibrary;
                }
                else if (tmp_list.Items.Count != 0)
                {
                    return SpecLibrary;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return null;
            }

        }

        //删除标准光谱库文件
        private DenseMatrix DeleEMSpec(DenseMatrix SpecLibrary, ListView tmp_list)
        {
            try
            {
                if (tmp_list.Items.Count != 0)
                {
                    if (tmp_list.SelectedItems.Count != 0)
                    {
                        foreach (ListViewItem lvi in tmp_list.SelectedItems)  //选中项遍历  
                        {
                            if (tmp_list.Items.Count != 1)
                            {
                                SpecLibrary = (DenseMatrix)SpecLibrary.RemoveColumn(lvi.Index);
                            }
                            else
                            {
                                SpecLibrary = new DenseMatrix(NumofBands, 1);//如果只剩下一个光谱，则将公用光谱库设置为初始状态，因为矩阵不能为空
                            }
                            tmp_list.Items.RemoveAt(lvi.Index); // 按索引移除                        
                        }
                        //--------------测试是否添加索引号Begin--------------//
                        foreach (ListViewItem lvi_update in tmp_list.Items)//遍历所有项，更新索引值
                        {
                            lvi_update.Text = (lvi_update.Index + 1).ToString();
                        }
                        //--------------测试是否添加索引号End--------------//
                        return SpecLibrary;
                    }
                    else
                    {
                        MessageBox.Show("请选择至少一条光谱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("该端元光谱库为空，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return null;
            }
        }

        //读取标准TXT光谱文件中的反射率值
        private DenseVector ReadSpecFile(string spec_Path)
        {
            try
            {
                //定义数组存储端元光谱反射率值
                //double[] Ref_Value = new double[NumofBands];
                DenseVector Ref_Value = new DenseVector(NumofBands);
                //利用ReadAllline读取光谱TXT文件所有行存入字符串数组
                string[] lines = System.IO.File.ReadAllLines(spec_Path, Encoding.Default).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                //MessageBox.Show(lines.Length.ToString());
                if (lines[0] != "**********************元数据**********************")
                {
                    MessageBox.Show(this, "该文件不是标准光谱文件，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                //如果端元光谱波段与影像波段不匹配，则直接退出
                if ((lines.Length - 23) != NumofBands)
                {
                    MessageBox.Show("所选端元光谱与影像波段不匹配，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                //如果端元光谱波段与影像波段匹配，则继续进行
                for (int i = 23; i < lines.Length; i++)
                {
                    if (lines[i].Contains(",") && i >= 23)//遍历光谱文件中的光谱数据行，获取光谱反射率值
                    {
                        string[] spectrumdata = lines[i].Trim().Split(',');
                        Ref_Value[i - 23] = double.Parse(spectrumdata[1]);
                    }
                }
                return Ref_Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return null;
            }
        }

        //MESMA是多个SMA的循环运行，寻找最小RMSE得到最佳端元模型
        //首先构造简单线性混合像元分解（SMA）函数用于对每个像元每个模型进行分解
        //输入：端元矩阵，像元光谱向量
        //输出：RMSE,端元丰度值
        private SMAResult SMA_Frac_Cacul(DenseMatrix Matrix_EM, DenseVector Vector_Pixel)
        {
            SMAResult sma_result = new SMAResult();

            //利用SVD求解线性方程组的最小二乘解，求取丰度值
            DenseVector Vector_Frac = (DenseVector)Matrix_EM.Svd().Solve(Vector_Pixel);
            DenseVector model_Pixel = Matrix_EM * Vector_Frac;//将所求丰度值带入原式，求得模型值
            DenseVector resid = Vector_Pixel - model_Pixel;//将像元反射率值-模型值，求得残差residual

            //计算残差平方和
            double resi_2_sum = 0;
            for (int m = 0; m < NumofBands; m++)
            {
                resi_2_sum += Math.Pow(resid[m], 2);
            }

            //存储端元模型中每个端元的丰度值
            sma_result.frac = Vector_Frac.ToArray();

            //存储端元模型中阴影端元的丰度值
            sma_result.shade = 1.0 - Vector_Frac.Sum();

            //计算RMSE并存储
            sma_result.rmse = Math.Round(Math.Sqrt(resi_2_sum / NumofBands), 5);

            return sma_result;

        }

        //根据界面设置的约束条件得到输出结果
        private double ConstrainedOutput(SMAResult tmp_sma_result)
        {
            //创建矢量存储约束条件标记
            Vector<double> constrain_tag = Vector<double>.Build.Dense(5);

            //端元丰度最小值约束
            if (FracMincheckBox.Checked == true)
            {
                // 获取设置的丰度最小值
                double min_frac = double.Parse(FracMintextBox.Text.ToString());
                //不同的端元模型进行比较,只比较非阴影端元丰度
                //二端元模型有两个丰度：一个非阴影端元丰度，一个阴影端元丰度
                //三端元模型有三个丰度：两个非阴影端元丰度，一个阴影端元丰度
                //四端元模型有四个丰度：三个非阴影端元丰度，一个阴影端元丰度
                switch (TypeofModel)
                {
                    case 2://二端元模型
                        if (tmp_sma_result.frac[0] >= min_frac)
                        {
                            constrain_tag[0] = 1;
                        }
                        break;
                    case 3://三端元模型
                        if ((tmp_sma_result.frac[0] >= min_frac) && (tmp_sma_result.frac[1] >= min_frac))
                        {
                            constrain_tag[0] = 1;
                        }
                        break;
                    case 4://四端元模型
                        if ((tmp_sma_result.frac[0] >= min_frac) && (tmp_sma_result.frac[1] >= min_frac) && (tmp_sma_result.frac[2] >= min_frac))
                        {
                            constrain_tag[0] = 1;
                        }
                        break;
                }

            }
            else
                constrain_tag[0] = 1;

            //端元丰度最大值约束
            if (FracMaxcheckBox.Checked == true)
            {
                // 获取设置的丰度最小值
                double max_frac = double.Parse(FracMaxtextBox.Text.ToString());
                //不同的端元模型进行比较,只比较非阴影端元丰度
                //二端元模型有两个丰度：一个非阴影端元丰度，一个阴影端元丰度
                //三端元模型有三个丰度：两个非阴影端元丰度，一个阴影端元丰度
                //四端元模型有四个丰度：三个非阴影端元丰度，一个阴影端元丰度
                switch (TypeofModel)
                {
                    case 2://二端元模型
                        if (tmp_sma_result.frac[0] <= max_frac)
                        {
                            constrain_tag[1] = 1;
                        }
                        break;
                    case 3://三端元模型
                        if ((tmp_sma_result.frac[0] <= max_frac) && (tmp_sma_result.frac[1] <= max_frac))
                        {
                            constrain_tag[1] = 1;
                        }
                        break;
                    case 4://四端元模型
                        if ((tmp_sma_result.frac[0] <= max_frac) && (tmp_sma_result.frac[1] <= max_frac) && (tmp_sma_result.frac[2] <= max_frac))
                        {
                            constrain_tag[1] = 1;
                        }
                        break;
                }

            }
            else
                constrain_tag[1] = 1;

            //阴影丰度约束
            if (ShadeMaxcheckBox.Checked == true)
            {
                // 获取设置的阴影丰度最大值
                double max_shade = double.Parse(ShadeMaxtextBox.Text.ToString());
                if (tmp_sma_result.shade <= max_shade)
                {
                    constrain_tag[2] = 1;
                }

            }
            else
                constrain_tag[2] = 1;

            //RMSE约束
            if (RMSEMaxcheckBox.Checked == true)
            {
                // 获取设置的RMSE最大值
                double max_rms = (double)RMSEMaxNumericUp.Value;
                if (tmp_sma_result.rmse <= max_rms)
                {
                    constrain_tag[3] = 1;
                }
            }
            else
                constrain_tag[3] = 1;

            //残差约束
            if (ResiMaxcheckBox.Checked == true)
            {
                /////??????????

            }
            else
                constrain_tag[4] = 1;

            //如果该RMSE最小模型满足以上约束条件，则输出结果

            return constrain_tag.Sum();
        }

        //将所选定的模型信息存储到表中
        private void ModelInfoTableReport()
        {
            try
            {
                ModelInfo_dt.Clear();//运行前清楚之前存储的所有模型信息

                switch (TypeofModel)
                {
                    case 2:
                        for (int i = 0; i < EMLibrary1listView.Items.Count; i++)
                        {
                            DataRow SingleModelRow = ModelInfo_dt.NewRow();//创建DataTable的行存储模型信息
                            string ModelEM1Name = Path.GetFileNameWithoutExtension(EMLibrary1listView.Items[i].SubItems[1].Text.ToString());
                            SingleModelRow["模型号"] = i + 1;
                            SingleModelRow["端元1"] = ModelEM1Name;
                            SingleModelRow["端元2"] = "阴影";
                            SingleModelRow["端元3"] = "Null";
                            SingleModelRow["端元4"] = "Null";
                            ModelInfo_dt.Rows.Add(SingleModelRow);
                        }
                        break;
                    case 3:
                        for (int i = 0; i < NumofEMLibrary1; i++)
                        {
                            for (int j = 0; j < NumofEMLibrary2; j++)
                            {
                                DataRow SingleModelRow = ModelInfo_dt.NewRow();//创建DataTable的行存储模型信息
                                string ModelEM1Name = Path.GetFileNameWithoutExtension(EMLibrary1listView.Items[i].SubItems[1].Text.ToString());
                                string ModelEM2Name = Path.GetFileNameWithoutExtension(EMLibrary2listView.Items[j].SubItems[1].Text.ToString());
                                SingleModelRow["模型号"] = i * NumofEMLibrary2 + j + 1;
                                SingleModelRow["端元1"] = ModelEM1Name;
                                SingleModelRow["端元2"] = ModelEM2Name;
                                SingleModelRow["端元3"] = "阴影";
                                SingleModelRow["端元4"] = "Null";
                                ModelInfo_dt.Rows.Add(SingleModelRow);
                            }
                        }
                        break;
                    case 4:
                        for (int i = 0; i < NumofEMLibrary1; i++)
                        {
                            for (int j = 0; j < NumofEMLibrary2; j++)
                            {
                                for (int z = 0; z < NumofEMLibrary3; z++)
                                {
                                    DataRow SingleModelRow = ModelInfo_dt.NewRow();//创建DataTable的行存储模型信息
                                    string ModelEM1Name = Path.GetFileNameWithoutExtension(EMLibrary1listView.Items[i].SubItems[1].Text.ToString());
                                    string ModelEM2Name = Path.GetFileNameWithoutExtension(EMLibrary2listView.Items[j].SubItems[1].Text.ToString());
                                    string ModelEM3Name = Path.GetFileNameWithoutExtension(EMLibrary3listView.Items[z].SubItems[1].Text.ToString());
                                    SingleModelRow["模型号"] = i * NumofEMLibrary2 * NumofEMLibrary3 + j * NumofEMLibrary3 + z + 1; ;
                                    SingleModelRow["端元1"] = ModelEM1Name;
                                    SingleModelRow["端元2"] = ModelEM2Name;
                                    SingleModelRow["端元3"] = ModelEM2Name;
                                    SingleModelRow["端元4"] = "阴影";
                                    ModelInfo_dt.Rows.Add(SingleModelRow);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return;
            }
        }

        //从参数设置界面获取不同老化阶段路面的模型号数组和其他参数
        public void ExtractReportParameter(int[] tmp_PAModelNumSet, int[] tmp_MAModelNumSet, int[] tmp_HAModelNumSet, int tmp_PavementFracBands,
            string tmp_RoadName, string tmp_RoadType, string tmp_RoadLevel, string tmp_PaveType, string tmp_RoadPaveDate, string tmp_YanghuDate, string tmp_UseLength)
        {
            try
            {
                // 获取公路属性信息
                PDF_RoadName = tmp_RoadName;//道路名称
                PDF_RoadType = tmp_RoadType;//道路类型
                PDF_RoadLevel = tmp_RoadLevel;//道路等级
                PDF_PaveType = tmp_PaveType;//路面材质类型
                PDF_RoadPaveDate = tmp_RoadPaveDate;//铺设日期
                PDF_YanghuDate = tmp_YanghuDate;//上一次路面养护日期
                PDF_RoadUseLength = tmp_UseLength;//使用年数    

                //获取不同老化阶段的混合像元模型号
                MESMA_PAModelNumSet = tmp_PAModelNumSet;
                MESMA_MAModelNumSet = tmp_MAModelNumSet;
                MESMA_HAModelNumSet = tmp_HAModelNumSet;
                MESMA_PavementFracBands = tmp_PavementFracBands;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return;
            }

        }

        //输出不同老化路面的丰度影像
        private void FracReport(int tmp_OutputModelNum, int PixelNum, double[] tmp_output_PA, double[] tmp_output_MA, double[] tmp_output_HA, double tmp_OutputAgingFrac)
        {
            try
            {
                //检查该模型号是否属于老化初期模型号数组，如果属于，则将该丰度值保存为老化初期路面影像中
                foreach (int tmp_i1 in MESMA_PAModelNumSet)
                {
                    if (tmp_OutputModelNum == tmp_i1)
                    {
                        tmp_output_PA[PixelNum] = tmp_OutputAgingFrac;
                        return;//如果找到直接返回
                    }
                }
                //检查该模型号是否属于老化中期模型号数组，如果属于，则将该丰度值保存为老化中期路面影像中
                foreach (int tmp_i2 in MESMA_MAModelNumSet)
                {
                    if (tmp_OutputModelNum == tmp_i2)
                    {
                        tmp_output_MA[PixelNum] = tmp_OutputAgingFrac;
                        return;//如果找到直接返回
                    }
                }
                //检查该模型号是否属于老化后期模型号数组，如果属于，则将该丰度值保存为老化后期路面影像中
                foreach (int tmp_i3 in MESMA_HAModelNumSet)
                {
                    if (tmp_OutputModelNum == tmp_i3)
                    {
                        tmp_output_HA[PixelNum] = tmp_OutputAgingFrac;
                        return;//如果找到直接返回
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return;
            }
        }

        //创建条形统计图
        private string CreateBarChart(double tmp_StaofPercentofPA, double tmp_StaofPercentofMA, double tmp_StaofPercentofHA)
        {
            try
            {
                //将比例值取整
                int CountofPA = (int)Math.Ceiling(tmp_StaofPercentofPA);
                int CountofMA = (int)Math.Ceiling(tmp_StaofPercentofMA);
                int CountofHA = (int)Math.Ceiling(tmp_StaofPercentofHA);

                //设置图片大小
                int height = 450, width = 700;
                Bitmap image = new Bitmap(width, height);
                //创建Graphics类对象
                Graphics g = Graphics.FromImage(image);
                try
                {
                    //清空图片背景色
                    g.Clear(Color.White);
                    //设置字体格式
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 10, FontStyle.Regular);
                    System.Drawing.Font font1 = new System.Drawing.Font("宋体", 20, FontStyle.Bold);
                    //设置笔刷样式
                    LinearGradientBrush brush = new LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.BlueViolet, 1.2f, true);
                    //填充图片背景
                    g.FillRectangle(Brushes.WhiteSmoke, 0, 0, width, height);
                    // Brush brush1 = new SolidBrush(Color.Blue);
                    //画统计表标题
                    g.DrawString("不同老化阶段路面面积比例统计图", font1, brush, new PointF(130, 30));
                    //画图片的边框线
                    g.DrawRectangle(new Pen(Color.Blue), 0, 0, image.Width - 1, image.Height - 1);

                    //绘制网格线条
                    Pen mypen = new Pen(brush, 1);
                    Pen mypen1 = new Pen(Color.Blue, 2);
                    //绘制纵向线条
                    int x = 70;
                    g.DrawLine(mypen1, x, 80, x, 380);//左边框
                    x = 630;
                    g.DrawLine(mypen1, x, 80, x, 380);//右边框      

                    //绘制横向线条
                    int y = 80;
                    g.DrawLine(mypen1, 70, y, 630, y);//上边框
                    for (int i = 0; i < 10; i++)
                    {
                        g.DrawLine(mypen, 70, y, 630, y);
                        y = y + 30;
                    }
                    y = 380;
                    g.DrawLine(mypen1, 70, y, 630, y);//下边框

                    //x轴
                    String[] n = { "老化初期", "老化中期", "老化后期" };
                    x = 190;
                    for (int i = 0; i < 3; i++)
                    {
                        g.DrawString(n[i].ToString(), font, Brushes.Blue, x, 390); //设置文字内容及输出位置
                        x = x + 140;
                    }
                    //y轴
                    String[] m = { "100", " 90", " 80", " 70", " 60", " 50", " 40", " 30", " 20", " 10", "  0" };
                    y = 72;
                    for (int i = 0; i < 10; i++)
                    {
                        g.DrawString(m[i].ToString(), font, Brushes.Blue, 35, y); //设置文字内容及输出位置
                        y = y + 30;
                    }
                    //绘制柱状图              
                    System.Drawing.Font font2 = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    SolidBrush PABrush = new SolidBrush(Color.Green);
                    SolidBrush MABrush = new SolidBrush(Color.Blue);
                    SolidBrush HABrush = new SolidBrush(Color.Red);

                    //老化初期
                    x = 200;
                    g.FillRectangle(PABrush, x, 380 - CountofPA * 3, 40, CountofPA * 3);
                    g.DrawString(tmp_StaofPercentofPA.ToString(), font2, Brushes.Red, x + 5, 380 - CountofPA * 3 - 15);

                    //老化中期
                    x = 340;
                    g.FillRectangle(MABrush, x, 380 - CountofMA * 3, 40, CountofMA * 3);
                    g.DrawString(tmp_StaofPercentofMA.ToString(), font2, Brushes.Red, x + 5, 380 - CountofMA * 3 - 15);

                    //老化后期
                    x = 480;
                    g.FillRectangle(HABrush, x, 380 - CountofHA * 3, 40, CountofHA * 3);
                    g.DrawString(tmp_StaofPercentofHA.ToString(), font2, Brushes.Red, x + 5, 380 - CountofHA * 3 - 15);
                    string tmpfilepath = SaveStaReporttextBox.Text.ToString();
                    string StaBarChartFilePath = Path.GetDirectoryName(tmpfilepath) + "\\" + Path.GetFileNameWithoutExtension(tmpfilepath) + ".png";
                    image.Save(StaBarChartFilePath);//默认保存格式为PNG，保存成jpg格式质量不是很好
                    g.Dispose();
                    image.Dispose();
                    return StaBarChartFilePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//捕捉异常
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
                return null;
            }
        }

        //设置统计报告PDF的输出格式
        private void StaReportPDFSetting()
        {
            //创建一个Document对象的实例
            Document document = new Document(PageSize.A4, 70, 70, 50, 50);
            //为该Document创建一个Writer实例
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(SaveStaReporttextBox.Text.ToString(), FileMode.Create));
            writer.PageEvent = new ITextEvents();
            writer.InitialLeading = 15;//行间距
            //打开当前Document
            document.Open();

            //定义字体   SIMSUN.TTC：宋体和新宋体 SIMKAI.TTF：楷体  SIMHEI.TTF：黑体   SIMFANG.TTF：仿宋体
            //BaseFont bfSun = BaseFont.CreateFont("C:\\Windows\\Fonts\\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//宋体 
            BaseFont bftitle = BaseFont.CreateFont(@"C:\Windows\Fonts\SIMHEI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //BaseFont bftitle1 = BaseFont.CreateFont(@"C:\Windows\Fonts\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            BaseFont bftitle1 = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //报告标题字体
            iTextSharp.text.Font FontReportTitle = new iTextSharp.text.Font(bftitle, 15);//报告标题字体
            iTextSharp.text.Font FontContentTitle = new iTextSharp.text.Font(bftitle1, 12, iTextSharp.text.Font.BOLD);//正文标题字体
            iTextSharp.text.Font FontContent = new iTextSharp.text.Font(bftitle1, 10.5f);//正文字体

            //统计报告标题
            Paragraph ReportTitle = new Paragraph("公路路面老化状况遥感监测与评估报告", FontReportTitle);
            ReportTitle.Alignment = 1;
            document.Add(ReportTitle);

            //输入一个空行以分开标题与内容
            Paragraph nullp0 = new Paragraph(" ", FontContent);
            nullp0.Leading = 20;
            document.Add(nullp0);

            //--------------------------公路属性信息-----------------------------//
            #region

            Paragraph RoadInfoSetting = new Paragraph("（1）公路属性信息", FontContentTitle);
            RoadInfoSetting.Alignment = 0;
            document.Add(RoadInfoSetting);

            //公路属性信息表标题
            Paragraph RoadInfoTableTitle = new Paragraph("公路属性信息表", FontContent);
            RoadInfoTableTitle.Alignment = 1;
            document.Add(RoadInfoTableTitle);

            //输入一个空行以分开公路属性信息表标题与表格
            Paragraph nullp1 = new Paragraph(" ", FontContent);
            nullp1.Leading = 10;
            document.Add(nullp1);

            //创建表格
            PdfPTable RoadInfoTable = new PdfPTable(4);
            RoadInfoTable.WidthPercentage = 100;
            //StaTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //创建单元格
            //第一行
            PdfPCell RIT_Rows1Cell1 = new PdfPCell();
            RIT_Rows1Cell1.Colspan = 1;
            Paragraph RoadName = new Paragraph("公路名称", FontContent);
            RoadName.Alignment = 1;
            RIT_Rows1Cell1.AddElement(RoadName);
            RoadInfoTable.AddCell(RIT_Rows1Cell1);

            PdfPCell RIT_Rows1Cell2 = new PdfPCell();
            RIT_Rows1Cell2.Colspan = 3;
            Paragraph RoadNameInfo = new Paragraph(PDF_RoadName, FontContent);
            RoadNameInfo.Alignment = 1;
            RIT_Rows1Cell2.AddElement(RoadNameInfo);
            RoadInfoTable.AddCell(RIT_Rows1Cell2);

            //第二行
            string[] RoadInfoTableContent = { "公路类型", PDF_RoadType, "公路等级", PDF_RoadLevel, "路面材质", PDF_PaveType, "铺设日期", PDF_RoadPaveDate, "养护日期", PDF_YanghuDate, "使用年数", PDF_RoadUseLength };
            for (int iRoadInfoCell = 0; iRoadInfoCell < 12; iRoadInfoCell++)
            {

                PdfPCell RIT_Rows2Cell1 = new PdfPCell();
                RIT_Rows2Cell1.Colspan = 1;
                Paragraph RoadType = new Paragraph(RoadInfoTableContent[iRoadInfoCell], FontContent);
                RoadType.Alignment = 1;
                RIT_Rows2Cell1.AddElement(RoadType);
                RoadInfoTable.AddCell(RIT_Rows2Cell1);
            }

            document.Add(RoadInfoTable);

            #endregion

            //输入一个空行以分开内容
            Paragraph nullp2 = new Paragraph(" ", FontContent);
            nullp2.Leading = 10;
            document.Add(nullp2);

            //--------------------------算法参数信息-----------------------------//
            #region
            Paragraph BasicInputSetting = new Paragraph("（2）算法参数信息", FontContentTitle);
            BasicInputSetting.Alignment = 0;
            document.Add(BasicInputSetting);

            //算法参数信息表标题
            Paragraph ModelInfoTableTitle = new Paragraph("算法参数信息表", FontContent);
            ModelInfoTableTitle.Alignment = 1;
            document.Add(ModelInfoTableTitle);

            //输入一个空行以分开公路属性信息表标题与表格
            Paragraph nullp3 = new Paragraph(" ", FontContent);
            nullp3.Leading = 10;
            document.Add(nullp3);

            //创建表格
            PdfPTable ModelInfoTable = new PdfPTable(4);
            ModelInfoTable.WidthPercentage = 100;
            //StaTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //创建单元格
            //第一行
            PdfPCell MIT_Rows1Cell1 = new PdfPCell();
            MIT_Rows1Cell1.Colspan = 1;
            MIT_Rows1Cell1.BackgroundColor = BaseColor.GRAY;
            Paragraph AlgorithmName = new Paragraph("算法名称", FontContent);
            AlgorithmName.Alignment = 1;
            MIT_Rows1Cell1.AddElement(AlgorithmName);
            ModelInfoTable.AddCell(MIT_Rows1Cell1);

            PdfPCell MIT_Rows1Cell2 = new PdfPCell();
            MIT_Rows1Cell2.Colspan = 3;
            Paragraph AlgorithmNameInfo = new Paragraph("多端元混合像元分解模型", FontContent);
            AlgorithmNameInfo.Alignment = 1;
            MIT_Rows1Cell2.AddElement(AlgorithmNameInfo);
            ModelInfoTable.AddCell(MIT_Rows1Cell2);

            //第二行
            PdfPCell MIT_Rows2Cell1 = new PdfPCell();
            MIT_Rows2Cell1.Colspan = 4;
            MIT_Rows2Cell1.BackgroundColor = BaseColor.GRAY;
            Paragraph RSImageInfo = new Paragraph("遥感影像信息", FontContent);
            RSImageInfo.Alignment = 1;
            MIT_Rows2Cell1.AddElement(RSImageInfo);
            ModelInfoTable.AddCell(MIT_Rows2Cell1);

            //第三行
            PdfPCell MIT_Rows3Cell1 = new PdfPCell();
            MIT_Rows3Cell1.Colspan = 1;
            Paragraph RSImageName = new Paragraph("遥感影像", FontContent);
            RSImageName.Alignment = 1;
            MIT_Rows3Cell1.AddElement(RSImageName);
            ModelInfoTable.AddCell(MIT_Rows3Cell1);

            PdfPCell MIT_Rows3Cell2 = new PdfPCell();
            MIT_Rows3Cell2.Colspan = 3;
            Paragraph RSImageNameInfo = new Paragraph(ImagePathtextBox.Text.ToString(), FontContent);
            RSImageNameInfo.Alignment = 0;
            MIT_Rows3Cell2.AddElement(RSImageNameInfo);
            ModelInfoTable.AddCell(MIT_Rows3Cell2);

            //第四行
            string[] line4content = { "影像波段数", NumofBands.ToString(), "影像分辨率", PixelSizeX.ToString() + "m X " + PixelSizeY.ToString() + "m" };
            for (int line4 = 0; line4 < 4; line4++)
            {
                PdfPCell MIT_Rows4Cell1 = new PdfPCell();
                MIT_Rows4Cell1.Colspan = 1;
                Paragraph RSImageBasicInfo = new Paragraph(line4content[line4], FontContent);
                RSImageBasicInfo.Alignment = 1;
                MIT_Rows4Cell1.AddElement(RSImageBasicInfo);
                ModelInfoTable.AddCell(MIT_Rows4Cell1);
            }

            //第五行
            PdfPCell MIT_Rows5Cell1 = new PdfPCell();
            MIT_Rows5Cell1.Colspan = 4;
            MIT_Rows5Cell1.BackgroundColor = BaseColor.GRAY;
            Paragraph SpeclibInfo = new Paragraph("光谱库信息", FontContent);
            SpeclibInfo.Alignment = 1;
            MIT_Rows5Cell1.AddElement(SpeclibInfo);
            ModelInfoTable.AddCell(MIT_Rows5Cell1);

            //第六行
            PdfPCell MIT_Rows6Cell1 = new PdfPCell();
            MIT_Rows6Cell1.Colspan = 4;
            Paragraph EMSpeclib1Info = new Paragraph("端元光谱库（一）", FontContent);
            EMSpeclib1Info.Alignment = 1;
            MIT_Rows6Cell1.AddElement(EMSpeclib1Info);
            ModelInfoTable.AddCell(MIT_Rows6Cell1);

            //第七至第EMLibrary1listView.Items.Count行
            for (int iEM1Spec = 0; iEM1Spec < EMLibrary1listView.Items.Count; iEM1Spec++)
            {
                PdfPCell MIT_Rows7Cell1 = new PdfPCell();
                MIT_Rows7Cell1.Colspan = 1;
                Paragraph EMSpeclib1Index = new Paragraph((iEM1Spec + 1).ToString(), FontContent);
                EMSpeclib1Index.Alignment = 1;
                MIT_Rows7Cell1.AddElement(EMSpeclib1Index);
                ModelInfoTable.AddCell(MIT_Rows7Cell1);

                PdfPCell MIT_Rows7Cell2 = new PdfPCell();
                MIT_Rows7Cell2.Colspan = 3;
                Paragraph EMSpeclib1Name = new Paragraph(EMLibrary1listView.Items[iEM1Spec].SubItems[1].Text.ToString(), FontContent);
                EMSpeclib1Name.Alignment = 0;
                MIT_Rows7Cell2.AddElement(EMSpeclib1Name);
                ModelInfoTable.AddCell(MIT_Rows7Cell2);
            }

            //第八行
            if (EMLibrary2checkBox.Checked == true)
            {
                PdfPCell MIT_Rows8Cell1 = new PdfPCell();
                MIT_Rows8Cell1.Colspan = 4;
                Paragraph EMSpeclib2Info = new Paragraph("端元光谱库（二）", FontContent);
                EMSpeclib2Info.Alignment = 1;
                MIT_Rows8Cell1.AddElement(EMSpeclib2Info);
                ModelInfoTable.AddCell(MIT_Rows8Cell1);
                //第九行
                for (int iEM2Spec = 0; iEM2Spec < EMLibrary2listView.Items.Count; iEM2Spec++)
                {
                    PdfPCell MIT_Rows9Cell1 = new PdfPCell();
                    MIT_Rows9Cell1.Colspan = 1;
                    Paragraph EMSpeclib2Index = new Paragraph((iEM2Spec + 1).ToString(), FontContent);
                    EMSpeclib2Index.Alignment = 1;
                    MIT_Rows9Cell1.AddElement(EMSpeclib2Index);
                    ModelInfoTable.AddCell(MIT_Rows9Cell1);

                    PdfPCell MIT_Rows9Cell2 = new PdfPCell();
                    MIT_Rows9Cell2.Colspan = 3;
                    Paragraph EMSpeclib2Name = new Paragraph(EMLibrary2listView.Items[iEM2Spec].SubItems[1].Text.ToString(), FontContent);
                    EMSpeclib2Name.Alignment = 0;
                    MIT_Rows9Cell2.AddElement(EMSpeclib2Name);
                    ModelInfoTable.AddCell(MIT_Rows9Cell2);
                }
            }

            //第十行
            if (EMLibrary3checkBox.Checked == true)
            {
                PdfPCell MIT_Rows10Cell1 = new PdfPCell();
                MIT_Rows10Cell1.Colspan = 4;
                Paragraph EMSpeclib3Info = new Paragraph("端元光谱库（三）", FontContent);
                EMSpeclib3Info.Alignment = 1;
                MIT_Rows10Cell1.AddElement(EMSpeclib3Info);
                ModelInfoTable.AddCell(MIT_Rows10Cell1);
                //第十一行
                for (int iEM3Spec = 0; iEM3Spec < EMLibrary3listView.Items.Count; iEM3Spec++)
                {
                    PdfPCell MIT_Rows11Cell1 = new PdfPCell();
                    MIT_Rows11Cell1.Colspan = 1;
                    Paragraph EMSpeclib3Index = new Paragraph((iEM3Spec + 1).ToString(), FontContent);
                    EMSpeclib3Index.Alignment = 1;
                    MIT_Rows11Cell1.AddElement(EMSpeclib3Index);
                    ModelInfoTable.AddCell(MIT_Rows11Cell1);

                    PdfPCell MIT_Rows9Cell2 = new PdfPCell();
                    MIT_Rows9Cell2.Colspan = 3;
                    Paragraph EMSpeclib3Name = new Paragraph(EMLibrary3listView.Items[iEM3Spec].SubItems[1].Text.ToString(), FontContent);
                    EMSpeclib3Name.Alignment = 0;
                    MIT_Rows9Cell2.AddElement(EMSpeclib3Name);
                    ModelInfoTable.AddCell(MIT_Rows9Cell2);
                }
            }

            //第十二行
            PdfPCell MIT_Rows12Cell1 = new PdfPCell();
            MIT_Rows12Cell1.Colspan = 4;
            MIT_Rows12Cell1.BackgroundColor = BaseColor.GRAY;
            Paragraph ModelDetailInfo = new Paragraph("模型信息", FontContent);
            ModelDetailInfo.Alignment = 1;
            MIT_Rows12Cell1.AddElement(ModelDetailInfo);
            ModelInfoTable.AddCell(MIT_Rows12Cell1);

            //第十三行
            string[] line13content = { "模型类型", TypeofModel.ToString() + "端元模型", "模型个数", NumofModel.ToString() };
            for (int line13 = 0; line13 < 4; line13++)
            {
                PdfPCell MIT_Rows13Cell1 = new PdfPCell();
                MIT_Rows13Cell1.Colspan = 1;
                Paragraph ModelBasicInfo = new Paragraph(line13content[line13], FontContent);
                ModelBasicInfo.Alignment = 1;
                MIT_Rows13Cell1.AddElement(ModelBasicInfo);
                ModelInfoTable.AddCell(MIT_Rows13Cell1);

            }

            //第十四行
            string FracMinResInfo;
            if (FracMincheckBox.Checked == false)
            {
                FracMinResInfo = "";
            }
            else
            {
                FracMinResInfo = FracMintextBox.Text.ToString();
            }
            string FracMaxResInfo;
            if (FracMaxcheckBox.Checked == false)
            {
                FracMaxResInfo = "";
            }
            else
            {
                FracMaxResInfo = FracMaxtextBox.Text.ToString();
            }

            string[] line14content = { "端元丰度最小值", FracMinResInfo, "端元丰度最大值", FracMaxResInfo };
            for (int line14 = 0; line14 < 4; line14++)
            {
                PdfPCell MIT_Rows14Cell1 = new PdfPCell();
                MIT_Rows14Cell1.Colspan = 1;
                Paragraph FracResInfo = new Paragraph(line14content[line14], FontContent);
                FracResInfo.Alignment = 1;
                MIT_Rows14Cell1.AddElement(FracResInfo);
                ModelInfoTable.AddCell(MIT_Rows14Cell1);
            }

            //第十五行
            string ShadeFracMaxResInfo;
            if (ShadeMaxcheckBox.Checked == false)
            {
                ShadeFracMaxResInfo = "";
            }
            else
            {
                ShadeFracMaxResInfo = ShadeMaxtextBox.Text.ToString();
            }
            string MaxRMSEResInfo;
            if (RMSEMaxcheckBox.Checked == false)
            {
                MaxRMSEResInfo = "";
            }
            else
            {
                MaxRMSEResInfo = RMSEMaxNumericUp.Value.ToString();
            }

            string[] line15content = { "阴影丰度最大值", ShadeFracMaxResInfo, "RMSE最大值", MaxRMSEResInfo };
            for (int line15 = 0; line15 < 4; line15++)
            {
                PdfPCell MIT_Rows15Cell1 = new PdfPCell();
                MIT_Rows15Cell1.Colspan = 1;
                Paragraph ShadeRMSEResInfo = new Paragraph(line15content[line15], FontContent);
                ShadeRMSEResInfo.Alignment = 1;
                MIT_Rows15Cell1.AddElement(ShadeRMSEResInfo);
                ModelInfoTable.AddCell(MIT_Rows15Cell1);
            }

            //第十六行
            string ResiMaxResInfo;
            string StaBandsResInfo;
            if (ResiMaxcheckBox.Checked == false)
            {
                ResiMaxResInfo = "";
                StaBandsResInfo = "";
            }
            else
            {
                ResiMaxResInfo = ResiMaxNumericUp.Value.ToString();
                StaBandsResInfo = ResiStaBandtextBox.Text.ToString();
            }

            string[] line16content = { "残差最大值", ResiMaxResInfo, "连续统计波段数", StaBandsResInfo };
            for (int line16 = 0; line16 < 4; line16++)
            {
                PdfPCell MIT_Rows16Cell1 = new PdfPCell();
                MIT_Rows16Cell1.Colspan = 1;
                Paragraph ResiResInfo = new Paragraph(line16content[line16], FontContent);
                ResiResInfo.Alignment = 1;
                MIT_Rows16Cell1.AddElement(ResiResInfo);
                ModelInfoTable.AddCell(MIT_Rows16Cell1);
            }

            //第十七行
            PdfPCell MIT_Rows17Cell1 = new PdfPCell();
            MIT_Rows17Cell1.Colspan = 4;
            MIT_Rows17Cell1.BackgroundColor = BaseColor.GRAY;
            Paragraph OutPutSettingInfo = new Paragraph("输出信息", FontContent);
            OutPutSettingInfo.Alignment = 1;
            MIT_Rows17Cell1.AddElement(OutPutSettingInfo);
            ModelInfoTable.AddCell(MIT_Rows17Cell1);

            //第十八行
            string[] outputname_arr = { "地物丰度影像", "路面丰度影像", "RMSE影像", "模型影像", "统计报告" };
            string[] outputfilepath = { SaveFracPathtextBox.Text.ToString(), SavePaveFractextBox.Text.ToString(), SaveRMSEtextBox.Text.ToString(), SaveModeltextBox.Text.ToString(), SaveStaReporttextBox.Text.ToString() };
            for (int iOutput = 0; iOutput < 5; iOutput++)
            {
                PdfPCell MIT_Rows18Cell1 = new PdfPCell();
                MIT_Rows18Cell1.Colspan = 1;
                Paragraph OutputName = new Paragraph(outputname_arr[iOutput], FontContent);
                OutputName.Alignment = 1;
                MIT_Rows18Cell1.AddElement(OutputName);
                ModelInfoTable.AddCell(MIT_Rows18Cell1);

                PdfPCell MIT_Rows18Cell2 = new PdfPCell();
                MIT_Rows18Cell2.Colspan = 3;
                Paragraph OutputPath = new Paragraph(outputfilepath[iOutput], FontContent);
                OutputPath.Alignment = 0;
                MIT_Rows18Cell2.AddElement(OutputPath);
                ModelInfoTable.AddCell(MIT_Rows18Cell2);
            }

            document.Add(ModelInfoTable);
            #endregion

            //输入一个空行（换行）以基本信息与统计表内容
            Paragraph nullp4 = new Paragraph(" ", FontContent);
            nullp4.Leading = 10;
            document.Add(nullp4);

            //--------------------------空间分布图信息-----------------------------//
            #region

            Paragraph BasicAgingFrac = new Paragraph("（3）不同老化阶段路面空间分布图", FontContentTitle);
            BasicAgingFrac.Alignment = 0;
            document.Add(BasicAgingFrac);

            string tmp_reportfilepath = SaveStaReporttextBox.Text.ToString();
            iTextSharp.text.Image agingfracimg = iTextSharp.text.Image.GetInstance(Path.GetDirectoryName(tmp_reportfilepath) + "\\" + Path.GetFileNameWithoutExtension(tmp_reportfilepath) + ".jpg");
            agingfracimg.Transparency = new int[] { 0, 0, 0, 0, 0, 0 };
            agingfracimg.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            agingfracimg.ScalePercent(40);

            //agingfracimg.SetAbsolutePosition((PageSize.POSTCARD.Width - agingfracimg.ScaledWidth) / 2, (PageSize.POSTCARD.Height - agingfracimg.ScaledHeight) / 2);
            //writer.DirectContent.AddImage(agingfracimg);
            document.Add(agingfracimg);
            document.NewPage();
            #endregion

            //输入一个空行（换行）以空间分布图信息与统计表内容
            Paragraph nullp5 = new Paragraph(" ", FontContent);
            nullp5.Leading = 20;
            document.Add(nullp5);

            //--------------------------统计表信息-----------------------------//
            #region

            Paragraph BasicStaTable = new Paragraph("（4）不同老化阶段路面统计表", FontContentTitle);
            BasicStaTable.Alignment = 0;
            document.Add(BasicStaTable);

            //统计表标题
            Paragraph TableTitle = new Paragraph("不同老化阶段路面面积及其比例统计表", FontContent);
            TableTitle.Alignment = 1;
            document.Add(TableTitle);

            //输入一个空行（换行）以分开标题与表格
            Paragraph nullp6 = new Paragraph(" ", FontContent);
            nullp6.Leading = 10;
            document.Add(nullp6);

            //创建表格
            PdfPTable StaTable = new PdfPTable(3);
            StaTable.WidthPercentage = 100;
            //StaTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //创建单元格
            string[] StaTableContent = { "路面老化类型", "面积(平方米)", "比例(%)", "老化初期", Round_StaofAreaofPA.ToString(),Round_StaofPercentofPA.ToString(),
                                           "老化中期",Round_StaofAreaofMA.ToString(),Round_StaofPercentofMA.ToString(), "老化后期",Round_StaofAreaofHA.ToString(),Round_StaofPercentofHA.ToString(), };
            for (int iStaTable = 0; iStaTable < 12; iStaTable++)
            {
                PdfPCell Rows1Cell1 = new PdfPCell();
                Rows1Cell1.Colspan = 1;
                Paragraph StaTableContentFill = new Paragraph(StaTableContent[iStaTable], FontContent);
                StaTableContentFill.Alignment = 1;
                Rows1Cell1.AddElement(StaTableContentFill);
                StaTable.AddCell(Rows1Cell1);
            }

            document.Add(StaTable);
            #endregion


            //输入一个空行（换行）以统计图与统计表内容
            Paragraph nullp7 = new Paragraph(" ", FontContent);
            nullp7.Leading = 20;
            document.Add(nullp7);

            //--------------------------统计图信息-----------------------------//
            #region
            Paragraph BasicStaBarChart = new Paragraph("（5）不同老化阶段路面统计图", FontContentTitle);
            BasicStaBarChart.Alignment = 0;
            document.Add(BasicStaBarChart);

            iTextSharp.text.Image agingfracStaBar = iTextSharp.text.Image.GetInstance(Path.GetDirectoryName(tmp_reportfilepath) + "\\" + Path.GetFileNameWithoutExtension(tmp_reportfilepath) + ".png");
            agingfracStaBar.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            agingfracStaBar.ScalePercent(50);
            //agingfracimg.SetAbsolutePosition((PageSize.POSTCARD.Width - agingfracimg.ScaledWidth) / 2, (PageSize.POSTCARD.Height - agingfracimg.ScaledHeight) / 2);
            //writer.DirectContent.AddImage(agingfracimg);
            document.Add(agingfracStaBar);
            #endregion

            //关闭PDF文档
            document.Close();
        }

        //检查程序错误列表
        private int ErrorCheck(int error)
        {

            switch (error)
            {
                case 0: //检查影像是否存在
                    if ((!File.Exists(ImagePathtextBox.Text.ToString())) || (ImagePathtextBox.Text.ToString() == ""))
                    {
                        MessageBox.Show("请输入遥感影像或影像不存在，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 1://检查端元光谱库是否为空
                    if (EMLibrary1listView.Items.Count == 0)
                    {
                        MessageBox.Show("端元光谱库为空，请至少启用一个端元光谱库！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 2://检查端元光谱库是否为空
                    if (EMLibrary2checkBox.Checked == true && EMLibrary2listView.Items.Count == 0)
                    {
                        MessageBox.Show("端元光谱库（二）已启用，但为空，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 3://检查端元光谱库是否为空
                    if (EMLibrary3checkBox.Checked == true && EMLibrary3listView.Items.Count == 0)
                    {
                        MessageBox.Show("端元光谱库（三）已启用，但为空，请添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 4://检查输出路径是否存在
                    if (SaveFracPathtextBox.Text.ToString() == "")
                    {
                        MessageBox.Show("请选择地物丰度影像输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    else if (!Directory.Exists(Path.GetDirectoryName(SaveFracPathtextBox.Text.ToString().Trim())))
                    {
                        MessageBox.Show("地物丰度影像输出路径不存在，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 5://检查输出路径是否存在            
                    if (SavePaveFraccheckBox.Checked == true && SavePaveFractextBox.Text.ToString() == "")
                    {
                        MessageBox.Show("请选择路面丰度影像输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    else if (SavePaveFraccheckBox.Checked == true && !Directory.Exists(Path.GetDirectoryName(SavePaveFractextBox.Text.ToString().Trim())))
                    {
                        MessageBox.Show("路面丰度影像输出路径不存在，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 6://检查输出路径是否存在            
                    if (SaveRMSEcheckBox.Checked == true && SaveRMSEtextBox.Text.ToString() == "")
                    {
                        MessageBox.Show("请选择RMSE影像输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    else if (SaveRMSEcheckBox.Checked == true && !Directory.Exists(Path.GetDirectoryName(SaveRMSEtextBox.Text.ToString().Trim())))
                    {
                        MessageBox.Show("RMSE影像输出路径不存在，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 7://检查输出路径是否存在            
                    if (SaveModelcheckBox.Checked == true && SaveModeltextBox.Text.ToString() == "")
                    {
                        MessageBox.Show("请选择模型影像输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    else if (SaveModelcheckBox.Checked == true && !Directory.Exists(Path.GetDirectoryName(SaveModeltextBox.Text.ToString().Trim())))
                    {
                        MessageBox.Show("模型影像输出路径不存在，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 8://检查输出路径是否存在            
                    if (StaReportCheckBox.Checked == true && SaveStaReporttextBox.Text.ToString() == "")
                    {
                        MessageBox.Show("请选择统计报告输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    else if (StaReportCheckBox.Checked == true && !Directory.Exists(Path.GetDirectoryName(SaveStaReporttextBox.Text.ToString().Trim())))
                    {
                        MessageBox.Show("统计报告输出路径不存在，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -1;
                    }
                    break;

                case 9://检查报告参数是否设置
                    if (StaReportCheckBox.Checked == true || SavePaveFraccheckBox.Checked == true)
                    {
                        //检查是否已经返回各老化阶段的模型号                                
                        if (MESMA_PAModelNumSet[0] == 0 || MESMA_MAModelNumSet[0] == 0 || MESMA_HAModelNumSet[0] == 0)
                        {
                            MessageBox.Show("请先设置路面参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return -1;
                        }
                    }
                    break;
            }
            return 10;
        }


        #endregion



    }
}
