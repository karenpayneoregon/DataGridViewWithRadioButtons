using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MockedDataLibrary;

namespace DataGridViewCheckBoxConventional
{
    public partial class Form1 : Form
    {
        private BindingSource bsCustomers = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var ops = new MockData();
            var table = ops.Table;

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "Process",
                DataType = typeof(bool),
                DefaultValue = false
            });

            table.Columns["Process"].SetOrdinal(0);
            table.Columns["CustomerIdentifier"].ColumnMapping = MappingType.Hidden;
            
            bsCustomers.DataSource = table;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = bsCustomers;
            dataGridView1.Columns["CompanyName"].HeaderText = "Company";
            dataGridView1.Columns["CompanyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            var results = ((DataTable) bsCustomers.DataSource)
                .AsEnumerable()
                .Where(row => row.Field<bool>("Process"))
                .Select(row => new Company()
                {
                    id = row.Field<int>("CustomerIdentifier"),
                    Name = row.Field<string>("CompanyName")
                }).ToList();

            if (results.Count >0)
            {
                var sb = new StringBuilder();
                foreach (Company company in results)
                {
                    sb.AppendLine(company.ToString());
                }

                MessageBox.Show(sb.ToString());
            }
            else
            {
                MessageBox.Show("Please select one or more companies.");
            }
        }
    }
}
