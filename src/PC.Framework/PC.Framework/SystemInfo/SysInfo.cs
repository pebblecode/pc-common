using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Diagnostics;
using PebbleCode.Framework.Logging;
namespace PebbleCode.Framework.SystemInfo
{
    public class SysInfo
    {
        public string GetRamDiskUsage()
        {
            try
            {
                StringBuilder infoBuilder = new StringBuilder();

                //Write available RAM:
                PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                float availableMbs = ramCounter.NextValue();
                infoBuilder.Append(string.Format("Available RAM: {0:0.00}Mb\r\n", availableMbs));

                Process process = Process.GetCurrentProcess();
                infoBuilder.Append(string.Format("{0} : {1:0.00}\r\n", "Virtual Memory MBs", BytesToMb(process.VirtualMemorySize64)));
                infoBuilder.Append(string.Format("{0} : {1:0.00}\r\n", "Working Set MBs", BytesToMb(process.WorkingSet64)));
                infoBuilder.Append(string.Format("{0} : {1}\r\n", "Threads", process.Threads.Count));


                //Check available hard disk space
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                disk.Get();
                double totalSpaceGb = Convert.ToDouble(disk["Size"]) / Math.Pow(1024, 3);
                double freeSpaceGb = Convert.ToDouble(disk["FreeSpace"]) / Math.Pow(1024, 3);
                infoBuilder.Append(string.Format("Disk usage {0:0.00}/{1:0.00}Gb\r\n", totalSpaceGb - freeSpaceGb, totalSpaceGb));
                return infoBuilder.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteWarning("Failed to get disk statistics: " + ex.Message, Category.GeneralError);
            }
            return "Fail to get system info";
        }

        private double BytesToMb(double sizeInBytes)
        {
            return sizeInBytes / Math.Pow(1024, 2);
        }
    }
}
