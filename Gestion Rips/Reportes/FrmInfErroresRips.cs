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
    public partial class FrmInfErroresRips : Form
    {
        public FrmInfErroresRips()
        {
            InitializeComponent();
        }

        private void FrmInfErroresRips_Load(object sender, EventArgs e)
        {


            string Tercero = "SELECT * FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE [CarAdmin] = '" + Utils.CarAdmin + "'";

            System.Data.DataSet InfoTercero = Conexion.SQLDataSet(Tercero);

            ReportDataSource rdsEmisor = new ReportDataSource("dsEmisor", InfoTercero.Tables[0]);


            string ErroresRipsSQL = Utils.SqlDatos;

            System.Data.DataSet ErroresRips = Conexion.SQLDataSet(ErroresRipsSQL);

            ReportDataSource rdsDetalle = new ReportDataSource("dsDetalle", ErroresRips.Tables[0]);



            string InfoEmpresaData = "SELECT * FROM [BDADMINSIG].[dbo].[Datos informacion de la empresa] WHERE [CodUnico] = '"+ Utils.codUnicoEmpresa +"'";

            System.Data.DataSet InfoEmpresa = Conexion.SQLDataSet(InfoEmpresaData);

            ReportDataSource rdsInfoEmpresa = new ReportDataSource("dsEmpresa", InfoEmpresa.Tables[0]);




            this.reportViewer2.RefreshReport();
            this.reportViewer2.LocalReport.DataSources.Clear();

            this.reportViewer2.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            this.reportViewer2.LocalReport.EnableExternalImages = true;

            this.reportViewer2.LocalReport.DataSources.Add(rdsEmisor);
            this.reportViewer2.LocalReport.DataSources.Add(rdsDetalle);
            this.reportViewer2.LocalReport.DataSources.Add(rdsInfoEmpresa);




            string reporte = "Gestion_Rips.Reportes.Rdlc." + Utils.infNombreInforme;


            this.reportViewer2.LocalReport.ReportEmbeddedResource = "Gestion_Rips.Reportes.Rdlc."+Utils.infNombreInforme+"";

            this.reportViewer2.SetDisplayMode(DisplayMode.PrintLayout);

            this.reportViewer2.ZoomMode = ZoomMode.Percent;

            this.reportViewer2.ZoomPercent = 100;



        }
    }
}
