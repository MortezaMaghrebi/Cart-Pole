using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
namespace pendulum
{
    public partial class Charts : Form
    {
        public Charts()
        {
            InitializeComponent();
        }


        public void Chart(ArrayList t, ArrayList  w, ArrayList  x, ArrayList a)
        {
            try
            {
                chart1.Series.Clear(); chart1.Series.Add("teta");
                chart2.Series.Clear(); chart2.Series.Add("voltage");
                chart3.Series.Clear(); chart3.Series.Add("x");
                chart4.Series.Clear(); chart4.Series.Add("a");
                for (int i = 0; i < t.Count; i++)
                {
                    chart1.Series["teta"].Points.Add((double)t[i]);
                    chart2.Series["voltage"].Points.Add((double)w[i]);
                    chart3.Series["x"].Points.Add((double)x[i]);
                    chart4.Series["a"].Points.Add((double)a[i]);
                }
                chart1.Series["teta"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series["voltage"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart3.Series["x"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart4.Series["a"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.Show();
            }
            catch { }
            

        }
        private void Charts_Load(object sender, EventArgs e)
        {

        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }
    }
}
