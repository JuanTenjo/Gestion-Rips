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
namespace Gestion_Rips.Forms.Exportar
{
    public partial class FrmExportarSedarips : Form
    {
        public FrmExportarSedarips()
        {
            InitializeComponent();
        }

        #region ComboBox
        private void CargarCombobox()
        {
            try
            {
                this.cboNameEntidades.DataSource = null;
                this.cboNameEntidades.Items.Clear();

                Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre" +
                                 " FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 'True') and(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 'True'))" +
                                 " and ([NomAdmin] + ' ' + [ProgrAmin]) is not null ORDER BY([NomAdmin] +' ' + [ProgrAmin])";

                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    this.cboNameEntidades.DataSource = dataSet.Tables[0];
                    this.cboNameEntidades.ValueMember = "CarAdmin";
                    this.cboNameEntidades.DisplayMember = "NP";
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
        private void CargarContratos()
        {
            try
            {
                this.cboContratos.DataSource = null;
                this.cboContratos.Items.Clear();

                Utils.SqlDatos = "SELECT ContratoN, NomContra FROM [ACDATOXPSQL].[dbo].[Datos contratos administradoras] WHERE CodAdmin = '" + txtCardinal.Text + "' AND Vigente = 1";

                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    this.cboContratos.DataSource = dataSet.Tables[0];
                    this.cboContratos.ValueMember = "ContratoN";
                    this.cboContratos.DisplayMember = "NomContra";
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CargarContratos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
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
                        if (string.IsNullOrEmpty(txtCardinal.Text) == false)
                        {
                            IDContrato.Text = NumContratoID(this.txtCardinal.Text);
                        }

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

        #endregion

        #region Funciones y Procesos

        private int ValidarProcedi(string c, double T, string CodDg)
        {
            try
            {
                string SqlProceTemp, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlProceTemp = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] ";
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
                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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

                SqlReNaciTemp = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] ";
                SqlReNaciTemp += "WHERE (([Datos temporal observacion RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlReNaciTemp += "((Datos temporal observacion RIPS].NumRemi) = '" + c + "');";
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
                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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
        private int ValidarOtros(string c, double T, string CodDg)
        {
            try
            {
                string SqlOtrosTemp, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlOtrosTemp = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] ";
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
                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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

                SqlMediTem = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] ";
                SqlMediTem += "WHERE (([Datos temporal observacion RIPS].CodDigita) = '" + CodDg + "') and ";
                SqlMediTem += "((Datos temporal observacion RIPS].NumRemi) = '" + c + "');";
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
                                        ObErr = ObErr + "El código " + TablaAux1["DxPrincIngre"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
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
                                        ObErr = ObErr + "El código " + TablaAux1["DxRelacion1"].ToString() + ", del diagnóstico DxRelacion1 no existe la resolución vigente.";
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
                                        ObErr = ObErr + "El código " + TablaAux1["DxRelacion2"].ToString() + ", del diagnóstico DxRelacion2 no existe la resolución vigente.";
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
                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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
                string SqlMediTem, Dp, mMuCi, ObErr, Z, RutaGeo, Msj, DxPr, FunDx;
                int RegExp, FunDpto, FunMuni, FiCon, VR;

                SqlMediTem = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] ";
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
                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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

                SqlConsulTem = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] " +
                "WHERE ((Datos temporal hospitalizacion RIPS].CodDigita) = '" + CodDg + "' ) and " +
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
                                        ObErr = ObErr + "El código " + TablaAux1["DxPrincIngre"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
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
                                        ObErr = ObErr + "El código " + TablaAux1["DxPrincEgre"].ToString() + ", el diagnóstico principal de egreso no existe la resolución vigente.";
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
                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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

                SqlConsulTem = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS] " +
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
                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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

                SqlFacTemp = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] " +
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
                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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
        private int ValidarUsuarios(string c, string TU, double T, string CodDg)
        {
            try
            {
                //'Permite validar los datos de los usuarios seleccionados de una entidad para los RIPS
                string SqlUsuaTemp, TD, ND, ObErr, Dp, MuCi, Z;
                int VR, VDev, RegExp;


                SqlUsuaTemp = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] " +
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

                            if (TabLocal["TipUsuario"].ToString() != TU)
                            {
                                RegExp = 1;
                                ObErr += " El tipo de usuario o regimen " + TabLocal["TipUsuario"].ToString() + " no corresponde a la entidad";
                            }

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
                            MuCi = TabLocal["CodDptoCity"].ToString();
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
                            Utils.SqlDatos += "WHERE CodigoDpto = '" + Dp + "' AND [Datos ciudades del dpto].CodDptoCity = '" + MuCi + "'";

                            SqlDataReader TablaAux11;

                            using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                command.Connection.Open();
                                TablaAux11 = command.ExecuteReader();


                                if (TablaAux11.HasRows == false)
                                {
                                    RegExp = 1;
                                    ObErr = "El Código del Municipio " + MuCi + ", no es valido para el DPTO.";
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
                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] ";
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
        private string NumContratoID(string CarBus)
        {
            try
            {
                string SqlContratos = null;

                SqlContratos = "SELECT ContratoN, NomContra, NumConID " +
                "FROM [ACDATOXPSQL].[dbo].[Datos contratos administradoras] " +
                "WHERE [CodAdmin]= N'" + CarBus + "' AND [Vigente]= 1 ";

                SqlDataReader TabContratos = Conexion.SQLDataReader(SqlContratos);

                if (TabContratos.HasRows) {
                    TabContratos.Read();

                    return TabContratos["NumConID"].ToString();
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "CControl para cargar formularios";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función NumContratoID." + "\r";
                Utils.Informa += " Error:" + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "0";
            }
        }
        private void DataGridFacturas_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (DataGridFacturas.SelectedCells.Count != 0)
                {
                    string CodArt = DataGridFacturas.SelectedCells[0].Value.ToString();
                    DigeteOrigen.Text = CodArt;
                }
                else
                {
                    DigeteOrigen.Text = null;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de actualizar la lista origen " + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static Boolean BorrarTempoRips(string UsSel, string ConMinRips)
        {
            try
            {
                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                Boolean EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS] WHERE CodDigita = '" + UsSel + "' AND NumRemi = '" + ConMinRips + "'";
                EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                return EstadoDelete;

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion borrar Tempo Rips" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
        private void CalcularTotalFactura()
        {
            try
            {
                int Contador = 0;
                int Contador2 = 0;

                foreach (DataGridViewRow item in DataGridFacturas.Rows)
                {
                    Contador += 1;
                }
                foreach (DataGridViewRow item in DataGridDestino.Rows)
                {
                    Contador2 += 1;
                }
                txtTotalCantidadFacturas.Text = (Contador).ToString();
                txtTotalCantidadDestino.Text = (Contador2).ToString();
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
        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                string Coenti01, CR;
                string Coenti02 = null, NEnti = null, UsSel = null, TDE = null, NCC = null, Para02 = null, Para01 = null, TUReg = null, AcRe = null;
                int SiNoP = 0, FunAudi = 0, FunUs = 0, FunFac = 0, FunCon = 0, FunHos = 0, FunObs = 0, FunMedi = 0, FunOtros = 0, FunReN = 0, FunProce = 0, TolInco = 0;
                double TolUsa, TolOtrosSer, TolConsul, TolHos, TolMedi, TolObs, TolOtros, TolReN, TolProce, TolFac;
                string Sqlsuarios, SqlFacturas, SqlHospitalizados, SqlUrgencias, SqlRNacidos, SqlConsultas, SqlMedica, SqlProcedimientos, SqlOtrosServi;
                Utils.Titulo01 = "Control para validar datos";

                Coenti01 = cboNameEntidades.SelectedValue.ToString();

                if (Coenti01 == null || string.IsNullOrEmpty(Coenti01))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha";
                    Utils.Informa += "seleccionado el nombre de la entidad.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        Coenti02 = Coenti01;
                        NEnti = sqlDataReader2["NP"].ToString();
                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();
                        TUReg = sqlDataReader2["RegimenAdmin"].ToString(); //'Código del tipo de usuario o regimen
                        AcRe = sqlDataReader2["ActiReali"].ToString(); //'Identifica si las actividades son de P y P, Nivel 1.... o SOAT
                        CR = txtRips.Text;
                        Para01 = Coenti02;
                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
                }

                if (string.IsNullOrEmpty(lblCodigoUser.Text) || lblCodigoUser.Text == "")
                {
                    Utils.Informa = "Lo siento pero el código del usuario";
                    Utils.Informa += "no es valido para validar datos. ";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblCodigoUser.Select();
                    return;
                }

                UsSel = lblCodigoUser.Text;

                //'Como los datos temporales de RIPS ahara se registran en la tabla RIPS, procedemos a contar desde la misma
                //Proceda a contar cada uno los archivos de los rips

                Sqlsuarios = "SELECT COUNT(CodDigita) AS TolUsuarios ";
                Sqlsuarios += "FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS]";
                Sqlsuarios += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                Sqlsuarios += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabUsuarios = Conexion.SQLDataReader(Sqlsuarios);

                if (TabUsuarios.HasRows == false)
                {
                    TolUsa = 0;
                }
                else
                {
                    TabUsuarios.Read();

                    if (Convert.ToInt32(TabUsuarios["TolUsuarios"].ToString()) <= 0)
                    {
                        TolUsa = 0;
                    }
                    else
                    {
                        TolUsa = Convert.ToDouble(TabUsuarios["TolUsuarios"].ToString());
                    }
                }

                TabUsuarios.Close();
                TabUsuarios = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                if (TolUsa <= 0)
                {
                    Utils.Informa = "El proceso de validación de este módulo no se";
                    Utils.Informa += "puede realizar mientras no se seleccione los ";
                    Utils.Informa += "usuarios de la entidad seleccionada.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblCodigoUser.Select();
                    return;
                }

                SqlFacturas = "SELECT COUNT(CodDigita) AS TolFacturas, SUM(Copago) AS ValCopaFac, SUM(ValorNeto) AS ValNetoFac ";
                SqlFacturas += "FROM [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] ";
                SqlFacturas += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlFacturas += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabFacturas = Conexion.SQLDataReader(SqlFacturas);


                if (TabFacturas.HasRows == false)
                {
                    TolFac = 0;
                }
                else
                {
                    TabFacturas.Read();
                    if (Convert.ToInt32(TabFacturas["TolFacturas"].ToString()) <= 0)
                    {
                        TolFac = 0;
                    }
                    else
                    {
                        TolFac = Convert.ToDouble(TabFacturas["TolFacturas"]);
                    }
                }

                TabFacturas.Close();
                TabFacturas = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlHospitalizados = "SELECT COUNT(CodDigita) AS TolHospi ";
                SqlHospitalizados += "FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] ";
                SqlHospitalizados += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlHospitalizados += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabHospitalizados = Conexion.SQLDataReader(SqlHospitalizados);

                if (TabHospitalizados.HasRows == false)
                {
                    TolHos = 0;
                }
                else
                {
                    TabHospitalizados.Read();
                    if (Convert.ToInt32(TabHospitalizados["TolHospi"].ToString()) <= 0)
                    {
                        TolHos = 0;
                    }
                    else
                    {
                        TolHos = Convert.ToDouble(TabHospitalizados["TolHospi"].ToString());
                    }
                }

                TabHospitalizados.Close();
                TabHospitalizados = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlUrgencias = "SELECT COUNT(CodDigita) AS TolObserva ";
                SqlUrgencias += "FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] ";
                SqlUrgencias += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlUrgencias += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabUrgencias = Conexion.SQLDataReader(SqlUrgencias);

                if (TabUrgencias.HasRows == false)
                {
                    TolObs = 0;
                }
                else
                {
                    TabUrgencias.Read();
                    if (Convert.ToInt32(TabUrgencias["TolObserva"].ToString()) <= 0)
                    {
                        TolObs = 0;
                    }
                    else
                    {
                        TolObs = Convert.ToDouble(TabUrgencias["TolObserva"]);
                    }
                }

                TabUrgencias.Close();
                TabUrgencias = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlRNacidos = "SELECT COUNT(CodDigita) AS TolNacido ";
                SqlRNacidos += "FROM [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS]";
                SqlRNacidos += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlRNacidos += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabRNacidos = Conexion.SQLDataReader(SqlRNacidos);

                if (TabRNacidos.HasRows == false)
                {
                    TolReN = 0;
                }
                else
                {
                    TabRNacidos.Read();
                    if (Convert.ToInt32(TabRNacidos["TolNacido"].ToString()) <= 0)
                    {
                        TolReN = 0;
                    }
                    else
                    {
                        TolReN = Convert.ToDouble(TabRNacidos["TolNacido"]);
                    }
                }

                TabRNacidos.Close();
                TabRNacidos = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlConsultas = "SELECT COUNT(CodDigita) AS TolConsultas, SUM(ValorConsul) AS ValtolConsul ";
                SqlConsultas += "FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS]";
                SqlConsultas += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlConsultas += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabConsultas = Conexion.SQLDataReader(SqlConsultas);

                if (TabConsultas.HasRows == false)
                {
                    TolConsul = 0;
                }
                else
                {
                    TabConsultas.Read();
                    if (Convert.ToInt32(TabConsultas["TolConsultas"].ToString()) <= 0)
                    {
                        TolConsul = 0;
                    }
                    else
                    {
                        TolConsul = Convert.ToDouble(TabConsultas["TolConsultas"]);
                    }
                }

                TabConsultas.Close();
                TabConsultas = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlMedica = "SELECT COUNT(CodDigita) AS TolMedicamentos, SUM(ValorTotal) AS ValtolMedi ";
                SqlMedica += "FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] ";
                SqlMedica += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlMedica += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabMedica = Conexion.SQLDataReader(SqlMedica);

                if (TabMedica.HasRows == false)
                {
                    TolMedi = 0;
                }
                else
                {
                    TabMedica.Read();
                    if (Convert.ToInt32(TabMedica["TolMedicamentos"].ToString()) <= 0)
                    {
                        TolMedi = 0;
                    }
                    else
                    {
                        TolMedi = Convert.ToDouble(TabMedica["TolMedicamentos"]);
                    }
                }

                TabMedica.Close();
                TabMedica = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlProcedimientos = "SELECT COUNT(CodDigita) AS TolProcedimientos, SUM(ValorProce) AS ValtolProce ";
                SqlProcedimientos += "FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] ";
                SqlProcedimientos += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlProcedimientos += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabProcedimientos = Conexion.SQLDataReader(SqlProcedimientos);

                if (TabProcedimientos.HasRows == false)
                {
                    TolProce = 0;
                }
                else
                {
                    TabProcedimientos.Read();
                    if (Convert.ToInt32(TabProcedimientos["TolProcedimientos"].ToString()) <= 0)
                    {
                        TolProce = 0;
                    }
                    else
                    {
                        TolProce = Convert.ToDouble(TabProcedimientos["TolProcedimientos"]);
                    }
                }

                TabProcedimientos.Close();
                TabProcedimientos = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlOtrosServi = "SELECT COUNT(CodDigita) AS TolOtrosSer, SUM(ValorTotal) AS ValtolOtros ";
                SqlOtrosServi += "FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] ";
                SqlOtrosServi += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlOtrosServi += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabOtrosServi = Conexion.SQLDataReader(SqlOtrosServi);

                if (TabOtrosServi.HasRows == false)
                {
                    TolOtrosSer = 0;
                }

                else
                {
                    TabOtrosServi.Read();
                    if (Convert.ToInt32(TabOtrosServi["TolOtrosSer"].ToString()) <= 0)
                    {
                        TolOtrosSer = 0;
                    }
                    else
                    {
                        TolOtrosSer = Convert.ToDouble(TabOtrosServi["TolOtrosSer"]);
                    }
                }

                TabOtrosServi.Close();

                TabOtrosServi = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                //'Elimine los datos de la tabla temporal

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS]  WHERE CodDigita = '" + UsSel + "' AND CodEnti = '" + Coenti01 + "'";
                Boolean EstadoDelete = Conexion.SQLDelete(Utils.SqlDatos);

                if (EstadoDelete == false)
                {
                    Utils.Informa = "El proceso de validación de este módulo ";
                    Utils.Informa += "no se pudo eliminar los datos temporales de errores Rips ";
                    Utils.Informa += "usuarios de la entidad seleccionada.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Utils.Informa = "¿Usted desea validar los datos de los RIPS ";
                Utils.Informa += "previamente seleccionados a la entidad ";
                Utils.Informa += NEnti + "?";
                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Respuesta == DialogResult.Yes)
                {
                    //Validamos el del usuarios
                    FunUs = ValidarUsuarios(Coenti02, TUReg, TolUsa, UsSel);

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
                            break;
                        default:
                            if (TolFac <= 0)
                            {
                                Utils.Informa = "El proceso de validación de este módulo no se";
                                Utils.Informa += "puede realizar mientras no se seleccione los ";
                                Utils.Informa += "facturas de los procedimientos realizados.";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                    }
                }

                //Empiece a validar las facturas

                FunFac = ValidarFacturas(Coenti02, TolFac, UsSel);

                if (FunFac == -1)
                {
                    return;
                }

                //Comience a validar cada uno de los archivos de prestaciones de servicios


                if (TolConsul > 0)
                {
                    //Validar el de consultas
                    FunCon = ValidaConsultas(Coenti02, AcRe, TolConsul, UsSel);
                }


                if (TolHos > 0)
                {
                    //'Validar el de hospitalizaciones
                    FunHos = ValidarHospi(Coenti02, TolObs, UsSel);
                }

                if (TolMedi > 0)
                {
                    //'Validar el de medicamentos
                    FunMedi = ValidarMedica(Coenti02, TolMedi, UsSel);
                }

                if (TolObs > 0)
                {
                    //'Validar el de observación de urgencias
                    FunObs = ValidarObserva(Coenti02, TolObs, UsSel);
                }


                if (TolOtrosSer > 0)
                {
                    //'Validar el de otros servicios
                    FunOtros = ValidarOtros(Coenti02, TolOtrosSer, UsSel);
                }

                if (TolReN > 0)
                {
                    //'Validar el de recien nacidos
                    FunReN = ValidarReNan(Coenti02, TolReN, UsSel);
                }

                if (TolProce > 0)
                {
                    //'Validar el de procedimientos
                    FunProce = ValidarProcedi(Coenti02, TolObs, UsSel);
                }

                Utils.SqlDatos = "SELECT COUNT(CodEnti) AS CuenCodEnti FROM [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] WHERE CodEnti = '" + Coenti01 + "' ";

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

                        Utils.SqlDatos = "SELECT [CodDigita],[TipARchi],[TipDocu],[NumDocu],[CodEnti],[FacturaN],[Observa1] FROM [DARIPSXPSQL].[dbo].[Datos temporal errores RIPS] WHERE CodEnti = '" + Coenti01 + "' ORDER BY NumDocu ASC  ";

                        Utils.infNombreInforme = "InfReporErroresRips";

                        Utils.CarAdmin = Coenti01;

                        Reportes.FrmInfErroresRips frm = new Reportes.FrmInfErroresRips();
                        frm.ShowDialog();

                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al validar las facturas seleccionadas " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                string Coenti02 = null, Coenti01 = null, NEnti = null, TDE = null, NCC = null, CMin2 = null, EnRutaRips = null, NRemEnvi = null, UsGra = null, FunCon = null, CinRips = null, Citer = null, Para04 = null;
                string Mj = null, UsSel = null, ND = null, CodRegEsp = null, TD = null, Msj = null, StrinConse = null, NF = null, NEenti = null;

                string Sqlsuarios, SqlFacTemp, SqlHospiTemp, SqlUrgenTemp, SqlRNaciTemp, SqlConsultas, SqlMedTemp, SqlProceTemp, SqlOtrosTemp, SqlFacturas, SqlsuaTemp, SqlMaestro, SqlContadores, SqlAPEB, SqlHospitalizados, SqlUrgencias, SqlRNacidos, SqlMedica, SqlProcedimientos, SqlOtrosServi;


                int SinoX = 0, MSino = 0, Siga = 0, CantUsExpor = 0, CantiFacEXpor = 0, SinoVer = 0, FunElim = 0, VR = 0, FunAudi = 0, RegExp = 0;

                double TolFac = 0, TolCon = 0, ConRemi = 0, TolMedi = 0, TolOtrosSer = 0, TolObs = 0, TolPro = 0, TolHos = 0, TolUsa = 0, TolOtr = 0, TolConsul = 0, TolOtros = 0, TolReN = 0, TolProce = 0;

                object CR, CMin1, VarRetor;

                string Para01 = DateInicial.Value.ToString("yyyy-MM-dd");
                string Para02 = DateFinal.Value.ToString("yyyy-MM-dd");

                string Date = DateTime.Now.ToString("yyyy-MM-dd");

                Boolean ExpUsar = false;


                Utils.Titulo01 = "Control para exportar RIPS";


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
                        NEnti = sqlDataReader["NP"].ToString();
                        TDE = sqlDataReader["TipoDocu"].ToString();
                        NCC = sqlDataReader["NumDocu"].ToString();
                        CodRegEsp = sqlDataReader["RegimenAdmin"].ToString();

                    }
                    sqlDataReader.Close();
                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
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



                //'Proceda a contar cuantos usuarios hay para exportar, y cuantas facturas


                Sqlsuarios = "SELECT COUNT(CodDigita) AS TolUsuarios ";
                Sqlsuarios += "FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS]";
                Sqlsuarios += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                Sqlsuarios += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabUsuarios = Conexion.SQLDataReader(Sqlsuarios);

                if (TabUsuarios.HasRows == false)
                {
                    TolUsa = 0;
                }
                else
                {
                    TabUsuarios.Read();

                    if (Convert.ToInt32(TabUsuarios["TolUsuarios"].ToString()) <= 0)
                    {
                        TolUsa = 0;
                    }
                    else
                    {
                        TolUsa = Convert.ToDouble(TabUsuarios["TolUsuarios"].ToString());
                    }
                }

                TabUsuarios.Close();
                TabUsuarios = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                if (TolUsa <= 0)
                {
                    Utils.Informa = "El proceso de validación de este módulo no se";
                    Utils.Informa += "puede realizar mientras no se seleccione los ";
                    Utils.Informa += "usuarios de la entidad seleccionada.";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                SqlFacturas = "SELECT COUNT(CodDigita) AS TolFacturas, SUM(Copago) AS ValCopaFac, SUM(ValorNeto) AS ValNetoFac ";
                SqlFacturas += "FROM [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] ";
                SqlFacturas += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlFacturas += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabFacturas = Conexion.SQLDataReader(SqlFacturas);


                if (TabFacturas.HasRows == false)
                {
                    TolFac = 0;
                }
                else
                {
                    TabFacturas.Read();
                    if (Convert.ToInt32(TabFacturas["TolFacturas"].ToString()) <= 0)
                    {
                        TolFac = 0;
                    }
                    else
                    {
                        TolFac = Convert.ToDouble(TabFacturas["TolFacturas"]);
                    }
                }

                if (TolFac <= 0)
                {
                    Utils.Informa = "Lo siento pero no existen facturas para";
                    Utils.Informa += "realizar el RIPS a la entidad o convenio " + CR;
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TabFacturas.Close();
                TabFacturas = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                //Revisamos si el código de la EPS existe en la base de datos RIPS

                SqlAPEB = "SELECT [Datos administradoras de planes].CodAdmin, [Datos administradoras de planes].CodInterno " +
                        "FROM [DARIPSXPSQL].[dbo].[Datos administradoras de planes] " +
                        "WHERE ((([Datos administradoras de planes].CodAdmin) = '" + CR + "' )) " +
                        "ORDER BY [Datos administradoras de planes].CodAdmin;";

                SqlDataReader TabAPEB = Conexion.SQLDataReader(SqlAPEB);

                if (TabAPEB.HasRows == false)
                {
                    Utils.Informa = "Lo siento pero el código SGSSS";
                    Utils.Informa += "pertenece a ninguna administradora de planes en SEDAS-RIPS Estandar. " + CR;
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    TabAPEB.Read();
                    CinRips = TabAPEB["CodInterno"].ToString();
                    Siga = 1;
                }

                TabAPEB.Close();

                TabAPEB = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlHospitalizados = "SELECT COUNT(CodDigita) AS TolHospi ";
                SqlHospitalizados += "FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] ";
                SqlHospitalizados += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlHospitalizados += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabHospitalizados = Conexion.SQLDataReader(SqlHospitalizados);

                if (TabHospitalizados.HasRows == false)
                {
                    TolHos = 0;
                }
                else
                {
                    TabHospitalizados.Read();
                    if (Convert.ToInt32(TabHospitalizados["TolHospi"].ToString()) <= 0)
                    {
                        TolHos = 0;
                    }
                    else
                    {
                        TolHos = Convert.ToDouble(TabHospitalizados["TolHospi"].ToString());
                    }
                }

                TabHospitalizados.Close();

                TabHospitalizados = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlUrgencias = "SELECT COUNT(CodDigita) AS TolObserva ";
                SqlUrgencias += "FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] ";
                SqlUrgencias += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlUrgencias += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabUrgencias = Conexion.SQLDataReader(SqlUrgencias);

                if (TabUrgencias.HasRows == false)
                {
                    TolObs = 0;
                }
                else
                {
                    TabUrgencias.Read();
                    if (Convert.ToInt32(TabUrgencias["TolObserva"].ToString()) <= 0)
                    {
                        TolObs = 0;
                    }
                    else
                    {
                        TolObs = Convert.ToDouble(TabUrgencias["TolObserva"]);
                    }
                }

                TabUrgencias.Close();
                TabUrgencias = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlRNacidos = "SELECT COUNT(CodDigita) AS TolNacido ";
                SqlRNacidos += "FROM [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS]";
                SqlRNacidos += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlRNacidos += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabRNacidos = Conexion.SQLDataReader(SqlRNacidos);

                if (TabRNacidos.HasRows == false)
                {
                    TolReN = 0;
                }
                else
                {
                    TabRNacidos.Read();
                    if (Convert.ToInt32(TabRNacidos["TolNacido"].ToString()) <= 0)
                    {
                        TolReN = 0;
                    }
                    else
                    {
                        TolReN = Convert.ToDouble(TabRNacidos["TolNacido"]);
                    }
                }

                TabRNacidos.Close();
                TabRNacidos = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlConsultas = "SELECT COUNT(CodDigita) AS TolConsultas, SUM(ValorConsul) AS ValtolConsul ";
                SqlConsultas += "FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS]";
                SqlConsultas += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlConsultas += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabConsultas = Conexion.SQLDataReader(SqlConsultas);

                if (TabConsultas.HasRows == false)
                {
                    TolConsul = 0;
                }
                else
                {
                    TabConsultas.Read();
                    if (Convert.ToInt32(TabConsultas["TolConsultas"].ToString()) <= 0)
                    {
                        TolConsul = 0;
                    }
                    else
                    {
                        TolConsul = Convert.ToDouble(TabConsultas["TolConsultas"]);
                    }
                }

                TabConsultas.Close();
                TabConsultas = null;

                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlMedica = "SELECT COUNT(CodDigita) AS TolMedicamentos, SUM(ValorTotal) AS ValtolMedi ";
                SqlMedica += "FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] ";
                SqlMedica += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlMedica += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabMedica = Conexion.SQLDataReader(SqlMedica);

                if (TabMedica.HasRows == false)
                {
                    TolMedi = 0;
                }
                else
                {
                    TabMedica.Read();
                    if (Convert.ToInt32(TabMedica["TolMedicamentos"].ToString()) <= 0)
                    {
                        TolMedi = 0;
                    }
                    else
                    {
                        TolMedi = Convert.ToDouble(TabMedica["TolMedicamentos"]);
                    }
                }

                TabMedica.Close();
                TabMedica = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlProcedimientos = "SELECT COUNT(CodDigita) AS TolProcedimientos, SUM(ValorProce) AS ValtolProce ";
                SqlProcedimientos += "FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] ";
                SqlProcedimientos += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlProcedimientos += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabProcedimientos = Conexion.SQLDataReader(SqlProcedimientos);

                if (TabProcedimientos.HasRows == false)
                {
                    TolProce = 0;
                }
                else
                {
                    TabProcedimientos.Read();
                    if (Convert.ToInt32(TabProcedimientos["TolProcedimientos"].ToString()) <= 0)
                    {
                        TolProce = 0;
                    }
                    else
                    {
                        TolProce = Convert.ToDouble(TabProcedimientos["TolProcedimientos"]);
                    }
                }

                TabProcedimientos.Close();
                TabProcedimientos = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                SqlOtrosServi = "SELECT COUNT(CodDigita) AS TolOtrosSer, SUM(ValorTotal) AS ValtolOtros ";
                SqlOtrosServi += "FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] ";
                SqlOtrosServi += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlOtrosServi += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabOtrosServi = Conexion.SQLDataReader(SqlOtrosServi);

                if (TabOtrosServi.HasRows == false)
                {
                    TolOtrosSer = 0;
                }

                else
                {
                    TabOtrosServi.Read();
                    if (Convert.ToInt32(TabOtrosServi["TolOtrosSer"].ToString()) <= 0)
                    {
                        TolOtrosSer = 0;
                    }
                    else
                    {
                        TolOtrosSer = Convert.ToDouble(TabOtrosServi["TolOtrosSer"]);
                    }
                }

                TabOtrosServi.Close();
                TabOtrosServi = null;
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                Utils.Informa = "¿Usted desea realizar la exportación de los ";
                Utils.Informa += "archivos RIPS de la entidad o convenio ";
                Utils.Informa += NEnti;
                Utils.Informa += "al programa SEDAS-RIPS.?";
                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Respuesta == DialogResult.Yes)
                {

                    //  'Revisa si el código de la entidad está relacionada con alguna entidad

                    UsGra = lblCodigoUser.Text;


                    SqlMaestro = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo maestro] " +
                                    "WHERE (([Datos archivo maestro].CodAdmin)= '" + CR + "') And " +
                                    "(([Datos archivo maestro].CerraRemi) = 0 );";


                    SqlDataReader TabMaestro = Conexion.SQLDataReader(SqlMaestro);

                    if (TabMaestro.HasRows == false)
                    {
                        Utils.Informa = "¿Acepta que el sistema cree automaticamente" + "\r";
                        Utils.Informa += "el archivo maestro de la remisión de envío?" + "\r";

                        var ExporArchivo = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (ExporArchivo == DialogResult.Yes)
                        {
                            SqlContadores = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos contadores sedas];";

                            SqlDataReader TabContadores = Conexion.SQLDataReader(SqlContadores);

                            if (TabContadores.HasRows == false)
                            {

                                Utils.Informa = "Error de administración de datos. ";
                                Utils.Informa += "El registro único de contadores " + "\r";
                                Utils.Informa += "no fué posible encontrarlo. ";
                                Siga = 0;
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                TabContadores.Read();

                                if (Convert.ToInt32(TabContadores["UlConRemi"].ToString()) == 0)
                                {
                                    //no existe remisiones perdidas
                                    ConRemi = Convert.ToInt32(TabContadores["ConsRemi"].ToString());
                                    ConRemi += 1;

                                    //Procesa a actualizar el campo de concecutivos

                                    Date = DateTime.Now.ToString("yyyy-MM-dd");

                                    Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos contadores sedas] SET [ConsRemi] = '" + ConRemi + "', [UsarRemi] = '" + UsGra + "', FecRemi = '" + Date + "'";

                                    Boolean EstaActConce = Conexion.SQLUpdate(Utils.SqlDatos);

                                    if (EstaActConce == false)
                                    {
                                        Utils.Informa = "Error de administración de datos. ";
                                        Utils.Informa += "al actualizar el concecutivo" + "\r";
                                        Siga = 0;
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                }
                                else
                                {
                                    ConRemi = Convert.ToDouble(TabContadores["UlConRemi"].ToString());

                                    Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos contadores sedas] SET [UlConRemi] = '" + 0 + "'";

                                    Boolean EstaActConce = Conexion.SQLUpdate(Utils.SqlDatos);

                                    if (EstaActConce == false)
                                    {
                                        Utils.Informa = "Error de administración de datos. ";
                                        Utils.Informa += "al actualizar el campo UlConRemi " + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                } // final   if (Convert.ToInt32(TabContadores["UlConRemi"].ToString()) == 0)

                                //Devuelva el campo convertido en string

                                StrinConse = "";

                                switch (ConRemi)
                                {
                                    case double estado when ConRemi >= 1 && ConRemi <= 9:
                                        StrinConse = "00000" + ConRemi;
                                        break;
                                    case double estado when ConRemi >= 10 && ConRemi <= 99:
                                        StrinConse = "0000" + ConRemi;
                                        break;
                                    case double estado when ConRemi >= 100 && ConRemi <= 999:
                                        StrinConse = "000" + ConRemi;
                                        break;
                                    case double estado when ConRemi >= 1000 && ConRemi <= 9999:
                                        StrinConse = "00" + ConRemi;
                                        break;
                                    case double estado when ConRemi >= 10000 && ConRemi <= 99999:
                                        StrinConse = "0" + ConRemi;
                                        break;
                                    case double estado when ConRemi >= 100000 && ConRemi <= 999999:
                                        StrinConse = Convert.ToString(ConRemi);
                                        break;
                                    default:
                                        Utils.Informa = "El consecutivo de remisiones ha pasado el";
                                        Utils.Informa = Utils.Informa + "limite de seis (6) digitos, por lo tanto no";
                                        Utils.Informa = Utils.Informa + "se puede generar otra remisión hasta que el";
                                        Utils.Informa = Utils.Informa + "administrador del sistema amplie el rango.";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        StrinConse = "";
                                        Siga = 0;
                                        break;
                                }

                                if (StrinConse != "")
                                {
                                    //'Proceda a registrar la nueva remisi'on
                                    Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo maestro]" +
                                       "(" +
                                       "ConseArchivo," +
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
                                       "FecRegis" +
                                       ")" +
                                       "VALUES" +
                                       "(" +
                                       "'" + StrinConse + "'," +
                                       "'" + CinRips + "'," +
                                       "'" + lblCodMinSalud.Text + "'," +
                                       "'" + CR + "'," +
                                       "'" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd")  + "'," +
                                       "'" + lblNombreUser.Text + "'," +
                                       "'" + Convert.ToDateTime(Para01).ToString("yyyy-MM-dd") + "'," +
                                       "'" + Convert.ToDateTime(Para02).ToString("yyyy-MM-dd") + "'," +
                                       "'" + TolFac + "'," +
                                       "'" + txtTeleIPS.Text + "'," +
                                       "'" + CodRegEsp + "'," +
                                       "'" + 0 + "'," +
                                       "'" + 0 + "'," +
                                       "'" + 0 + "'," +
                                       "'" + UsGra + "'," +
                                       "'" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "'" +
                                       ")";

                                    Boolean RegistraRemision = Conexion.SqlInsert(Utils.SqlDatos);

                                    if(RegistraRemision == false)
                                    {
                                        Siga = 0;
                                    }

                                    NRemEnvi = StrinConse;

                                }  //Final StringConse != ""
                            } //Final TabContadores

                            TabContadores.Close();
                            TabContadores = null;

                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                        } //FInal pregunta

                    }
                    else
                    {
                        //existe una remision abierta
                        TabMaestro.Read();

                        NRemEnvi = TabMaestro["ConseArchivo"].ToString();
                        Int32 NumFacturasact = Convert.ToInt32(TabMaestro["NumFacturas"].ToString());
                        Utils.Informa = "El sistema ha encontrado abierta la remisión" + "\r";
                        Utils.Informa += "Numero " + NRemEnvi + " del código SGSSS " + "\r";
                        Utils.Informa += "¿Desea agregarle los datos seleccionados?.";
                        var agregar = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (agregar == DialogResult.Yes)
                        {
                          
                            string QUERY = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo maestro] " +
                                    "SET NumFacturas = (" + NumFacturasact + " + " + TolFac + ") WHERE [Datos archivo maestro].CodAdmin= '" + CR + "'";

                            Boolean EstadoAct = Conexion.SQLUpdate(QUERY);

                            if (EstadoAct)
                            {
                                Siga = 1;
                            }
                        }
                        else
                        {
                            Siga = 0;
                        }
                    }//Final tab maestro

                    TabMaestro.Close();
                    TabMaestro = null;
                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                    if (Siga == 1)
                    {
                        VR = 0;
                        SqlsuaTemp = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS]  " +
                       "WHERE ([Datos temporal usuarios RIPS].[CodDigita] = N'" + UsSel + "') AND " +
                       "([Datos temporal usuarios RIPS].[NumRemi] = N'" + Coenti02 + "')";

                        SqlDataReader TabUsuaTemp;
                        //    SqlDataReader TabUsuaTemp = Conexion.SQLDataReader(SqlsuaTemp);

                        using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                        {
                            SqlCommand command2 = new SqlCommand(SqlsuaTemp, connection2);
                            command2.Connection.Open();
                            TabUsuaTemp = command2.ExecuteReader();


                            if (TabUsuaTemp.HasRows == false)
                            {
                                Utils.Informa = "Lo siento pero en la tabal de registro ";
                                Utils.Informa = Utils.Informa + "temporal de usuarios no hay registros para ";
                                Utils.Informa = Utils.Informa + "para empezar el proceso de exportación.";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Siga = 0;
                            }
                            else
                            {
                                VR = 0;
                                while (TabUsuaTemp.Read())
                                {
                                    TD = TabUsuaTemp["TipoDocum"].ToString();
                                    ND = TabUsuaTemp["NumDocum"].ToString();


                                    if (TabUsuaTemp["TipUsuario"].ToString() == "" || Convert.ToInt32(TabUsuaTemp["TipUsuario"].ToString()) < 1 || Convert.ToInt32(TabUsuaTemp["TipUsuario"].ToString()) > 8)
                                    {
                                        Utils.Informa = "Lo siento pero el usuario identificado ";
                                        Utils.Informa = Utils.Informa + "con el documento " + TD + ":" + ND + " ";
                                        Utils.Informa = Utils.Informa + "no tiene definido el tipo de usuario.";
                                        Utils.Informa = Utils.Informa + "en una de las cuenta de las facturas";
                                        Utils.Informa = Utils.Informa + "a exportar a nombre del mismo.";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        ExpUsar = false;
                                    }
                                    else
                                    {
                                        ExpUsar = true;


                                        Sqlsuarios = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo usuarios] " +
                                                    "WHERE (([Datos archivo usuarios].NumRemi)= '" + NRemEnvi + "') And " +
                                                    "(([Datos archivo usuarios].TipoDocum)= '" + TD + "' ) And " +
                                                    "(([Datos archivo usuarios].NumDocum)= '" + ND + "' );";



                                        TabUsuarios = null;

                                        //    TabUsuarios = Conexion.SQLDataReader(Sqlsuarios);

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Sqlsuarios, connection);
                                            command.Connection.Open();
                                            TabUsuarios = command.ExecuteReader();

                                            if (TabUsuarios.HasRows == false)
                                            {
                                                //Adicionelo
                                                //Active la suguiente rutina de error

                                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo usuarios]" +
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
                                                "'" + NRemEnvi + "'," +
                                                "'" + TD + "'," +
                                                "'" + ND + "'," +
                                                "'" + TabUsuaTemp["CodAdmin"].ToString() + "'," +
                                                "'" + TabUsuaTemp["TipUsuario"].ToString() + "'," +
                                                "'" + TabUsuaTemp["Apellido1"].ToString() + "'," +
                                                "'" + TabUsuaTemp["Apellido2"].ToString() + "'," +
                                                "'" + TabUsuaTemp["Nombre1"].ToString() + "'," +
                                                "'" + TabUsuaTemp["Nombre2"].ToString() + "'," +
                                                "'" + TabUsuaTemp["Edad"].ToString() + "'," +
                                                "'" + TabUsuaTemp["EdadMedi"].ToString() + "'," +
                                                "'" + TabUsuaTemp["Sexo"].ToString() + "'," +
                                                "'" + TabUsuaTemp["CodDpto"].ToString() + "'," +
                                                "'" + TabUsuaTemp["CodMuni"].ToString() + "'," +
                                                "'" + TabUsuaTemp["ZonaResi"].ToString() + "'" +
                                                ")";

                                                Boolean RegistrarArcUsuarios = Conexion.SqlInsert(Utils.SqlDatos);

                                                if (RegistrarArcUsuarios == false)
                                                {
                                                    Utils.Informa = "Lo siento pero no se pudo insertar el usuario ";
                                                    Utils.Informa = Utils.Informa + "con el documento " + TD + ":" + ND + " ";
                                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }

                                            }
                                            else
                                            {
                                                //Modifique algunos datos
                                                Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo usuarios] SET " +
                                               "CodAdmin = '" + TabUsuaTemp["CodAdmin"].ToString() + "'," +
                                               "TipUsuario = '" + TabUsuaTemp["TipUsuario"].ToString() + "'," +
                                               "Apellido1 = '" + TabUsuaTemp["Apellido1"].ToString() + "'," +
                                               "Apellido2 = '" + TabUsuaTemp["Apellido2"].ToString() + "'," +
                                               "Nombre1 = '" + TabUsuaTemp["Nombre1"].ToString() + "'," +
                                               "Nombre2 = '" + TabUsuaTemp["Nombre2"].ToString() + "'," +
                                               "Edad = '" + TabUsuaTemp["Edad"].ToString() + "'," +
                                               "EdadMedi = '" + TabUsuaTemp["EdadMedi"].ToString() + "'," +
                                               "Sexo = '" + TabUsuaTemp["Sexo"].ToString() + "'," +
                                               "CodDpto = '" + TabUsuaTemp["CodDpto"].ToString() + "'," +
                                               "CodMuni = '" + TabUsuaTemp["CodMuni"].ToString() + "'," +
                                               "ZonaResi = '" + TabUsuaTemp["ZonaResi"].ToString() + "' " +
                                               "WHERE NumRemi = '" + NRemEnvi + "' AND TipoDocum = '" + TD + "' AND NumDocum = '" + ND + "' ";

                                                Boolean ActualizarArcUsuarios = Conexion.SQLUpdate(Utils.SqlDatos);

                                                if (ActualizarArcUsuarios == false)
                                                {
                                                    Utils.Informa = "Lo siento pero no se pudo actualizar el usuario ";
                                                    Utils.Informa = Utils.Informa + "con el documento " + TD + ":" + ND + " ";
                                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }


                                            }//Final Tab Usuarios

                                        } //Fianl USing

                                        TabUsuarios.Close();

                                    }//Final TabUsuaTemp["TipUsuario"].ToString() == "" 


                                    VR += 1;

                                    RegExp += 1;

                                } //FIN WHILE


                                CantUsExpor = RegExp;

                                //Edite el registro como exportado

                                if (ExpUsar == true)
                                {
                                    Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] SET [Datos temporal usuarios RIPS].[Exportado] = 1 WHERE ([Datos temporal usuarios RIPS].[CodDigita] = N'" + UsSel + "') AND " +
                                    "([Datos temporal usuarios RIPS].[NumRemi] = N'" + Coenti02 + "')";

                                    Boolean ActComoExportado = Conexion.SQLUpdate(Utils.SqlDatos);

                                    if (ActComoExportado == false)
                                    {
                                        Utils.Informa = Utils.Informa + "Lo siento, pero de los " + TolUsa + " usuarios ";
                                        Utils.Informa = Utils.Informa + "a exportar, no se pudieron copiar todos";
                                        Utils.Informa = Utils.Informa + "Por tanto no se puede continuar.";
                                        Utils.Informa = Utils.Informa + "¿Quiere saber cuales son esos usuarios?";
                                        var verUsua = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                        if (verUsua == DialogResult.Yes)
                                        {
                                            //Muestre el informe 

                                            string data = "SELECT [Datos temporal usuarios RIPS].CodDigita, [Datos temporal usuarios RIPS].NumRemi, [Datos temporal usuarios RIPS].CodAdmin, [Datos temporal usuarios RIPS].TipoDocum, [Datos temporal usuarios RIPS].NumDocum, [Datos temporal usuarios RIPS].TipUsuario, [Datos temporal usuarios RIPS].Apellido1, [Datos temporal usuarios RIPS].Apellido2, [Datos temporal usuarios RIPS].Nombre1, [Datos temporal usuarios RIPS].Nombre2, [Datos temporal usuarios RIPS].Edad, [Datos temporal usuarios RIPS].EdadMedi, [Datos temporal usuarios RIPS].Sexo, [Datos temporal usuarios RIPS].CodDpto, [Datos temporal usuarios RIPS].CodMuni, [Datos temporal usuarios RIPS].ZonaResi, Trim([Datos empresas y terceros].[NomAdmin] + ' ' + [Datos empresas y terceros].[ProgrAmin]) AS NoAdmin, [Datos empresas y terceros].NomPlan " +
                                                        " FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] INNER JOIN [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] ON [Datos empresas y terceros].CarAdmin = [Datos temporal usuarios RIPS].NumRemi " +
                                                        " WHERE [Datos empresas y terceros].[CodDigita] = '" + UsSel + "' AND [Datos empresas y terceros].[NumRemi] = '" + Coenti02 + "' AND [Datos empresas y terceros].[Exportado] = 0  " +
                                                        " ORDER BY [Datos temporal usuarios RIPS].TipoDocum, [Datos temporal usuarios RIPS].NumDocum; ";

                                            Utils.SqlDatos = data;

                                            Utils.infNombreInforme = "InfReporUserPorRemision";

                                            Reportes.FrmInfUsuariosRemi frm = new Reportes.FrmInfUsuariosRemi();

                                            frm.ShowDialog();

                                        }

                                        Siga = 0;
                                    }
                                    else
                                    {
                                        Siga = 1;
                                    }

                                }

                            } //TabUsuaTemp 

                        } //fIN usING

                        TabUsuaTemp.Close();
                        TabUsuaTemp = null;


                        if (Siga == 1)
                        {
                            SqlFacTemp = "SELECT * " +
                                     "FROM [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] " +
                                     "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                     "(NumRemi = N'" + Coenti02 + "')";

                            SqlDataReader TabFacTemp = Conexion.SQLDataReader(SqlFacTemp);



                            if (TabFacTemp.HasRows == false)
                            {
                                //Dificilmente entra por aquí porque y a se evaluo arriba
                                Siga = 0;
                            }
                            else
                            {
                                VR = 0;
                                while (TabFacTemp.Read())
                                {
                                    NF = TabFacTemp["NumFactur"].ToString();

                                    SqlFacturas = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] " +
                                                 "WHERE (([Datos archivo de transacciones].NumRemi)= '" + NRemEnvi + "') And " +
                                                 "(([Datos archivo de transacciones].NumFactur)= '" + NF + "' ); ";

                                    //TabFacturas = Conexion.SQLDataReader(SqlFacturas);


                                    TabFacturas = null;

                                    //    TabUsuarios = Conexion.SQLDataReader(Sqlsuarios);

                                    using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                    {
                                        SqlCommand command = new SqlCommand(SqlFacturas, connection);
                                        command.Connection.Open();
                                        TabFacturas = command.ExecuteReader();

                                        if (TabFacturas.HasRows == false)
                                        {
                                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] " +
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
                                            "'" + NRemEnvi + "'," +
                                            "'" + TabFacTemp["CodIps"].ToString() + "'," +
                                            "'" + TabFacTemp["RazonSocial"].ToString() + "'," +
                                            "'" + TabFacTemp["TipIdenti"].ToString() + "'," +
                                            "'" + TabFacTemp["NumIdenti"].ToString() + "'," +
                                            "'" + NF + "'," +
                                            "'" + Convert.ToDateTime(TabFacTemp["FecFactur"]).ToString("yyyy-MM-dd") + "'," +
                                            "'" + Convert.ToDateTime(TabFacTemp["FecInicio"]).ToString("yyyy-MM-dd") + "'," +
                                            "'" + Convert.ToDateTime(TabFacTemp["FecFinal"]).ToString("yyyy-MM-dd") + "'," +
                                            "'" + TabFacTemp["CodAdmin"].ToString() + "'," +
                                            "'" + TabFacTemp["NomAdmin"].ToString() + "'," +
                                            "'" + TabFacTemp["NumContra"].ToString() + "'," +
                                            "'" + TabFacTemp["PlanBene"].ToString() + "'," +
                                            "'" + TabFacTemp["NumPoli"].ToString() + "'," +
                                            "'" + TabFacTemp["Copago"].ToString() + "'," +
                                            "'" + TabFacTemp["ValorComi"].ToString() + "'," +
                                            "'" + TabFacTemp["ValorDes"].ToString() + "'," +
                                            "'" + TabFacTemp["ValorNeto"].ToString() + "'" +
                                            ")";

                                            Boolean RegistrarArcTransacciones = Conexion.SqlInsert(Utils.SqlDatos);

                                        }
                                        else
                                        {
                                            //Modifique algunos datos
                                            Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] SET " +
                                           "CodIps = '" + TabFacTemp["CodIps"].ToString() + "'," +
                                           "RazonSocial = '" + TabFacTemp["RazonSocial"].ToString() + "'," +
                                           "TipIdenti = '" + TabFacTemp["TipIdenti"].ToString() + "'," +
                                           "NumIdenti = '" + TabFacTemp["NumIdenti"].ToString() + "'," +
                                           "FecFactur = '" + Convert.ToDateTime(TabFacTemp["FecFactur"]).ToString("yyyy-MM-dd") + "'," +
                                           "FecInicio = '" + Convert.ToDateTime(TabFacTemp["FecInicio"]).ToString("yyyy-MM-dd") + "'," +
                                           "FecFinal = '" + Convert.ToDateTime(TabFacTemp["FecFinal"]).ToString("yyyy-MM-dd") + "'," +
                                           "CodAdmin = '" + TabFacTemp["CodAdmin"].ToString() + "'," +
                                           "NomAdmin = '" + TabFacTemp["NomAdmin"].ToString() + "'," +
                                           "NumContra = '" + TabFacTemp["NumContra"].ToString() + "'," +
                                           "PlanBene = '" + TabFacTemp["PlanBene"].ToString() + "'," +
                                           "NumPoli = '" + TabFacTemp["NumPoli"].ToString() + "'," +
                                           "Copago = '" + TabFacTemp["Copago"].ToString() + "'," +
                                           "ValorComi = '" + TabFacTemp["ValorComi"].ToString() + "'," +
                                           "ValorDes = '" + TabFacTemp["ValorDes"].ToString() + "'," +
                                           "ValorNeto = '" + TabFacTemp["ValorNeto"].ToString() + "' " +
                                           "WHERE [Datos archivo de transacciones].[NumRemi] = '" + NRemEnvi + "' AND [Datos archivo de transacciones].[NumFactur] =  '" + NF + "' ";

                                            Boolean ActualizarArcUsuarios = Conexion.SQLUpdate(Utils.SqlDatos);


                                        } //TabFacturas.HasRows == false)

                                        TabFacturas.Close();
                                        TabFacturas = null;

                                        VR += 1;

                                    }


                                }// FINAL WHILE TABFACB TEMP

                                CantiFacEXpor = VR;

                                Siga = 1;

                            } // Final (TabFacTemp.HasRows

                            TabFacTemp.Close();
                            TabFacTemp = null;

                            if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                        }//Final SIGA = 1

                        Mj = "";

                        if (Siga == 1)
                        {
                            if (TolHos > 0)
                            {
                                VR = 0;


                                SqlHospiTemp = "SELECT * " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS]" +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabHospiTemp = Conexion.SQLDataReader(SqlHospiTemp);

                                if (TabHospiTemp.HasRows == false)
                                {
                                    //No problem
                                }
                                else
                                {
                                    while (TabHospiTemp.Read())
                                    {
                                        //Simplemente adiciona los hospitalizados

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de hospitalizacion] " +
                                        "(" +
                                        "NumRemi," +
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
                                        "HorSalida" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabHospiTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabHospiTemp["CodIps"].ToString() + "'," +
                                        "'" + TabHospiTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabHospiTemp["NumDocum"].ToString() + "'," +
                                        "'" + TabHospiTemp["ViaDIngreso"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabHospiTemp["FecIngresa"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabHospiTemp["HorIngresa"].ToString() + "'," +
                                        "'" + TabHospiTemp["AutoriNum"].ToString() + "'," +
                                        "'" + TabHospiTemp["CausExter"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxPrincIngre"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxPrincEgre"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxRelacion1"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxRelacion2"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxRelacion3"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxComplica"].ToString() + "'," +
                                        "'" + TabHospiTemp["EstadoSal"].ToString() + "'," +
                                        "'" + TabHospiTemp["DxMuerte"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabHospiTemp["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabHospiTemp["HorSalida"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoHospitali = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }
                                    
                                } //TabHospiTemp.HasRows

                                TabHospiTemp.Close();

                                TabHospiTemp = null;

                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                Mj = Mj + "Cantidad de hospitalizaciones: " + VR + "\r";

                            }//Final de TolHol > 0

                            if (TolObs > 0)
                            {
                                VR = 0;


                                SqlUrgenTemp = "SELECT * " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS]" +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabUrgenTemp = Conexion.SQLDataReader(SqlUrgenTemp);

                                if (TabUrgenTemp.HasRows == false)
                                {
                                    TolObs = 0;
                                    //No problem
                                }
                                else
                                {
                                    while (TabUrgenTemp.Read())
                                    {
                                        //Simplemente adiciona los hospitalizados

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de observacion urgencias] " +
                                        "(" +
                                        "NumRemi," +
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
                                        "HorSalida" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabUrgenTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabUrgenTemp["CodIps"].ToString() + "'," +
                                        "'" + TabUrgenTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabUrgenTemp["NumDocum"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabUrgenTemp["FecIngresa"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabUrgenTemp["HorIngresa"].ToString() + "'," +
                                        "'" + TabUrgenTemp["AutoriNum"].ToString() + "'," +
                                        "'" + TabUrgenTemp["CausExter"].ToString() + "'," +
                                        "'" + TabUrgenTemp["DxPrincIngre"].ToString() + "'," +
                                        "'" + TabUrgenTemp["DxRelacion1"].ToString() + "'," +
                                        "'" + TabUrgenTemp["DxRelacion2"].ToString() + "'," +
                                        "'" + TabUrgenTemp["DxRelacion3"].ToString() + "'," +
                                        "'" + TabUrgenTemp["Destino"].ToString() + "'," +
                                        "'" + TabUrgenTemp["EstadoSal"].ToString() + "'," +
                                        "'" + TabUrgenTemp["DxMuerte"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabUrgenTemp["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabUrgenTemp["HorSalida"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoHospitali = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }

                           

                                } //TabUrgenTemp.HasRows

                                TabUrgenTemp.Close();
                                TabUrgenTemp = null;

                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                Mj = Mj + "Cantidad usuarios en observación: " + VR + "\r";

                            }//Final de TolObs > 0


                            if (TolReN > 0)
                            {
                                VR = 0;


                                SqlRNaciTemp = "SELECT * " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS]" +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabRNaciTemp = Conexion.SQLDataReader(SqlRNaciTemp);

                                if (TabRNaciTemp.HasRows == false)
                                {
                                    TolObs = 0;
                                    //No problem
                                }
                                else
                                {
                                    while (TabRNaciTemp.Read())
                                    {
                                        //Simplemente adiciona los hospitalizados

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de recien nacido] " +
                                        "(" +
                                        "NumRemi," +
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
                                        "DxRecien," +
                                        "DxMuerte," +
                                        "FecMuerte," +
                                        "HorMuerte" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabRNaciTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabRNaciTemp["CodIps"].ToString() + "'," +
                                        "'" + TabRNaciTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabRNaciTemp["NumDocum"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabRNaciTemp["FecNaci"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabRNaciTemp["HorIngresa"].ToString() + "'," +
                                        "'" + TabRNaciTemp["EdadGesta"].ToString() + "'," +
                                        "'" + TabRNaciTemp["ControlPrena"].ToString() + "'," +
                                        "'" + TabRNaciTemp["SexoRecien"].ToString() + "'," +
                                        "'" + TabRNaciTemp["PesoRecien"].ToString() + "'," +
                                        "'" + TabRNaciTemp["DxRecien"].ToString() + "'," +
                                        "'" + TabRNaciTemp["DxMuerte"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabRNaciTemp["FecMuerte"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabRNaciTemp["HorMuerte"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoRecienNacidos = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }

                                

                                } //TabRNaciTemp.HasRow
                          
                                TabRNaciTemp.Close();
                                TabRNaciTemp = null;
                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
                                Mj = Mj + "Cantidad de recien nacidos: " + VR + "\r";

                            }//Final de TolReN > 0



                            if (TolConsul > 0)
                            {
                                VR = 0;


                                SqlConsultas = "SELECT CodDigita, NumRemi, NumFactur, CodIPS, TipoDocum, NumDocum, FecConsul, AutoriNum, CodConsul, FinalConsul, CausExter, DxPrincipal, DxRelacion1, DxRelacion2, DxRelacion3, TipoDxPrin, ValorConsul, ValorCuota, ValorNeto, VezAno " +
                                "FROM DARIPSXPSQL.dbo.[Datos temporal consultas RIPS]" +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabConsuTemp = Conexion.SQLDataReader(SqlConsultas);

                                if (TabConsuTemp.HasRows == false)
                                {
                                    TolConsul = 0;
                                    //No problem
                                }
                                else
                                {
                                    while (TabConsuTemp.Read()) //error
                                    {
                                        //Simplemente adiciona los hospitalizados

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de consulta] " +
                                        "(" +
                                        "NumRemi," +
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
                                        "ValorNeto" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabConsuTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabConsuTemp["CodIps"].ToString() + "'," +
                                        "'" + TabConsuTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabConsuTemp["NumDocum"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabConsuTemp["FecConsul"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabConsuTemp["AutoriNum"].ToString() + "'," +
                                        "'" + TabConsuTemp["CodConsul"].ToString() + "'," +
                                        "'" + TabConsuTemp["FinalConsul"].ToString() + "'," +
                                        "'" + TabConsuTemp["CausExter"].ToString() + "'," +
                                        "'" + TabConsuTemp["DxPrincipal"].ToString() + "'," +
                                        "'" + TabConsuTemp["DxRelacion1"].ToString() + "'," +
                                        "'" + TabConsuTemp["DxRelacion2"].ToString() + "'," +
                                        "'" + TabConsuTemp["DxRelacion3"].ToString() + "'," +
                                        "'" + TabConsuTemp["TipoDxPrin"].ToString() + "'," +
                                        "'" + TabConsuTemp["ValorConsul"].ToString() + "'," +
                                        "'" + TabConsuTemp["ValorCuota"].ToString() + "'," +
                                        "'" + TabConsuTemp["ValorNeto"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoRecienNacidos = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }

                                  

                                } //TabConsuTemp.HasRows
                                TabConsuTemp.Close();
                                TabConsuTemp = null;
                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                Mj = Mj + "Cantidad de consultas: " + VR + "\r";

                            }//Final de TolConsul > 0

                            if (TolMedi > 0)
                            {
                                VR = 0;

                                //Copia los medicamento

                                SqlMedTemp = "SELECT * " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] " +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabMedTemp = Conexion.SQLDataReader(SqlMedTemp);

                                if (TabMedTemp.HasRows == false)
                                {
                                    TolMedi = 0;
                                    //No problem
                                }
                                else
                                {
                                    while (TabMedTemp.Read())
                                    {

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de medicamentos] " +
                                        "(" +
                                        "NumRemi," +
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
                                        "ValorTotal" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabMedTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabMedTemp["CodIps"].ToString() + "'," +
                                        "'" + TabMedTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabMedTemp["NumDocum"].ToString() + "'," +
                                        "'" + TabMedTemp["AutoriNum"].ToString() + "'," +
                                        "'" + TabMedTemp["CodMedica"].ToString() + "'," +
                                        "'" + TabMedTemp["TipoMedica"].ToString() + "'," +
                                        "'" + TabMedTemp["NomGenerico"].ToString() + "'," +
                                        "'" + TabMedTemp["FormaFarma"].ToString() + "'," +
                                        "'" + TabMedTemp["ConcenMedi"].ToString() + "'," +
                                        "'" + TabMedTemp["UniMedida"].ToString() + "'," +
                                        "'" + TabMedTemp["NumUnidad"].ToString() + "'," +
                                        "'" + TabMedTemp["ValorUnita"].ToString() + "'," +
                                        "'" + TabMedTemp["ValorTotal"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoMedicamentos = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }

                                    

                                } //TabMedTemp.HasRows
                                TabMedTemp.Close();
                                TabMedTemp = null;
                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                Mj = Mj + "Cantidad de medicamentos: " + VR + "\r";


                            }//Final de TolMedi > 0


                            if (TolProce > 0)
                            {
                                VR = 0;

                                //Copia los medicamento

                                SqlProceTemp = "SELECT * " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS]" +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabProceTemp = Conexion.SQLDataReader(SqlProceTemp);

                                if (TabProceTemp.HasRows == false)
                                {
                                    TolProce = 0;
                                    //No problem
                                }
                                else
                                {
                                    while (TabProceTemp.Read())
                                    {
                                        //Simplemente adiciona los hospitalizados

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de procedimientos] " +
                                        "(" +
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
                                        "ValorProce" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabProceTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabProceTemp["CodIps"].ToString() + "'," +
                                        "'" + TabProceTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabProceTemp["NumDocum"].ToString() + "'," +
                                        "'" + Convert.ToDateTime(TabProceTemp["FecProce"]).ToString("yyyy-MM-dd") + "'," +
                                        "'" + TabProceTemp["AutoriNum"].ToString() + "'," +
                                        "'" + TabProceTemp["CodProce"].ToString() + "'," +
                                        "'" + TabProceTemp["AmbitoReal"].ToString() + "'," +
                                        "'" + TabProceTemp["FinalProce"].ToString() + "'," +
                                        "'" + TabProceTemp["PersonAten"].ToString() + "'," +
                                        "'" + TabProceTemp["DxPrincipal"].ToString() + "'," +
                                        "'" + TabProceTemp["DxRelacion"].ToString() + "'," +
                                        "'" + TabProceTemp["Complicacion"].ToString() + "'," +
                                        "'" + TabProceTemp["RealiActo"].ToString() + "'," +
                                        "'" + TabProceTemp["ValorProce"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoProcedimientos = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }

                            

                                } //TabProceTemp.HasRows

                                TabProceTemp.Close();
                                TabProceTemp = null;

                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                Mj = Mj + "Cantidad de procedimientos: " + VR + "\r";

                            }//Final de TolProce > 0



                            if (TolOtros > 0)
                            {
                                VR = 0;

                                //Copia los medicamento

                                SqlOtrosTemp = "SELECT * " +
                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS]" +
                                "WHERE (CodDigita = N'" + UsSel + "') AND " +
                                "(NumRemi = N'" + Coenti02 + "')";

                                SqlDataReader TabOtrosTemp = Conexion.SQLDataReader(SqlOtrosTemp);

                                if (TabOtrosTemp.HasRows == false)
                                {
                                    TolOtros = 0;
                                    //No problem
                                }
                                else
                                {
                                    while (TabOtrosTemp.Read())
                                    {
                                        //Simplemente adiciona los hospitalizados

                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de otros servicios] " +
                                        "(" +
                                        "NumRemi," +
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
                                        "ValorTotal" +
                                        ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + NRemEnvi + "'," +
                                        "'" + TabOtrosTemp["NumFactur"].ToString() + "'," +
                                        "'" + TabOtrosTemp["CodIps"].ToString() + "'," +
                                        "'" + TabOtrosTemp["TipoDocum"].ToString() + "'," +
                                        "'" + TabOtrosTemp["NumDocum"].ToString() + "'," +
                                        "'" + TabOtrosTemp["AutoriNum"].ToString() + "'," +
                                        "'" + TabOtrosTemp["TipoServicio"].ToString() + "'," +
                                        "'" + TabOtrosTemp["CodiServi"].ToString() + "'," +
                                        "'" + TabOtrosTemp["NomServi"].ToString() + "'," +
                                        "'" + TabOtrosTemp["Cantidad"].ToString() + "'," +
                                        "'" + TabOtrosTemp["ValorUnita"].ToString() + "'," +
                                        "'" + TabOtrosTemp["ValorTotal"].ToString() + "'" +
                                        ")";

                                        Boolean RegistoOtrosServicios = Conexion.SqlInsert(Utils.SqlDatos);

                                        VR += 1;

                                    }

                              

                                } //TabOtrosTemp.HasRows
                                TabOtrosTemp.Close();
                                TabOtrosTemp = null;

                                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                Mj = Mj + "Cantidad de otros servicios: " + VR + "\r";

                            }//Final de TolOtros > 0

                            //Desmarque las facturas previamente selecciionadas

                        } // Final SIGA = 1


                        //Resumen de lo exportado
                        Utils.Informa = "Se han exportado los siguientes datos:" + "\r";
                        Utils.Informa += "Cantidad de usuarios: " + CantUsExpor + "\r";
                        Utils.Informa += "Cantidad de facturas:  " + CantiFacEXpor + "\r";
                        Utils.Informa += Mj;
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);


                    } // sIGA = 1

                    //'***************   Borre todos los datos temporales que crearron la bse de datos **********************


                    BorrarTempoRips(UsSel, Coenti01);

                }// Respuesta YES
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón exportar" + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }

        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            try
            {
                string Coenti02 = null, NomUsReal = null, Coenti01 = null, NEnti = null, TDE = null, NCC = null, Para01 = null;
                string Mj = null, UsSel = null, ND = null, CodRegEsp = null, TD = null, Msj = null, NEenti = null;
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

                FrmReportesRips FrmReportesRips = new FrmReportesRips();
                FrmReportesRips.Show();



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
        private void DataGridDestino_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (DataGridDestino.SelectedCells.Count != 0)
                {
                    string CodArt = DataGridDestino.SelectedCells[0].Value.ToString();
                    lblDestinoDocument.Text = CodArt;
                }
                else
                {
                    lblDestinoDocument.Text = null;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de actualizar la lista destino " + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Texbox Botones RadioButton
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
                            if (string.IsNullOrEmpty(txtCardinal.Text) == false)
                            {
                                IDContrato.Text = NumContratoID(this.txtCardinal.Text);
                            }

                        }
                        else
                        {
                            Utils.Titulo01 = "Control de ejecución";
                            Utils.Informa += "no encontro ninguna entidad por el numero de cardinal digitado" + "\r";
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
        private void btnUna_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.Titulo01 = "Control de ejecucion";
                string Coenti01 = null, NDO = null, SqlFacturas = null, DefiCuenta = null, CERips = null, Factura = null, CardiTer = null, UsSel = null, Para02 = null, Para03 = null, NContra = null;
                Boolean AnulFac;

                Coenti01 = cboNameEntidades.SelectedValue.ToString();
                CardiTer = cboNameEntidades.SelectedValue.ToString();


                if (string.IsNullOrWhiteSpace(Coenti01) || string.IsNullOrEmpty(Coenti01))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad a mostrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text) || string.IsNullOrEmpty(lblCodigoUser.Text))
                {
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "no es valido para seleccionar datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UsSel = lblCodigoUser.Text;
                Para02 = DateInicial.Value.ToString("yyyy-MM-dd");
                Para03 = DateFinal.Value.ToString("yyyy-MM-dd");

                if (string.IsNullOrWhiteSpace(DigeteOrigen.Text))
                {
                    Utils.Informa = "Lo siento pero usted no ha seleccionado o" + "\r";
                    Utils.Informa += "digitado el número del documento a agregar" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    NDO = DigeteOrigen.Text;
                    switch (Seleccion)
                    {
                        case 1:

                            Utils.Informa = "¿Usted desea agregar la factura número " + NDO + "\r";
                            Utils.Informa += ", al listado destino para RIP?" + "\r";

                            var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (Respuesta == DialogResult.Yes)
                            {

                                SqlFacturas = "SELECT [Datos de las facturas realizadas].Cartercero, [Datos de las facturas realizadas].ExpoRips, " +
                               "[Datos de las facturas realizadas].CodSele, [Datos de las facturas realizadas].AnuladaFac, " +
                               "[Datos cuentas de consumos].DefiCuenta " +
                               "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                               "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                               "WHERE (([Datos de las facturas realizadas].NumFactura) = '" + NDO + "')" +
                               "Order by NumFactura";

                                SqlDataReader TabFacturas = Conexion.SQLDataReader(SqlFacturas);

                                if (TabFacturas.HasRows)
                                {
                                    TabFacturas.Read();
                                    CERips = cboNameEntidades.SelectedValue.ToString();
                                    Factura = TabFacturas["Cartercero"].ToString();
                                    AnulFac = Convert.ToBoolean(TabFacturas["AnuladaFac"]);
                                    CERips = cboNameEntidades.SelectedValue.ToString();
                                    DefiCuenta = TabFacturas["DefiCuenta"].ToString();
                                    if (Factura == CERips)
                                    {
                                        if (AnulFac == false)
                                        {
                                            if (DefiCuenta != "0")
                                            {

                                                Utils.SqlDatos = "UPDATE [Datos de las facturas realizadas] " +
                                                                "SET ExpoRips = 1, CodSele = '" + lblCodigoUser.Text + "' " +
                                                                "FROM[Datos de las facturas realizadas] INNER JOIN[Datos cuentas de consumos] ON[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                                                                "WHERE[Datos de las facturas realizadas].NumFactura = '" + NDO + "' " +
                                                                "AND[Datos de las facturas realizadas].AnuladaFac = 0 " +
                                                                "AND[Datos cuentas de consumos].DefiCuenta <> N'0' ";

                                                Boolean ActDatos = Conexion.SQLUpdate(Utils.SqlDatos);

                                                if (ActDatos)
                                                {

                                                    Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                                    "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                                    "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                                    "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                                    "(([Datos de las facturas realizadas].ExpoRips) = 1) And " +
                                                    "(([Datos cuentas de consumos].DefiCuenta)<>'0') And " +
                                                    "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                                    "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                                    "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                                    "(([Datos de las facturas realizadas].AnuladaFac) = 0)) OR " +
                                                    "((([Datos de las facturas realizadas].NumFactura) = '" + NDO + "' ))";

                                                    DataSet sqlDataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                                                    if (sqlDataSet.Tables.Count > 0)
                                                    {

                                                        DataGridDestino.DataSource = null;
                                                        DataGridDestino.DataSource = sqlDataSet.Tables[0];

                                                    }

                                                    Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                                        "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                                        "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                                        "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                                        "(([Datos de las facturas realizadas].ExpoRips) = 0) And " +
                                                        "(([Datos cuentas de consumos].DefiCuenta)<>'0') And " +
                                                        "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                                        "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                                        "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                                        "(([Datos de las facturas realizadas].AnuladaFac) = 0))";

                                                    DataSet sqlDataSet2 = Conexion.SQLDataSet(Utils.SqlDatos);

                                                    if (sqlDataSet2.Tables.Count > 0)
                                                    {

                                                        DataGridFacturas.DataSource = null;
                                                        DataGridFacturas.DataSource = sqlDataSet2.Tables[0];

                                                    }

                                                    CalcularTotalFactura();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Utils.Informa = "Lo siento pero la factura No. " + NDO + "\r";
                                            Utils.Informa += "fue anulada." + "\r";
                                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Utils.Informa = "Lo siento pero el número de factura " + NDO + "\r";
                                        Utils.Informa += "no pertenece a la entidad" + cboNameEntidades.Text + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else
                                {
                                    Utils.Informa = "Lo siento pero el número de factura " + "\r";
                                    Utils.Informa += "no se encuentra en este sistema. " + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                TabFacturas.Close();
                            } //Final pregunta
                              //no quiere agregar
                            break;

                        case 2:
                            Utils.Informa = "¿Usted desea agregar todas las facturas" + "\r";
                            Utils.Informa += "de la cuenta de cobro número " + NDO + "\r";
                            Utils.Informa += "al listado destino para RIPS?" + "\r";
                            Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (Respuesta == DialogResult.Yes)
                            {
                                SqlFacturas = "SELECT [Datos de las facturas realizadas].Cartercero, [Datos de las facturas realizadas].ExpoRips, " +
                               "[Datos de las facturas realizadas].CodSele, [Datos de las facturas realizadas].AnuladaFac, " +
                               "[Datos cuentas de consumos].DefiCuenta " +
                               "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                               "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                               "WHERE (([Datos de las facturas realizadas].CuentaCobro) = '" + NDO + "' AND " +
                               "([Datos de las facturas realizadas].AnuladaFac) = 0 AND  " +
                               "([Datos cuentas de consumos].DefiCuenta) <> '0')";


                                SqlDataReader TabFacturas = Conexion.SQLDataReader(SqlFacturas);

                                if (TabFacturas.HasRows)
                                {

                                    Utils.SqlDatos = "UPDATE [Datos de las facturas realizadas] " +
                                                    "SET ExpoRips = 1, CodSele = '" + lblCodigoUser.Text + "' " +
                                                    "FROM[Datos de las facturas realizadas] INNER JOIN[Datos cuentas de consumos] ON[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                                                    "WHERE[Datos de las facturas realizadas].CuentaCobro = '" + NDO + "' " +
                                                    "AND[Datos de las facturas realizadas].AnuladaFac = 0 " +
                                                    "AND[Datos cuentas de consumos].DefiCuenta <> N'0' "; ;

                                    Boolean Act = Conexion.SQLUpdate(Utils.SqlDatos);

                                    if (Act)
                                    {
                                        Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                                   "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                                   "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                                   "WHERE ((([Datos de las facturas realizadas].CuentaCobro) = '" + NDO + "') And " +
                                                   "(([Datos de las facturas realizadas].ExpoRips) = 1) And " +
                                                   "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                                   "(([Datos de las facturas realizadas].AnuladaFac) = 0) AND " +
                                                   "(([Datos cuentas de consumos].DefiCuenta) <> '0')) " +
                                                   "ORDER BY [Datos de las facturas realizadas].NumFactura;";

                                        DataSet DataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                                        if (DataSet.Tables.Count > 0)
                                        {
                                            DataGridDestino.DataSource = null;

                                            DataGridDestino.DataSource = DataSet.Tables[0];

                                            if (Seleccion == 3 || Seleccion == 1)
                                            {
                                                DataGridFacturas.Rows.Clear();
                                            }
                                            else
                                            {
                                                DataGridFacturas.ClearSelection();
                                                DataGridFacturas.Rows.Remove(DataGridFacturas.CurrentRow);
                                            }
                                            CalcularTotalFactura();
                                            DigeteOrigen.Text = null;
                                        }

                                    }
                                }
                                else
                                {
                                    Utils.Informa = "Lo siento pero la cuenta de cobro " + NDO + "\r";
                                    Utils.Informa += "no tiene facturas relacionadas en este sistema." + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Question);
                                }
                                TabFacturas.Close();
                            }
                            break;
                        case 3:

                            NContra = cboContratos.SelectedValue.ToString();

                            Utils.Informa = "¿Usted desea agregar la factura número " + NDO + "\r";
                            Utils.Informa += ", al listado destino para RIPS?" + "\r";
                            Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (Respuesta == DialogResult.Yes)
                            {


                                SqlFacturas = "SELECT [Datos de las facturas realizadas].Cartercero, [Datos de las facturas realizadas].ExpoRips, " +
                               "[Datos de las facturas realizadas].CodSele, [Datos de las facturas realizadas].AnuladaFac, " +
                               "[Datos de las facturas realizadas].NumContra, [Datos cuentas de consumos].DefiCuenta " +
                               "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                               "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                               "WHERE (([Datos de las facturas realizadas].NumFactura) = '" + NDO + "') " +
                               "Order by NumFactura ";

                                SqlDataReader reader = Conexion.SQLDataReader(SqlFacturas);

                                if (reader.HasRows)
                                {
                                    CERips = cboNameEntidades.SelectedValue.ToString();
                                    reader.Read();
                                    string CarTercero, NumCotra;
                                    Boolean AnulFact;
                                    CarTercero = reader["Cartercero"].ToString();
                                    AnulFact = Convert.ToBoolean(reader["AnuladaFac"].ToString());
                                    NumCotra = reader["NumContra"].ToString();
                                    DefiCuenta = reader["DefiCuenta"].ToString();

                                    if (CarTercero == CERips)
                                    {
                                        if (AnulFact == false)
                                        {
                                            if (NumCotra == NContra)
                                            {
                                                if (DefiCuenta != "0")
                                                {
                                                    Utils.SqlDatos = "UPDATE [Datos de las facturas realizadas] " +
                                                    "SET ExpoRips = 1, CodSele = '" + lblCodigoUser.Text + "' " +
                                                    "FROM[Datos de las facturas realizadas] INNER JOIN[Datos cuentas de consumos] ON[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                                                    "WHERE[Datos de las facturas realizadas].NumFactura = '" + NDO + "' " +
                                                    "AND[Datos de las facturas realizadas].AnuladaFac = 0 " +
                                                    "AND[Datos cuentas de consumos].DefiCuenta <> N'0' ";

                                                    Boolean Act = Conexion.SQLUpdate(Utils.SqlDatos);

                                                    if (Act)
                                                    {
                                                        Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                                       "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                                       "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                                       "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                                       "(([Datos de las facturas realizadas].ExpoRips) = 1) And " +
                                                       "(([Datos de las facturas realizadas].NumContra) = '" + NContra + "') And " +
                                                       "(([Datos cuentas de consumos].DefiCuenta)<>'0') And " +
                                                       "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                                       "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                                       "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                                       "(([Datos de las facturas realizadas].AnuladaFac) = 0)) OR " +
                                                       "((([Datos de las facturas realizadas].NumFactura) = '" + NDO + "' ))";

                                                        DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);
                                                        if (dataSet.Tables.Count > 0)
                                                        {

                                                            DataGridDestino.DataSource = null;
                                                            DataGridDestino.DataSource = dataSet.Tables[0];

                                                        }

                                                        //ACTUALIZAMOS EL ORIGEN

                                                        Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                                         "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                                         "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                                         "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                                         "(([Datos de las facturas realizadas].ExpoRips) = 0) And " +
                                                         "(([Datos de las facturas realizadas].NumContra) = '" + NContra + "') And " +
                                                         "(([Datos cuentas de consumos].DefiCuenta)<>'0') And " +
                                                         "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                                         "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                                         "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                                         "(([Datos de las facturas realizadas].AnuladaFac) = 0)) ";

                                                        DataSet dataSet1 = Conexion.SQLDataSet(Utils.SqlDatos);
                                                        if (dataSet1.Tables.Count > 0)
                                                        {

                                                            DataGridFacturas.DataSource = null;
                                                            DataGridFacturas.DataSource = dataSet1.Tables[0];

                                                        }

                                                        CalcularTotalFactura();

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Utils.Informa = "Lo siento pero la factura " + NDO + "\r";
                                    Utils.Informa += " no se encontro en el sistema" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Question);
                                }
                                reader.Close();
                            }
                            break;
                        default:
                            break;
                    } //FIN DE SWICH
                } // FIN DEL ELSE DEL IF QUE VALIDA EL LABEL DigeteOrigen
            } //TRY CATCH
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al pasar factura seleccionada" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnTodas_Click(object sender, EventArgs e)
        {
            try
            {

                Utils.Titulo01 = "Control para agregar datos";

                string Coenti01 = null, UsSel = null, CardiTer = null, Fec1Sql = null, Fec2Sql = null, Para02 = null, Para03 = null, SqlFacturas = null, NContra = null;
                DateTime Fec01, Fec02;


                Fec1Sql = DateInicial.Value.ToString("yyyy-MM-dd");
                Fec2Sql = DateFinal.Value.ToString("yyyy-MM-dd");


                CardiTer = txtCardinal.Text;
                UsSel = lblCodigoUser.Text;

                if (cboNameEntidades.SelectedIndex < 0) {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad a mostrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Coenti01 = cboNameEntidades.SelectedValue.ToString();

                if (string.IsNullOrWhiteSpace(Coenti01) || string.IsNullOrEmpty(Coenti01))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad a mostrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text) || string.IsNullOrEmpty(lblCodigoUser.Text))
                {
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "no es valido para seleccionar datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                switch (Seleccion)
                {
                    case 1: //Muestra por factura.
                        Utils.Informa = "¿Usted desea agregar todas las facturas";
                        Utils.Informa = Utils.Informa + "del origen al listado destino para RIPS?";

                        SqlFacturas = "UPDATE [Datos de las facturas realizadas] SET [Datos de las facturas realizadas].ExpoRips = 1, " +
                        "[Datos de las facturas realizadas].CodSele = N'" + UsSel + "' " +
                        "FROM  [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                        "WHERE ([Datos de las facturas realizadas].FechaFac >= CONVERT(DATETIME, '" + Fec1Sql + "', 102)) AND " +
                        "([Datos de las facturas realizadas].FechaFac <= CONVERT(DATETIME, '" + Fec2Sql + "', 102)) AND " +
                        "([Datos de las facturas realizadas].Cartercero = '" + CardiTer + "') AND " +
                        "([Datos de las facturas realizadas].AnuladaFac = 0) AND " +
                        "([Datos cuentas de consumos].DefiCuenta <> N'0')";


                        //Arme el SQL para mostrar la lista destino


                        Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                        "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                        "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                        "WHERE ((([Datos de las facturas realizadas].Cartercero)='" + CardiTer + "') AND " +
                        "(([Datos de las facturas realizadas].ExpoRips)=1) AND " +
                        "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                        "(([Datos de las facturas realizadas].FechaFac)>='" + Fec1Sql + "' And " +
                        "([Datos de las facturas realizadas].FechaFac)<='" + Fec2Sql + "') AND " +
                        "(([Datos de las facturas realizadas].AnuladaFac)=0) AND " +
                        "(([Datos cuentas de consumos].DefiCuenta)<>'0')) " +
                        "ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura; ";

                        break;

                    case 2:  //Por cuenta de cobro
                        Utils.Informa = "¿Usted desea agregar todas las facturas de";
                        Utils.Informa = Utils.Informa + "todas las cuentas de cobros del listado de origen, al listado destino para RIPS.";

                        SqlFacturas = "UPDATE [Datos de las facturas realizadas] " +
                        "SET [Datos de las facturas realizadas].ExpoRips = 1, [Datos de las facturas realizadas].CodSele = '" + UsSel + "' " +
                        "FROM [DACARTXPSQL].[dbo].[Datos de cuentas de cobro] INNER JOIN [Datos de las facturas realizadas] ON " +
                        "[DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CtaCobroNo = [Datos de las facturas realizadas].CuentaCobro INNER JOIN " +
                        "[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                        "WHERE ([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta >= CONVERT(DATETIME, '" + Fec1Sql + "', 102)) AND " +
                        "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta <= CONVERT(DATETIME, '" + Fec2Sql + "', 102)) AND " +
                        "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CardinalTer = '" + CardiTer + "') AND " +
                        "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CerraCuenta = 1) AND " +
                        "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].AnulCuenta = 0) AND " +
                        "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].ExpoRips = 1) AND " +
                        "([Datos de las facturas realizadas].AnuladaFac = 0) AND " +
                        "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CodSele = '" + UsSel + "') AND " +
                        "([Datos cuentas de consumos].DefiCuenta <> N'0')";


                        Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                    "FROM [Datos cuentas de consumos] INNER JOIN ([DACARTXPSQL].[dbo].[Datos de cuentas de cobro] INNER JOIN " +
                                    "[Datos de las facturas realizadas] ON [DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CtaCobroNo = " +
                                    "[Datos de las facturas realizadas].CuentaCobro) ON [Datos cuentas de consumos].CuenNum = " +
                                    "[Datos de las facturas realizadas].NumCuenFac " +
                                    "WHERE ((([Datos de las facturas realizadas].ExpoRips)=1) AND " +
                                    "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                                    "(([Datos de las facturas realizadas].AnuladaFac)=0) AND " +
                                    "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta)>= '" + Fec1Sql + "' And " +
                                    "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta)<='" + Fec2Sql + "') AND " +
                                    "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CardinalTer)='" + CardiTer + "') AND " +
                                    "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CerraCuenta)=1) AND " +
                                    "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].AnulCuenta)=0) AND " +
                                    "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].ExpoRips)=1) AND " +
                                    "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CodSele)='" + UsSel + "') AND " +
                                    "(([Datos cuentas de consumos].DefiCuenta)<>'0'))" +
                                    "ORDER BY [Datos de las facturas realizadas].NumFactura;";

                        break;

                    case 3:
                        Utils.Informa = "¿Usted desea agregar todas las facturas";
                        Utils.Informa = Utils.Informa + "del origen al listado destino para RIPS?";

                        NContra = cboContratos.SelectedValue.ToString();

                        SqlFacturas = "UPDATE [Datos de las facturas realizadas] SET [Datos de las facturas realizadas].ExpoRips = 1, " +
                                    "[Datos de las facturas realizadas].CodSele = '" + UsSel + "' " +
                                    "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                                    "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                                    "WHERE ((([Datos de las facturas realizadas].FechaFac) >= CONVERT(DATETIME,'" + Fec1Sql + "',102) And " +
                                    "([Datos de las facturas realizadas].FechaFac) <= CONVERT(DATETIME,'" + Fec2Sql + "',102)) And " +
                                    "(([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "' ) And " +
                                    "(([Datos de las facturas realizadas].NumContra ) = '" + NContra + "' ) And " +
                                    "(([Datos de las facturas realizadas].AnuladaFac) = 0) AND ([Datos cuentas de consumos].DefiCuenta <> N'0')) ";

                        //Arme el SQL para mostrar la lista destino


                        Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                        "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                        "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                        "WHERE ((([Datos de las facturas realizadas].Cartercero)='" + CardiTer + "') AND " +
                        "(([Datos de las facturas realizadas].ExpoRips)=1) AND " +
                        "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                        "(([Datos de las facturas realizadas].FechaFac)>='" + Fec1Sql + "' And " +
                        "([Datos de las facturas realizadas].FechaFac)<='" + Fec2Sql + "') AND " +
                        "(([Datos de las facturas realizadas].NumContra) = '" + NContra + "' ) and " +
                        "(([Datos de las facturas realizadas].AnuladaFac)=0) AND " +
                        "(([Datos cuentas de consumos].DefiCuenta)<>'0')) " +
                        "ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura; ";

                        break;
                    default:
                        Utils.Informa = "Seleccione un tipo de datos" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Respuesta == DialogResult.Yes)
                {
                    Boolean TabFacturas = Conexion.SQLUpdate(SqlFacturas);

                    DataSet sqlDataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                    if (sqlDataSet.Tables.Count > 0)
                    {
                        DataGridDestino.DataSource = null;

                        DataGridDestino.DataSource = sqlDataSet.Tables[0];

                        if (Seleccion == 3 || Seleccion == 1) //Esto se hace para cuando es cuenta de cobro solo elimine la cuenta de cobro que se paso y no todas
                        {
                            DataGridFacturas.DataSource = null;
                        }
                        else
                        {
                            DataGridFacturas.ClearSelection();
                            DataGridFacturas.Rows.Remove(DataGridFacturas.CurrentRow);
                        }
                        CalcularTotalFactura();
                    }

                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al pasar todas las facturas al otro lado" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnQUna_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.Titulo01 = "Control de ejecucion";
                string Coenti01 = null, NDO = null, DQ = null, CodSele = null, SqlFacturas = null, DefiCuenta = null, CERips = null, Factura = null, CardiTer = null, UsSel = null, Para02 = null, Para03 = null, NContra = null;
                Boolean AnulFac;

                if (cboNameEntidades.SelectedIndex < 0)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad para quitar documentos" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CardiTer = cboNameEntidades.SelectedValue.ToString();


                if (string.IsNullOrWhiteSpace(CardiTer) || string.IsNullOrEmpty(CardiTer))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad para quitar documentos" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text) || string.IsNullOrEmpty(lblCodigoUser.Text))
                {
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "no es valido para seleccionar datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UsSel = lblCodigoUser.Text;
                Para02 = DateInicial.Value.ToString("yyyy-MM-dd");
                Para03 = DateFinal.Value.ToString("yyyy-MM-dd");

                if (string.IsNullOrWhiteSpace(lblDestinoDocument.Text))
                {
                    Utils.Informa = "Lo siento pero usted no ha seleccionado o" + "\r";
                    Utils.Informa += "digitado el número de factura a quitar" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Utils.Informa = "¿Usted desea quitar la facturas No. " + DQ + "\r";
                Utils.Informa += ", al listado destino para RIP?" + "\r";

                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                DQ = lblDestinoDocument.Text;

                if (Respuesta == DialogResult.Yes)
                {
                    SqlFacturas = "SELECT [Datos de las facturas realizadas].Cartercero, [Datos de las facturas realizadas].ExpoRips, " +
                             "[Datos de las facturas realizadas].CodSele, [Datos de las facturas realizadas].AnuladaFac, " +
                             "[Datos cuentas de consumos].DefiCuenta " +
                             "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                             "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                             "WHERE (([Datos de las facturas realizadas].NumFactura) = '" + DQ + "')" +
                             "Order by NumFactura";

                    SqlDataReader reader = Conexion.SQLDataReader(SqlFacturas);

                    if (reader.HasRows == false)
                    {

                        Utils.Informa = "Lo siento pero el número de factura " + DQ + "\r";
                        Utils.Informa += ", al listado destino para RIP?" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        reader.Read();
                        CERips = cboNameEntidades.SelectedValue.ToString();
                        Factura = reader["Cartercero"].ToString();
                        AnulFac = Convert.ToBoolean(reader["AnuladaFac"]);
                        CERips = cboNameEntidades.SelectedValue.ToString();
                        CodSele = reader["CodSele"].ToString();
                        DefiCuenta = reader["DefiCuenta"].ToString();
                        if (Factura == CERips)
                        {
                            if (CodSele == UsSel)
                            {
                                if (DefiCuenta != "0")
                                {
                                    Utils.SqlDatos = "UPDATE [Datos de las facturas realizadas] " +
                                       "SET ExpoRips = 0, CodSele = '" + lblCodigoUser.Text + "' " +
                                       "FROM[Datos de las facturas realizadas] INNER JOIN[Datos cuentas de consumos] ON[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                                       "WHERE[Datos de las facturas realizadas].NumFactura = '" + DQ + "' " +
                                       "AND[Datos de las facturas realizadas].AnuladaFac = 0 " +
                                       "AND[Datos cuentas de consumos].DefiCuenta <> N'0' ";

                                    Boolean ActDatos = Conexion.SQLUpdate(Utils.SqlDatos);
                                }
                                else
                                {
                                    Utils.Informa = "Lo siento pero la factura No. " + DQ + "\r";
                                    Utils.Informa += "no se considera de producción directa, por ende" + "\r";
                                    Utils.Informa += "no debe generar RIPS, por ejemplo es capita." + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                Utils.Informa = "Lo siento pero la factura No. " + DQ + "\r";
                                Utils.Informa += "no se encuentra seleccionada por " + "\r";
                                Utils.Informa += "usted para ser exportada a los RIPS " + "\r";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            Utils.Informa = "Lo siento pero la factura No. " + DQ + "\r";
                            Utils.Informa += "no pertenece a la entidad " + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        } //final read.hasrow

                        switch (Seleccion)
                        {
                            case 2:
                                break;
                            default:

                                Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                "(([Datos de las facturas realizadas].ExpoRips) = 0) And " +
                                "(([Datos cuentas de consumos].DefiCuenta)<>'0') And " +
                                "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                "(([Datos de las facturas realizadas].AnuladaFac) = 0)) OR " +
                                "((([Datos de las facturas realizadas].NumFactura) = '" + DQ + "' ))" +
                                "ORDER BY [Datos de las facturas realizadas].NumFactura;";

                                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                                if (dataSet.Tables.Count > 0)
                                {
                                    DataGridFacturas.DataSource = null;
                                    DataGridFacturas.DataSource = dataSet.Tables[0];

                                }

                                Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                                "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                                "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                "(([Datos de las facturas realizadas].ExpoRips) = 1) And " +
                                "(([Datos cuentas de consumos].DefiCuenta)<>'0') And " +
                                "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "') And " +
                                "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                "(([Datos de las facturas realizadas].AnuladaFac) = 0)) " +
                                "ORDER BY [Datos de las facturas realizadas].NumFactura;";

                                DataSet dataSet2 = Conexion.SQLDataSet(Utils.SqlDatos);

                                if (dataSet2.Tables.Count > 0)
                                {
                                    DataGridDestino.DataSource = null;
                                    DataGridDestino.DataSource = dataSet2.Tables[0];

                                }

                                CalcularTotalFactura();

                                break;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al pasar factura al lado opuesto" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnQTodas_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.Titulo01 = "Control de ejecucion";
                string Coenti01 = null, NDO = null, DQ = null, CodSele = null, SqlFacturas = null, DefiCuenta = null, CERips = null, Factura = null, CardiTer = null, UsSel = null, Para02 = null, Para03 = null, NContra = null;
                Boolean AnulFac;

                if (cboNameEntidades.SelectedIndex < 0)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad para quitar documentos" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CardiTer = cboNameEntidades.SelectedValue.ToString();


                if (string.IsNullOrWhiteSpace(CardiTer) || string.IsNullOrEmpty(CardiTer))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad para quitar documentos" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text) || string.IsNullOrEmpty(lblCodigoUser.Text))
                {
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "no es valido para seleccionar datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UsSel = lblCodigoUser.Text;
                Para02 = DateInicial.Value.ToString("yyyy-MM-dd");
                Para03 = DateFinal.Value.ToString("yyyy-MM-dd");


                Utils.Informa = "¿Usted desea quitar todas las facturas" + "\r";
                Utils.Informa += ", de listado destino para RIP?" + "\r";

                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (Respuesta == DialogResult.Yes)
                {
                    SqlFacturas = "UPDATE [Datos de las facturas realizadas] SET [Datos de las facturas realizadas].ExpoRips = 0, " +
                   "[Datos de las facturas realizadas].CodSele = N'000' " +
                   "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                   "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                   "WHERE ((([Datos de las facturas realizadas].CodSele) = '" + UsSel + "' ) And ([Datos cuentas de consumos].DefiCuenta <> '0') AND " +
                   "(([Datos de las facturas realizadas].ExpoRips) = 1 )) ";

                    Boolean act = Conexion.SQLUpdate(SqlFacturas);

                    if (act)
                    {
                        switch (Seleccion)
                        {
                            case 2:
                                break;
                            default:
                                Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                                           "FROM [Datos de las facturas realizadas] " +
                                           "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + CardiTer + "') And " +
                                           "(([Datos de las facturas realizadas].ExpoRips) = 0) And " +
                                           "(([Datos de las facturas realizadas].CodSele) = '" + "000" + "') And " +
                                           "(([Datos de las facturas realizadas].FechaFac) >= '" + Para02 + "' And " +
                                           "([Datos de las facturas realizadas].FechaFac) <= '" + Para03 + "') And " +
                                           "(([Datos de las facturas realizadas].AnuladaFac) = 0)) " +
                                           "ORDER BY [Datos de las facturas realizadas].NumFactura;";

                                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                                if (dataSet.Tables.Count > 0)
                                {
                                    DataGridFacturas.DataSource = null;
                                    DataGridFacturas.DataSource = dataSet.Tables[0];
                                    DataGridDestino.DataSource = null;
                                }

                                CalcularTotalFactura();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al pasar todas las facturas al lado opuesto" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RbFacturas_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Seleccion = 1;
                lblTipoRips.Text = "Facturas";
                cboContratos.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en Radio Button Facturas" + ex.Message);
            }

        }
        private void RbCtaCobro_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Seleccion = 2;
                lblTipoRips.Text = "Ctas de cobro";
                cboContratos.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en Radio Button Cta. de cobro." + ex.Message);
            }

        }
        private void RbContrato_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Seleccion = 3;
                lblTipoRips.Text = "Contrato";
                cboContratos.Enabled = true;
                CargarContratos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en Radio Button Contrato. " + ex.Message);
            }

        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            DataGridDestino.DataSource = null;
            DataGridFacturas.DataSource = null;
            txtTotalCantidadFacturas.Clear();
            txtTotalCantidadDestino.Clear();
        }

        //private void ActualizarDataGrisDestino()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.Titulo01 = "Control de errores de ejecución";
        //        Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
        //        Utils.Informa += "al actualziar el datagrid destino " + "\r";
        //        Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
        //        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {

                string Coenti01 = null, TDE = null, NCC = null, UsSel = null, SqlFacturas = null, SqlCuenCobros = null;
                string Para01 = null, Para02 = null, Para03 = null, Para07 = null, Para08 = null, Para11 = null, Para12 = null, Para13 = null, Para14 = null, Para15 = null, Para16 = null;
                string Fec1Sql, Fec2Sql, NCF = null;
                int TM, FacCuen;
                byte TDocEx;

                Utils.Titulo01 = "Control para mostrar documentos";


                if (cboNameEntidades.SelectedIndex < 0)
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad a mostrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Coenti01 = (cboNameEntidades.SelectedValue).ToString();

                if (string.IsNullOrWhiteSpace(Coenti01) || string.IsNullOrEmpty(Coenti01))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad a mostrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    TDE = txtTipoDocu.Text;
                    NCC = txtDocumento.Text;
                }


                if (DateInicial.Value > DateFinal.Value || DateFinal.Value < DateInicial.Value)
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "El rango de fechas no es correcto" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    DateTime Fec01 = DateInicial.Value;
                    DateTime Fec02 = DateFinal.Value;
                }

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "po es valido para seleccionar datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UsSel = lblCodigoUser.Text;

                Para01 = Coenti01;
                Para02 = DateInicial.Value.ToString("yyyy-MM-dd");
                Para03 = DateFinal.Value.ToString("yyyy-MM-dd");
                Para07 = txtNombreIps.Text;
                Fec1Sql = Para02;
                Fec2Sql = Para03;

                Para11 = txtTipoDocu.Text;
                Para12 = txtDocumento.Text;
                Para13 = txtRips.Text;
                Para14 = lblCodMinSalud.Text;
                Para15 = txtTipoDocuIps.Text;
                Para16 = txtDocuIps.Text;

                switch (Seleccion)
                {
                    case 1:
                        TDocEx = 1;

                        Utils.Titulo01 = "Control de ejecucion";
                        Utils.Informa = "¿Usted desea mostrar todas las facturas" + "\r";
                        Utils.Informa += "realizadas en el rango de fecha dado?" + "\r";

                        FacCuen = 1;

                        break;
                    case 2:
                        TDocEx = 2;

                        Utils.Titulo01 = "Control de ejecucion";
                        Utils.Informa = "¿Usted desea mostrar todas las cuentas de cobros" + "\r";
                        Utils.Informa += "realizadas en el rango de fecha dado?" + "\r";

                        FacCuen = 2;

                        break;
                    case 3:
                        TDocEx = 1;

                        if (cboContratos.SelectedIndex < 0 || string.IsNullOrEmpty(cboContratos.Text))
                        {

                            Utils.Titulo01 = "Control de error ejecucion";
                            Utils.Informa = "Lo siento pero mientras usted no digite el" + "\r";
                            Utils.Informa += "número de contrato de las facturas a mostrar," + "\r";
                            Utils.Informa += "no puede realizar la exportación de datos." + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        NCF = cboContratos.SelectedValue.ToString();
                        Para08 = NCF;

                        Utils.Titulo01 = "Control de ejecucion";
                        Utils.Informa = "¿Usted desea mostrar todas las facturas" + "\r";
                        Utils.Informa += "realizadas bajo el número de contrato" + "\r";
                        Utils.Informa += "en el rango de fechas dado?" + "\r";

                        FacCuen = 3;

                        break;

                    default:
                        Utils.Titulo01 = "Control de error ejecucion";
                        Utils.Informa = "Lo siento pero mientras usted no seleccione" + "\r";
                        Utils.Informa += "un tipo de consulta," + "\r";
                        Utils.Informa += "no puede realizar la exportación de datos." + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Respuesta == DialogResult.Yes)
                {
                    txtTotalCantidadFacturas.Text = "0";
                    txtTotalCantidadDestino.Text = "0";
                    switch (Seleccion)
                    {
                        case 1:
                            txtTotalCantidadFacturas.Clear();
                            txtTotalCantidadDestino.Clear();
                            DataGridFacturas.DataSource = null;
                            DataGridDestino.DataSource = null;

                            //********************* el 23 de noviembre de 2020, Hernando modificò, incluyendo que las facturas capitas no se relacionen en la selecciòn *****************

                            SqlFacturas = SqlFacturas = "UPDATE [Datos de las facturas realizadas] SET [Datos de las facturas realizadas].ExpoRips = 0, " +
                            "[Datos de las facturas realizadas].CodSele = N'" + UsSel + "' " +
                            "FROM  [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                            "WHERE ([Datos de las facturas realizadas].FechaFac >= CONVERT(DATETIME, '" + Fec1Sql + "', 102)) AND " +
                            "([Datos de las facturas realizadas].FechaFac <= CONVERT(DATETIME, '" + Fec2Sql + "', 102)) AND " +
                            "([Datos de las facturas realizadas].Cartercero = '" + Coenti01 + "') AND " +
                            "([Datos de las facturas realizadas].AnuladaFac = 0) AND " +
                            "([Datos cuentas de consumos].DefiCuenta <> N'0')";

                            Boolean Act = false;

                            Act = Conexion.SQLUpdate(SqlFacturas);


                            SqlFacturas = "";

                            Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                            "FROM [ACDATOXPSQL].[dbo].[Datos cuentas de consumos] INNER JOIN [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] ON " +
                            "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                            "WHERE ((([Datos de las facturas realizadas].Cartercero)='" + Coenti01 + "') AND " +
                            "(([Datos de las facturas realizadas].ExpoRips)= 0) AND " +
                            "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                            "(([Datos de las facturas realizadas].FechaFac)>='" + Para02 + "' And " +
                            "([Datos de las facturas realizadas].FechaFac)<='" + Para03 + "') AND " +
                            "(([Datos de las facturas realizadas].AnuladaFac)= 0 ) AND " +
                            "(([Datos cuentas de consumos].DefiCuenta)<>'0')) " +
                            "ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura; ";

                            DataSet sqlDataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                            if (sqlDataSet.Tables.Count > 0)
                            {
                                DataGridFacturas.DataSource = null;
                                DataGridFacturas.DataSource = sqlDataSet.Tables[0];
                            }

                            Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                            "FROM [ACDATOXPSQL].[dbo].[Datos cuentas de consumos] INNER JOIN [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] ON " +
                            "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                            "WHERE ((([Datos de las facturas realizadas].Cartercero)='" + Coenti01 + "') AND " +
                            "(([Datos de las facturas realizadas].ExpoRips)= 1) AND " +
                            "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                            "(([Datos de las facturas realizadas].FechaFac)>='" + Para02 + "' And " +
                            "([Datos de las facturas realizadas].FechaFac)<='" + Para03 + "') AND " +
                            "(([Datos de las facturas realizadas].AnuladaFac)= 0 ) AND " +
                            "(([Datos cuentas de consumos].DefiCuenta)<>'0')) " +
                            "ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura; ";

                            DataSet sqlDataSet2 = Conexion.SQLDataSet(Utils.SqlDatos);

                            if (sqlDataSet2.Tables.Count > 0)
                            {
                                DataGridDestino.DataSource = null;

                                DataGridDestino.DataSource = sqlDataSet2.Tables[0];
                            }

                            CalcularTotalFactura();

                            break;

                        case 2:

                            txtTotalCantidadFacturas.Clear();
                            txtTotalCantidadDestino.Clear();
                            DataGridFacturas.DataSource = null;
                            DataGridDestino.DataSource = null;

                            SqlCuenCobros = "UPDATE [DACARTXPSQL].[dbo].[Datos de cuentas de cobro] SET [DACARTXPSQL].[dbo].[Datos de cuentas de cobro].ExpoRips = 1, " +
                                            "[DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CodSele = N'" + UsSel + "' " +
                                            "FROM [DACARTXPSQL].[dbo].[Datos de cuentas de cobro] " +
                                            "WHERE ((([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta) >= CONVERT(DATETIME,'" + Fec1Sql + "',102) And " +
                                            "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta) <= CONVERT(DATETIME,'" + Fec2Sql + "',102)) And " +
                                            "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CardinalTer) = '" + Coenti01 + "' ) And " +
                                            "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CerraCuenta) = 1 ) And " +
                                            "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].AnulCuenta) = 0 )) ";

                            Act = false;
                            Act = Conexion.SQLUpdate(SqlCuenCobros);


                            Utils.SqlDatos = "SELECT [DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CtaCobroNo, Format([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].[FecCuenta],'dd-MMM-yyyy') AS FD " +
                                         "FROM [DACARTXPSQL].[dbo].[Datos de cuentas de cobro] " +
                                         "WHERE ((([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CardinalTer) = '" + Coenti01 + "') And " +
                                         "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].ExpoRips) = 1) And " +
                                         "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CodSele) = '" + UsSel + "' ) And " +
                                         "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta) >= '" + Para02 + "' And " +
                                         "([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].FecCuenta) <= '" + Para03 + "') And " +
                                         "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CerraCuenta) = 1) And " +
                                         "(([DACARTXPSQL].[dbo].[Datos de cuentas de cobro].AnulCuenta) = 0))" +
                                         "ORDER BY [DACARTXPSQL].[dbo].[Datos de cuentas de cobro].CtaCobroNo;";

                            DataSet sqlDataSetCuenCobro = Conexion.SQLDataSet(Utils.SqlDatos);

                            DataGridFacturas.Rows.Clear();
                            DataGridFacturas.DataSource = null;

                            if (sqlDataSetCuenCobro.Tables.Count > 0)
                            {
                                DataGridFacturas.DataSource = sqlDataSetCuenCobro.Tables[0];
                            }

                            DataGridDestino.Rows.Clear();
                            CalcularTotalFactura();

                            break;

                        case 3:

                            txtTotalCantidadFacturas.Clear();
                            txtTotalCantidadDestino.Clear();
                            DataGridFacturas.DataSource = null;
                            DataGridDestino.DataSource = null;

                            SqlFacturas = "UPDATE [Datos de las facturas realizadas] SET [Datos de las facturas realizadas].ExpoRips = 0, " +
                           "[Datos de las facturas realizadas].CodSele = N'" + UsSel + "' " +
                           "FROM [Datos de las facturas realizadas] INNER JOIN [Datos cuentas de consumos] ON " +
                           "[Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                           "WHERE ((([Datos de las facturas realizadas].FechaFac) >= CONVERT(DATETIME,'" + Fec1Sql + "',102) And " +
                           "([Datos de las facturas realizadas].FechaFac) <= CONVERT(DATETIME,'" + Fec2Sql + "',102)) And " +
                           "(([Datos de las facturas realizadas].Cartercero) = '" + Coenti01 + "' ) And " +
                           "(([Datos de las facturas realizadas].NumContra)= '" + NCF + "' ) AND " +
                           "(([Datos de las facturas realizadas].AnuladaFac) = 0 )  AND " +
                           "([Datos cuentas de consumos].DefiCuenta <> N'0'))";

                            Act = false;
                            Act = Conexion.SQLUpdate(SqlFacturas);
                            SqlFacturas = "";

                            Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                           "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                           "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                           "WHERE ((([Datos de las facturas realizadas].Cartercero)='" + Coenti01 + "') AND " +
                           "(([Datos de las facturas realizadas].ExpoRips)=0) AND " +
                           "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                           "(([Datos de las facturas realizadas].FechaFac)>='" + Para02 + "' And " +
                           "([Datos de las facturas realizadas].FechaFac)<='" + Para03 + "') AND " +
                           "(([Datos de las facturas realizadas].NumContra)= '" + NCF + "' ) AND " +
                           "(([Datos de las facturas realizadas].AnuladaFac)=0) AND " +
                           "(([Datos cuentas de consumos].DefiCuenta)<>'0')) " +
                           "ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura; ";


                            DataSet sqlDataSetContra = Conexion.SQLDataSet(Utils.SqlDatos);

                            if (sqlDataSetContra.Tables.Count > 0)
                            {
                                DataGridFacturas.DataSource = null;
                                DataGridFacturas.Rows.Clear();
                                DataGridFacturas.DataSource = sqlDataSetContra.Tables[0];
                            }

                            Utils.SqlDatos = "SELECT [Datos de las facturas realizadas].NumFactura, Format([FechaFac],'dd-MMM-yyyy') AS FD " +
                           "FROM [Datos cuentas de consumos] INNER JOIN [Datos de las facturas realizadas] ON " +
                           "[Datos cuentas de consumos].CuenNum = [Datos de las facturas realizadas].NumCuenFac " +
                           "WHERE ((([Datos de las facturas realizadas].Cartercero)='" + Coenti01 + "') AND " +
                           "(([Datos de las facturas realizadas].ExpoRips)=1) AND " +
                           "(([Datos de las facturas realizadas].CodSele)='" + UsSel + "') AND " +
                           "(([Datos de las facturas realizadas].FechaFac)>='" + Para02 + "' And " +
                           "([Datos de las facturas realizadas].FechaFac)<='" + Para03 + "') AND " +
                           "(([Datos de las facturas realizadas].NumContra)= '" + NCF + "' ) AND " +
                           "(([Datos de las facturas realizadas].AnuladaFac)=0) AND " +
                           "(([Datos cuentas de consumos].DefiCuenta)<>'0')) " +
                           "ORDER BY [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumFactura; ";

                            DataSet sqlDataSetContra2 = Conexion.SQLDataSet(Utils.SqlDatos);

                            if (sqlDataSetContra2.Tables.Count > 0)
                            {
                                DataGridDestino.DataSource = null;
                                DataGridDestino.Rows.Clear();
                                DataGridDestino.DataSource = sqlDataSetContra2.Tables[0];
                            }

                            CalcularTotalFactura(); //calcula los totales

                            break;
                    }
                }
                else
                {
                    //  No quiere mostrar nada
                    return;
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el boton Mostrar" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            string Coenti01 = Convert.ToString(cboNameEntidades.SelectedValue);
            string TDE = null, Coenti02 = null, SqlFacSele = null, SqlRecieNacidos = null, NCC = null, NEenti = null, MT = null, ConMinRips = null, UsSel = null, NCF = null, SqlFacturas = null, SqlHospitalizados = null, CuenBus = null, SqlConsumos1 = null;
            string TipUsSel = null, HisBus = null, SqlMedicamentos = null, CodInterIPs = null, GBus = null, CodTomo = null, ClaSerBus = null, NumDocSel = null, CexterAten = null, SqlUrgencias = null, SqlRNacidos = null, SqlConsultas = null, SqlMedica = null, SqlProcedimientos = null, SqlOtrosServi = null, SqlConsumos = null, FacturError = null, Sqlsuarios = null;
            int TM = 0, SubTolD = 0, TolD = 0;
            object PoliFactu;
            DateTime FecEnPer;
            string NumFac, DxEntra, DxSalida, DxRelac01, DxRelac02, DxRelac03, DxComplica, DxMuerte;
            double RegisConsul, CanFacSel, ValdetaFac;
            Boolean Seguir = false, SqlInsert;
            try
            {


                Utils.Titulo01 = "Control para seleccionar datos";

                Seguir = false;

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
                        MT = sqlDataReader["ManualTari"].ToString();
                    }
                    sqlDataReader.Close();

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                }

                if (string.IsNullOrWhiteSpace(txtRips.Text)) {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero el código de la Administradora" + "\r";
                    Utils.Informa += "de pagos en salud, no se encuentra definido para seleccionar los datos. " + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRips.Select();
                    return;
                }

                ConMinRips = txtRips.Text;

                if (string.IsNullOrWhiteSpace(lblCodigoUser.Text))
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero el código del usuario" + "\r";
                    Utils.Informa += "no es valido para seleccionar datos. " + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblCodigoUser.Select();
                    return;
                }

                //Borramos los datos temporales


                UsSel = lblCodigoUser.Text;
                Boolean FunEli = BorrarTempoRips(UsSel, Coenti02);

                if (FunEli == false)
                {
                    return;
                }

                //'Revisamos si la persona ya digitó al menos algunos documentos
                //Seleccion es una variable gobar que controla los datepcker
                TM = Convert.ToInt32(Seleccion);
                SubTolD = Convert.ToInt32(txtTotalCantidadDestino.Text);
                TolD = (SubTolD + Convert.ToInt32(txtTotalCantidadFacturas.Text));

                if (SubTolD <= 0)
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero no existen facturas" + "\r";
                    Utils.Informa += "para ejecutar este proceso." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalCantidadDestino.Select();
                    return;
                }

                switch (TM)
                {
                    case 1:
                        //Seleccionamos por facturas
                        Utils.Titulo01 = "Control de ejecución";
                        Utils.Informa = "¿Usted desea seleccionar los datos necesarios" + "\r";
                        Utils.Informa += "para realizar los RIPS de la entidad " + NEenti + ".?" + "\r";
                        Utils.Informa += "Son: " + SubTolD + " Facturas para Rips, de " + TolD;

                        SqlFacSele = "SELECT [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, [Datos de las facturas realizadas].NumCuenFac, " +
                        "[Datos empresas y terceros].NomAdmin, [Datos de las facturas realizadas].Cartercero, [Datos empresas y terceros].NomPlan, " +
                        "[Datos de las facturas realizadas].NumContra, [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago, " +
                        "[Datos de las facturas realizadas].CodSele, [Datos de las facturas realizadas].ValorTotal, [Datos de las facturas realizadas].ExpoRips, " +
                        "[Datos de las facturas realizadas].PrefiFac, [Datos de las facturas realizadas].NumResol " +
                        "FROM  [Datos empresas y terceros] INNER JOIN [Datos de las facturas realizadas] ON [Datos empresas y terceros].CarAdmin = " +
                        "[Datos de las facturas realizadas].Cartercero INNER JOIN [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = " +
                        "[Datos cuentas de consumos].CuenNum " +
                        "WHERE ([Datos de las facturas realizadas].Cartercero = '" + Coenti02 + "') AND " +
                        "([Datos de las facturas realizadas].CodSele = '" + UsSel + "') AND " +
                        "([Datos de las facturas realizadas].AnuladaFac = 0) AND " +
                        "([Datos de las facturas realizadas].ExpoRips = 1) AND " +
                        "([Datos cuentas de consumos].DefiCuenta <> N'0')";

                        break;
                    case 2:

                        //Seleccionamos por cuenta de cobro
                        Utils.Titulo01 = "Control de ejecución";
                        Utils.Informa = "¿Usted desea seleccionar los datos necesarios" + "\r";
                        Utils.Informa += "para realizar los RIPS de la entidad " + NEenti + ".?" + "\r";
                        Utils.Informa += "Son: " + SubTolD + "Facturas para Rips, de " + TolD;

                        SqlFacSele = "SELECT [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, " +
                        "[Datos de las facturas realizadas].NumCuenFac, [Datos empresas y terceros].NomAdmin, " +
                        "[Datos de las facturas realizadas].Cartercero, [Datos empresas y terceros].NomPlan, " +
                        "[Datos de las facturas realizadas].NumContra, [Datos de las facturas realizadas].ValorFac, " +
                        "[Datos de las facturas realizadas].Copago, [Datos de las facturas realizadas].CodSele, " +
                        "[Datos de las facturas realizadas].ValorTotal, [Datos de las facturas realizadas].ExpoRips, " +
                        "[Datos de las facturas realizadas].PrefiFac, [Datos de las facturas realizadas].NumResol " +
                        "FROM [Datos empresas y terceros] INNER JOIN [Datos de las facturas realizadas] ON " +
                        "[Datos empresas y terceros].CarAdmin = [Datos de las facturas realizadas].Cartercero INNER JOIN " +
                        "[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                        "WHERE ([Datos de las facturas realizadas].CodSele = '" + UsSel + "') AND " +
                        "([Datos de las facturas realizadas].Cartercero = '" + Coenti02 + "') AND " +
                        "([Datos de las facturas realizadas].AnuladaFac = 0) AND " +
                        "([Datos de las facturas realizadas].CuentaCobro <> '000000') AND " +
                        "([Datos de las facturas realizadas].SiCobro = 1) AND " +
                        "([Datos de las facturas realizadas].ExpoRips = 1) AND " +
                        "([Datos cuentas de consumos].DefiCuenta <> N'0')";


                        break;
                    case 3:
                        //Seleccionamos por contrato
                        Utils.Titulo01 = "Control de ejecución";
                        Utils.Informa = "¿Usted desea seleccionar los datos necesarios" + "\r";
                        Utils.Informa += "para realizar los RIPS de la entidad " + NEenti + ".?" + "\r";
                        Utils.Informa += "Son: " + SubTolD + "Facturas para Rips, de " + TolD;

                        NCF = Convert.ToString(cboContratos.SelectedValue);

                        SqlFacSele = "SELECT [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, " +
                                    "[Datos de las facturas realizadas].NumCuenFac, [Datos empresas y terceros].NomAdmin, " +
                                    "[Datos de las facturas realizadas].Cartercero, [Datos empresas y terceros].NomPlan, " +
                                    "[Datos de las facturas realizadas].NumContra, [Datos de las facturas realizadas].ValorFac, " +
                                    "[Datos de las facturas realizadas].Copago, [Datos de las facturas realizadas].CodSele, " +
                                    "[Datos de las facturas realizadas].ValorTotal, [Datos de las facturas realizadas].ExpoRips, " +
                                    "[Datos de las facturas realizadas].PrefiFac, [Datos de las facturas realizadas].NumResol " +
                                    "FROM [Datos empresas y terceros] INNER JOIN [Datos de las facturas realizadas] ON " +
                                    "[Datos empresas y terceros].CarAdmin = [Datos de las facturas realizadas].Cartercero INNER JOIN " +
                                    "[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum " +
                                    "WHERE ((([Datos de las facturas realizadas].Cartercero) = '" + Coenti02 + "' ) And " +
                                    "(([Datos de las facturas realizadas].CodSele) = '" + UsSel + "' ) And " +
                                    "(([Datos de las facturas realizadas].NumContra) = '" + NCF + "' ) And " +
                                    "(([Datos cuentas de consumos].DefiCuenta) <> N'0')  And " +
                                    "(([Datos de las facturas realizadas].AnuladaFac) = 0 ) And " +
                                    "(([Datos de las facturas realizadas].ExpoRips) = 1 )) ";


                        break;
                    default:
                        Utils.Titulo01 = "Control de ejecución";
                        Utils.Informa = "No ha seleccionado ningun radio button " + "\r";
                        break;
                }



                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Respuesta == DialogResult.No)
                {
                    return;
                }


                RegisConsul = SubTolD;


                //SqlDataReader TabFacSele = Conexion.SQLDataReader(SqlFacSele);

                SqlDataReader TabFacSele;

                using (SqlConnection connection3 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command3 = new SqlCommand(SqlFacSele, connection3);
                    command3.Connection.Open();
                    TabFacSele = command3.ExecuteReader();

                    if (TabFacSele.HasRows == false)
                    {
                        Utils.Titulo01 = "Control de ejecución";
                        Utils.Informa = "Lo siento pero en este sistema no se encuentra" + "\r";
                        Utils.Informa += "ninguna factura seleccionada por el digitador" + "\r";
                        Utils.Informa += "actual y por la entidad seleccionada.";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {


                        CanFacSel = 0;


                        SqlConsumos = "SELECT [Datos cuentas de consumos].CuenNum, [Datos del Paciente].TipoIden, " +
                        "[Datos del Paciente].NumIden, [Datos registros de consumos].FechaCon, " +
                        "[Datos cuentas de consumos].NumRemi, [Datos registros de consumos].CodiSOAT, " +
                        "[Datos registros de consumos].CodiISS, [Datos registros de consumos].CodiCUPS, " +
                        "[Datos registros de consumos].CodInter, [Datos registros de consumos].CodInter, " +
                        "[Datos registros de consumos].FinalConsul, [Datos cuentas de consumos].CausaExterna, " +
                        "[Datos cuentas de consumos].DxSalida, [Datos cuentas de consumos].DxRelac01, [Datos cuentas de consumos].DxRelac02, " +
                        "[Datos cuentas de consumos].DxRelac03, [Datos cuentas de consumos].TipoDxPrin, [Datos registros de consumos].ValorUnitario, [Datos registros de consumos].SubValorUnita, " +
                        "[Datos registros de consumos].Copagos, ([ValorUnitario]-[Copagos]) AS VN, [Datos catalogo de servicios].GrupoServi, ";


                        SqlConsumos = SqlConsumos + "[Datos cuentas de consumos].DxComplica," +
                        "[Datos registros de consumos].Cantidad, " +
                        "[Datos registros de consumos].RealizadoEn, " +
                        "[Datos registros de consumos].FinalProce, " +
                        "[Datos registros de consumos].PerAtien, " +
                        "[Datos registros de consumos].FormaRealiza, " +
                        "[Datos registros de consumos].AutoriNum, " +
                        "[Datos catalogo de servicios].NomServicio, [Datos catalogo de servicios].ClasiSer, " +
                        "[Datos catalogo de servicios].CodiMedMin, " +
                        "[Datos catalogo de servicios].PosMedi, " +
                        "[Datos catalogo de servicios].CodInterno, ";


                        SqlConsumos = SqlConsumos + "[Datos del Paciente].HistorPaci, " +
                        "[Datos del Paciente].Apellido1, " +
                        "[Datos del Paciente].Apellido2, " +
                        "[Datos del Paciente].Nombre1, " +
                        "[Datos del Paciente].Nombre2, " +
                        "[Datos cuentas de consumos].TipoUsuario, [Datos cuentas de consumos].DxEntra," +
                        "[Datos cuentas de consumos].TipoCuenta, " +
                        "[Datos cuentas de consumos].DiasEstancias, " +
                        "[Datos cuentas de consumos].ServiRips,  [Datos cuentas de consumos].HorasObser," +
                        "[Datos cuentas de consumos].FecEntrada, [Datos cuentas de consumos].HorEntrada, ";


                        SqlConsumos = SqlConsumos + "[Datos cuentas de consumos].EstaSalida, [Datos cuentas de consumos].FecSalida, " +
                        "[Datos cuentas de consumos].Destino, " +
                        "[Datos cuentas de consumos].HorSalida, [Datos cuentas de consumos].DxMuerte, ";


                        SqlConsumos = SqlConsumos + "[Datos del Paciente].Sexo, [Datos del Paciente].ZonaResiden, " +
                        "[Datos del Paciente].CodDpto, [Datos del Paciente].CodMuni, ";


                        SqlConsumos = SqlConsumos + "[Datos cuentas de consumos].ValorEdad, " +
                        "[Datos cuentas de consumos].UnidadEdad, " +
                        "[Datos cuentas de consumos].NumPoliza ";



                        SqlConsumos = SqlConsumos + "FROM [Datos catalogo de servicios] INNER JOIN ([Datos del Paciente] INNER JOIN " +
                        "([Datos cuentas de consumos] INNER JOIN [Datos registros de consumos] ON " +
                        "[Datos cuentas de consumos].CuenNum = [Datos registros de consumos].CuenConsu) ON " +
                        "[Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum) ON " +
                        "[Datos catalogo de servicios].CodInterno = [Datos registros de consumos].CodInter ";


                        while (TabFacSele.Read())
                        {
                            ValdetaFac = 0; //En esta variable se va ha registrar los valores de detalle de cada factura para después auditar qquien tiene descuadre
                            CanFacSel += 1;

                            CuenBus = TabFacSele["NumCuenFac"].ToString();
                            FacturError = TabFacSele["NumFactura"].ToString();



                            SqlConsumos1 = SqlConsumos + "WHERE ((([Datos cuentas de consumos].CuenNum) = '" + CuenBus + "' ) and " +
                            "(([Datos registros de consumos].SeRepRips) = 1 ) And " +
                            "(([Datos registros de consumos].PagaHoja) = 1) AND (([Datos registros de consumos].Cantidad) > 0) " +
                            "AND (([Datos registros de consumos].ValorUnitario + [Datos registros de consumos].SubValorUnita) > 0)) ";

                            SqlConsumos1 = SqlConsumos1 + "ORDER BY [Datos cuentas de consumos].CuenNum, [Datos catalogo de servicios].GrupoServi ";

                            //SqlDataReader TabConsumos = Conexion.SQLDataReader(SqlConsumos1);

                            SqlDataReader TabConsumos;

                            using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command2 = new SqlCommand(SqlConsumos1, connection2);
                                command2.Connection.Open();
                                TabConsumos = command2.ExecuteReader();

                                if (TabConsumos.HasRows == false)
                                {
                                    //Por integridad dificilmente entra aqui
                                }
                                else
                                {
                                    TabConsumos.Read();

                                    TipUsSel = TabConsumos["TipoIden"].ToString();
                                    NumDocSel = TabConsumos["NumIden"].ToString();
                                    CexterAten = TabConsumos["CausaExterna"].ToString(); //Causa externa de la atención

                                    Sqlsuarios = "SELECT [Datos temporal usuarios RIPS].* " +
                                                "FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] " +
                                                "WHERE ((([Datos temporal usuarios RIPS].CodDigita) = '" + UsSel + "' ) And " +
                                                "(([Datos temporal usuarios RIPS].NumRemi) = '" + Coenti01 + "' ) And " +
                                                "(([Datos temporal usuarios RIPS].TipoDocum) = '" + TipUsSel + "' ) And " +
                                                "(([Datos temporal usuarios RIPS].NumDocum) = '" + NumDocSel + "')) ";

                                    SqlDataReader TabUsuarios;

                                    using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                    {
                                        SqlCommand command = new SqlCommand(Sqlsuarios, connection);
                                        command.Connection.Open();
                                        TabUsuarios = command.ExecuteReader();

                                        if (TabUsuarios.HasRows == false)
                                        {
                                            string codMuni = TabConsumos["CodMuni"].ToString();
                                            codMuni = codMuni.Substring(0, 3);

                                            string data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS]" +
                                                        "(CodDigita," +
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
                                                        "CodDptoCity," +
                                                        "ZonaResi)" +
                                                         "VALUES(" +
                                                         "'" + UsSel + "'," +
                                                         "'" + Coenti01 + "'," +
                                                         "'" + TabConsumos["TipoIden"].ToString() + "'," +
                                                         "'" + TabConsumos["NumIden"].ToString() + "'," +
                                                         "'" + ConMinRips + "'," +
                                                         "'" + TabConsumos["TipoUsuario"].ToString() + "'," +
                                                         "'" + TabConsumos["Apellido1"].ToString() + "'," +
                                                         "'" + TabConsumos["Apellido2"].ToString() + "'," +
                                                         "'" + TabConsumos["Nombre1"].ToString() + "'," +
                                                         "'" + TabConsumos["Nombre2"].ToString() + "'," +
                                                         "'" + TabConsumos["ValorEdad"].ToString() + "'," +
                                                         "'" + TabConsumos["UnidadEdad"].ToString() + "'," +
                                                         "'" + TabConsumos["Sexo"].ToString() + "'," +
                                                         "'" + TabConsumos["CodDpto"].ToString() + "'," +
                                                         "'" + codMuni + "'," +
                                                         "'" + TabConsumos["CodMuni"].ToString() + "'," +
                                                         "'" + TabConsumos["ZonaResiden"].ToString() + "')";

                                            SqlInsert = Conexion.SqlInsert(data);
                                        }

                                        TabUsuarios.Close();
                                        TabUsuarios = null;

                                    }

                                    //     SqlDataReader TabUsuarios = Conexion.SQLDataReader(Sqlsuarios);



                                    if (TabConsumos["TipoCuenta"].ToString() == "04" || TabConsumos["TipoCuenta"].ToString() == "05")
                                    {
                                        /*   'El paciente fue facturado como hospitalizado en este sistema
                                              'A partir de hoy 02 de marzo, se incluye urgencias porque hay veces que no se hospitaliza en el sistema
                                              'Miramos los d'ias de estancias u horas */

                                        if (TabFacSele["NumResol"].ToString() != "0")
                                        {
                                            NumFac = (TabFacSele["PrefiFac"].ToString() + TabFacSele["NumFactura"].ToString());
                                        }
                                        else
                                        {
                                            NumFac = TabFacSele["NumFactura"].ToString();
                                        }

                                        DxEntra = (TabConsumos["DxEntra"].ToString() == "0000" ? "" : TabConsumos["DxEntra"].ToString());
                                        DxSalida = (TabConsumos["DxSalida"].ToString() == "0000" ? "" : TabConsumos["DxSalida"].ToString());
                                        DxRelac01 = (TabConsumos["DxRelac01"].ToString() == "0000" ? "" : TabConsumos["DxRelac01"].ToString());
                                        DxRelac02 = (TabConsumos["DxRelac02"].ToString() == "0000" ? "" : TabConsumos["DxRelac02"].ToString());
                                        DxRelac03 = (TabConsumos["DxRelac03"].ToString() == "0000" ? "" : TabConsumos["DxRelac03"].ToString());
                                        DxComplica = (TabConsumos["DxComplica"].ToString() == "0000" ? "" : TabConsumos["DxComplica"].ToString());
                                        DxMuerte = (TabConsumos["DxMuerte"].ToString() == "0000" ? "" : TabConsumos["DxMuerte"].ToString());

                                        //'Miramos los d'ias de estancias u horas
                                        if (Convert.ToInt32(TabConsumos["DiasEstancias"]) > 0)
                                        {

                                            //HOSPITALIZACION

                                            Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS]" +
                                                  "(" +
                                                  "CodDigita," +
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
                                                  "DxMuerte" +
                                                  ")" +
                                                  "VALUES" +
                                                  "(" +
                                                  "'" + UsSel + "'," +
                                                  "'" + Coenti02 + "'," +
                                                  "'" + NumFac + "'," +
                                                  "'" + lblCodMinSalud.Text + "'," +
                                                  "'" + TabConsumos["TipoIden"].ToString() + "'," +
                                                  "'" + TabConsumos["NumIden"].ToString() + "'," +
                                                  "'" + TabConsumos["ServiRips"].ToString() + "'," +
                                                  "'" + Convert.ToDateTime(TabConsumos["FecEntrada"]).ToString("yyyy-MM-dd") + "'," +
                                                  "'" + Convert.ToDateTime(TabConsumos["HorEntrada"]).ToString("hh:mm:ss") + "'," +
                                                  "'" + TabConsumos["NumRemi"].ToString() + "'," +
                                                  "'" + TabConsumos["CausaExterna"].ToString() + "'," +
                                                  "'" + DxEntra + "'," +
                                                  "'" + DxSalida + "'," +
                                                  "'" + DxRelac01 + "'," +
                                                  "'" + DxRelac02 + "'," +
                                                  "'" + DxRelac03 + "'," +
                                                  "'" + DxComplica + "'," +
                                                  "'" + TabConsumos["EstaSalida"].ToString() + "'," +
                                                  "'" + Convert.ToDateTime(TabConsumos["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                                  "'" + Convert.ToDateTime(TabConsumos["HorSalida"]).ToString("hh:mm:ss") + "'," +
                                                  "'" + DxMuerte + "'" +
                                                  ")";

                                            Boolean RegisHospitalizacion = Conexion.SqlInsert(Utils.SqlDatos);

                                        }
                                        else
                                        {

                                            //OBSERVACION URGENCIAS

                                            //Revisamos si realmente eztuvo en observaci'on
                                            if (Convert.ToInt32(TabConsumos["HorasObser"]) > 0)
                                            {

                                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS]" +
                                                    "(" +
                                                    "CodDigita," +
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
                                                    "FecSalida," +
                                                    "HorSalida," +
                                                    "DxMuerte" +
                                                    ")" +
                                                    "VALUES" +
                                                    "(" +
                                                    "'" + UsSel + "'," +
                                                    "'" + Coenti02 + "'," +
                                                    "'" + NumFac + "'," +
                                                    "'" + lblCodMinSalud.Text + "'," +
                                                    "'" + TabConsumos["TipoIden"].ToString() + "'," +
                                                    "'" + TabConsumos["NumIden"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(TabConsumos["FecEntrada"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(TabConsumos["HorEntrada"]).ToString("hh:mm:ss") + "'," +
                                                    "'" + TabConsumos["NumRemi"].ToString() + "'," +
                                                    "'" + TabConsumos["CausaExterna"].ToString() + "'," +
                                                    "'" + DxSalida + "'," +
                                                    "'" + DxRelac01 + "'," +
                                                    "'" + DxRelac02 + "'," +
                                                    "'" + DxRelac03 + "'," +
                                                    "'" + TabConsumos["Destino"].ToString() + "'," +
                                                    "'" + TabConsumos["EstaSalida"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(TabConsumos["FecSalida"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(TabConsumos["HorSalida"]).ToString("hh:mm:ss") + "'," +
                                                    "'" + DxMuerte + "'" +
                                                    ")";

                                                Boolean RegisObservacion = Conexion.SqlInsert(Utils.SqlDatos);

                                            } //Fin Hora > 0
                                        } //Fin dias estancias 
                                    } // Fin tipo Cuentas 04 -05

                                    //4. LOS POSIBLES RECIEN NACIDOS

                                    if (TabConsumos["Sexo"].ToString() == "F" || TabConsumos["Sexo"].ToString() == "f")
                                    {

                                        //RECIEN NACIDOS
                                        //Revisamos si la cuenta tiene registrado algun parto
                                        HisBus = TabConsumos["HistorPaci"].ToString();

                                        if (TabFacSele["NumResol"].ToString() != "0")
                                        {
                                            NumFac = (TabFacSele["PrefiFac"].ToString() + TabFacSele["NumFactura"].ToString());
                                        }
                                        else
                                        {
                                            NumFac = TabFacSele["NumFactura"].ToString();
                                        }

                                        DxEntra = (TabConsumos["DxEntra"].ToString() == "0000" ? "" : TabConsumos["DxEntra"].ToString());
                                        DxSalida = (TabConsumos["DxSalida"].ToString() == "0000" ? "" : TabConsumos["DxSalida"].ToString());
                                        DxRelac01 = (TabConsumos["DxRelac01"].ToString() == "0000" ? "" : TabConsumos["DxRelac01"].ToString());
                                        DxRelac02 = (TabConsumos["DxRelac02"].ToString() == "0000" ? "" : TabConsumos["DxRelac02"].ToString());
                                        DxRelac03 = (TabConsumos["DxRelac03"].ToString() == "0000" ? "" : TabConsumos["DxRelac03"].ToString());
                                        DxComplica = (TabConsumos["DxComplica"].ToString() == "0000" ? "" : TabConsumos["DxComplica"].ToString());
                                        DxMuerte = (TabConsumos["DxMuerte"].ToString() == "0000" ? "" : TabConsumos["DxMuerte"].ToString());

                                        SqlRecieNacidos = "SELECT [Datos de recien nacidos].* " +
                                        "FROM [ACDATOXPSQL].[dbo].[Datos de recien nacidos] " +
                                        "WHERE ((([Datos de recien nacidos].HistorMadre) = '" + HisBus + "') And " +
                                        "(([Datos de recien nacidos].CuenParto) = '" + CuenBus + "') And " +
                                        "(([Datos de recien nacidos].ParAnul) = 0)) " +
                                        "ORDER BY [Datos de recien nacidos].HistorMadre, [Datos de recien nacidos].CuenParto; ";

                                        SqlDataReader RecienNacidos;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(SqlRecieNacidos, connection);
                                            command.Connection.Open();
                                            RecienNacidos = command.ExecuteReader();

                                            if (RecienNacidos.HasRows)
                                            {
                                                while (RecienNacidos.Read())
                                                {
                                                    //   '***************  Las siguientes instrucciones se incluyen a partir del 27 de Noviembre de 2020, por HERNANDO ***********************

                                                    if (string.IsNullOrWhiteSpace(RecienNacidos["FecMuerNaci"].ToString()))
                                                    { //Entra si existe una fehca de muerte Naci

                                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS] " +
                                                               "(" +
                                                               "CodDigita," +
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
                                                               "DxMuerte," +
                                                               "FecMuerte," +
                                                               "HorMuerte" +
                                                               ")" +
                                                               "VALUES" +
                                                               "(" +
                                                               "'" + UsSel + "'," +
                                                               "'" + Coenti02 + "'," +
                                                               "'" + NumFac + "'," +
                                                               "'" + lblCodMinSalud.Text + "'," +
                                                               "'" + TabConsumos["TipoIden"].ToString() + "'," +
                                                               "'" + TabConsumos["NumIden"].ToString() + "'," +
                                                               "'" + Convert.ToDateTime(RecienNacidos["FechaNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                               "'" + Convert.ToDateTime(RecienNacidos["HoraNaci"]).ToString("hh:mm:ss") + "'," +
                                                               "'" + RecienNacidos["EdadGesta"].ToString() + "'," +
                                                               "'" + RecienNacidos["ConPrena"].ToString() + "'," +
                                                               "'" + RecienNacidos["SexoNaci"].ToString() + "'," +
                                                               "'" + RecienNacidos["PesoNaci"].ToString() + "'," +
                                                               "'" + RecienNacidos["DxNaci"].ToString() + "'," +
                                                               "'" + RecienNacidos["DxMuerNaci"].ToString() + "'," +
                                                               "'" + Convert.ToDateTime(RecienNacidos["FecMuerNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                                "'" + RecienNacidos["HorMuerNaci"].ToString() + "'" +
                                                               ")";
                                                    }
                                                    else
                                                    {
                                                        Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS] " +
                                                                   "(" +
                                                                   "CodDigita," +
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
                                                                   "DxMuerte" +
                                                                   ")" +
                                                                   "VALUES" +
                                                                   "(" +
                                                                   "'" + UsSel + "'," +
                                                                   "'" + Coenti02 + "'," +
                                                                   "'" + NumFac + "'," +
                                                                   "'" + lblCodMinSalud.Text + "'," +
                                                                   "'" + TabConsumos["TipoIden"].ToString() + "'," +
                                                                   "'" + TabConsumos["NumIden"].ToString() + "'," +
                                                                   "'" + Convert.ToDateTime(RecienNacidos["FechaNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                                   "'" + Convert.ToDateTime(RecienNacidos["HoraNaci"]).ToString("hh:mm:ss") + "'," +
                                                                   "'" + RecienNacidos["EdadGesta"].ToString() + "'," +
                                                                   "'" + RecienNacidos["ConPrena"].ToString() + "'," +
                                                                   "'" + RecienNacidos["SexoNaci"].ToString() + "'," +
                                                                   "'" + RecienNacidos["PesoNaci"].ToString() + "'," +
                                                                   "'" + RecienNacidos["DxNaci"].ToString() + "'," +
                                                                   "'" + RecienNacidos["DxMuerNaci"].ToString() + "'" +
                                                                   ")";
                                                    }


                                                    Boolean RegisRecienNadido = Conexion.SqlInsert(Utils.SqlDatos);

                                                }
                                            } //HasRow

                                            RecienNacidos.Close();

                                        } //Final Using

                                        //SqlDataReader RecienNacidos = Conexion.SQLDataReader(SqlRecieNacidos);


                                    }//if del tipo de sexo

                                    //   'Tomamos el posible numero de poliza, para grabarlo en la factura

                                    PoliFactu = TabConsumos["NumPoliza"].ToString();
                                    FecEnPer = Convert.ToDateTime(TabConsumos["FecEntrada"].ToString());


                                    SqlDataReader TabConsumos2 = Conexion.SQLDataReader(SqlConsumos1);

                                    if (TabConsumos2.HasRows == false)
                                    {
                                        Utils.Titulo01 = "Control de ejecución";
                                        Utils.Informa = "Lo siento pero en este sistema no se encuentra" + "\r";
                                        Utils.Informa += "ningun consumo por la actual entidad" + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    while (TabConsumos2.Read())
                                    {
                                        CodInterIPs = TabConsumos2["CodInter"].ToString();

                                        ValdetaFac = ValdetaFac + ((Convert.ToInt32(TabConsumos2["Cantidad"]) * Convert.ToInt32(TabConsumos2["ValorUnitario"])) + Convert.ToInt32(TabConsumos2["SubValorUnita"]));

                                        GBus = TabConsumos2["GrupoServi"].ToString();

                                        ClaSerBus = TabConsumos2["ClasiSer"].ToString();

                                        switch (MT)
                                        {
                                            case "1": //Manual SOAT
                                                CodTomo = TabConsumos2["CodiSOAT"].ToString();
                                                break;
                                            case "2": // 'Manual ISS
                                                CodTomo = TabConsumos2["CodiISS"].ToString();
                                                break;
                                            case "3":  // 'Manual CUPS
                                                CodTomo = TabConsumos2["CodiCUPS"].ToString();
                                                break;
                                            case "4": //Manual IPS
                                                CodTomo = TabConsumos2["CodInter"].ToString();
                                                break;
                                            default:
                                                CodTomo = TabConsumos2["CodInter"].ToString();
                                                break;
                                        }

                                        int GBus2 = Convert.ToInt32(GBus);

                                        switch (GBus2)
                                        {
                                            case 1: //Consultas
                                                    //procede a agregar registro


                                                if (TabFacSele["NumResol"].ToString() != "0")
                                                {
                                                    NumFac = (TabFacSele["PrefiFac"].ToString() + TabFacSele["NumFactura"].ToString());
                                                }
                                                else
                                                {
                                                    NumFac = TabFacSele["NumFactura"].ToString();
                                                }

                                                DxEntra = (TabConsumos2["DxEntra"].ToString() == "0000" ? "" : TabConsumos2["DxEntra"].ToString());
                                                DxSalida = (TabConsumos2["DxSalida"].ToString() == "0000" ? "" : TabConsumos2["DxSalida"].ToString());
                                                DxRelac01 = (TabConsumos2["DxRelac01"].ToString() == "0000" ? "" : TabConsumos2["DxRelac01"].ToString());
                                                DxRelac02 = (TabConsumos2["DxRelac02"].ToString() == "0000" ? "" : TabConsumos2["DxRelac02"].ToString());
                                                DxRelac03 = (TabConsumos2["DxRelac03"].ToString() == "0000" ? "" : TabConsumos2["DxRelac03"].ToString());
                                                DxComplica = (TabConsumos2["DxComplica"].ToString() == "0000" ? "" : TabConsumos2["DxComplica"].ToString());
                                                DxMuerte = (TabConsumos2["DxMuerte"].ToString() == "0000" ? "" : TabConsumos2["DxMuerte"].ToString());

                                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS]" +
                                                "(" +
                                                "CodDigita," +
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
                                                "ValorNeto" +
                                                ")" +
                                                "VALUES" +
                                                "(" +
                                                "'" + UsSel + "'," +
                                                "'" + Coenti02 + "'," +
                                                "'" + NumFac + "'," +
                                                "'" + lblCodMinSalud.Text + "'," +
                                                "'" + TabConsumos2["TipoIden"].ToString() + "'," +
                                                "'" + TabConsumos2["NumIden"].ToString() + "',";
                                                // '****************** lo siguiente se coloca por los caprichos de COMFAMILIAR ******************* 01 de Agosto de 2020
                                                if (DefFecTrans.Text == "Consultas")
                                                {
                                                    Utils.SqlDatos += "'" + Convert.ToDateTime(TabFacSele["FechaFac"]).ToString("yyyy-MM-dd") + "',";
                                                }
                                                else
                                                {
                                                    Utils.SqlDatos += "'" + Convert.ToDateTime(TabConsumos2["FechaCon"]).ToString("yyyy-MM-dd") + "',";
                                                }

                                                Utils.SqlDatos += "'" + TabConsumos2["AutoriNum"] + "'," +
                                                "'" + CodTomo + "'," +
                                                "'" + TabConsumos2["FinalConsul"].ToString() + "'," +
                                                "'" + TabConsumos2["CausaExterna"].ToString() + "'," +
                                                "'" + DxSalida + "'," +
                                                "'" + DxRelac01 + "'," +
                                                "'" + DxRelac02 + "'," +
                                                "'" + DxRelac03 + "'," +
                                                "'" + TabConsumos2["TipoDxPrin"].ToString() + "'," +
                                                "'" + TabConsumos2["ValorUnitario"].ToString() + "'," +
                                                "'" + TabConsumos2["Copagos"].ToString() + "'," +
                                                "'" + TabConsumos2["VN"].ToString() + "'" +
                                                ")";

                                                Boolean RegisConsulta = Conexion.SqlInsert(Utils.SqlDatos);
                                                break;

                                            case int G2 when GBus2 >= 2 && GBus2 <= 5: //Procedimientos

                                                //Como los procedimientos se deben grabar uno por uno,
                                                //tenemos que hacer unas iteraciones


                                                DxSalida = (TabConsumos2["DxSalida"].ToString() == "0000" ? "" : TabConsumos2["DxSalida"].ToString());
                                                DxComplica = (TabConsumos2["DxComplica"].ToString() == "0000" ? "" : TabConsumos2["DxComplica"].ToString());
                                                DxRelac01 = (TabConsumos2["DxRelac01"].ToString() == "0000" ? "" : TabConsumos2["DxRelac01"].ToString());



                                                for (int x = 0; x < Convert.ToInt32(TabConsumos2["Cantidad"]); x++)
                                                {

                                                    if (TabFacSele["NumResol"].ToString() != "0")
                                                    {
                                                        NumFac = (TabFacSele["PrefiFac"].ToString() + TabFacSele["NumFactura"].ToString());
                                                    }
                                                    else
                                                    {
                                                        NumFac = TabFacSele["NumFactura"].ToString();
                                                    }

                                                    //  '******************** Lo sigueinte se cambia a partir del 01 Agosto de 2020 HERNANDO *******************
                                                    Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] " +
                                                    "(" +
                                                    "CodDigita," +
                                                    "NumRemi," +
                                                    "NumFactur," +
                                                    "CodIPS," +
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
                                                    "ValorProce" +
                                                    ")" +
                                                    "VALUES" +
                                                    "(" +
                                                    "'" + UsSel + "'," +
                                                    "'" + Coenti02 + "'," +
                                                    "'" + NumFac + "'," +
                                                    "'" + lblCodMinSalud.Text + "'," +
                                                    "'" + TabConsumos2["TipoIden"].ToString() + "'," +
                                                    "'" + TabConsumos2["NumIden"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(TabConsumos2["FechaCon"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + TabConsumos2["AutoriNum"].ToString() + "'," +
                                                    "'" + CodTomo + "'," +
                                                    "'" + TabConsumos2["RealizadoEn"].ToString() + "'," +
                                                    "'" + TabConsumos2["FinalProce"].ToString() + "',";



                                                    if (GBus2 == 4 || GBus2 == 5)
                                                    {
                                                        if (ClaSerBus == "B" || ClaSerBus == "C")
                                                        {
                                                            Utils.SqlDatos += "'" + TabConsumos2["PerAtien"] + "',";
                                                        }
                                                        else
                                                        {
                                                            Utils.SqlDatos += "'" + "" + "',";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Utils.SqlDatos += "'" + "" + "',";
                                                    }

                                                    Utils.SqlDatos += "'" + DxSalida + "'," +
                                                    "'" + DxRelac01 + "'," +
                                                    "'" + DxComplica + "'," +
                                                    "'" + TabConsumos2["FormaRealiza"].ToString() + "'," +
                                                    "'" + (Convert.ToDouble(TabConsumos2["ValorUnitario"]) + Convert.ToDouble(TabConsumos2["SubValorUnita"])) + "'" +

                                                    ")";

                                                    Boolean RegisProcedimientos = Conexion.SqlInsert(Utils.SqlDatos);

                                                }

                                                break;

                                            case int G2 when GBus2 >= 6 && GBus2 <= 14: //'OTROS SERVICIOS

                                                if (TabFacSele["NumResol"].ToString() != "0")
                                                {
                                                    NumFac = (TabFacSele["PrefiFac"].ToString() + TabFacSele["NumFactura"].ToString());
                                                }
                                                else
                                                {
                                                    NumFac = TabFacSele["NumFactura"].ToString();
                                                }

                                                if (GBus2 == 12 || GBus2 == 13)
                                                {
                                                    //  'Son medicamentos, por lo tanto se deben tomar los datos complementarios

                                                    SqlMedicamentos = "SELECT [Datos productos farmaceuticos].CodigoPro, [Datos forma farmaceutica].CodForFar, " +
                                                                       "[Datos forma farmaceutica].NomForFar, [Datos productos farmaceuticos].Medida, " +
                                                                       "[Datos unidades de medidas].AbreMedida, [Datos unidades de medidas].Descripcion, " +
                                                                       "[Datos productos farmaceuticos].CodiMinSa, [Datos productos farmaceuticos].SiPos, " +
                                                                       "[Datos productos farmaceuticos].Concentra " +
                                                                       "FROM [BDFARMA].[dbo].[Datos forma farmaceutica] INNER JOIN [BDFARMA].[dbo].[Datos productos farmaceuticos] ON " +
                                                                       "[Datos forma farmaceutica].CodForFar = [Datos productos farmaceuticos].Formafarma INNER JOIN " +
                                                                       "[BDFARMA].[dbo].[Datos unidades de medidas] ON [Datos productos farmaceuticos].Medida = [Datos unidades de medidas].CodigoMedida " +
                                                                       "WHERE (([Datos productos farmaceuticos].CodigoPro) = N'" + CodInterIPs + "') ";

                                                    SqlDataReader TabMedicamentos;

                                                    //SqlDataReader TabMedicamentos = Conexion.SQLDataReader(SqlMedicamentos);

                                                    using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                                    {
                                                        try
                                                        {
                                                            SqlCommand command = new SqlCommand(SqlMedicamentos, connection);

                                                            command.Connection.Open();

                                                            TabMedicamentos = command.ExecuteReader();

                                                            if (TabMedicamentos.HasRows == false)
                                                            {
                                                                Utils.Titulo01 = "Control de ejecución";
                                                                Utils.Informa = "Lo siento pero el medicamento de código interno " + "\r";
                                                                Utils.Informa += "no se encuentra definido en base de datos de farmacia, por lo" + "\r";
                                                                Utils.Informa += "tanto no se pudo agregar a los datos de los RIPS";
                                                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            }
                                                            else
                                                            {

                                                                //'***************  Las siguientes instrucciones se incluyen a partir del 27 de Noviembre de 2020, por HERNANDO ***********************

                                                                TabMedicamentos.Read();
                                                                DxSalida = (TabConsumos2["DxSalida"].ToString() == "0000" ? "" : TabConsumos2["DxSalida"].ToString());
                                                                DxComplica = (TabConsumos2["DxComplica"].ToString() == "0000" ? "" : TabConsumos2["DxComplica"].ToString());
                                                                DxRelac01 = (TabConsumos2["DxRelac01"].ToString() == "0000" ? "" : TabConsumos2["DxRelac01"].ToString());

                                                                string NomForFar = TabMedicamentos["NomForFar"].ToString();

                                                                if (string.IsNullOrWhiteSpace(NomForFar) == false && NomForFar.Length > 20)
                                                                {
                                                                    NomForFar = NomForFar.Substring(0, 20);
                                                                }


                                                                string Descripcion = TabMedicamentos["Descripcion"].ToString();

                                                                if (string.IsNullOrWhiteSpace(Descripcion) == false && Descripcion.Length > 20)
                                                                {
                                                                    Descripcion = Descripcion.Substring(0, 20);
                                                                }


                                                                string Concentra = TabMedicamentos["Concentra"].ToString();

                                                                if (string.IsNullOrWhiteSpace(Concentra) == false && Concentra.Length > 20)
                                                                {
                                                                    Concentra = Concentra.Substring(0, 20);
                                                                }


                                                                string NomGenerico = TabConsumos2["NomServicio"].ToString();

                                                                if (string.IsNullOrWhiteSpace(NomGenerico) == false && NomGenerico.Length > 30)
                                                                {
                                                                    NomGenerico = NomGenerico.Substring(0, 30);
                                                                }



                                                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS]" +
                                                                "(" +
                                                                "CodDigita," +
                                                                "NumRemi," +
                                                                "NumFactur," +
                                                                "CodIPS," +
                                                                "TipoDocum," +
                                                                "NumDocum," +
                                                                "AutoriNum," +
                                                                //Se debe registrar el nombre de la forma
                                                                "FormaFarma," +
                                                                //Se busca la unidad de medida
                                                                "UniMedida," +
                                                                "ConcenMedi," +
                                                                "CodMedica," +
                                                                "TipoMedica," +
                                                                "NomGenerico," +
                                                                "NumUnidad," +
                                                                "ValorUnita," +
                                                                "ValorTotal" +
                                                                ")" +
                                                                "VALUES" +
                                                                "(" +
                                                                "'" + UsSel + "'," +
                                                                "'" + Coenti02 + "'," +
                                                                "'" + NumFac + "'," +
                                                                "'" + lblCodMinSalud.Text + "'," +
                                                                "'" + TabConsumos2["TipoIden"].ToString() + "'," +
                                                                "'" + TabConsumos2["NumIden"].ToString() + "'," +
                                                                "'" + TabConsumos2["AutoriNum"].ToString() + "'," +
                                                                "'" + NomForFar + "'," + //Se debe registrar el nombre de la forma
                                                                "'" + Descripcion + "'," + // Se busca la unidad de medida
                                                                "'" + Concentra + "',";

                                                                switch (MT)
                                                                {
                                                                    case "1": //'Manual SOAT
                                                                        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                        break;
                                                                    case "2": //'Manual CUPS-CUM
                                                                        CodTomo = TabConsumos2["CodiISS"].ToString();
                                                                        break;
                                                                    case "3": //'Manual CUPS
                                                                        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                        break;
                                                                    case "4": //'Manual IPS
                                                                        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                        break;
                                                                    case "5": //Manual SOAT-CUM
                                                                        CodTomo = TabConsumos2["CodiISS"].ToString();
                                                                        break;
                                                                    default: //'Utilice el manual IPS
                                                                        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                        break;
                                                                }

                                                                if (TabConsumos2["PosMedi"].ToString() == "2")
                                                                {
                                                                    //El medicamento no es POS
                                                                    if (MT == "2")  //Para ISS
                                                                    {
                                                                        Utils.SqlDatos += "'" + CodTomo + "',";
                                                                    }
                                                                    else
                                                                    {
                                                                        Utils.SqlDatos += "'" + "" + "',"; //Los no pos no llevan codigos
                                                                    }

                                                                    Utils.SqlDatos += "'" + "2" + "',"; //Los no pos no llevan codigos
                                                                }
                                                                else
                                                                {
                                                                    // 'El medicamento es POS
                                                                    Utils.SqlDatos += "'" + CodTomo + "',";
                                                                    Utils.SqlDatos += "'" + "1" + "',";
                                                                }

                                                                Utils.SqlDatos += "'" + NomGenerico + "'," +
                                                                      "'" + TabConsumos2["Cantidad"].ToString() + "'," +
                                                                      "'" + TabConsumos2["ValorUnitario"].ToString() + "'," +
                                                                      "'" + (Convert.ToDouble(TabConsumos2["Cantidad"]) * Convert.ToDouble(TabConsumos2["ValorUnitario"])) + "'" +
                                                                      ")";

                                                                Boolean InsertMedicamentos = Conexion.SqlInsert(Utils.SqlDatos);


                                                            } //TabMedicamentos.HasRows == false
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Utils.Titulo01 = "Control de errores de ejecución";
                                                            Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                                                            Utils.Informa += "al buscar los medicmanetos complementarios" + "\r";
                                                            Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                                                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        }

                                                    } //Fin Using Medicamento

                                                }
                                                else
                                                {
                                                    string NomServicio = TabConsumos2["NomServicio"].ToString();

                                                    if (string.IsNullOrWhiteSpace(NomServicio) == false && NomServicio.Length > 60)
                                                    {
                                                        NomServicio = NomServicio.Substring(0, 60);
                                                    }

                                                    //   'TRATELO CON TRAQUILIDAD QUE ES OTRO SERVICIO 

                                                    Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS]" +
                                                     "(" +
                                                     "CodDigita," +
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
                                                     "ValorTotal" +
                                                     ")" +
                                                     "VALUES" +
                                                     "(" +
                                                     "'" + UsSel + "'," +
                                                     "'" + Coenti02 + "'," +
                                                     "'" + NumFac + "'," +
                                                     "'" + lblCodMinSalud.Text + "'," +
                                                     "'" + TabConsumos2["TipoIden"].ToString() + "'," +
                                                     "'" + TabConsumos2["NumIden"].ToString() + "'," +
                                                     "'" + TabConsumos2["AutoriNum"].ToString() + "'," +
                                                     "'" + TabConsumos2["FinalProce"].ToString() + "'," +
                                                     "'" + CodTomo + "'," +
                                                     "'" + NomServicio + "'," +
                                                     "'" + TabConsumos2["Cantidad"].ToString() + "'," +
                                                     "'" + TabConsumos2["ValorUnitario"].ToString() + "'," +
                                                     "'" + (Convert.ToDouble(TabConsumos2["Cantidad"]) * Convert.ToDouble(TabConsumos2["ValorUnitario"])) + "'" +
                                                     ")";


                                                    Boolean InsertMedicamentos = Conexion.SqlInsert(Utils.SqlDatos);

                                                } // Final de GBus = "12" Or GBus = "13"

                                                break;
                                            default:

                                                break;

                                        } //Fin del swicch

                                    } //WHILE CONSUMOS READ

                                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                                    // 2. copiamos las facturas, despues de registrar, datos de los archivos de movimientos,
                                    //para poder registrar el detalle individual de cada factura

                                    string TipoDocuIPS = txtTipoDocuIps.Text;

                                    if (TipoDocuIPS.Length > 2)
                                    {
                                        TipoDocuIPS = TipoDocuIPS.Substring(0, 2);
                                    }



                                    string NomAdmin = TabFacSele["NomAdmin"].ToString();

                                    if (string.IsNullOrWhiteSpace(NomAdmin) == false && NomAdmin.Length > 30)
                                    {
                                        NomAdmin = NomAdmin.Substring(0, 30);

                                    }

                                    string NumContra = TabFacSele["NumContra"].ToString();

                                    if (string.IsNullOrWhiteSpace(NumContra) == false && NumContra.Length > 15)
                                    {
                                        NumContra = NumContra.Substring(0, 15);
                                    }

                                    string NomPlan = TabFacSele["NomPlan"].ToString();

                                    if (string.IsNullOrWhiteSpace(NomPlan) == false && NomPlan.Length > 30)
                                    {
                                        NomPlan = NomPlan.Substring(0, 30);
                                    }


                                    if (TabFacSele["NumResol"].ToString() != "0")
                                    {
                                        NumFac = (TabFacSele["PrefiFac"].ToString() + TabFacSele["NumFactura"].ToString());
                                    }
                                    else
                                    {
                                        NumFac = TabFacSele["NumFactura"].ToString();
                                    }


                                    Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS]" +
                                        "(" +
                                        "CodDigita," +
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
                                        "ValorNeto," +
                                        "VaLorDeta," +
                                        "CausExter" +
                                         ")" +
                                        "VALUES" +
                                        "(" +
                                        "'" + UsSel + "'," +
                                        "'" + Coenti02 + "'," +
                                        "'" + lblCodMinSalud.Text + "'," +
                                        "'" + txtNombreIps.Text + "'," +
                                        "'" + TipoDocuIPS + "'," +
                                        "'" + txtDocuIps.Text + "'," +
                                        "'" + NumFac + "'," +
                                        "'" + Convert.ToDateTime(TabFacSele["FechaFac"]).ToString("yyyy-MM-dd") + "',";
                                    //   '***************  Las siguientes instrucciones se incluyen a partir del 27 de Noviembre de 2020, por HERNANDO ***********************
                                    if (DefFecTrans.Text == "No aplica")
                                    {

                                        Utils.SqlDatos += "'" + Convert.ToDateTime(DateInicial.Value).ToString("yyyy-MM-dd") + "'," +
                                        "'" + Convert.ToDateTime(DateFinal.Value).ToString("yyyy-MM-dd") + "',";
                                    }
                                    else
                                    {

                                        Utils.SqlDatos += "'" + Convert.ToDateTime(FecEnPer).ToString("yyyy-MM-dd") + "'," +
                                        "'" + Convert.ToDateTime(TabFacSele["FechaFac"]).ToString("yyyy-MM-dd") + "',";
                                    }

                                    Utils.SqlDatos += "'" + ConMinRips + "'," +
                                     "'" + NomAdmin + "',";

                                    if (IDContrato.Text == "0")
                                    {
                                        Utils.SqlDatos += "'" + NumContra + "',";
                                    }
                                    else
                                    {
                                        //    'A todas las facturas coloquele el código ID del contrato, CREADO EL 29 DE julio de 2020 por HERNANDO
                                        Utils.SqlDatos += "'" + IDContrato.Text + "',";
                                    }

                                    Utils.SqlDatos += "'" + NomPlan + "'," +
                                    "'" + PoliFactu + "'," +
                                    "'" + TabFacSele["Copago"].ToString() + "'," +
                                    "'" + TabFacSele["ValorFac"].ToString() + "'," +
                                    "'" + ValdetaFac + "'," +
                                    "'" + CexterAten + "'" +
                                    ")";


                                    Boolean TabFacturas = Conexion.SqlInsert(Utils.SqlDatos);


                                }  //Final  de TabConsumos.hasrow

                                TabConsumos.Close();

                            } //Fin Using TABCONSUMOS

                            //'Desacivo la factura marcada, para que no se vuelva a seleccionar

                            Utils.SqlDatos = "UPDATE [Datos de las facturas realizadas] SET ExpoRips = 0, CodSele = '" + UsSel + "'  WHERE [NumFactura] = '" + TabFacSele["NumFactura"] + "'";

                            Boolean ActFactur = Conexion.SQLUpdate(Utils.SqlDatos);

                        } // FINAL WHILE TABFACSELEC

                        Utils.Informa = "He terminado de procesar todos ";
                        Utils.Informa += "los datos que conforman los RIPS ";
                        Utils.Informa += "de las facturas seleccionadas.";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }//FINAL TABFACSELE.HASROW

                } //Fin Suing



                TabFacSele.Close();

                DataGridDestino.DataSource = null;

                CalcularTotalFactura();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el boton seleccionar" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        int Seleccion = 1;

        private void FrmExportarSedarips_Load(object sender, EventArgs e)
        {
            try
            {
                DatosDeLaEmpresa();
                CargarCombobox();
                CargarDatosUser();
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al abrir el formulario Exportar Rips" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
            }
        }


    }
}
