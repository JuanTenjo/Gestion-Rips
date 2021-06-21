using Gestion_Rips.Clases;
using Gestion_Rips.Forms.Exportar;
using Gestion_Rips.Forms.RipsPorRegimen;
using Gestion_Rips.Forms.RipsTodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Rips.Forms
{
    public partial class FrmPrincipal : Form
    {
      //  private int childFormNumber = 0;

        public FrmPrincipal()
        {
            InitializeComponent();
        }


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.BaseDeDatosPrincipal = "ACDATOXPSQL";

                Conexion.conexionACCESS = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SIIGHOSPLUS\LogPlus.LogSip;Jet OLEDB:Database Password=SIIGHOS33";

                Utils.SqlDatos = "SELECT * FROM [Local registro del usuario]";

                OleDbDataReader dr = Conexion.AccessDataReaderOLEDB(Utils.SqlDatos);

                if (dr.HasRows)
                {
                    dr.Read();

                    // Se procede a validar las credenciales de acceso al Servidor SQL Server
                    // Y verificar el tipo de cliente de SQL Server

                    Conexion.servidor = dr["NomServi"].ToString();
                    Conexion.username = dr["NomUsar"].ToString();
                    Conexion.password = dr["PassWusa"].ToString();


                    //Conexion.servidor = @"HAROLD-PC\PC";
                    //Conexion.username = "sa";
                    //Conexion.password = "SIIGHOS33*";

                    Conexion.conexionSQL = "Server=" + Conexion.servidor + "; " +
                                           "Initial Catalog=" + Utils.BaseDeDatosPrincipal + ";" +
                                           "User ID= " + Conexion.username + "; " +
                                           "Password=" + Conexion.password;

                    Utils.codUsuario = dr["CodigUsar"].ToString();
                    Utils.nomUsuario = dr["NombreUsar"].ToString();
                    Utils.nivelPermiso = dr["NivelPermiso"].ToString();
                    Utils.codUnicoEmpresa = dr["CodRegEn"].ToString(); // CodEnti
                    Utils.CodAplicacion = dr["CodApli"].ToString();


                    this.lblFecha.Text = DateTime.Now.ToString("dddd dd 'de' MMMM 'de' yyyy") + "   -";
                    this.lblCodUsuario.Text = Utils.codUsuario;
                    this.lblNomUsuario.Text = Utils.nomUsuario;

                    Utils.SqlDatos = @"SELECT CodiMinSalud, NitCCEmpresa, NomEmpresa, TipoDocEmp, TelPrin " +
                                   "FROM [BDADMINSIG].[dbo].[Datos informacion de la empresa] " +
                                   "WHERE CodUnico = @codUnicoEmpresa";

                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@codUnicoEmpresa", SqlDbType.VarChar, 2) { Value = Utils.codUnicoEmpresa }
                    };

                    SqlDataReader Sqldr = Conexion.SQLDataReader(Utils.SqlDatos, parameters);

                    if (Sqldr.HasRows)
                    {
                        Sqldr.Read();
                        Utils.codMinSalud = Sqldr["CodiMinSalud"].ToString();
                        Utils.nitEmpresa = Sqldr["NitCCEmpresa"].ToString();
                        Utils.nomEmpresa = Sqldr["NomEmpresa"].ToString();
                        Utils.tipoDocEmp = Sqldr["TipoDocEmp"].ToString();
                        Utils.TelEmpresa = Sqldr["TelPrin"].ToString();
                    }


                    Sqldr.Close();
                }
                else
                {
                    this.Close();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al abrir el formulario principal" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exportarToolStripMenuItem1_Click(object sender, EventArgs e) //Exportar Rips Entandar
        {
            Utils.BaseDeDatosPrincipal = "ACDATOXPSQL";

            Conexion.conexionSQL = "Server=" + Conexion.servidor + "; " +
                                   "Initial Catalog=" + Utils.BaseDeDatosPrincipal + ";" +
                                   "User ID= " + Conexion.username + "; " +
                                   "Password=" + Conexion.password;

            FrmExportarSedarips frmExportarSedarips = new FrmExportarSedarips();
            frmExportarSedarips.ShowDialog();
        }
        private void ripsPorRegimenToolStripMenuItem_Click(object sender, EventArgs e) //Exportar Rips por Regimen
        {
            Utils.BaseDeDatosPrincipal = "ACDATOXPSQL";

            Conexion.conexionSQL = "Server=" + Conexion.servidor + "; " +
                                   "Initial Catalog=" + Utils.BaseDeDatosPrincipal + ";" +
                                   "User ID= " + Conexion.username + "; " +
                                   "Password=" + Conexion.password;

            FrmRipsRegimen FrmRipRegimen = new FrmRipsRegimen();
            FrmRipRegimen.ShowDialog();
        }

        private void archivoMaestroToolStripMenuItem1_Click(object sender, EventArgs e) //Maestro con Rips Estandar
        {
            Utils.BaseDeDatosPrincipal = "DARIPSXPSQL";


            Conexion.conexionSQL = "Server=" + Conexion.servidor + "; " +
                                   "Initial Catalog=" + Utils.BaseDeDatosPrincipal + ";" +
                                   "User ID= " + Conexion.username + "; " +
                                   "Password=" + Conexion.password;

            FrmArchivoMaestro FrmArchivoMaestro = new FrmArchivoMaestro();
            FrmArchivoMaestro.Text = "FrmGestionRipsEstandar";
            FrmArchivoMaestro.ShowDialog();

        }

        private void gestionRipsEspecialToolStripMenuItem_Click(object sender, EventArgs e)  //Maestro con Rips Especial
        {
            Utils.BaseDeDatosPrincipal = "DARIPSESSQL";

            Conexion.conexionSQL = "Server=" + Conexion.servidor + "; " +
                                   "Initial Catalog=" + Utils.BaseDeDatosPrincipal + ";" +
                                   "User ID= " + Conexion.username + "; " +
                                   "Password=" + Conexion.password;

            FrmArchivoMaestro FrmArchivoMaestro = new FrmArchivoMaestro();
            FrmArchivoMaestro.Text = "FrmGestionRipsEspecial";
            FrmArchivoMaestro.ShowDialog();
        }

        private void ripsTodosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.BaseDeDatosPrincipal = "ACDATOXPSQL";

            Conexion.conexionSQL = "Server=" + Conexion.servidor + "; " +
                                   "Initial Catalog=" + Utils.BaseDeDatosPrincipal + ";" +
                                   "User ID= " + Conexion.username + "; " +
                                   "Password=" + Conexion.password;


            FrmExportarSedaripsTodos frmExportarSedaripsTodos = new FrmExportarSedaripsTodos();
            frmExportarSedaripsTodos.ShowDialog();

        }
    }
}
