#pragma warning disable CS1591
namespace UserConsoleLib
{
    partial class UserConsole
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ListOuput = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.TextboxInput = new System.Windows.Forms.TextBox();
            this.ButtonSubmit = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ListOuput);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(682, 349);
            this.splitContainer1.SplitterDistance = 319;
            this.splitContainer1.TabIndex = 0;
            // 
            // ListOuput
            // 
            this.ListOuput.BackColor = System.Drawing.Color.Black;
            this.ListOuput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListOuput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListOuput.ForeColor = System.Drawing.Color.LightGray;
            this.ListOuput.Location = new System.Drawing.Point(0, 0);
            this.ListOuput.Name = "ListOuput";
            this.ListOuput.ShowLines = false;
            this.ListOuput.ShowPlusMinus = false;
            this.ListOuput.ShowRootLines = false;
            this.ListOuput.Size = new System.Drawing.Size(682, 319);
            this.ListOuput.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TextboxInput);
            this.splitContainer2.Panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ButtonSubmit);
            this.splitContainer2.Size = new System.Drawing.Size(682, 26);
            this.splitContainer2.SplitterDistance = 603;
            this.splitContainer2.TabIndex = 0;
            // 
            // TextboxInput
            // 
            this.TextboxInput.BackColor = System.Drawing.SystemColors.Window;
            this.TextboxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextboxInput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextboxInput.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TextboxInput.Location = new System.Drawing.Point(0, 0);
            this.TextboxInput.Name = "TextboxInput";
            this.TextboxInput.Size = new System.Drawing.Size(603, 26);
            this.TextboxInput.TabIndex = 0;
            this.TextboxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleKeyDown);
            // 
            // ButtonSubmit
            // 
            this.ButtonSubmit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonSubmit.Location = new System.Drawing.Point(0, 0);
            this.ButtonSubmit.Name = "ButtonSubmit";
            this.ButtonSubmit.Size = new System.Drawing.Size(75, 26);
            this.ButtonSubmit.TabIndex = 0;
            this.ButtonSubmit.Text = "Submit";
            this.ButtonSubmit.UseVisualStyleBackColor = true;
            this.ButtonSubmit.Click += new System.EventHandler(this.OnClickSubmit);
            // 
            // UserConsole
            // 
            this.AcceptButton = this.ButtonSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 349);
            this.Controls.Add(this.splitContainer1);
            this.Name = "UserConsole";
            this.ShowIcon = false;
            this.Text = "Console";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        protected System.Windows.Forms.TextBox TextboxInput;
        protected System.Windows.Forms.TreeView ListOuput;
        protected System.Windows.Forms.Button ButtonSubmit;
    }
}