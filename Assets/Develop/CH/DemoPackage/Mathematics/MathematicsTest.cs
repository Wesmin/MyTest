using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MathematicsTest : MonoBehaviour
{



    /// <summary>
    /// 组合数
    /// </summary>
    long Combination(int n, int k)
    {
        //阶乘函数 
        long Factorial(int x)
        {
            long result = 1;
            for (int i = 2; i <= x; i++)
                result *= i;
            return result;
        }

        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }


   


    void Start()
    {
        // 角度
        var angles = new Dictionary<int, double>();
        foreach (int deg in new[] { 0, 15, 30, 45, 60, 75, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 })
        {
            angles[deg] = deg * Math.PI / 180;
        }

        // 微分
        Func<double, double> f1 = x => x * x; // f(x) = x^2
        double x0 = 2.0; // 要求导的点
        double h1 = 1e-5; // 非常小的值
        double df = (f1(x0 + h1) - f1(x0)) / h1;


        // 极限趋向无穷, 求 lim(x→∞) (1 + 1/x)^x ≈ e
        double LimitApproximation(int n)
        {
            double x = n;
            return Math.Pow(1 + 1 / x, x);
        }

        // 极限趋向0
        double fx(double x) => Math.Sin(x) / x;
        double limit = fx(1e-6);  // 趋向于 1


        Func<double, double> f = x => Math.Sin(x);
        double h = 1e-5;
        double x1 = Math.PI / 3;

        double derivative = (f(x1 + h) - f(x1 - h)) / (2 * h);
        Console.WriteLine($"f'({x1}) ≈ {derivative}");

        string[] str = new string[]
        {
            $"极限趋向无穷：{LimitApproximation(100000)}",
            $"极限趋向0：{limit}",
            $"微分表达：{df}",
            $"组合数6选3：{Combination(6, 3)}",
            $"平方根： {Math.Sqrt(144)}",
            $"4次根： {Math.Pow(81, 1.0 / 4)}",
            $"勾股定理： {Math.Sqrt(3 * 3 + 4 * 4)}",
            $"+无穷：{double.PositiveInfinity}、{1.0 / 0.0}",
            $"-无穷：{double.NegativeInfinity}、{-1.0 / 0.0}",
            $"+无穷判断：{double.IsPositiveInfinity(double.PositiveInfinity)}",
            $"-无穷判断：{double.IsNegativeInfinity(double.NegativeInfinity)}",
            $"无穷判断：{double.IsInfinity(double.PositiveInfinity)}、{double.IsInfinity(double.NegativeInfinity)}",
            $"圆周率： {Math.PI}",
            $"Sin30°：{Math.Sin(angles[30])}",
            $"Cos60°：{Math.Cos(angles[60])}",
            $"Tan45°：{Math.Tan(angles[45])}",
            $"Ln(e)：   {Math.Log(Math.E)}",
            $"Log10：  {Math.Log10(10)}",
        };

        Debug.Log(string.Join(Environment.NewLine, str));

    }

}
