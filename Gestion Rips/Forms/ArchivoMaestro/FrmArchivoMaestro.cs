using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gestion_Rips.Clases;
using System.Data.SqlClient;
using System.IO;

namespace Gestion_Rips.Forms.Exportar
{
    public partial class FrmArchivoMaestro : Form
    {
        public FrmArchivoMaestro()
        {
            InitializeComponent();
        }

        int Bandera = 0;
        string ManuelTari = "";

        #region Eventos
        private void cboNomAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Bandera == 1)
            {
                CargarDatosAdminPlanes();
            }
        }

        #endregion

        #region ComboBox


        private void CargarComboBox()
        {
            try
            {
                this.cboNomAdmin.DataSource = null;
                this.cboNomAdmin.Items.Clear();

                DataSet AdminPLanes = Conexion.SQLDataSet("SELECT [Datos administradoras de planes].CodInterno, [Datos administradoras de planes].NomAdmin, [Datos administradoras de planes].CodAdmin, [Datos administradoras de planes].NitCC, [Datos administradoras de planes].ManualTari " +
                                                         " FROM [DARIPSXPSQL].[dbo].[Datos administradoras de planes] " +
                                                         " ORDER BY [Datos administradoras de planes].NomAdmin;");


                if (AdminPLanes != null && AdminPLanes.Tables.Count > 0)
                {
                    this.cboNomAdmin.DataSource = AdminPLanes.Tables[0];
                    this.cboNomAdmin.ValueMember = "CodInterno";
                    this.cboNomAdmin.DisplayMember = "NomAdmin";
                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al cargar los combobox" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Funciones

        private int ElimDatosLocales(string NT)
        {
            try
            {
                //'Permite eliminar todos los datos de una tabla local

                Utils.SqlDatos = "DELETE FROM '" + NT + "'";

                Boolean EstaEliDato = Conexion.SQLDelete(Utils.SqlDatos);

                if (EstaEliDato)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ElimDatosLocales del " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int ActualMaestro(string R)
        {
            try
            {
                string ARC, CRips, Para01, NAr;
                Double TolR;

                //'Abrimos la tabla nombre de archivos

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] WHERE ConseArchivo = '" + R + "' ";

                SqlDataReader TabNomArchi = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TabNomArchi.HasRows == false)
                {
                    //'El número d remisión no se pudo encontrar en el sistema
                    return 0;
                }
                else
                {
                    TabNomArchi.Read();
                    if (Convert.ToBoolean(TabNomArchi["ActualRemi"].ToString()) == true) //aqui
                    {
                        //   'Al archivo ya se le corrió el proceso de actualización
                        TabNomArchi.Close();
                        return 1;
                    }
                    else
                    {
                        TabNomArchi.Close();
                        return 2;
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ActualMaestro del " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int CerrarRemision(string R, string U)
        {
            try
            {
                string SqlNomArchi;

                SqlNomArchi = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] WHERE [ConseArchivo] = '" + R + "'";

                SqlDataReader TabNomArchi = Conexion.SQLDataReader(SqlNomArchi);

                if (TabNomArchi.HasRows == false)
                {
                    return 0;
                }
                else
                {

                    string Date = DateTime.Now.ToString("yyyy-MM-dd");

                    Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo maestro] SET CerraRemi = 1, CodModi = '" + U + "', FecModi = '" + Date + "' WHERE [ConseArchivo] = '" + R + "' ";

                    Boolean EstaAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    if (EstaAct)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

                TabNomArchi.Close();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion: Cerrar Remision" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int RegisAccion(string Rm, string A, string Rz, string F, string Us)
        {
            try
            {
                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos control de remisiones] " +
                    "(" +
                    "CodiMas," +
                     "AcReal," +
                     "RazReal," +
                     "VezAccion," +
                     "FecRegis," +
                     "CodiRegis" +
                     ")" +
                     "VALUES" +
                     "(" +
                     "'" + Rm + "'," +
                     "'" + A + "'," +
                     "'" + Rz + "'," +
                     "'" + 1 + "'," +
                     "'" + F + "'," +
                     "'" + Us + "'" +
                     ")";

                Boolean EstadoInsertMovi = Conexion.SqlInsert(Utils.SqlDatos);

                if (EstadoInsertMovi)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion: RegisAccion" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private int AnularRemision(string R, string Rz, string F, string U)
        {
            try
            {
                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] WHERE ConseArchivo = '" + R + "'";

                SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux1.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo maestro] SET AnulRemi = 1, RazAnul = '" + Rz + "', CodAnul = '" + U + "', FecAnul = '" + F + "' WHERE ConseArchivo = '" + R + "'";

                    Boolean EstaAnul = Conexion.SQLUpdate(Utils.SqlDatos);

                    if (EstaAnul)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion: AnularRemision en el" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int ActualArchiControl(string NR, string CIPS, string FeN)
        {
            try
            {
                string ARC, CRips, Para01, NAr, SqlARControl, SqlNomArchi;
                double TolR = 0;
                int ConverCodigo;

                Para01 = NR;

                //Abrimos la tabla nombre de archivos

                //********************** 02 de octubre  de 2009 ****************

                SqlNomArchi = "SELECT [Datos nombres de archivos].CodArchivo, [Datos nombres de archivos].CodRIPS " +
                              "  FROM [DARIPSXPSQL].[dbo].[Datos nombres de archivos] " +
                              "  ORDER BY [Datos nombres de archivos].CodArchivo; ";

                SqlDataReader TabNomArchi = Conexion.SQLDataReader(SqlNomArchi);

                if (TabNomArchi.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    while (TabNomArchi.Read())
                    {
                        ARC = TabNomArchi["CodArchivo"].ToString();
                        CRips = TabNomArchi["CodRIPS"].ToString();

                        switch (ARC)
                        {
                            case "01":
                                TolR = 0;
                                break;
                            case "02": //'Totaliza los de archivo de control
                                SqlDataReader reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "03": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo usuarios] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "04": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "05": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de consulta] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "06": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de procedimientos] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "07": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de hospitalizacion] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "08": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de observacion urgencias] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "9": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de recien nacido] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "10": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de medicamentos] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            case "11": //'Totaliza los de archivo de control
                                reader = Conexion.SQLDataReader("SELECT COUNT(NumRemi) as TolNumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de otros servicios] WHERE NumRemi = '" + Para01 + "'");
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    TolR = Convert.ToDouble(reader["TolNumRemi"].ToString());
                                }
                                else
                                {
                                    TolR = 0;
                                }
                                break;
                            default:
                                break;
                        }



                        NAr = CRips + NR;

                        if (TolR > 0)
                        {
                            SqlARControl = " SELECT [Datos archivo de control].* " +
                                          " FROM [DARIPSXPSQL].[dbo].[Datos archivo de control] " +
                                          " WHERE ((([Datos archivo de control].CodArchivo) = '" + NAr + "')) " +
                                          " ORDER BY [Datos archivo de control].CodArchivo; ";

                            SqlDataReader TabARControl = Conexion.SQLDataReader(SqlARControl);

                            if (TabARControl.HasRows == false)
                            {
                                //SE VA A CREAR POR PRIMERA VEZ

                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de control] " +
                                                    "(" +
                                                    "NumRemi, " +
                                                    "CodIPS, " +
                                                    "FechaRemi, " +
                                                    "CodArchivo, " +
                                                    "TotalRegis " +
                                                    ") " +
                                                    "VALUES" +
                                                    "(" +
                                                    "'" + NR + "'," +
                                                    "'" + CIPS + "'," +
                                                    "'" + Convert.ToDateTime(FeN).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + NAr + "'," +
                                                    "'" + TolR + "'" +
                                                    ")";

                                Boolean EstadoInsert = Conexion.SqlInsert(Utils.SqlDatos);
                            }
                            else
                            {
                                //Ya existe, por lo tanto de debe modificar
                                Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de control] SET " +
                                                "CodIPS = '" + CIPS + "', " +
                                                "FechaRemi = '" + Convert.ToDateTime(FeN).ToString("yyyy-MM-dd") + "', " +
                                                "CodArchivo  = '" + NAr + "', " +
                                                "TotalRegis = '" + TolR + "' " +
                                                "WHERE [Datos archivo de control].CodArchivo = '" + NAr + "'";

                                Boolean EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);


                            } //fiNAL TabARControl.HasRows == false

                        } //TolR > 0

                    } // Final While

                    return 1;
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ActualArchiControl del" + "\r";
                Utils.Informa += "Módulo gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

        }

        private double TotalGrupo(string R)
        {
            try
            {
                //'Permite saber si el valor total del archivo grupo de un a remisión

                string TemRe, Estandatos, SqlFactu, SqlAgrupa;
                //'*********************** 02 de octubre  de 2009 ****************

                SqlAgrupa = "SELECT Sum([Datos archivo de servicios agrupados].Valtotal) AS SumaDeValtotal " +
                            "FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                            "WHERE ((([Datos archivo de servicios agrupados].NumRemi) = '" + R + "'));";

                SqlDataReader TabAgrupa = Conexion.SQLDataReader(SqlAgrupa);

                if (TabAgrupa.HasRows == false)
                {
                    // Los nombre de la tabla fueron borrados
                    return -2;
                }
                else
                {
                    TabAgrupa.Read();
                    return Convert.ToDouble(TabAgrupa["SumaDeValtotal"].ToString());
                }

                TabAgrupa.Close();


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la  función: TotalGrupo del" + "\r";
                Utils.Informa += "Módulo gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private double TotalTransac(string R)
        {
            try
            {
                string TemRe, Estandatos, SqlFactu, SqlAgrupa;
                double ValTotal = 0;
                //aqui
                //'*********************** 02 de octubre  de 2009 ****************

                SqlFactu = "SELECT Sum(([ValorNeto] + [Copago])) AS TolFactura " +
                           " FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] " +
                           " WHERE ((([Datos archivo de transacciones].NumRemi) = '" + R + "'));";

                SqlDataReader TabFactu = Conexion.SQLDataReader(SqlFactu);

                if (TabFactu.HasRows == false)
                {
                    // Los nombre de la tabla fueron borrados
                    return -2;
                }
                else
                {
                    TabFactu.Read();
                    if (string.IsNullOrWhiteSpace(TabFactu["TolFactura"].ToString()))
                    {
                        TabFactu.Close();
                        return -2;
                    }
                    else
                    {
                        ValTotal = Convert.ToDouble(TabFactu["TolFactura"].ToString());
                        TabFactu.Close();
                        return ValTotal;
                    }

                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion: TotalTransac del " + "\r";
                Utils.Informa += "Módulo gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int AgruparOtrosServi(string RM, Int32 TM)
        {
            try
            {
                string Estandatos, TemRe, F, CD, I, SqlOtros, FunGr;

                double VT, Uniol;

                Int16 Esta = 0;

                SqlOtros = "SELECT [Datos archivo de otros servicios].NumRemi, [Datos archivo de otros servicios].NumFactur,  " +
                                " [Datos archivo de otros servicios].CodIPS, [Datos archivo de otros servicios].CodiServi,  " +
                                " Sum([Datos archivo de otros servicios].Cantidad) AS SumaDeCantidad, Sum([ValorTotal])/ Sum(CASE WHEN[Cantidad] = 0 THEN 1 ELSE[Cantidad] END) AS Unitario, " +
                                " Sum([Datos archivo de otros servicios].ValorTotal) AS SumaDeValorTotal  " +
                                " FROM [DARIPSXPSQL].[dbo].[Datos archivo de otros servicios] " +
                                " GROUP BY [Datos archivo de otros servicios].NumRemi, [Datos archivo de otros servicios].NumFactur,  " +
                                " [Datos archivo de otros servicios].CodIPS, [Datos archivo de otros servicios].CodiServi  " +
                                " HAVING ((([Datos archivo de otros servicios].NumRemi) = '" + RM + "')) " +
                                " ORDER BY [Datos archivo de otros servicios].NumRemi; ";


                SqlDataReader TabOtros = Conexion.SQLDataReader(SqlOtros);

                if (TabOtros.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    // 'Se empieza a grabar en la tabla de agrupados, porque ya se eliminaron los datos y n puede haber repetidos
                    while (TabOtros.Read())
                    {
                        //  'Proceda a agregar el grupo a la remisión

                        CD = TabOtros["CodiServi"].ToString();


                        FunGr = GrupoProceServi(CD, TM);

                        if (FunGr != "-1")
                        {

                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                                                "(" +
                                                "NumRemi, " +
                                                "NumFactur, " +
                                                "CodIPS, " +
                                                "CodConcepto, " +
                                                "CantiGrupo, " +
                                                "ValUnita, " +
                                                "Valtotal" +
                                                ") " +
                                                "VALUES" +
                                                "(" +
                                                "'" + TabOtros["NumRemi"].ToString() + "'," +
                                                "'" + TabOtros["NumFactur"].ToString() + "'," +
                                                "'" + TabOtros["CodIPS"].ToString() + "'," +
                                                "'" + FunGr + "'," +
                                                "'" + TabOtros["SumaDeCantidad"].ToString() + "'," +
                                                "'" + TabOtros["Unitario"].ToString() + "'," +
                                                "'" + TabOtros["SumaDeValorTotal"].ToString() + "'" +
                                                ")";

                            Boolean sqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (sqlInsert)
                            {
                                Esta = 1;
                            }
                            else
                            {
                                Esta = 0;
                            }
                        }


                    }

                    return Esta;

                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función:AgruparOtrosServi del " + "\r";
                Utils.Informa += "Módul gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

        }

        private int AgruparMedicamentos(string RM, Int32 TM)
        {
            try
            {
                string Estandatos, TemRe, F, CD, I, SqlMedicamentos, FunGr;

                double VT, Uniol;

                Int16 Esta = 0;

                SqlMedicamentos = "SELECT [Datos archivo de medicamentos].NumRemi, [Datos archivo de medicamentos].NumFactur,  " +
                                " [Datos archivo de medicamentos].CodIPS, [Datos archivo de medicamentos].TipoMedica,  " +
                                " Sum([Datos archivo de medicamentos].NumUnidad) AS SumaDeNumUnidad, " +
                                " Sum([ValorTotal])/ Sum(CASE WHEN[NumUnidad] = 0 THEN 1 ELSE[NumUnidad] END) AS Uni, " +
                                " Sum([Datos archivo de medicamentos].ValorTotal) AS SumaDeValorTotal " +
                                " FROM [DARIPSXPSQL].[dbo].[Datos archivo de medicamentos] " +
                                " GROUP BY [Datos archivo de medicamentos].NumRemi, [Datos archivo de medicamentos].NumFactur, " +
                                " [Datos archivo de medicamentos].CodIPS, [Datos archivo de medicamentos].TipoMedica " +
                                " HAVING ((([Datos archivo de medicamentos].NumRemi) = '" + RM + "')); ";


                SqlDataReader TabMedicamentos = Conexion.SQLDataReader(SqlMedicamentos);

                if (TabMedicamentos.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    // 'Se empieza a grabar en la tabla de agrupados, porque ya se eliminaron los datos y n puede haber repetidos
                    while (TabMedicamentos.Read())
                    {
                        //  'Proceda a agregar el grupo a la remisión
                        if (Convert.ToInt32(TabMedicamentos["TipoMedica"].ToString()) == 1)
                        {
                            //Es medicamento pos
                            FunGr = "12";
                        }
                        else
                        {
                            FunGr = "13";
                        } //'Final de TablaAux5![TipoMedica] = 1
                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                                            "(" +
                                            "NumRemi, " +
                                            "NumFactur, " +
                                            "CodIPS, " +
                                            "CodConcepto, " +
                                            "CantiGrupo, " +
                                            "ValUnita, " +
                                            "Valtotal" +
                                            ") " +
                                            "VALUES" +
                                            "(" +
                                            "'" + TabMedicamentos["NumRemi"].ToString() + "'," +
                                            "'" + TabMedicamentos["NumFactur"].ToString() + "'," +
                                            "'" + TabMedicamentos["CodIPS"].ToString() + "'," +
                                            "'" + FunGr + "'," +
                                            "'" + TabMedicamentos["SumaDeNumUnidad"].ToString() + "'," +
                                            "'" + TabMedicamentos["Uni"].ToString() + "'," +
                                            "'" + TabMedicamentos["SumaDeValorTotal"].ToString() + "'" +
                                            ")";

                        Boolean sqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                        if (sqlInsert)
                        {
                            Esta = 1;
                        }
                        else
                        {
                            Esta = 0;
                        }

                    }

                    return Esta;

                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el función:AgruparMedicamentos del  " + "\r";
                Utils.Informa += "Módul gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

        }

        private int AgruparProce(string Rm, int TM)
        {
            try
            {
                //'Permite agrupas los registros hecho en la tabla de procedimientos a una remisión

                //'*********************** 02 de octubre  de 2009 ****************

                string SqlProce, CD;
                string FunReg;
                int esta = 0;

                SqlProce = "SELECT [Datos archivo de procedimientos].NumRemi, [Datos archivo de procedimientos].NumFactur, " +
                        "[Datos archivo de procedimientos].CodIPS, [Datos archivo de procedimientos].CodProce, " +
                        "Sum([Datos archivo de procedimientos].ValorProce) AS SumaDeValorProce, " +
                        "Count([Datos archivo de procedimientos].IndiceRIPSAP) AS CuentaDeIndiceRIPSAP " +
                        "FROM [DARIPSXPSQL].[dbo].[Datos archivo de procedimientos] " +
                        "GROUP BY [Datos archivo de procedimientos].NumRemi, [Datos archivo de procedimientos].NumFactur, " +
                        "[Datos archivo de procedimientos].CodIPS, [Datos archivo de procedimientos].CodProce " +
                        "HAVING ((([Datos archivo de procedimientos].NumRemi) = '" + Rm + "' ))" +
                        "ORDER BY [Datos archivo de procedimientos].NumRemi;";

                SqlDataReader TabProce = Conexion.SQLDataReader(SqlProce);

                if (TabProce.HasRows == false)
                {
                    //El numeor de Remision no se encuentra
                    return 0;
                }
                else
                {
                    //Se empieza a grabar en la tabla de agrupados, porque ya se eliminaron los datos y n puede haber repetidos


                    while (TabProce.Read())
                    {
                        CD = TabProce["CodProce"].ToString();
                        FunReg = GrupoProceServi(CD, TM);

                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                                            "(" +
                                            "NumRemi, " +
                                            "NumFactur, " +
                                            "CodIPS, " +
                                            "CodConcepto, " +
                                            "CantiGrupo, " +
                                            "ValUnita, " +
                                            "Valtotal" +
                                            ") " +
                                            "VALUES" +
                                            "(" +
                                            "'" + TabProce["NumRemi"].ToString() + "'," +
                                            "'" + TabProce["NumFactur"].ToString() + "'," +
                                            "'" + TabProce["CodIPS"].ToString() + "'," +
                                            "'" + FunReg + "'," +
                                            "'" + TabProce["CuentaDeIndiceRIPSAP"].ToString() + "'," +
                                            "'" + Convert.ToDouble(TabProce["SumaDeValorProce"].ToString()) / Convert.ToDouble(TabProce["CuentaDeIndiceRIPSAP"].ToString()) + "'," +
                                            "'" + Convert.ToDouble(TabProce["SumaDeValorProce"].ToString()) + "'" +
                                            ")";

                        Boolean sqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                        if (sqlInsert)
                        {
                            esta = 1;
                        }
                        else
                        {
                            esta = 0;
                        }

                    }

                    TabProce.Close();
                    return esta;

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función:AgruparProce del" + "\r";
                Utils.Informa += "Módulo gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private string GrupoProceServi(string C, Int32 T)
        {

            try
            {
                //'Permite devolver el grupo al cual pertenece un procedimient o servicio
                //'de acuerdo al código tarifario.

                //'A partir del 02 de octubre de 2009 este dato se debe buscar en la tabla de catal;ogo de facturacion
                //'qporque es enesta donde deben estar bien definidas estas cos
                //'       HERNANDO ESTO TE LO EVITARIAS SI CREARAS UN CAMPO CON EL GRUPO EN LA TABLA DE OTROS PROCEDIMIENTOS, COSA
                //'       QUE CUANDO SE SELECCIONAN LOS RIPS LO TRAIGA

                //'On Error GoTo Error_GrupoProceServi

                string Estandatos;
                int ConverCodigo;
                string SqlCatalogo;

                SqlCatalogo = "SELECT [Datos catalogo de servicios].GrupoServi, [Datos catalogo de servicios].CodInterno, ";
                SqlCatalogo = SqlCatalogo + "[Datos catalogo de servicios].CodiSOAT, [Datos catalogo de servicios].CodiISS, ";
                SqlCatalogo = SqlCatalogo + "[Datos catalogo de servicios].CodiCUPS ";
                SqlCatalogo = SqlCatalogo + "FROM [ACDATOXPSQL].[dbo].[Datos catalogo de servicios] ";


                switch (T)
                {
                    case 1: //Busca por el manual interno IPS
                        SqlCatalogo = SqlCatalogo + "WHERE CodInterno = '" + C + "'";
                        break;
                    case 2: //Busca por el manual SOAT
                        SqlCatalogo = SqlCatalogo + "WHERE CodiSOAT = '" + C + "'";
                        break;
                    case 3: //Busca por el manual ISS
                        SqlCatalogo = SqlCatalogo + "WHERE CodiISS = '" + C + "'";
                        break;
                    case 4: //Busca por el manual CUPS
                        SqlCatalogo = SqlCatalogo + "WHERE CodiCUPS = '" + C + "'";
                        break;
                    default:
                        break;
                }

                SqlDataReader TabCatalogo = Conexion.SQLDataReader(SqlCatalogo);

                if (TabCatalogo.HasRows == false)
                {
                    return "06";
                }
                else
                {
                    TabCatalogo.Read();
                    //Devuelva grupos
                    return TabCatalogo["GrupoServi"].ToString();
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcionGrupoProceServi del Módulo " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "-1";
            }

        }

        private int AgruparConsultas(string Rm, int TM)
        {
            try
            {
                //'Permite agrupas los registros hecho en la tabla de consultas a una remisión
                string SqlConsultas, GruDefec;

                GruDefec = "01";
                int Esta = 0;

                //'*********************** 02 de octubre  de 2009 ****************
                //'Hago una consulta de agrupamiento, ya que sabemos que todas las consultas se clasifican en el grupo 01, mientras no cambien las reglas de juego

                SqlConsultas = "SELECT [Datos archivo de consulta].NumRemi, [Datos archivo de consulta].NumFactur, " +
                                "[Datos archivo de consulta].CodIPS, Avg([Datos archivo de consulta].ValorConsul) AS PromedioDeValorConsul, " +
                                "Count([Datos archivo de consulta].IndiceRIPSAC) As CuentaDeIndiceRIPSAC " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos archivo de consulta] " +
                                "GROUP BY [Datos archivo de consulta].NumRemi, [Datos archivo de consulta].NumFactur, " +
                                "[Datos archivo de consulta].CodIPS " +
                                "HAVING ((([Datos archivo de consulta].NumRemi)= '" + Rm + "' ));";

                SqlDataReader TabConsultas = Conexion.SQLDataReader(SqlConsultas);

                if (TabConsultas.HasRows == false)
                {
                    //El numero de Remision no se encutra
                    return 0;
                }
                else
                {
                    while (TabConsultas.Read())
                    {
                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                                        "(" +
                                        "NumRemi, " +
                                        "NumFactur, " +
                                        "CodIPS, " +
                                        "CodConcepto, " +
                                        "CantiGrupo, " +
                                        "ValUnita, " +
                                        "Valtotal" +
                                        ") " +
                                        "VALUES" +
                                        "(" +
                                        "'" + TabConsultas["NumRemi"].ToString() + "'," +
                                        "'" + TabConsultas["NumFactur"].ToString() + "'," +
                                        "'" + TabConsultas["CodIPS"].ToString() + "'," +
                                        "'" + GruDefec + "'," +
                                        "'" + TabConsultas["CuentaDeIndiceRIPSAC"].ToString() + "'," +
                                        "'" + TabConsultas["PromedioDeValorConsul"].ToString() + "'," +
                                        "'" + Convert.ToDouble(TabConsultas["PromedioDeValorConsul"].ToString()) * Convert.ToDouble(TabConsultas["CuentaDeIndiceRIPSAC"].ToString()) + "'" +
                                        ")";

                        Boolean sqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                        if (sqlInsert)
                        {
                            Esta = 1;
                        }
                        else
                        {
                            Esta = 0;
                        }
                    }

                    TabConsultas.Close();
                    return Esta;

                } //Fin TabConsultas.HasRows 

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion AgruparConsultas  " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                string NAP, NR, UniRuta, Ruta = "", CEsp, SubDir, EstasdoRemi, R, NT, NE, NomArchi, NLA, Estandatos, TAE, DirArchi, RutVer;
                int FunAct, FunElim, FunCopy, P, B, M, FunEliT, DVF, InD, SobrEscri, NuT;
                Double TolReg;

                double[] CanReg = new double[11];
                string[] NomAr = new string[11];





                Utils.Titulo01 = "Control para exportar archivos RIPS";


                if (string.IsNullOrWhiteSpace(txtCodigIPS.Text) || string.IsNullOrWhiteSpace(txtCodigIPS.Text))
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "código de SGSSS, el cual permita " + "\r";
                    Utils.Informa += "crear la remisión de envío." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cboNomAdmin.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "seleccionado el nombre de la" + "\r";
                    Utils.Informa += "cadministradora de planes." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Revise si seleccionó la remisión de envio

                if (DataGridRemi.SelectedRows.Count > 0)
                {

                    NR = DataGridRemi.SelectedCells[0].Value.ToString();

                }
                else
                {
                    Utils.Informa = "Lo siento  pero  usted no ha seleccionado " + "\r";
                    Utils.Informa += "la remisión de envío a exportar." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Validamo si la remisión seleccionada está cerrada

                EstasdoRemi = DataGridRemi.SelectedCells[9].Value.ToString();

                if (Convert.ToBoolean(EstasdoRemi) == false)
                {
                    Utils.Informa = "Lo siento pero la remisión " + NR + "\r";
                    Utils.Informa += "se encuentra abierta, por tanto no se" + "\r";
                    Utils.Informa += "le puede exportar los archivos RIPS." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //'Revisamos si la remisión ya fue actualizada

                FunAct = ActualMaestro(NR);

                switch (FunAct)
                {
                    case -1:
                        return;
                        break;
                    case 0:
                        Utils.Informa = "Error fatal, el número de remisión " + NR + "\r";
                        Utils.Informa += "no se pudo encontrar en este sistema" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        break;
                    case 1:
                        //'Ya se le corrió el proceso de actualización
                        break;
                    case 2:
                        Utils.Informa = "Lo siento pero a la remisión No. " + NR + "\r";
                        Utils.Informa += "no se le ha ejecutado el proceso de actualización" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        break;
                }

                NAP = cboNomAdmin.Text;

                Utils.Informa = "¿Usted desea EXPORTAR los archivos" + "\r";
                Utils.Informa += "de la remisión " + NR + " registrada a la" + "\r";
                Utils.Informa += "administradora de planes@" + NAP + "?" + "\r";
                var RES = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (RES == DialogResult.Yes)
                {


                    //   'Proceda a correr el proceso de copia de archivos a las tablas temporales
                    //1. Eliminar lo que haya en ellas

                    //FunElim = EliminarTodosTemp();  Esta funcion la comentareo porque estaba eliminando tablas termporales de acces las cuales ya no se utilizan 


                    //FunCopy = CopiarTodosTemp(NR)  Esta funcion la comentareo porque estaba rellenando tablas termporales de acces las cuales ya no se utilizan

                    if (string.IsNullOrWhiteSpace(txtCodigAdmin.Text))
                    {
                        UniRuta = @"C:\RIPS\PARTIC\";
                    }
                    else
                    {
                        UniRuta = @"C:\RIPS\" + txtCodigAdmin.Text + @"\";
                    }

                    Utils.Informa = "Los archivos se copiaran en la siguiente ruta: " + "\r";
                    Utils.Informa += UniRuta + "  Presiona:" + "\r";
                    Utils.Informa += "Si = Para que se guarden en la anterior ruta " + "\r";
                    Utils.Informa += "NO = Para seleccionar una nueva carpeta " + "\r";

                    var Res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (Res == DialogResult.Yes)
                    {
                        //'Revisamos si el directorio digitado existe
                        if (!Directory.Exists(UniRuta))
                        {
                            //Si no existe lo creamos
                            Directory.CreateDirectory(UniRuta);

                            Ruta = UniRuta;

                        }
                        else
                        {
                            Ruta = UniRuta;
                        }
                    }
                    else
                    {
                        //Procede a escoger una nueva carpeta donde se guardaran los archivos

                        CEsp = @"\";

                        var fbd = new FolderBrowserDialog();
                        DialogResult result = fbd.ShowDialog();
                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {
                            Ruta = fbd.SelectedPath;

                            if (string.IsNullOrWhiteSpace(Ruta))
                            {
                                Ruta = UniRuta;
                            }
                            else
                            {
                                Ruta = fbd.SelectedPath;
                            }

                        }

                        string UtimoCaracterRuta = Ruta.Substring(Ruta.Length - 1, 1);

                        if (UtimoCaracterRuta != CEsp)
                        {
                            Ruta = Ruta + @"\";
                        }
                    }


                    //Proceda a exportar los archivos
                    //Abrimos la tabla nombre de archivos

                    Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos nombres de archivos]";

                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (TablaAux1.HasRows == false)
                    {

                        Utils.Informa = "Error fatal, la tabla de nombres de archivos" + "\r";
                        Utils.Informa += "le han sido borrado sus datos. " + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        NuT = 0;

                        while (TablaAux1.Read())
                        {
                            TolReg = 0;
                            NT = TablaAux1["NomTabRemo"].ToString(); //Nombre de la tabla en sql
                            NE = TablaAux1["NomEspicifica"].ToString(); //Nombre de la especificación
                            InD = Convert.ToInt32(TablaAux1["Indispensable"]); //Nombre de la especificación
                            NLA = TablaAux1["NomArchivo"].ToString(); //Nombre de la especificación
                            TAE = TablaAux1["CodRIPS"].ToString(); //Nombre de la especificación

                            Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";

                            SqlDataReader TablaAux3 = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (TablaAux3.HasRows == false)
                            {
                                //No tiene nada la tabla
                                if (InD == 1)
                                {
                                    //'Debe parar el proceso ya que es un archivo importante
                                    Utils.Informa = "Lo siento pero el archivo" + "\r";
                                    Utils.Informa += NLA + "\r";
                                    Utils.Informa += "no está definido y es necesario para la exportación" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                //Cuente el número de registros que contiene el archivo o tabla

                                while (TablaAux3.Read())
                                {
                                    TolReg += 1;
                                }



                            } // fINAL TablaAux3.HasRows == false

                            if (TolReg > 0)
                            {
                                NuT += 1;
                                CanReg[NuT] = TolReg;
                                NomAr[NuT] = NLA;

                                //Se procede a exportar
                                NomArchi = TAE + NR + ".txt";

                                if (string.IsNullOrEmpty(Ruta) == false)
                                {
                                    // 'El archivo se va agregar nuevo, mientras no se presente error


                                    switch (NT)
                                    {
                                        case "Datos archivo de control":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(FechaRemi)), LTRIM(RTRIM(CodArchivo)), LTRIM(RTRIM(TotalRegis)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de transacciones":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(CodIPS)),LTRIM(RTRIM(RazonSocial)),LTRIM(RTRIM(TipIdenti)),LTRIM(RTRIM(NumIdenti)),LTRIM(RTRIM(NumFactur)),LTRIM(RTRIM(FecFactur)),LTRIM(RTRIM(FecInicio)),LTRIM(RTRIM(FecFinal)),LTRIM(RTRIM(CodAdmin)),LTRIM(RTRIM(NomAdmin)),LTRIM(RTRIM(NumContra)),LTRIM(RTRIM(PlanBene)),LTRIM(RTRIM(NumPoli)),LTRIM(RTRIM(Copago)),LTRIM(RTRIM(ValorComi)),LTRIM(RTRIM(ValorDes)),LTRIM(RTRIM(ValorNeto)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo usuarios":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)), LTRIM(RTRIM(CodAdmin)), LTRIM(RTRIM(TipUsuario)),LTRIM(RTRIM(Apellido1)),LTRIM(RTRIM(Apellido2)),LTRIM(RTRIM(Nombre1)),LTRIM(RTRIM(Nombre2)),LTRIM(RTRIM(Edad)),LTRIM(RTRIM(EdadMedi)),LTRIM(RTRIM(Sexo)),LTRIM(RTRIM(CodDpto)),LTRIM(RTRIM(CodMuni)),LTRIM(RTRIM(ZonaResi)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de servicios agrupados":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(CodConcepto)), LTRIM(RTRIM(CantiGrupo)),LTRIM(RTRIM(ValUnita)),LTRIM(RTRIM(Valtotal)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de consulta":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)),LTRIM(RTRIM(FecConsul)),LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(CodConsul)),LTRIM(RTRIM(CausExter)),LTRIM(RTRIM(DxPrincipal)),LTRIM(RTRIM(DxRelacion1)),LTRIM(RTRIM(DxRelacion2)),LTRIM(RTRIM(DxRelacion3)),LTRIM(RTRIM(TipoDxPrin)),LTRIM(RTRIM(ValorConsul)),LTRIM(RTRIM(ValorCuota)),LTRIM(RTRIM(ValorNeto)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de procedimientos":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)),LTRIM(RTRIM(FecProce)),LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(CodProce)),LTRIM(RTRIM(AmbitoReal)),LTRIM(RTRIM(FinalProce)),LTRIM(RTRIM(PersonAten)),LTRIM(RTRIM(DxPrincipal)),LTRIM(RTRIM(DxRelacion)),LTRIM(RTRIM(Complicacion)),LTRIM(RTRIM(RealiActo)),LTRIM(RTRIM(ValorProce)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de hospitalizacion":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)),LTRIM(RTRIM(ViaDIngreso)),LTRIM(RTRIM(FecIngresa)),LTRIM(RTRIM(HorIngresa)),LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(CausExter)),LTRIM(RTRIM(DxPrincIngre)),LTRIM(RTRIM(DxPrincEgre)),LTRIM(RTRIM(DxRelacion1)),LTRIM(RTRIM(DxRelacion2)),LTRIM(RTRIM(DxRelacion3)),LTRIM(RTRIM(DxComplica)),LTRIM(RTRIM(EstadoSal)),LTRIM(RTRIM(DxMuerte)),LTRIM(RTRIM(FecSalida)),LTRIM(RTRIM(HorSalida)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de observacion urgencias":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)),LTRIM(RTRIM(FecIngresa)),LTRIM(RTRIM(HorIngresa)),LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(CausExter)),LTRIM(RTRIM(DxPrincipal)),LTRIM(RTRIM(DxRelacion1)),LTRIM(RTRIM(DxRelacion2)),LTRIM(RTRIM(DxRelacion3)),LTRIM(RTRIM(Destino)),LTRIM(RTRIM(EstadoSal)),LTRIM(RTRIM(DxMuerte)),FecSalida,LTRIM(RTRIM(HorSalida)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de recien nacido":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)), LTRIM(RTRIM(FecNaci)),LTRIM(RTRIM(HorIngresa)),LTRIM(RTRIM(EdadGesta)),LTRIM(RTRIM(ControlPrena)),LTRIM(RTRIM(SexoRecien)),LTRIM(RTRIM(PesoRecien)),LTRIM(RTRIM(DxRecien)),LTRIM(RTRIM(DxMuerte)),LTRIM(RTRIM(FecMuerte)),LTRIM(RTRIM(HorMuerte)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de medicamentos":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)), LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(CodMedica)),LTRIM(RTRIM(TipoMedica)),LTRIM(RTRIM(NomGenerico)),LTRIM(RTRIM(FormaFarma)),LTRIM(RTRIM(ConcenMedi)),LTRIM(RTRIM(UniMedida)),LTRIM(RTRIM(NumUnidad)),LTRIM(RTRIM(ValorUnita)),LTRIM(RTRIM(ValorTotal)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                        case "Datos archivo de otros servicios":
                                            Utils.SqlDatos = "SELECT LTRIM(RTRIM(NumFactur)), LTRIM(RTRIM(CodIPS)), LTRIM(RTRIM(TipoDocum)), LTRIM(RTRIM(NumDocum)),LTRIM(RTRIM(AutoriNum)),LTRIM(RTRIM(TipoServicio)),LTRIM(RTRIM(CodiServi)),LTRIM(RTRIM(NomServi)),LTRIM(RTRIM(Cantidad)),LTRIM(RTRIM(ValorUnita)),LTRIM(RTRIM(ValorTotal)) FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;

                                        default:
                                            Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[" + NT + "] WHERE [NumRemi]  = '" + NR + "'";
                                            break;
                                    }



                                    DataTable dt = Conexion.SQLDataTable(Utils.SqlDatos);

                                    using (StreamWriter file = new StreamWriter(Ruta + NomArchi, true))
                                    {
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            List<string> items = new List<string>();
                                            foreach (DataColumn col in dt.Columns)
                                            {

                                                items.Add(Convert.ToString(row[col.ColumnName]));

                                            }
                                            string linea = string.Join(",", items.ToArray());
                                            file.WriteLine(linea);
                                        }
                                    }

                                }

                            }// fiNAL TolReg > 0

                        }// Final while 

                        //'Muestrelos resultados

                        Utils.Informa = "Se ha exportado los siguientes archivos" + "\r";
                        M = 1;
                        while (M != NuT)
                        {
                            Utils.Informa += NomAr[M].ToString() + ". Con " + CanReg[M].ToString() + " registros." + "\r"; ;
                            M += 1;
                        }

                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    } // TablaAux1.HasRows == false


                } // Dialogo yes


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa += "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de dar click al boton de exportar " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string RemisAbierta(string CI, Boolean RA)
        {
            try
            {
                //'Permite averiguar que remisión tiene abierta una entidad

                string Estandatos, ConverCodigo;

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] WHERE CodInterAdmi= '" + CI + "' AND CerraRemi= '" + RA + "'";

                SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux1.HasRows == false)
                {
                    return "0";
                }
                else
                {
                    TablaAux1.Read();
                    return TablaAux1["ConseArchivo"].ToString();
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion RemisAbierta  " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "-1";
            }
        }

        private void CargaUsuario()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Utils.codUsuario))
                {
                    lblCodigoUsaF.Text = "000";
                    lblNombreUsa.Text = "SOFTWARE PIRATA";
                    lblNivelPermitido.Text = "0";
                    this.txtNomIPS.Text = "EMPRESA FANTASMA";
                    this.txtNitCCIPS.Text = "0";
                    this.txtCodigIPS.Text = "0";
                }
                else
                {
                    lblCodigoUsaF.Text = Utils.codUsuario;
                    lblNombreUsa.Text = Utils.nomUsuario;
                    lblNivelPermitido.Text = Utils.nivelPermiso;
                    this.txtNomIPS.Text = Utils.nomEmpresa;
                    this.txtNitCCIPS.Text = Utils.nitEmpresa;
                    this.txtCodigIPS.Text = Utils.codMinSalud;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CargaUsuario" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosAdminPlanes()
        {
            try
            {
                if (Bandera == 1)
                {
                    SqlDataReader AdminPLanes = Conexion.SQLDataReader("SELECT [Datos administradoras de planes].CodInterno, [Datos administradoras de planes].NomAdmin, [Datos administradoras de planes].CodAdmin, [Datos administradoras de planes].NitCC, [Datos administradoras de planes].ManualTari " +
                                            " FROM [DARIPSXPSQL].[dbo].[Datos administradoras de planes] WHERE [Datos administradoras de planes].CodInterno = '" + cboNomAdmin.SelectedValue + "' " +
                                            " ORDER BY [Datos administradoras de planes].NomAdmin;");

                    if (AdminPLanes.HasRows)
                    {
                        AdminPLanes.Read();

                        txtNitCCAdmin.Text = AdminPLanes["NitCC"].ToString();
                        txtCodigAdmin.Text = AdminPLanes["CodAdmin"].ToString();
                        ManuelTari = AdminPLanes["ManualTari"].ToString();
                        AdminPLanes.Close();
                    }

                    DataSet DatosMaestro = Conexion.SQLDataSet("SELECT [Datos archivo maestro].ConseArchivo, [Datos archivo maestro].CodInterAdmi, [Datos archivo maestro].CodIPS, [Datos archivo maestro].FecRemite, [Datos archivo maestro].NomRespon, [Datos archivo maestro].TelResponsa, [Datos archivo maestro].Periodo1, [Datos archivo maestro].Periodo2, [Datos archivo maestro].NumFacturas, [Datos archivo maestro].CerraRemi, [Datos archivo maestro].AnulRemi  " +
                                                                        "FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] " +
                                                                        "WHERE [Datos archivo maestro].CodInterAdmi = '" + cboNomAdmin.SelectedValue + "' " +
                                                                        "ORDER BY [Datos archivo maestro].ConseArchivo DESC; ");


                    if (DatosMaestro != null && DatosMaestro.Tables.Count > 0)
                    {
                        DataGridRemi.DataSource = null;

                        DataGridRemi.DataSource = DatosMaestro.Tables[0];
                    }


                    if (DataGridRemi.Rows.Count > 0)
                    {

                        txtTotalRemisiones.Clear();
                        txtTotalRemisiones.Text = DataGridRemi.Rows.Count.ToString();

                    }



                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al cargar los datos del administrador de planes" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ElimRemiAgrupados(string RM)
        {
            try
            {
                //'Permite eliminar todos los datos o grupos de una remisión de la tablas grupos

                //Abrimos la tabla nombre de archivos

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] WHERE NumRemi = '" + RM + "'";

                SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux1.HasRows == false)
                {
                    //El numero de factura no se encuentra
                    return 0;
                }
                else
                {
                    Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] WHERE NumRemi = '" + RM + "' ";

                    Boolean EliminaDatosAgri = Conexion.SQLDelete(Utils.SqlDatos);

                    if (EliminaDatosAgri)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }

                TablaAux1.Close();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion ElimRemiAgrupados  " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        #endregion 

        #region Botones
        private void btnNuevaRemi_Click(object sender, EventArgs e)
        {
            try
            {
                string NAP, CiN, FunAbier;
                int SinoC;
                Boolean SN;

                Utils.Titulo01 = "Control para crear remisiones";

                if (string.IsNullOrWhiteSpace(txtCodigIPS.Text) || string.IsNullOrWhiteSpace(txtCodigIPS.Text))
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "código de SGSSS, el cual permita " + "\r";
                    Utils.Informa += "crear la remisión de envío." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cboNomAdmin.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "seleccionado el nombre de la" + "\r";
                    Utils.Informa += "cadministradora de planes." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                NAP = cboNomAdmin.Text;

                //Revisamos si la entidad ya tiene una remisión abierta

                CiN = cboNomAdmin.SelectedValue.ToString();
                SN = false;

                FunAbier = RemisAbierta(CiN, SN);

                switch (FunAbier)
                {
                    case "-1":
                        return;
                        break;
                    case "0":

                        break;
                    default:
                        Utils.Informa = "Lo siento pero la entidad de nombre" + NAP + "\r";
                        Utils.Informa += "Tiene la remisión " + FunAbier + " Abierta " + "\r";
                        Utils.Informa += "Por tanto no se le puede crear otra." + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        break;
                }

                Utils.Informa = "¿Usted desea agregar una NUEVA" + "\r";
                Utils.Informa += "remisión de envío a la entidad " + NAP + "?" + "\r";

                var RESP = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (RESP == DialogResult.Yes)
                {
                    Utils.NomAdmin = cboNomAdmin.SelectedValue.ToString();
                    Utils.CodiIPS = txtCodigIPS.Text;
                    Utils.CodigAdmin = txtCodigAdmin.Text;
                    Gestion_Rips.Forms.ArchivoMaestro.FrmCrearModificarMaestro frmCrearModificarMaestro = new Gestion_Rips.Forms.ArchivoMaestro.FrmCrearModificarMaestro();
                    frmCrearModificarMaestro.ShowDialog();
                    CargarDatosAdminPlanes();
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el boton nuevo  " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string NAP, NR, USC, CIp, Estandatos, FEv;
                string EstasdoRemi;
                int MT, SinoG, FunAct, FunGru, FunAC;
                Double FG, FT, DFA = 0;

                if (string.IsNullOrWhiteSpace(txtCodigIPS.Text) || string.IsNullOrWhiteSpace(txtCodigIPS.Text))
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "código de SGSSS, el cual permita " + "\r";
                    Utils.Informa += "crear la remisión de envío." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cboNomAdmin.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "seleccionado el nombre de la" + "\r";
                    Utils.Informa += "cadministradora de planes." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Revise si se selecciono la remision a actualizar

                if (DataGridRemi.SelectedRows.Count > 0)
                {

                    NR = DataGridRemi.SelectedCells[0].Value.ToString();

                    //Validamos si la remision seleccionada esta cerrada
                    EstasdoRemi = DataGridRemi.SelectedCells[9].Value.ToString();

                    if (Convert.ToBoolean(EstasdoRemi) == true)
                    {
                        Utils.Informa = "Lo siento pero la remisión " + NR + "\r";
                        Utils.Informa += "se encuentra cerrada, por tanto no se" + "\r";
                        Utils.Informa += "le puede ejecutar la actualización." + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    Utils.Informa = "Lo siento  pero  usted no ha " + "\r";
                    Utils.Informa += "el número de la remisión a actualizar" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                NAP = this.cboNomAdmin.Text;

                Utils.Informa = "¿Usted desea ACTUALIZAR los datos" + "\r";
                Utils.Informa += "de la remisión " + NR + " registrada a la" + "\r";
                Utils.Informa += "administradora de planes" + "\r";
                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    USC = lblCodigoUsaF.Text;

                    //  'Revisamos con que tipo de codificación (manual funciona la administradora)
                    MT = Convert.ToInt32(ManuelTari);

                    FunGru = ElimRemiAgrupados(NR);

                    if (FunGru == -1)
                    {
                        return;
                    }

                    //Proceda a generar el archivo de agrupados
                    FunGru = AgruparConsultas(NR, MT);

                    if (FunGru != -1)
                    {
                        FunGru = AgruparProce(NR, MT);
                        if (FunGru != -1)
                        {
                            FunGru = AgruparMedicamentos(NR, MT);
                            if (FunGru != 1)
                            {
                                FunGru = AgruparOtrosServi(NR, MT);
                            }
                        }
                    }


                    //   'Revise el valor total del archivo de transaciones es igual al de agrupados

                    FT = TotalTransac(NR);


                    switch (FT)
                    {
                        case -2:
                            Utils.Informa = "Lo siento pero en el archivo de transacciones no" + "\r";
                            Utils.Informa += "existen facturas registradas a la remisión No. " + NR + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                        case -1:
                            //Error en la funcion
                            return;
                            break;
                        default:
                            break;
                    }

                    FG = TotalGrupo(NR);

                    switch (FG)
                    {
                        case -2: //'NO tiene registros en el archivo grupo
                            Utils.Informa = "Lo siento pero en el archivo de agrupados no" + "\r";
                            Utils.Informa += "existen grupos registrados a la remisión No. " + NR + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                        case -1:
                            return;
                            break;
                        default:
                            break;
                    }

                    //Revisamos la diferencia

                    if (FT == FG)
                    {
                        //PUEDE CONTINUAR A GENERAR EL ARCHIVO DEL CONTROL

                        CIp = this.txtCodigIPS.Text;
                        //  FEv = this.de

                        FEv = DataGridRemi.SelectedCells[3].Value.ToString();

                        FunAC = ActualArchiControl(NR, CIp, FEv);

                        switch (FunAC)
                        {
                            case -1:
                                return;
                                break;
                            case 0: //La tabla de nombres de archivos no tiene datos
                                Utils.Informa = "Lo siento pero la tabla de nombres" + "\r";
                                Utils.Informa += "de archivos no contiene información, por tanto no se puede actualizar " + "\r";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                                break;
                            case 1:
                                break;
                            default:

                                break;
                        }

                        //Proceda a marcar como sí (actualizada) el registro de la remisión
                        //Abrimos la tabla nombre de archivos

                        Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] WHERE ConseArchivo = '" + NR + "' ";

                        SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                        if (TablaAux1.HasRows == false)
                        {
                            Utils.Informa = "Error fatal, pero el número de remisión no" + "\r";
                            Utils.Informa += "se pudo encontrar en el archivo de maestro" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo maestro] SET ActualRemi = '" + 1 + "' WHERE ConseArchivo = '" + NR + "'";

                            Boolean eSTAaCT = Conexion.SQLUpdate(Utils.SqlDatos);

                        }

                        TablaAux1.Close();
                    }
                    else
                    {
                        DFA = (FG - FT);
                        DFA = Math.Abs(DFA);
                        Utils.Informa = "Lo siento pero entre el archivo de" + "\r";
                        Utils.Informa += "transacciones y el archivo agrupados" + "\r";
                        Utils.Informa += "existe una diferencia de " + DFA + "\r";
                        Utils.Informa += "por tanto no se puede actualizar." + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón actualizar " + "\r";
                Utils.Informa += "Módulo gestionar archivos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {

            try
            {
                Utils.Titulo01 = "Control para anular remisiones";
                string NR, NAP;
                int FunCer;

                if (string.IsNullOrWhiteSpace(txtCodigIPS.Text) || string.IsNullOrWhiteSpace(txtCodigIPS.Text))
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "código de SGSSS, el cual permita " + "\r";
                    Utils.Informa += "crear la remisión de envío." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cboNomAdmin.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "seleccionado el nombre de la" + "\r";
                    Utils.Informa += "cadministradora de planes." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Revise si seleccionó la remisión de envio

                if (DataGridRemi.SelectedRows.Count > 0)
                {

                    NR = DataGridRemi.SelectedCells[0].Value.ToString();

                }
                else
                {
                    Utils.Informa = "Lo siento  pero  usted no ha seleccionado " + "\r";
                    Utils.Informa += "la remisión de envío a exportar." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Validamos si la remisión seleccionada está Abierta.

                string EstasdoRemi = DataGridRemi.SelectedCells[9].Value.ToString();

                if (Convert.ToBoolean(EstasdoRemi) == false)
                {
                    Utils.Informa = "Lo siento pero la remisión " + NR + "\r";
                    Utils.Informa += "se encuentra abierta, por tanto no" + "\r";
                    Utils.Informa += "le puede exportar los archivos RIPS." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                NAP = cboNomAdmin.Text;

                Utils.Informa = "Lo siento pero la remisión " + NR + "\r";
                Utils.Informa += "se encuentra abierta, por tanto no" + "\r";
                Utils.Informa += "le puede exportar los archivos RIPS." + "\r";
                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    Utils.RemiAnular = NR;
                    FrmAnularRemi FrmAnularRemi = new FrmAnularRemi();
                    FrmAnularRemi.Show();
                    string USC = lblCodigoUsaF.Text;

                    string Rz = Utils.RemiAnular;
                    string Date = DateTime.Now.ToString("yyyy-MM-dd");

                    FunCer = AnularRemision(NR, Rz, Date, USC);

                    switch (FunCer)
                    {
                        case -1: //ERROR EN LA FUNCION
                            return;
                            break;
                        case 0: //No se encontro
                            Utils.Informa = "Lo siento pero el número de la remisión no se pudo encontrar en este sistema" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                        case 1: //Todo bien
                            //Registre la accion realizada

                            int FunFec = RegisAccion(NR, "1", Rz, Date, USC);

                            Utils.Informa = "La remisión No. " + NR + " ha sido anulada en este sistema" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            CargarDatosAdminPlanes();
                            break;

                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el boton anular" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                string NAP, NR, USC;
                int FunAct, SinoG, FunCer;
                double FT = 0, FG, DFA;

                if (string.IsNullOrWhiteSpace(txtCodigIPS.Text) || string.IsNullOrWhiteSpace(txtCodigIPS.Text))
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "código de SGSSS, el cual permita " + "\r";
                    Utils.Informa += "crear la remisión de envío." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cboNomAdmin.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero la IPS no tiene un " + "\r";
                    Utils.Informa += "seleccionado el nombre de la" + "\r";
                    Utils.Informa += "cadministradora de planes." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Revise si seleccionó la remisión de envio

                if (DataGridRemi.SelectedRows.Count > 0)
                {

                    NR = DataGridRemi.SelectedCells[0].Value.ToString();

                }
                else
                {
                    Utils.Informa = "Lo siento  pero  usted no ha seleccionado " + "\r";
                    Utils.Informa += "la remisión de envío a exportar." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Validamo si la remisión seleccionada está cerrada

                string EstasdoRemi = DataGridRemi.SelectedCells[9].Value.ToString();

                if (Convert.ToBoolean(EstasdoRemi) == true)
                {
                    Utils.Informa = "Lo siento pero la remisión " + NR + "\r";
                    Utils.Informa += "se encuentra cerrada, por tanto no se" + "\r";
                    Utils.Informa += "le puede exportar los archivos RIPS." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                NAP = cboNomAdmin.Text;

                Utils.Informa = "¿Usted desea CERRAR la remisión " + NR + "\r";
                Utils.Informa += "registrada a la administradora de planes@" + "\r";
                Utils.Informa += NAP + "\r";
                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    USC = lblCodigoUsaF.Text;

                    // 'Se debe revisar si se le corrió el proceso de actualización

                    FunAct = ActualMaestro(NR);

                    switch (FunAct)
                    {
                        case -1:

                            return;
                            break;

                        case 0: //No se econtro el numero de remision

                            Utils.Informa = "Error fatal, el número de remisión " + NR + "\r";
                            Utils.Informa += "no se pudo encontrar en este sistema" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;

                        case 1: //Ya se corrio el proceso de actualizacion


                            //       'Proceda revisar si el valor del archivo de transacciones es igual al de grupos
                            FT = TotalTransac(NR);

                            switch (FT)
                            {
                                case -2: //No tiene registos en el archivo grupo
                                    Utils.Informa = "Lo siento pero en el archivo de transacciones no" + "\r";
                                    Utils.Informa += "existen facturas registradas a la remisión No. " + NR + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                    break;
                                case -1: //ERROR EN LA FUNCION
                                    return;
                                    break;
                                default:
                                    //Devuelve algun valor
                                    break;
                            }

                            FG = TotalGrupo(NR);

                            switch (FG)
                            {
                                case -2: //No tiene registros en el archivo grupo
                                    Utils.Informa = "Lo siento pero en el archivo de agrupados no" + "\r";
                                    Utils.Informa += "existen grupos registrados a la remisión No. " + NR + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                    break;
                                case -1: //error en la funcion
                                    return;
                                    break;
                                default:
                                    //Devuelve algun valor
                                    break;
                            }


                            //Revisamos la diferencia

                            if (FT == FG)
                            {
                                //TODO BIEN
                            }
                            else
                            {
                                DFA = (FG - FT);
                                DFA = Math.Abs(DFA);
                                Utils.Informa = "Lo siento pero entre el archivo de" + "\r";
                                Utils.Informa += "transacciones y el archivo agrupados" + "\r";
                                Utils.Informa += "existe una diferencia de " + DFA + "\r";
                                Utils.Informa += "a pesar de estar este actualizado." + "\r";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            break;
                        case 2: //No se ha actualizado
                            Utils.Informa = "Lo siento pero a la remisión No. " + NR + "\r";
                            Utils.Informa += "no se le ha ejecutado el proceso de actualización" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                        default:
                            break;

                    }

                    //Proceda a Cerrar la Remision

                    FunCer = CerrarRemision(NR, USC);

                    CargarDatosAdminPlanes();


                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón Cerrar " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        } //Fin boton cerrar

        private void BtnBorrarRemi_Click(object sender, EventArgs e)
        {

            if (DataGridRemi.SelectedRows.Count > 0)
            {

                string NR = DataGridRemi.SelectedCells[0].Value.ToString();


                if (string.IsNullOrWhiteSpace(NR) == false)
                {
                    Utils.NumRemi = NR;
                    Gestion_Rips.Forms.ArchivoMaestro.FrmBorrarRemision FrmBorrarRemision = new Gestion_Rips.Forms.ArchivoMaestro.FrmBorrarRemision();
                    FrmBorrarRemision.ShowDialog();
                    CargarDatosAdminPlanes();
                }
            }
            else
            {
                Utils.Informa = "Lo siento  pero  usted no ha " + "\r";
                Utils.Informa += "seleccionado la remision a borrar" + "\r";
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        #endregion

        private void FrmArchivoMaestro_Load(object sender, EventArgs e)
        {
            try
            {
                CargaUsuario();

                CargarComboBox();

                Bandera = 1;

                CargarDatosAdminPlanes();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el formulario Archivo Maestro" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }




        private void btnUnificar_Click(object sender, EventArgs e)
        {
            try
            {



            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de hacer click es Unificar" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}

