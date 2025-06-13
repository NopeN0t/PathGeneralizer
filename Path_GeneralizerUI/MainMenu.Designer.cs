namespace Path_GeneralizerUI
{
    partial class Main_Menu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)DataView).BeginInit();
            SuspendLayout();
            // 
            // DataView
            // 
            DataView.AllowUserToAddRows = false;
            DataView.AllowUserToDeleteRows = false;
            DataView.AllowUserToResizeColumns = false;
            DataView.AllowUserToResizeRows = false;
            DataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataView.EnableHeadersVisualStyles = false;
            DataView.Location = new Point(12, 12);
            DataView.MultiSelect = false;
            DataView.Name = "DataView";
            DataView.ReadOnly = true;
            DataView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            DataView.RowHeadersVisible = false;
            DataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataView.Size = new Size(374, 224);
            DataView.TabIndex = 0;
            // 
            // Main_Menu
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(784, 361);
            Controls.Add(DataView);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Main_Menu";
            Text = "Path Generalizer";
            ((System.ComponentModel.ISupportInitialize)DataView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DataView;
    }
}
