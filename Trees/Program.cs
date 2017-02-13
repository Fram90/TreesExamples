using System;
using System.Collections;
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
            var tree = new BTree(3);

            BTree.Node n1 = new BTree.Node(new List<int>() { 1, 2, 3 });
            BTree.Node n2 = new BTree.Node(new List<int>() { 4, 17, 31 });
            BTree.Node n3 = new BTree.Node(new List<int>() { 7, 9, 11, 13, 16 });
            BTree.Node n4 = new BTree.Node(new List<int>() { 19, 26, 27 });
            BTree.Node n5 = new BTree.Node(new List<int>() { 96, 97, 99 });

            n2.left = n1;
            n2.right = n3;

            tree.Get(3);

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

        public void Remove(T1 key)
        {
            Node x = root, y = null;

            while (x != null)
            {
                var cmp = key.CompareTo(x.key);

                if (cmp == 0)
                {
                    break;
                }
                y = x;
                x = cmp < 0 ? x.left : x.right;
            }

            if (x == null)
            {
                return;
            }

            if (x.right == null)
            {
                if (y == null)
                {
                    root = x.left;
                }
                else
                {
                    if (x == y.left)
                    {
                        y.left = x.left;
                    }
                    else
                    {
                        y.right = x.left;
                    }
                }
            }
            else
            {
                var leftMost = x.right;
                y = null;

                while (leftMost.left != null)
                {
                    y = leftMost;
                    leftMost = leftMost.left;
                }

                if (y != null)
                {
                    y.left = leftMost.right;
                }
                else
                {
                    x.right = leftMost.right;
                }

                x.key = leftMost.key;
                x.value = leftMost.value;
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
            if (rootNode == null)
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

    public class BTree
    {
        int t;
        private Node rootNode = null;

        public BTree(int measure)
        {
            t = measure;
        }

        public int Get(int key)
        {
            var x = rootNode;

            if (rootNode == null) return -1;

            while (true)
            {
                var nodeKey = x.NextKey();

                while (nodeKey < key)
                {
                    if (nodeKey == -1)
                    {
                        return -1;
                    }

                    if (nodeKey == key)
                    {
                        return key;
                    }

                    nodeKey = x.NextKey();

                }

                x = x.left;

            }

        }

        public class Node
        {
            List<NodeKey> _keys;
            private int i;



            public Node(List<NodeKey> keys)
            {
                _keys = keys;
            }

            public NodeKey NextKey()
            {
                return _keys.Count > i ? _keys[i++] : null;
            }

            public int KeysCount()
            {
                return _keys?.Count ?? 0;
            }

            public int this[int index] => _keys[index].Key;
        }

        public class NodeKey
        {
            public int Key { get; }

            public Node left;
            public Node right;

            public NodeKey(int key)
            {
                Key = key;
            }
        }



        public void AddNode(int key)
        {
            var x = rootNode;
        }


    }

}
