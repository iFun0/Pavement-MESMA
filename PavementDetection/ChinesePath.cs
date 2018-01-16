using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PavementDetection
{
    class ChinesePath
    {
        //检查是否包含中文路径及文件名
        public static bool IsChinaorSpace(string CString)
        {
            bool BoolValue = true;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = false;
                }
                else
                {
                    MessageBox.Show("路径和文件名中都不能含有中文字符", " 错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return BoolValue = true;
                }
            }
            string trim = Regex.Replace(CString, @"\s", "");
            if (trim.Length < CString.Length)
            {
                MessageBox.Show("路径及文件名中都不能含有空白字符", " 错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return BoolValue = true;
            }
            else
            {
                BoolValue = false;
            }
            return BoolValue;
        }
    }
}
