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
    public partial class FrmInInformeAuditar : Form
    {
        public FrmInInformeAuditar()
        {
            InitializeComponent();
        }

        private void FrmInInformeAuditar_Load(object sender, EventArgs e)
        {

            string ConsultaRips = Utils.SqlDatos;

            System.Data.DataSet ErroresRips = Conexion.SQLDataSet(ConsultaRips);

            ReportDataSource rdsDetalle = new ReportDataSource("dsDetalle", ErroresRips.Tables[0]);


            this.reportViewer1.RefreshReport();

            this.reportViewer1.LocalReport.DataSources.Clear();

            this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            this.reportViewer1.LocalReport.EnableExternalImages = true;

            this.reportViewer1.LocalReport.DataSources.Add(rdsDetalle);


            string reporte = "Gestion_Rips.Reportes.Rdlc."+ Utils.infNombreInforme;

            this.reportViewer1.LocalReport.ReportEmbeddedResource = reporte;

            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            this.reportViewer1.ZoomMode = ZoomMode.Percent;

            this.reportViewer1.ZoomPercent = 100;

            this.reportViewer1.RefreshReport();
        }
    }
}
