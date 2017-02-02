using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new BinaryTree<int, int>();
            tree.Add(5,5);
            tree.Add(7,7);
            tree.Add(2,2);
            tree.Add(3,3);
            tree.Add(9,9);
            tree.Add(1,1);
            tree.Add(4,4);
            tree.Add(10, 10);
            tree.Add(6,6);
            tree.Add(8,8);

            tree.ReverseInorderPrint(tree.root);

            Console.ReadLine();

        }
    }

    public class BinaryTree<T1, T2> where T1 : IComparable<T1>
    {
        public Node root;

        public class Node
        {
            public T1 key;
            public T2 value;

            public Node left, right;

            public Node(T1 key, T2 value)
            {
                this.key = key;
                this.value = value;
            }

            public override string ToString()
            {
                return $"{key}: {value}";
            }
        }

        public T2 Get(T1 key)
        {
            Node x = root;
            while (x != null)
            {
                var comp = key.CompareTo(x.key);

                if (comp == 0)
                {
                    return x.value;
                }

                x = comp < 0 ? x.left : x.right;
            }
            return default(T2);
        }

        public void Add(T1 k, T2 v)
        {
            Node x = root, y = null;

            while (x != null)
            {
                int cmp = k.CompareTo(x.key);

                if (cmp == 0)
                {
                    x.value = v;
                    return;
                }

                y = x;
                x = cmp < 0 ? x.left : x.right;
            }

            var newNode = new Node(k, v);

            if (y == null)
            {
                root = newNode;
                return;
            }

            if (k.CompareTo(y.key) < 0)
            {
                y.left = newNode;
            }
            else
            {
                y.right = newNode;
            }

        }

        public void LevelOrderPrint(Node rootNode)
        {
            if (rootNode == null)
            {
                return;
            }

            Queue<Node> q = new Queue<Node>();
            q.Enqueue(rootNode);

            while (q.Count != 0)
            {
                Node temp = q.Dequeue();

                Console.WriteLine(temp);

                if (temp.left != null)
                {
                    q.Enqueue(temp.left);
                }

                if (temp.right != null)
                {
                    q.Enqueue(temp.right);
                }
            }
        }

        public void PreorderPrint(Node rootNode)
        {
            if (rootNode==null)
            {
                return;
            }

            Console.WriteLine(rootNode);

            PreorderPrint(rootNode.left);
            PreorderPrint(rootNode.right);
        }

        public void InorderPrint(Node rootNode)
        {
            if (rootNode == null)
            {
                return;
            }

            InorderPrint(rootNode.left);
            Console.WriteLine(rootNode);
            InorderPrint(rootNode.right);
        }

        public void PostorderPrint(Node rootNode)
        {
            if (rootNode == null)
            {
                return;
            }

            PostorderPrint(rootNode.left);
            PostorderPrint(rootNode.right);
            Console.WriteLine(rootNode);
        }

        public void ReverseInorderPrint(Node rootNode)
        {
            if (rootNode == null)
            {
                return;
            }

            ReverseInorderPrint(rootNode.right);
            Console.WriteLine(rootNode);
            ReverseInorderPrint(rootNode.left);
            
        }
    }

}
