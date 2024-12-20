using System.Diagnostics;

namespace Patrones;

public class Tree<T>(TreeNode<T> root) : IIterable<T>
{
    public TreeNode<T> Root = root;

    public IIterator<T> GetIterator()
    {
        return new TreeIterator<T>(Root);
    }
}


public class TreeIterator<T> : IIterator<T>
{
    private TreeNode<T> _currentNode;

    public TreeIterator(TreeNode<T> root)
    {
        _currentNode = root;
    }

    public T Value => _currentNode.Value;

    public bool NextValue()
    {
        var nextNode = GetNextRelative(null, _currentNode);
        if (nextNode != null && nextNode != _currentNode)
        {
            _currentNode = nextNode;
            return true;
        }
        else
        {
            return false;
        }
    }

    private static TreeNode<T>? GetNextRelative(TreeNode<T>? lastNode, TreeNode<T> currentNode)
    {
        var lastNodeSiblings = currentNode.Children;
        var indexOfCurrentNodeAsSibling = lastNode == null ? -1 : lastNodeSiblings.IndexOf(lastNode);

        if (indexOfCurrentNodeAsSibling < lastNodeSiblings.Count - 1)
        {
            return lastNodeSiblings[indexOfCurrentNodeAsSibling + 1];
        }

        if (currentNode.Father != null)
        {
            return GetNextRelative(currentNode, currentNode.Father);
        }

        return null;
    }
}

