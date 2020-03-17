using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.AnalysisServices.AdomdClient;


namespace WDEwebproject
{
    public partial class index : System.Web.UI.Page
    {
        //@"Data Source=FAHD;Initial Catalog=heartDisease;Integrated Security=True"
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["heartDiseaseConnectionString"].ToString();
            string query = "select disease, chest_pain_type, rest_blood_pressure, blood_sugar, rest_electro, max_heart_rate, exercice_angina from [heartdataset]";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(table);
            conn.Close();
            da.Dispose();

        }

        protected void MS_Algo(object sender, EventArgs e)
        {
            //string ex;
            //string x;
            //if (CheckBox1.Checked == true) ex = "TRUE"; else ex = "FALSE";
            //if (CheckBox2.Checked == true) x = "yes"; else x = "no";
            AdomdConnection con = new AdomdConnection(@"Data Source=NANS-F;Catalog=HeartDiseaseClass");
            AdomdConnection con2 = new AdomdConnection(@"Data Source=NANS-F;Catalog=HeartDiseaseClass");
            con.Open();
            con2.Open();
            AdomdCommand com = con.CreateCommand();
            AdomdCommand com2 = con2.CreateCommand();
            string s = @"select Disease from HeartDatasetMM
            natural prediction join
            (select '" + age_ms.Text + @"' as [Age],'" +
            blood_suger_ms.SelectedValue + @"' as [Blood Sugar], '" +
            max_heart_rate_ms.Text + @"' as [Max Heart Rate], '" +
            rest_electro_ms.SelectedValue + @"' as [Rest Electro], '" +
            chest_ms.SelectedValue + @"' as [Chest Pain Type], '" +
            exercice_angina_ms.SelectedValue + @"' as [Exercice Angina], '" +
            blood_press_ms.Text + @"' as [Rest Blood Pressure]) as t ;";
            string s2 = @"select flattened predicthistogram(Disease) from HeartDatasetMM
            natural prediction join
            (select '" + age_ms.Text + @"' as [Age],'" +
            blood_suger_ms.SelectedValue + @"' as [Blood Sugar], '" +
            max_heart_rate_ms.Text + @"' as [Max Heart Rate], '" +
            rest_electro_ms.SelectedValue + @"' as [Rest Electro], '" +
            chest_ms.SelectedValue + @"' as [Chest Pain Type], '" +
            exercice_angina_ms.SelectedValue + @"' as [Exercice Angina], '" +
            blood_press_ms.Text + @"' as [Rest Blood Pressure]) as t ;";


            com.CommandText = s;
            AdomdDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                result2.Text = dr[0].ToString();
            }
            com2.CommandText = s2;
            AdomdDataReader dr2 = com2.ExecuteReader();
            if (dr2.Read())
            {
                if(dr2["Expression.Disease"].ToString() == "positive")
                {
                    ms_pos.Text = dr2["Expression.$PROBABILITY"].ToString();
                }
                else
                {
                    ms_neg.Text = dr2["Expression.$PROBABILITY"].ToString();
                }
            }
            if (dr2.Read())
            {
                if (dr2["Expression.Disease"].ToString() == "positive")
                {
                    ms_pos.Text = dr2["Expression.$PROBABILITY"].ToString();
                }
                else
                {
                    ms_neg.Text = dr2["Expression.$PROBABILITY"].ToString();
                }
            }

            
            dr.Close();
            dr2.Close();
            con.Close();
            con2.Close();
        }

        protected void C_Coded_Algo(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            string connString = "Data Source=NANS-F;Initial Catalog=heartdis;Integrated Security=True";
            string query = "select disease, age, chest_pain_type, rest_blood_pressure, blood_sugar, rest_electro, max_heart_rate, exercice_angina from [heartdataset]";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            // create data adapter
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            // this will query your database and return the result to your datatable
            table.Load(dr);
            conn.Close();

            Classifier classifier = new Classifier();
            classifier.TrainClassifier(table);
            DataTable table2 = new DataTable();
            table2.Clear();
            table2.Columns.Add("V1", typeof(string));
            table2.Columns.Add("V2", typeof(string));


            table2.Rows.Add("age", age.Text);
            table2.Rows.Add("chest_pain_type", chest_pain_type.SelectedValue.ToString());
            table2.Rows.Add("rest_blood_pressure", blood_pressure.Text);
            table2.Rows.Add("blood_sugar", blood_suger.SelectedValue.ToString());
            table2.Rows.Add("rest_electro", rest_electro.SelectedValue.ToString());
            table2.Rows.Add("max_heart_rate", max_heart_rate.Text);
            table2.Rows.Add("exercice_angina", exercice_angina.SelectedValue.ToString());
            Dictionary<string, double> score = new Dictionary<string, double>();

            score = classifier.Classify(table2);

            double value = 0;
            foreach (var t in score)
            {
                value += t.Value;
            }
            foreach (var val in score)
            {

                if (val.Key.ToString() == "positive")
                {
                    c_pos.Text = Convert.ToString(val.Value / value);
                    
                }
                else
                {
                    c_neg.Text = Convert.ToString(val.Value / value);
                    
                }
            }
            double maxOne = score.Max(c => c.Value);

            var name = (from c in score
                        where c.Value == maxOne
                        select c.Key).First();
            result.Text = name;
            table2.Clear();
        }
    }
}