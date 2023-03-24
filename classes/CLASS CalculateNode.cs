using System;
using System.Collections.Generic;


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