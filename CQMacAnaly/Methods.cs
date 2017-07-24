using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CQMacAnaly
{
    class Methods
    {
        public static string ReadGzip(byte[] bytes, string encoding = "GB2312")
        {
            string result = string.Empty;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (GZipStream decompressedStream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    using (StreamReader sr = new StreamReader(decompressedStream, Encoding.GetEncoding(encoding)))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            return result;
        }

        public static DataTable String2DataTable(String str, string tableName = "DataTable", string colSplit = "\t", string rowSplit = "\r\n")
        {
            DataTable dtResult = new DataTable();
            dtResult.TableName = tableName;
            var rows = Regex.Split(str, rowSplit);
            for (int i = 0; i < rows.Length; i++)
            {
                if (i == 0)
                {
                    //定义列
                    var columns = Regex.Split(rows[0], colSplit);
                    foreach (var column in columns)
                    {
                        dtResult.Columns.Add(column);
                    }
                }
                else
                {
                    //填充数据
                    if (String.IsNullOrEmpty(rows[i]))
                    {
                        continue;
                    }
                    var columns = Regex.Split(rows[i], colSplit);
                    var c = columns.Take(dtResult.Columns.Count).ToArray();
                    dtResult.Rows.Add(c);
                }
            }
            return dtResult;
        }

        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = new System.DateTime(1970, 1, 1, 8, 0, 0);
            time = startTime.AddSeconds(d);
            return time;
        }

        public static long ConvertDateTimeInt(DateTime time)
        {
            TimeSpan ts = (TimeSpan)(time - new DateTime(1970, 1, 1, 8, 0, 0));
            return Int64.Parse(Math.Ceiling(ts.TotalSeconds) + "");
        }
    }
}
