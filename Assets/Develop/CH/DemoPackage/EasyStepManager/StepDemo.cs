using TS.Tools;
using UnityEngine;
/// <summary>
/// 简易流程管理器测试Demo
/// </summary>
public class StepDemo : MonoBehaviour
{
    private void Awake()
    {
        //默认开始流程测试一
        StartProcess(1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StepManager.NextStep();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //开始流程测试一
            StartProcess(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //开始流程测试二
            StartProcess(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //设置流程索引至第3步
            StepManager.StepSet(3);
        }
    }

    /// <summary>
    /// 开始流程
    /// </summary>
    public void StartProcess(int index)
    {
        // 构造方法名
        string methodName = $"Process{index}";
        // 获取当前类型
        var type = this.GetType();
        // 查找对应的方法
        var method = type.GetMethod(methodName);
        if (method != null)
        {
            // 如果找到了方法，则调用它
            method.Invoke(this, null);
            // 调用下一步
            BaseConfig.NextStep();
        }
        else
        {
            Debug.LogWarning($"Method {methodName} not found.");
        }
       
    }

    /// <summary>
    /// GUI提示
    /// </summary>
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 40;

        fontStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 50, 60),
            "键盘空格：执行下一步\n\n" +
            "键盘 1 键：开始流程测试一\n\n" +
            "键盘 2 键：开始流程测试二\n\n" +
            "键盘 3 键：将当前流程步骤跳转至第 3 步", fontStyle);
    }

    /// <summary>
    /// 流程一
    /// </summary>
    public void Process1()
    {
        //清除步骤
        StepManager.StepClear();
        Debug.Log(string.Format("<color=cyan>{0}</color>", "开始流程一"));

        //第一步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第1步");
        });
        //第二步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第2步");
        });
        //第三步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第3步");
        });
        //第四步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第4步");
        });
        //第五步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第5步");
        });
        //第六步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第6步");
        });
        //第七步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试一：执行第7步");
        });
    }
    /// <summary>
    /// 流程二
    /// </summary>
    public void Process2()
    {
        //清除步骤
        StepManager.StepClear();
        Debug.Log(string.Format("<color=cyan>{0}</color>", "开始流程二"));

        //第一步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第1步");
        });
        //第二步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第2步");
        });
        //第三步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第3步");
        });
        //第四步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第4步");
        });
        //第五步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第5步");
        });
        //第六步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第6步");
        });
        //第七步
        StepManager.StepAdd(delegate
        {
            Debug.Log("测试二：执行第7步");
        });
    }
}
