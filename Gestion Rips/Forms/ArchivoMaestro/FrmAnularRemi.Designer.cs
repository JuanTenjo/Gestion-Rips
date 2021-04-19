
namespace Gestion_Rips.Forms.Exportar
{
    partial class FrmAnularRemi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAnularRemi));
            this.lblTexto = new System.Windows.Forms.Label();
            this.txtRazonAnul = new System.Windows.Forms.TextBox();
            this.BtnAceptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTexto
            // 
            this.lblTexto.BackColor = System.Drawing.Color.LightSeaGreen;
            this.lblTexto.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTexto.ForeColor = System.Drawing.Color.White;
            this.lblTexto.Location = new System.Drawing.Point(0, -5);
            this.lblTexto.Name = "lblTexto";
            this.lblTexto.Size = new System.Drawing.Size(457, 26);
            this.lblTexto.TabIndex = 6;
            this.lblTexto.Text = "Por favor registre las razones por las cuales anula la remisión: ";
            this.lblTexto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRazonAnul
            // 
            this.txtRazonAnul.Location = new System.Drawing.Point(12, 30);
            this.txtRazonAnul.Multiline = true;
            this.txtRazonAnul.Name = "txtRazonAnul";
            this.txtRazonAnul.Size = new System.Drawing.Size(432, 60);
            this.txtRazonAnul.TabIndex = 7;
            // 
            // BtnAceptar
            // 
            this.BtnAceptar.BackColor = System.Drawing.Color.Transparent;
            this.BtnAceptar.FlatAppearance.BorderSize = 0;
            this.BtnAceptar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkCyan;
            this.BtnAceptar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkCyan;
            this.BtnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAceptar.Location = new System.Drawing.Point(12, 96);
            this.BtnAceptar.Name = "BtnAceptar";
            this.BtnAceptar.Size = new System.Drawing.Size(432, 23);
            this.BtnAceptar.TabIndex = 8;
            this.BtnAceptar.Text = "Listo";
            this.BtnAceptar.UseVisualStyleBackColor = false;
            this.BtnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // FrmAnularRemi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(456, 127);
            this.ControlBox = false;
            this.Controls.Add(this.BtnAceptar);
            this.Controls.Add(this.txtRazonAnul);
            this.Controls.Add(this.lblTexto);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAnularRemi";
            this.Text = "FrmAnularRemi";
            this.Load += new System.EventHandler(this.FrmAnularRemi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTexto;
        private System.Windows.Forms.TextBox txtRazonAnul;
        private System.Windows.Forms.Button BtnAceptar;
    }
}