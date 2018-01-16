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

namespace PavementDetection
{
    ///<summary>
    ///文件名:EAROpt.cs
    ///功能：EAR用于选取同一类地物中的最优端元，将端元收集器中的选定端元按照不同地物种类分别求算EAR，并填入到列表中，用于选取最优端元的参考标准。
    ///算法：同一类地物端元中，将一条端元用于分解剩余端元，得到的RMSE并求取平均值，即EAR，EAR越小，代表性越好。
    ///约束规则：
    ///(1)无约束：  0-->求解结果全部符合标准
    ///(2)局部约束：0-->求解结果符合标准； 1-->丰度值不符合标准，RMSE符合标准；2-->丰度值不符合标准，RMSE也不符合标准
    ///(3)全局约束：0-->求解结果符合标准； 2-->丰度值符合标准，RMSE不符合标准
    ///                                    2-->丰度值不符合标准，RMSE符合标准
    ///                                    2-->丰度值不符合标准，RMSE也不符合标准
    /// 创建人:潘一凡
    /// 单位:北京大学遥感所
    /// 联系方式:yfpan@pku.edu.cn
    /// 创建日期：2016-07-15
    /// 修改人：
    /// 修改日期：
    /// 修改备注：
    /// 版本：1.0
    /// </summary>

    public partial class EAROpt : Form
    {

        int con;//利用con参数检查约束选中的状态，无约束：con=0；部分约束，con=1；全约束：con=2
        int NumofSpec;//端元光谱个数
        int NumofBands;//端元光谱波段数
        //double[,] class_spec;
        DenseMatrix class_spec;//创建端元光谱矩阵
        EMCollection tmp_fatherform;//定义EMCollection存储父窗口对象

        public EAROpt()//原始构造函数
        {
            InitializeComponent();
        }

        public EAROpt(EMCollection fatherform, DataSet spec_trans)//定义构造函数
        {
            InitializeComponent();
            tmp_fatherform = fatherform;
            NumofSpec = spec_trans.Tables.Count;//端元光谱个数等于数据表中表的个数
            NumofBands = spec_trans.Tables[0].Rows.Count;//端元光谱波段数等于数据表中表的行数
            //class_spec = new double[NumofBands, NumofSpec];
            class_spec = new DenseMatrix(NumofBands, NumofSpec);
            for (int i = 0; i < NumofSpec; i++)//列数
            {
                for (int j = 0; j < NumofBands; j++)//行数
                {
                    class_spec[j, i] = double.Parse(spec_trans.Tables[i].Rows[j][1].ToString());
                }

            }
            //var class_matrix = DenseMatrix.OfArray(class_spec);
            //MessageBox.Show(class_matrix.ToString(), "端元数组", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void EAROpt_Load(object sender, EventArgs e)
        {
            //初始化时，无约束按钮选中
            UnconsRadiobt.Checked = true;
            //初始化时，约束设置按钮不能启用
            EARminValuetrackBar.Enabled = false;
            EARminValuetextBox.Enabled = false;
            EARminValuecheckBox.Enabled = false;
            EARmaxValuetrackBar.Enabled = false;
            EARmaxValuetextBox.Enabled = false;
            EARmaxValuecheckBox.Enabled = false;
            RMSEnumericUp.Enabled = false;
            RMSEcheckBox.Enabled = false;
        }

        //运行EAR优化
        private void EARRunbt_Click(object sender, EventArgs e)
        {
            try
            {
                //约束按钮选中状态，无约束：con=0；部分约束，con=1；全约束：con=2
                if (con >= 1)
                {
                    if (!EARmaxValuecheckBox.Checked && !EARminValuecheckBox.Checked && !RMSEcheckBox.Checked)
                    {
                        MessageBox.Show("约束条件下，请至少选择一种约束条件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }

                //求解线性方程组：EM1*Frac=EM2
                //条件：EM1和EM2属于同一类地物，属于光谱库中的一条光谱数据，用EM1分解光谱EM2；
                //方法：利用SVD分解解算上述线性方程组

                //定义RMSE数组存储rmse；定义EAR数组存储ear
                double[,] rmse_array = new double[NumofSpec, NumofSpec];
                double[] ear_array = new double[NumofSpec];
                //定义约束标志数组，即0,1,2
                double[,] con_array = new double[NumofSpec, NumofSpec];

                #region 一类地物端元的计算
                for (int i = 0; i < NumofSpec; i++)
                {
                    //存储每一列RMSE的总和，用于计算EAR
                    double rmse_sum = 0;
                    //提取第i个端元光谱数据存入tmp_em1
                    //double[] tmp_em1 = ExtractEM(i, class_spec);
                    DenseMatrix EM1 = DenseMatrix.OfColumnVectors(class_spec.Column(i));
                    for (int j = 0; j < NumofSpec; j++)
                    {
                        //依次提取其他端元（包括本身）光谱数据存入tmp_em2
                        //double[] tmp_em2 = ExtractEM(j, class_spec);
                        //tmp_em1组成系数矩阵
                        //var EM1 = DenseMatrix.OfColumnArrays(tmp_em1);
                        //tmp_em2组成右侧向量
                        //var EM2 = new DenseVector(tmp_em2);
                        DenseVector EM2 = (DenseVector)class_spec.Column(j);
                        //利用SVD方法求解丰度Fraction参数
                        var Frac = EM1.Svd().Solve(EM2);

                        /*******根据约束类型，选择不同的Fraction值********/
                        double Frac_value = (Frac.ToArray())[0];//将矢量Frac转换为实数值
                        if (con >= 1)//如果con大于等于1，则说明启用约束设置
                        {
                            //创建临时丰度变量Frac_tmp存储丰度
                            double Frac_tmp = 0;
                            //查看最小值约束是否启用
                            if (EARminValuecheckBox.Checked == true)
                            {
                                //如果所求丰度值Frac_value小于所设置的最小值，则将最小阈值赋值给Frac_tmp,否则将原值Frac_Value赋值                           
                                double Frac_min = double.Parse(EARminValuetextBox.Text.ToString());
                                Frac_tmp = Frac_value > Frac_min ? Frac_value : Frac_min;
                            }
                            //查看最大值约束是否启用
                            if (EARmaxValuecheckBox.Checked == true)
                            {
                                //如果所求丰度值Frac_value大于所设置的最大值，则将最大阈值赋值给Frac_tmp,否则将原值Frac_Value赋值
                                double Frac_max = double.Parse(EARmaxValuetextBox.Text.ToString());
                                Frac_tmp = Frac_value < Frac_max ? Frac_value : Frac_max;
                            }
                            //如果最大值和最小值都没有启用，则将原值Frac_Value赋值给Frac_tmp
                            if (EARminValuecheckBox.Checked == false && EARmaxValuecheckBox.Checked == false)
                            {
                                Frac_tmp = Frac_value;
                            }
                            if (con == 1)//如果con等于1，则属于部分约束
                            {
                                if (Frac_tmp != Frac_value)
                                {
                                    con_array[j, i] = 1;//如果所求丰度值不在阈值范围内，则标记为1
                                    Frac_value = Frac_tmp;//将阈值赋值为原值，使用阈值计算下一参量，如RMSE
                                }
                            }
                            else//如果con等于2，则属于全部约束
                            {
                                if (Frac_tmp != Frac_value)
                                {
                                    con_array[j, i] = 2;//如果所求丰度值不在阈值范围内，则标记为2
                                }
                            }

                        }
                        //将最终的Frac_value值代入原式计算模型值
                        var model = EM1 * Frac_value;
                        //将模型值和真值比较计算残差
                        var resi = EM2.ToColumnMatrix() - model;
                        //将矩阵resi转化为数组,计算RMSE
                        var array_resi = resi.ToArray();
                        double resi_2_sum = 0;
                        for (int m = 0; m < NumofBands; m++)
                        {
                            resi_2_sum += Math.Pow(array_resi[m, 0], 2);
                        }
                        rmse_array[j, i] = Math.Round(Math.Sqrt(resi_2_sum / NumofBands), 3);
                        //如果RMSE选中，则检查所求RMSE是否符合阈值范围，不符合则将标识数组为赋值为2
                        if (con >= 1)
                        {
                            if (RMSEcheckBox.Checked == true && rmse_array[j, i] > double.Parse(RMSEnumericUp.Value.ToString()))
                                con_array[j, i] = 2;
                        }
                        //根据RMSE数组，求算同一类中每个端元的EAR
                        rmse_sum += rmse_array[j, i];
                        //更新端元收集器中的进度条
                        tmp_fatherform.UpdateProgess(i * NumofSpec + j + 1);

                    }//第i个端元分解另外一个端元遍历完毕
                    ear_array[i] = Math.Round(rmse_sum / (NumofSpec - 1), 4);
                }//所有端元分解其他所有端元遍历完毕

                var rmse_matrix = DenseMatrix.OfArray(rmse_array);
                MessageBox.Show(rmse_matrix.ToString(), "RMSE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                var ear_matrix = DenseMatrix.OfColumnArrays(ear_array);
                MessageBox.Show(ear_matrix.ToString(), "EAR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                var cons_matrix = DenseMatrix.OfArray(con_array);
                MessageBox.Show(cons_matrix.ToString(), "Constraint", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //将EAR数组传回端元收集器
                tmp_fatherform.Add_EAR(ear_array);
                //关闭EAR窗口
                this.Close();
                # endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }
        }

        //将第i列数据存储为一个临时数组
        public double[] ExtractEM(int m, double[,] tmp_spec)
        {
            int v_row = tmp_spec.GetLength(0);
            int v_column = tmp_spec.GetLength(1);
            //创建临时数组存储第i列的光谱数据
            double[] tmp_array = new double[v_row];
            for (int i = 0; i < v_row; i++)
            {
                tmp_array[i] = tmp_spec[i, m];
            }
            return tmp_array;
        }

        //求数组平均值
        static double Array_Average(double[] vals)
        {
            double sum = 0;
            int num = vals.Length;
            foreach (double val in vals)
            { sum += val; }
            return sum / num;
        }

        //无约束按钮为选中状态时的事件
        private void UnconsRadiobt_CheckedChanged(object sender, EventArgs e)
        {
            //无约束按钮启用
            if (UnconsRadiobt.Checked == true)
            {
                con = 0;
                EARminValuetrackBar.Enabled = false;
                EARminValuetextBox.Enabled = false;
                EARminValuecheckBox.Checked = false;
                EARminValuecheckBox.Enabled = false;
                EARmaxValuetrackBar.Enabled = false;
                EARmaxValuetextBox.Enabled = false;
                EARmaxValuecheckBox.Enabled = false;
                EARmaxValuecheckBox.Checked = false;
                RMSEnumericUp.Enabled = false;
                RMSEcheckBox.Enabled = false;
                RMSEcheckBox.Checked = false;
            }
        }

        //部分约束按钮为选中状态时的事件
        private void PartialconsRadiobt_CheckedChanged(object sender, EventArgs e)
        {
            //约束设置按钮启用
            if (PartialconsRadiobt.Checked == true)
            {
                con = 1;
                EARminValuecheckBox.Enabled = true;
                EARmaxValuecheckBox.Enabled = true;
                RMSEcheckBox.Enabled = true;
            }
        }

        //全约束按钮为选中状态时的事件
        private void FullconsRadiobt_CheckedChanged(object sender, EventArgs e)
        {
            //约束设置按钮启用
            if (FullconsRadiobt.Checked == true)
            {
                con = 2;
                EARminValuecheckBox.Enabled = true;
                EARmaxValuecheckBox.Enabled = true;
                RMSEcheckBox.Enabled = true;
            }
        }

        //取消按钮事件
        private void EARCancelbt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //最小值丰度约束按钮启用事件
        private void EARminValuecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EARminValuecheckBox.Checked == true)
            {
                EARminValuetrackBar.Enabled = true;
                EARminValuetextBox.Enabled = true;
            }
            else
            {
                EARminValuetrackBar.Enabled = false;
                EARminValuetextBox.Enabled = false;
            }

        }
        //最大值丰度约束按钮启用事件
        private void EARmaxValuecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EARmaxValuecheckBox.Checked == true)
            {
                EARmaxValuetrackBar.Enabled = true;
                EARmaxValuetextBox.Enabled = true;
            }
            else
            {
                EARmaxValuetrackBar.Enabled = false;
                EARmaxValuetextBox.Enabled = false;
            }
        }

        //RMSE约束按钮启用事件
        private void RMSEcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RMSEcheckBox.Checked == true)
                RMSEnumericUp.Enabled = true;
            else
                RMSEnumericUp.Enabled = false;
        }

        //最小值滑动条滚动取值事件
        private void EARminValuetrackBar_Scroll(object sender, EventArgs e)
        {
            this.EARminValuetextBox.Text = (this.EARminValuetrackBar.Value / 100.0).ToString();
        }

        //最小值滑动条滚动取值事件
        private void EARmaxValuetrackBar_Scroll(object sender, EventArgs e)
        {
            this.EARmaxValuetextBox.Text = (this.EARmaxValuetrackBar.Value / 100.0).ToString();
        }

    }
}
