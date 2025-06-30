using System;
using System.Collections.Generic;
using UnityEngine;

//菜鸟教程：https://www.runoob.com/csharp/csharp-tutorial.html

namespace TS.Tools
{
    public class Code : MonoBehaviour
    {
        public TypeTest CurrentType = TypeTest.字符切割;


        //数组的三种写法
        int[] mark1 = new int[3] { 13, 23, 28 };
        int[] mark2 = { 1, 2, 3 };
        int[] mark3 = new int[3];


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (CurrentType)
                {
                    #region 字符替换
                    case TypeTest.字符替换:
                        //替换字符，@为引号字符串，【@"C:\Windows"】等价于【"C:\\Windows"】
                        string str03 = @"C:\Windows\User";
                        Debug.Log("原字符串：" + str03);
                        str03 = str03.Replace(@"\", @"/");
                        Debug.Log("字符替换：" + str03);
                        break;
                    #endregion

                    #region 字符切割
                    case TypeTest.字符切割:
                        string str01 = "你是捉摸不住的风";
                        for (int i = 0; i < str01.Length; i++)
                        {
                            char CutStr = str01[i];
                            Debug.Log(CutStr);
                        }
                        break;
                    #endregion

                    #region 字符组合
                    case TypeTest.字符组合:
                        char[] Poem = { '是', '十', '二', '月', '晴', '朗' };
                        string str02 = new string(Poem);
                        Debug.Log(str02);
                        break;
                    #endregion

                    #region 字符串联
                    case TypeTest.字符串联:
                        //用Join里的字符将其他字符串联起来
                        string[] sarray = { "想", "藏", "起", "你" };
                        string message = String.Join("+", sarray);
                        Debug.Log(message);
                        break;
                    #endregion

                    #region 数型转换
                    case TypeTest.数型转换:
                        int intValue = 1;
                        float FloatValue = (float)intValue;
                        string stringValue = intValue.ToString();
                        Debug.Log("在" + FloatValue + stringValue + "月的肖邦里");
                        break;
                    #endregion

                    #region 变色打印
                    case TypeTest.变色打印:
                        string str04 = "不敢声张";
                        Debug.Log(string.Format("<color=yellow>{0}</color>", str04));
                        break;
                    #endregion

                    #region 遍历与for循环
                    case TypeTest.遍历与for循环:
                        //foreach的使用，j=marks[i]
                        foreach (int j in mark1)
                        {
                            Debug.Log(j);
                        }
                        //等同于for，但foreach适用性更强
                        for (int i = 0; i < mark1.Length; i++)
                        {
                            Debug.Log(mark1[i]);
                        }
                        break;
                    #endregion

                    #region 异常try
                    case TypeTest.异常try:
                        //类似于Debug,当try报错，依然执行catch，throw可以用来检测异常情况
                        try
                        {
                            Debug.Log(mark1[5]);
                        }
                        catch
                        {
                            throw new Exception("超出marks队列了");
                        }
                        break;
                    #endregion

                    #region 判空赋值与非空不赋值
                    case TypeTest.判空赋值与非空不赋值:
                        double? num1 = null;
                        double? num2 = new double?();
                        double? num3 = 99;
                        double num4, num5, num6;
                        string str4 = "非空", str5 = "非空", str6 = "非空";
                        num4 = num1 ?? 11;
                        num5 = num2 ?? 11;
                        num6 = num3 ?? 11;
                        if (num4 == 11) str4 = "空";
                        if (num5 == 11) str5 = "空";
                        if (num6 == 11) str6 = "空";
                        Debug.Log("num4为" + str4 + "，num5为" + str5 + "，num6为" + str6);
                        break;
                    #endregion

                    #region 三目运算
                    case TypeTest.三目运算:
                        int AA = 2;
                        int BB = 5;
                        Debug.Log(AA);
                        Debug.Log(BB);
                        Debug.Log("最大数为：" + (AA > BB ? AA : BB));
                        break;
                    #endregion

                    #region 结构体与类的引用
                    case TypeTest.结构体与类的引用:
                        //结构体的引用
                        Album Album1;
                        Album Album2;
                        //Album1 详述
                        Album1.Title = "《XXX》";
                        Album1.Producer = "王小明";
                        Album1.Num = 1;
                        Debug.Log(Album1.Title + "是" + Album1.Producer + "的第" + Album1.Num + "张专辑");
                        //Album2 详述
                        Album2.Title = "《YYY》";
                        Album2.Producer = "舒效珑";
                        Album2.Num = 3;
                        Debug.Log(Album2.Title + "是" + Album2.Producer + "的第" + Album2.Num + "张专辑");
                        Debug.Log("====================");
                        //类的引用
                        AlbumClass Album3 = new AlbumClass();
                        AlbumClass Album4 = new AlbumClass();
                        Album3.Detail("《XXX》", "王小明", 1);
                        Album4.Detail("《YYY》", "舒效珑", 3);
                        Album3.DebugPlay();
                        Album4.DebugPlay();
                        break;
                    #endregion

                    #region 枚举的数型转换
                    case TypeTest.枚举的数型转换:
                        int Sort = (int)TypeTest.枚举的数型转换;
                        Debug.Log("这是第" + Sort + "个基础教程");
                        break;
                    #endregion

                    #region 类的封装
                    case TypeTest.类的封装:
                        //将类里的变量封装成含参方法，获取返回值
                        Box Box1 = new Box();
                        double volume = Box1.setLength(6);
                        Debug.Log("打印的是6吗？是" + volume);
                        break;
                    #endregion

                    #region 继承基础
                    case TypeTest.继承基础:
                        //继承的基础运用
                        Dog dog = new Dog();
                        dog.DogName = "狗";
                        dog.Kind = "犬科";
                        Debug.Log(dog.DogName + "属于" + dog.Kind);
                        break;
                    #endregion

                    #region 继承接口
                    case TypeTest.继承接口:
                        //继承
                        InterfaceBase Base1 = new InterfaceBase();
                        Debug.Log(Base1.ClickDown());
                        break;
                    #endregion

                    #region 多态重载
                    case TypeTest.多态重载:
                        Printdata dataClass = new Printdata();
                        int add1 = dataClass.Add(1, 2);
                        string add3 = dataClass.Add("Hello World");
                        Debug.Log("int类：" + add1);
                        Debug.Log("String类：" + add3);
                        break;
                    #endregion

                    #region 多态动态
                    case TypeTest.多态动态:
                        //为Rectangle的length，width赋值
                        Rectangle ab = new Rectangle(10, 7);
                        Rectangle s = new Rectangle("面积：");
                        double value = ab.area();
                        string Area = s.DebugArea();
                        Debug.Log(Area + value);
                        break;
                    #endregion


                    #region 多态派生
                    case TypeTest.多态派生:
                        //创建一个 新的List<Over>对象，并向该对象添加 Task01、Task02 和 Task03
                        var NewOvers = new List<Over>
                    {
                        new Task01(),
                        new Task02(),
                        new Task03()
                    };
                        // 使用 foreach 循环对该列表的派生类进行循环访问，并对其中的每个 Over 对象调用 Reply 方法
                        foreach (var Over in NewOvers)
                        {
                            Over.Reply();
                        }
                        break;
                    #endregion

                    #region 运算符重载
                    case TypeTest.运算符重载:
                        BoxPro boxPro1 = new BoxPro();
                        BoxPro boxPro2 = new BoxPro();
                        BoxPro boxPro3 = new BoxPro();

                        boxPro1.setLength(4);
                        boxPro2.setLength(5);
                        boxPro3 = boxPro1 + boxPro2;
                        double Value = boxPro3.getVolume();
                        Debug.Log(Value);
                        break;
                        #endregion


                }

            }
        }



    }
    #region 结构体与类的引用
    /// <summary>
    /// 结构体
    /// </summary>
    struct Album
    {
        public string Title;
        public string Producer;
        public int Num;
    };

    /// <summary>
    /// 类
    /// </summary>
    class AlbumClass
    {
        public string Title;
        public string Producer;
        public int Num;

        /// <summary>
        /// 详情
        /// </summary>
        public void Detail(string title, string producer, int num)
        {
            Title = title;
            Producer = producer;
            Num = num;
        }
        public void DebugPlay()
        {
            Debug.Log(Title + "是" + Producer + "的第" + Num + "张专辑");
        }
    }
    #endregion

    #region 类的封装
    /// <summary>
    /// 类的封装
    /// </summary>
    class Box
    {
        private double length;   // 长度
        public double setLength(double len)
        {
            length = len;
            return length;
        }
    }
    #endregion

    #region 继承基础
    /// <summary>
    /// 继承基础
    /// </summary>
    class Animal
    {
        public string Kind;
    }
    class Dog : Animal
    {
        public string DogName;
    }
    #endregion

    #region 继承接口
    /// <summary>
    /// 继承接口
    /// </summary>
    class NullClass
    {
        protected int length = 3;
    }
    public interface InPoint
    {
        //方法
        int ClickDown();
    }
    class InterfaceBase : NullClass, InPoint
    {
        public int ClickDown()
        {
            length += 10;
            return length;
        }
    }
    #endregion

    #region 多态重载
    /// <summary>
    /// 多态重载
    /// </summary>
    class Printdata
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        public string Add(string s)
        {
            return s;
        }
    }
    #endregion

    #region 多态动态
    /// <summary>
    /// 抽象类
    /// </summary>
    abstract class Shape
    {
        abstract public int area();
        abstract public string DebugArea();
    }
    class Rectangle : Shape
    {
        private int length;
        private int width;
        private string Str;
        //构造函数
        public Rectangle(int a = 0, int b = 0)
        {
            length = a;
            width = b;
        }
        public Rectangle(string str)
        {
            Str = str;
        }
        //对抽象类的area()方法进行重写
        public override int area()
        {
            return (width * length);
        }
        public override string DebugArea()
        {
            return Str;
        }
    }

    #endregion

    #region 多态派生
    /// <summary>
    /// 统一的父类
    /// </summary>
    public class Over
    {
        // 虚方法
        public virtual void Reply()
        {
            Debug.Log("收到Over");
        }
    }
    class Task01 : Over
    {
        public override void Reply()
        {
            //重写虚方法：将Over父类的 Debug.Log("Over完成") 重写成 Debug.Log("执行任务一")
            Debug.Log("执行任务一");
            //重写后 再次引用虚方法 
            base.Reply();
        }
    }
    class Task02 : Over
    {
        public override void Reply()
        {
            Debug.Log("执行任务二");
            base.Reply();
        }
    }
    class Task03 : Over
    {
        public override void Reply()
        {
            Debug.Log("执行任务三");
            base.Reply();
        }
    }
    #endregion


    class BoxPro
    {
        private double length;      // 长度
        public static BoxPro operator +(BoxPro b, BoxPro c)
        {
            BoxPro box = new BoxPro();
            return box;
        }
        public void setLength(double len)
        {
            length = len;
        }
        public double getVolume()
        {
            return length;
        }
    }

    public enum TypeTest
    {
        Null,
        字符替换,
        字符切割,
        字符组合,
        字符串联,
        数型转换,
        变色打印,
        遍历与for循环,
        异常try,
        判空赋值与非空不赋值,
        三目运算,
        结构体与类的引用,
        枚举的数型转换,
        类的封装,
        继承基础,
        继承接口,
        多态重载,
        多态动态,
        多态派生,
        运算符重载,
    }




    class Other
    {
        #region 查找所有按钮
        //Button[] Btns;
        //void GetButtonExample()
        //{
        //    //Awake
        //    Btns = transform.GetComponentsInChildren<Button>();
        //    //Start
        //    foreach (var item in Btns)
        //    {
        //        item.onClick.AddListener(() => { OnBtnClickEvent(item); });
        //    }
        //}
        //void OnBtnClickEvent(Button Btn)
        //{
        //    switch (Btn.name)
        //    {
        //        case "name1":

        //            break;
        //        case "name2":

        //            break;
        //        case "name3":

        //            break;
        //        case "name4":

        //            break;
        //        case "name5":

        //            break;
        //        case "name6":

        //            break;
        //        default:

        //            break;
        //    }
        //}
        #endregion


        Sprite sprite = Resources.Load("UI/旋转", typeof(Sprite)) as Sprite;

        ////小数位和留零
        //Text FloatZero_Text;
        //float FloatNum;
        //protected void FloatZero()
        //{
        //    //数字转文本
        //    //效果 001.28或128.00
        //    FloatZero_Text.text = string.Format("{0:000.00}", FloatNum);
        //    //文本转数字 
        //    FloatNum = float.Parse(FloatZero_Text.text);
        //}

        public void Funtion()
        {
            ////数型转换
            //NumText.text = string.Format("{0:00.00}", NumFloat);
            //NumFloat = float.Parse(NumText.text);

            ////变色打印
            //Debug.Log(string.Format("<color=yellow>{0}</color>", text));

            ////字符型转换成枚举
            //Foods e = (Foods)System.Enum.Parse(typeof(Foods), other.name);

            ////得到枚举名称
            //Foods.名称.ToString();

            ////得到枚举值
            //Foods.名称.GetHashCode();


    


            //文本打印
            //private TMP_Text info;
            //private string currentInfo = "打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印打印";
            //private void OnEnable()
            //{
            //    StartCoroutine(ShowText());
            //}
            //IEnumerator ShowText()
            //{
            //    for (int i = 0; i <= CurrentInfo.Length; i++)
            //    {
            //        info.text = CurrentInfo.Substring(0, i);
            //        yield return new WaitForSeconds(0.05f);
            //    }
            //}


            //复制到剪切板
            //GUIUtility.systemCopyBuffer = string;


            string str = "沙克哈斯卡很快就撒";
            char[] Array = str.ToCharArray();
            for (int i = 0; i < Array.Length; i++)
            {
                Debug.Log(Array[i]);
            }

            //https://blog.csdn.net/qq_37430247/article/details/114387125
            string DataPath = @"F:\UnitySVN\ShuiLunJi_App\ShuiLunJi_App\Assets\StreamingAssets\Substring";
            string substring = DataPath.Substring(DataPath.LastIndexOf(@"\") + 1, DataPath.Length - DataPath.LastIndexOf(@"\") - 1);
            Debug.Log("文件的名字为：" + substring);
        }




       

    }
}