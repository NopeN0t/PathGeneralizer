using GeneralizerDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_GeneralizerUI
{
    partial class Main_Menu
    {
        private DataTable GetDataTable(Generalizer_Core gc)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Source", typeof(string));
            dt.Columns.Add("Target", typeof(string));
            gc.Config.GetAllData().ToList().ForEach(data =>
            {
                string[] parts = data.Split("|||");
                if (parts.Length == 2)
                {
                    DataRow row = dt.NewRow();
                    row["Source"] = parts[0];
                    row["Target"] = parts[1];
                    dt.Rows.Add(row);
                }
            });
            return dt;
        }
    }
}
