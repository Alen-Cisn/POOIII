namespace Patrones;

public class Program
{
    public static void Main()
    {
        int numberOfNodes = 10;
        Tree<int> randomTree = CreateRandomTree(numberOfNodes);


        var iterator = randomTree.GetIterator();
        Console.WriteLine("Valor actual: " + iterator.Value);
        while (iterator.NextValue())
        {
            Console.WriteLine("Siguiente valor: " + iterator.Value);
        }
    }

    private static Tree<int> CreateRandomTree(int numberOfNodes)
    {
        if (numberOfNodes <= 0) throw new ArgumentException("Number of nodes must be greater than zero.");

        TreeNode<int> root = new(1);
        Queue<TreeNode<int>> queue = new();
        queue.Enqueue(root);

        Random random = new();
        int currentNodeValue = 2;

        while (currentNodeValue <= numberOfNodes)
        {
            TreeNode<int> currentNode = queue.Dequeue();
            int childrenCount = random.Next(1, 4); // Randomly decide how many children (1 to 3)

            for (int i = 0; i < childrenCount && currentNodeValue <= numberOfNodes; i++)
            {
                TreeNode<int> childNode = new(currentNodeValue++, currentNode);
                currentNode.Children.Add(childNode);
                queue.Enqueue(childNode);
            }
        }

        return new Tree<int>(root);
    }
}
