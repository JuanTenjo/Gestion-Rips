using Gestion_Rips.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Gestion_Rips.Forms.RipsPorRegimen
{
    public partial class FrmRipsRegimen : Form
    {
        public FrmRipsRegimen()
        {
            InitializeComponent();
        }

        #region DatagridView
        private void DataGridFacturas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CalcularTotalFactura();
        }
        #endregion

        #region Texbox

        private void txtCardinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (int)Keys.Enter)
                {
                    if (string.IsNullOrWhiteSpace(txtCardinal.Text) == false)
                    {
                        Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                        " FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] " +
                        " WHERE CarAdmin = '" + txtCardinal.Text + "' AND ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 'True') and (([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 'True')) " +
                        " AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null ";

                        SqlDataReader sqlDataReader = Conexion.SQLDataReader(Utils.SqlDatos);

                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();
                            this.txtCardinal.Text = sqlDataReader["CarAdmin"].ToString();
                            this.txtTipoDocu.Text = sqlDataReader["TipoDocu"].ToString();
                            this.txtDocumento.Text = sqlDataReader["NumDocu"].ToString();
                            cboNameEntidades.SelectedValue = sqlDataReader["CarAdmin"].ToString();
                            this.txtRips.Text = sqlDataReader["CodiMinSalud"].ToString();


                        }
                        else
                        {
                            Utils.Titulo01 = "Control de ejecución";
                            Utils.Informa = "no encontro ninguna entidad por el numero de cardinal digitado" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlDataReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después intentar buscar por cardinal" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ComboBox

        private void cboNameEntidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cboNameEntidades.Items.Count > 0)
                {
                    SqlDataReader sqlDataReader = Conexion.SQLDataReader("SELECT [CarAdmin],[NomAdmin] ,[TipoDocu],[NumDocu],[CodiMinSalud] FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE CarAdmin = '" + cboNameEntidades.SelectedValue + "' ");
                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        this.txtCardinal.Text = sqlDataReader["CarAdmin"].ToString();
                        this.txtTipoDocu.Text = sqlDataReader["TipoDocu"].ToString();
                        this.txtDocumento.Text = sqlDataReader["NumDocu"].ToString();
                        this.txtRips.Text = sqlDataReader["CodiMinSalud"].ToString();


                    }

                    sqlDataReader.Close();

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al cargar los datos de la entidad seleccionada" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarComboBox()
        {
            try
            {
                //argamos primeramente los combos, abriendo las instancias que se pueden cerrar inmediatament


                this.cboNameEntidades.DataSource = null;
                this.cboNameEntidades.Items.Clear();


                Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre" +
                                 " FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 'True') and (([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 'True'))" +
                                 " and ([NomAdmin] + ' ' + [ProgrAmin]) is not null ORDER BY([NomAdmin] +' ' + [ProgrAmin])";

                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    this.cboNameEntidades.DataSource = dataSet.Tables[0];
                    this.cboNameEntidades.ValueMember = "CarAdmin";
                    this.cboNameEntidades.DisplayMember = "NP";
                }

                this.cboRegNom.DataSource = null;
                this.cboRegNom.Items.Clear();

                Utils.SqlDatos = " SELECT [Datos tipos de usuarios].CodTipoUsuar, [Datos tipos de usuarios].NomTipo " +
                                 " FROM [Datos tipos de usuarios] " +
                                 " ORDER BY [Datos tipos de usuarios].NomTipo;";

                DataSet dataSet2 = Conexion.SQLDataSet(Utils.SqlDatos);

                if (dataSet2 != null && dataSet2.Tables.Count > 0)
                {
                    this.cboRegNom.DataSource = dataSet2.Tables[0];
                    this.cboRegNom.ValueMember = "CodTipoUsuar";
                    this.cboRegNom.DisplayMember = "NomTipo";
                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CargarCombobox" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Funciones
        private int CopiaRipsTrans(string NR, string CI, double TolF, int RutCopy)
        {
            try
            {
                string NF, SqlTrans = null;
                int VR = 0;

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] WHERE NumRemi = '" + CI + "'";


                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay transacciones para copiar para esta entidad
                        return -2;
                    }
                    else
                    {
                        while (TabLocal.Read())
                        {
                            NF = TabLocal["NumFactur"].ToString();

                            SqlTrans = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos archivo de transacciones] ";
                            SqlTrans = SqlTrans + "WHERE (([Datos archivo de transacciones].NumRemi)= '" + NR + "') And ";
                            SqlTrans = SqlTrans + "(([Datos archivo de transacciones].NumFactur)= '" + NF + "' ); ";

                            SqlDataReader TabTrans;

                            using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command2 = new SqlCommand(SqlTrans, connection2);
                                command2.Connection.Open();
                                TabTrans = command2.ExecuteReader();

                                if (TabTrans.HasRows == false)
                                {

                                    //Adicionelo
                                    Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de transacciones] " +
                                           "(" +
                                           "NumRemi," +
                                           "CodIps," +
                                           "RazonSocial," +
                                           "TipIdenti," +
                                           "NumIdenti," +
                                           "NumFactur," +
                                           "FecFactur," +
                                           "FecInicio," +
                                           "FecFinal," +
                                           "CodAdmin," +
                                           "NomAdmin," +
                                           "NumContra," +
                                           "PlanBene," +
                                           "NumPoli," +
                                           "Copago," +
                                           "ValorComi," +
                                           "ValorDes," +
                                           "ValorNeto" +
                                           ")" +
                                           "VALUES" +
                                           "(" +
                                           "'" + NR + "'," +
                                           "'" + TabLocal["CodIps"].ToString() + "'," +
                                           "'" + TabLocal["RazonSocial"].ToString() + "'," +
                                           "'" + TabLocal["TipIdenti"].ToString() + "'," +
                                           "'" + TabLocal["NumIdenti"].ToString() + "'," +
                                           "'" + NF + "'," +
                                           "'" + Convert.ToDateTime(TabLocal["FecFactur"]).ToString("yyyy-MM-dd") + "'," +
                                           "'" + Convert.ToDateTime(TabLocal["FecInicio"]).ToString("yyyy-MM-dd") + "'," +
                                           "'" + Convert.ToDateTime(TabLocal["FecFinal"]).ToString("yyyy-MM-dd") + "'," +
                                           "'" + TabLocal["CodAdmin"].ToString() + "'," +
                                           "'" + TabLocal["NomAdmin"].ToString() + "'," +
                                           "'" + TabLocal["NumContra"].ToString() + "'," +
                                           "'" + TabLocal["PlanBene"].ToString() + "'," +
                                           "'" + TabLocal["NumPoli"].ToString() + "'," +
                                           "'" + TabLocal["Copago"].ToString() + "'," +
                                           "'" + TabLocal["ValorComi"].ToString() + "'," +
                                           "'" + TabLocal["ValorDes"].ToString() + "'," +
                                           "'" + TabLocal["ValorNeto"].ToString() + "'" +
                                           ")";

                                    Boolean RegistrarArcTransacciones = Conexion.SqlInsert(Utils.SqlDatos);
                                }
                                else
                                {
                                    //Modifique algunos datos
                                    Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos archivo de transacciones] SET " +
                                   "CodIps = '" + TabLocal["CodIps"].ToString() + "'," +
                                   "RazonSocial = '" + TabLocal["RazonSocial"].ToString() + "'," +
                                   "TipIdenti = '" + TabLocal["TipIdenti"].ToString() + "'," +
                                   "NumIdenti = '" + TabLocal["NumIdenti"].ToString() + "'," +
                                   "FecFactur = '" + Convert.ToDateTime(TabLocal["FecFactur"]).ToString("yyyy-MM-dd") + "'," +
                                   "FecInicio = '" + Convert.ToDateTime(TabLocal["FecInicio"]).ToString("yyyy-MM-dd") + "'," +
                                   "FecFinal = '" + Convert.ToDateTime(TabLocal["FecFinal"]).ToString("yyyy-MM-dd") + "'," +
                                   "CodAdmin = '" + TabLocal["CodAdmin"].ToString() + "'," +
                                   "NomAdmin = '" + TabLocal["NomAdmin"].ToString() + "'," +
                                   "NumContra = '" + TabLocal["NumContra"].ToString() + "'," +
                                   "PlanBene = '" + TabLocal["PlanBene"].ToString() + "'," +
                                   "NumPoli = '" + TabLocal["NumPoli"].ToString() + "'," +
                                   "Copago = '" + TabLocal["Copago"].ToString() + "'," +
                                   "ValorComi = '" + TabLocal["ValorComi"].ToString() + "'," +
                                   "ValorDes = '" + TabLocal["ValorDes"].ToString() + "'," +
                                   "ValorNeto = '" + TabLocal["ValorNeto"].ToString() + "' " +
                                   "WHERE [Datos archivo de transacciones].[NumRemi] = '" + NR + "' AND [Datos archivo de transacciones].[NumFactur] =  '" + NF + "' ";

                                    Boolean ActualizarArcUsuarios = Conexion.SQLUpdate(Utils.SqlDatos);
                                }

                                TabTrans.Close();
                                TabTrans = null;

                            }
                            VR += 1;
                        } //Fin while
                    }
                }//Fin using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsTrans" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int CopiaRipsUsa(string NR, string CI, double TolU, Int16 RutCopy)
        {
            try
            {

                int VR = 0;
                string TD = null, ND = null, SqlUsua = null;
                Int32 RegExp = 0;
                //Permite copiar los usuarios para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] where NumRemi = '"+ CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay usuarios para copiar para esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;
                        while (TabLocal.Read())
                        {
                            TD = TabLocal["TipoDocum"].ToString();
                            ND = TabLocal["NumDocum"].ToString();

                            SqlUsua = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos archivo usuarios] " +
                            "WHERE (([Datos archivo usuarios].NumRemi)= '" + NR + "') And " +
                            "(([Datos archivo usuarios].TipoDocum)= '" + TD + "' ) And " +
                            "(([Datos archivo usuarios].NumDocum)= '" + ND + "' );";

                            SqlDataReader TabUsuarios;

                            using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command2 = new SqlCommand(SqlUsua, connection2);
                                command2.Connection.Open();
                                TabUsuarios = command2.ExecuteReader();

                                if (TabUsuarios.HasRows == false)
                                {
                                    //Adicionelo
                                    //Active la suguiente rutina de error

                                    Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo usuarios]" +
                                    "(" +
                                    "NumRemi," +
                                    "TipoDocum," +
                                    "NumDocum," +
                                    "CodAdmin," +
                                    "TipUsuario," +
                                    "Apellido1," +
                                    "Apellido2," +
                                    "Nombre1," +
                                    "Nombre2," +
                                    "Edad," +
                                    "EdadMedi," +
                                    "Sexo," +
                                    "CodDpto," +
                                    "CodMuni," +
                                    "ZonaResi" +
                                    ")" +
                                    "VALUES" +
                                    "(" +
                                    "'" + NR + "'," +
                                    "'" + TD + "'," +
                                    "'" + ND + "'," +
                                    "'" + TabLocal["CodAdmin"].ToString() + "'," +
                                    "'" + TabLocal["TipUsuario"].ToString() + "'," +
                                    "'" + TabLocal["Apellido1"].ToString() + "'," +
                                    "'" + TabLocal["Apellido2"].ToString() + "'," +
                                    "'" + TabLocal["Nombre1"].ToString() + "'," +
                                    "'" + TabLocal["Nombre2"].ToString() + "'," +
                                    "'" + TabLocal["Edad"].ToString() + "'," +
                                    "'" + TabLocal["EdadMedi"].ToString() + "'," +
                                    "'" + TabLocal["Sexo"].ToString() + "'," +
                                    "'" + TabLocal["CodDpto"].ToString() + "'," +
                                    "'" + TabLocal["CodMuni"].ToString() + "'," +
                                    "'" + TabLocal["ZonaResi"].ToString() + "'" +
                                    ")";

                                    Boolean RegistrarArcUsuarios = Conexion.SqlInsert(Utils.SqlDatos);

                                    if (RegistrarArcUsuarios == false)
                                    {
                                        Utils.Informa = "Lo siento pero no se pudo insertar el usuario ";
                                        Utils.Informa = Utils.Informa + "con el documento " + TD + ":" + ND + " ";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return -1;
                                    }

                                }
                                else
                                {
                                    //Modifique algunos datos
                                    Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos archivo usuarios] SET " +
                                    "CodAdmin = '" + TabLocal["CodAdmin"].ToString() + "'," +
                                    "TipUsuario = '" + TabLocal["TipUsuario"].ToString() + "'," +
                                    "Apellido1 = '" + TabLocal["Apellido1"].ToString() + "'," +
                                    "Apellido2 = '" + TabLocal["Apellido2"].ToString() + "'," +
                                    "Nombre1 = '" + TabLocal["Nombre1"].ToString() + "'," +
                                    "Nombre2 = '" + TabLocal["Nombre2"].ToString() + "'," +
                                    "Edad = '" + TabLocal["Edad"].ToString() + "'," +
                                    "EdadMedi = '" + TabLocal["EdadMedi"].ToString() + "'," +
                                    "Sexo = '" + TabLocal["Sexo"].ToString() + "'," +
                                    "CodDpto = '" + TabLocal["CodDpto"].ToString() + "'," +
                                    "CodMuni = '" + TabLocal["CodMuni"].ToString() + "'," +
                                    "ZonaResi = '" + TabLocal["ZonaResi"].ToString() + "' " +
                                    "WHERE NumRemi = '" + NR + "' AND TipoDocum = '" + TD + "' AND NumDocum = '" + ND + "' ";

                                    Boolean ActualizarArcUsuarios = Conexion.SQLUpdate(Utils.SqlDatos);

                                    if (ActualizarArcUsuarios == false)
                                    {
                                        Utils.Informa = "Lo siento pero no se pudo actualizar el usuario ";
                                        Utils.Informa = Utils.Informa + "con el documento " + TD + ":" + ND + " ";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return -1;
                                    }


                                }//Final Tab Usuarios

                                TabUsuarios.Close();

                                //Edite el registro como exportado

                                Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] SET [Datos temporal usuarios RIPS].[Exportado] = 1 WHERE ([Datos temporal usuarios RIPS].[CodDigita] = N'" + lblCodigoUser.Text + "') AND " +
                                "([Datos temporal usuarios RIPS].[NumRemi] = N'" + NR + "')";

                                Boolean ActComoExportado = Conexion.SQLUpdate(Utils.SqlDatos);

                            }
                            RegExp += 1;
                        }//Fin While
                    }
                }// Fin Using


                return RegExp;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: CopiaRipsUsa del Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private string ConseRemisiones(Boolean a, string US)
        {
            try
            {

                string SqlContadores = null, Convertido = null;
                string Date;
                double Fac = 0;

                SqlContadores = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos contadores sedas];";

                SqlDataReader TabContadores = Conexion.SQLDataReader(SqlContadores);

                if (TabContadores.HasRows == false)
                {
                    return "0";
                }
                else
                {
                    TabContadores.Read();

                    if (Convert.ToInt32(TabContadores["UlConRemi"].ToString()) == 0)
                    {
                        //no existe remisiones perdidas
                        Fac = Convert.ToInt32(TabContadores["ConsRemi"].ToString());
                        Fac += 1;

                        //Procesa a actualizar el campo de concecutivos

                        Date = DateTime.Now.ToString("yyyy-MM-dd");

                        if (a)
                        {
                            Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos contadores sedas] SET [ConsRemi] = '" + Fac + "', [UsarRemi] = '" + US + "', FecRemi = '" + Date + "'";

                            Boolean EstaActConce = Conexion.SQLUpdate(Utils.SqlDatos);

                            if (EstaActConce == false)
                            {
                                Utils.Informa = "Error de administración de datos. ";
                                Utils.Informa += "al actualizar el concecutivo" + "\r";

                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                    }
                    else
                    {
                        Fac = Convert.ToDouble(TabContadores["UlConRemi"].ToString());

                        Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos contadores sedas] SET [UlConRemi] = '" + 0 + "'";

                        Boolean EstaActConce = Conexion.SQLUpdate(Utils.SqlDatos);

                        if (EstaActConce == false)
                        {
                            Utils.Informa = "Error de administración de datos. ";
                            Utils.Informa += "al actualizar el campo UlConRemi " + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    } // final   if (Convert.ToInt32(TabContadores["UlConRemi"].ToString()) == 0)

                    //Devuelva el campo convertido en string

                    Convertido = "";

                    switch (Fac)
                    {
                        case double estado when Fac >= 1 && Fac <= 9:
                            Convertido = "00000" + Fac;
                            break;
                        case double estado when Fac >= 10 && Fac <= 99:
                            Convertido = "0000" + Fac;
                            break;
                        case double estado when Fac >= 100 && Fac <= 999:
                            Convertido = "000" + Fac;
                            break;
                        case double estado when Fac >= 1000 && Fac <= 9999:
                            Convertido = "00" + Fac;
                            break;
                        case double estado when Fac >= 10000 && Fac <= 99999:
                            Convertido = "0" + Fac;
                            break;
                        case double estado when Fac >= 100000 && Fac <= 999999:
                            Convertido = Convert.ToString(Fac);
                            break;
                        default:
                            Utils.Informa = "El consecutivo de remisiones ha pasado el";
                            Utils.Informa = Utils.Informa + "limite de seis (6) digitos, por lo tanto no";
                            Utils.Informa = Utils.Informa + "se puede generar otra remisión hasta que el";
                            Utils.Informa = Utils.Informa + "administrador del sistema amplie el rango.";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Convertido = "-3";
                            break;
                    }

                    TabContadores.Close();
                    return Convertido;

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion ConseRemisiones " + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "-1";
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }
        private string CodInterAdminEs(string CSgsss)
        {
            try
            {
                //Permite buscar el código interno de la entidad según el código del minsalud
                //en la base de datos de SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos administradoras de planes] WHERE CodAdmin = '" + CSgsss + "' ";

                SqlDataReader reader = Conexion.SQLDataReader(Utils.SqlDatos);

                if (reader.HasRows)
                {
                    reader.Read();

                    return reader["CodInterno"].ToString();

                }
                else
                {
                    return "0";
                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CodInterAdminEs " + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "-1";
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }
        private int ProceSoloPorFacturas(string NumFactur, string CodIPS, string NumRemi, string CT, string M, int TolDoc)
        {
            try
            {

                string AutoNum = null, CodProce = null, DxPrin = null, DxRel1 = null, DxCom = null, TD = null, NDocum = null;
                double TolPro;
                int CanPro = 0, VR = 0;


                string SqlProcedimientos = "SELECT [Datos cuentas de consumos].CuenNum, [Datos cuentas de consumos].HistoNum, [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos registros de consumos].CodiSOAT, [Datos registros de consumos].CodiISS,[Datos registros de consumos].CodiCUPS,[Datos registros de consumos].CodInter,  " +
                                            " [Datos cuentas de consumos].DxSalida, [Datos cuentas de consumos].NumRemi, [Datos registros de consumos].Cantidad,[Datos registros de consumos].ValorUnitario, [Datos registros de consumos].SubValorUnita, " +
                                            " [Datos cuentas de consumos].DxRelac01, [Datos cuentas de consumos].DxComplica, [Datos catalogo de servicios].GrupoServi, " +
                                            " [Datos registros de consumos].FechaCon, [Datos registros de consumos].RealizadoEn, [Datos registros de consumos].FinalProce, " +
                                            " [Datos registros de consumos].PerAtien, [Datos registros de consumos].FormaRealiza, [Datos registros de consumos].AutoriNum " +
                                            " FROM [Datos del Paciente] INNER JOIN [Datos cuentas de consumos] ON [Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum " +
                                            " INNER JOIN[Datos registros de consumos] ON[Datos cuentas de consumos].CuenNum = [Datos registros de consumos].CuenConsu " +
                                            " INNER JOIN[Datos catalogo de servicios] ON[Datos registros de consumos].CodInter = [Datos catalogo de servicios].CodInterno " +
                                            " WHERE [Datos cuentas de consumos].CuenNum = '" + CT + "' " +
                                            " AND [Datos registros de consumos].Cantidad > 0 " +
                                            " AND [Datos registros de consumos].ValorUnitario > 0 " +
                                            " AND [Datos registros de consumos].PagaHoja = 1 " +
                                            " AND [Datos registros de consumos].SeRepRips = 1 " +
                                            " AND ([Datos catalogo de servicios].GrupoServi = '02' OR[Datos catalogo de servicios].GrupoServi = '03'   OR[Datos catalogo de servicios].GrupoServi = '04' OR[Datos catalogo de servicios].GrupoServi = '05') ";

                SqlDataReader TabLocal = Conexion.SQLDataReader(SqlProcedimientos);

                if (TabLocal.HasRows == false)
                {
                    //No hay documentos previamente seleccionados a nombre del cardinal selevccionado;
                    return 0;
                }
                else
                {
                    while (TabLocal.Read())
                    {

                        AutoNum = TabLocal["AutoriNum"].ToString();

                        DxPrin = (TabLocal["DxSalida"].ToString() == "0000" ? null : TabLocal["DxSalida"].ToString());

                        DxRel1 = (TabLocal["DxRelac01"].ToString() == "0000" ? null : TabLocal["DxRelac01"].ToString());

                        DxCom = (TabLocal["DxComplica"].ToString() == "0000" ? null : TabLocal["DxComplica"].ToString());

                        TD = TabLocal["TipoIden"].ToString();

                        if (string.IsNullOrWhiteSpace(TD) == false && TD.Length > 2)
                        {
                            TD = TD.Substring(0, 2);

                        }

                        switch (M)
                        {
                            case "1": //Manual SOAT
                                CodProce = TabLocal["CodiSOAT"].ToString();
                                break;
                            case "2": //Manual UUS
                                CodProce = TabLocal["CodiISS"].ToString();
                                break;
                            case "3": //Manual CUPS
                                CodProce = TabLocal["CodiCUPS"].ToString();
                                break;
                            case "4": //Manual SOAT
                                CodProce = TabLocal["CodInter"].ToString();
                                break;
                            default:
                                CodProce = TabLocal["CodInter"].ToString();
                                break;
                        }


                        NDocum = TabLocal["NumIden"].ToString();

                        TolPro = Convert.ToDouble(TabLocal["ValorUnitario"].ToString()) + Convert.ToDouble(TabLocal["SubValorUnita"].ToString());

                        if (TolPro > 0)
                        {
                            CanPro = Convert.ToInt32(TabLocal["Cantidad"].ToString());

                            for (int i = 1; i <= CanPro; i++)
                            {
                                //Empiece agregar el registro a la tabla temporal


                                Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] " +
                                      "(CodDigita," +
                                      "NumRemi," +
                                      "NumFactur," +
                                      "CodIps," +
                                      "TipoDocum," +
                                      "NumDocum," +
                                      "FecProce," +
                                      "AutoriNum," +
                                      "CodProce," +
                                      "AmbitoReal," +
                                      "FinalProce," +
                                      "PersonAten," +
                                      "DxPrincipal," +
                                      "DxRelacion," +
                                      "Complicacion," +
                                      "RealiActo," +
                                      "ValorProce)" +
                                      "VALUES(" +
                                      "'" + lblCodigoUser.Text + "'," +
                                      "'" + NumRemi + "'," +
                                      "'" + NumFactur + "'," +
                                      "'" + CodIPS + "'," +
                                      "'" + TD + "'," +
                                      "'" + NDocum + "'," +
                                      "'" + Convert.ToDateTime(TabLocal["FechaCon"]).ToString("yyyy-MM-dd") + "'," +
                                      "'" + AutoNum + "'," +
                                      "'" + CodProce + "'," +
                                      "'" + TabLocal["RealizadoEn"].ToString() + "'," +
                                      "'" + TabLocal["FinalProce"].ToString() + "'," +
                                      "'" + TabLocal["PerAtien"].ToString() + "'," +
                                      "'" + DxPrin + "'," +
                                      "'" + DxRel1 + "'," +
                                      "'" + DxCom + "'," +
                                      "'" + TabLocal["FormaRealiza"].ToString() + "'," +
                                      "'" + TolPro + "');";


                                Boolean SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            } //fIN fOR

                        }//FIN TOL > 0

                        VR += 1;

                    }
                }


                return VR;


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después ejecutar la funcion ProceSoloPorFacturas" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        public int ComDatosMedica(string Ct)
        {
            try
            {

                string c = null, FormaFarma = null, ConcenMedi = null, NumFactur = null;

                //'**************  Creada el 11 de diciembre de 2003 ***************
                //'Permite colocar la información complementaria de los medicamentos NO POS y POS
                //'como es la forma la concentración, presentación, unidad, etc
                //'Esto funciona para aquella entidades que tienen definido el modulo de farmacia

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] where NumRemi =  '" + Ct + "' ";



                SqlDataReader ArchivoMedicamentos;

                using (SqlConnection connection9 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection9);
                    command8.Connection.Open();

                    ArchivoMedicamentos = command8.ExecuteReader();

                    if (ArchivoMedicamentos.HasRows == false)
                    {
                        //No se consiguió el código del producto en tabla de farmacia
                    }
                    else
                    {
                        //Proceda a actualizar

                        Utils.SqlDatos = "SELECT [Datos productos farmaceuticos].CodigoPro, [Datos forma farmaceutica].CodForFar, [Datos forma farmaceutica].NomForFar, [Datos productos farmaceuticos].Medida, [Datos unidades de medidas].AbreMedida,  " +
                                        " [Datos unidades de medidas].Descripcion, [Datos productos farmaceuticos].CodiMinSa, [Datos productos farmaceuticos].SiPos, [Datos productos farmaceuticos].Concentra " +
                                        " FROM [BDFARMA].[dbo].[Datos forma farmaceutica] INNER JOIN " +
                                        " [BDFARMA].[dbo].[Datos productos farmaceuticos] ON [BDFARMA].[dbo].[Datos forma farmaceutica].CodForFar = [BDFARMA].[dbo].[Datos productos farmaceuticos].Formafarma INNER JOIN " +
                                        " [BDFARMA].[dbo].[Datos unidades de medidas] ON[BDFARMA].[dbo].[Datos productos farmaceuticos].Medida = [BDFARMA].[dbo].[Datos unidades de medidas].CodigoMedida";

                        while (ArchivoMedicamentos.Read())
                        {
                            c = ArchivoMedicamentos["CodInterMedi"].ToString();

                            Utils.SqlDatos += "WHERE [Datos productos farmaceuticos].CodigoPro = '" + c + "'";


                            SqlDataReader reader;

                            using (SqlConnection connection10 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command9 = new SqlCommand(Utils.SqlDatos, connection10);
                                command9.Connection.Open();

                                reader = command9.ExecuteReader();


                                if (reader.HasRows == false)
                                {
                                    //No se consiguió el código del producto en tabla de farmacia
                                }
                                else
                                {
                                    reader.Read();
                                    if (Convert.ToInt32(reader["SiPos"]) == 0)
                                    {
                                        //'se debe buscar la forma
                                        FormaFarma = reader["NomForFar"].ToString();

                                        NumFactur = ArchivoMedicamentos["NumFactur"].ToString();

                                        if (string.IsNullOrWhiteSpace(FormaFarma) == false && FormaFarma.Length > 20)
                                        {
                                            FormaFarma = FormaFarma.Substring(0, 20);
                                        }
                                        //     'Se busca la unidad de medida
                                        ConcenMedi = reader["Concentra"].ToString();

                                        if (string.IsNullOrWhiteSpace(ConcenMedi) == false && ConcenMedi.Length > 20)
                                        {
                                            ConcenMedi = ConcenMedi.Substring(0, 20);
                                        }

                                        Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] SET CodMedica = '', FormaFarma = '" + FormaFarma + "'," +
                                                         " UniMedida = '" + reader["Descripcion"].ToString() + "', ConcenMedi = '" + ConcenMedi + "', TipoMedica = '2' WHERE NumFactur = '" + NumFactur + "' ";
                                    }
                                    else
                                    {
                                        Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] SET CodMedica = '" + reader["CodiMinSa"].ToString() + "', TipoMedica = '1' WHERE NumFactur = '" + NumFactur + "' ";
                                    }

                                    Boolean SqlUpdate = Conexion.SQLUpdate(Utils.SqlDatos);


                                }
                            }
                            reader.Close();
                        }
                        ArchivoMedicamentos.Close();
                    }
                }


                return 1;


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de llamar la funcion ComDatosMedica" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private string NomDiagnostico(string CoDx)
        {
            try
            {
                string SqlDatos = "SELECT [Datos listado de diagnosticos].* ";
                SqlDatos = SqlDatos + "FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos]";
                SqlDatos = SqlDatos + "WHERE ((([[Datos listado de diagnosticos]].[CodiDx]) = '" + CoDx + "')) ";
                SqlDatos = SqlDatos + "ORDER BY [[Datos listado de diagnosticos]].[CodiDx];";

                SqlDataReader TablaAux9 = Conexion.SQLDataReader(SqlDatos);

                if (TablaAux9.HasRows == false)
                {
                    return "0";
                }
                else
                {
                    return TablaAux9["NombreDx"].ToString();
                }

            }

            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: NomDiagnostico del Módulo" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "-1";
            }
        }
        private int ValidarProcedi(string c, double T, string CodDg)
        {
            try
            {
                string SqlProceTemp, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlProceTemp = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] ";
                SqlProceTemp += "WHERE (([Datos temporal procedimientos RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlProceTemp += "(([Datos temporal procedimientos RIPS].NumRemi) = '" + c + "');";


                SqlDataReader TabProce;

                // SqlDataReader TabProce = Conexion.SQLDataReader(SqlProceTemp);

                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command2 = new SqlCommand(SqlProceTemp, connection2);

                    command2.Connection.Open();

                    TabProce = command2.ExecuteReader();

                    if (TabProce.HasRows == false)
                    {
                        return 0;
                    }
                    else
                    {
                        VR = 0;
                        while (TabProce.Read())
                        {
                            RegExp = 0;
                            ObErr = "";
                            VR = 0;

                            //Validamos Ambito

                            switch (TabProce["AmbitoReal"])
                            {
                                case "1":
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = ObErr + "El identificador para determinar el ámbito de realización del procedimiento no es valido.";
                                    break;
                            }

                            switch (TabProce["FinalProce"])
                            {
                                case "1":
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    break;
                                case "4":
                                    break;
                                case "5":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = ObErr + "La finalidad del procedimiento no es valida para normatividad vigente.";
                                    break;
                            }

                            // Validamos el código del diagnóstico principal

                            if (string.IsNullOrEmpty(TabProce["DxPrincipal"].ToString()) || TabProce["DxPrincipal"].ToString() == "")
                            {

                            }
                            else
                            {
                                string DxPrincipal = TabProce["DxPrincipal"].ToString();
                                int lenDxPrincipal = DxPrincipal.Length;
                                if (lenDxPrincipal < 4)
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabProce["DxPrincipal"].ToString() + ", del diagnóstico  no es valido.";
                                }
                                else
                                {
                                    if (TabProce["DxPrincipal"].ToString() == "0000")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabProce["DxPrincipal"].ToString() + ", del diagnóstico  no es valido.";
                                    }
                                    else
                                    {
                                        DxPr = TabProce["DxPrincipal"].ToString();
                                        //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                        Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                        Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                        SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                        if (TablaAux1.HasRows == false)
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabProce["DxPrincipal"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                        }
                                        TablaAux1.Close();

                                        if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                                    }
                                }
                            }

                            //   'Validamos el Diagnostico Relacional unico


                            if (string.IsNullOrEmpty(TabProce["DxRelacion"].ToString()) || TabProce["DxRelacion"].ToString() == "")
                            {

                            }
                            else
                            {
                                string DxPrincipal = TabProce["DxRelacion"].ToString();
                                int lenDxPrincipal = DxPrincipal.Length;
                                if (lenDxPrincipal < 4)
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabProce["DxRelacion"].ToString() + ", del diagnóstico Relacional no es valido.";
                                }
                                else
                                {
                                    if (TabProce["DxRelacion"].ToString() == "0000")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabProce["DxRelacion"].ToString() + ", del diagnóstico Relacional  no es valido.";
                                    }
                                    else
                                    {
                                        DxPr = TabProce["DxRelacion"].ToString();
                                        //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                        Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                        Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                        SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                        if (TablaAux1.HasRows == false)
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabProce["DxPrincipal"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                        }
                                        TablaAux1.Close();

                                        if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                    }
                                }
                            }

                            //Validamos el Diagnostico de la complicación



                            if (string.IsNullOrEmpty(TabProce["Complicacion"].ToString()) || TabProce["Complicacion"].ToString() == "")
                            {

                            }
                            else
                            {
                                string DxPrincipal = TabProce["Complicacion"].ToString();
                                int lenDxPrincipal = DxPrincipal.Length;
                                if (lenDxPrincipal < 4)
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabProce["Complicacion"].ToString() + ", del diagnóstico Complicacion no es valido.";
                                }
                                else
                                {
                                    if (TabProce["Complicacion"].ToString() == "0000")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabProce["Complicacion"].ToString() + ", del diagnóstico Complicacion  no es valido.";
                                    }
                                    else
                                    {
                                        DxPr = TabProce["Complicacion"].ToString();
                                        //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                        Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                        Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);

                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabProce["DxPrincipal"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                            }

                                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                        }

                                    }
                                }
                            }

                            //Validamos La forma de realización

                            switch (TabProce["RealiActo"].ToString())
                            {
                                case "1":
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    break;
                                case "4":
                                    break;
                                case "5":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = ObErr + "El identificador " + TabProce["RealiActo"].ToString() + ", no es valido para determinar la forma de realización del acto quirúrgico.";
                                    break;
                            }

                            if (Convert.ToDouble(TabProce["ValorProce"].ToString()) < 0)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El valor del procedimiento no puede ser menor a cero ";
                            }


                            if (RegExp == 1)
                            {
                                Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                                Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                                Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AP','" + TabProce["TipoDocum"].ToString() + "','" + TabProce["NumDocum"].ToString() + "','" + c + "','" + TabProce["NumFactur"].ToString() + "','" + ObErr + "')";

                                Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                            }

                            VR += 1;

                        } //FIN WHILE

                        return 1;
                    }
                }

                TabProce.Close();


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función:  ValidarProcedi del " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int ValidarReNan(string c, double T, string CodDg)
        {
            try
            {
                string SqlReNaciTemp, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlReNaciTemp = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS] ";
                SqlReNaciTemp += "WHERE (([Datos temporal recien nacidos RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlReNaciTemp += "(([Datos temporal recien nacidos RIPS].NumRemi) = '" + c + "');";
                SqlDataReader TabReNan = Conexion.SQLDataReader(SqlReNaciTemp);

                if (TabReNan.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    VR = 0;
                    while (TabReNan.Read())
                    {
                        RegExp = 0;
                        ObErr = "";
                        VR = 0;

                        // 'Solo validamos las cosas necesarias
                        //'Se debe validar el código de la consulta de acuero al manual que utiliza la entidad
                        //'Validamos la finalidad


                        if (string.IsNullOrEmpty(TabReNan["EdadGesta"].ToString()) || TabReNan["EdadGesta"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "Falta el número de semanas de gestación.";
                        }
                        else
                        {
                            if (Convert.ToInt32(TabReNan["EdadGesta"].ToString()) > 42)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El número de semanas de gestación no puede ser mayor de 42 semanas.";
                            }
                        }
                        //   'Validamos el control prenatal

                        switch (TabReNan["ControlPrena"].ToString())
                        {
                            case "1":
                                break;
                            case "2":
                                break;
                            default:
                                RegExp = 1;
                                ObErr = ObErr + "El identificador " + TabReNan["ControlPrena"].ToString() + ", no es valido para el control prenatal";
                                break;
                        }

                        //'Validamos el sexo

                        switch (TabReNan["SexoRecien"].ToString())
                        {
                            case "M":
                                break;
                            case "F":
                                break;
                            case "I":
                                break;
                            default:
                                RegExp = 1;
                                ObErr = ObErr + "El identificador " + TabReNan["SexoRecien"].ToString() + ", no es valido para definir el sexo.";
                                break;
                        }

                        //Validamos el peso
                        int peso = Convert.ToInt32(TabReNan["PesoRecien"].ToString());

                        if (peso <= 0)
                        {
                            RegExp = 1;
                            ObErr = ObErr + "Falta el peso del recién nacido.";
                        }
                        if (peso <= 1000)
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El peso del recién nacido esta por debajo de la media.";
                        }
                        if (peso > 600)
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El peso del recién nacido parece estar sobrevalorado.";
                        }

                        //Validamos el código del diagnóstico del recien nacido


                        if (string.IsNullOrEmpty(TabReNan["DxRecien"].ToString()) || TabReNan["DxRecien"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "No tiene definido el diagnóstico del recién nacido";
                        }
                        else
                        {
                            string DxPrincipal = TabReNan["DxRecien"].ToString();
                            int lenDxPrincipal = DxPrincipal.Length;
                            if (lenDxPrincipal < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabReNan["DxRecien"].ToString() + ", del diagnóstico  no es valido.";
                            }
                            else
                            {
                                if (TabReNan["DxRecien"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabReNan["DxRecien"].ToString() + ", del diagnóstico  no es valido.";
                                }
                                else
                                {
                                    DxPr = TabReNan["DxRecien"].ToString();
                                    FunDx = NomDiagnostico(DxPr);

                                    if (FunDx == "0")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabReNan["DxRecien"].ToString() + ", del diagnóstico del recién nacido no existe la resolución vigente.";
                                    }
                                }
                            }
                        }


                        //Si se murio debe llevar un codigo

                        if (string.IsNullOrEmpty(TabReNan["DxMuerte"].ToString()) || TabReNan["DxMuerte"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "A pesar que el paciente murió, no tiene definido el diagnóstico de la causa básica";
                        }
                        else
                        {

                            string DxRelacion2 = TabReNan["DxMuerte"].ToString();
                            int lenDxRelacion2 = DxRelacion2.Length;
                            if (lenDxRelacion2 < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabReNan["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no es valido.";
                            }
                            else
                            {
                                if (TabReNan["DxMuerte"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabReNan["DxMuerte"].ToString() + ",  del diagnóstico de la causa básica no es valido.";
                                }
                                else
                                {
                                    DxPr = TabReNan["DxMuerte"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabReNan["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }



                        if (RegExp == 1)
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                            Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                            Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AN','" + TabReNan["TipoDocum"].ToString() + "','" + TabReNan["NumDocum"].ToString() + "','" + c + "','" + TabReNan["NumFactur"].ToString() + "','" + ObErr + "')";

                            Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                        }

                        VR += 1;

                    } //FIN WHILE

                    return 1;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarReNan del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int ValidarOtros(string c, double T, string CodDg)
        {
            try
            {
                string SqlOtrosTemp, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlOtrosTemp = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] ";
                SqlOtrosTemp += "WHERE (([Datos temporal otros servicios RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlOtrosTemp += "(([Datos temporal otros servicios RIPS].NumRemi) = '" + c + "');";

                SqlDataReader TabOtros = Conexion.SQLDataReader(SqlOtrosTemp);

                if (TabOtros.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    VR = 0;
                    while (TabOtros.Read())
                    {
                        RegExp = 0;
                        ObErr = "";

                        // 'Solo validamos las cosas necesarias
                        //'Se debe validar el código de la consulta de acuero al manual que utiliza la entidad
                        //'Validamos la finalidad

                        switch (TabOtros["TipoServicio"].ToString())
                        {
                            case "1":
                                break;
                            case "2":
                                break;
                            case "3":
                                break;
                            case "4":
                                break;
                            default:
                                ObErr = ObErr + "El identificador para determinar el tipo de servicio hace falta en el servicio de código ";
                                break;
                        }

                        // 'Validamos el código del servicio

                        if (string.IsNullOrEmpty(TabOtros["CodiServi"].ToString()) || TabOtros["CodiServi"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = "El nombre del servicio hace falta.";
                        }

                        //validamos la cantidad

                        if (Convert.ToDecimal(TabOtros["ValorUnita"].ToString()) <= 0)
                        {
                            RegExp = 1;
                            ObErr = ObErr + "el valor unitario del servicio no puede ser menor o igual a cero.";
                        }


                        if (RegExp == 1)
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                            Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                            Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AT','" + TabOtros["TipoDocum"].ToString() + "','" + TabOtros["NumDocum"].ToString() + "','" + c + "','" + TabOtros["NumFactur"].ToString() + "','" + ObErr + "')";

                            Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                        }

                        VR += 1;

                    } //FIN WHILE

                    return 1;
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarOtros del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int ValidarObserva(string c, double T, string CodDg)
        {
            try
            {
                string SqlMediTem, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlMediTem = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS] ";
                SqlMediTem += "WHERE (([Datos temporal observacion RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlMediTem += "(([Datos temporal observacion RIPS].NumRemi) = '" + c + "');";
                SqlDataReader TabObserva = Conexion.SQLDataReader(SqlMediTem);

                if (TabObserva.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    VR = 0;
                    while (TabObserva.Read())
                    {
                        RegExp = 0;
                        ObErr = "";
                        VR = 0;

                        // 'Solo validamos las cosas necesarias
                        //'Se debe validar el código de la consulta de acuero al manual que utiliza la entidad
                        //'Validamos la finalidad

                        switch (TabObserva["CausExter"].ToString())
                        {
                            case "01":
                            case "02":
                            case "03":
                            case "04":
                            case "05":
                            case "06":
                            case "07":
                            case "08":
                            case "09":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            default:
                                RegExp = 1;
                                ObErr += "La causa externa de la urgencia no es valida para la normatividad vigente.";
                                break;
                        }



                        if (string.IsNullOrEmpty(TabObserva["DxPrincIngre"].ToString()) || TabObserva["DxPrincIngre"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "La consulta no tiene definido el diagnóstico principal";
                        }
                        else
                        {
                            string DxPrincipal = TabObserva["DxPrincIngre"].ToString();
                            int lenDxPrincipal = DxPrincipal.Length;
                            if (lenDxPrincipal < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabObserva["DxPrincIngre"].ToString() + ", del diagnóstico principal no es valido.";
                            }
                            else
                            {
                                if (TabObserva["DxPrincIngre"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabObserva["DxPrincIngre"].ToString() + ", del diagnóstico principal no es valido.";
                                }
                                else
                                {
                                    DxPr = TabObserva["DxPrincIngre"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabObserva["DxPrincIngre"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el Diagnostico Relacional 1

                        if (string.IsNullOrEmpty(TabObserva["DxRelacion1"].ToString()) || TabObserva["DxRelacion1"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "La consulta no tiene definido el diagnóstico DxRelacion1";
                        }
                        else
                        {
                            string DxPrincipal = TabObserva["DxRelacion1"].ToString();
                            int lenDxPrincipal = DxPrincipal.Length;
                            if (lenDxPrincipal < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabObserva["DxRelacion1"].ToString() + ", del diagnóstico DxRelacion1 no es valido.";
                            }
                            else
                            {
                                if (TabObserva["DxRelacion1"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabObserva["DxRelacion1"].ToString() + ", del diagnóstico DxRelacion1 no es valido.";
                                }
                                else
                                {
                                    DxPr = TabObserva["DxRelacion1"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabObserva["DxRelacion1"].ToString() + ", del diagnóstico DxRelacion1 no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }


                        //Validamos el Diagnostico Relacional 2

                        if (string.IsNullOrEmpty(TabObserva["DxRelacion2"].ToString()) || TabObserva["DxRelacion2"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "La consulta no tiene definido el diagnóstico DxRelacion2";
                        }
                        else
                        {
                            string DxPrincipal = TabObserva["DxRelacion2"].ToString();
                            int lenDxPrincipal = DxPrincipal.Length;
                            if (lenDxPrincipal < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabObserva["DxRelacion2"].ToString() + ", del diagnóstico DxRelacion1 no es valido.";
                            }
                            else
                            {
                                if (TabObserva["DxRelacion2"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabObserva["DxRelacion2"].ToString() + ", del diagnóstico DxRelacion2 no es valido.";
                                }
                                else
                                {
                                    DxPr = TabObserva["DxRelacion2"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabObserva["DxRelacion2"].ToString() + ", del diagnóstico DxRelacion2 no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el destino del usuario

                        switch (TabObserva["Destino"].ToString())
                        {
                            case "1":
                                break;
                            case "2":
                                break;
                            case "3":
                                break;
                            default:
                                RegExp = 1;
                                ObErr = ObErr + "El identificador para determinar el destino del usuario a la salida de observación no es valido.";
                                break;
                        }


                        switch (TabObserva["EstadoSal"].ToString())
                        {
                            case "1":
                                break;
                            case "2":

                                //Si se murio debe llevar un codigo
                                if (string.IsNullOrEmpty(TabObserva["DxMuerte"].ToString()) || TabObserva["DxMuerte"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "A pesar que el paciente murió, no tiene definido el diagnóstico de la causa básica";
                                }
                                else
                                {

                                    string DxRelacion2 = TabObserva["DxMuerte"].ToString();
                                    int lenDxRelacion2 = DxRelacion2.Length;
                                    if (lenDxRelacion2 < 4)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabObserva["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no es valido.";
                                    }
                                    else
                                    {
                                        if (TabObserva["DxMuerte"].ToString() == "0000")
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabObserva["DxMuerte"].ToString() + ",  del diagnóstico de la causa básica no es valido.";
                                        }
                                        else
                                        {
                                            DxPr = TabObserva["DxMuerte"].ToString();
                                            //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                            Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                            Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                            SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabObserva["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                            }
                                            TablaAux1.Close();
                                        }
                                    }
                                }

                                break;
                            default:
                                RegExp = 1;
                                ObErr = ObErr + "El identificador para determinar el estado a la salida no es valido para la normatividad vigente.";
                                break;
                        }

                        //'Validamos si lleva la fecha de salida

                        if (string.IsNullOrEmpty(TabObserva["FecSalida"].ToString()))
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El usuario no tiene la fecha de salida de observacion de urgencias ";
                        }



                        if (string.IsNullOrEmpty(TabObserva["HorSalida"].ToString()))
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El usuario no tiene la hora de salida de observacion de urgencias ";
                        }



                        if (RegExp == 1)
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                            Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                            Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AU','" + TabObserva["TipoDocum"].ToString() + "','" + TabObserva["NumDocum"].ToString() + "','" + c + "','" + TabObserva["NumFactur"].ToString() + "','" + ObErr + "')";

                            Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                        }

                        VR += 1;

                    } //FIN WHILE

                    return 1;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarObserva del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int ValidarMedica(string c, double T, string CodDg)
        {
            try
            {
                string SqlMediTem, Dp, ObErr, Z;
                int RegExp, VR;

                SqlMediTem = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] ";
                SqlMediTem += "WHERE (([Datos temporal medicamentos RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlMediTem += "(([Datos temporal medicamentos RIPS].NumRemi) = '" + c + "');";

                SqlDataReader TabMedi = Conexion.SQLDataReader(SqlMediTem);

                if (TabMedi.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    VR = 0;
                    while (TabMedi.Read())
                    {
                        RegExp = 0;
                        ObErr = "";

                        // 'Solo validamos las cosas necesarias
                        //'Se debe validar el código de la consulta de acuero al manual que utiliza la entidad
                        //'Validamos la finalidad

                        switch (TabMedi["TipoMedica"].ToString())
                        {
                            case "01": //'Es POS

                                //Validamos el código del medicamento
                                if (string.IsNullOrEmpty(TabMedi["CodMedica"].ToString()) || TabMedi["CodMedica"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr = "El código del medicamento hace falta cuando este es POS.";
                                }

                                break;

                            case "02": //No es Pos

                                //Validamos el codigo del medicamento
                                if (string.IsNullOrEmpty(TabMedi["NomGenerico"].ToString()) || TabMedi["NomGenerico"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr = "El forma farmacéutica del medicamento hace falta cuando este es NO POS.";
                                }

                                //Validamos La forma
                                if (string.IsNullOrEmpty(TabMedi["FormaFarma"].ToString()) || TabMedi["FormaFarma"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr += "El forma farmacéutica del medicamento hace falta cuando este es NO POS.";
                                }

                                //Validamos la concentración
                                if (string.IsNullOrEmpty(TabMedi["ConcenMedi"].ToString()) || TabMedi["ConcenMedi"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr += "El forma farmacéutica del medicamento hace falta cuando este es NO POS.";
                                }

                                //'Validamos la unidad de medida
                                if (string.IsNullOrEmpty(TabMedi["UniMedida"].ToString()) || TabMedi["UniMedida"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr += "La unidad de medida del medicamento hace falta cuando este es NO POS.";
                                }
                                break;
                            default:

                                ObErr += "El identificador para determinar la condición del medicamento en el plan de beneficios hace falta.";

                                break;
                        }


                        //Validamos la unidades administrdas

                        if (Convert.ToDecimal(TabMedi["NumUnidad"].ToString()) <= 0)
                        {
                            RegExp = 1;
                            ObErr = ObErr + "La Cantidad o las unidades aplicadas no puede ser menor o igual a cero..";
                        }
                        else
                        {
                            if (Convert.ToDecimal(TabMedi["NumUnidad"].ToString()) > 1000)
                            {
                                ObErr = ObErr + "La Cantidad o las unidades aplicadas no puede ser mayor a 1.000.";
                            }
                        }

                        // 'Validamos que el valor unitario

                        if (Convert.ToDecimal(TabMedi["ValorUnita"].ToString()) <= 0)
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El valor unitario del medicamento no puede ser menor o igual a cero.";
                        }


                        if (RegExp == 1)
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                            Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                            Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AM','" + TabMedi["TipoDocum"].ToString() + "','" + TabMedi["NumDocum"].ToString() + "','" + c + "','" + TabMedi["NumFactur"].ToString() + "','" + ObErr + "')";

                            Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                        }

                        VR += 1;

                    } //FIN WHILE

                    return 1;
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarMedica del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }
        private int ValidarHospi(string c, double T, string CodDg)
        {
            try
            {

                string SqlConsulTem, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlConsulTem = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS] " +
                "WHERE (([Datos temporal hospitalizacion RIPS].CodDigita) = '" + CodDg + "' ) and " +
                "(([Datos temporal hospitalizacion RIPS].NumRemi) = '" + c + "');";

                SqlDataReader TabHospi = Conexion.SQLDataReader(SqlConsulTem);

                if (TabHospi.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    VR = 0;
                    while (TabHospi.Read())
                    {
                        RegExp = 0;
                        ObErr = "";

                        // 'Solo validamos las cosas necesarias
                        //'Se debe validar el código de la consulta de acuero al manual que utiliza la entidad
                        //'Validamos la finalidad

                        switch (TabHospi["ViaDIngreso"].ToString())
                        {
                            case "01":
                                break;
                            case "02":
                                break;
                            case "03":
                                break;
                            case "04":
                                break;
                            default:
                                RegExp = 1;
                                ObErr = "El identificador para determinar la vía de ingreso a la institución no es valida.";
                                break;
                        }

                        //'Validamos si lleva la fecha de ingreso

                        if (string.IsNullOrEmpty(TabHospi["FecIngresa"].ToString()))
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El usuario no tiene la fecha de ingreso a la institución ";
                        }

                        //'Validamos si lleva la fecha de salida

                        if (string.IsNullOrEmpty(TabHospi["HorIngresa"].ToString()))
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El usuario no tiene la hora de ingreso a la institución ";
                        }

                        switch (TabHospi["CausExter"].ToString())
                        {
                            case "01":
                            case "02":
                            case "03":
                            case "04":
                            case "05":
                            case "06":
                            case "07":
                            case "08":
                            case "09":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            default:
                                RegExp = 1;
                                ObErr = "La causa externa de la consulta no es valida para la normatividad vigente.";
                                break;
                        }


                        //Validamos el código del diagnóstico principal

                        if (string.IsNullOrEmpty(TabHospi["DxPrincIngre"].ToString()) || TabHospi["DxPrincIngre"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "La consulta no tiene definido el diagnóstico principal";
                        }
                        else
                        {
                            string DxPrincipal = TabHospi["DxPrincIngre"].ToString();
                            int lenDxPrincipal = DxPrincipal.Length;
                            if (lenDxPrincipal < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabHospi["DxPrincIngre"].ToString() + ", del diagnóstico principal no es valido.";
                            }
                            else
                            {
                                if (TabHospi["DxPrincIngre"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabHospi["DxPrincIngre"].ToString() + ", del diagnóstico principal no es valido.";
                                }
                                else
                                {
                                    DxPr = TabHospi["DxPrincIngre"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxPrincIngre"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el código del diagnóstico principal de egreso

                        if (string.IsNullOrEmpty(TabHospi["DxPrincEgre"].ToString()) || TabHospi["DxPrincEgre"].ToString() == "")
                        {
                            RegExp = 1;
                            ObErr = ObErr + "La consulta no tiene definido el diagnóstico principal de egreso ";
                        }
                        else
                        {
                            string DxPrincipal = TabHospi["DxPrincEgre"].ToString();
                            int lenDxPrincipal = DxPrincipal.Length;
                            if (lenDxPrincipal < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabHospi["DxPrincEgre"].ToString() + ", del diagnóstico principal de egreso no es valido.";
                            }
                            else
                            {
                                if (TabHospi["DxPrincEgre"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabHospi["DxPrincEgre"].ToString() + ", del diagnóstico principal de egreso no es valido.";
                                }
                                else
                                {
                                    DxPr = TabHospi["DxPrincEgre"].ToString();

                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no

                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxPrincEgre"].ToString() + ", el diagnóstico principal de egreso no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el tipo de diagnóstico


                        //Validamos el Diagnostico Relacional 1

                        if (string.IsNullOrEmpty(TabHospi["DxRelacion1"].ToString()) || TabHospi["DxRelacion1"].ToString() == "")
                        {
                            //Todo bien
                        }
                        else
                        {

                            string DxRelacion1 = TabHospi["DxRelacion1"].ToString();
                            int lenDxRelacion1 = DxRelacion1.Length;
                            if (lenDxRelacion1 < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabHospi["DxRelacion1"].ToString() + ", del diagnóstico relacional 1  no es valido.";
                            }
                            else
                            {
                                if (TabHospi["DxRelacion1"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabHospi["DxRelacion1"].ToString() + ", del diagnóstico relacional 1  no es valido.";
                                }
                                else
                                {
                                    DxPr = TabHospi["DxRelacion1"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxRelacion1"].ToString() + ", del diagnóstico relacional 1 no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el Diagnostico Relacional 2

                        if (string.IsNullOrEmpty(TabHospi["DxRelacion2"].ToString()) || TabHospi["DxRelacion2"].ToString() == "")
                        {
                            //Todo bien
                        }
                        else
                        {

                            string DxRelacion2 = TabHospi["DxRelacion2"].ToString();
                            int lenDxRelacion2 = DxRelacion2.Length;
                            if (lenDxRelacion2 < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabHospi["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 no es valido.";
                            }
                            else
                            {
                                if (TabHospi["DxRelacion2"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabHospi["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 no es valido.";
                                }
                                else
                                {
                                    DxPr = TabHospi["DxRelacion2"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el Diagnostico Relacional 3

                        if (string.IsNullOrEmpty(TabHospi["DxRelacion3"].ToString()) || TabHospi["DxRelacion3"].ToString() == "")
                        {
                            //Todo bien
                        }
                        else
                        {

                            string DxRelacion2 = TabHospi["DxRelacion3"].ToString();
                            int lenDxRelacion2 = DxRelacion2.Length;
                            if (lenDxRelacion2 < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabHospi["DxRelacion3"].ToString() + ", del diagnóstico  Relacional 3 no es valido.";
                            }
                            else
                            {
                                if (TabHospi["DxRelacion3"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabHospi["DxRelacion3"].ToString() + ", del diagnóstico  Relacional 3 no es valido.";
                                }
                                else
                                {
                                    DxPr = TabHospi["DxRelacion3"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxRelacion3"].ToString() + ", del diagnóstico  Relacional 3 no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el Diagnostico DxComplica

                        if (string.IsNullOrEmpty(TabHospi["DxComplica"].ToString()) || TabHospi["DxComplica"].ToString() == "")
                        {
                            //Todo bien
                        }
                        else
                        {

                            string DxRelacion2 = TabHospi["DxComplica"].ToString();
                            int lenDxRelacion2 = DxRelacion2.Length;
                            if (lenDxRelacion2 < 4)
                            {
                                RegExp = 1;
                                ObErr = ObErr + "El código " + TabHospi["DxComplica"].ToString() + ", del diagnóstico de la complicación no es valido.";
                            }
                            else
                            {
                                if (TabHospi["DxComplica"].ToString() == "0000")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabHospi["DxComplica"].ToString() + ", del diagnóstico de la complicación no es valido.";
                                }
                                else
                                {
                                    DxPr = TabHospi["DxComplica"].ToString();
                                    //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                    Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                    Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                    SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                    if (TablaAux1.HasRows == false)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxComplica"].ToString() + ", del diagnóstico  Relacional 3 no existe la resolución vigente.";
                                    }
                                    TablaAux1.Close();
                                }
                            }
                        }

                        //Validamos el estado a la salida

                        switch (TabHospi["EstadoSal"].ToString())
                        {
                            case "1":
                            case "2":

                                //Si se murio debe llevar un codigo
                                if (string.IsNullOrEmpty(TabHospi["DxMuerte"].ToString()) || TabHospi["DxMuerte"].ToString() == "")
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "A pesar que el paciente murió, no tiene definido el diagnóstico de la causa básica";
                                }
                                else
                                {

                                    string DxRelacion2 = TabHospi["DxMuerte"].ToString();
                                    int lenDxRelacion2 = DxRelacion2.Length;
                                    if (lenDxRelacion2 < 4)
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabHospi["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no es valido.";
                                    }
                                    else
                                    {
                                        if (TabHospi["DxMuerte"].ToString() == "0000")
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabHospi["DxMuerte"].ToString() + ",  del diagnóstico de la causa básica no es valido.";
                                        }
                                        else
                                        {
                                            DxPr = TabHospi["DxMuerte"].ToString();
                                            //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                            Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                            Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                            SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                            }
                                            TablaAux1.Close();
                                        }
                                    }
                                }

                                break;

                            default:
                                RegExp = 1;
                                ObErr = ObErr + "El identificador para determinar el estado a la salida no es valido para la normatividad vigente.";
                                break;
                        }

                        //'Validamos si lleva la fecha de salida

                        if (string.IsNullOrEmpty(TabHospi["FecSalida"].ToString()))
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El usuario no tiene la fecha de salida de observacion de urgencias ";
                        }



                        if (string.IsNullOrEmpty(TabHospi["HorSalida"].ToString()))
                        {
                            RegExp = 1;
                            ObErr = ObErr + "El usuario no tiene la hora de salida de observacion de urgencias ";
                        }


                        if (RegExp == 1)
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                            Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                            Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AH','" + TabHospi["TipoDocum"].ToString() + "','" + TabHospi["NumDocum"].ToString() + "','" + c + "','" + TabHospi["NumFactur"].ToString() + "','" + ObErr + "')";

                            Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                        }

                        VR += 1;
                    } //fin WHILE

                    return 1;

                } //'Final de TabLocal.BOF

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarHospi del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }
        private int ValidaConsultas(string c, string AR, double T, string CodDg)
        {
            try
            {

                string SqlConsulTem, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlConsulTem = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] " +
                "WHERE (([Datos temporal consultas RIPS].CodDigita) = '" + CodDg + "' ) and " +
                "(([Datos temporal consultas RIPS].NumRemi) = '" + c + "');";


                SqlDataReader TabConsul;

                //   SqlDataReader TabConsul = Conexion.SQLDataReader(SqlConsulTem);

                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command2 = new SqlCommand(SqlConsulTem, connection2);

                    command2.Connection.Open();

                    TabConsul = command2.ExecuteReader();

                    if (TabConsul.HasRows == false)
                    {
                        return 0;
                    }
                    else
                    {
                        VR = 0;
                        while (TabConsul.Read())
                        {
                            RegExp = 0;
                            ObErr = "";

                            // 'Solo validamos las cosas necesarias
                            //'Se debe validar el código de la consulta de acuero al manual que utiliza la entidad
                            //'Validamos la finalidad

                            switch (TabConsul["FinalConsul"].ToString())
                            {
                                case "01":
                                    break;
                                case "02":
                                    break;
                                case "03":
                                    break;
                                case "04":
                                    break;
                                case "05":
                                    break;
                                case "06":
                                    break;
                                case "07":
                                    break;
                                case "08":
                                    break;
                                case "09":
                                    break;
                                case "10":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = "El código de laa finalidad de la consulta no es valida para la normatividad vigente.";
                                    break;
                            }

                            if (AR == "0" || AR == "07")
                            {
                                //La empresa realiza o factura actividades de P y P, por tanto se necesita su finalidad
                                //Validamos la finalidad de la consulta
                                if (TabConsul["FinalConsul"].ToString() == "10")
                                {
                                    RegExp = 1;
                                    ObErr += "La finalidad de la consulta no puede ser 10, porque se está facturando P y P.";
                                }
                            }
                            else
                            {
                                if (TabConsul["FinalConsul"].ToString() != "10")
                                {
                                    RegExp = 1;
                                    ObErr += "La finalidad de la consulta " + TabConsul["FinalConsul"].ToString() + " no aplica para entidades que no facturan P y P.";
                                }
                            }


                            switch (TabConsul["CausExter"].ToString())
                            {
                                case "01":
                                    break;
                                case "02":
                                    break;
                                case "03":
                                    break;
                                case "04":
                                    break;
                                case "05":
                                    break;
                                case "06":
                                    break;
                                case "07":
                                    break;
                                case "08":
                                    break;
                                case "09":
                                    break;
                                case "10":
                                    break;
                                case "11":
                                    break;
                                case "12":
                                    break;
                                case "13":
                                    break;
                                case "14":
                                    break;
                                case "15":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = "La causa externa de la consulta no es valida para la normatividad vigente.";
                                    break;
                            }

                            //Validamos el código del diagnóstico principal

                            if (string.IsNullOrEmpty(TabConsul["DxPrincipal"].ToString()) || TabConsul["DxPrincipal"].ToString() == "")
                            {
                                RegExp = 1;
                                ObErr = ObErr + "La consulta no tiene definido el diagnóstico principal";
                            }
                            else
                            {
                                string DxPrincipal = TabConsul["DxPrincipal"].ToString();
                                int lenDxPrincipal = DxPrincipal.Length;
                                if (lenDxPrincipal < 4)
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabConsul["DxPrincipal"].ToString() + ", del diagnóstico principal no es valido.";
                                }
                                else
                                {
                                    if (TabConsul["DxPrincipal"].ToString() == "0000")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabConsul["DxPrincipal"].ToString() + ", del diagnóstico principal no es valido.";
                                    }
                                    else
                                    {
                                        DxPr = TabConsul["DxPrincipal"].ToString();
                                        //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                        Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                        Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                        SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                        if (TablaAux1.HasRows == false) //aqui
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabConsul["DxPrincipal"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                        }

                                        if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
                                    }
                                }
                            }

                            //Validamos el Diagnostico Relacional 1

                            if (string.IsNullOrEmpty(TabConsul["DxRelacion1"].ToString()) || TabConsul["DxRelacion1"].ToString() == "")
                            {
                                //Todo bien
                            }
                            else
                            {

                                string DxRelacion1 = TabConsul["DxRelacion1"].ToString();
                                int lenDxRelacion1 = DxRelacion1.Length;
                                if (lenDxRelacion1 < 4)
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabConsul["DxRelacion1"].ToString() + ", del diagnóstico relacional 1  no es valido.";
                                }
                                else
                                {
                                    if (TabConsul["DxRelacion1"].ToString() == "0000")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabConsul["DxRelacion1"].ToString() + ", del diagnóstico relacional 1  no es valido.";
                                    }
                                    else
                                    {
                                        DxPr = TabConsul["DxRelacion1"].ToString();
                                        //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                        Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                        Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                        SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                        if (TablaAux1.HasRows == false)
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabConsul["DxRelacion1"].ToString() + ", del diagnóstico relacional 1 no existe la resolución vigente.";
                                        }
                                        TablaAux1.Close();

                                        if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
                                    }

                                }
                            }

                            //Validamos el Diagnostico Relacional 2

                            if (string.IsNullOrEmpty(TabConsul["DxRelacion2"].ToString()) || TabConsul["DxRelacion2"].ToString() == "")
                            {
                                //Todo bien
                            }
                            else
                            {

                                string DxRelacion2 = TabConsul["DxRelacion2"].ToString();
                                int lenDxRelacion2 = DxRelacion2.Length;
                                if (lenDxRelacion2 < 4)
                                {
                                    RegExp = 1;
                                    ObErr = ObErr + "El código " + TabConsul["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 no es valido.";
                                }
                                else
                                {
                                    if (TabConsul["DxRelacion2"].ToString() == "0000")
                                    {
                                        RegExp = 1;
                                        ObErr = ObErr + "El código " + TabConsul["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 no es valido.";
                                    }
                                    else
                                    {
                                        DxPr = TabConsul["DxRelacion2"].ToString();
                                        //Si pasó todo lo anterior pasemos a verificar si el diagnóstico existe o no
                                        Utils.SqlDatos = "SELECT [Datos listado de diagnosticos].*, [Datos listado de diagnosticos].[CodiDx] FROM [GEOGRAXPSQL].[dbo].[Datos listado de diagnosticos] ";
                                        Utils.SqlDatos += "WHERE [Datos listado de diagnosticos].[CodiDx] = '" + DxPr + "' ORDER BY [Datos listado de diagnosticos].[CodiDx] ";

                                        SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                                        if (TablaAux1.HasRows == false)
                                        {
                                            RegExp = 1;
                                            ObErr = ObErr + "El código " + TabConsul["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 1 no existe la resolución vigente.";
                                        }
                                        TablaAux1.Close();
                                    }
                                }
                            }

                            //Validamos el tipo de diagnóstico

                            switch (TabConsul["TipoDxPrin"].ToString())
                            {
                                case "1":
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = ObErr + "El identificador " + TabConsul["TipoDxPrin"].ToString() + ", no es valido para determinar el tipo de Dx.";
                                    break;
                            }


                            if (RegExp == 1)
                            {
                                Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                                Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                                Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AC','" + TabConsul["TipoDocum"].ToString() + "','" + TabConsul["NumDocum"].ToString() + "','" + c + "','" + TabConsul["NumFactur"].ToString() + "','" + ObErr + "')";

                                Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                            }

                            VR += 1;
                        }

                        return 1;

                    }

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidaConsultas del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }
        private int ValidarFacturas(string c, double T, string CodDg)
        {
            try
            {
                string SqlFacTemp, Z, ObErr, Dp, MuCi;
                int RegExp, FunDoc, FunDpto, VR;

                SqlFacTemp = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] " +
                            "WHERE (([Datos temporal transacciones RIPS].CodDigita) = '" + CodDg + "') and " +
                            "(([Datos temporal transacciones RIPS].NumRemi) = '" + c + "');";

                SqlDataReader TabLoc = Conexion.SQLDataReader(SqlFacTemp);

                if (TabLoc.HasRows == false)
                {
                    return 0;
                }
                else
                {
                    VR = 0;
                    while (TabLoc.Read())
                    {
                        RegExp = 0;
                        ObErr = "";

                        // Solo validamos las cosas necesarios

                        //Validar cuando la causa de ingreso es accidebte pedir poliza

                        if (TabLoc["CausExter"].ToString() == "02") // Esto se empieza a validar desde el 22 de marzo de 2012
                                                                    //Revisamos si se registro un numero de poliza
                        {
                            if (string.IsNullOrEmpty(TabLoc["NumPoli"].ToString()) || TabLoc["NumPoli"].ToString() == "0")
                            {
                                RegExp = 1;
                                ObErr = "Por ser un accidente de transito, el número de la póliza es obligatorio.";
                            }
                        }

                        //Validamos copago

                        if (Convert.ToInt32(TabLoc["Copago"].ToString()) < 0)
                        {
                            RegExp = 1;
                            ObErr = "El valor del copago de la factura no puede ser negativo.";
                        }

                        //Validamos el valor de la comision

                        if (Convert.ToInt32(TabLoc["ValorComi"].ToString()) < 0)
                        {
                            RegExp = 1;
                            ObErr += "El valor de la comisión de la factura no puede ser negativo.";
                        }

                        //Validamos el valor total de descuentos

                        if (Convert.ToInt32(TabLoc["ValorDes"].ToString()) < 0)
                        {
                            RegExp = 1;
                            ObErr += "El valor total del descuento de la factura no puede ser negativo.";
                        }

                        if (Convert.ToInt32(TabLoc["ValorNeto"].ToString()) < 0)
                        {
                            RegExp = 1;
                            ObErr += "El valor neto de la factura no puede ser negativo.";
                        }

                        if ((Convert.ToDecimal(TabLoc["Copago"].ToString()) + Convert.ToDecimal(TabLoc["ValorNeto"].ToString())) != Convert.ToDecimal(TabLoc["VaLorDeta"].ToString()))
                        {
                            RegExp = 1;
                            ObErr += "El valor neto de la factura no puede ser diferente al valor total del detalle.";
                        }

                        if (RegExp == 1)
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                            Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                            Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'AF','" + TabLoc["TipIdenti"].ToString() + "','" + TabLoc["NumIdenti"].ToString() + "','" + c + "','" + TabLoc["NumFactur"].ToString() + "','" + ObErr + "')";

                            Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                        }

                        VR += 1;

                    }//fINAL While

                    return 1;

                    TabLoc.Close();
                }//Final TabLocal

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarFacturas del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int AuditaDetaFacturas(string CI)
        {
            try
            {
                double TolCon = 0, TolMedi = 0, TolOtros = 0, TolProce = 0, VaLorDetaActual;
                string TemEnti, NF;


                Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] SET VaLorDeta = 0 WHERE NumRemi = '" + CI + "' ";

                Boolean UpdateTrans = Conexion.SQLUpdate(Utils.SqlDatos);

                //Auditamos cada una de las facturas de consultas


                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal1;

                using (SqlConnection connection8 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection8);
                    command8.Connection.Open();

                    TabLocal1 = command8.ExecuteReader();

                    if (TabLocal1.HasRows == false)
                    {
                        TolCon = 0;
                    }
                    else
                    {
                        while (TabLocal1.Read())
                        {
                            NF = TabLocal1["NumFactur"].ToString();
                            TolCon = Convert.ToDouble(TabLocal1["ValorConsul"]);


                            Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] SET VaLorDeta = (VaLorDeta + " + TolCon + ") WHERE NumRemi = '" + CI + "' and NumFactur = '" + NF + "' ";

                            Boolean EstaAct = Conexion.SQLUpdate(Utils.SqlDatos);

                        }
                    }
                }

                //'Suma los medicamentos


                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal2;

                using (SqlConnection connection8 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection8);
                    command8.Connection.Open();

                    TabLocal2 = command8.ExecuteReader();

                    if (TabLocal2.HasRows == false)
                    {
                        TolMedi = 0;
                    }
                    else
                    {
                        while (TabLocal2.Read())
                        {
                            NF = TabLocal2["NumFactur"].ToString();
                            TolMedi = Convert.ToDouble(TabLocal2["ValorTotal"]);


                            Utils.SqlDatos = "UPDATE  [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] SET VaLorDeta = (VaLorDeta + " + TolMedi + ") WHERE NumRemi = '" + CI + "' and NumFactur = '" + NF + "' ";

                            Boolean EstaAct = Conexion.SQLUpdate(Utils.SqlDatos);

                        }
                    }
                }


                //Proceda a sumar otros servicios


                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal3;

                using (SqlConnection connection8 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection8);
                    command8.Connection.Open();

                    TabLocal3 = command8.ExecuteReader();

                    if (TabLocal3.HasRows == false)
                    {
                        TolOtros = 0;
                    }
                    else
                    {
                        while (TabLocal3.Read())
                        {
                            NF = TabLocal3["NumFactur"].ToString();
                            TolOtros = Convert.ToDouble(TabLocal3["ValorTotal"]);


                            Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] SET VaLorDeta = (VaLorDeta + " + TolOtros + ") WHERE NumRemi = '" + CI + "' and NumFactur = '" + NF + "' ";

                            Boolean EstaAct = Conexion.SQLUpdate(Utils.SqlDatos);

                        }
                    }
                }

                //Proceda a sumar los procedimientos


                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal4;

                using (SqlConnection connection8 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection8);
                    command8.Connection.Open();

                    TabLocal4 = command8.ExecuteReader();

                    if (TabLocal4.HasRows == false)
                    {
                        TolProce = 0;
                    }
                    else
                    {
                        while (TabLocal4.Read())
                        {
                            NF = TabLocal4["NumFactur"].ToString();
                            TolProce = Convert.ToDouble(TabLocal4["ValorProce"]);


                            Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS]  SET VaLorDeta =  (VaLorDeta + " + TolProce + ") WHERE NumRemi = '" + CI + "' and NumFactur = '" + NF + "' ";

                            Boolean EstaAct = Conexion.SQLUpdate(Utils.SqlDatos);

                        }
                    }
                }


                return 1;


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion AuditaDetaFacturas " + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int ValidarUsuarios(string c, string TU, double T, string CodDg)
        {
            try
            {
                //'Permite validar los datos de los usuarios seleccionados de una entidad para los RIPS
                string SqlUsuaTemp, TD, ND, ObErr, Dp, MuCi, Z;
                int VR, VDev, RegExp;


                SqlUsuaTemp = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] " +
                "WHERE (([Datos temporal usuarios RIPS].CodDigita) = '" + CodDg + "') and " +
                "(([Datos temporal usuarios RIPS].NumRemi) = '" + c + "');";


                SqlDataReader TabLocal;

                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command2 = new SqlCommand(SqlUsuaTemp, connection2);
                    command2.Connection.Open();
                    TabLocal = command2.ExecuteReader();

                    //  SqlDataReader TabLocal = Conexion.SQLDataReader(SqlUsuaTemp);

                    if (TabLocal.HasRows == false)
                    {
                        //  'No hay usuarios para validar de esta esta entidad
                        return 0;
                    }
                    else
                    {
                        VR = 0;
                        while (TabLocal.Read())
                        {
                            RegExp = 0;
                            ObErr = "";
                            TD = TabLocal["TipoDocum"].ToString();
                            ND = TabLocal["NumDocum"].ToString();

                            //Validamos el tipo de documento

                            Utils.SqlDatos = "SELECT * FROM [Datos documentos usuarios] ";
                            Utils.SqlDatos = Utils.SqlDatos + "WHERE (([Datos documentos usuarios].CodIdenti) = '" + TD + "'); ";

                            SqlDataReader TablaAux4;

                            using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                command.Connection.Open();
                                TablaAux4 = command.ExecuteReader();

                                if (TablaAux4.HasRows == false)
                                {
                                    RegExp = 1;
                                    ObErr = "El tipo de documento " + TD + " no es valido";
                                }

                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            }

                            //  SqlDataReader TablaAux4 = Conexion.SQLDataReader(Utils.SqlDatos);


                            //Validamos el tipo de usuario de la entidad

                            //Esta no se hace en este formulario

                            //if (TabLocal["TipUsuario"].ToString() != TU)
                            //{
                            //    RegExp = 1;
                            //    ObErr += " El tipo de usuario o regimen " + TabLocal["TipUsuario"].ToString() + " no corresponde a la entidad";
                            //}

                            //Validamos la unidad de la edad.

                            switch (Convert.ToInt32(TabLocal["EdadMedi"].ToString()))
                            {
                                case 1:
                                    if (Convert.ToInt32(TabLocal["Edad"].ToString()) < 0 || Convert.ToInt32(TabLocal["Edad"].ToString()) > 150)
                                    {
                                        RegExp = 1;
                                        ObErr += "Lo siento pero el valor de la edad no es valido en años.";
                                    }
                                    break;
                                case 2:
                                    if (Convert.ToInt32(TabLocal["Edad"].ToString()) < 0 || Convert.ToInt32(TabLocal["Edad"].ToString()) > 11)
                                    {
                                        RegExp = 1;
                                        ObErr += "Lo siento pero el valor de la edad no es valido en meses.";
                                    }
                                    break;
                                case 3:
                                    if (Convert.ToInt32(TabLocal["Edad"].ToString()) < 0 || Convert.ToInt32(TabLocal["Edad"].ToString()) > 31)
                                    {
                                        RegExp = 1;
                                        ObErr += "Lo siento pero el valor de la edad no es valido en días.";
                                    }
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr += "Lo siento pero la unidad de la edad " + TabLocal["EdadMedi"].ToString() + ", no es valida.";
                                    break;
                            }

                            //Validamos el sexo

                            switch (TabLocal["Sexo"].ToString())
                            {
                                case "M":
                                    break;
                                case "F":
                                    break;
                                case "I":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr += "Lo siento pero el identificador del sexo " + TabLocal["Sexo"].ToString() + ", no es valido.";
                                    break;
                            }

                            //Validamos el codigo del DPTO

                            Dp = TabLocal["CodDpto"].ToString();
 
                            Z = TabLocal["ZonaResi"].ToString();

                            Utils.SqlDatos = "SELECT * FROM [GEOGRAXPSQL].[dbo].[Datos de los Dpto o Estados] ";
                            Utils.SqlDatos += "WHERE CodigoDpto = '" + Dp + "' ";

                            SqlDataReader TablaAux9;

                            using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                command.Connection.Open();
                                TablaAux9 = command.ExecuteReader();

                                if (TablaAux9.HasRows == false)
                                {
                                    RegExp = 1;
                                    ObErr = "El Código del DPTO " + Dp + ", no es valido.";
                                }

                                TablaAux9.Close();

                            }



                            //Validamos el codigo del municipio

                            Utils.SqlDatos = "SELECT * FROM [GEOGRAXPSQL].[dbo].[Datos ciudades del dpto] ";
                            Utils.SqlDatos += "WHERE CodigoDpto = '" + Dp + "'";

                            SqlDataReader TablaAux11;

                            using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                command.Connection.Open();
                                TablaAux11 = command.ExecuteReader();


                                if (TablaAux11.HasRows == false)
                                {
                                    RegExp = 1;
                                    ObErr = "El Código del Municipio " + Dp + ", no es valido para el DPTO.";
                                }

                                TablaAux11.Close();

                                // SqlDataReader TablaAux11 = Conexion.SQLDataReader(Utils.SqlDatos);

                            }



                            //Validamos la Zona

                            switch (Z)
                            {
                                case "U":
                                    break;
                                case "R":
                                    break;
                                default:
                                    RegExp = 1;
                                    ObErr = "El identificador " + Z + ", de la zona de residencia no es valido.";
                                    break;
                            }

                            if (RegExp == 1)
                            {
                                Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] ";
                                Utils.SqlDatos += "([CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1]) ";
                                Utils.SqlDatos += "VALUES('" + lblCodigoUser.Text + "', 'US','" + TD + "','" + ND + "','" + c + "','0','" + ObErr + "')";

                                Boolean TabLocal2 = Conexion.SqlInsert(Utils.SqlDatos);
                            }

                            VR += 1;

                        }// final while

                        return VR;

                    }// final tabUsuarios

                } //Final Using

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ValidarUsuarios del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int ElimdatosRIPS(string UsSel, string ConMinRips)
        {
            try
            {

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                Boolean EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                return 1;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: ElimdatosRIPS del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private void DatosDeLaEmpresa()
        {
            try
            {
                txtDocuIps.Text = Utils.nitEmpresa;
                txtNombreIps.Text = Utils.nomEmpresa;
                txtTipoDocuIps.Text = Utils.tipoDocEmp;
                txtTeleIPS.Text = Utils.TelEmpresa;
                lblCodMinSalud.Text = Utils.codMinSalud;
                lblNivelPermitido.Text = Utils.nivelPermiso;
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al cargar la informacion de la empresa" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarDatosUser()
        {
            try
            {
                lblCodigoUser.Text = Utils.codUsuario;
                lblNombreUser.Text = Utils.nomUsuario;
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al cargar la informacion del usario" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarRangoFechas()
        {
            try
            {
                DateTime FechaActual = DateTime.Now;

                DateTime FechaUnMesAntes = DateTime.Now.AddMonths(-1);

                DateInicial.Value = FechaUnMesAntes;

                DateFinal.Value = FechaActual;
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al abrir funcion CargarRangoFechas" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CalcularTotalFactura()
        {
            try
            {
                int Contador = 0;
                int Contador2 = 0;

                foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                {

                    int Estado = Convert.ToInt32(Row.Cells["Estado"].Value);
                    if (Estado == 1)
                    {
                        Contador2 += 1;
                    }
                    Contador += 1;
                }

                TxtTotalFact.Text = (Contador).ToString();

                TxtMarcadas.Text = (Contador2).ToString();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al calcular el total de la grilla de facturas" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Botones
        private void btnReportes_Click(object sender, EventArgs e)
        {
            try
            {
                string Coenti02 = null, NomUsReal = null, Coenti01 = null, TDE = null, NCC = null, Para01 = null;
                string Mj = null, UsSel = null, CodRegEsp = null, NEenti = null;
                int NivPerReal;

                object CR;

                if (cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                    Utils.Informa += "usted no ha seleccionado el nombre de la entidad" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboNameEntidades.Select();
                    return;
                }

                Coenti01 = cboNameEntidades.SelectedValue.ToString();

                if (string.IsNullOrWhiteSpace(Coenti01))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                    Utils.Informa += "usted no ha seleccionado el nombre de la entidad" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboNameEntidades.Select();
                    return;
                }
                else
                {
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                          "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                          "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti01 + "' ORDER BY([NomAdmin] +' ' + [ProgrAmin])";
                    SqlDataReader sqlDataReader = Conexion.SQLDataReader(Utils.SqlDatos);
                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        Coenti02 = Coenti01;
                        NEenti = sqlDataReader["NP"].ToString();
                        TDE = sqlDataReader["TipoDocu"].ToString();
                        NCC = sqlDataReader["NumDocu"].ToString();
                        CodRegEsp = sqlDataReader["RegimenAdmin"].ToString();
                        Para01 = Coenti01;
                    }
                    sqlDataReader.Close();
                }

                if (string.IsNullOrWhiteSpace(txtRips.Text))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero el código de la Administradora" + "\r";
                    Utils.Informa += "de pagos en salud, no se encuentra definido para seleccionar los datos. " + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRips.Select();
                    return;
                }

                CR = txtRips.Text;

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "no es valido para seleccionar datos. " + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblCodigoUser.Select();
                    return;
                }


                UsSel = lblCodigoUser.Text;
                NomUsReal = lblNombreUser.Text;
                NivPerReal = Convert.ToInt32(lblNivelPermitido.Text);

                Utils.CarAdmin = Coenti01;
                Utils.CodRips = CR.ToString();
                Utils.NomTerc = NEenti;

                //'Abra el formulario de reportes de RIPS

                FrmReporteRipsRegimen frmReporteRipsRegimen = new FrmReporteRipsRegimen();

                frmReporteRipsRegimen.ShowDialog();

                //'Como los datos temporales de RIPS ahara se registran en la tabla RIPS, procedemos a contar desde la misma

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón reportes" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.Titulo01 = "Control para seleccionar datos";
                Boolean SqlInsert = true;
                string Coenti01, TDE, NCC, Regimen = null, NEnti = null, FunCon = null, llamarfuncion = null, UsGra = null, MT = null, SqlDatos = null, CR = null, CinRips = null, NRemEnvi = null;
                int FunEli, FunCopUs = 0, FunCopFac = 0, FunCopCon = 0, FunCopHos = 0, FunCopMed = 0, FunCopObs = 0, FunCopOtr = 0, FunCopRec = 0, FunCopPro = 0, FunElim = 0;
                string data, MJ;
                double TolUsa = 0, TolFac = 0, TolConsul = 0, TolHos = 0, TolMedi = 0, TolObs = 0, TolOtros = 0, TolReN = 0, TolProce = 0;
                SqlDataReader ContarRips;
                string Date = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime Fecha1 = DateInicial.Value;
                DateTime Fecha2 = DateFinal.Value;
                string Periodo1 = Fecha1.ToString("yyyy-MM-dd");
                string Periodo2 = Fecha2.ToString("yyyy-MM-dd");

                Utils.Titulo01 = "Control para exportar RIPS";

                if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre de la entidad.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboNameEntidades.Select();
                    return;
                }
                else
                {
                    Coenti01 = txtCardinal.Text;
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                            "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                            "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti01 + "'";

                    SqlDataReader sqlDataReader2 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader2.HasRows)
                    {
                        sqlDataReader2.Read();
                        NEnti = sqlDataReader2["NP"].ToString();
                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();
                        Regimen = sqlDataReader2["RegimenAdmin"].ToString();
                        CR = txtRips.Text;
                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                } //'Final de IsNull(Coenti01) Or (Coenti01 = " ")

                //'Revisa si el código de la entidad está relacionada con alguna entidad

                CinRips = CodInterAdminEs(CR);

                switch (CinRips)
                {
                    case "-1": // error en la función
                        return;
                        break;
                    case "0": // NO existe la administradora
                        Utils.Informa = "Lo siento pero el código SGSSS " + CR + " no" + "\r";
                        Utils.Informa += "pertenece a ninguna administradora de planes" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        break;
                    case "1": //'No existe la ruta
                        Utils.Informa = "Lo siento pero los archivos de SEDAS-RIPS" + "\r";
                        Utils.Informa += "no se han encontrado en la ruta de datos" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        break;
                    default:
                        //todo bien
                        break;
                }

                //'Proceda a contar cuantos usuarios hay para exportar, y cuantas facturas


                //Usuarios

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolUsuarios FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] WHERE NumRemi = '" + Coenti01 + "'";
                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolUsa = Convert.ToDouble(ContarRips["TolUsuarios"]);

                    if (TolUsa <= 0)
                    {
                        Utils.Informa = "Lo siento pero no existen facturas para" + "\r";
                        Utils.Informa += "realizar el RIPS a la entidad o convenio" + "\r";
                        Utils.Informa += NEnti;
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                //transacciones 

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolFac FROM [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();

                    TolFac = Convert.ToDouble(ContarRips["TolFac"]);
                    if (TolFac <= 0)
                    {
                        Utils.Informa = "Lo siento pero no existen usuarios para" + "\r";
                        Utils.Informa += "realizar el RIPS a la entidad o convenio" + "\r";
                        Utils.Informa += NEnti;
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();



                Utils.Informa = "¿Usted desea realizar la exportación de los" + "\r";
                Utils.Informa += "archivos RIPS de la entidad o convenio" + "\r";
                Utils.Informa += NEnti;
                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (res == DialogResult.Yes)
                {
                    //Proceda a revisar si la entidad ya tiene un archivo maestro abierto en SEDAS-RIPS

                    UsGra = lblCodigoUser.Text;


                    Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos archivo maestro] WHERE CodInterAdmi = '" + CinRips + "' and CerraRemi = 0";

                    SqlDataReader ArchivoMaestro = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (ArchivoMaestro.HasRows == false)
                    {
                        //'NO existe un maestro abierto para la entidad seleccionado
                        Utils.Informa = "¿Acepta que el sistema cree automaticamente" + "\r";
                        Utils.Informa += "el archivo maestro de la remisión de envío?" + "\r";
                        res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (res == DialogResult.Yes)
                        {
                            //Proceda a crear el maestro


                            FunCon = ConseRemisiones(true, UsGra);

                            switch (FunCon)
                            {
                                case "-3":
                                    Utils.Informa = "Lo siento pero el número consecutivo de" + "\r";
                                    Utils.Informa += "remisiones de envío llegó a 999.999" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                    break;
                                case "-2":
                                    Utils.Informa = "Lo siento pero la fecha del sistema es" + "\r";
                                    Utils.Informa += "menor a la de la última remisión generada" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                    break;
                                case "-1":

                                    return;
                                    break;
                                case "0":
                                    Utils.Informa = "Error de administración de datos." + "\r";
                                    Utils.Informa += "El registro único de contadores no fué posible encontrarlo." + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                    break;
                                default: //TODO BIEN

                                    //TERMINA

                                    data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo maestro] " +
                                     "(ConseArchivo," +
                                     "CodInterAdmi," +
                                     "CodIps," +
                                     "CodAdmin," +
                                     "FecRemite," +
                                     "NomRespon," +
                                     "Periodo1," +
                                     "Periodo2," +
                                     "NumFacturas," +
                                     "TelResponsa," +
                                     "CodRegEsp," +
                                     "CerraRemi," +
                                     "AnulRemi," +
                                     "ActualRemi," +
                                     "CodiRegis," +
                                     "FecRegis)" +
                                     "VALUES(" +
                                     "'" + FunCon + "'," +
                                     "'" + CinRips + "'," +
                                     "'" + lblCodMinSalud.Text + "'," +
                                     "'" + CR + "'," +
                                     "'" + Date + "'," +
                                     "'" + lblNombreUser.Text + "'," +
                                     "'" + Periodo1 + "'," +
                                     "'" + Periodo2 + "'," +
                                     "'" + TolFac + "'," +
                                     "'" + txtTeleIPS.Text + "'," +
                                     "'" + Regimen + "'," +
                                     "'" + 0 + "'," +
                                     "'" + 0 + "'," +
                                     "'" + 0 + "'," +
                                     "'" + UsGra + "'," +
                                     "'" + Date + "')";

                                    SqlInsert = Conexion.SqlInsert(data);

                                    //Comience el proceso de copiado de archivos
                                    NRemEnvi = FunCon;

                                    break;
                            }//Fin swich


                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        ArchivoMaestro.Read();
                        NRemEnvi = ArchivoMaestro["ConseArchivo"].ToString();

                        Utils.Informa = "El sistema ha encontrado abierta la remisión" + "\r";
                        Utils.Informa += "Número " + NRemEnvi + " del codigo SGSS " + CR;
                        res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (res == DialogResult.No)
                        {
                            return;
                        }

                    }
                    ArchivoMaestro.Close();

                    //'Inicia el proceso de copiados


                    FunCopUs = CopiaRipsUsa(NRemEnvi, Coenti01, TolUsa, 2);


                    switch (FunCopUs)
                    {
                        case -1:
                            return; //Error en la funcion
                            break;
                        case -2:
                            return; //No exiten datos para copiar
                            break;
                        default: //Siga a copiar las transacciones

                            if (TolUsa > FunCopUs)
                            {
                                Utils.Titulo01 = "Control de ejecución";
                                Utils.Informa = "Lo siento, pero de " + TolUsa + " usuarios" + "\r";
                                Utils.Informa += "a exportar, solo se copiaron " + FunCopUs + "\r";
                                Utils.Informa += "¿Quiere saber cuales son esos usuarios?" + "\r";
                                res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (res == DialogResult.Yes)
                                {
                                    //Muestre el informe 

                                    string ReporUser = "SELECT [Datos temporal usuarios RIPS].CodDigita, [Datos temporal usuarios RIPS].NumRemi, [Datos temporal usuarios RIPS].CodAdmin, [Datos temporal usuarios RIPS].TipoDocum, [Datos temporal usuarios RIPS].NumDocum, [Datos temporal usuarios RIPS].TipUsuario, [Datos temporal usuarios RIPS].Apellido1, [Datos temporal usuarios RIPS].Apellido2, [Datos temporal usuarios RIPS].Nombre1, [Datos temporal usuarios RIPS].Nombre2, [Datos temporal usuarios RIPS].Edad, [Datos temporal usuarios RIPS].EdadMedi, [Datos temporal usuarios RIPS].Sexo, [Datos temporal usuarios RIPS].CodDpto, [Datos temporal usuarios RIPS].CodMuni, [Datos temporal usuarios RIPS].ZonaResi, Trim([Datos empresas y terceros].[NomAdmin] + ' ' + [Datos empresas y terceros].[ProgrAmin]) AS NoAdmin, [Datos empresas y terceros].NomPlan " +
                                                " FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] INNER JOIN [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] ON [Datos empresas y terceros].CarAdmin = [Datos temporal usuarios RIPS].NumRemi " +
                                                " WHERE [Datos empresas y terceros].[CodDigita] = '" + UsGra + "' AND [Datos empresas y terceros].[NumRemi] = '" + Coenti01 + "' AND [Datos empresas y terceros].[Exportado] = 0  " +
                                                " ORDER BY [Datos temporal usuarios RIPS].TipoDocum, [Datos temporal usuarios RIPS].NumDocum; ";

                                    Utils.SqlDatos = ReporUser;

                                    Utils.infNombreInforme = "InfReporUserPorRemision";

                                    Reportes.FrmInfUsuariosRemi frm = new Reportes.FrmInfUsuariosRemi();

                                    frm.ShowDialog();

                                }

                            }
                            break;
                    }// Fin switch


                    FunCopFac = CopiaRipsTrans(NRemEnvi, Coenti01, TolFac, 2);

                    switch (FunCopFac)
                    {
                        case -1: //error en la funcion
                            return;
                            break;
                        case -2: //NO existe nada para copiar
                            return;
                            break;
                        default:
                            //Cuente cada uno de los archivos

                            //Consultas

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolConsul FROM [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolConsul = Convert.ToDouble(ContarRips["TolConsul"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            //Hospitalizados

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolHos FROM [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolHos = Convert.ToDouble(ContarRips["TolHos"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            //Medicamentos

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolMedi FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolMedi = Convert.ToDouble(ContarRips["TolMedi"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            //Observaciones

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolObs FROM [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolObs = Convert.ToDouble(ContarRips["TolObs"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            //OtroServicios

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolOtros FROM [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolOtros = Convert.ToDouble(ContarRips["TolOtros"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            //Recien Nacidos

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolReN FROM [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolReN = Convert.ToDouble(ContarRips["TolReN"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                            //Procedimientos

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolProce FROM [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                            ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (ContarRips.HasRows)
                            {
                                ContarRips.Read();
                                TolProce = Convert.ToDouble(ContarRips["TolProce"]);
                            }
                            ContarRips.Close();
                            ContarRips = null;
                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
                            break;
                    }//Fin Swich

                    MJ = "";

                    if(TolConsul > 0)
                    {
                        //Copia las consultas
                        FunCopCon = CopiaRipsConsul(NRemEnvi, Coenti01, TolConsul);
                        MJ = "Cantidad de consultas: " + FunCopCon + "\r";
                    }

                    if (TolHos > 0)
                    {
                        //Copia los usuarios hospitalizados
                        FunCopHos = CopiaRipsHospi(NRemEnvi, Coenti01, TolHos);
                        MJ += "Cantidad de hospitalizaciones: " + FunCopHos + "\r";
                    }

                    if (TolMedi > 0)
                    {
                        //Copia los medicamentos
                        FunCopMed = CopiaRipsMedica(NRemEnvi, Coenti01, TolMedi);
                        MJ += "Cantidad de medicamentos: " + FunCopMed + "\r";
                    }

                    if (TolObs > 0)
                    {
                        //Copia los usuarios en observación
                        FunCopObs = CopiaRipsObserva(NRemEnvi, Coenti01, TolObs);
                        MJ += "Cantidad de observaciónes: " + FunCopObs + "\r";
                    }

                    if (TolOtros > 0)
                    {
                        //Copia los otros servicios
                        FunCopOtr = CopiaRipsOtros(NRemEnvi, Coenti01, TolOtros);
                        MJ += "Cantidad de otros servicios: " + FunCopOtr + "\r";
                    }

                    if (TolReN > 0)
                    {
                        //Copia los recien nacidos
                        FunCopRec = CopiaRipsRecien(NRemEnvi, Coenti01, TolReN);
                        MJ += "Cantidad de otros servicios: " + FunCopRec + "\r";
                    }

                    if (TolProce > 0)
                    {
                        //Copia los procedimientos
                        FunCopPro = CopiaRipsProce(NRemEnvi, Coenti01, TolProce);
                        MJ += "Cantidad de procedimientos: " + FunCopPro + "\r";
                    }


                    FunElim = ElimdatosRIPS(lblCodigoUser.Text, Coenti01);

                    //'Resumen de los exportado

                    Utils.Titulo01 = "Control de ejecución";
                    Utils.Informa = "Se han exportado los siguientes datos:" + "\r";
                    Utils.Informa += "Cantidad de usuarios: " + FunCopUs + "\r";
                    Utils.Informa += "Cantidad de facturas: " + FunCopFac + "\r";
                    Utils.Informa += MJ + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }// Fin msgox si
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón exportar" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private int CopiaRipsProce(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay medicamentos para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona los otros servicios

                        string DxPrincipal;

                        while (TabLocal.Read())
                        {

                             DxPrincipal = Convert.ToString(TabLocal["DxPrincipal"]) != "0000" ? TabLocal["DxPrincipal"].ToString() : "";

                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de procedimientos] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "FecProce," +
                                             "AutoriNum," +
                                             "CodProce," +
                                             "AmbitoReal," +
                                             "FinalProce," +
                                             "PersonAten," +
                                             "DxPrincipal," +
                                             "DxRelacion," +
                                             "Complicacion," +
                                             "RealiActo," +
                                             "ValorProce)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecProce"]).ToString("yyyy-MM-dd") + "'," +
                                             "'" + TabLocal["AutoriNum"].ToString() + "'," +
                                             "'" + TabLocal["CodProce"].ToString() + "'," +
                                             "'" + TabLocal["AmbitoReal"].ToString() + "'," +
                                             "'" + TabLocal["FinalProce"].ToString() + "'," +
                                             "'" + TabLocal["PersonAten"].ToString() + "'," +
                                             "'" + DxPrincipal + "'," +
                                             "'" + TabLocal["DxRelacion"].ToString() + "'," +
                                             "'" + TabLocal["Complicacion"].ToString() + "'," +
                                             "'" + TabLocal["RealiActo"].ToString() + "'," +
                                             "'" + TabLocal["ValorProce"].ToString() + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();

                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsProce" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private int CopiaRipsRecien(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay medicamentos para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona los otros servicios

                        while (TabLocal.Read())
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de recien nacido] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "FecNaci," +
                                             "HorIngresa," +
                                             "EdadGesta," +
                                             "ControlPrena," +
                                             "SexoRecien," +
                                             "PesoRecien," +
                                             "DxRecien)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecNaci"]).ToString("yyyy-MM-dd") + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["HorIngresa"]).ToString("hh:mm:ss") + "'," +
                                             "'" + TabLocal["EdadGesta"].ToString() + "'," +
                                             "'" + TabLocal["ControlPrena"].ToString() + "'," +
                                             "'" + TabLocal["SexoRecien"].ToString() + "'," +
                                             "'" + TabLocal["PesoRecien"].ToString() + "'," +
                                             "'" + TabLocal["DxRecien"].ToString() + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();

                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsRecien" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private int CopiaRipsOtros(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay medicamentos para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona los otros servicios

                        while (TabLocal.Read())
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de otros servicios] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "AutoriNum," +
                                             "TipoServicio," +
                                             "CodiServi," +
                                             "NomServi," +
                                             "Cantidad," +
                                             "ValorUnita," +
                                             "ValorTotal)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + TabLocal["AutoriNum"].ToString() + "'," +
                                             "'" + TabLocal["TipoServicio"].ToString() + "'," +
                                             "'" + TabLocal["CodiServi"].ToString() + "'," +
                                             "'" + TabLocal["NomServi"].ToString() + "'," +
                                             "'" + TabLocal["Cantidad"].ToString() + "'," +
                                             "'" + TabLocal["ValorUnita"].ToString() + "'," +
                                             "'" + TabLocal["ValorTotal"].ToString() + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();

                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsOtros" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private int CopiaRipsObserva(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay medicamentos para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona las observacion

                        while (TabLocal.Read())
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de observacion urgencias] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "FecIngresa," +
                                             "HorIngresa," +
                                             "AutoriNum," +
                                             "CausExter," +
                                             "DxPrincIngre," +
                                             "DxRelacion1," +
                                             "DxRelacion2," +
                                             "DxRelacion3," +
                                             "Destino," +
                                             "EstadoSal," +
                                             "DxMuerte," +
                                             "FecSalida," +
                                             "HorSalida)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecIngresa"]).ToString("yyyy-MM-dd") + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["HorIngresa"]).ToString("hh:mm:ss") + "'," +
                                             "'" + TabLocal["AutoriNum"].ToString() + "'," +
                                             "'" + TabLocal["CausExter"].ToString() + "'," +
                                             "'" + TabLocal["DxPrincIngre"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion1"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion2"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion3"].ToString() + "'," +
                                             "'" + TabLocal["Destino"].ToString() + "'," +
                                             "'" + TabLocal["EstadoSal"].ToString() + "'," +
                                             "'" + TabLocal["DxMuerte"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["HorSalida"]).ToString("hh:mm:ss") + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();

                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsObserva" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private int CopiaRipsMedica(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay medicamentos para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona las consultas

                        while (TabLocal.Read())
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de medicamentos] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "AutoriNum," +
                                             "CodMedica," +
                                             "TipoMedica," +
                                             "NomGenerico," +
                                             "FormaFarma," +
                                             "ConcenMedi," +
                                             "UniMedida," +
                                             "NumUnidad," +
                                             "ValorUnita," +
                                             "ValorTotal)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + TabLocal["AutoriNum"].ToString() + "'," +
                                             "'" + TabLocal["CodMedica"].ToString() + "'," +
                                             "'" + TabLocal["TipoMedica"].ToString() + "'," +
                                             "'" + TabLocal["NomGenerico"].ToString() + "'," +
                                             "'" + TabLocal["FormaFarma"].ToString() + "'," +
                                             "'" + TabLocal["ConcenMedi"].ToString() + "'," +
                                             "'" + TabLocal["UniMedida"].ToString() + "'," +
                                             "'" + TabLocal["NumUnidad"].ToString() + "'," +
                                             "'" + TabLocal["ValorUnita"].ToString() + "'," +
                                             "'" + TabLocal["ValorTotal"].ToString() + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();

                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsMedica" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private int CopiaRipsHospi(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if (TabLocal.HasRows == false)
                    {
                        //No hay consultas para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona las consultas

                        while (TabLocal.Read())
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de hospitalizacion] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "ViaDIngreso," +
                                             "FecIngresa," +
                                             "HorIngresa," +
                                             "AutoriNum," +
                                             "CausExter," +
                                             "DxPrincIngre," +
                                             "DxPrincEgre," +
                                             "DxRelacion1," +
                                             "DxRelacion2," +
                                             "DxRelacion3," +
                                             "DxComplica," +
                                             "EstadoSal," +
                                             "DxMuerte," +
                                             "FecSalida," +
                                             "HorSalida)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + TabLocal["ViaDIngreso"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecIngresa"]).ToString("yyyy-MM-dd") + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["HorIngresa"]).ToString("hh:mm:ss") + "'," +
                                             "'" + TabLocal["AutoriNum"].ToString() + "'," +
                                             "'" + TabLocal["CausExter"].ToString() + "'," +

                                             "'" + TabLocal["DxPrincIngre"].ToString() + "'," +
                                             "'" + TabLocal["DxPrincEgre"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion1"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion2"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion3"].ToString() + "'," +
                                             "'" + TabLocal["DxComplica"].ToString() + "'," +
                                             "'" + TabLocal["EstadoSal"].ToString() + "'," +
                                             "'" + TabLocal["DxMuerte"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["HorSalida"]).ToString("hh:mm:ss") + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();

                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsHospi" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private int CopiaRipsConsul(string NR, string CI, double TolC)
        {
            try
            {
                int VR = 0;
                bool SqlInsert;
                //Permite copiar las consultas para RIPS a SEDAS-RIPS

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] WHERE NumRemi = '" + CI + "'";

                SqlDataReader TabLocal;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                    command.Connection.Open();
                    TabLocal = command.ExecuteReader();

                    if(TabLocal.HasRows == false)
                    {
                        //No hay consultas para copiar a esta entidad
                        return -2;
                    }
                    else
                    {
                        VR = 0;

                        //Simplemente adiciona las consultas

                        while (TabLocal.Read())
                        {
                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de consulta] " +
                                             "(NumRemi," +
                                             "NumFactur," +
                                             "CodIps," +
                                             "TipoDocum," +
                                             "NumDocum," +
                                             "FecConsul," +
                                             "AutoriNum," +
                                             "CodConsul," +
                                             "FinalConsul," +
                                             "CausExter," +
                                             "DxPrincipal," +
                                             "DxRelacion1," +
                                             "DxRelacion2," +
                                             "DxRelacion3," +
                                             "TipoDxPrin," +
                                             "ValorConsul," +
                                             "ValorCuota," +
                                             "ValorNeto)" +
                                             "VALUES(" +
                                             "'" + NR + "'," +
                                             "'" + TabLocal["NumFactur"].ToString() + "'," +
                                             "'" + TabLocal["CodIps"].ToString() + "'," +
                                             "'" + TabLocal["TipoDocum"].ToString() + "'," +
                                             "'" + TabLocal["NumDocum"].ToString() + "'," +
                                             "'" + Convert.ToDateTime(TabLocal["FecConsul"]).ToString("yyy-MM-dd") + "'," +
                                             "'" + TabLocal["AutoriNum"].ToString() + "'," +
                                             "'" + TabLocal["CodConsul"].ToString() + "'," +
                                             "'" + TabLocal["FinalConsul"].ToString() + "'," +
                                             "'" + TabLocal["CausExter"].ToString() + "'," +
                                             "'" + TabLocal["DxPrincipal"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion1"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion2"].ToString() + "'," +
                                             "'" + TabLocal["DxRelacion3"].ToString() + "'," +
                                             "'" + TabLocal["TipoDxPrin"].ToString() + "'," +
                                             "'" + TabLocal["ValorConsul"].ToString() + "'," +
                                             "'" + TabLocal["ValorCuota"].ToString() + "'," +
                                             "'" + TabLocal["ValorNeto"].ToString() + "')";

                            SqlInsert = Conexion.SqlInsert(Utils.SqlDatos);

                            if (SqlInsert)
                            {
                                VR += 1;
                            }

                        }//Fin While

                    }//Fin TabLocal.HasRows == false

                    TabLocal.Close();
                   
                }//Fin Using

                return VR;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CopiaRipsConsul" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                //  *******************  Diciembre 02 de 2.003  **************************
                //  *******************  Junio 2021 Juan Diego   **************************

                //Permite validar los datos de los RIPS


                Utils.Titulo01 = "Control para validar datos";

                string Coenti01, UsSel = null, NEnti, TDE, NCC, CD, CR, Para02, Para01, TUReg = null, AcRe = null;
                double TolUsa = 0, TolConsul = 0, TolHos = 0, TolMedi = 0, TolObs = 0, TolOtros = 0, TolReN = 0, TolProce = 0, TolFac = 0, FunAudi = 0;

                int FunUs, FunFac, FunCon, FunHos, FunObs, FunMedi, FunOtros, FunReN, FunProce, TolInco = 0;

                SqlDataReader ContarRips;

                UsSel = lblCodigoUser.Text;

                if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre de la entidad.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboNameEntidades.Select();
                    return;
                }
                else
                {
                    Coenti01 = txtCardinal.Text;
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                            "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                            "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti01 + "'";

                    SqlDataReader sqlDataReader2 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader2.HasRows)
                    {
                        sqlDataReader2.Read();
                        NEnti = sqlDataReader2["NP"].ToString();
                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();
                        TUReg = sqlDataReader2["RegimenAdmin"].ToString();
                        AcRe = sqlDataReader2["ActiReali"].ToString();
                        CR = lblCodMinSalud.Text;
                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                } //'Final de IsNull(Coenti01) Or (Coenti01 = " ")

                //Usuarios

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolUsuarios FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] WHERE NumRemi = '" + Coenti01 + "'";
                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolUsa = Convert.ToDouble(ContarRips["TolUsuarios"]);

                }
                else
                {
                    Utils.Informa = "El proceso de validación de este módulo no se" + "\r";
                    Utils.Informa += "puede realizar mientras no se seleccione los" + "\r";
                    Utils.Informa += "pusuarios de la entidad seleccionada." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                //transacciones 

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolFac FROM [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolFac = Convert.ToDouble(ContarRips["TolFac"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                //Consultas

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolConsul FROM [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolConsul = Convert.ToDouble(ContarRips["TolConsul"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //Hospitalizados

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolHos FROM [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolHos = Convert.ToDouble(ContarRips["TolHos"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //Medicamentos

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolMedi FROM [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolMedi = Convert.ToDouble(ContarRips["TolMedi"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //Observaciones

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolObs FROM [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolObs = Convert.ToDouble(ContarRips["TolObs"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //OtroServicios

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolOtros FROM [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolOtros = Convert.ToDouble(ContarRips["TolOtros"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //Recien Nacidos

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolReN FROM [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolReN = Convert.ToDouble(ContarRips["TolReN"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //Procedimientos

                Utils.SqlDatos = "SELECT COUNT(NumRemi) AS TolProce FROM [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE NumRemi = '" + Coenti01 + "'";

                ContarRips = Conexion.SQLDataReader(Utils.SqlDatos);

                if (ContarRips.HasRows)
                {
                    ContarRips.Read();
                    TolProce = Convert.ToDouble(ContarRips["TolProce"]);
                }
                ContarRips.Close();
                ContarRips = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();





                //Elimine los datos de la tabla temporal


                Utils.SqlDatos = "DELETE FROM [DARIPSESSQL].[dbo].[Datos temporal errores RIPS]";


                Boolean EliDatos = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.Informa = "¿Usted desea validar los datos de los RIPS" + "\r";
                Utils.Informa += "previamente seleccionados a la entidad " + "\r";

                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    // 'Validamos el de usuarios

                    FunUs = ValidarUsuarios(Coenti01, TUReg, TolUsa, UsSel); //US


                    switch (FunUs)
                    {
                        case -1: //error en la funcion
                            return;
                            break;
                        case 0: // Casi imposible que entre aqui
                            Utils.Informa = "El proceso de validación de este módulo no se";
                            Utils.Informa += "puede realizar mientras no se seleccione los ";
                            Utils.Informa += "usuarios de la entidad seleccionada.";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                        default:
                            if (TolFac <= 0)
                            {
                                Utils.Informa = "El proceso de validación de este módulo no se";
                                Utils.Informa += "puede realizar mientras no se seleccione los ";
                                Utils.Informa += "facturas de los procedimientos realizados.";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            break;
                    }


                    FunAudi = AuditaDetaFacturas(Coenti01);

                    if (FunAudi == -1)
                    {
                        return;
                    }

                    //Empiece a validar las facturas

                    FunFac = ValidarFacturas(Coenti01, TolFac, UsSel); //AF

                    if (FunFac == -1)
                    {
                        return;
                    }



                    if (TolConsul > 0)
                    {
                        //Validar el de consultas
                        FunCon = ValidaConsultas(Coenti01, AcRe, TolConsul, UsSel);  //AC 
                    }


                    if (TolHos > 0)
                    {
                        //'Validar el de hospitalizaciones
                        FunHos = ValidarHospi(Coenti01, TolObs, UsSel); //AH
                    }

                    if (TolMedi > 0)
                    {
                        //'Validar el de medicamentos
                        FunMedi = ValidarMedica(Coenti01, TolMedi, UsSel); //AM
                    }

                    if (TolObs > 0)
                    {
                        //'Validar el de observación de urgencias
                        FunObs = ValidarObserva(Coenti01, TolObs, UsSel); //AU
                    }


                    if (TolOtros > 0)
                    {
                        //'Validar el de otros servicios
                        FunOtros = ValidarOtros(Coenti01, TolOtros, UsSel); //AT
                    }

                    if (TolReN > 0)
                    {
                        //'Validar el de recien nacidos
                        FunReN = ValidarReNan(Coenti01, TolReN, UsSel); //AN
                    }

                    if (TolProce > 0)
                    {
                        //'Validar el de procedimientos
                        FunProce = ValidarProcedi(Coenti01, TolObs, UsSel); //AP
                    }


                    Utils.SqlDatos = "SELECT COUNT(CodEnti) AS CuenCodEnti FROM [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] WHERE CodEnti = '" + Coenti01 + "' ";

                    SqlDataReader reader = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (reader.HasRows)
                    {
                        reader.Read();
                        TolInco = Convert.ToInt32(reader["CuenCodEnti"].ToString());
                    }

                    if (TolInco == 0)
                    {
                        Utils.Titulo01 = "Control de validacion";
                        Utils.Informa = "Los datos de los seleccionados han validado exitosamente.";
                        Utils.Informa = Utils.Informa + "Recuerde este no es el validador oficial de MinSalud.";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (TolInco > 0)
                        {

                            Utils.SqlDatos = "SELECT [CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1] FROM [DARIPSESSQL].[dbo].[Datos temporal errores RIPS] WHERE CodEnti = '" + Coenti01 + "' ORDER BY NumDocu ASC  ";

                            Utils.infNombreInforme = "InfReporErroresRips";

                            Utils.CarAdmin = Coenti01;

                            Reportes.FrmInfErroresRips frm = new Reportes.FrmInfErroresRips();
                            frm.ShowDialog();

                        }
                    }
                    reader.Close();


                }





            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de dar click en el boton validar " + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarcarTodas_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                {

                    Row.Cells["Estado"].Value = 1;

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues del dar click BtnMarcarTodas_Click" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDesmarcarTodas_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                {

                    Row.Cells["Estado"].Value = 0;

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues del dar click BtnDesmarcarTodas_Click" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {

                string NCF, Regi02, Coenti01 = null, TDE, UsSel = null, NCC, Fec01, Fec02, Para01, Para02, Para03, Para04, Para05, Para06, Msj, VDev, ElimDat, Para07, Para08, Regi01, SqlFacturas;
                int TM;
                int SinoM;
                int VarRetor;
                int FacCuen;
                long Tol;
                DateTime FecIni;
                DateTime FecFin;

                Utils.Titulo01 = "Control para mostrar documentos";


                if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre de la entidad.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNameEntidades.Select();
                    return;
                }
                else
                {
                    Coenti01 = cboNameEntidades.SelectedValue.ToString();
                }

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "po es valido para seleccionar datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Coenti01 == null || string.IsNullOrEmpty(Coenti01))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre de la entidad.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNameEntidades.Select();
                    return;
                }
                else
                {
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                            "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                            "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti01 + "'";

                    SqlDataReader sqlDataReader2 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader2.HasRows)
                    {
                        sqlDataReader2.Read();

                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();

                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                } //'Final de IsNull(Coenti01) Or (Coenti01 = " ")

                //'Revisamos que se haya selecionado el nombre del regimen

                if (string.IsNullOrWhiteSpace(cboRegNom.SelectedValue.ToString()) == true || cboRegNom.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre del regimen a mostrar los datos";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboNameEntidades.Select();
                    return;
                }

                Regi02 = cboRegNom.SelectedValue.ToString();
                Para02 = DateInicial.Value.ToString("yyyy-MM-dd");
                Para03 = DateFinal.Value.ToString("yyyy-MM-dd");

                if (txtNombreIps.Text.Length > 60)
                {
                    Para07 = txtNombreIps.Text.Substring(0, 60);
                }
                else
                {
                    Para07 = txtNombreIps.Text;
                }


                Utils.Informa = "¿Usted desea mostrar todas las facturas ";
                Utils.Informa = Utils.Informa + "realizadas al regimen " + cboRegNom.Text;
                Utils.Informa = Utils.Informa + " entre " + Para02 + " y el " + Para03 + "?";

                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {

                    TxtTotalFact.Clear();
                    TxtTotalFact.Clear();
                    DataGridFacturas.Rows.Clear();


                    //********************* el 23 de noviembre de 2020, Hernando modificò, incluyendo que las facturas capitas no se relacionen en la selecciòn *****************

                    SqlFacturas = "";


                    //Debo Insertar los datos que salen en usuarios

                    Utils.SqlDatos = "SELECT 1 as Estado, [Datos de las facturas realizadas].NumFactura, Format([Datos de las facturas realizadas].FechaFac,'dd-MM-yyyy') AS Fecha," +
                                    " [Datos cuentas de consumos].TipoUsuario, [Datos cuentas de consumos].ValorEdad, [Datos cuentas de consumos].UnidadEdad, [Datos cuentas de consumos].NumPoliza, " +
                                    " [Datos empresas y terceros].NomAdmin, [Datos tipos de usuarios].NomTipo, [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].NumCuenFac, [Datos cuentas de consumos].HistoNum, [Datos de las facturas realizadas].Cartercero, " +
                                    " [Datos de las facturas realizadas].NumContra, [Datos de las facturas realizadas].Copago  " +
                                    " FROM [Datos de las facturas realizadas] INNER JOIN " +
                                    " [Datos empresas y terceros] ON [Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                    " [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum INNER JOIN " +
                                    " [Datos tipos de usuarios] ON[Datos cuentas de consumos].TipoUsuario = [Datos tipos de usuarios].CodTipoUsuar " +
                                    " WHERE [Datos cuentas de consumos].TipoUsuario = '" + Regi02 + "' " +
                                    " AND [Datos de las facturas realizadas].FechaFac >= CONVERT(Datetime, N'" + Para02 + "', 102) " +
                                    " AND [Datos de las facturas realizadas].FechaFac <= CONVERT(Datetime, N'" + Para03 + "', 102) " +
                                    " AND [Datos cuentas de consumos].CuenActiva = 0 " +
                                    " AND [Datos cuentas de consumos].CuenAnulada = 0 " +
                                    " AND [Datos cuentas de consumos].DefiCuenta <> N'0' " +
                                    " AND [Datos de las facturas realizadas].AnuladaFac = 0 " +
                                    " ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura;";


                    SqlDataReader sqlDataReader = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            DataGridFacturas.Rows.Add(sqlDataReader["Estado"], sqlDataReader["NumCuenFac"], sqlDataReader["NumPoliza"], sqlDataReader["Cartercero"], sqlDataReader["NumContra"], sqlDataReader["HistoNum"], sqlDataReader["TipoUsuario"], sqlDataReader["ValorEdad"], sqlDataReader["UnidadEdad"], sqlDataReader["NumFactura"].ToString(), sqlDataReader["Fecha"].ToString(), sqlDataReader["NomAdmin"].ToString(), sqlDataReader["NomTipo"].ToString(), sqlDataReader["ValorFac"].ToString(), sqlDataReader["Copago"].ToString());
                        }
                    }

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                    CalcularTotalFactura();

                    if (Convert.ToInt32(TxtTotalFact.Text) <= 0)
                    {
                        Utils.Titulo01 = "Control de errores de ejecución";
                        Utils.Informa = "Lo siento pero en el rango de fechas" + "\r";
                        Utils.Informa += "seleccionado no se encuentran facturas" + "\r";
                        Utils.Informa += "realizadas al regimen " + cboNameEntidades.Text + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Utils.Titulo01 = "Control para mostrar documentos";
                        Utils.Informa = "Se han seleccionado " + TxtTotalFact.Text + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón mostrar" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //FIN  btnMostrar_Clic

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Utils.Titulo01 = "Control de ejecución";
            Utils.Informa = "¿Usted  desea quitar todos los" + "\r";
            Utils.Informa += "datos previamente seleccionado?" + "\r";
            var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                DataGridFacturas.Rows.Clear();
                CalcularTotalFactura();
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {
                BarraSeleccionar.Minimum = 1;
                BarraSeleccionar.Maximum = Convert.ToInt32(TxtMarcadas.Text);
                Utils.Titulo01 = "Control para seleccionar datos";
                Boolean SqlInsert = true;
                string Coenti01, TDE, NCC, NEnti = null, MT = null, SqlDatos = null;
                int FunEli;
                string data;
                if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre de la entidad.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboNameEntidades.Select();
                    return;
                }
                else
                {
                    Coenti01 = txtCardinal.Text;
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                            "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                            "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti01 + "'";

                    SqlDataReader sqlDataReader2 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader2.HasRows)
                    {
                        sqlDataReader2.Read();
                        NEnti = sqlDataReader2["NP"].ToString();
                        MT = sqlDataReader2["ManualTari"].ToString();
                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();

                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                } //'Final de IsNull(Coenti01) Or (Coenti01 = " ")


                if (string.IsNullOrWhiteSpace(txtRips.Text))
                {
                    Utils.Informa = "Lo siento pero la entidad seleccionada no tiene" + "\r";
                    Utils.Informa += "definido el código RIPS para reportar los registros." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRips.Select();
                    return;
                }

                if (Convert.ToInt32(TxtMarcadas.Text) <= 0)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha ejecutado el " + "\r";
                    Utils.Informa += "proceso mostrar facturas, para generar RIPS." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFacturaDestino.Select();
                    return;
                }



                Utils.Informa = "¿Usted desea seleccionar los datos necesarios";
                Utils.Informa = Utils.Informa + "para realizar los RIPS de la entidad ";
                Utils.Informa = Utils.Informa + NEnti + ".?";
                Utils.Informa = Utils.Informa + " Son: " + TxtMarcadas.Text + " " + "Facturas para rips de " + TxtTotalFact.Text;

                var re = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (re == DialogResult.Yes)
                {
                    BarraSeleccionar.Minimum = 1;
                    BarraSeleccionar.Maximum = Convert.ToInt32(TxtMarcadas.Text);

                    string UsSel = lblCodigoUser.Text;

                    FunEli = ElimdatosRIPS(UsSel, Coenti01);

                    if (FunEli == -1)
                    {
                        return;
                    }

                    // Corra el proceso de selección de datos por facturas

                    //Seleccionamos los usuarios

                    string CodRips = txtRips.Text;
                    string CodIPS = lblCodMinSalud.Text;

                    string FechaInicial = DateInicial.Value.ToString("yyyy-MM-dd");
                    string FechaFinal = DateFinal.Value.ToString("yyyy-MM-dd");

                    string RazonSocial = txtNombreIps.Text;

                    if (string.IsNullOrWhiteSpace(RazonSocial) == false && RazonSocial.Length > 60)
                    {
                        RazonSocial = RazonSocial.Substring(0, 60);
                    }


                    string TipoDocuIPS = txtTipoDocuIps.Text;

                    if (TipoDocuIPS.Length > 2)
                    {
                        TipoDocuIPS = TipoDocuIPS.Substring(0, 2);
                    }


                    foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                    {
    

                        int Estado = Convert.ToInt32(Row.Cells["Estado"].Value);

                        if (Estado == 1)
                        {
                            string HistoNum = Convert.ToString(Row.Cells["HistoNum"].Value);

                            string TipoUsuario = Convert.ToString(Row.Cells["TipoUsuario"].Value);

                            string ValorEdad = Convert.ToString(Row.Cells["ValorEdad"].Value);

                            string UnidadEdad = Convert.ToString(Row.Cells["UnidadEdad"].Value);

                            string NumFactur = Convert.ToString(Row.Cells["NumFactura"].Value);

                            DateTime FecFactura = Convert.ToDateTime(Row.Cells["Fecha"].Value);

                            string FecFactur = FecFactura.ToString("yyyy-MM-dd");

                            string Cartercero = Convert.ToString(Row.Cells["Cartercero"].Value);

                            string NumContra = Convert.ToString(Row.Cells["NumContra"].Value);

                            if (string.IsNullOrWhiteSpace(NumContra) == false && NumContra.Length > 15)
                            {
                                NumContra = NumContra.Substring(0, 15);
                            }


                            string NumPoliza = Convert.ToString(Row.Cells["NumPoliza"].Value);

                            string Copago = Convert.ToString(Row.Cells["Copago"].Value);

                            string ValorFac = Convert.ToString(Row.Cells["ValorFac"].Value);

                            string NumCuenFac = Convert.ToString(Row.Cells["NumCuenFac"].Value);



                            //USUARIOS ------------------------------------------------------------------------------------

                            SqlDatos = "SELECT [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos del Paciente].Apellido1, [Datos del Paciente].Apellido2, [Datos del Paciente].Nombre1,  " +
                                    " [Datos del Paciente].Nombre2, [Datos del Paciente].Sexo, [Datos del Paciente].CodDpto, [Datos del Paciente].CodMuni,  " +
                                    " [Datos del Paciente].ZonaResiden" +
                                    " FROM [Datos del Paciente] " +
                                    " WHERE [Datos del Paciente].HistorPaci = '" + HistoNum + "'";


                            SqlDataReader reader;

                            using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command2 = new SqlCommand(SqlDatos, connection2);
                                command2.Connection.Open();
                                reader = command2.ExecuteReader();

                                if (reader.HasRows)
                                {
                                    reader.Read();

                                    Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] " +
                                                        "WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + Coenti01 + "' AND TipoDocum = '" + reader["TipoIden"].ToString() + "' AND NumDocum = '" + reader["NumIden"].ToString() + "'";

                                    SqlDataReader DatosTemporalUser;

                                    using (SqlConnection connection3 = new SqlConnection(Conexion.conexionSQL))
                                    {
                                        SqlCommand command3 = new SqlCommand(Utils.SqlDatos, connection3);
                                        command3.Connection.Open();
                                        DatosTemporalUser = command3.ExecuteReader();

                                        if (DatosTemporalUser.HasRows == false)
                                        {


                                            string codMuni = reader["CodMuni"].ToString();

                                            if (codMuni.Length > 3 && codMuni.Length == 5)
                                            {
                                                codMuni = codMuni.Substring(2, 3);
                                            }
                                            else
                                            {
                                                Utils.Titulo01 = "Control de inserccion";
                                                Utils.Informa = "No se pudo cortar el codigo del municipio " + codMuni + "\r";
                                                Utils.Informa += "en los ultimos 3 caracteres" + "\r";
                                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }

                                            string TipoIden = reader["TipoIden"].ToString();

                                            if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                            {
                                                TipoIden = TipoIden.Substring(0, 2);

                                            }

                                            data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] " +
                                            "(TipoDocum," +
                                            "NumDocum," +
                                            "TipUsuario," +
                                            "Apellido1," +
                                            "Apellido2," +
                                            "Nombre1," +
                                            "Nombre2," +
                                            "Edad," +
                                            "EdadMedi," +
                                            "Sexo," +
                                            "CodDpto," +
                                            "CodMuni," +
                                            "ZonaResi," +
                                            "NumRemi," +
                                            "CodAdmin," +
                                            "CodDigita)" +
                                            "VALUES(" +
                                            "'" + TipoIden + "'," +
                                            "'" + reader["NumIden"].ToString() + "'," +
                                            "'" + TipoUsuario + "'," +
                                            "'" + reader["Apellido1"].ToString() + "'," +
                                            "'" + reader["Apellido2"].ToString() + "'," +
                                            "'" + reader["Nombre1"].ToString() + "'," +
                                            "'" + reader["Nombre2"].ToString() + "'," +
                                            "'" + ValorEdad + "'," +
                                            "'" + UnidadEdad + "'," +
                                            "'" + reader["Sexo"].ToString() + "'," +
                                            "'" + reader["CodDpto"].ToString() + "'," +
                                            "'" + codMuni + "'," +
                                            "'" + reader["ZonaResiden"].ToString() + "'," +
                                            "'" + Coenti01 + "'," +
                                            "'" + CodRips + "'," +
                                            "'" + UsSel + "')";

                                            SqlInsert = Conexion.SqlInsert(data);

                                            if (SqlInsert == false)
                                            {
                                                Utils.Titulo01 = "Control de inserccion";
                                                Utils.Informa = "No se pudo insertar el usario con N: " + reader["TipoIden"].ToString() + reader["NumIden"].ToString() + "\r";
                                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }

                                        } //(DatosTemporalUser.HasRows == false
                                    }//Using

                                    DatosTemporalUser.Close();
                                    DatosTemporalUser = null;

                                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                } //reader.HasRows  
                            }//Using

                            reader.Close();
                            reader = null;





                            //Transacciones -------------------------------------------------------------------------------------------------------------------------------------------------------

                            Utils.SqlDatos = " SELECT [Datos empresas y terceros].CarAdmin, [Datos empresas y terceros].CodiMinSalud, [Datos empresas y terceros].NomPlan, [Datos empresas y terceros].NomAdmin " +
                                            " FROM [Datos empresas y terceros] " +
                                            " WHERE [Datos empresas y terceros].CarAdmin = '" + Cartercero + "' ";


                            SqlDataReader DatosEmpresasYTercero;

                            using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                command.Connection.Open();

                                DatosEmpresasYTercero = command.ExecuteReader();

                                if (DatosEmpresasYTercero.HasRows)
                                {
                                    DatosEmpresasYTercero.Read();

                                    string NomPlan = DatosEmpresasYTercero["NomPlan"].ToString();

                                    if (string.IsNullOrWhiteSpace(NomPlan) == false && NomPlan.Length > 30)
                                    {
                                        NomPlan = NomPlan.Substring(0, 30);
                                    }

                                    string NomAdmin = DatosEmpresasYTercero["NomAdmin"].ToString();

                                    if (string.IsNullOrWhiteSpace(NomAdmin) == false && NomAdmin.Length > 30)
                                    {
                                        NomAdmin = NomAdmin.Substring(0, 30);

                                    }

                                    string CodAdmin = DatosEmpresasYTercero["CodiMinSalud"].ToString();

                                    if (string.IsNullOrWhiteSpace(CodAdmin) == false && CodAdmin.Length > 6)
                                    {
                                        CodAdmin = CodAdmin.Substring(0, 60);
                                    }

                                    data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS] " +
                                    "(CodDigita," +  //USER
                                    "NumRemi," + // FORMULARIO CBO
                                    "CodIPS," +  //CODIGO IPS FORMULARIO
                                    "RazonSocial," +  //NOMBRE HOSPITAL FORMULARIO
                                    "TipIdenti," + // TIPO DOCU HOSPITAL
                                    "NumIdenti," + // NUM DOCUM HOSPITAL
                                    "NumFactur," + // FACTURA GRILLA
                                    "FecFactur," + // GRILLA
                                    "FecInicio," + //GRILLA
                                    "FecFinal," + // GRILLA
                                    "CodAdmin," +  //(EMPRESAS Y TERCERO POR EL CARDINAL DE LA GRILLA BUSCO LA EMPRESA, DEBO COLOCAR CodiMinSalud
                                    "NomAdmin," +  //EMPRESAS Y TERCERO POR EL CARDINAL DE LA GRILLA BUSCO LA EMPRESA, DEBO COLOCAR NomAdmin
                                    "NumContra," +  //GRILLA DE LA TABLA DE FACTURA
                                    "PlanBene," + // EMPRESAS Y TERCERO POR EL CARDINAL DE LA GRILLA BUSCO LA EMPRESA, DEBO COLOCAR NomPlan
                                    "NumPoli," +  //GRILLA DE LA TABLA DE CUENTA CONSUMOS
                                    "Copago," + // GRILLA DE LA TABLA DE CUENTA FACTURA
                                    "ValorNeto)" +  //GRILLA DE LA TABLA DE CUENTA FACTURA
                                    "VALUES(" +
                                    "'" + UsSel + "'," +
                                    "'" + Coenti01 + "'," +
                                    "'" + CodIPS + "'," +
                                    "'" + RazonSocial + "'," +
                                    "'" + TipoDocuIPS + "'," +
                                    "'" + txtDocuIps.Text + "'," +
                                    "'" + NumFactur + "'," +
                                    "'" + FecFactur + "'," +
                                    "'" + FechaInicial + "'," +
                                    "'" + FechaFinal + "'," +
                                    "'" + CodAdmin + "'," +
                                    "'" + NomAdmin + "'," +
                                    "'" + NumContra + "'," +
                                    "'" + NomPlan + "'," +
                                    "'" + NumPoliza + "'," +
                                    "'" + Copago + "'," +
                                    "'" + ValorFac + "')";



                                    SqlInsert = Conexion.SqlInsert(data);

                                    if (SqlInsert == false)
                                    {
                                        Utils.Titulo01 = "Control de inserccion";
                                        Utils.Informa = "No se pudo insertar transacciones : " + NumFactur + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                }
                            }






                            //CONSULTAS -------------------------------------------------------------------------------------------------------------------------------------------------------------

                            string Para02 = "01"; //'Código del grupo de consultas
                                                  // Permite copiar los datos de las consultas, para RIPS, por facturas

                            Utils.SqlDatos = "SELECT [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos registros de consumos].FechaCon, [Datos registros de consumos].AutoriNum, ";

                            switch (MT)
                            {
                                case "1": //Manual SOAT
                                    Utils.SqlDatos += "[Datos registros de consumos].CodiSOAT as CodConsul,";
                                    break;
                                case "2": //Manual ISS
                                    Utils.SqlDatos += "[Datos registros de consumos].CodiISS as CodConsul,";
                                    break;
                                case "3": //Manual CUPS
                                    Utils.SqlDatos += "[Datos registros de consumos].CodiCUPS as CodConsul,";
                                    break;
                                case "4": //Manual IPS
                                    Utils.SqlDatos += "[Datos registros de consumos].CodInter as CodConsul,";
                                    break;
                                default: //Utilice el manual IPS
                                    Utils.SqlDatos += "[Datos registros de consumos].CodInter as CodConsul,";
                                    break;
                            }


                            Utils.SqlDatos += " [Datos registros de consumos].FinalConsul, [Datos cuentas de consumos].CausaExterna, [Datos cuentas de consumos].DxSalida, [Datos cuentas de consumos].DxRelac01, [Datos cuentas de consumos].DxRelac02,   " +
                                " [Datos cuentas de consumos].DxRelac03, [Datos cuentas de consumos].TipoDxPrin, [Datos registros de consumos].ValorUnitario, [Datos registros de consumos].Copagos,  " +
                                " ([Datos registros de consumos].ValorUnitario - [Datos registros de consumos].Copagos) as VN " +
                                " FROM [Datos catalogo de servicios] INNER JOIN " +
                                " [Datos registros de consumos] ON [Datos catalogo de servicios].CodInterno = [Datos registros de consumos].CodInter INNER JOIN " +
                                " [Datos cuentas de consumos] ON [Datos registros de consumos].CuenConsu = [Datos cuentas de consumos].CuenNum INNER JOIN " +
                                " [Datos del Paciente] ON [Datos cuentas de consumos].HistoNum = [Datos del Paciente].HistorPaci " +
                                " WHERE [Datos registros de consumos].ValorUnitario > 0 " +
                                " AND [Datos registros de consumos].PagaHoja = 1 " +
                                " AND [Datos registros de consumos].Cantidad > 0 " +
                                " AND [Datos catalogo de servicios].GrupoServi = '" + Para02 + "' " +
                                " AND [Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "'  ";


                            SqlDataReader ArchivoConsultas;

                            using (SqlConnection connection3 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command3 = new SqlCommand(Utils.SqlDatos, connection3);
                                command3.Connection.Open();

                                ArchivoConsultas = command3.ExecuteReader();


                                if (ArchivoConsultas.HasRows)
                                {

  
                                    while (ArchivoConsultas.Read())
                                    {
                                        string TipoIden = ArchivoConsultas["TipoIden"].ToString();

                                        if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                        {
                                            TipoIden = TipoIden.Substring(0, 2);

                                        }

                                        data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS] " +
                                              "(CodDigita," +
                                              "NumRemi," +
                                              "NumFactur," +
                                              "CodIPS," +
                                              "TipoDocum," +
                                              "NumDocum," +
                                              "FecConsul," +
                                              "AutoriNum," +
                                              "CodConsul," +
                                              "FinalConsul," +
                                              "CausExter," +
                                              "DxPrincipal," +
                                              "DxRelacion1," +
                                              "DxRelacion2," +
                                              "DxRelacion3," +
                                              "TipoDxPrin," +
                                              "ValorConsul," +
                                              "ValorCuota," +
                                              "ValorNeto)" +
                                              "VALUES(" +
                                              "'" + UsSel + "'," +
                                              "'" + Coenti01 + "'," +
                                              "'" + NumFactur + "'," +
                                              "'" + CodIPS + "'," +
                                              "'" + TipoIden + "'," +
                                              "'" + ArchivoConsultas["NumIden"].ToString() + "'," +
                                              "'" + Convert.ToDateTime(ArchivoConsultas["FechaCon"]).ToString("yyyy-MM-dd") + "'," +
                                              "'" + ArchivoConsultas["AutoriNum"].ToString() + "'," +
                                              "'" + ArchivoConsultas["CodConsul"].ToString() + "'," +
                                              "'" + ArchivoConsultas["FinalConsul"].ToString() + "'," +
                                              "'" + ArchivoConsultas["CausaExterna"].ToString() + "'," +
                                              "'" + ArchivoConsultas["DxSalida"].ToString() + "'," +
                                              "'" + ArchivoConsultas["DxRelac01"].ToString() + "'," +
                                              "'" + ArchivoConsultas["DxRelac02"].ToString() + "'," +
                                              "'" + ArchivoConsultas["DxRelac03"].ToString() + "'," +
                                              "'" + ArchivoConsultas["TipoDxPrin"].ToString() + "'," +
                                              "'" + ArchivoConsultas["ValorUnitario"].ToString() + "'," +
                                              "'" + ArchivoConsultas["Copagos"].ToString() + "'," +
                                              "'" + ArchivoConsultas["VN"].ToString() + "');";


                                        SqlInsert = Conexion.SqlInsert(data);
                                    }
                                }

                            } //FIN Using

                            int SubTolD = Convert.ToInt32(TxtMarcadas.Text);



                            //PROCEDIMIENTOS -------------------------------------------------------------------------------------------------------------------------

                            int FunP = ProceSoloPorFacturas(NumFactur, CodIPS, Coenti01, NumCuenFac, MT, SubTolD);

                            if (FunP == -1)
                            {
                            }
                            else
                            {

                                //OTROS SEVICIOS -------------------------------------------------------------------------------------------------------------------------

                                Utils.SqlDatos = "SELECT  [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos registros de consumos].AutoriNum, [Datos registros de consumos].FinalProce,";

                                switch (MT)
                                {
                                    case "1": //Manual SOAT
                                        Utils.SqlDatos += "[Datos registros de consumos].CodiSOAT as CodConsul,";
                                        break;
                                    case "2": //Manual ISS
                                        Utils.SqlDatos += "[Datos registros de consumos].CodiISS as CodConsul,";
                                        break;
                                    case "3": //Manual CUPS
                                        Utils.SqlDatos += "[Datos registros de consumos].CodiCUPS as CodConsul,";
                                        break;
                                    case "4": //Manual IPS
                                        Utils.SqlDatos += "[Datos registros de consumos].CodInter as CodConsul,";
                                        break;
                                    default: //Utilice el manual IPS
                                        Utils.SqlDatos += "[Datos registros de consumos].CodInter as CodConsul,";
                                        break;
                                }

                                Utils.SqlDatos += " [Datos catalogo de servicios].NomServicio, [Datos registros de consumos].Cantidad, [Datos registros de consumos].ValorUnitario, ([Datos registros de consumos].Cantidad * [Datos registros de consumos].ValorUnitario) as TolSer " +
                                                " FROM [Datos del Paciente] INNER JOIN " +
                                                " [Datos cuentas de consumos] ON [Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum INNER JOIN " +
                                                " [Datos registros de consumos] ON [Datos cuentas de consumos].CuenNum = [Datos registros de consumos].CuenConsu INNER JOIN " +
                                                " [Datos catalogo de servicios] ON [Datos registros de consumos].CodInter = [Datos catalogo de servicios].CodInterno " +
                                                " WHERE [Datos registros de consumos].ValorUnitario > 0 AND [Datos registros de consumos].PagaHoja = 1 AND [Datos registros de consumos].Cantidad > 0 AND[Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "' " +
                                                " AND ([Datos catalogo de servicios].GrupoServi = '06' OR[Datos catalogo de servicios].GrupoServi = '07' " +
                                                " OR [Datos catalogo de servicios].GrupoServi = '08' OR[Datos catalogo de servicios].GrupoServi = '09' " +
                                                " OR [Datos catalogo de servicios].GrupoServi = '10' OR[Datos catalogo de servicios].GrupoServi = '11' " +
                                                " OR [Datos catalogo de servicios].GrupoServi = '14'); ";


                                SqlDataReader ArchivoOtrosServicios;

                                using (SqlConnection connection4 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command4 = new SqlCommand(Utils.SqlDatos, connection4);
                                    command4.Connection.Open();

                                    ArchivoOtrosServicios = command4.ExecuteReader();

                                    if (ArchivoOtrosServicios.HasRows)
                                    {
                                        while (ArchivoOtrosServicios.Read())
                                        {

                                            string TipoIden = ArchivoOtrosServicios["TipoIden"].ToString();

                                            if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                            {
                                                TipoIden = TipoIden.Substring(0, 2);

                                            }

                                            string NomServi = ArchivoOtrosServicios["NomServicio"].ToString();

                                            if (string.IsNullOrWhiteSpace(NomServi) == false && NomServi.Length > 60)
                                            {
                                                NomServi = NomServi.Substring(0, 60);

                                            }


                                            data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS] " +
                                                    "(CodDigita," +
                                                    "NumRemi," +
                                                    "NumFactur," +
                                                    "CodIPS," +
                                                    "TipoDocum," +
                                                    "NumDocum," +
                                                    "AutoriNum," +
                                                    "TipoServicio," +
                                                    "CodiServi," +
                                                    "NomServi," +
                                                    "Cantidad," +
                                                    "ValorUnita," +
                                                    "ValorTotal)" +
                                                    "VALUES(" +
                                                    "'" + UsSel + "'," +
                                                    "'" + Coenti01 + "'," +
                                                    "'" + NumFactur + "'," +
                                                    "'" + CodIPS + "'," +
                                                    "'" + TipoIden + "'," +
                                                    "'" + ArchivoOtrosServicios["NumIden"].ToString() + "'," +
                                                    "'" + ArchivoOtrosServicios["AutoriNum"].ToString() + "'," +
                                                    "'" + ArchivoOtrosServicios["FinalProce"].ToString() + "'," +
                                                    "'" + ArchivoOtrosServicios["CodConsul"].ToString() + "'," +
                                                    "'" + NomServi + "'," +
                                                    "'" + ArchivoOtrosServicios["Cantidad"].ToString() + "'," +
                                                    "'" + ArchivoOtrosServicios["ValorUnitario"].ToString() + "'," +
                                                    "'" + ArchivoOtrosServicios["TolSer"].ToString() + "');";

                                            SqlInsert = Conexion.SqlInsert(data);
                                        }
                                    }

                                    ArchivoOtrosServicios.Close();

                                }


                                //HOSPITALIZADOS

                                Para02 = "04"; //El tipo de cuenta de hospitalizados

                                //Permite copiar los datos de los hospitalizados, para RIPS, por facturas


                                Utils.SqlDatos = "SELECT [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos cuentas de consumos].ServiRips, [Datos cuentas de consumos].FecEntrada, [Datos cuentas de consumos].HorEntrada," +
                                                " [Datos cuentas de consumos].NumRemi, [Datos cuentas de consumos].CausaExterna, [Datos cuentas de consumos].DxEntra, [Datos cuentas de consumos].DxSalida, [Datos cuentas de consumos].DxRelac01," +
                                                " [Datos cuentas de consumos].DxRelac02, [Datos cuentas de consumos].DxRelac03, [Datos cuentas de consumos].DxComplica, [Datos cuentas de consumos].EstaSalida, [Datos cuentas de consumos].FecSalida," +
                                                " [Datos cuentas de consumos].HorSalida, [Datos cuentas de consumos].DxMuerte, [Datos cuentas de consumos].TipoCuenta " +
                                                " FROM [Datos del Paciente] INNER JOIN " +
                                                " [Datos cuentas de consumos] ON[Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum " +
                                                " WHERE [Datos cuentas de consumos].TipoCuenta = '04' AND[Datos cuentas de consumos].DiasEstancias <> 0 AND[Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "'";


                                SqlDataReader ArchivoHospitalizacion;

                                using (SqlConnection connection5 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command5 = new SqlCommand(Utils.SqlDatos, connection5);
                                    command5.Connection.Open();

                                    ArchivoHospitalizacion = command5.ExecuteReader();


                                    if (ArchivoHospitalizacion.HasRows)
                                    {         
                                        while (ArchivoHospitalizacion.Read())
                                        {
                                            string TipoIden = ArchivoHospitalizacion["TipoIden"].ToString();

                                            if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                            {
                                                TipoIden = TipoIden.Substring(0, 2);

                                            }

                                            data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS] " +
                                                    "(CodDigita," +
                                                    "NumRemi," +
                                                    "NumFactur," +
                                                    "CodIPS," +
                                                    "TipoDocum," +
                                                    "NumDocum," +
                                                    "ViaDIngreso," +
                                                    "FecIngresa," +
                                                    "HorIngresa," +
                                                    "AutoriNum," +
                                                    "CausExter," +
                                                    "DxPrincIngre," +
                                                    "DxPrincEgre," +
                                                    "DxRelacion1," +
                                                    "DxRelacion2," +
                                                    "DxRelacion3," +
                                                    "DxComplica," +
                                                    "EstadoSal," +
                                                    "FecSalida," +
                                                    "HorSalida," +
                                                    "DxMuerte)" +
                                                    "VALUES(" +
                                                    "'" + UsSel + "'," +
                                                    "'" + Coenti01 + "'," +
                                                    "'" + NumFactur + "'," +
                                                    "'" + CodIPS + "'," +
                                                    "'" + TipoIden + "'," +
                                                    "'" + ArchivoHospitalizacion["NumIden"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["ServiRips"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoHospitalizacion["FecEntrada"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoHospitalizacion["HorEntrada"]).ToString("hh:mm:ss") + "'," +
                                                    "'" + ArchivoHospitalizacion["NumRemi"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["CausaExterna"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["DxEntra"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["DxSalida"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["DxRelac01"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["DxRelac02"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["DxRelac03"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["DxComplica"].ToString() + "'," +
                                                    "'" + ArchivoHospitalizacion["EstaSalida"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoHospitalizacion["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoHospitalizacion["HorSalida"]).ToString("hh:mm:ss") + "'," +
                                                    "'" + ArchivoHospitalizacion["DxMuerte"].ToString() + "');";

                                            SqlInsert = Conexion.SqlInsert(data);
                                        }                              
                                    }
                                    ArchivoHospitalizacion.Close();
                                }

                                //OBSERVACION

                                Para02 = "04"; //El tipo de cuenta de hospitalizados

                                //Permite copiar los datos de los hospitalizados, para RIPS, por facturas

                                Utils.SqlDatos = "SELECT [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos cuentas de consumos].ServiRips, [Datos cuentas de consumos].FecEntrada, [Datos cuentas de consumos].HorEntrada, " +
                                                 " [Datos cuentas de consumos].NumRemi, [Datos cuentas de consumos].CausaExterna, [Datos cuentas de consumos].DxEntra, [Datos cuentas de consumos].DxSalida, [Datos cuentas de consumos].DxRelac01,  " +
                                                 " [Datos cuentas de consumos].DxRelac02, [Datos cuentas de consumos].DxRelac03,[Datos cuentas de consumos].Destino, [Datos cuentas de consumos].EstaSalida, [Datos cuentas de consumos].FecSalida,  " +
                                                 " [Datos cuentas de consumos].HorSalida, [Datos cuentas de consumos].DxMuerte " +
                                                 " FROM [Datos del Paciente] INNER JOIN " +
                                                 " [Datos cuentas de consumos] ON[Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum " +
                                                 " WHERE [Datos cuentas de consumos].TipoCuenta = '" + Para02 + "' AND [Datos cuentas de consumos].DiasEstancias = 0 AND[Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "'";


                                SqlDataReader ArchivoObservacion;

                                using (SqlConnection connection6 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command6 = new SqlCommand(Utils.SqlDatos, connection6);
                                    command6.Connection.Open();

                                    ArchivoObservacion = command6.ExecuteReader();

                                    if (ArchivoObservacion.HasRows)
                                    {
        
                                        while (ArchivoObservacion.Read())
                                        {
                                            string TipoIden = ArchivoObservacion["TipoIden"].ToString();

                                            if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                            {
                                                TipoIden = TipoIden.Substring(0, 2);

                                            }

                                            data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS] " +
                                                    "(CodDigita," +
                                                    "NumRemi," +
                                                    "NumFactur," +
                                                    "CodIPS," +
                                                    "TipoDocum," +
                                                    "NumDocum," +
                                                    "FecIngresa," +
                                                    "HorIngresa," +
                                                    "AutoriNum," +
                                                    "CausExter," +
                                                    "DxPrincIngre," +
                                                    "DxRelacion1," +
                                                    "DxRelacion2," +
                                                    "DxRelacion3," +
                                                    "Destino," +
                                                    "EstadoSal," +
                                                    "DxMuerte," +
                                                    "FecSalida," +
                                                    "HorSalida)" +
                                                    "VALUES(" +
                                                    "'" + UsSel + "'," +
                                                    "'" + Coenti01 + "'," +
                                                    "'" + NumFactur + "'," +
                                                    "'" + CodIPS + "'," +
                                                    "'" + TipoIden + "'," +
                                                    "'" + ArchivoObservacion["NumIden"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoObservacion["FecEntrada"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoObservacion["HorEntrada"]).ToString("hh:mm:ss") + "'," +
                                                    "'" + ArchivoObservacion["NumRemi"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["CausaExterna"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["DxSalida"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["DxRelac01"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["DxRelac02"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["DxRelac03"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["Destino"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["EstaSalida"].ToString() + "'," +
                                                    "'" + ArchivoObservacion["DxMuerte"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoObservacion["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(ArchivoObservacion["HorSalida"]).ToString("hh:mm:ss") + "');";

                                            SqlInsert = Conexion.SqlInsert(data);
                                        }                       

                                    }
                                } //Fin Suing

                                ArchivoObservacion.Close();


                                //RECIEN NACIDOS ----------------------------------------------------------

                                //  'Permite copiar los datos de los recien nacidos, para RIPS, por facturas

                                Utils.SqlDatos = "SELECT [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos de recien nacidos].FechaNaci, [Datos de recien nacidos].HoraNaci, [Datos de recien nacidos].EdadGesta, [Datos de recien nacidos].ConPrena,  " +
                                                " [Datos de recien nacidos].SexoNaci, [Datos de recien nacidos].PesoNaci, [Datos de recien nacidos].DxNaci, [Datos de recien nacidos].DxMuerNaci, [Datos de recien nacidos].FecMuerNaci,  " +
                                                " [Datos de recien nacidos].HorMuerNaci " +
                                                " FROM [Datos del Paciente] INNER JOIN " +
                                                " [Datos cuentas de consumos] ON[Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum INNER JOIN " +
                                                " [Datos de recien nacidos] ON[Datos cuentas de consumos].CuenNum = [Datos de recien nacidos].CuenParto AND[Datos del Paciente].HistorPaci = [Datos de recien nacidos].HistorMadre " +
                                                " WHERE [Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "'";


                                SqlDataReader ArchivoRecienNacidos;

                                using (SqlConnection connection7 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command7 = new SqlCommand(Utils.SqlDatos, connection7);
                                    command7.Connection.Open();

                                    ArchivoRecienNacidos = command7.ExecuteReader();


                                    if (ArchivoRecienNacidos.HasRows)
                                    {

                                        while (ArchivoRecienNacidos.Read())
                                        {
                                            string TipoIden = ArchivoRecienNacidos["TipoIden"].ToString();

                                            if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                            {
                                                TipoIden = TipoIden.Substring(0, 2);

                                            }




                                            data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS]" +
                                                    "(CodDigita," +
                                                    "NumRemi," +
                                                    "NumFactur," +
                                                    "CodIPS," +
                                                    "TipoDocum," +
                                                    "NumDocum," +
                                                    "FecNaci," +
                                                    "HorIngresa," +
                                                    "EdadGesta," +
                                                    "ControlPrena," +
                                                    "SexoRecien," +
                                                    "PesoRecien," +
                                                    "DxRecien," +
                                                    "DxMuerte";
                                                    if (string.IsNullOrWhiteSpace(ArchivoRecienNacidos["FecMuerNaci"].ToString())) //si la fecha viene null termine el insert aqui
                                                    {
                                                        data += ")";
                                                    }
                                                    else
                                                    {
                                                        data += ",FecMuerte," +
                                                                "HorMuerte)";
                                                    }

                                            data += "VALUES(" +
                                                     "'" + UsSel + "'," +
                                                     "'" + Coenti01 + "'," +
                                                     "'" + NumFactur + "'," +
                                                     "'" + CodIPS + "'," +
                                                     "'" + TipoIden + "'," +
                                                     "'" + ArchivoRecienNacidos["NumIden"].ToString() + "'," +
                                                     "'" + Convert.ToDateTime(ArchivoRecienNacidos["FechaNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                     "'" + Convert.ToDateTime(ArchivoRecienNacidos["HoraNaci"]).ToString("hh:mm:ss") + "'," +
                                                     "'" + ArchivoRecienNacidos["EdadGesta"].ToString() + "'," +
                                                     "'" + ArchivoRecienNacidos["ConPrena"].ToString() + "'," +
                                                     "'" + ArchivoRecienNacidos["SexoNaci"].ToString() + "'," +
                                                     "'" + ArchivoRecienNacidos["PesoNaci"].ToString() + "'," +
                                                     "'" + ArchivoRecienNacidos["DxNaci"].ToString() + "'," +
                                                     "'" + ArchivoRecienNacidos["DxMuerNaci"].ToString() + "'";
                                                    if (string.IsNullOrWhiteSpace(ArchivoRecienNacidos["FecMuerNaci"].ToString()))
                                                    {
                                                        data += ")";
                                                    }
                                                    else
                                                    {
                                                        data += ",'" + Convert.ToDateTime(ArchivoRecienNacidos["FecMuerNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                            "'" + Convert.ToDateTime(ArchivoRecienNacidos["HorMuerNaci"]).ToString("hh:mm:ss") + "')";
                                                    } 

                                            SqlInsert = Conexion.SqlInsert(data);
                                        }                  

                                    }
                                    ArchivoRecienNacidos.Close();
                                }


                                //MEDICAMENTO ----------------------------------------------------------------------------------

                                Utils.SqlDatos = "SELECT [Datos del Paciente].TipoIden, [Datos del Paciente].NumIden, [Datos cuentas de consumos].NumRemi, [Datos catalogo de servicios].CodiMedMin, [Datos catalogo de servicios].PosMedi,  " +
                                                " [Datos catalogo de servicios].NomServicio, [Datos registros de consumos].Cantidad,[Datos catalogo de servicios].GrupoServi, [Datos registros de consumos].ValorUnitario, ([Datos registros de consumos].Cantidad * [Datos registros de consumos].ValorUnitario) as VT, [Datos registros de consumos].CodInter " +
                                                " FROM [Datos del Paciente] INNER JOIN " +
                                                " [Datos cuentas de consumos] ON[Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum INNER JOIN " +
                                                " [Datos registros de consumos] ON[Datos cuentas de consumos].CuenNum = [Datos registros de consumos].CuenConsu INNER JOIN " +
                                                " [Datos catalogo de servicios] ON[Datos registros de consumos].CodInter = [Datos catalogo de servicios].CodInterno " +
                                                " WHERE [Datos registros de consumos].PagaHoja = 1 AND[Datos registros de consumos].ValorUnitario > 0 AND[Datos registros de consumos].Cantidad > 0 AND[Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "' " +
                                                " AND ([Datos catalogo de servicios].GrupoServi = '12' OR[Datos catalogo de servicios].GrupoServi = '13')";

                                SqlDataReader ArchivoMedicamentos;

                                using (SqlConnection connection8 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection8);
                                    command8.Connection.Open();

                                    ArchivoMedicamentos = command8.ExecuteReader();


                                    if (ArchivoMedicamentos.HasRows)
                                    {
                                        while (ArchivoMedicamentos.Read())
                                        {
                                            string NomGenerico = ArchivoMedicamentos["NomServicio"].ToString();

                                            if (string.IsNullOrWhiteSpace(NomGenerico) == false && NomGenerico.Length > 30)
                                            {
                                                NomGenerico = NomGenerico.Substring(0, 30);
                                            }

                                            string TipoIden = ArchivoMedicamentos["TipoIden"].ToString();

                                            if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                            {
                                                TipoIden = TipoIden.Substring(0, 2);

                                            }

                                            data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS] " +
                                                    "(CodDigita," +
                                                    "NumRemi," +
                                                    "NumFactur," +
                                                    "CodIPS," +
                                                    "TipoDocum," +
                                                    "NumDocum," +
                                                    "AutoriNum," +
                                                    "CodMedica," +
                                                    "TipoMedica," +
                                                    "NomGenerico," +
                                                    "NumUnidad," +
                                                    "ValorUnita," +
                                                    "ValorTotal)" +
                                                    "VALUES(" +
                                                    "'" + UsSel + "'," +
                                                    "'" + Coenti01 + "'," +
                                                    "'" + NumFactur + "'," +
                                                    "'" + CodIPS + "'," +
                                                    "'" + TipoIden + "'," +
                                                    "'" + ArchivoMedicamentos["NumIden"].ToString() + "'," +
                                                    "'" + ArchivoMedicamentos["NumRemi"].ToString() + "'," +
                                                    "'" + ArchivoMedicamentos["CodiMedMin"].ToString() + "'," +
                                                    "'" + ArchivoMedicamentos["PosMedi"].ToString() + "'," +
                                                    "'" + NomGenerico + "'," +
                                                    "'" + ArchivoMedicamentos["Cantidad"].ToString() + "'," +
                                                    "'" + ArchivoMedicamentos["ValorUnitario"].ToString() + "'," +
                                                    "'" + ArchivoMedicamentos["VT"].ToString() + "')";

                                            SqlInsert = Conexion.SqlInsert(data);

                                        }

                                    }
                                }
                                ArchivoMedicamentos.Close();

                                //FunP = ComDatosMedica(Coenti01);


                            }//final funcion FunP


                        }//Estado Grilla

                        BarraSeleccionar.Increment(1);
                    } //Foreach Grilla


                    if (SqlInsert)
                    {
                        Utils.Informa = "He terminado de procesar todos ";
                        Utils.Informa += "los datos que conforman los RIPS ";
                        Utils.Informa += "de las facturas seleccionadas.";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    BarraSeleccionar.Minimum = 0;
                    BarraSeleccionar.Maximum = 1;
                    BarraSeleccionar.Value = 0;

                } // Dialogo Yes
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el boton seleccionar" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                BarraSeleccionar.Minimum = 0;
                BarraSeleccionar.Maximum = 1;
                BarraSeleccionar.Value = 0;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void FrmRipsRegimen_Load(object sender, EventArgs e)
        {
            try
            {
                DatosDeLaEmpresa();
                CargarDatosUser();
                CargarRangoFechas();
                CargarComboBox();
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al abrir formulario Rips por regimen" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }
}
