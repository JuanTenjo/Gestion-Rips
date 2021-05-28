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
                                    " [Datos cuentas de consumos].TipoUsuario, [Datos cuentas de consumos].ValorEdad, [Datos cuentas de consumos].UnidadEdad, "+
                                    " [Datos empresas y terceros].NomAdmin, [Datos tipos de usuarios].NomTipo, [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].NumCuenFac, [Datos cuentas de consumos].HistoNum " +
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
                            DataGridFacturas.Rows.Add(sqlDataReader["Estado"], sqlDataReader["HistoNum"], sqlDataReader["TipoUsuario"], sqlDataReader["ValorEdad"], sqlDataReader["UnidadEdad"], sqlDataReader["NumFactura"].ToString(), sqlDataReader["Fecha"].ToString(), sqlDataReader["NomAdmin"].ToString(), sqlDataReader["NomTipo"].ToString(), sqlDataReader["ValorFac"].ToString());
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
                Boolean SqlInsert = false;
                string Coenti01, TDE, NCC, NEnti = null, MT,SqlDatos = null;
                int FunEli;

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

                    foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                    {

                        int Estado = Convert.ToInt32(Row.Cells["Estado"].Value);

                        if (Estado == 1)
                        {
                            string HistoNum = Convert.ToString(Row.Cells["HistoNum"].Value);

                            // string Cardinal = Convert.ToString(Row.Cells["Estado"].Value);

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

                                    using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                    {
                                        SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                        command.Connection.Open();
                                        DatosTemporalUser = command.ExecuteReader();

                                        if (DatosTemporalUser.HasRows == false)
                                        {
                                            string TipoUsuario = Convert.ToString(Row.Cells["TipoUsuario"].Value);

                                            string ValorEdad = Convert.ToString(Row.Cells["ValorEdad"].Value);

                                            string UnidadEdad = Convert.ToString(Row.Cells["UnidadEdad"].Value);

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

                                            string data = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS] " +
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
