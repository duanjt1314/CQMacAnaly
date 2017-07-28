using Ionic.Utils.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CQMacAnaly.bean;
using Newtonsoft.Json;
using System.Diagnostics;
using HZ.Common;

namespace CQMacAnaly
{
    public partial class MainForm : Form
    {
        //String connStr = ConfigurationManager.AppSettings["connStr"];
        //需要分析的对象的集合，DataTable
        List<MacObj> anList = new List<MacObj>();
        Dictionary<string, List<MacObj>> dicList = new Dictionary<string, List<MacObj>>();

        public MainForm()
        {
            InitializeComponent();
        }

        //查找MAC采集情况
        private void btnStartMAC_Click(object sender, EventArgs e)
        {
            string dataPath = txtDataPath.Text;
            string tempPath = txtTmpPath.Text;
            if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(tempPath))
            {
                MessageBox.Show("必须选择数据文件目录！");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_mac.Text))
            {
                MessageBox.Show("必须输入MAC！");
                return;
            }
            Thread thread = new Thread(new ParameterizedThreadStart(FindMAC));
            thread.IsBackground = true;
            thread.Start(new FindMacEntity() { DataPath = dataPath, TempPath = tempPath, MacList = txt_mac.Text.Split(',').ToList() });
        }

        //MAC伴随分析
        private void btnAnalyMac_Click(object sender, EventArgs e)
        {
            string dataPath = txtDataPath.Text;
            string tempPath = txtTmpPath.Text;
            if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(tempPath))
            {
                MessageBox.Show("必须选择数据文件目录！");
                return;
            }

            string anPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "分析内容.txt");
            if (!File.Exists(anPath))
            {
                MessageBox.Show("没有找到需要分析的MAC文件:" + anPath);
                return;
            }

            var anTable = Methods.String2DataTable(File.ReadAllText(anPath));
            foreach (DataRow row in anTable.Rows)
            {
                var site_id = row["site_id"].ToString();
                var terminal_mac = row["terminal_mac"].ToString();
                var detect_time = Convert.ToInt64(row["detect_time"]);
                anList.Add(new MacObj()
                {
                    SiteId = site_id,
                    TerminalMac = terminal_mac,
                    DetectTime = detect_time
                });
            }

            Thread thread = new Thread(new ParameterizedThreadStart(FindTogether));
            thread.IsBackground = true;
            thread.Start(new FindMacEntity() { DataPath = dataPath, TempPath = tempPath, MacList = txt_mac.Text.Split(',').ToList() });
        }

        private void btnSelectDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtDataPath.Text = fd.SelectedPath;
            }
        }

        private void btnSelectTmpPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtTmpPath.Text = fd.SelectedPath;
            }
        }


        private void FindMAC(object obj)
        {
            try
            {
                FindMacEntity entity = obj as FindMacEntity;
                Dictionary<string, string> logDic = new Dictionary<string, string>();
                //查找 ZIP文件列表，并解压到临时目录处理
                var files = Directory.EnumerateFiles(entity.DataPath, "*", SearchOption.TopDirectoryOnly);
                files.ToList().ForEach(file =>
                {
                    var extPath = Path.Combine(entity.TempPath, Path.GetFileNameWithoutExtension(file));

                    Disply("开始解压文件" + file + "");
                    using (ZipFile zipFile = new ZipFile(file))
                    {
                        zipFile.ExtractAll(extPath);
                    }
                    Disply("文件" + file + "解压成功，开始分析数据文件，该操作耗时较长，请耐心等待...");
                    //只获取终端特征信息的文件
                    var zipFiles = Directory.EnumerateFiles(extPath, "*_WifiTerminalInfoLog_*", SearchOption.TopDirectoryOnly);
                    zipFiles.ToList().ForEach(macFile =>
                    {
                        try
                        {
                            string data = Methods.ReadGzip(File.ReadAllBytes(macFile));
                            DataTable dt = Methods.String2DataTable(data);

                            var row = dt.AsEnumerable().Where(x =>
                            {
                                return (from a in entity.MacList where a.Equals(x.Field<string>("terminal_mac"), StringComparison.OrdinalIgnoreCase) select a).Any();
                            });

                            row.ToList().ForEach(r =>
                            {
                                string siteId = r["site_id"].ToString();
                                string mac = r["terminal_mac"].ToString();
                                string time = Methods.ConvertIntDateTime(long.Parse(r["detect_time"].ToString())).ToString();
                                string log = string.Format("被{0}在{1}采集到，gz数据文件为{2},压缩包文件为{3}", siteId, time, Path.GetFileName(macFile), Path.GetFileName(file));
                                if (logDic.ContainsKey(mac))
                                {
                                    logDic[mac] = logDic[mac] + Environment.NewLine + log;
                                }
                                else
                                {
                                    logDic.Add(mac, log);
                                }
                            });

                        }
                        catch (Exception ex)
                        {
                            LogWriter.WriteLog("错误,文件名:" + macFile + "\r\n" + ex.ToString());
                        }
                    });
                });

                foreach (string ss in logDic.Keys)
                {
                    LogWriter.WriteLog(ss + Environment.NewLine + logDic[ss]);
                }
                Disply("分析完成，请到程序根目录下查看XXX.LOG日志文件查看结果");
                logDic = new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog("错误:" + ex.ToString());
            }

        }

        private void FindTogether(object obj)
        {
            FindMacEntity entity = obj as FindMacEntity;
            Dictionary<string, string> logDic = new Dictionary<string, string>();
            //查找 ZIP文件列表，并解压到临时目录处理
            var files = Directory.EnumerateFiles(entity.DataPath, "*", SearchOption.TopDirectoryOnly);
            files.ToList().ForEach(file =>
            {
                Disply("开始解压文件" + file + "");
                using (ZipFile zipFile = new ZipFile(file))
                {
                    zipFile.ExtractAll(entity.TempPath + "/" + Path.GetFileNameWithoutExtension(file), true);
                }
                Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待...");
                //只获取终端特征信息的文件
                var zipFiles = Directory.EnumerateFiles(entity.TempPath + "/" + Path.GetFileNameWithoutExtension(file), "*_WifiTerminalInfoLog_*", SearchOption.TopDirectoryOnly);
                int i = 0;
                int count = zipFiles.Count();
                zipFiles.ToList().ForEach(macFile =>
                {
                    try
                    {
                        i++;
                        Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待..." + "当前步骤" + i + "/" + count);
                        string data = Methods.ReadGzip(File.ReadAllBytes(macFile));
                        DataTable dt = Methods.String2DataTable(data);
                        foreach (DataRow row in dt.Rows)
                        {
                            var site_id = row["site_id"].ToString();
                            var terminal_mac = row["terminal_mac"].ToString();
                            var detect_time = Convert.ToInt64(row["detect_time"]);

                            foreach (var item in anList)
                            {
                                var minValue = item.DetectTime - this.nudSecond.Value;
                                var maxValue = item.DetectTime + this.nudSecond.Value;
                                if (site_id == item.SiteId
                                    && detect_time >= minValue
                                    && detect_time <= maxValue)
                                {
                                    //同行
                                    if (dicList.ContainsKey(item.TerminalMac))
                                    {
                                        var list = dicList[item.TerminalMac];
                                        var macObj = list.SingleOrDefault(f => f.TerminalMac == terminal_mac && f.SiteId == site_id);
                                        if (macObj == null)
                                        {
                                            list.Add(new MacObj()
                                            {
                                                SiteId = site_id,
                                                TerminalMac = terminal_mac,
                                                Times = 1
                                            });
                                        }
                                        else
                                        {
                                            macObj.Times += 1;
                                        }
                                        dicList[item.TerminalMac] = list;
                                    }
                                    else
                                    {
                                        dicList.Add(item.TerminalMac, new List<MacObj>()
                                        {
                                            new MacObj(){
                                                SiteId=site_id,
                                                TerminalMac=terminal_mac,
                                                Times=1
                                            }                                            
                                        });
                                    }
                                }
                            }
                        }

                    }
                    catch
                    {

                    }
                });
            });

            //写入
            foreach (var key in dicList.Keys)
            {
                var item = dicList[key];
                StringBuilder sb = new StringBuilder();

                var group = item.GroupBy(f => f.TerminalMac).Select(k => new { mac = k.Key, times = k.Sum(l => l.Times) });
                foreach (var groupItem in group.OrderByDescending(f => f.times))
                {
                    if (groupItem.times >= this.nudTimes.Value)
                    {
                        var detail = item.Where(f => f.TerminalMac == groupItem.mac).GroupBy(f => f.SiteId).Select(k => new { siteId = k.Key, times = k.Sum(l => l.Times) });

                        sb.AppendFormat("mac:{0}和mac{1}总共同行了{2}次。", key, groupItem.mac, groupItem.times);

                        foreach (var detailItem in detail)
                        {
                            sb.AppendFormat("在场所{0}同行了{1}次。", detailItem.siteId, detailItem.times);
                        }
                        sb.AppendFormat("\r\n");
                    }
                }

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "同行分析");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllText(Path.Combine(path, key + ".txt"), sb.ToString());
            }
            Disply("文件分析完成,结果保存在根目录");
        }

        delegate void DelegateDisply(string text);
        private void Disply(string text)
        {
            DelegateDisply gwl = p =>
            {

                lblInfo.Text = text;
                this.lblInfo.Focus();//获取焦点
            };
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new DelegateDisply(gwl), text);
            }
        }

        class FindMacEntity
        {
            public string DataPath { get; set; }
            public string TempPath { get; set; }
            public List<string> MacList { get; set; }
        }

        class MacObj
        {
            public string SiteId { get; set; }
            public string TerminalMac { get; set; }
            public long DetectTime { get; set; }
            public long Times { get; set; }
        }

        class MacCount
        {
            public string DataPath { get; set; }
            public string TempPath { get; set; }
            public long BeginTime { get; set; }
            public long EndTime { get; set; }
            public String SiteIds { get; set; }
        }


        //解析日志文件,格式是mac,时间，场所编码
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFileName.Text))
            {
                MessageBox.Show("请选择要转换的文件");
                return;
            }
            var table = Methods.String2DataTable(File.ReadAllText(this.txtFileName.Text));

            StringBuilder sb = new StringBuilder("terminal_mac\tdetect_time\tsite_id\r\n");

            foreach (DataRow row in table.Rows)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\r\n", row["terminal_mac"].ToString(),
                    Methods.ConvertDateTimeInt(Convert.ToDateTime(row["detect_time"].ToString())),
                    row["site_id"].ToString());
            }
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + "转换结果.txt", sb.ToString());
        }

        private void txtSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                this.txtFileName.Text = dialog.FileName;
            }
        }

        //查找目标MAC出现的时间和场所,并保存到本地文件
        private void button2_Click(object sender, EventArgs e)
        {
            string dataPath = txtDataPath.Text;
            string tempPath = txtTmpPath.Text;
            if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(tempPath))
            {
                MessageBox.Show("必须选择数据文件目录！");
                return;
            }

            string anPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "分析内容.txt");
            if (!File.Exists(anPath))
            {
                MessageBox.Show("没有找到需要分析的MAC文件:" + anPath);
                return;
            }

            Thread thread = new Thread(new ParameterizedThreadStart(FindTogetherMac));
            thread.IsBackground = true;
            thread.Start(new FindMacEntity() { DataPath = dataPath, TempPath = tempPath, MacList = txt_mac.Text.Split(',').ToList() });
        }

        private void FindTogetherMac(object obj)
        {
            List<MacObj> list = new List<MacObj>();
            FindMacEntity entity = obj as FindMacEntity;
            Dictionary<string, string> logDic = new Dictionary<string, string>();
            //查找 ZIP文件列表，并解压到临时目录处理
            var files = Directory.EnumerateFiles(entity.DataPath, "*", SearchOption.TopDirectoryOnly);
            files.ToList().ForEach(file =>
            {
                Disply("开始解压文件" + file + "");
                using (ZipFile zipFile = new ZipFile(file))
                {
                    zipFile.ExtractAll(entity.TempPath + "/" + Path.GetFileNameWithoutExtension(file), true);
                }
                Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待...");
                //只获取终端特征信息的文件
                var zipFiles = Directory.EnumerateFiles(entity.TempPath + "/" + Path.GetFileNameWithoutExtension(file), "*_WifiTerminalInfoLog_*", SearchOption.TopDirectoryOnly);
                int i = 0;
                int count = zipFiles.Count();
                zipFiles.ToList().ForEach(macFile =>
                {
                    try
                    {
                        i++;
                        Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待..." + "当前步骤" + i + "/" + count);
                        string data = Methods.ReadGzip(File.ReadAllBytes(macFile));
                        DataTable dt = Methods.String2DataTable(data);
                        foreach (DataRow row in dt.Rows)
                        {
                            var mac = row["terminal_mac"].ToString();
                            if (this.textBox1.Text.Contains(mac))
                            {
                                list.Add(new MacObj()
                                {
                                    SiteId = row["site_id"].ToString(),
                                    DetectTime = Convert.ToInt64(row["detect_time"]),
                                    TerminalMac = row["terminal_mac"].ToString(),
                                });
                            }
                        }

                    }
                    catch
                    {

                    }
                });
            });

            StringBuilder sb = new StringBuilder("terminal_mac\tdetect_time\tsite_id\r\n");
            foreach (var item in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\r\n", item.TerminalMac,
                    item.DetectTime,
                    item.SiteId);
            }
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + "查找MAC转换结果.txt", sb.ToString());
            Disply("文件分析完成,结果保存在根目录");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dataPath = txtDataPath.Text;
            string tempPath = txtTmpPath.Text;
            if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(tempPath))
            {
                MessageBox.Show("必须选择数据文件目录！");
                return;
            }

            Thread thread = new Thread(new ParameterizedThreadStart(FindMacCount));
            thread.IsBackground = true;
            thread.Start(new MacCount
            {
                DataPath = dataPath,
                TempPath = tempPath,
                BeginTime = Methods.ConvertDateTimeInt(this.dtpBegin.Value),
                EndTime = Methods.ConvertDateTimeInt(this.dtpEnd.Value),
                SiteIds = this.textBox2.Text
            });
        }


        private void FindMacCount(object obj)
        {
            //外层的键表示场所编码,内层的键表示具体的MAC
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            MacCount macCount = (MacCount)obj;
            //查找 ZIP文件列表，并解压到临时目录处理
            var files = Directory.EnumerateFiles(macCount.DataPath, "*", SearchOption.TopDirectoryOnly);
            files.ToList().ForEach(file =>
            {
                Disply("开始解压文件" + file + "");
                using (ZipFile zipFile = new ZipFile(file))
                {
                    zipFile.ExtractAll(macCount.TempPath + "/" + Path.GetFileNameWithoutExtension(file), true);
                }
                Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待...");
                //只获取终端特征信息的文件
                var zipFiles = Directory.EnumerateFiles(macCount.TempPath + "/" + Path.GetFileNameWithoutExtension(file), "*_WifiTerminalInfoLog_*", SearchOption.TopDirectoryOnly);
                int i = 0;
                int count = zipFiles.Count();
                zipFiles.ToList().ForEach(macFile =>
                {
                    try
                    {
                        i++;
                        Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待..." + "当前步骤" + i + "/" + count);
                        string data = Methods.ReadGzip(File.ReadAllBytes(macFile));
                        DataTable dt = Methods.String2DataTable(data);
                        foreach (DataRow row in dt.Rows)
                        {
                            var mac = row["terminal_mac"].ToString();
                            var detect_time = Convert.ToInt64(row["detect_time"].ToString());
                            var site_id = row["site_id"].ToString();

                            if (detect_time >= macCount.BeginTime && detect_time <= macCount.EndTime && macCount.SiteIds.Contains(site_id))
                            {
                                if (dic.ContainsKey(site_id))
                                {
                                    var macs = dic[site_id];
                                    if (!macs.ContainsKey(mac))
                                    {
                                        macs.Add(mac, null);
                                    }
                                }
                                else
                                {
                                    Dictionary<string, string> macs = new Dictionary<string, string>();
                                    macs.Add(mac, null);
                                    dic.Add(site_id, macs);
                                }

                            }

                        }

                    }
                    catch
                    {

                    }
                });
            });

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("场所去重采集数据统计:");
            sb.AppendLine("场所编码\t采集数量");
            foreach (var key in dic.Keys)
            {
                sb.AppendLine(key + "\t" + dic[key].Count);
            }

            String fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + "去重统计" + macCount.BeginTime.ToString() + " - " + macCount.EndTime.ToString() + ".txt";
            File.WriteAllText(fileName, sb.ToString());
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
            Disply("文件分析完成,数据保存在:" + fileName);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string dataPath = txtDataPath.Text;
            string tempPath = txtTmpPath.Text;
            if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(tempPath))
            {
                MessageBox.Show("必须选择数据文件目录！");
                return;
            }

            Thread thread = new Thread(new ParameterizedThreadStart(FindDetectCount));
            thread.IsBackground = true;
            thread.Start(new MacCount
            {
                DataPath = dataPath,
                TempPath = tempPath,
                BeginTime = Methods.ConvertDateTimeInt(this.dateTimePicker2.Value),
                EndTime = Methods.ConvertDateTimeInt(this.dateTimePicker1.Value),
                SiteIds = this.textBox3.Text
            });
        }

        private void FindDetectCount(object obj)
        {
            //外层的键表示场所编码,内层的键表示具体的MAC
            Dictionary<string, int> dic = new Dictionary<string, int>();
            MacCount macCount = (MacCount)obj;
            //查找 ZIP文件列表，并解压到临时目录处理
            var files = Directory.EnumerateFiles(macCount.DataPath, "*", SearchOption.TopDirectoryOnly);
            files.ToList().ForEach(file =>
            {
                Disply("开始解压文件" + file + "");
                using (ZipFile zipFile = new ZipFile(file))
                {
                    zipFile.ExtractAll(macCount.TempPath + "/" + Path.GetFileNameWithoutExtension(file), true);
                }
                Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待...");
                //只获取终端特征信息的文件
                var zipFiles = Directory.EnumerateFiles(macCount.TempPath + "/" + Path.GetFileNameWithoutExtension(file), "*_WifiTerminalInfoLog_*", SearchOption.TopDirectoryOnly);
                int i = 0;
                int count = zipFiles.Count();
                zipFiles.ToList().ForEach(macFile =>
                {
                    try
                    {
                        i++;
                        Disply("文件" + file + "解压成功，开始数据分析，该操作耗时较长，请耐心等待..." + "当前步骤" + i + "/" + count);
                        string data = Methods.ReadGzip(File.ReadAllBytes(macFile));
                        DataTable dt = Methods.String2DataTable(data);
                        foreach (DataRow row in dt.Rows)
                        {
                            var mac = row["terminal_mac"].ToString();
                            var detect_time = Convert.ToInt64(row["detect_time"].ToString());
                            var site_id = row["site_id"].ToString();

                            if (detect_time >= macCount.BeginTime && detect_time <= macCount.EndTime && macCount.SiteIds.Contains(site_id))
                            {
                                if (dic.ContainsKey(site_id))
                                {
                                    var num = dic[site_id];
                                    dic[site_id] = num + 1;
                                }
                                else
                                {
                                    dic.Add(site_id, 1);
                                }
                            }
                        }

                    }
                    catch
                    {

                    }
                });
            });

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("场所采集数据统计:");
            sb.AppendLine("场所编码\t采集数量");
            foreach (var key in dic.Keys)
            {
                sb.AppendLine(key + "\t" + dic[key]);
            }

            String fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + "采集统计" + macCount.BeginTime.ToString() + " - " + macCount.EndTime.ToString() + ".txt";
            File.WriteAllText(fileName, sb.ToString());
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
            Disply("文件分析完成,数据保存在:" + fileName);
        }

        //MAC采集和去重统计,使用多线程来跑
        private void button5_Click(object sender, EventArgs e)
        {
            string dataPath = txtDataPath.Text;
            string tempPath = txtTmpPath.Text;
            if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(tempPath))
            {
                MessageBox.Show("必须选择数据文件目录！");
                return;
            }

            Thread thread = new Thread(new ParameterizedThreadStart(FindDetectCount2));
            thread.IsBackground = true;
            thread.Start(new MacCount
            {
                DataPath = dataPath,
                TempPath = tempPath,
                BeginTime = Methods.ConvertDateTimeInt(this.dateTimePicker4.Value),
                EndTime = Methods.ConvertDateTimeInt(this.dateTimePicker3.Value),
                SiteIds = this.textBox4.Text
            });
        }

        private object lockObj = new object();

        private void FindDetectCount2(object obj)
        {
            int fileCount = 0;
            List<TerminalInfo> terminalInfos = new List<TerminalInfo>();
            MacCount macCount = (MacCount)obj;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //查找 ZIP文件列表，并解压到临时目录处理
            var files = Directory.EnumerateFiles(macCount.DataPath, "*", SearchOption.TopDirectoryOnly);
            List<Task> tasks = new List<Task>();
            files.ToList().ForEach(file =>
            {
                Task task = Task.Factory.StartNew((f) =>
                {
                    Disply("开始解压文件" + file + "");
                    using (ZipFile zipFile = new ZipFile(file))
                    {
                        zipFile.ExtractAll(macCount.TempPath + "/" + Path.GetFileNameWithoutExtension(file), true);
                    }

                    //只获取终端特征信息的文件
                    var zipFiles = Directory.EnumerateFiles(macCount.TempPath + "/" + Path.GetFileNameWithoutExtension(file), "*_WifiTerminalInfoLog_*", SearchOption.TopDirectoryOnly);
                    int count = zipFiles.Count();
                    zipFiles.ToList().ForEach(macFile =>
                    {
                        try
                        {
                            string data = Methods.ReadGzip(File.ReadAllBytes(macFile));
                            DataTable dt = Methods.String2DataTable(data);

                            //锁住
                            lock (lockObj)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    var mac = row["terminal_mac"].ToString();
                                    var detect_time = row["detect_time"].ToString().GetInt64();
                                    var site_id = row["site_id"].ToString();

                                    if (detect_time >= macCount.BeginTime && detect_time <= macCount.EndTime && macCount.SiteIds.Contains(site_id))
                                    {
                                        terminalInfos.Add(new TerminalInfo()
                                        {
                                            SiteId = site_id,
                                            MAC = mac
                                        });
                                    }
                                }
                                fileCount++;
                            }

                            Disply("文件:" + macFile + "处理成功,即将删除");
                            File.Delete(macFile);

                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log.Error("", ex);
                        }
                    });

                }, file);
                tasks.Add(task);

                if (tasks.Count >= 3)
                {
                    Task.WaitAll(tasks.ToArray());
                    tasks.Clear();
                }

            });

            if (tasks.Count > 0)
                Task.WaitAll(tasks.ToArray());

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("场所采集数据统计:");
            sb.AppendLine("场所编码\t采集数量\t去重数量");

            foreach (var item in macCount.SiteIds.Split(','))
            {
                var caiji = terminalInfos.Count(f => f.SiteId == item);
                var quchong = terminalInfos.Where(f => f.SiteId == item).Select(f => f.MAC).Distinct().Count();
                //打印出来看看
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(terminalInfos.Where(f => f.SiteId == item).ToList()));

                sb.AppendLine(item + "\t" + caiji + "\t" + quchong);
            }

            watch.Stop();
            sb.AppendLine("共处理gz文件" + fileCount + "个,耗时:" + watch.ElapsedMilliseconds + "毫秒");

            String fileName = AppDomain.CurrentDomain.BaseDirectory + "\\"
                + "采集统计"
                + Methods.ConvertIntDateTime(macCount.BeginTime).ToString("yyyyMMddHHmmss")
                + " - "
                + Methods.ConvertIntDateTime(macCount.EndTime).ToString("yyyyMMddHHmmss")
                + ".txt";
            File.WriteAllText(fileName, sb.ToString());
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
            Disply("文件分析完成,数据保存在:" + fileName);
        }
    }
}
