using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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