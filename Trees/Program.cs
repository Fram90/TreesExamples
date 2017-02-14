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

            n2._keys[0].left = n1;
            n2._keys[0].right = n3;
            n2._keys[1].left = n3;
            n2._keys[1].right = n4;
            n2._keys[2].left = n4;
            n2._keys[2].right = n5;

            tree.rootNode = n2;

            tree.AddNode(20);
            //Console.WriteLine(q?.Key.ToString() ?? "no element");

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
        int t, lowerBound, upperBound;
        public Node rootNode = null;

        public BTree(int measure)
        {
            t = measure;
            lowerBound = t - 1;
            upperBound = 2 * t - 1;
        }

        public NodeKey Get(int key)
        {
            if (rootNode == null) return null;

            return RecursiveGet(key, rootNode);

        }

        private NodeKey RecursiveGet(int key, Node node)
        {
            NodeKey prev = null;
            int i = 0;

            var currentkey = node.GetNodeKey(i);

            while (currentkey != null && currentkey.Key <= key)
            {
                if (currentkey.Key == key) return currentkey;
                prev = currentkey;
                currentkey = node.GetNodeKey(++i);
            }

            var nextNode = currentkey == null ? prev.right : currentkey.left;

            return RecursiveGet(key, nextNode);
        }

        public class Node
        {
            public List<NodeKey> _keys;

            public int Count => _keys?.Count ?? 0;

            public bool IsLeaf => _keys == null;

            public Node(List<NodeKey> keys)
            {
                _keys = keys;
            }

            public Node(List<int> keys)
            {
                var keyList = new List<NodeKey>();

                foreach (var key in keys)
                {
                    keyList.Add(new NodeKey(key));
                }

                _keys = keyList;
            }

            public NodeKey GetNodeKey(int index)
            {
                return _keys.Count > index ? _keys[index] : null;
            }

            public NodeKey this[int index] => _keys.Count > index ? _keys[index] : null;

            public NodeKey GetMeanElement()
            {

            }
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

            var placeToAdd = FindPlaceToAdd(20, rootNode);
        }

        private int FindPlaceToAdd(int key, Node node)
        {
            NodeKey prev = null;
            int i = 0;

            var currentkey = node.GetNodeKey(i);

            while (currentkey != null && currentkey.Key <= key)
            {
                if (currentkey.Key == key) return currentkey.Key;
                prev = currentkey;
                currentkey = node.GetNodeKey(++i);
            }

            var nextNode = currentkey == null ? prev.right : currentkey.left;

            if (nextNode == null)
            {
                return node._keys.IndexOf(currentkey);
            }

            return FindPlaceToAdd(key, nextNode);

        }


    }

}
