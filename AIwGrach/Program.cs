using System;

namespace AIwGrach
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileHandler = new FileHandler { FileName = "data.txt" };
            var objectList = fileHandler.ReadFile();

            var newEntropyLibrary = new ID3Library
            {
                ImportedData = objectList
            };
            var tree = newEntropyLibrary.FillTheTree();

            DisplayHelper.DisplayTree(tree, "");

            Console.ReadKey();
        }
    }
}
