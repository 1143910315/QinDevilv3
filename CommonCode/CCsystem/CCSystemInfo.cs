using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace CommonCode.CCsystem {
    public class CCSystemInfo {
        //获取CPU序列号
        public static string GetCpuID() {
            try {
                string cpuInfo = "";//cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc) {
                    if (cpuInfo.Length == 0) {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    }
                }
                if (cpuInfo.Length != 0) {
                    return cpuInfo;
                }
            } catch (Exception) {
            }
            return "unknow";
        }
        //获取网卡硬件地址 
        public static string GetMacAddress() {
            try {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc) {
                    if ((bool)mo["IPEnabled"] == true) {
                        if (mac.Length == 0) {
                            mac = mo["MacAddress"].ToString();
                        }
                    }
                }
                if (mac.Length != 0) {
                    return mac;
                }
            } catch (Exception) {
            }
            return "unknow";
        }
    }
}