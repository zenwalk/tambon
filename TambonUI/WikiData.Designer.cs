﻿namespace De.AHoerstemeier.Tambon.UI
{
    partial class WikiData
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
            if ( disposing && (components != null) )
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
            this.btnStat = new System.Windows.Forms.Button();
            this.btnCountInterwiki = new System.Windows.Forms.Button();
            this.btnTestGet = new System.Windows.Forms.Button();
            this.btnTestSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStat
            // 
            this.btnStat.Location = new System.Drawing.Point(12, 12);
            this.btnStat.Name = "btnStat";
            this.btnStat.Size = new System.Drawing.Size(98, 23);
            this.btnStat.TabIndex = 0;
            this.btnStat.Text = "Statistics";
            this.btnStat.UseVisualStyleBackColor = true;
            this.btnStat.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnCountInterwiki
            // 
            this.btnCountInterwiki.Location = new System.Drawing.Point(12, 41);
            this.btnCountInterwiki.Name = "btnCountInterwiki";
            this.btnCountInterwiki.Size = new System.Drawing.Size(98, 23);
            this.btnCountInterwiki.TabIndex = 1;
            this.btnCountInterwiki.Text = "by Language";
            this.btnCountInterwiki.UseVisualStyleBackColor = true;
            this.btnCountInterwiki.Click += new System.EventHandler(this.btnCountInterwiki_Click);
            // 
            // btnTestGet
            // 
            this.btnTestGet.Location = new System.Drawing.Point(12, 70);
            this.btnTestGet.Name = "btnTestGet";
            this.btnTestGet.Size = new System.Drawing.Size(98, 23);
            this.btnTestGet.TabIndex = 2;
            this.btnTestGet.Text = "Test Get";
            this.btnTestGet.UseVisualStyleBackColor = true;
            this.btnTestGet.Click += new System.EventHandler(this.btnTestGet_Click);
            // 
            // btnTestSet
            // 
            this.btnTestSet.Location = new System.Drawing.Point(12, 99);
            this.btnTestSet.Name = "btnTestSet";
            this.btnTestSet.Size = new System.Drawing.Size(98, 23);
            this.btnTestSet.TabIndex = 3;
            this.btnTestSet.Text = "Test Set";
            this.btnTestSet.UseVisualStyleBackColor = true;
            this.btnTestSet.Click += new System.EventHandler(this.btnTestSet_Click);
            // 
            // WikiData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnTestSet);
            this.Controls.Add(this.btnTestGet);
            this.Controls.Add(this.btnCountInterwiki);
            this.Controls.Add(this.btnStat);
            this.Name = "WikiData";
            this.Text = "WikiData";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStat;
        private System.Windows.Forms.Button btnCountInterwiki;
        private System.Windows.Forms.Button btnTestGet;
        private System.Windows.Forms.Button btnTestSet;
    }
}