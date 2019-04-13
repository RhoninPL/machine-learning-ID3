using System;
using System.Collections.Generic;
using System.Linq;

namespace AIwGrach
{
    public class ID3Library
    {
        #region Properties

        public List<List<string>> ImportedData { get; set; }

        #endregion

        #region Public Methods

        public TreeNode FillTheTree()
        {
            if (ImportedData.Count == 0)
                throw new Exception("No data found.");

            var data = new Data();
            // Ustawienie nazw kolumn
            data.SetAttributesNames(ImportedData);
            // Ustawienie wierszy danych
            data.Rows = ImportedData;
            // Rozpoczęcie nauki drzewa
            return Learn(data, string.Empty, null);
        }

        #endregion

        #region  Private Methods

        private TreeNode Learn(Data data, string parentName, TreeNode parent)
        {
            // szukanie korzenia
            var root = GetRootNode(data, parentName, parent);

            foreach (var item in root.AttributeNames)
            {
                // sprawdzanie czy nie ma już właściwości z entropią = 0, tworzony jest liść, inaczej szukaj dalej rekursywnie
                var isLeaf = CheckIsLeaf(root, data, item);

                if (isLeaf)
                    continue;

                // Zmniejszenie tablicy danych z przykładami potrzebnymi tylko do obliczenia na następnej gałęzi/liścia
                var reducedTable = data.ReduceData(item, root.TableIndex);
                root.ChildNodes.Add(Learn(reducedTable, item, parent));
            }

            return root;
        }

        private bool CheckIsLeaf(TreeNode node, Data data, string attributeToCheck)
        {
            if (data.Columns.Count == 1)
                return true;

            var isLeaf = true;
            var allEndValues = new List<string>();

            foreach (var row in data.Rows)
            {
                if (row[node.TableIndex].Equals(attributeToCheck))
                    allEndValues.Add(row[data.Columns.Count - 1]);
            }

            if (allEndValues.Count > 0 && allEndValues.Any(x => x != allEndValues[0]))
                isLeaf = false;

            if (isLeaf)
                node.ChildNodes.Add(new TreeNode(true, allEndValues[0], attributeToCheck, 0, node));

            return isLeaf;
        }

        private TreeNode GetRootNode(Data data, string parentName, TreeNode parent)
        {
            // Obliczanie entropii atrybutu decyzyjnego
            double decisionEntropy = CalculateEntropy(data.Rows.Select(node => node.Last()).Count(d => d.ToUpper().Equals("YES")), data.Rows.Count);

            int decisionAttributeIndex = data.Rows.FirstOrDefault().Count - 1;
            List<TmpNode> nodeGains = new List<TmpNode>();
            int propertiesCount = data.Rows.FirstOrDefault().Count;
            for (int index = 0; index < propertiesCount - 1; index++)
            {
                List<TmpNode> attribute = GetListOfAttributesEntropies(data, decisionAttributeIndex, index);

                nodeGains.Add(new TmpNode
                {
                    Index = index,
                    Nodes = attribute,
                    // Obliczanie przyrostu informacji
                    Gain = CalculateGain(decisionEntropy, attribute, data)
                });
            }

            TmpNode newNode = FindMaxGainNode(nodeGains);
            List<string> attributesNames = new List<string>();
            foreach (var dataRow in data.Rows)
            {
                var found = attributesNames.Any(t => t.ToUpper().Equals(dataRow[newNode.Index].ToString().ToUpper()));

                if (!found)
                    attributesNames.Add(dataRow[newNode.Index]);
            }

            return new TreeNode(false, parentName, data.Columns[newNode.Index], newNode.Index, attributesNames, parent);
        }

        private List<TmpNode> GetListOfAttributesEntropies(Data data, int decisionAttributeIndex, int index)
        {
            List<TmpNode> attribute = new List<TmpNode>();
            List<string> properties = GetAttributeProperties(index, data);
            foreach (var caseProperty in properties)
            {
                attribute.Add(new TmpNode
                {
                    Name = caseProperty,
                    Indexes = FindAllIndexes(index, caseProperty, data),
                    Entropy = CalculateEntropy(
                        data.Rows.Count(d => d[index].Equals(caseProperty) && d[decisionAttributeIndex].ToUpper().Equals("YES")),
                        data.Rows.Count(d => d[index].Equals(caseProperty)))
                });
            }

            return attribute;
        }

        private double CalculateGain(double decisionEntropy, List<TmpNode> nodes, Data data)
        {
            double sum = 0;

            foreach (var node in nodes)
                sum += node.Entropy * (node.Indexes.Count() / (double)data.Rows.Count);

            return decisionEntropy - sum;
        }

        private TmpNode FindMaxGainNode(List<TmpNode> tmpNodes)
        {
            if (tmpNodes.Count == 0)
                throw new Exception("Missing nodes.");

            TmpNode newNode = new TmpNode { Gain = -1.0 };
            foreach (var node in tmpNodes)
            {
                if (newNode.Gain < node.Gain)
                    newNode = node;
            }

            return newNode;
        }

        private List<int> FindAllIndexes(int index, string caseProperty, Data data)
        {
            int i;
            List<int> indexes = new List<int>();
            do
            {
                if (indexes.Any())
                {
                    i = data.Rows.FindIndex(indexes.Last() + 1, d => d[index].Contains(caseProperty));
                }
                else
                {
                    i = data.Rows.FindIndex(d => d[index].Contains(caseProperty));
                }

                if (i >= 0)
                    indexes.Add(i);
                else
                    break;
            } while (true);

            return indexes;
        }

        private List<string> GetAttributeProperties(int index, Data data)
        {
            List<string> attribute = new List<string>();
            foreach (var att in data.Rows.Select(d => d[index]))
            {
                if (!attribute.Contains(att))
                    attribute.Add(att);
            }

            return attribute;
        }

        private double CalculateEntropy(int positive, int total)
        {
            var negative = total - positive;

            if (positive == 0 || total == 0)
                return 0;

            var positiveP = (double)positive / total;
            var negativeP = (double)negative / total;

            var mathLog = !double.IsInfinity(Math.Log(negativeP, 2)) ? Math.Log(negativeP, 2) : 0;

            return -positiveP * Math.Log(positiveP, 2)
                   - negativeP * mathLog;
        }

        #endregion
    }


    public class TmpNode
    {
        #region Properties

        public List<TmpNode> Nodes { get; set; }

        public int Index { get; set; }

        public List<int> Indexes { get; set; }

        public double Entropy { get; set; }

        public double Gain { get; set; }

        public string Name { get; set; }

        #endregion
    }
}