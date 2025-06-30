using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    private string dataFilePath;//路径
    public InputField Value;
    public InputField Content;
    public Button Get;
    public Button Save;
   
    void Start()
    {
        GetData(); //初始读取储存信息
        Save.onClick.AddListener(SaveData);//保存按钮
        Get.onClick.AddListener(GetData);
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    void GetData()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "Data.txt");
        //没有就生成，有就读取
        if (!File.Exists(dataFilePath))
        {
            WriteDefaultData();//初始化默认数据写入
        }
        else
        {
            //本地数据读取 
            string[] data = ReadData();
            int value = int.Parse(data[0]);//字符转数值 参考
            Value.text = value.ToString();//数值转字符 参考

            Content.text = data[1].ToString();
            Debug.Log(string.Format("<color=yellow>{0}</color>", "读取数据OK"));
        }
    }
    /// <summary>
    /// 储存数据
    /// </summary>
    void SaveData()
    {
        // 写入默认数据
        using (StreamWriter writer = new StreamWriter(dataFilePath))
        {
            //数据存入本地 
            writer.WriteLine(Value.text.ToString());
            writer.WriteLine(Content.text.ToString());
            Debug.Log(string.Format("<color=cyan>{0}</color>", "保存数据OK"));
        }
    }
    /// <summary>
    /// 初始化默认数据存入
    /// </summary>
    void WriteDefaultData()
    {
        using (StreamWriter writer = new StreamWriter(dataFilePath))
        {
            writer.WriteLine("200");
            writer.WriteLine("贰佰");
            Debug.Log(string.Format("<color=green>{0}</color>", "生成数据OK"));
        }
    }
    /// <summary>
    /// 读取数据方法
    /// </summary>
    private string[] ReadData()
    {
        if (!File.Exists(dataFilePath))
        {
            Debug.LogError("Data file does not exist!");
            return null;
        }

        // 读取数据
        string[] dataLines = File.ReadAllLines(dataFilePath);
        return dataLines;
    }
}
