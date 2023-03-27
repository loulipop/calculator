using System.Data;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Zn
{
    abstract class Node
    {
        protected object data;
        public List<Node> children = new List<Node>();

        public abstract object GetValue();
    }

    abstract class Tree
    {
        protected Node root;
        public abstract object Travel();

    }

    class CalculateNode<T> : Node
    {

        public CalculateNode(T data) => base.data = data;


        public override object GetValue() //Method lấy giá trị của một nút 
        {
            if (typeof(T) == typeof(Operator))
            {
                Operator O = (dynamic)data;
                return (dynamic)O.Invoke((dynamic)children[0].GetValue(), (dynamic)children[1].GetValue());
            }

            return (dynamic)data;
        }
    }

    class CalculateTree : Tree
    {
        public CalculateTree(List<string> rpn) => Generate(rpn);

        public override object Travel() => root.GetValue();

        private void Generate(List<string> rpn) // Method sinh cây theo biểu thức RPN
        {
            Stack<Node> nodes = new Stack<Node>();
            for (int i = 0; i < rpn.Count; i++)
            {
                Node node;
                switch (rpn[i])
                {
                    case "+":
                    case "-":
                    case "x":
                    case "/":
                    case "%":
                    case "^":
                        node = new CalculateNode<Operator>(Operators.operatorsList[rpn[i]]);
                        node.children.Insert(0, nodes.Pop());
                        node.children.Insert(0, nodes.Pop());
                        nodes.Push(node);
                        break;
                    default:
                        node = new CalculateNode<double>(double.Parse(rpn[i]));
                        nodes.Push(node);
                        break;
                }
            }
            root = nodes.Pop();
        }
    }



    delegate double Operator(double a, double b);
    static class Operators
    {
        //Danh sách các phép toán: Kiểu Dictionary, truy cập theo dạng [key] --> value
        public static Dictionary<string, Operator> operatorsList = new Dictionary<string, Operator>()
        {
            
            ["+"] = Add,
            ["-"] = Subtract,
            ["x"] = Multiply,
            ["/"] = Div,
            ["%"] = Mod,
            ["^"] = Exp
        };

        //Định nghĩa các hàm tương ứng với loại phép toán
        private static double Add(double a, double b) => a + b;
        private static double Subtract(double a, double b) => a - b;
        private static double Multiply(double a, double b) => a * b;
        private static double Div(double a, double b) => b != 0 ? a / b : throw new DivideByZeroException();
        // Phép chia với trường hợp b = 0: Quăng DivideByZero Exception
        private static double Mod(double a, double b) => a % b;
        private static double Exp(double a, double b) => Math.Pow(a, b);
    }

    static class RPN
    {
        private static List<string> rpn;
        private static Stack<string> stack;

        public static List<string> getRPN(string input)
        {
            FormatInput(ref input); // Format input
            ConvertToRPN(input); // chuyển input sang ký pháp Ba Lan nghịch
            return rpn;
        }


        //INTERNAL METHODS
        private static void FormatInput(ref string input)
        {
            //format thêm dấu nhân trước mỗi cặp dấu )( vd: (3+3)(4-3) --> (3+3)x(4-3)
            char[] charSet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ')' };
            char[] operators = { '+', '-', 'x', '/', '^', '%' };
            int y = 0;
            do
            {
                y = input.IndexOf('(', y);
                if (y > 0 && charSet.Contains(input[y - 1]))
                    input = input.Insert(y, "x");
                y++;
            }
            while (y > 0);
            
            

            //thêm kí tự trống giữa các thành phần, vd: 33-4 --> 33 - 4 
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        input = input.Insert(i + 1, " ");
                        i++;
                        break;
                    case ')':
                        input = input.Insert(i, " ");
                        i++;
                        break;
                    case '+':
                    case '-':
                    case 'x':
                    case '/':
                    case '%':
                    case '^':
                        input = input.Insert(i + 1, " ");
                        input = input.Insert(i, " ");
                        i += 2;
                        break;
                    default:
                        break;
                }
            }
            input = input.Trim() + " ";
        }
        private static void ConvertToRPN(string input)
        {
            stack = new Stack<string>();
            rpn = new List<string>();

            //duyệt input string
            string O = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != ' ')
                    O += input[i];
                else
                {
                    switch (O)
                    {
                        case "(":
                            stack.Push(O);
                            break;
                        case ")":
                            string x;
                            do
                            {
                                x = stack.Pop();
                                if (x != "(")
                                    rpn.Add(x);
                            }
                            while (x != "(");
                            break;
                        case "+":
                        case "-":
                        case "x":
                        case "/":
                        case "%":
                        case "^":
                            while (stack.Count != 0 && Priority(O) <= Priority(stack.Peek()))
                                rpn.Add(stack.Pop());
                            stack.Push(O);
                            break;
                        default:
                            rpn.Add(O);
                            break;
                    }
                    O = "";
                }
            }
            while (stack.Count != 0)
                rpn.Add(stack.Pop());

        }
        private static int Priority(string c) //Hàm mức độ ưu tiên
        {
            switch (c)
            {
                case "^":
                    return 3;
                case "x":
                case "/":
                case "%":
                    return 2;
                case "+":
                case "-":
                    return 1;
            }
            // default (case "(" )
            return 0;
        }

    }

    public class Program
    {
        static void Main()
        {
            try
            {
                string input = Console.ReadLine();
                Tree tree = new CalculateTree(RPN.getRPN(input));
                Console.WriteLine(tree.Travel());
            }
            catch(DivideByZeroException)
            {
                Console.WriteLine("MATH ERROR");
            }
            catch (Exception)
            {
                Console.WriteLine("SYNTAX ERROR");
            }
        }
    }

}
