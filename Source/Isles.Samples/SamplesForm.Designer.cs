﻿namespace Isles.Samples
{
    partial class SamplesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Samples = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Samples
            // 
            this.Samples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Samples.FormattingEnabled = true;
            this.Samples.Location = new System.Drawing.Point(0, 0);
            this.Samples.Name = "Samples";
            this.Samples.Size = new System.Drawing.Size(258, 472);
            this.Samples.TabIndex = 0;
            // 
            // SamplesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 480);
            this.Controls.Add(this.Samples);
            this.Name = "SamplesForm";
            this.Text = "Isles Test Tools";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox Samples;

    }
}