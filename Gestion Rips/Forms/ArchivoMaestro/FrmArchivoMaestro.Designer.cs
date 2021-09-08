
namespace Gestion_Rips.Forms.Exportar
{
    partial class FrmArchivoMaestro
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmArchivoMaestro));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboNomAdmin = new System.Windows.Forms.ComboBox();
            this.txtCodigAdmin = new System.Windows.Forms.TextBox();
            this.txtCodigIPS = new System.Windows.Forms.TextBox();
            this.txtNomIPS = new System.Windows.Forms.TextBox();
            this.txtNitCCIPS = new System.Windows.Forms.TextBox();
            this.BtnBorrar = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNitCCAdmin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCodigoUsaF = new System.Windows.Forms.Label();
            this.lblNombreUsa = new System.Windows.Forms.Label();
            this.lblNivelPermitido = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.DataGridRemi = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTotalRemisiones = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnUnificar = new System.Windows.Forms.Button();
            this.BtnBorrarRemi = new System.Windows.Forms.Button();
            this.BtnExportar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnAnular = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.BtnModifica = new System.Windows.Forms.Button();
            this.btnNuevaRemi = new System.Windows.Forms.Button();
            this.BarraExportar = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.BrnCerrarForm = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridRemi)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.cboNomAdmin);
            this.groupBox1.Controls.Add(this.txtCodigAdmin);
            this.groupBox1.Controls.Add(this.txtCodigIPS);
            this.groupBox1.Controls.Add(this.txtNomIPS);
            this.groupBox1.Controls.Add(this.txtNitCCIPS);
            this.groupBox1.Controls.Add(this.BtnBorrar);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtNitCCAdmin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(5, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1060, 139);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cboNomAdmin
            // 
            this.cboNomAdmin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNomAdmin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNomAdmin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNomAdmin.FormattingEnabled = true;
            this.cboNomAdmin.Location = new System.Drawing.Point(229, 98);
            this.cboNomAdmin.Margin = new System.Windows.Forms.Padding(4);
            this.cboNomAdmin.Name = "cboNomAdmin";
            this.cboNomAdmin.Size = new System.Drawing.Size(593, 26);
            this.cboNomAdmin.TabIndex = 16;
            this.cboNomAdmin.SelectedIndexChanged += new System.EventHandler(this.cboNomAdmin_SelectedIndexChanged);
            this.cboNomAdmin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboNomAdmin_KeyPress);
            // 
            // txtCodigAdmin
            // 
            this.txtCodigAdmin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigAdmin.Location = new System.Drawing.Point(824, 98);
            this.txtCodigAdmin.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigAdmin.Name = "txtCodigAdmin";
            this.txtCodigAdmin.Size = new System.Drawing.Size(221, 26);
            this.txtCodigAdmin.TabIndex = 15;
            this.txtCodigAdmin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodigAdmin_KeyPress);
            // 
            // txtCodigIPS
            // 
            this.txtCodigIPS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigIPS.Location = new System.Drawing.Point(824, 44);
            this.txtCodigIPS.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigIPS.Name = "txtCodigIPS";
            this.txtCodigIPS.ReadOnly = true;
            this.txtCodigIPS.Size = new System.Drawing.Size(221, 26);
            this.txtCodigIPS.TabIndex = 9;
            // 
            // txtNomIPS
            // 
            this.txtNomIPS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomIPS.Location = new System.Drawing.Point(231, 44);
            this.txtNomIPS.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomIPS.Name = "txtNomIPS";
            this.txtNomIPS.ReadOnly = true;
            this.txtNomIPS.Size = new System.Drawing.Size(592, 26);
            this.txtNomIPS.TabIndex = 8;
            // 
            // txtNitCCIPS
            // 
            this.txtNitCCIPS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNitCCIPS.Location = new System.Drawing.Point(12, 44);
            this.txtNitCCIPS.Margin = new System.Windows.Forms.Padding(4);
            this.txtNitCCIPS.Name = "txtNitCCIPS";
            this.txtNitCCIPS.ReadOnly = true;
            this.txtNitCCIPS.Size = new System.Drawing.Size(219, 26);
            this.txtNitCCIPS.TabIndex = 7;
            // 
            // BtnBorrar
            // 
            this.BtnBorrar.BackColor = System.Drawing.Color.LightSeaGreen;
            this.BtnBorrar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBorrar.ForeColor = System.Drawing.Color.White;
            this.BtnBorrar.Location = new System.Drawing.Point(227, 71);
            this.BtnBorrar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BtnBorrar.Name = "BtnBorrar";
            this.BtnBorrar.Size = new System.Drawing.Size(597, 27);
            this.BtnBorrar.TabIndex = 11;
            this.BtnBorrar.Text = "Nombre de la administradora de planes";
            this.BtnBorrar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(820, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 27);
            this.label3.TabIndex = 6;
            this.label3.Text = "Código SGSSS";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNitCCAdmin
            // 
            this.txtNitCCAdmin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNitCCAdmin.Location = new System.Drawing.Point(12, 98);
            this.txtNitCCAdmin.Margin = new System.Windows.Forms.Padding(4);
            this.txtNitCCAdmin.Name = "txtNitCCAdmin";
            this.txtNitCCAdmin.Size = new System.Drawing.Size(219, 26);
            this.txtNitCCAdmin.TabIndex = 13;
            this.txtNitCCAdmin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNitCCAdmin_KeyPress);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(227, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(597, 27);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nombre de la IPS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 71);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(216, 27);
            this.label6.TabIndex = 10;
            this.label6.Text = "Nit o CC";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(820, 71);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(227, 27);
            this.label4.TabIndex = 12;
            this.label4.Text = "Código SGSSS";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 27);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nit o CC";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(5, 142);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1060, 28);
            this.label7.TabIndex = 16;
            this.label7.Text = "Listado de remisiones de archivos enviados a la administradora seleccionada";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(103, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 18);
            this.label8.TabIndex = 17;
            this.label8.Text = "Usuario:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCodigoUsaF
            // 
            this.lblCodigoUsaF.AutoSize = true;
            this.lblCodigoUsaF.BackColor = System.Drawing.Color.Transparent;
            this.lblCodigoUsaF.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoUsaF.ForeColor = System.Drawing.Color.Black;
            this.lblCodigoUsaF.Location = new System.Drawing.Point(180, 18);
            this.lblCodigoUsaF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigoUsaF.Name = "lblCodigoUsaF";
            this.lblCodigoUsaF.Size = new System.Drawing.Size(0, 21);
            this.lblCodigoUsaF.TabIndex = 18;
            this.lblCodigoUsaF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNombreUsa
            // 
            this.lblNombreUsa.BackColor = System.Drawing.Color.Transparent;
            this.lblNombreUsa.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreUsa.ForeColor = System.Drawing.Color.Black;
            this.lblNombreUsa.Location = new System.Drawing.Point(8, 53);
            this.lblNombreUsa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombreUsa.Name = "lblNombreUsa";
            this.lblNombreUsa.Size = new System.Drawing.Size(296, 46);
            this.lblNombreUsa.TabIndex = 19;
            this.lblNombreUsa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNivelPermitido
            // 
            this.lblNivelPermitido.AutoSize = true;
            this.lblNivelPermitido.BackColor = System.Drawing.Color.Transparent;
            this.lblNivelPermitido.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNivelPermitido.ForeColor = System.Drawing.Color.Black;
            this.lblNivelPermitido.Location = new System.Drawing.Point(941, 604);
            this.lblNivelPermitido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNivelPermitido.Name = "lblNivelPermitido";
            this.lblNivelPermitido.Size = new System.Drawing.Size(0, 17);
            this.lblNivelPermitido.TabIndex = 20;
            this.lblNivelPermitido.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(9, 17);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 16);
            this.label12.TabIndex = 21;
            this.label12.Text = "Nueva";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label13.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(89, 17);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 16);
            this.label13.TabIndex = 22;
            this.label13.Text = "Modificar";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(168, 17);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 16);
            this.label15.TabIndex = 24;
            this.label15.Text = "Actualiza";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label16.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(329, 17);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 16);
            this.label16.TabIndex = 25;
            this.label16.Text = "Exportar";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label18.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(249, 17);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(76, 16);
            this.label18.TabIndex = 27;
            this.label18.Text = "Cerrar";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label19.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(409, 17);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(76, 16);
            this.label19.TabIndex = 28;
            this.label19.Text = "Anular";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataGridRemi
            // 
            this.DataGridRemi.AllowUserToAddRows = false;
            this.DataGridRemi.AllowUserToDeleteRows = false;
            this.DataGridRemi.AllowUserToOrderColumns = true;
            this.DataGridRemi.AllowUserToResizeRows = false;
            this.DataGridRemi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridRemi.BackgroundColor = System.Drawing.Color.White;
            this.DataGridRemi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridRemi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridRemi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridRemi.DefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridRemi.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DataGridRemi.Location = new System.Drawing.Point(5, 172);
            this.DataGridRemi.Margin = new System.Windows.Forms.Padding(4);
            this.DataGridRemi.MultiSelect = false;
            this.DataGridRemi.Name = "DataGridRemi";
            this.DataGridRemi.ReadOnly = true;
            this.DataGridRemi.RowHeadersVisible = false;
            this.DataGridRemi.RowHeadersWidth = 51;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DataGridRemi.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.DataGridRemi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridRemi.Size = new System.Drawing.Size(1060, 177);
            this.DataGridRemi.TabIndex = 40;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightSeaGreen;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(755, 393);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(211, 28);
            this.label9.TabIndex = 17;
            this.label9.Text = "Total remisiones vigentes";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalRemisiones
            // 
            this.txtTotalRemisiones.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalRemisiones.Location = new System.Drawing.Point(965, 393);
            this.txtTotalRemisiones.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotalRemisiones.Name = "txtTotalRemisiones";
            this.txtTotalRemisiones.ReadOnly = true;
            this.txtTotalRemisiones.Size = new System.Drawing.Size(97, 27);
            this.txtTotalRemisiones.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(484, 17);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 16);
            this.label10.TabIndex = 41;
            this.label10.Text = "Borrar";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(567, 17);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 16);
            this.label5.TabIndex = 43;
            this.label5.Text = "Unificar";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUnificar
            // 
            this.btnUnificar.BackColor = System.Drawing.Color.Transparent;
            this.btnUnificar.BackgroundImage = global::Gestion_Rips.Properties.Resources.icons8_agrupar_objetos_30;
            this.btnUnificar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUnificar.FlatAppearance.BorderSize = 0;
            this.btnUnificar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnUnificar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnUnificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnificar.Location = new System.Drawing.Point(567, 37);
            this.btnUnificar.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnificar.Name = "btnUnificar";
            this.btnUnificar.Size = new System.Drawing.Size(76, 70);
            this.btnUnificar.TabIndex = 44;
            this.btnUnificar.TabStop = false;
            this.btnUnificar.UseVisualStyleBackColor = false;
            this.btnUnificar.Click += new System.EventHandler(this.btnUnificar_Click);
            // 
            // BtnBorrarRemi
            // 
            this.BtnBorrarRemi.BackColor = System.Drawing.Color.Transparent;
            this.BtnBorrarRemi.BackgroundImage = global::Gestion_Rips.Properties.Resources.icons8_delete_30;
            this.BtnBorrarRemi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnBorrarRemi.FlatAppearance.BorderSize = 0;
            this.BtnBorrarRemi.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.BtnBorrarRemi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtnBorrarRemi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBorrarRemi.Location = new System.Drawing.Point(489, 37);
            this.BtnBorrarRemi.Margin = new System.Windows.Forms.Padding(4);
            this.BtnBorrarRemi.Name = "BtnBorrarRemi";
            this.BtnBorrarRemi.Size = new System.Drawing.Size(76, 70);
            this.BtnBorrarRemi.TabIndex = 42;
            this.BtnBorrarRemi.TabStop = false;
            this.BtnBorrarRemi.UseVisualStyleBackColor = false;
            this.BtnBorrarRemi.Click += new System.EventHandler(this.BtnBorrarRemi_Click);
            // 
            // BtnExportar
            // 
            this.BtnExportar.BackColor = System.Drawing.Color.Transparent;
            this.BtnExportar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnExportar.BackgroundImage")));
            this.BtnExportar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnExportar.FlatAppearance.BorderSize = 0;
            this.BtnExportar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.BtnExportar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExportar.Location = new System.Drawing.Point(329, 37);
            this.BtnExportar.Margin = new System.Windows.Forms.Padding(4);
            this.BtnExportar.Name = "BtnExportar";
            this.BtnExportar.Size = new System.Drawing.Size(76, 70);
            this.BtnExportar.TabIndex = 39;
            this.BtnExportar.TabStop = false;
            this.BtnExportar.UseVisualStyleBackColor = false;
            this.BtnExportar.Click += new System.EventHandler(this.BtnExportar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Transparent;
            this.btnCerrar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCerrar.BackgroundImage")));
            this.btnCerrar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Location = new System.Drawing.Point(249, 37);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(4);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(76, 70);
            this.btnCerrar.TabIndex = 37;
            this.btnCerrar.TabStop = false;
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnAnular
            // 
            this.btnAnular.BackColor = System.Drawing.Color.Transparent;
            this.btnAnular.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAnular.BackgroundImage")));
            this.btnAnular.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAnular.FlatAppearance.BorderSize = 0;
            this.btnAnular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnAnular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnAnular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnular.Location = new System.Drawing.Point(409, 37);
            this.btnAnular.Margin = new System.Windows.Forms.Padding(4);
            this.btnAnular.Name = "btnAnular";
            this.btnAnular.Size = new System.Drawing.Size(76, 70);
            this.btnAnular.TabIndex = 36;
            this.btnAnular.TabStop = false;
            this.btnAnular.UseVisualStyleBackColor = false;
            this.btnAnular.Click += new System.EventHandler(this.btnAnular_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.Transparent;
            this.btnActualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnActualizar.BackgroundImage")));
            this.btnActualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnActualizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Location = new System.Drawing.Point(169, 37);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(4);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(76, 70);
            this.btnActualizar.TabIndex = 33;
            this.btnActualizar.TabStop = false;
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // BtnModifica
            // 
            this.BtnModifica.BackColor = System.Drawing.Color.Transparent;
            this.BtnModifica.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnModifica.BackgroundImage")));
            this.BtnModifica.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnModifica.FlatAppearance.BorderSize = 0;
            this.BtnModifica.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.BtnModifica.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtnModifica.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnModifica.Location = new System.Drawing.Point(89, 37);
            this.BtnModifica.Margin = new System.Windows.Forms.Padding(4);
            this.BtnModifica.Name = "BtnModifica";
            this.BtnModifica.Size = new System.Drawing.Size(76, 70);
            this.BtnModifica.TabIndex = 30;
            this.BtnModifica.TabStop = false;
            this.BtnModifica.UseVisualStyleBackColor = false;
            this.BtnModifica.Click += new System.EventHandler(this.BtnModifica_Click);
            // 
            // btnNuevaRemi
            // 
            this.btnNuevaRemi.BackColor = System.Drawing.Color.Transparent;
            this.btnNuevaRemi.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNuevaRemi.BackgroundImage")));
            this.btnNuevaRemi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNuevaRemi.FlatAppearance.BorderSize = 0;
            this.btnNuevaRemi.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnNuevaRemi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnNuevaRemi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevaRemi.Location = new System.Drawing.Point(9, 37);
            this.btnNuevaRemi.Margin = new System.Windows.Forms.Padding(4);
            this.btnNuevaRemi.Name = "btnNuevaRemi";
            this.btnNuevaRemi.Size = new System.Drawing.Size(76, 70);
            this.btnNuevaRemi.TabIndex = 29;
            this.btnNuevaRemi.TabStop = false;
            this.btnNuevaRemi.UseVisualStyleBackColor = false;
            this.btnNuevaRemi.Click += new System.EventHandler(this.btnNuevaRemi_Click);
            // 
            // BarraExportar
            // 
            this.BarraExportar.Location = new System.Drawing.Point(5, 357);
            this.BarraExportar.Margin = new System.Windows.Forms.Padding(4);
            this.BarraExportar.Name = "BarraExportar";
            this.BarraExportar.Size = new System.Drawing.Size(1060, 28);
            this.BarraExportar.TabIndex = 45;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.BrnCerrarForm);
            this.groupBox2.Controls.Add(this.btnNuevaRemi);
            this.groupBox2.Controls.Add(this.BtnModifica);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnUnificar);
            this.groupBox2.Controls.Add(this.btnActualizar);
            this.groupBox2.Controls.Add(this.BtnBorrarRemi);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.btnAnular);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.btnCerrar);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.BtnExportar);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Location = new System.Drawing.Point(5, 422);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(743, 121);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(655, 16);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 16);
            this.label11.TabIndex = 46;
            this.label11.Text = "Salir";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BrnCerrarForm
            // 
            this.BrnCerrarForm.BackColor = System.Drawing.Color.Transparent;
            this.BrnCerrarForm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BrnCerrarForm.BackgroundImage")));
            this.BrnCerrarForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BrnCerrarForm.FlatAppearance.BorderSize = 0;
            this.BrnCerrarForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrnCerrarForm.Location = new System.Drawing.Point(655, 37);
            this.BrnCerrarForm.Margin = new System.Windows.Forms.Padding(4);
            this.BrnCerrarForm.Name = "BrnCerrarForm";
            this.BrnCerrarForm.Size = new System.Drawing.Size(76, 70);
            this.BrnCerrarForm.TabIndex = 45;
            this.BrnCerrarForm.UseVisualStyleBackColor = false;
            this.BrnCerrarForm.Click += new System.EventHandler(this.BrnCerrarForm_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblNombreUsa);
            this.groupBox3.Controls.Add(this.lblCodigoUsaF);
            this.groupBox3.Location = new System.Drawing.Point(755, 422);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(311, 121);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            // 
            // FrmArchivoMaestro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1075, 550);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BarraExportar);
            this.Controls.Add(this.txtTotalRemisiones);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.DataGridRemi);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblNivelPermitido);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmArchivoMaestro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GESTION DEL ARCHIVO MAESTRO ";
            this.Load += new System.EventHandler(this.FrmArchivoMaestro_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridRemi)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCodigAdmin;
        private System.Windows.Forms.TextBox txtCodigIPS;
        private System.Windows.Forms.TextBox txtNomIPS;
        private System.Windows.Forms.TextBox txtNitCCIPS;
        private System.Windows.Forms.Label BtnBorrar;
        private System.Windows.Forms.TextBox txtNitCCAdmin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCodigoUsaF;
        private System.Windows.Forms.Label lblNombreUsa;
        private System.Windows.Forms.Label lblNivelPermitido;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnNuevaRemi;
        private System.Windows.Forms.Button BtnModifica;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnAnular;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button BtnExportar;
        private System.Windows.Forms.DataGridView DataGridRemi;
        private System.Windows.Forms.ComboBox cboNomAdmin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTotalRemisiones;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BtnBorrarRemi;
        private System.Windows.Forms.Button btnUnificar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar BarraExportar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button BrnCerrarForm;
    }
}