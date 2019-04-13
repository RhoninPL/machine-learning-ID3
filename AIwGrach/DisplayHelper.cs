using System;

namespace AIwGrach
{
    public class DisplayHelper
    {
        #region Public Methods

        public static void DisplayTree(TreeNode node, string indent = "")
        {
            if (string.IsNullOrEmpty(node.ParentName))
            {
                Console.WriteLine($"{node.AttributeName} => ");

                for (var index = 0; index < node.ChildNodes.Count; index++)
                    DisplayTree(node.ChildNodes[index], string.Concat(indent, $"\t {node.AttributeNames[index]} -> "));
            }
            else
            {
                var decision = node.ParentName.Equals("Yes") || node.ParentName.Equals("No") ? $"\t{node.ParentName}" : "";
                if (!string.IsNullOrEmpty(decision))
                {
                    Console.Write(string.Concat(indent, decision));
                    Console.WriteLine();
                }
                else
                {
                    indent = string.Concat(indent, $"\t{node.AttributeName}  => ");
                    for (var index = 0; index < node.ChildNodes.Count; index++)
                        DisplayTree(node.ChildNodes[index], string.Concat(indent, $"\t {node.AttributeNames[index]} -> "));
                }
            }
        }

        #endregion
    }
}