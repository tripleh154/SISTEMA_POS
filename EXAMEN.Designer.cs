
namespace PantallaDeLogin
{
    partial class EXAMEN
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
            this.BTinicio = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TBcontrasena = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TBnombre = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BTinicio
            // 
            this.BTinicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTinicio.Location = new System.Drawing.Point(309, 239);
            this.BTinicio.Margin = new System.Windows.Forms.Padding(2);
            this.BTinicio.Name = "BTinicio";
            this.BTinicio.Size = new System.Drawing.Size(267, 39);
            this.BTinicio.TabIndex = 9;
            this.BTinicio.Text = "INICIAR";
            this.BTinicio.UseVisualStyleBackColor = true;
            this.BTinicio.Click += new System.EventHandler(this.BTinicio_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(103, 121);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "CONTRASEÑA";
            // 
            // TBcontrasena
            // 
            this.TBcontrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBcontrasena.Location = new System.Drawing.Point(269, 115);
            this.TBcontrasena.Margin = new System.Windows.Forms.Padding(2);
            this.TBcontrasena.Name = "TBcontrasena";
            this.TBcontrasena.PasswordChar = '*';
            this.TBcontrasena.Size = new System.Drawing.Size(323, 26);
            this.TBcontrasena.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(75, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "NOMBRE DE USUARIO";
            // 
            // TBnombre
            // 
            this.TBnombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBnombre.Location = new System.Drawing.Point(269, 47);
            this.TBnombre.Margin = new System.Windows.Forms.Padding(2);
            this.TBnombre.Name = "TBnombre";
            this.TBnombre.Size = new System.Drawing.Size(323, 26);
            this.TBnombre.TabIndex = 5;
            // 
            // EXAMEN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTinicio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBcontrasena);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBnombre);
            this.Name = "EXAMEN";
            this.Text = "EXAMEN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTinicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBcontrasena;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBnombre;
    }
}