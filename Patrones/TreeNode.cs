
namespace Patrones;

public class TreeNode<T>(T value, TreeNode<T>? father = null, List<TreeNode<T>> children = null!)
{

    public T Value = value;

    public TreeNode<T>? Father = father;

    public List<TreeNode<T>> Children = children ?? [];

}
