using System;
using System.Collections.Generic;

abstract class Node
{
    protected object data;
    public List<Node> children = new List<Node>();

    public abstract object GetValue();
}