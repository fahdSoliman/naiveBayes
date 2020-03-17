using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WDEwebproject
{
    public class Classifier
    {
        public DataSet dataSet = new DataSet();

        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }


        public void TrainClassifier(DataTable table)
        {
            dataSet.Tables.Add(table);

            //table
            DataTable GaussianDistribution = dataSet.Tables.Add("Gaussian");
            GaussianDistribution.Columns.Add(table.Columns[0].ColumnName);
            DataTable BayessianDistribution = DataSet.Tables.Add("Bayessian");
            BayessianDistribution.Columns.Add(table.Columns[0].ColumnName);

            //columns
            for (int i = 1; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].DataType.Name.ToString() == "Double")
                {
                    GaussianDistribution.Columns.Add(table.Columns[i].ColumnName + "Mean");
                    GaussianDistribution.Columns.Add(table.Columns[i].ColumnName + "Variance");
                }
                else if (table.Columns[i].DataType.Name.ToString() == "String")
                {
                    var Slst = (from myRow in table.AsEnumerable()
                                orderby myRow.Field<string>(table.Columns[i]) ascending
                                group myRow by myRow.Field<string>(table.Columns[i].ColumnName) into g
                                select new { Name = g.Key, Count = g.Count() }).ToList();
                    foreach (var itm in Slst)
                    {
                        if ((string)itm.Name != "")
                        {
                            BayessianDistribution.Columns.Add(table.Columns[i].ColumnName + " " + itm.Name);
                        }
                    }
                }
                else if (table.Columns[i].DataType.Name.ToString() == "Boolean")
                {
                    var Blst = (from myRow in table.AsEnumerable()
                                orderby myRow.Field<bool>(table.Columns[i]) ascending
                                group myRow by myRow.Field<bool>(table.Columns[i].ColumnName) into g
                                select new { Name = g.Key, Count = g.Count() }).ToList();
                    foreach (var itm in Blst)
                    {
                        BayessianDistribution.Columns.Add(table.Columns[i].ColumnName + " " + itm.Name);
                    }
                }
            }


            //calc data
            var results = (from myRow in table.AsEnumerable()
                           group myRow by myRow.Field<string>(table.Columns[0].ColumnName) into g
                           select new { Name = g.Key, Count = g.Count() }).ToList();

            for (int j = 0; j < results.Count; j++)
            {
                DataRow row = GaussianDistribution.Rows.Add();
                row[0] = results[j].Name;

                int a = 1;
                for (int i = 1; i < table.Columns.Count; i++)
                {
                    if (table.Columns[i].DataType.Name.ToString() == "Double")
                    {
                        row[a] = Helper.Mean(SelectRows(table, i, string.Format("{0} = '{1}'", table.Columns[0].ColumnName, results[j].Name)));
                        row[++a] = Helper.Variance(SelectRows(table, i, string.Format("{0} = '{1}'", table.Columns[0].ColumnName, results[j].Name)));
                        a++;
                    }
                }
            }
            var Dresults = (from myRow in table.AsEnumerable()
                            group myRow by myRow.Field<string>(table.Columns[0].ColumnName) into g
                            select new { Name = g.Key, Count = g.Count() }).ToList();
            for (int j = 0; j < Dresults.Count; j++)
            {
                DataRow row = BayessianDistribution.Rows.Add();
                double r = Convert.ToDouble(results[j].Count) / (Convert.ToDouble(results[0].Count) + Convert.ToDouble(results[1].Count));
                row[0] = r;

                int b = 1;
                for (int i = 1; i < table.Columns.Count; i++)
                {

                    if (table.Columns[i].DataType.Name.ToString() == "String")
                    {

                        var Cresults = (from myRow in table.AsEnumerable()
                                        orderby myRow.Field<string>(table.Columns[i]) ascending
                                        group myRow by myRow.Field<string>(table.Columns[i].ColumnName) into g
                                        select new { Name = g.Key, Count = g.Count() }).ToList();
                        var PNresults = (from myRow in table.AsEnumerable()
                                         orderby myRow.Field<string>(table.Columns[i]) ascending
                                         where myRow.Field<string>(table.Columns[0].ColumnName.ToString()) == Dresults[j].Name
                                         group myRow by myRow.Field<string>(table.Columns[i].ColumnName) into g
                                         select new { Name = g.Key, Count = g.Count() }).ToList();

                        foreach (var item in PNresults)
                        {
                            if ((string)item.Name != "")
                            {

                                float R = (float)item.Count / Dresults[j].Count;
                                row[b] = R.ToString();
                                b++;
                            }
                        }
                    }
                    else if (table.Columns[i].DataType.Name.ToString() == "Boolean")
                    {

                        var Cresults = (from myRow in table.AsEnumerable()
                                        orderby myRow.Field<bool>(table.Columns[i]) ascending
                                        group myRow by myRow.Field<bool>(table.Columns[i].ColumnName) into g
                                        select new { Name = g.Key, Count = g.Count() }).ToList();
                        var Bresults = (from myRow in table.AsEnumerable()
                                        orderby myRow.Field<bool>(table.Columns[i]) ascending
                                        where myRow.Field<string>(table.Columns[0].ColumnName.ToString()) == Dresults[j].Name
                                        group myRow by myRow.Field<bool>(table.Columns[i].ColumnName) into g
                                        select new { Name = g.Key, Count = g.Count() }).ToList();

                        foreach (var item in Bresults)
                        {

                            double R = (double)item.Count / Dresults[j].Count;
                            row[b] = R.ToString();
                            b++;

                        }
                    }
                }
            }
        }



        public Dictionary<string, double> Classify(DataTable table2)
        {
            Dictionary<string, double> score = new Dictionary<string, double>();
            var results = (from myRow in dataSet.Tables[0].AsEnumerable()
                           group myRow by myRow.Field<string>(dataSet.Tables[0].Columns[0].ColumnName) into g
                           select new { Name = g.Key, Count = g.Count() }).ToList();
            var Cresults = (from myRow in table2.AsEnumerable()
                            select new
                            {
                                Name = myRow.Field<string>(table2.Columns[0]),
                                key = myRow.Field<string>(table2.Columns[1])
                            }).ToList();


            for (int i = 0; i < results.Count; i++)
            {
                List<double> subScoreList = new List<double>();
                subScoreList.Add(Convert.ToDouble(dataSet.Tables["Bayessian"].Rows[i][0]));

                for (int f = 0; f < Cresults.Count; f++)
                {
                    for (int k = 1; k < dataSet.Tables["Gaussian"].Columns.Count; k = k + 2)
                    {
                        if (dataSet.Tables["Gaussian"].Columns[k].ColumnName.ToString() == Cresults[f].Name.ToString() + "Mean")
                        {
                            double mean = Convert.ToDouble(dataSet.Tables["Gaussian"].Rows[i][k]);
                            double variance = Convert.ToDouble(dataSet.Tables["Gaussian"].Rows[i][k + 1]);
                            double result = Helper.NormalDist(Convert.ToDouble(Cresults[f].key), mean, Helper.SquareRoot(variance));
                            subScoreList.Add(result);
                        }
                    }
                }
                for (int f = 0; f < Cresults.Count; f++)
                {
                    for (int j = 1; j < dataSet.Tables["Bayessian"].Columns.Count; j++)
                    {
                        if (dataSet.Tables["Bayessian"].Columns[j].ToString() == Cresults[f].Name.ToString() + " " + Cresults[f].key.ToString())
                        {
                            double result = Convert.ToDouble(dataSet.Tables["Bayessian"].Rows[i][j]);
                            subScoreList.Add(result);
                        }
                    }
                }

                double finalScore = 0;
                foreach (double itm in subScoreList)
                {
                    if (finalScore == 0)
                    {
                        finalScore = itm;
                    }
                    else
                    {
                        finalScore = finalScore * itm;
                    }
                }
                score.Add(results[i].Name, finalScore * 0.5);
            }
            double maxOne = score.Max(c => c.Value);
            
            var name = (from c in score
                        where c.Value == maxOne
                        select c.Key).First();
            
            return score;

        }

        #region Helper Function

        public IEnumerable<double> SelectRows(DataTable table, int column, string filter)
        {
            List<double> _doubleList = new List<double>();
            DataRow[] rows = table.Select(filter);
            for (int i = 0; i < rows.Length; i++)
            {
                _doubleList.Add((double)rows[i][column]);
            }

            return _doubleList;
        }

        public void Clear()
        {
            dataSet = new DataSet();
        }

        #endregion
    }
}
