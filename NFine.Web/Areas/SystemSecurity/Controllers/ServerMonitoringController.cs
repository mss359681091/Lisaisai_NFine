/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: Lss
 * Description: NFine快速开发平台
 * Website：http://blog.csdn.net/mss359681091
*********************************************************************************/
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public struct WeekInfo
    {
        public CPU dataCpu;
        public ARM dataArm;
    }
    public struct CPU
    {
        public int cpu1;
        public int cpu2;
        public int cpu3;
        public int cpu4;
        public int cpu5;
        public int cpu6;
        public int cpu7;
    }
    public struct ARM
    {
        public int arm1;
        public int arm2;
        public int arm3;
        public int arm4;
        public int arm5;
        public int arm6;
        public int arm7;
    }
    public struct SERVER_INFO
    {
        public string osTitle;
        public string osVersion;
        public string serverIIS;
        public string serverIP;
        public string userName;
    }
    //定义内存的信息结构    
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }
    public class ServerMonitoringController : ControllerBase
    {
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        public static PerformanceCounter cpu;

        //获取CUP使用率
        public uint GetCpuInfo()
        {
            cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var percentage = cpu.NextValue();
            while (true)
            {
                percentage = cpu.NextValue();
                if (percentage > 0 && percentage < 100)
                {
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }
            return (uint)percentage;
        }
        //获取ARM使用率
        public uint GetArmInfo()
        {
            MEMORY_INFO MemInfo;
            MemInfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref MemInfo);
            return MemInfo.dwMemoryLoad;
        }
        ////获取一周的CPU和内存使用率报表
        //public string GetWeekInfo()
        //{
        //    WeekInfo weekInfo = new WeekInfo();
        //    DateTime dt = DateTime.Now;
        //    string caseDay = dt.DayOfWeek.ToString();
        //    int caseNum = 0;
        //    switch (caseDay)
        //    {
        //        case "Monday":
        //            caseNum = 1;
        //            break;
        //        case "Tuesday":
        //            caseNum = 2;
        //            break;
        //        case "Wednesday":
        //            caseNum = 3;
        //            break;
        //        case "Thursday":
        //            caseNum = 4;
        //            break;
        //        case "Friday":
        //            caseNum = 5;
        //            break;
        //        case "Saturday":
        //            caseNum = 6;
        //            break;
        //        case "Sunday":
        //            caseNum = 7;
        //            break;
        //    }
        //    weekInfo.dataCpu.cpu1 = caseNum > 1 ? 35 : (caseNum == 1 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataCpu.cpu2 = caseNum > 2 ? 25 : (caseNum == 2 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataCpu.cpu3 = caseNum > 3 ? 31 : (caseNum == 3 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataCpu.cpu4 = caseNum > 4 ? 45 : (caseNum == 4 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataCpu.cpu5 = caseNum > 5 ? 15 : (caseNum == 5 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataCpu.cpu6 = caseNum > 6 ? 66 : (caseNum == 6 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataCpu.cpu7 = caseNum > 7 ? 10 : (caseNum == 7 ? (int)GetCpuInfo() : 0);
        //    weekInfo.dataArm.arm1 = caseNum > 1 ? 43 : (caseNum == 1 ? (int)GetArmInfo() : 0);
        //    weekInfo.dataArm.arm2 = caseNum > 2 ? 40 : (caseNum == 2 ? (int)GetArmInfo() : 0);
        //    weekInfo.dataArm.arm3 = caseNum > 3 ? 51 : (caseNum == 3 ? (int)GetArmInfo() : 0);
        //    weekInfo.dataArm.arm4 = caseNum > 4 ? 47 : (caseNum == 4 ? (int)GetArmInfo() : 0);
        //    weekInfo.dataArm.arm5 = caseNum > 5 ? 39 : (caseNum == 5 ? (int)GetArmInfo() : 0);
        //    weekInfo.dataArm.arm6 = caseNum > 6 ? 49 : (caseNum == 6 ? (int)GetArmInfo() : 0);
        //    weekInfo.dataArm.arm7 = caseNum > 7 ? 45 : (caseNum == 7 ? (int)GetArmInfo() : 0);
        //    return weekInfo.ToJson();
        //}
        //获取服务器信息
        public string GetServerInfo()
        {
            SERVER_INFO serverInfo = new SERVER_INFO();
            //获取服务器名
            serverInfo.osTitle = Server.MachineName;
            //操作系统版本 
            serverInfo.osVersion = Environment.OSVersion.ToString();
            //获取服务器IIS版本
            serverInfo.serverIIS = Request.ServerVariables["SERVER_SOFTWARE"];
            //获取服务器IP
            serverInfo.serverIP = this.GetLocalIP();
            //用户名
            serverInfo.userName = Environment.UserName;
            return serverInfo.ToJson();
        }
        public string GetLocalIP()
        {
            string result = RunApp("route", "print", true);
            Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            if (m.Success)
            {
                return m.Groups[2].Value;
            }
            else
            {
                try
                {
                    System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient();
                    c.Connect("www.baidu.com", 80);
                    string ip = ((System.Net.IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
                    c.Close();
                    return ip;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        /// <summary>  
        /// 运行一个控制台程序并返回其输出参数。  
        /// </summary>  
        /// <param name="filename">程序名</param>  
        /// <param name="arguments">输入参数</param>  
        /// <returns></returns>  
        public string RunApp(string filename, string arguments, bool recordLog)
        {
            try
            {
                if (recordLog)
                {
                    Trace.WriteLine(filename + " " + arguments);
                }
                Process proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    //string txt = sr.ReadToEnd();  
                    //sr.Close();  
                    //if (recordLog)  
                    //{  
                    //    Trace.WriteLine(txt);  
                    //}  
                    //if (!proc.HasExited)  
                    //{  
                    //    proc.Kill();  
                    //}  
                    //上面标记的是原文，下面是我自己调试错误后自行修改的  
                                                 //貌似调用系统的nslookup还未返回数据或者数据未编码完成，程序就已经跳过直接执行  
                                                 //txt = sr.ReadToEnd()了，导致返回的数据为空，故睡眠令硬件反应  
                    if (!proc.HasExited)         //在无参数调用nslookup后，可以继续输入命令继续操作，如果进程未停止就直接执行  
                    {                            //txt = sr.ReadToEnd()程序就在等待输入，而且又无法输入，直接掐住无法继续运行  
                        proc.Kill();
                    }
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    if (recordLog)
                        Trace.WriteLine(txt);
                    return txt;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.Message;
            }
        }
    }
}
