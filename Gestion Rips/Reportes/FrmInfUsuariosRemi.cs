using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion_Rips.Clases;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Reporting.WinForms;
using System.Drawing.Imaging;
using System.IO;

namespace Gestion_Rips.Reportes
{
    public partial class FrmInfUsuariosRemi : Form
    {
        public FrmInfUsuariosRemi()
        {
            InitializeComponent();
        }

        private void FrmInfUsuariosRemi_Load(object sender, EventArgs e)
        {

            string UsuariosRips = Utils.SqlDatos;

            System.Data.DataSet ErroresRips = Conexion.SQLDataSet(UsuariosRips);

            ReportDataSource rdsDetalle = new ReportDataSource("dsDetalle", ErroresRips.Tables[0]);


            this.reportViewer1.RefreshReport();
            this.reportViewer1.LocalReport.DataSources.Clear();

            this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            this.reportViewer1.LocalReport.EnableExternalImages = true;

            this.reportViewer1.LocalReport.DataSources.Add(rdsDetalle);

            this.reportViewer1.LocalReport.ReportPath = System.IO.Path.Combine(Application.StartupPath + @"\Reportes\rdlc", "InfReporUserPorRemision.rdlc");


            String reporte = "Gestion_Rips." + Utils.infNombreInforme + ".rdlc";


            this.reportViewer1.LocalReport.ReportEmbeddedResource = reporte;

            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            this.reportViewer1.ZoomMode = ZoomMode.Percent;

            this.reportViewer1.ZoomPercent = 100;
            this.reportViewer1.RefreshReport();
        }
    }
}
