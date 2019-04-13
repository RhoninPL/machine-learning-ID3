using System.Collections.Generic;
using System.Linq;

namespace AIwGrach
{
    public class Data
    {
        #region Properties

        public List<string> Columns { get; set; }

        public List<List<string>> Rows { get; set; }

        #endregion

        #region Public Methods

        public void SetAttributesNames(List<List<string>> importedData)
        {
            Columns = importedData.First();
            importedData.RemoveAt(0);
        }

        public Data ReduceData(string attribute, int index)
        {
            var newData = new Data { Columns = new List<string>(), Rows = new List<List<string>>() };

            foreach (var column in Columns)
                newData.Columns.Add(column);

            for (var i = 0; i < Rows.Count; i++)
            {
                if (!Rows[i][index].Equals(attribute))
                    continue;

                var row = new List<string>();
                for (var j = 0; j < Columns.Count; j++)
                {
                    if (j != index)
                        row.Add(Rows[i][j]);
                }

                newData.Rows.Add(row);
            }

            newData.Columns.RemoveAt(index);

            return newData;
        }

        #endregion
    }
}