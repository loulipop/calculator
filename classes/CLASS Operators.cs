using System;
using System.Collections.Generic;

delegate double Operator(double a, double b);
static class Operators
{
    //Danh sách các phép toán: Kiểu Dictionary, truy cập theo dạng [key] --> value
    public static Dictionary<string, Operator> operatorsList = new Dictionary<string, Operator>()
    {

        ["+"] = Add,
        ["-"] = Substract,
        ["x"] = Multiply,
        ["/"] = Div,
        ["%"] = Mod,
        ["^"] = Exp
    };

    //Định nghĩa các hàm tương ứng với loại phép toán
    private static double Add(double a, double b) => a + b;
    private static double Substract(double a, double b) => a - b;
    private static double Multiply(double a, double b) => a * b;
    private static double Div(double a, double b) => b != 0 ? a / b : throw new DivideByZeroException();
    // Phép chia với trường hợp b = 0: Quăng DivideByZero Exception
    private static double Mod(double a, double b) => a % b;
    private static double Exp(double a, double b) => Math.Pow(a, b);
}