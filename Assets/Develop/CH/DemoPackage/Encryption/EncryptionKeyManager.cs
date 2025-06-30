using UnityEngine;
using System;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.ComTypes;

public class EncryptionKeyManager : MonoBehaviour
{
    public bool isFirst = true;
    public string NextSence;
    public UserType currentUser = UserType.User;//身份

    Transform UserPanel;//用户界面
    InputField UserKeyInput;//输入密钥框
    Button UserLoginBtn; //用户登录按钮

    Transform AdminPanel; //管理员界面
    InputField AdminDeviceInput; //输入设备码框
    Button AdminKeyBtn; //生成密钥按钮

    Text Prompt;//用户登录反馈

    string deviceCode; //设备码
    string generatedKey; //初始生成的密钥
    string adminGeneratedKey; //管理员生成的密钥

    // 创建一个字典，用于存储英文字母和数字与汉字的映射关系
    private Dictionary<char, string> charToChineseMap = new Dictionary<char, string>();

    private void Awake()
    {
        //初始化映射关系
        InitializeMapping();
        UserPanel = transform.Find("UserPanel");
        UserKeyInput = UserPanel.Find("UserKeyInput").GetComponent<InputField>();
        UserLoginBtn = UserPanel.Find("UserLoginBtn").GetComponent <Button>();
        Prompt = UserPanel.Find("Prompt").GetComponent<Text>();
        AdminPanel = transform.Find("AdminPanel");
        AdminDeviceInput = AdminPanel.Find("AdminInput").GetComponent<InputField>();
        AdminKeyBtn = AdminPanel.Find("AdminKeyBtn").GetComponent<Button>();
        //判断是否是管理员
        if (currentUser == UserType.Admin) AdminPanel.gameObject.SetActive(true);
        //加载数据
        ReadData();
    }

    private void Start()
    {
        //用户登录按钮
        UserLoginBtn.onClick.AddListener(UserLoginButtonClicked);
        //生成密钥按钮
        AdminKeyBtn.onClick.AddListener(GenerateKeyButtonClicked);
        //如果之前的密钥正确，直接进入
        if (isFirst == false) 
        {
            SceneManager.LoadScene(NextSence);
        }
        else
        {
            //生成设备码并初始密钥
            GenerateAndSaveDeviceCode();
            generatedKey = GenerateKeyFromDeviceCode(deviceCode);
        }
    }

    #region 设备码和密钥
    /// <summary>
    /// 生成设备码并保存到桌面
    /// </summary>
    void GenerateAndSaveDeviceCode()
    {
        //根据计算机的MAC地址生成固定的设备码
        deviceCode = GetMacAddress();

        //保存设备码到桌面
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "【设备码】.txt");
        File.WriteAllText(filePath, deviceCode);
        Debug.Log($"设备码已生成并保存到桌面: {filePath}");
    }

    /// <summary>
    /// 辅助方法以获取MAC地址
    /// </summary>
    string GetMacAddress()
    {
        string macAddress = "";
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                macAddress = nic.GetPhysicalAddress().ToString();
                break;
            }
        }
        return macAddress;
    }

    /// <summary>
    /// 用户登录按钮点击事件
    /// </summary>
    void UserLoginButtonClicked()
    {
        // 获取用户输入的密钥
        string userInputKey = UserKeyInput.text;
        // 验证密钥是否正确
        if (IsValidKey(userInputKey))
        {
            Prompt.text = "密钥正确，正在登录中...";
            ChangeData(false);
            if (isFirst == false) SceneManager.LoadScene(NextSence);

        }
        else
        {
            // 检查是否为空或者只包含空格
            if (string.IsNullOrWhiteSpace(userInputKey))
            {
                Prompt.text = "提示：密钥不可为空，请先输入密钥";
                UserKeyInput.text = "";
            }
            else
            {
                Prompt.text = "提示：密钥错误，请联系供应商获取正确的密钥";
            }
        }
        Prompt.GetComponent<Animation>().Play();
    }

    /// <summary>
    /// 生成密钥按钮点击事件
    /// </summary>
    void GenerateKeyButtonClicked()
    {
        // 获取管理员输入的设备码
        string adminDeviceCode = AdminDeviceInput.text;

        // 验证设备码是否有效（可选，根据需要添加其他验证逻辑）
        if (IsValidDeviceCode(adminDeviceCode))
        {
            // 生成管理员密钥
            adminGeneratedKey = GenerateKeyFromDeviceCode(adminDeviceCode);

            AdminDeviceInput.text = adminGeneratedKey;

            Debug.Log("管理员生成的密钥："+adminGeneratedKey);
        }
        else
        {
            Debug.LogWarning("无效的设备码，请重新输入。");
        }
    }

    /// <summary>
    /// 验证密钥是否有效
    /// </summary>
    bool IsValidKey(string key)
    {
        return key == generatedKey;
    }

    /// <summary>
    /// 验证设备码是否有效
    /// </summary>
    bool IsValidDeviceCode(string deviceCode)
    {
        return !string.IsNullOrEmpty(deviceCode);
    }
    #endregion

    #region 密钥处理
    /// <summary>
    /// 根据设备码生成密钥的逻辑
    /// </summary>
    string GenerateKeyFromDeviceCode(string deviceCode)
    {
        string result = MapAndCombineString(deviceCode);
        return result;
    }
    // 初始化映射关系的方法
    void InitializeMapping()
    {
        // 将英文字母和数字与对应的汉字建立映射关系
        charToChineseMap.Add('A', "獨");
        charToChineseMap.Add('B', "亞");
        charToChineseMap.Add('C', "逺");
        charToChineseMap.Add('D', "陳");
        charToChineseMap.Add('E', "厷");
        charToChineseMap.Add('F', "迋");
        charToChineseMap.Add('G', "菏");
        charToChineseMap.Add('H', "睢");
        charToChineseMap.Add('I', "踽");
        charToChineseMap.Add('J', "張");
        charToChineseMap.Add('K', "限");
        charToChineseMap.Add('L', "呞");
        charToChineseMap.Add('M', "姙");
        charToChineseMap.Add('N', "遖");
        charToChineseMap.Add('O', "萪");
        charToChineseMap.Add('P', "菘");
        charToChineseMap.Add('Q', "熊");
        charToChineseMap.Add('R', "洧");
        charToChineseMap.Add('S', "鶴");
        charToChineseMap.Add('T', "餮");
        charToChineseMap.Add('U', "技");
        charToChineseMap.Add('V', "饕");
        charToChineseMap.Add('W', "喬");
        charToChineseMap.Add('X', "龍");
        charToChineseMap.Add('Y', "瀅");
        charToChineseMap.Add('Z', "瓞");
        charToChineseMap.Add('1', "瀣");
        charToChineseMap.Add('2', "瑭");
        charToChineseMap.Add('3', "瀅");
        charToChineseMap.Add('4', "潪");
        charToChineseMap.Add('5', "嚠");
        charToChineseMap.Add('6', "滘");
        charToChineseMap.Add('7', "煢");
        charToChineseMap.Add('8', "禮");
        charToChineseMap.Add('9', "凾");
        charToChineseMap.Add('0', "誌");
    }

    // 根据输入的英文字母或数字获取对应的汉字
    string GetChineseCharacter(char inputChar)
    {
        string chineseCharacter;

        if (charToChineseMap.TryGetValue(inputChar, out chineseCharacter))
        {
            return chineseCharacter;
        }
        else
        {
            return inputChar.ToString(); // 如果没有映射关系，保持原字符
        }
    }
    // 对输入字符串进行拆分、映射和重新组合
    string MapAndCombineString(string inputString)
    {
        char[] inputChars = inputString.ToCharArray();
        List<string> mappedCharacters = new List<string>();

        // 遍历输入字符串的每个字符
        foreach (char inputChar in inputChars)
        {
            // 获取映射后的汉字
            string mappedCharacter = GetChineseCharacter(inputChar);
            mappedCharacters.Add(mappedCharacter);
        }

        // 重新组合映射后的字符串
        string combinedString = string.Join("", mappedCharacters);
        return combinedString;
    }
    #endregion

    #region 储存数据

    private string filePath;

   
    /// <summary>
    /// 读取数据
    /// </summary>
    void ReadData()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Data.json");
        // 检查文件是否存在，如果不存在则生成数据
        if (!File.Exists(filePath))
        {
            ChangeData(true);
        }

        // 读取文件中的数据
        string jsonData = File.ReadAllText(filePath);

        // 将JSON数据转换为对象
        var dataObject = JsonUtility.FromJson<Data>(jsonData);
        // 访问布尔值
        isFirst = dataObject.FirstUse;
        Debug.Log("读取isFirst的布尔值：" + isFirst);
    }

    /// <summary>
    /// 更改数据
    /// </summary>
    void ChangeData(bool bo)
    {
        // 将数据写入文件
        File.WriteAllText(filePath, JsonUtility.ToJson(new Data { FirstUse = bo }));
        isFirst = bo;
    }

    // 定义一个数据类，用于存储你的数据结构
    [System.Serializable]
    public class Data
    {
        public bool FirstUse;
    }
    #endregion

    #region 管理员
    private int tKeyPressCount = 0;
    private float lastTKeyPressTime = 0f;
    private float timeThreshold = 1.0f; //设置两次T键按下的时间阈值

    void Update()
    {
        // 检测用户按下T键
        if (Input.GetKeyDown(KeyCode.T))
        {
            KeyCodeCallBack(delegate
            {
                AdminPanel.gameObject.SetActive(true);//开启管理员
            });
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            KeyCodeCallBack(delegate
            {
                AdminPanel.gameObject.SetActive(false);//关闭管理员
            });
        }
    }

    void KeyCodeCallBack(Action action=null)
    {
        //获取当前时间
        float currentTime = Time.time;

        //判断两次T键按下的时间间隔是否小于阈值
        if (currentTime - lastTKeyPressTime < timeThreshold)
        {
            //在这里执行连续按下T键后的逻辑
            tKeyPressCount++;

            //判断是否达到你希望的次数
            if (tKeyPressCount >= 13) 
            {
                action?.Invoke();// 回调
            }
        }
        else
        {
            tKeyPressCount = 1; //重置计数器
        }

        //更新上一次按下T键的时间戳
        lastTKeyPressTime = currentTime;
    }
    #endregion
}

public enum UserType
{
    User,
    Admin
}
