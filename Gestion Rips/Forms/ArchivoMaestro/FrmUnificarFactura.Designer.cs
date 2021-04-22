
namespace Gestion_Rips.Forms.ArchivoMaestro
{
    partial class FrmUnificarFactura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUnificarFactura));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblNivelPermitido = new System.Windows.Forms.Label();
            this.LblNombreUsa = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LblCodigoUsaF = new System.Windows.Forms.Label();
            this.BtnActualizar = new System.Windows.Forms.Button();
            this.txtRemi = new System.Windows.Forms.TextBox();
            this.txtFacUnica = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Factura única:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(53, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Remision No:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(53, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "Actualizar";
            // 
            // LblNivelPermitido
            // 
            this.LblNivelPermitido.AutoSize = true;
            this.LblNivelPermitido.Location = new System.Drawing.Point(374, 104);
            this.LblNivelPermitido.Name = "LblNivelPermitido";
            this.LblNivelPermitido.Size = new System.Drawing.Size(33, 13);
            this.LblNivelPermitido.TabIndex = 3;
            this.LblNivelPermitido.Text = "Level";
            // 
            // LblNombreUsa
            // 
            this.LblNombreUsa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblNombreUsa.Location = new System.Drawing.Point(210, 123);
            this.LblNombreUsa.Name = "LblNombreUsa";
            this.LblNombreUsa.Size = new System.Drawing.Size(197, 13);
            this.LblNombreUsa.TabIndex = 4;
            this.LblNombreUsa.Text = "NombreUsa";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(210, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "ID:";
            // 
            // LblCodigoUsaF
            // 
            this.LblCodigoUsaF.AutoSize = true;
            this.LblCodigoUsaF.Location = new System.Drawing.Point(241, 104);
            this.LblCodigoUsaF.Name = "LblCodigoUsaF";
            this.LblCodigoUsaF.Size = new System.Drawing.Size(48, 13);
            this.LblCodigoUsaF.TabIndex = 6;
            this.LblCodigoUsaF.Text = "CodUser";
            // 
            // BtnActualizar
            // 
            this.BtnActualizar.BackColor = System.Drawing.Color.Transparent;
            this.BtnActualizar.BackgroundImage = global::Gestion_Rips.Properties.Resources.icons8_actualizar_30;
            this.BtnActualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnActualizar.FlatAppearance.BorderSize = 0;
            this.BtnActualizar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            this.BtnActualizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.BtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnActualizar.Location = new System.Drawing.Point(56, 105);
            this.BtnActualizar.Name = "BtnActualizar";
            this.BtnActualizar.Size = new System.Drawing.Size(57, 39);
            this.BtnActualizar.TabIndex = 33;
            this.BtnActualizar.TabStop = false;
            this.BtnActualizar.UseVisualStyleBackColor = false;
            this.BtnActualizar.Click += new System.EventHandler(this.BtnActualizar_Click);
            // 
            // txtRemi
            // 
            this.txtRemi.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemi.Location = new System.Drawing.Point(154, 22);
            this.txtRemi.Name = "txtRemi";
            this.txtRemi.Size = new System.Drawing.Size(202, 21);
            this.txtRemi.TabIndex = 34;
            // 
            // txtFacUnica
            // 
            this.txtFacUnica.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFacUnica.Location = new System.Drawing.Point(154, 52);
            this.txtFacUnica.Name = "txtFacUnica";
            this.txtFacUnica.Size = new System.Drawing.Size(202, 21);
            this.txtFacUnica.TabIndex = 35;
            // 
            // FrmUnificarFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(417, 149);
            this.Controls.Add(this.txtFacUnica);
            this.Controls.Add(this.txtRemi);
            this.Controls.Add(this.BtnActualizar);
            this.Controls.Add(this.LblCodigoUsaF);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LblNombreUsa);
            this.Controls.Add(this.LblNivelPermitido);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmUnificarFactura";
            this.Text = "FrmUnificarFactura";
            this.Load += new System.EventHandler(this.FrmUnificarFactura_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LblNivelPermitido;
        private System.Windows.Forms.Label LblNombreUsa;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LblCodigoUsaF;
        private System.Windows.Forms.Button BtnActualizar;
        private System.Windows.Forms.TextBox txtRemi;
        private System.Windows.Forms.TextBox txtFacUnica;
    }
}