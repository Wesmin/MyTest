using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

public class DeviceTool
{
    #region 设备信息，需要UnityEngine
    /// <summary>
    /// 获取设备名称
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceName()
    {
        return UnityEngine.SystemInfo.deviceName;
    }

    /// <summary>
    /// 获取设备模型
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceModel()
    {
        return UnityEngine.SystemInfo.deviceModel;
    }

    /// <summary>
    /// 获取设备唯一标识码
    /// </summary>
    /// <returns></returns>
    public static string GetDeviceUniqueIdentifier()
    {
        return UnityEngine.SystemInfo.deviceUniqueIdentifier;
    }
    #endregion

    /// <summary>
    /// 获取局域网Ip
    /// </summary>
    /// <param name="addressType">IPv4或IPv6</param>
    /// <returns></returns>
    public static string GetLocalIp(AddressType addressType)
    {
        if (addressType == AddressType.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = string.Empty;

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;
            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (addressType == AddressType.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (addressType == AddressType.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return output;
    }

    /// <summary>
    /// 获取外网Ip
    /// </summary>
    /// <returns></returns>
    public static string GetExtranetIp()
    {
        string IP = string.Empty;
        try
        {
            //从网址中获取本机ip数据  
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.Default;
            IP = client.DownloadString("http://checkip.amazonaws.com/");
            client.Dispose();
            IP = Regex.Replace(IP, @"[\r\n]", "");
        }
        catch (Exception) { }

        return IP;
    }

    /// <summary>
    /// 获取MAC地址
    /// </summary>
    /// <returns></returns>
    public static List<string> GetMACList(OperationalStatus operationalStatus = OperationalStatus.Up, bool isAppendName = true)
    {
        var list = new List<string>();
        //这里使用 NetworkInterface 获取网络设备信息，能够直接获取网络设备类型，描述，名称等信息
        NetworkInterface[] allNetWork = NetworkInterface.GetAllNetworkInterfaces();
        if (allNetWork.Length > 0)
        {
            foreach (var item in allNetWork)
            {
                if (item.OperationalStatus == operationalStatus)
                {
                    //对MAC地址加上网卡名称，方便进行对应和选择
                    string strInfo = isAppendName ? item.GetPhysicalAddress().ToString() + $"({item.Name})" : item.GetPhysicalAddress().ToString();
                    list.Add(strInfo);
                }
            }
        }
        else
        {
            Console.WriteLine("找不到可用的网卡！");
        }
        return list;
    }
}

public enum AddressType
{
    IPv4,
    IPv6,
}