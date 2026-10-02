namespace Modless
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            groupBox1 = new System.Windows.Forms.GroupBox();
            button4 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            button6 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            button8 = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            comboBox3 = new System.Windows.Forms.ComboBox();
            comboBox2 = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            groupBox4 = new System.Windows.Forms.GroupBox();
            textBox2 = new System.Windows.Forms.TextBox();
            textBox1 = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            groupBox5 = new System.Windows.Forms.GroupBox();
            comboBox6 = new System.Windows.Forms.ComboBox();
            comboBox7 = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(331, 53);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "1";
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(249, 22);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(75, 23);
            button4.TabIndex = 11;
            button4.Text = "기둥 생성";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(168, 22);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(75, 23);
            button3.TabIndex = 8;
            button3.Text = "천장 생성";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(87, 22);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "벽 생성";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(6, 22);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "바닥 생성";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button7);
            groupBox2.Controls.Add(button8);
            groupBox2.Location = new System.Drawing.Point(12, 71);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(331, 53);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "2 - 룸 마감";
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(168, 22);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(75, 23);
            button6.TabIndex = 8;
            button6.Text = "천장 생성";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(87, 22);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(75, 23);
            button7.TabIndex = 5;
            button7.Text = "벽 생성";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(6, 22);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(75, 23);
            button8.TabIndex = 2;
            button8.Text = "바닥 생성";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(comboBox3);
            groupBox3.Controls.Add(comboBox2);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(comboBox1);
            groupBox3.Controls.Add(label1);
            groupBox3.Location = new System.Drawing.Point(12, 159);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(200, 106);
            groupBox3.TabIndex = 13;
            groupBox3.TabStop = false;
            groupBox3.Text = "유형 선택";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new System.Drawing.Point(73, 76);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(121, 23);
            comboBox3.TabIndex = 15;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new System.Drawing.Point(73, 48);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(121, 23);
            comboBox2.TabIndex = 3;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 79);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(31, 15);
            label3.TabIndex = 14;
            label3.Text = "천장";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 51);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(19, 15);
            label2.TabIndex = 2;
            label2.Text = "벽";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(73, 20);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(121, 23);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 23);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(31, 15);
            label1.TabIndex = 0;
            label1.Text = "바닥";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox2);
            groupBox4.Controls.Add(textBox1);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(label5);
            groupBox4.Location = new System.Drawing.Point(231, 159);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(200, 79);
            groupBox4.TabIndex = 16;
            groupBox4.TabStop = false;
            groupBox4.Text = "높이 입력";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(94, 48);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(100, 23);
            textBox2.TabIndex = 16;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(94, 20);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(100, 23);
            textBox1.TabIndex = 15;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 51);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(31, 15);
            label4.TabIndex = 14;
            label4.Text = "천장";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(6, 23);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(19, 15);
            label5.TabIndex = 2;
            label5.Text = "벽";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(comboBox6);
            groupBox5.Controls.Add(comboBox7);
            groupBox5.Controls.Add(label6);
            groupBox5.Controls.Add(label7);
            groupBox5.Location = new System.Drawing.Point(450, 159);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(200, 79);
            groupBox5.TabIndex = 17;
            groupBox5.TabStop = false;
            groupBox5.Text = "레벨 선택";
            // 
            // comboBox6
            // 
            comboBox6.FormattingEnabled = true;
            comboBox6.Location = new System.Drawing.Point(73, 48);
            comboBox6.Name = "comboBox6";
            comboBox6.Size = new System.Drawing.Size(121, 23);
            comboBox6.TabIndex = 15;
            comboBox6.SelectedIndexChanged += comboBox6_SelectedIndexChanged;
            // 
            // comboBox7
            // 
            comboBox7.FormattingEnabled = true;
            comboBox7.Location = new System.Drawing.Point(73, 20);
            comboBox7.Name = "comboBox7";
            comboBox7.Size = new System.Drawing.Size(121, 23);
            comboBox7.TabIndex = 3;
            comboBox7.SelectedIndexChanged += comboBox7_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 51);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(31, 15);
            label6.TabIndex = 14;
            label6.Text = "상부";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(6, 23);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(31, 15);
            label7.TabIndex = 2;
            label7.Text = "하부";
            // 
            // MainForm
            // 
            ClientSize = new System.Drawing.Size(719, 354);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "MainForm";
            Text = "Sample Test";
            Load += MainForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}
