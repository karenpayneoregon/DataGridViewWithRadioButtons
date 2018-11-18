using System.Windows.Forms;

namespace DataGridViewMultiCheckBoxes.Extensions
{
    public static class DataGridviewExtensions
    {
        public static DataGridViewCell CurrentCellValue(this DataGridView pDataGridView)
        {
            return pDataGridView[pDataGridView.Columns[pDataGridView.CurrentCell.ColumnIndex].Index, pDataGridView.CurrentRow.Index];
        }
    }
}
