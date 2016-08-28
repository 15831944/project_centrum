namespace project_centrum
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
            this.btn_input = new System.Windows.Forms.Button();
            this.btn_output = new System.Windows.Forms.Button();
            this.txt_status = new System.Windows.Forms.TextBox();
            this.cb_view = new System.Windows.Forms.CheckBox();
            this.cb_mark = new System.Windows.Forms.CheckBox();
            this.cb_section = new System.Windows.Forms.CheckBox();
            this.cb_red = new System.Windows.Forms.CheckBox();
            this.cb_dim = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_line = new System.Windows.Forms.CheckBox();
            this.cb_txt = new System.Windows.Forms.CheckBox();
            this.cb_detail = new System.Windows.Forms.CheckBox();
            this.cb_onoff = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_copy_fast = new System.Windows.Forms.RadioButton();
            this.rb_copy_all = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rb_paste_fast = new System.Windows.Forms.RadioButton();
            this.rb_paste_all = new System.Windows.Forms.RadioButton();
            this.cb_offset = new System.Windows.Forms.CheckBox();
            this.cb_predict = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_input
            // 
            this.btn_input.Location = new System.Drawing.Point(12, 301);
            this.btn_input.Name = "btn_input";
            this.btn_input.Size = new System.Drawing.Size(96, 47);
            this.btn_input.TabIndex = 0;
            this.btn_input.Text = "COPY";
            this.btn_input.UseVisualStyleBackColor = true;
            this.btn_input.Click += new System.EventHandler(this.btn_input_all_Click);
            // 
            // btn_output
            // 
            this.btn_output.Location = new System.Drawing.Point(114, 301);
            this.btn_output.Name = "btn_output";
            this.btn_output.Size = new System.Drawing.Size(97, 47);
            this.btn_output.TabIndex = 0;
            this.btn_output.Text = "PASTE";
            this.btn_output.UseVisualStyleBackColor = true;
            this.btn_output.Click += new System.EventHandler(this.btn_output_Click);
            // 
            // txt_status
            // 
            this.txt_status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_status.Location = new System.Drawing.Point(217, 12);
            this.txt_status.Multiline = true;
            this.txt_status.Name = "txt_status";
            this.txt_status.ReadOnly = true;
            this.txt_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_status.Size = new System.Drawing.Size(253, 718);
            this.txt_status.TabIndex = 1;
            // 
            // cb_view
            // 
            this.cb_view.AutoSize = true;
            this.cb_view.Location = new System.Drawing.Point(15, 180);
            this.cb_view.Name = "cb_view";
            this.cb_view.Size = new System.Drawing.Size(54, 17);
            this.cb_view.TabIndex = 2;
            this.cb_view.Text = "Views";
            this.cb_view.UseVisualStyleBackColor = true;
            // 
            // cb_mark
            // 
            this.cb_mark.AutoSize = true;
            this.cb_mark.Checked = true;
            this.cb_mark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_mark.Location = new System.Drawing.Point(15, 60);
            this.cb_mark.Name = "cb_mark";
            this.cb_mark.Size = new System.Drawing.Size(55, 17);
            this.cb_mark.TabIndex = 3;
            this.cb_mark.Text = "Marks";
            this.cb_mark.UseVisualStyleBackColor = true;
            // 
            // cb_section
            // 
            this.cb_section.AutoSize = true;
            this.cb_section.Checked = true;
            this.cb_section.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_section.Location = new System.Drawing.Point(91, 60);
            this.cb_section.Name = "cb_section";
            this.cb_section.Size = new System.Drawing.Size(67, 17);
            this.cb_section.TabIndex = 4;
            this.cb_section.Text = "Sections";
            this.cb_section.UseVisualStyleBackColor = true;
            // 
            // cb_red
            // 
            this.cb_red.AutoSize = true;
            this.cb_red.Location = new System.Drawing.Point(15, 200);
            this.cb_red.Name = "cb_red";
            this.cb_red.Size = new System.Drawing.Size(143, 17);
            this.cb_red.TabIndex = 5;
            this.cb_red.Text = "RED (* Not found marks)";
            this.cb_red.UseVisualStyleBackColor = true;
            // 
            // cb_dim
            // 
            this.cb_dim.AutoSize = true;
            this.cb_dim.Checked = true;
            this.cb_dim.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_dim.Location = new System.Drawing.Point(91, 100);
            this.cb_dim.Name = "cb_dim";
            this.cb_dim.Size = new System.Drawing.Size(78, 17);
            this.cb_dim.TabIndex = 9;
            this.cb_dim.Text = "Dimentions";
            this.cb_dim.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Reposition:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Create:";
            // 
            // cb_line
            // 
            this.cb_line.AutoSize = true;
            this.cb_line.Checked = true;
            this.cb_line.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_line.Location = new System.Drawing.Point(91, 120);
            this.cb_line.Name = "cb_line";
            this.cb_line.Size = new System.Drawing.Size(51, 17);
            this.cb_line.TabIndex = 14;
            this.cb_line.Text = "Lines";
            this.cb_line.UseVisualStyleBackColor = true;
            // 
            // cb_txt
            // 
            this.cb_txt.AutoSize = true;
            this.cb_txt.Checked = true;
            this.cb_txt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_txt.Location = new System.Drawing.Point(15, 80);
            this.cb_txt.Name = "cb_txt";
            this.cb_txt.Size = new System.Drawing.Size(63, 17);
            this.cb_txt.TabIndex = 15;
            this.cb_txt.Text = "TextFile";
            this.cb_txt.UseVisualStyleBackColor = true;
            // 
            // cb_detail
            // 
            this.cb_detail.AutoSize = true;
            this.cb_detail.Checked = true;
            this.cb_detail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_detail.Location = new System.Drawing.Point(91, 80);
            this.cb_detail.Name = "cb_detail";
            this.cb_detail.Size = new System.Drawing.Size(58, 17);
            this.cb_detail.TabIndex = 16;
            this.cb_detail.Text = "Details";
            this.cb_detail.UseVisualStyleBackColor = true;
            // 
            // cb_onoff
            // 
            this.cb_onoff.AutoSize = true;
            this.cb_onoff.Checked = true;
            this.cb_onoff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_onoff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.cb_onoff.Location = new System.Drawing.Point(15, 15);
            this.cb_onoff.Name = "cb_onoff";
            this.cb_onoff.Size = new System.Drawing.Size(81, 17);
            this.cb_onoff.TabIndex = 17;
            this.cb_onoff.Text = "ON / OFF";
            this.cb_onoff.UseVisualStyleBackColor = true;
            this.cb_onoff.CheckedChanged += new System.EventHandler(this.cb_onoff_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rb_copy_fast);
            this.panel1.Controls.Add(this.rb_copy_all);
            this.panel1.Location = new System.Drawing.Point(12, 354);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 47);
            this.panel1.TabIndex = 18;
            // 
            // rb_copy_fast
            // 
            this.rb_copy_fast.AutoSize = true;
            this.rb_copy_fast.Location = new System.Drawing.Point(3, 27);
            this.rb_copy_fast.Name = "rb_copy_fast";
            this.rb_copy_fast.Size = new System.Drawing.Size(93, 17);
            this.rb_copy_fast.TabIndex = 1;
            this.rb_copy_fast.Text = "Selected (fast)";
            this.rb_copy_fast.UseVisualStyleBackColor = true;
            // 
            // rb_copy_all
            // 
            this.rb_copy_all.AutoSize = true;
            this.rb_copy_all.Checked = true;
            this.rb_copy_all.Location = new System.Drawing.Point(3, 4);
            this.rb_copy_all.Name = "rb_copy_all";
            this.rb_copy_all.Size = new System.Drawing.Size(66, 17);
            this.rb_copy_all.TabIndex = 0;
            this.rb_copy_all.TabStop = true;
            this.rb_copy_all.Text = "All (slow)";
            this.rb_copy_all.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rb_paste_fast);
            this.panel2.Controls.Add(this.rb_paste_all);
            this.panel2.Location = new System.Drawing.Point(114, 354);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(97, 47);
            this.panel2.TabIndex = 19;
            // 
            // rb_paste_fast
            // 
            this.rb_paste_fast.AutoSize = true;
            this.rb_paste_fast.Location = new System.Drawing.Point(4, 27);
            this.rb_paste_fast.Name = "rb_paste_fast";
            this.rb_paste_fast.Size = new System.Drawing.Size(93, 17);
            this.rb_paste_fast.TabIndex = 1;
            this.rb_paste_fast.Text = "Selected (fast)";
            this.rb_paste_fast.UseVisualStyleBackColor = true;
            // 
            // rb_paste_all
            // 
            this.rb_paste_all.AutoSize = true;
            this.rb_paste_all.Checked = true;
            this.rb_paste_all.Location = new System.Drawing.Point(4, 4);
            this.rb_paste_all.Name = "rb_paste_all";
            this.rb_paste_all.Size = new System.Drawing.Size(66, 17);
            this.rb_paste_all.TabIndex = 0;
            this.rb_paste_all.TabStop = true;
            this.rb_paste_all.Text = "All (slow)";
            this.rb_paste_all.UseVisualStyleBackColor = true;
            // 
            // cb_offset
            // 
            this.cb_offset.AutoSize = true;
            this.cb_offset.Location = new System.Drawing.Point(15, 240);
            this.cb_offset.Name = "cb_offset";
            this.cb_offset.Size = new System.Drawing.Size(74, 17);
            this.cb_offset.TabIndex = 20;
            this.cb_offset.Text = "Use offset";
            this.cb_offset.UseVisualStyleBackColor = true;
            // 
            // cb_predict
            // 
            this.cb_predict.AutoSize = true;
            this.cb_predict.Location = new System.Drawing.Point(15, 220);
            this.cb_predict.Name = "cb_predict";
            this.cb_predict.Size = new System.Drawing.Size(155, 17);
            this.cb_predict.TabIndex = 21;
            this.cb_predict.Text = "Predict marks (* Beam only)";
            this.cb_predict.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Advanced:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 736);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_predict);
            this.Controls.Add(this.cb_offset);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cb_onoff);
            this.Controls.Add(this.cb_detail);
            this.Controls.Add(this.cb_txt);
            this.Controls.Add(this.cb_line);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_dim);
            this.Controls.Add(this.cb_red);
            this.Controls.Add(this.cb_section);
            this.Controls.Add(this.cb_mark);
            this.Controls.Add(this.cb_view);
            this.Controls.Add(this.txt_status);
            this.Controls.Add(this.btn_output);
            this.Controls.Add(this.btn_input);
            this.Name = "Form1";
            this.Text = "Centrum DEV";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_input;
        private System.Windows.Forms.Button btn_output;
        private System.Windows.Forms.TextBox txt_status;
        private System.Windows.Forms.CheckBox cb_view;
        private System.Windows.Forms.CheckBox cb_mark;
        private System.Windows.Forms.CheckBox cb_section;
        private System.Windows.Forms.CheckBox cb_red;
        private System.Windows.Forms.CheckBox cb_dim;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cb_line;
        private System.Windows.Forms.CheckBox cb_txt;
        private System.Windows.Forms.CheckBox cb_detail;
        private System.Windows.Forms.CheckBox cb_onoff;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rb_copy_fast;
        private System.Windows.Forms.RadioButton rb_copy_all;
        private System.Windows.Forms.RadioButton rb_paste_fast;
        private System.Windows.Forms.RadioButton rb_paste_all;
        private System.Windows.Forms.CheckBox cb_offset;
        private System.Windows.Forms.CheckBox cb_predict;
        private System.Windows.Forms.Label label2;
    }
}

