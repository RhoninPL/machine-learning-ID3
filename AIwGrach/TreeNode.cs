using System.Collections.Generic;

namespace AIwGrach
{
    public class TreeNode
    {
        private TreeNode() { }

        public TreeNode(bool isleaf, string parentName, string attributeName, int tableIndex, TreeNode parent)
        {
            IsLeaf = isleaf;
            ParentName = parentName;
            TableIndex = tableIndex;
            ChildNodes = new List<TreeNode>();
            AttributeName = attributeName;
            Parent = parent;
        }

        public TreeNode(bool isleaf, string parentName, string attributesName, int newNodeIndex, List<string> attributesNames, TreeNode parent)
        {
            IsLeaf = isleaf;
            ParentName = parentName;
            TableIndex = newNodeIndex;
            ChildNodes = new List<TreeNode>();
            AttributeName = attributesName;
            AttributeNames = attributesNames;
            Parent = parent;
        }

        public string AttributeName { get; }

        public List<string> AttributeNames { get; }

        public string ParentName { get; }

        public TreeNode Parent { get; }

        public List<TreeNode> ChildNodes { get; }

        public int TableIndex { get; }

        public bool IsLeaf { get; }
    }
}