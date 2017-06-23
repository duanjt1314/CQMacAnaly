using PluginKernel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacListen
{
    [PlugInImplements(typeof(IPlugIn))]
    public class Listen : PlugInBase
    {
        public override string CallPlugIn(object obj)
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            if (obj is DataSet)
            {
                DataSet set = obj as DataSet;
                DataTable table = set.Tables[0];
                if (table.TableName.ToLower() == "terminaltrace")
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (Config.GetIntall().Macs.Contains(row["terminal_mac"].ToString()))
                        {
                            sb.AppendFormat("{0}\t{1}\t{2}\r\n", row["terminal_mac"].ToString(), row["detect_time"].ToString(), row["site_id"].ToString());
                        }
                    }

                    //写入文件
                    if (sb.Length > 0)
                    {
                        File.AppendAllText("F:/分析.txt", sb.ToString());
                    }
                }
            }
            return result;
        }

        public override void Dispose()
        {

        }

        public override string GetRunningMsg()
        {
            return "";
        }
    }
}
