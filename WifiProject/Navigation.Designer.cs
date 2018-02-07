namespace WifiProject
{
    partial class Navigation
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
            this.btn_pre = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.btn_go = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // btn_pre
            // 
            this.btn_pre.Location = new System.Drawing.Point(12, 12);
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.Size = new System.Drawing.Size(47, 37);
            this.btn_pre.TabIndex = 0;
            this.btn_pre.Text = "<<";
            this.btn_pre.UseVisualStyleBackColor = true;
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(65, 12);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(42, 37);
            this.btn_next.TabIndex = 1;
            this.btn_next.Text = ">>";
            this.btn_next.UseVisualStyleBackColor = true;
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(113, 12);
            this.txt_url.Multiline = true;
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(400, 37);
            this.txt_url.TabIndex = 2;
            // 
            // btn_go
            // 
            this.btn_go.Location = new System.Drawing.Point(519, 12);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(52, 37);
            this.btn_go.TabIndex = 3;
            this.btn_go.Text = "Go";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.button3_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(-1, 55);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(581, 349);
            this.webBrowser1.TabIndex = 4;
            // 
            // Navigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 404);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.btn_pre);
            this.Name = "Navigation";
            this.Text = "Navigation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_pre;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}