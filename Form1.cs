using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fillGrid();
        }

        private void fillGrid()
        {
            for (int i = 0; i < numericUpDownElements.Value; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
                dataGridView1.Columns[i].DefaultCellStyle.NullValue = "0";
            }
            for (int i = 0; i < numericUpDownCriteries.Value; i++)
            {
                dataGridView1.Rows.Add();
            }
        }

        private void numericUpDownCriteries_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(((UpDownBase)sender).Text) < numericUpDownCriteries.Value)
            {
                dataGridView1.Rows.Add();
            }
            else
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }
        }

        private void numericUpDownElements_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(((UpDownBase)sender).Text) < numericUpDownElements.Value)
            {
                dataGridView1.Columns.Add((dataGridView1.Columns.Count).ToString(), (dataGridView1.Columns.Count).ToString());
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].DefaultCellStyle.NullValue = "0";

            }
            else
            {
                dataGridView1.Columns.Remove((Convert.ToInt32(((UpDownBase)sender).Text) - 1).ToString());
            }
        }

        private void buttonRandomize_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Value = rnd.Next(0, 15).ToString();
                }
            }
        }

        private static double GetSumOfVector(List<double> vector)
        {
            double sum = 0;
            foreach (var element in vector)
            {
                sum += element;
            }
            return sum;
        }

        private static List<List<double>> dgwTo2dListOfDouble(DataGridView dataGridView)
        {
            List<List<double>> list = new List<List<double>>();
            for (int i2 = 0; i2 < dataGridView.Rows.Count; i2++)
            {
                list.Add(new List<double>());
                for (int j2 = 0; j2 < dataGridView.Columns.Count; j2++)
                {
                    list[i2].Add(Convert.ToDouble(dataGridView.Rows[i2].Cells[j2].Value));
                }
            }
            return list;
        }

        private static List<List<double>> Pareto(List<List<double>> matrix)
        {
            List<List<double>> paretoSet = new List<List<double>>();

            matrix.ForEach(v => paretoSet.Add(v));

            int i = 0;
            int j = 1;

        step2:
            if (j == matrix.Count) return paretoSet;
            if (GetSumOfVector(matrix[i]) > GetSumOfVector(matrix[j]))
            {
                paretoSet.Remove(matrix[j]);
                if (j < matrix.Count)
                {
                    j++;
                    goto step2;
                }
                else
                {
                    goto step7;
                }
            }
            else
            {
                if (j == matrix.Count) return paretoSet;
                if (GetSumOfVector(matrix[j]) > GetSumOfVector(matrix[i]))
                {
                    paretoSet.Remove(matrix[i]);
                    goto step7;
                }
                else
                {
                    if (j < matrix.Count)
                    {
                        j++;
                        goto step2;
                    }
                    else
                    {
                        goto step7;
                    }
                }
            }

        step7:
            if (i < matrix.Count - 1)
            {
                i++;
                j = i + 1;
                goto step2;
            }
            else
            {
                return paretoSet;
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Refresh();

            List<List<double>> matrix = dgwTo2dListOfDouble(dataGridView1);

            List<List<double>> result = Pareto(matrix);

            for (int i = 0; i <= numericUpDownElements.Value; i++)
            {
                dataGridView2.Columns.Add(i.ToString(), i.ToString());
                dataGridView2.Columns[i].DefaultCellStyle.NullValue = "0";
            }

            for (int i = 0; i < result.Count; i++)
            {
                dataGridView2.Rows.Add();
                for (int j = 0; j < result[i].Count; j++)
                {
                    dataGridView2.Rows[i].Cells[j].Value = result[i][j];
                }
            }
        }
    }
}
