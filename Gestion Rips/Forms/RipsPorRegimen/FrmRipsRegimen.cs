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
                                 " FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 'True') and(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 'True'))" +
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

        private int ElimdatosRIPS(string UsSel,string ConMinRips)
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
                    if(Estado == 1)
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
                Utils.Titulo01 = "Control para seleccionar datos";
                Boolean SqlInsert = true;
                string Coenti01, TDE, NCC, NEnti = null, MT = null,SqlDatos = null;
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

                var re  = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (re == DialogResult.Yes)
                {

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

                                    Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] " +
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

                                            data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] " +
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
                                            "'" + reader["TipoIden"].ToString() + "'," +
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

                                    data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] " +
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
                                " AND [Datos catalogo de servicios].GrupoServi = '"+ Para02 + "' " +
                                " AND [Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "'  ";


                            SqlDataReader ArchivoConsultas;

                            using (SqlConnection connection3 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command3 = new SqlCommand(Utils.SqlDatos, connection3);
                                command3.Connection.Open();

                                ArchivoConsultas = command3.ExecuteReader();


                                if (ArchivoConsultas.HasRows)
                                {

                                    ArchivoConsultas.Read();

                                    string TipoIden = ArchivoConsultas["TipoIden"].ToString();

                                    if (string.IsNullOrWhiteSpace(TipoIden) == false && TipoIden.Length > 2)
                                    {
                                        TipoIden = TipoIden.Substring(0, 2);

                                    }

                                    data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS] " +
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

                            } //FIN Using

                            int SubTolD = Convert.ToInt32(TxtMarcadas.Text);



                            //PROCEDIMIENTOS -------------------------------------------------------------------------------------------------------------------------

                            int FunP = ProceSoloPorFacturas(NumFactur,  CodIPS, Coenti01, NumCuenFac, MT, SubTolD);

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
                                                " WHERE [Datos registros de consumos].ValorUnitario > 0 AND [Datos registros de consumos].PagaHoja = 1 AND [Datos registros de consumos].Cantidad > 0 AND[Datos cuentas de consumos].CuenNum = '"+ NumCuenFac + "' " +
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
                                        ArchivoOtrosServicios.Read();
                                        data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] " +
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
                                                "'" + ArchivoOtrosServicios["TipoIden"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["NumIden"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["AutoriNum"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["FinalProce"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["CodConsul"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["NomServicio"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["Cantidad"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["ValorUnitario"].ToString() + "'," +
                                                "'" + ArchivoOtrosServicios["TolSer"].ToString() + "');";

                                        SqlInsert = Conexion.SqlInsert(data);

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
                                                " WHERE [Datos cuentas de consumos].TipoCuenta = '04' AND[Datos cuentas de consumos].DiasEstancias <> 0 AND[Datos cuentas de consumos].CuenNum = '"+ NumCuenFac + "'";


                                SqlDataReader ArchivoHospitalizacion;

                                using (SqlConnection connection5 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command5 = new SqlCommand(Utils.SqlDatos, connection5);
                                    command5.Connection.Open();

                                    ArchivoHospitalizacion = command5.ExecuteReader();


                                    if (ArchivoHospitalizacion.HasRows)
                                    {
                                        ArchivoHospitalizacion.Read();
                                        data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] " +
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
                                                "'" + ArchivoHospitalizacion["TipoIden"].ToString() + "'," +
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
                                                 " WHERE [Datos cuentas de consumos].TipoCuenta = '"+ Para02 + "' AND [Datos cuentas de consumos].DiasEstancias = 0 AND[Datos cuentas de consumos].CuenNum = '" + NumCuenFac + "'";


                                SqlDataReader ArchivoObservacion;

                                using (SqlConnection connection6 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command6 = new SqlCommand(Utils.SqlDatos, connection6);
                                    command6.Connection.Open();

                                    ArchivoObservacion = command6.ExecuteReader();

                                    if (ArchivoObservacion.HasRows)
                                    {
                                        ArchivoObservacion.Read();
                                        data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] " +
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
                                                "'" + ArchivoObservacion["TipoIden"].ToString() + "'," +
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

                                        ArchivoRecienNacidos.Read();
                                        data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS]" +
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
                                                "DxMuerte," +
                                                "FecMuerte," +
                                                "HorMuerte)" +
                                                "VALUES(" +
                                                "'" + UsSel + "'," +
                                                "'" + Coenti01 + "'," +
                                                "'" + NumFactur + "'," +
                                                "'" + CodIPS + "'," +
                                                "'" + ArchivoRecienNacidos["TipoIden"].ToString() + "'," +
                                                "'" + ArchivoRecienNacidos["NumIden"].ToString() + "'," +
                                                "'" + Convert.ToDateTime(ArchivoRecienNacidos["FechaNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                "'" + Convert.ToDateTime(ArchivoRecienNacidos["HoraNaci"]).ToString("hh:mm:ss") + "'," +
                                                "'" + ArchivoRecienNacidos["EdadGesta"].ToString() + "'," +
                                                "'" + ArchivoRecienNacidos["ConPrena"].ToString() + "'," +
                                                "'" + ArchivoRecienNacidos["SexoNaci"].ToString() + "'," +
                                                "'" + ArchivoRecienNacidos["PesoNaci"].ToString() + "'," +
                                                "'" + ArchivoRecienNacidos["DxNaci"].ToString() + "'," +
                                                "'" + ArchivoRecienNacidos["DxMuerNaci"].ToString() + "'," +
                                                "'" + Convert.ToDateTime(ArchivoRecienNacidos["FecMuerNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                "'" + Convert.ToDateTime(ArchivoRecienNacidos["HorMuerNaci"]).ToString("hh:mm:ss") + "')";

                                        SqlInsert = Conexion.SqlInsert(data);

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
                                                " WHERE [Datos registros de consumos].PagaHoja = 1 AND[Datos registros de consumos].ValorUnitario > 0 AND[Datos registros de consumos].Cantidad > 0 AND[Datos cuentas de consumos].CuenNum = '"+ NumCuenFac + "' " +
                                                " AND ([Datos catalogo de servicios].GrupoServi = '12' OR[Datos catalogo de servicios].GrupoServi = '13')";

                                SqlDataReader ArchivoMedicamentos;

                                using (SqlConnection connection8 = new SqlConnection(Conexion.conexionSQL))
                                {
                                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection8);
                                    command8.Connection.Open();

                                    ArchivoMedicamentos = command8.ExecuteReader();


                                    if (ArchivoMedicamentos.HasRows)
                                    {
                                        ArchivoMedicamentos.Read();

                                        data = "INSERT INTO[DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] " +
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
                                                "'" + ArchivoMedicamentos["TipoIden"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["NumIden"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["NumRemi"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["CodiMedMin"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["PosMedi"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["NomServicio"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["Cantidad"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["ValorUnitario"].ToString() + "'," +
                                                "'" + ArchivoMedicamentos["VT"].ToString() + "')";

                                        SqlInsert = Conexion.SqlInsert(data);


                                    }
                                }
                                ArchivoMedicamentos.Close();

                                //FunP = ComDatosMedica(Coenti01);


                            }//final funcion FunP


                        }//Estado Grilla


                    } //Foreach Grilla


                    if (SqlInsert)
                    {
                        MessageBox.Show("Listo");
                    }

                } // Dialogo Yes
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "después de hacer click sobre el botón agregar una" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        public int ComDatosMedica(string Ct)
        {
            try
            {

                string c = null, FormaFarma = null, ConcenMedi = null, NumFactur = null;
              
                //'**************  Creada el 11 de diciembre de 2003 ***************
                //'Permite colocar la información complementaria de los medicamentos NO POS y POS
                //'como es la forma la concentración, presentación, unidad, etc
                //'Esto funciona para aquella entidades que tienen definido el modulo de farmacia

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] where NumRemi =  '"+ Ct +"' ";



                SqlDataReader ArchivoMedicamentos;

                using (SqlConnection connection9 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command8 = new SqlCommand(Utils.SqlDatos, connection9);
                    command8.Connection.Open();

                    ArchivoMedicamentos = command8.ExecuteReader();

                    if(ArchivoMedicamentos.HasRows == false)
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


                                if(reader.HasRows == false)
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

                                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] SET CodMedica = '', FormaFarma = '" + FormaFarma + "'," +
                                                         " UniMedida = '" + reader["Descripcion"].ToString() + "', ConcenMedi = '" + ConcenMedi + "', TipoMedica = '2' WHERE NumFactur = '"+ NumFactur +"' ";
                                    }
                                    else
                                    {
                                          Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] SET CodMedica = '"+ reader["CodiMinSa"].ToString() + "', TipoMedica = '1' WHERE NumFactur = '"+ NumFactur +"' ";
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

        private int ProceSoloPorFacturas(string NumFactur, string CodIPS, string NumRemi, string CT, string M,int TolDoc)
        {
            try
            {

               string AutoNum = null, CodProce = null, DxPrin = null, DxRel1 = null, DxCom    = null, TD = null, NDocum = null;
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
                                            " WHERE [Datos cuentas de consumos].CuenNum = '"+ CT +"' " +
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

                        if(TolPro > 0)
                        {
                            CanPro = Convert.ToInt32(TabLocal["Cantidad"].ToString());

                            for (int i = 1; i <= CanPro; i++)
                            {
                                //Empiece agregar el registro a la tabla temporal


                                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] " +
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
                                      "'" + TolPro  + "');";


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
