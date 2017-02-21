using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new BTree(2);

            BTree.Node n1 = new BTree.Node(new List<int>() { 1, 2, 3 });
            BTree.Node n2 = new BTree.Node(new List<int>() { 4, 17, 31 });
            BTree.Node n3 = new BTree.Node(new List<int>() { 7, 9, 11 });//, 13, 16 });
            BTree.Node n4 = new BTree.Node(new List<int>() { 19, 26, 27 });
            BTree.Node n5 = new BTree.Node(new List<int>() { 96, 97, 99 });

            //n2.Children.Add(n1);
            //n2.Children.Add(n3);
            //n2.Children.Add(n4);
            //n2.Children.Add(n5);
            //tree.rootNode = n2;

            for (int j = 1; j < 10; j++)
            {
                tree.Insert(new BTree.NodeKey(j));
            }

            int i = 0;
            var test = tree.Search(tree.rootNode, 9, ref i);

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
        private static int _t;
        int lowerBound, upperBound;
        public Node rootNode = null;

        public BTree(int measure)
        {
            _t = measure;
            lowerBound = _t - 1;
            upperBound = 2 * _t - 1;
        }

        public Node Search(Node root, int key, ref int index)
        {
            int i = 0;
            while (i < root.Count() && key >= root[i].Key)
            {
                i++;
            }

            if (i < _t && i < root.Count() && key == root[i].Key)
            {
                index = i;
                return root;
            }

            return root.IsLeaf ? null : Search(root.Children[i], key, ref index);
        }

        private void SplitChild(Node parent, int i, Node child)
        {
            var z = new Node();

            for (int j = 0; j < _t - 1; j++)
            {
                z.AddOrUpdateKey(j, child[j + _t]);
            }

            if (!child.IsLeaf)
            {
                for (int j = 0; j < _t; j++)
                {
                    z.AddOrUpdateChild(j, child.Children[j + _t]);
                }
            }

            for (int j = parent.Count(); j > i; j--)
            {
                parent.AddOrUpdateChild(j + 1, parent.Children[j]);
            }

            parent.AddOrUpdateChild(i + 1, z);

            for (int j = parent.Count() - 1; j >= i; j--)
            {
                parent.AddOrUpdateKey(j + 1, parent[j]);
            }

            parent.AddOrUpdateKey(i, child[_t - 1]);
            child._keys.RemoveRange(_t - 1, child.Count() - _t + 1);
        }

        public void Insert(NodeKey key)
        {
            var root = rootNode;

            if (root == null)
            {
                var node = new Node();
                node._keys.Add(key);
                this.rootNode = node;
                return;
            }

            if (root.Count() == 2 * _t - 1)
            {
                var node = new Node();
                rootNode = node;
                node.Children.Add(root);
                SplitChild(node, 1, root);
                InsertNonFull(node, key);
            }
            else
            {
                InsertNonFull(root, key);
            }

        }

        private void InsertNonFull(Node child, NodeKey key)
        {
            int i = child.Count() - 1;

            if (child.IsLeaf)
            {
                while (i >= 0 && key.Key < child[i].Key)
                {
                    child[i + 1].Key = child[i].Key;
                    i--;
                }
                child.AddOrUpdateKey(i + 1, key);
            }
            else
            {
                while (i >= 0 && key.Key < child[i].Key)
                {
                    i--;
                }
                i++;

                if (child.Children[i].Count() == 2 * _t - 1)
                {
                    SplitChild(child, i, child.Children[i]);
                    if (key.Key > child[i].Key)
                    {
                        i++;
                    }
                }
                InsertNonFull(child.Children[i], key);
            }
        }

        //public NodeKey Get(int key)
        //{
        //    if (rootNode == null) return null;

        //    return RecursiveGet(key, rootNode);

        //}

        //private NodeKey RecursiveGet(int key, Node node)
        //{
        //    NodeKey prev = null;
        //    int i = 0;

        //    var currentkey = node.GetNodeKey(i);

        //    while (currentkey != null && currentkey.Key <= key)
        //    {
        //        if (currentkey.Key == key) return currentkey;
        //        prev = currentkey;
        //        currentkey = node.GetNodeKey(++i);
        //    }

        //    var nextNode = currentkey == null ? prev.right : currentkey.left;

        //    return RecursiveGet(key, nextNode);
        //}

        public class Node
        {
            public List<NodeKey> _keys;

            public List<Node> Children { get; set; }

            public bool IsLeaf => !Children.Any();

            public void AddOrUpdateKey(int index, NodeKey key)
            {
                if (index > _keys.Count - 1)
                {
                    _keys.Add(key);
                }
                else
                {
                    _keys[index] = key;
                }
            }

            public void AddOrUpdateChild(int index, Node child)
            {
                if (index > Children.Count - 1)
                {
                    Children.Add(child);
                }
                else
                {
                    Children[index] = child;
                }
            }

            public int Count()
            {
                int i = 0;
                foreach (var nodeKey in _keys)
                {
                    if (nodeKey.Key > 0)
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                return i;
            }

            public Node()
            {
                _keys = new List<NodeKey>();
                Children = new List<Node>();
            }

            public Node(List<NodeKey> keys)
            {
                //_keys = new int[_t];
                Children = new List<Node>();
            }

            public Node(List<int> keys)
            {
                var keyList = new List<NodeKey>();

                foreach (var key in keys)
                {
                    keyList.Add(new NodeKey(key));
                }

                _keys = keyList;
                Children = new List<Node>();
            }

            public NodeKey this[int index]
            {
                get { return _keys[index]; }
                set { _keys[index] = value; }
            }
        }

        public class NodeKey
        {
            public int Key { get; set; }

            public NodeKey(int key)
            {
                Key = key;
            }
        }



        //public void AddNode(int key)
        //{
        //    var x = rootNode;

        //    var placeToAdd = FindPlaceToAdd(20, rootNode);
        //}

        //private int FindPlaceToAdd(int key, Node node)
        //{
        //    NodeKey prev = null;
        //    int i = 0;

        //    var currentkey = node.GetNodeKey(i);

        //    while (currentkey != null && currentkey.Key <= key)
        //    {
        //        if (currentkey.Key == key) return currentkey.Key;
        //        prev = currentkey;
        //        currentkey = node.GetNodeKey(++i);
        //    }

        //    var nextNode = currentkey == null ? prev.right : currentkey.left;

        //    if (nextNode == null)
        //    {
        //        return node._keys.IndexOf(currentkey);
        //    }

        //    return FindPlaceToAdd(key, nextNode);

        ////}


    }

}
