using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Hard_Chatik
{
    internal class HardWare
    {
        public string GetProcessorInfo()
        {
            var procInfo = new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>().FirstOrDefault();
            return procInfo["Manufacturer"] + " " + procInfo["Name"];
        }
        public string GetVideoInfo()
        {
            var videoInfo = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>().FirstOrDefault();
            return videoInfo["Caption"].ToString();
        }
        public string GetBaseInfo()
        {
            var baseInfo = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard").Get().Cast<ManagementObject>().FirstOrDefault();
            return baseInfo["Manufacturer"] + " " + baseInfo["Product"];
        }
        public string GetRamInfo()
        {
            var ramInfo = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory").Get().Cast<ManagementObject>();
            
            List<string> ramPlates = new List<string>();
            
            foreach (var ramCurr in ramInfo)
            {
                ramPlates.Add(Convert.ToUInt64(ramCurr["Capacity"]) / Math.Pow(1024, 3) + "GB");
            }
            return string.Join(" ", ramPlates.Select((item, index) => $"{index + 1}) {item}"));
        }

        public string GetMemoryInfo()
        {            
            var diskInfo = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get().Cast<ManagementObject>();

            List<string> memoryBlocks = new List<string>();

            foreach (var diskCurr in diskInfo)
            {
                memoryBlocks.Add($"{Math.Round(Convert.ToInt64(diskCurr["size"]) / Math.Pow(1024, 3))}GB");
            }

            return string.Join(" ", memoryBlocks.Select((item, index) => $"{index + 1}) {item}"));
        }
        public string GetMotherBoardId()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");

            foreach (ManagementObject motherboard in searcher.Get())
            {
                return motherboard["SerialNumber"].ToString();
            }
            return "N/A";
        }
    }
}
