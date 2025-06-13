using GeneralizerDLL;

namespace Path_GeneralizerUI
{
    public partial class Main_Menu : Form
    {
        public static Generalizer_Core gc = new Generalizer_Core();
        public Main_Menu()
        {
            InitializeComponent();
            Generalizer_Events.Warning += (sender, message) =>
            {
                MessageBox.Show($"[Generalizer] Warning: {message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
            DataView.DataSource = GetDataTable(gc);
            for (int i = 0; i < DataView.Columns.Count; i++)
                DataView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
