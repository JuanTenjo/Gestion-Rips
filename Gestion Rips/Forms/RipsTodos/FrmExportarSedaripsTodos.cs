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

namespace Gestion_Rips.Forms.RipsTodos
{
    public partial class FrmExportarSedaripsTodos : Form
    {
        public FrmExportarSedaripsTodos()
        {
            InitializeComponent();
        }

        int MarSedes = 2;
        int MarLimRegis = 1;

        #region ComboBox


        private void CargarComboBox()
        {
            try
            {
                //Agregamos primeramente los combos, abriendo las instancias que se pueden cerrar inmediatament


                this.cboNameEntidades.DataSource = null;
                this.cboNameEntidades.Items.Clear();
                this.cboSedeVivi.DataSource = null;
                this.cboSedeVivi.Items.Clear();


                Utils.SqlDatos = "SELECT CodInterno, NomAdmin, TipoDocumento, NitCC, CodAdmin  " +
                                " FROM [Datos administradoras de planes] " +
                                " ORDER BY NomAdmin ";


                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    this.cboNameEntidades.DataSource = dataSet.Tables[0];
                    this.cboNameEntidades.ValueMember = "CodInterno";
                    this.cboNameEntidades.DisplayMember = "NomAdmin";
                }

                string SqlSedesIns = "SELECT CodSede, NomSede, PrefiFac, TipSede, HabilSede " +
                             "FROM [BDADMINSIG].[dbo].[Datos sedes de instalacion] " +
                             "WHERE  (HabilSede = 'True') " +
                             "ORDER BY NomSede";

                DataSet dataSet2 = Conexion.SQLDataSet(SqlSedesIns);

                if (dataSet2 != null && dataSet2.Tables.Count > 0)  
                {
                    this.cboSedeVivi.DataSource = dataSet2.Tables[0];
                    this.cboSedeVivi.ValueMember = "CodSede";
                    this.cboSedeVivi.DisplayMember = "NomSede";
                }

                string SqlGruEsDx = "SELECT CodEsDx, NomEsDx, ObserEspe  " +
                 "FROM [GEOGRAXPSQL].[dbo].[Datos grupos especiales de Dx] " +
                 "ORDER BY NomEsDx";

                DataSet TabGruEsDx = Conexion.SQLDataSet(SqlGruEsDx);

                if (TabGruEsDx != null && TabGruEsDx.Tables.Count > 0)
                {
                    this.CboGrupEspRegis.DataSource = TabGruEsDx.Tables[0];
                    this.CboGrupEspRegis.ValueMember = "CodEsDx";
                    this.CboGrupEspRegis.DisplayMember = "NomEsDx";
                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CargarCombobox" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion


        #region RadioButon

        private void RbPorSedes_CheckedChanged(object sender, EventArgs e)
        {
            MarSedes = 1;
            cboSedeVivi.Enabled = true;
        }

        private void RbTodasSedes_CheckedChanged(object sender, EventArgs e)
        {
            MarSedes = 2;
            cboSedeVivi.Enabled = false;
        }

        private void RbTodosRegistros_CheckedChanged(object sender, EventArgs e)
        {
            MarLimRegis = 1;
            CboGrupEspRegis.Enabled = false;
        }

        private void RbPorLista_CheckedChanged(object sender, EventArgs e)
        {
            MarLimRegis = 2;
            CboGrupEspRegis.Enabled = true;
        }
        #endregion

        #region Funciones
        private void DatosDeLaEmpresa()
        {
            try
            {
                txtDocuIps.Text = Utils.nitEmpresa;
                txtNombreIps.Text = Utils.nomEmpresa;
                txtTipoDocuIps.Text = Utils.tipoDocEmp;
                txtTeleIPS.Text = Utils.TelEmpresa;
                TxtCodMinSalud.Text = Utils.codMinSalud;
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


        #endregion

        #region ComboBox

        private void cboNameEntidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cboNameEntidades.Items.Count > 0)
                {


                    Utils.SqlDatos = "SELECT CodInterno, NomAdmin, TipoDocumento, NitCC, CodAdmin  " +
                    " FROM [Datos administradoras de planes] WHERE CodInterno  = '" + cboNameEntidades.SelectedValue + "' " +
                    " ORDER BY NomAdmin ";




                    SqlDataReader sqlDataReader = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        this.txtCardinal.Text = sqlDataReader["CodInterno"].ToString();
                        this.txtTipoDocu.Text = sqlDataReader["TipoDocumento"].ToString();
                        this.txtDocumento.Text = sqlDataReader["NitCC"].ToString();
                        this.txtRips.Text = sqlDataReader["CodAdmin"].ToString();


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

        #region Texbox
        private void txtCardinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (int)Keys.Enter)
                {
                    if (string.IsNullOrWhiteSpace(txtCardinal.Text) == false)
                    {

                        string SqlEmTer;



                        SqlEmTer = "SELECT CodInterno, NomAdmin, TipoDocumento, NitCC, CodAdmin  " +
                                    " FROM [Datos administradoras de planes] WHERE CodInterno = '" + txtCardinal.Text + "' " +
                                    " ORDER BY NomAdmin ";



                        SqlDataReader sqlDataReader = Conexion.SQLDataReader(SqlEmTer);

                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();

                            this.txtCardinal.Text = sqlDataReader["CodInterno"].ToString();
                            this.txtTipoDocu.Text = sqlDataReader["TipoDocumento"].ToString();
                            this.txtDocumento.Text = sqlDataReader["NitCC"].ToString();
                            cboNameEntidades.SelectedValue = sqlDataReader["CodInterno"].ToString();
                            this.txtRips.Text = sqlDataReader["CodAdmin"].ToString();


                        }
                        else
                        {
                            this.txtCardinal.Text = null;
                            this.txtTipoDocu.Text = null;
                            this.txtDocumento.Text = null;
                            cboNameEntidades.Text = "";
                            this.txtRips.Text = null;
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

      
        private void FrmExportarSedaripsTodos_Load(object sender, EventArgs e)
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
                Utils.Informa += "al cargar el formulario FrmExportar Sedarips Todos" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {

                string Coenti02 = null, TDE = null, NCC = null, SqlEmTer = null, Para01 = null, Para07 = null, FecIni = null, FecFin = null, TipSede = null, PreSedBus = null;




                Para07 = txtNombreIps.Text;
            
                if(Para07.Length > 60)
                {
                    Para07 = Para07.Substring(0, 60);
                }



                FecIni = DateInicial.Value.ToString("yyyy-MM-dd");
                FecFin = DateFinal.Value.ToString("yyyy-MM-dd");

                if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "nombre de la entidad de los RIPS a reportar" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    //Cargamos los datos de la entidad


                    SqlEmTer = "SELECT CodInterno, NomAdmin, TipoDocumento, NitCC, CodAdmin  " +
                                " FROM [Datos administradoras de planes] WHERE CodInterno = '" + cboNameEntidades.SelectedValue + "' ";

                    SqlDataReader sqlDataReader2 = Conexion.SQLDataReader(SqlEmTer);

                    if (sqlDataReader2.HasRows)
                    {
                        sqlDataReader2.Read();
                        Coenti02 = sqlDataReader2["CodInterno"].ToString();
                        TDE = sqlDataReader2["TipoDocumento"].ToString();
                        NCC = sqlDataReader2["NitCC"].ToString();

                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();


                } //Fin  if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)


                switch (MarSedes)
                {
                    case 1: //Por sede



                        if (string.IsNullOrWhiteSpace(cboSedeVivi.SelectedValue.ToString()) == true || cboSedeVivi.SelectedIndex == -1)
                        {
                            Utils.Titulo01 = "Control de errores de ejecución";
                            Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                            Utils.Informa += "nel nombre de la sede a mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string SqlSedesIns = "SELECT CodSede, NomSede, PrefiFac, TipSede, HabilSede " +
                         "FROM [BDADMINSIG].[dbo].[Datos sedes de instalacion] " +
                         "WHERE  (HabilSede = 'True') AND CodSede = '" + cboSedeVivi.SelectedValue.ToString() + "'";

                        SqlDataReader dataSet2 = Conexion.SQLDataReader(SqlSedesIns);

                        if (dataSet2.HasRows)
                        {
                            dataSet2.Read();

                            TipSede = dataSet2["TipSede"].ToString();
                            PreSedBus = dataSet2["PrefiFac"].ToString();
                        }
                        else
                        {
                            return;
                        }

                        //aquiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii quede 




                        break;
                    default:
                        break;
                }



            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de hacer click en el boton mostrar" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
