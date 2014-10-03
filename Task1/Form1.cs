using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Task1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            dgvResult.Rows.Add(9);
            
        }

        private void запускToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Solver slv = new Solver();
            int[] data = { 10, 100, 1000 };
            for (int i = 0; i < 9; i++)
            {
                dgvResult.Rows[i].Cells[0].Value = (i + 1).ToString(); // номер
                dgvResult.Rows[i].Cells[1].Value = data[i / 3].ToString(); // размерность
                dgvResult.Rows[i].Cells[2].Value = "[-" + data[i % 3].ToString() + ";" + data[i % 3].ToString() + "]"; // диапазон

                dgvResult.Rows[i].Cells[3].Value = "";
                dgvResult.Rows[i].Cells[4].Value = "";
            }
            Application.DoEvents();
            for (int i = 0; i < 9; i++)
            {
                
                double zn1 = 0, zn2 = 0;
                dgvResult.Rows[i].Cells[0].Value = (i + 1).ToString(); // номер
                dgvResult.Rows[i].Cells[1].Value = data[i / 3].ToString(); // размерность
                dgvResult.Rows[i].Cells[2].Value = "[-" + data[i % 3].ToString() + ";" + data[i % 3].ToString() + "]"; // диапазон
                slv.Form_Answer(10000, data[i / 3], data[i % 3], ref zn1, ref zn2);
                dgvResult.Rows[i].Cells[3].Value = zn1.ToString("G3", CultureInfo.InvariantCulture);
                dgvResult.Rows[i].Cells[4].Value = zn2.ToString("G3", CultureInfo.InvariantCulture);
                Application.DoEvents();
            }
        }
    }
}
