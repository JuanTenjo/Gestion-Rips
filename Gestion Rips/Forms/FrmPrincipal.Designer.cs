
namespace Gestion_Rips.Forms
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ripsPorRegimenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ripsTodosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.archivoMaestroToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionRipsEspecialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblFecha = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCodUsuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNomUsuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.exportarToolStripMenuItem,
            this.exportarToolStripMenuItem2});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(60, 20);
            this.fileMenu.Text = "&Archivo";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.exitToolStripMenuItem.Text = "&Salir";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            // 
            // exportarToolStripMenuItem
            // 
            this.exportarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportarToolStripMenuItem1,
            this.ripsPorRegimenToolStripMenuItem,
            this.ripsTodosToolStripMenuItem});
            this.exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            this.exportarToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.exportarToolStripMenuItem.Text = "Procesar";
            // 
            // exportarToolStripMenuItem1
            // 
            this.exportarToolStripMenuItem1.Name = "exportarToolStripMenuItem1";
            this.exportarToolStripMenuItem1.Size = new System.Drawing.Size(167, 22);
            this.exportarToolStripMenuItem1.Text = "Rips por Entidad";
            this.exportarToolStripMenuItem1.Click += new System.EventHandler(this.exportarToolStripMenuItem1_Click);
            // 
            // ripsPorRegimenToolStripMenuItem
            // 
            this.ripsPorRegimenToolStripMenuItem.Name = "ripsPorRegimenToolStripMenuItem";
            this.ripsPorRegimenToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.ripsPorRegimenToolStripMenuItem.Text = "Rips por Régimen";
            this.ripsPorRegimenToolStripMenuItem.Click += new System.EventHandler(this.ripsPorRegimenToolStripMenuItem_Click);
            // 
            // ripsTodosToolStripMenuItem
            // 
            this.ripsTodosToolStripMenuItem.Name = "ripsTodosToolStripMenuItem";
            this.ripsTodosToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.ripsTodosToolStripMenuItem.Text = "Rips Todos";
            this.ripsTodosToolStripMenuItem.Click += new System.EventHandler(this.ripsTodosToolStripMenuItem_Click);
            // 
            // exportarToolStripMenuItem2
            // 
            this.exportarToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoMaestroToolStripMenuItem1,
            this.gestionRipsEspecialToolStripMenuItem});
            this.exportarToolStripMenuItem2.Name = "exportarToolStripMenuItem2";
            this.exportarToolStripMenuItem2.Size = new System.Drawing.Size(63, 20);
            this.exportarToolStripMenuItem2.Text = "Exportar";
            // 
            // archivoMaestroToolStripMenuItem1
            // 
            this.archivoMaestroToolStripMenuItem1.Name = "archivoMaestroToolStripMenuItem1";
            this.archivoMaestroToolStripMenuItem1.Size = new System.Drawing.Size(187, 22);
            this.archivoMaestroToolStripMenuItem1.Text = "Gestión Rips Estándar";
            this.archivoMaestroToolStripMenuItem1.Click += new System.EventHandler(this.archivoMaestroToolStripMenuItem1_Click);
            // 
            // gestionRipsEspecialToolStripMenuItem
            // 
            this.gestionRipsEspecialToolStripMenuItem.Name = "gestionRipsEspecialToolStripMenuItem";
            this.gestionRipsEspecialToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.gestionRipsEspecialToolStripMenuItem.Text = "Gestión Rips Especial";
            this.gestionRipsEspecialToolStripMenuItem.Click += new System.EventHandler(this.gestionRipsEspecialToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFecha,
            this.lblCodUsuario,
            this.lblNomUsuario});
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(632, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // lblFecha
            // 
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(118, 17);
            this.lblFecha.Text = "toolStripStatusLabel1";
            this.lblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCodUsuario
            // 
            this.lblCodUsuario.Name = "lblCodUsuario";
            this.lblCodUsuario.Size = new System.Drawing.Size(118, 17);
            this.lblCodUsuario.Text = "toolStripStatusLabel1";
            // 
            // lblNomUsuario
            // 
            this.lblNomUsuario.Name = "lblNomUsuario";
            this.lblNomUsuario.Size = new System.Drawing.Size(118, 17);
            this.lblNomUsuario.Text = "toolStripStatusLabel2";
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FrmPrincipal";
            this.Text = "GAP-RIPS  0.0.1 (03-FEB-2020) *** SIIGHOS PLUS ***";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblFecha;
        private System.Windows.Forms.ToolStripStatusLabel lblCodUsuario;
        private System.Windows.Forms.ToolStripStatusLabel lblNomUsuario;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem archivoMaestroToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ripsPorRegimenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionRipsEspecialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ripsTodosToolStripMenuItem;
    }
}



