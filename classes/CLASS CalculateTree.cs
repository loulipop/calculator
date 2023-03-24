using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;

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