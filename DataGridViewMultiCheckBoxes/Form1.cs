using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataGridViewMultiCheckBoxes.Extensions;
using MockedDataLibrary;

namespace DataGridViewMultiCheckBoxes
{
    
    public partial class Form1 : Form
    {
        private BindingSource bsCustomers = new BindingSource();
        private List<string> _checkBoxColumnNames = new List<string>();
        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var ops = new MockData();

            bsCustomers.DataSource = ops.Table;

            dataGridView1.AutoGenerateColumns = false;

            // assign customers to the datagridview
            dataGridView1.DataSource = bsCustomers;

            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;

            _checkBoxColumnNames = dataGridView1.Columns.OfType<DataGridViewCheckBoxColumn>()
                .Select(col => col.Name)
                .ToList();

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // assert we have a valid row
            if (dataGridView1.CurrentRow == null) return;

            // we only want to work with checkbox cells
            if (!(dataGridView1.CurrentCell is DataGridViewCheckBoxCell)) return;

            // there are three checkbox cells, get the others ones other then the current cell
            var others = _checkBoxColumnNames
                .Where(name => name != dataGridView1.Columns[dataGridView1.CurrentCellValue().ColumnIndex].Name)
                .ToArray();

            // ensure only one is selected
            if ((bool)dataGridView1.CurrentCell.Value)
            {
                foreach (var colName in others)
                {
                    dataGridView1.CurrentRow.Cells[colName].Value = false;
                }
            }
        }
        /// <summary>
        /// Commit change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell is DataGridViewCheckBoxCell)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < dataGridView1.Rows.Count; index++)
            {
                foreach (var columnName in _checkBoxColumnNames)
                {
                    dataGridView1.Rows[index].Cells[columnName].Value = false;
                }
            }
        }

        private void cmdGetSelection_Click(object sender, EventArgs e)
        {
            var ratings = new List<SelectedItem>();
            var notRated = new StringBuilder();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells.OfType<DataGridViewCheckBoxCell>().Any(t => t.Value != null))
                {
                    var item = new SelectedItem
                    {
                        CompanyName = row.Cells["CompanyNameColumn"].Value.ToString(),
                        Rating = row.Cells.OfType<DataGridViewCheckBoxCell>()
                            .FirstOrDefault(cell => (bool) cell.Value).OwningColumn.HeaderText
                    };

                    ratings.Add(item);
                }
                else
                {
                    notRated.AppendLine(row.Cells["CompanyNameColumn"].Value.ToString());
                }
            }
            
            if (notRated.Length >0)
            {
                MessageBox.Show($"Please rate these\n\n{notRated}");
            }
        }
    }
}
