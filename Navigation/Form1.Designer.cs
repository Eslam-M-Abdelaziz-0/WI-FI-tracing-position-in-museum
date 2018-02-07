namespace Navigation
{
    partial class Form1
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.btn_go = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.btn_pre = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(2, 48);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(650, 374);
            this.webBrowser1.TabIndex = 0;
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(165, 12);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(221, 20);
            this.txt_url.TabIndex = 1;
            // 
            // btn_go
            // 
            this.btn_go.Location = new System.Drawing.Point(403, 12);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(42, 23);
            this.btn_go.TabIndex = 2;
            this.btn_go.Text = "GO";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(111, 12);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(48, 20);
            this.btn_next.TabIndex = 3;
            this.btn_next.Text = ">>";
            this.btn_next.UseVisualStyleBackColor = true;
            // 
            // btn_pre
            // 
            this.btn_pre.Location = new System.Drawing.Point(57, 12);
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.Size = new System.Drawing.Size(48, 20);
            this.btn_pre.TabIndex = 4;
            this.btn_pre.Text = "<<";
            this.btn_pre.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 423);
            this.Controls.Add(this.btn_pre);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Navigation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button btn_pre;
    }
}

