namespace KeyLog
{
    partial class MainForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.TCode = new System.Windows.Forms.TextBox();
      this.TText = new System.Windows.Forms.TextBox();
      this.LCode = new System.Windows.Forms.Label();
      this.LText = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // TCode
      // 
      this.TCode.Location = new System.Drawing.Point(12, 29);
      this.TCode.Multiline = true;
      this.TCode.Name = "TCode";
      this.TCode.ReadOnly = true;
      this.TCode.Size = new System.Drawing.Size(260, 321);
      this.TCode.TabIndex = 1;
      // 
      // TText
      // 
      this.TText.Location = new System.Drawing.Point(278, 29);
      this.TText.Multiline = true;
      this.TText.Name = "TText";
      this.TText.ReadOnly = true;
      this.TText.Size = new System.Drawing.Size(260, 321);
      this.TText.TabIndex = 2;
      // 
      // LCode
      // 
      this.LCode.AutoSize = true;
      this.LCode.Location = new System.Drawing.Point(9, 13);
      this.LCode.Name = "LCode";
      this.LCode.Size = new System.Drawing.Size(52, 13);
      this.LCode.TabIndex = 3;
      this.LCode.Text = "Key code";
      // 
      // LText
      // 
      this.LText.AutoSize = true;
      this.LText.Location = new System.Drawing.Point(275, 13);
      this.LText.Name = "LText";
      this.LText.Size = new System.Drawing.Size(45, 13);
      this.LText.TabIndex = 4;
      this.LText.Text = "Key text";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(551, 362);
      this.Controls.Add(this.LText);
      this.Controls.Add(this.LCode);
      this.Controls.Add(this.TText);
      this.Controls.Add(this.TCode);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainForm";
      this.Text = "Key Log";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TCode;
    private System.Windows.Forms.TextBox TText;
    private System.Windows.Forms.Label LCode;
    private System.Windows.Forms.Label LText;
  }
}

