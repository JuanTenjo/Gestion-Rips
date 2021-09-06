using Gestion_Rips.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gestion_Rips.Forms.RipsPorRegimen;
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

        private void BuscarFactura(string NumFact)
        {
            try
            {
                //DataGridView.Rows.Insert(0, txtNombre.Text, txtDireccion.Text, txtTelefono.Text);

                Utils.Titulo01 = "Control de seleccionar facturas";
                string Coenti01 = null, NDO = null, CardiTer = null, UsSel = null, Para02 = null, Para03 = null;


                Coenti01 = cboNameEntidades.SelectedValue.ToString();
                CardiTer = cboNameEntidades.SelectedValue.ToString();


                if (string.IsNullOrWhiteSpace(Coenti01) || string.IsNullOrEmpty(Coenti01))
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "el nombre de la entidad a mostrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                NDO = txtBusquedaFactura.Text;


                Utils.Informa = "¿Usted desea agregar la factura número " + NDO + "," + "\r";
                Utils.Informa += " al listado destino para RIP?" + "\r";

                var Respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Respuesta == DialogResult.Yes)
                {


                    string SqlFacturas = "SELECT 1 as Estado,  [Datos de las facturas realizadas].NumFactura, Format([Datos de las facturas realizadas].FechaFac, 'dd-MM-yyyy') as FechaFac, ";
                    SqlFacturas = SqlFacturas + "[Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, ";
                    SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago, [Datos sedes de instalacion].TipSede ";


                    SqlFacturas = SqlFacturas + "FROM [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] INNER JOIN [ACDATOXPSQL].[dbo].[Datos empresas y terceros] ON ";
                    SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN ";
                    SqlFacturas = SqlFacturas + "[ACDATOXPSQL].[dbo].[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = ";
                    SqlFacturas = SqlFacturas + "[Datos cuentas de consumos].CuenNum LEFT OUTER JOIN BDADMINSIG.dbo.[Datos sedes de instalacion] ON ";
                    SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].PrefiFac = BDADMINSIG.dbo.[Datos sedes de instalacion].PrefiFac ";

                    SqlFacturas = SqlFacturas + "WHERE ([Datos de las facturas realizadas].AnuladaFac = 'False') AND ([Datos cuentas de consumos].DefiCuenta <> N'0') AND [Datos de las facturas realizadas].NumFactura = '" + NumFact + "'  ";

                    SqlDataReader sqlDataReader = Conexion.SQLDataReader(SqlFacturas);

                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();

                        switch (MarSedes)
                        {
                            case 1:

                                if (sqlDataReader["TipSede"].ToString() == TipoSede)
                                {



                                    DataGridFacturas.Rows.Insert(0, sqlDataReader["Estado"], sqlDataReader["NumFactura"].ToString(), sqlDataReader["FechaFac"].ToString(), sqlDataReader["NomAdmin"].ToString(), sqlDataReader["CodiMinSalud"].ToString(), sqlDataReader["ValorFac"], sqlDataReader["Copago"]);

                                    Utils.Informa = "La factura " + NumFact + " se agrego correctamente" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    DataGridFacturas.CurrentCell = DataGridFacturas.Rows[0].Cells[0]; //mover a la foco 0 de la fila a la fila 0
                                    DataGridFacturas.Rows[0].Selected = true; //Seleccionamos la fila que encontramos

                                }
                                else
                                {
                                    Utils.Informa = "Lo siento pero el número de factura " + "\r";
                                    Utils.Informa += "no se pertenece a la sede seleccionada " + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                }

                                break;
                            case 2:

                                DataGridFacturas.Rows.Insert(0, sqlDataReader["Estado"], sqlDataReader["NumFactura"].ToString(), sqlDataReader["FechaFac"].ToString(), sqlDataReader["NomAdmin"].ToString(), sqlDataReader["CodiMinSalud"].ToString(), sqlDataReader["ValorFac"], sqlDataReader["Copago"]);

                                Utils.Informa = "La factura " + NumFact + " se agrego correctamente" + "\r";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                                DataGridFacturas.CurrentCell = DataGridFacturas.Rows[0].Cells[0]; //mover a la foco 0 de la fila a la fila 0
                                DataGridFacturas.Rows[0].Selected = true; //Seleccionamos la fila que encontramos

                                break;
                            default:

                                break;
                        }

                    }
                    else
                    {
                        Utils.Informa = "Lo siento pero el número de factura " + "\r";
                        Utils.Informa += "no se encuentra en el este sistema o esta anulada " + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();

                    CalcularTotalFactura();


                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función BuscarFactura" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //cAMBIA cONSULTA


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
                                             "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecProce"]).ToString("yyyy-MM-dd") + "',102)," +
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

                            string data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos archivo de recien nacido]" +
                            "(NumRemi," +
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
                            if (string.IsNullOrWhiteSpace(TabLocal["FecMuerte"].ToString())) //si la fecha viene null termine el insert aqui
                            {
                                data += ")";
                            }
                            else
                            {
                                data += ",FecMuerte," +
                                        "HorMuerte)";
                            }

                            data += "VALUES(" +
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
                                     "'" + TabLocal["DxRecien"].ToString() + "'," +
                                     "'" + TabLocal["DxMuerte"].ToString() + "'";
                            if (string.IsNullOrWhiteSpace(TabLocal["FecMuerte"].ToString())) //si la fecha viene null termine el insert aqui
                            {
                                data += ")";
                            }
                            else
                            {
                                data += ",'" + Convert.ToDateTime(TabLocal["FecMuerte"]).ToString("yyyy-MM-dd") + "'," +
                                    "'" + Convert.ToDateTime(TabLocal["HorMuerte"]).ToString("hh:mm:ss") + "')";
                            }

                            SqlInsert = Conexion.SqlInsert(data);

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
                                              "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecIngresa"]).ToString("yyyy-MM-dd") + "',102)," +
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
                                              "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecSalida"]).ToString("yyyy-MM-dd") + "',102)," +
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
                                             "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecIngresa"]).ToString("yyyy-MM-dd") + "',102)," +
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
                                             "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecSalida"]).ToString("yyyy-MM-dd") + "',102)," +
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
                                             "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecConsul"]).ToString("yyyy-MM-dd") + "',102)," +
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
                            else
                            {
                                return -1;
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
                                           "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecFactur"]).ToString("yyyy-MM-dd") + "',102)," +
                                           "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecInicio"]).ToString("yyyy-MM-dd") + "',102)," +	
                                           "CONVERT(DATETIME,'" + Convert.ToDateTime(TabLocal["FecFinal"]).ToString("yyyy-MM-dd") + "',102)," +	
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

                Utils.SqlDatos = "SELECT * FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] where NumRemi = '" + CI + "'";

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
                            Utils.SqlDatos = "UPDATE [DARIPSESSQL].[dbo].[Datos contadores sedas] SET [ConsRemi] = '" + Fac + "', [UsarRemi] = '" + US + "', FecRemi = CONVERT(DATETIME,'" + Date + "',102)";

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
                SqlDatos = SqlDatos + "WHERE ((([Datos listado de diagnosticos].[CodiDx]) = '" + CoDx + "')) ";
                SqlDatos = SqlDatos + "ORDER BY [Datos listado de diagnosticos].[CodiDx];";


                SqlDataReader TablaAux9;

                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command2 = new SqlCommand(SqlDatos, connection2);
                    command2.Connection.Open();
                    TablaAux9 = command2.ExecuteReader();

                    if (TablaAux9.HasRows == false)
                    {
                        return "0";
                    }
                    else
                    {
                        TablaAux9.Read();
                        string NombreDx = TablaAux9["NombreDx"].ToString();
                        return NombreDx;
                    }
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

                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();
                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabProce["DxPrincipal"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                            }

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

                      
                    }

                    TabProce.Close();
                    return 1;
                }
  

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

                SqlDataReader TabReNan;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(SqlReNaciTemp, connection);
                    command.Connection.Open();
                    TabReNan = command.ExecuteReader();

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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command2 = new SqlCommand(Utils.SqlDatos, connection2);
                                            command2.Connection.Open();
                                            TablaAux1 = command2.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabReNan["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                            }

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

                SqlDataReader TabOtros;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(SqlOtrosTemp, connection);
                    command.Connection.Open();
                    TabOtros = command.ExecuteReader();

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

                SqlDataReader TabObserva;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(SqlMediTem, connection);
                    command.Connection.Open();
                    TabObserva = command.ExecuteReader();

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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command2 = new SqlCommand(Utils.SqlDatos, connection2);
                                            command2.Connection.Open();
                                            TablaAux1 = command2.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabObserva["DxPrincIngre"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                            }
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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command2 = new SqlCommand(Utils.SqlDatos, connection2);
                                            command2.Connection.Open();
                                            TablaAux1 = command2.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabObserva["DxRelacion1"].ToString() + ", del diagnóstico DxRelacion1 no existe la resolución vigente.";
                                            }

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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command2 = new SqlCommand(Utils.SqlDatos, connection2);
                                            command2.Connection.Open();
                                            TablaAux1 = command2.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabObserva["DxRelacion2"].ToString() + ", del diagnóstico DxRelacion2 no existe la resolución vigente.";
                                            }
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

                                                SqlDataReader TablaAux1;

                                                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                                                {
                                                    SqlCommand command2 = new SqlCommand(Utils.SqlDatos, connection2);
                                                    command2.Connection.Open();
                                                    TablaAux1 = command2.ExecuteReader();

                                                    if (TablaAux1.HasRows == false)
                                                    {
                                                        RegExp = 1;
                                                        ObErr = ObErr + "El código " + TabObserva["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                                    }

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

                SqlDataReader TabMedi;

                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command = new SqlCommand(SqlMediTem, connection);
                    command.Connection.Open();
                    TabMedi = command.ExecuteReader();


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




                SqlDataReader TabHospi;

                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command2 = new SqlCommand(SqlConsulTem, connection2);
                    command2.Connection.Open();

                    TabHospi = command2.ExecuteReader();

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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxPrincIngre"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                            }

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

             
                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxPrincEgre"].ToString() + ", el diagnóstico principal de egreso no existe la resolución vigente.";
                                            }
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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxRelacion1"].ToString() + ", del diagnóstico relacional 1 no existe la resolución vigente.";
                                            }
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

                      
                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 no existe la resolución vigente.";
                                            }

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

                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxRelacion3"].ToString() + ", del diagnóstico  Relacional 3 no existe la resolución vigente.";
                                            }

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

                    
                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();

                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabHospi["DxComplica"].ToString() + ", del diagnóstico  Relacional 3 no existe la resolución vigente.";
                                            }
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


                                                SqlDataReader TablaAux1;

                                                using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                                {
                                                    SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                                    command.Connection.Open();

                                                    TablaAux1 = command.ExecuteReader();

                                                    if (TablaAux1.HasRows == false)
                                                    {
                                                        RegExp = 1;
                                                        ObErr = ObErr + "El código " + TabHospi["DxMuerte"].ToString() + ", del diagnóstico de la causa básica no existe en la resolución vigente.";
                                                    }
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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();
                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false) //aqui
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabConsul["DxPrincipal"].ToString() + ", del diagnóstico principal no existe la resolución vigente.";
                                            }

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


                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();
                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabConsul["DxRelacion1"].ToString() + ", del diagnóstico relacional 1 no existe la resolución vigente.";
                                            }

                                            TablaAux1.Close();
                                        }               

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

                             

                                        SqlDataReader TablaAux1;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(Utils.SqlDatos, connection);
                                            command.Connection.Open();
                                            TablaAux1 = command.ExecuteReader();

                                            if (TablaAux1.HasRows == false)
                                            {
                                                RegExp = 1;
                                                ObErr = ObErr + "El código " + TabConsul["DxRelacion2"].ToString() + ", del diagnóstico  Relacional 2 1 no existe la resolución vigente.";
                                            }

                                        }
                                        if (Conexion.sqlConnection.State == ConnectionState.Open) Conexion.sqlConnection.Close();
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

                SqlDataReader TabLoc;

                using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                {
                    SqlCommand command2 = new SqlCommand(SqlFacTemp, connection2);
                    command2.Connection.Open();
                    TabLoc = command2.ExecuteReader();

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

                        TabLoc.Close();

                        return 1;
                        
                    }//Final TabLocal

                }

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
        private void CalcularTotalFactura()
        {
            try
            {
                Int32 CantidadFact = 0;
                Int32 CantidadFactMarcadas = 0;
                decimal TolFacturado = 0;
                decimal TolFacturadoMarca = 0;
                decimal TolCopago = 0;
                decimal TolCopagoMarca = 0;
                TxtCanFacMos.Clear();
                TxtCanFacMar.Clear();
                TxtTolFacMos.Clear();
                TolFacMar.Clear();
                TxtTolCopMos.Clear();
                TxtTolCopMar.Clear();

                foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                {

                    int Estado = Convert.ToInt32(Row.Cells["Estado"].Value);
                    if (Estado == 1)
                    {
                        CantidadFactMarcadas += 1;
                        TolFacturadoMarca = TolFacturadoMarca + Convert.ToDecimal(Row.Cells["VrFactura"].Value);
                        TolCopagoMarca = TolCopagoMarca + Convert.ToDecimal(Row.Cells["VrCopago"].Value);
                    }

                    CantidadFact += 1;
                    TolFacturado = TolFacturado + Convert.ToDecimal(Row.Cells["VrFactura"].Value);
                    TolCopago = TolCopago + Convert.ToDecimal(Row.Cells["VrCopago"].Value);

                }


                TxtCanFacMos.Text = CantidadFact.ToString();
                TxtCanFacMar.Text = CantidadFactMarcadas.ToString();

                TxtTolFacMos.Text = string.Format("{0:C2}", TolFacturado);
        
                TolFacMar.Text = string.Format("{0:C2}", TolFacturadoMarca);
                TxtTolCopMos.Text = string.Format("{0:C2}", TolCopago);
                TxtTolCopMar.Text = string.Format("{0:C2}", TolCopagoMarca);



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
        #endregion

        #region Texbox y button

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        private void txtBusquedaFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (int)Keys.Enter)
                {

                    if (string.IsNullOrWhiteSpace(txtBusquedaFactura.Text) == false)
                    {

                        if (DataGridFacturas.CurrentCell.RowIndex == -1)
                        {
                            return;
                        }

                        string Filtro = txtBusquedaFactura.Text;
                        int RowSelect;
                        bool contadorEncontro = false;

                        foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                        {
                            string NumFactura = Convert.ToString(Row.Cells["NumCuenFac"].Value);

                            if (string.IsNullOrWhiteSpace(Filtro) == false)
                            {
                                if (Filtro == NumFactura)
                                {

                                    contadorEncontro = true;

                                    RowSelect = DataGridFacturas.CurrentRow.Index;

                                    Boolean estado = Convert.ToBoolean(Row.Cells["Estado"].Value);

                                    if (estado == false) //Valida si esta chekout, si no lo esta lo hacemos y si esta pues lo quitamos, simple.
                                    {
                                        //Hecho por JuanTenjo
                                        Row.Cells["Estado"].Value = true;
                                        Utils.Titulo01 = "Control de seleccion";
                                        Utils.Informa = "Se checkout la factura " + NumFactura + " correctamente " + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        DataGridFacturas.FirstDisplayedScrollingRowIndex = RowSelect; //Le dice que muestre a partir de la fila actual
                                        DataGridFacturas.Refresh(); //Refresca la grilla
                                        DataGridFacturas.CurrentCell = DataGridFacturas.Rows[RowSelect].Cells[0]; //mover a la foco 0 de la fila a la fila 0
                                        DataGridFacturas.Rows[RowSelect].Selected = true; //Seleccionamos la fila que encontramos
                                    }
                                    else
                                    {
                                        Row.Cells["Estado"].Value = false;
                                        Utils.Titulo01 = "Control de seleccion";
                                        Utils.Informa = "Se quito el checkout a la factura " + NumFactura + " " + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        DataGridFacturas.FirstDisplayedScrollingRowIndex = RowSelect; //Le dice que muestre a partir de la fila actual
                                        DataGridFacturas.Refresh(); //Refresca la grilla
                                        DataGridFacturas.CurrentCell = DataGridFacturas.Rows[RowSelect].Cells[0]; //mover a la foco 0 de la fila a la fila 0
                                        DataGridFacturas.Rows[RowSelect].Selected = true; //Seleccionamos la fila que encontramos
                                    }


                                    break; //Se hace para que cuando lo encutre no recorra mas filas.

                                }
                            }
                        }//Fin Foreach


                        if (!contadorEncontro)
                        {
                            BuscarFactura(txtBusquedaFactura.Text);
                        }

                    }
                    else
                    {
                        Utils.Informa = "No ha digitado ninguna factura" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al buscar y checkout la factura" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //NO SE NI QUE HACE
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
                        CR = TxtCodMinSalud.Text;
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


                    SqlDataReader reader;

                    using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                    {
                        SqlCommand command2 = new SqlCommand(Utils.SqlDatos, connection2);

                        command2.Connection.Open();

                        reader = command2.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            TolInco = Convert.ToInt32(reader["CuenCodEnti"].ToString());
                        }
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

                            Utils.infNombreInforme = "InfReporErroresRips.rdlc";

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

                    Coenti02 = txtCardinal.Text;
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                            "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                            "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti02 + "'";

                    SqlDataReader sqlDataReader2 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (sqlDataReader2.HasRows)
                    {
                        sqlDataReader2.Read();
                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();
                        NEenti = sqlDataReader2["NP"].ToString();

                    }

                    sqlDataReader2.Close();
                    sqlDataReader2 = null;

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
                        MT = sqlDataReader2["ManualTari"].ToString();
                        TDE = sqlDataReader2["TipoDocu"].ToString();
                        NCC = sqlDataReader2["NumDocu"].ToString();
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
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        break;
                    case "1": //'No existe la ruta
                        Utils.Informa = "Lo siento pero los archivos de SEDAS-RIPS" + "\r";
                        Utils.Informa += "no se han encontrado en la ruta de datos" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                Utils.Informa += " al programa SEDAS-RIPS- Especial.?" + "\r";
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
                                    "'" + TxtCodMinSalud.Text + "'," +
                                    "'" + CR + "'," +
                                    "CONVERT(DATETIME,'" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "',102)," +
                                    "'" + lblNombreUser.Text + "'," +
                                    "CONVERT(DATETIME,'" + Convert.ToDateTime(Periodo1).ToString("yyyy-MM-dd") + "',102)," +
                                    "CONVERT(DATETIME,'" + Convert.ToDateTime(Periodo2).ToString("yyyy-MM-dd") + "',102)," +
                                    "'" + TolFac + "'," +
                                    "'" + txtTeleIPS.Text + "'," +
                                    "'" + 0 + "'," +
                                    "'" + 0 + "'," +
                                    "'" + 0 + "'," +
                                    "'" + 0 + "'," +
                                    "'" + UsGra + "'," +
                                    "CONVERT(DATETIME,'" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "',102)" +
                                    ")";

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

                                    string ReporUser = "SELECT [Datos temporal usuarios RIPS].CodDigita, [Datos temporal usuarios RIPS].NumRemi, [Datos temporal usuarios RIPS].CodAdmin, [Datos temporal usuarios RIPS].TipoDocum, [Datos temporal usuarios RIPS].NumDocum, [Datos temporal usuarios RIPS].TipUsuario, [Datos temporal usuarios RIPS].Apellido1, [Datos temporal usuarios RIPS].Apellido2, [Datos temporal usuarios RIPS].Nombre1, [Datos temporal usuarios RIPS].Nombre2, [Datos temporal usuarios RIPS].Edad, [Datos temporal usuarios RIPS].EdadMedi, [Datos temporal usuarios RIPS].Sexo, [Datos temporal usuarios RIPS].CodDpto, [Datos temporal usuarios RIPS].CodMuni, [Datos temporal usuarios RIPS].ZonaResi, RTrim([Datos empresas y terceros].[NomAdmin] + ' ' + [Datos empresas y terceros].[ProgrAmin]) AS NoAdmin, [Datos empresas y terceros].NomPlan " +
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

                    if (TolConsul > 0)
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
        private void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {

                string Coenti02 = null, TituColum = null, GrupDx = null, SqlFacturas = null, TDE = null, NCC = null, SqlEmTer = null, Para01 = null, Para07 = null, FecIni = null, FecFin = null, PreSedBus = null;

                DialogResult res;

                int Vr = 0; ;

                Para07 = txtNombreIps.Text;

                if (Para07.Length > 60)
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
                                " FROM DARIPSESSQL.dbo.[Datos administradoras de planes] WHERE CodInterno = '" + cboNameEntidades.SelectedValue + "' ";

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

                            TipoSede = dataSet2["TipSede"].ToString();
                            PreSedBus = dataSet2["PrefiFac"].ToString();

                        }
                        else
                        {
                            return;
                        }


                        if (Convert.ToInt32(cboSedeVivi.SelectedValue.ToString()) == 1)
                        {

                            //Se suman todos los prefijos de todos los portatiles


                            //'************************************* se modifica el 23 de NOviembre de 2020 *************************



                            SqlFacturas = "SELECT 1 as Estado,  [Datos de las facturas realizadas].NumFactura, Format([Datos de las facturas realizadas].FechaFac, 'dd-MM-yyyy') as FechaFac, ";
                            SqlFacturas = SqlFacturas + "[Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].ValorFac , [Datos de las facturas realizadas].Copago ";


                            SqlFacturas = SqlFacturas + "FROM [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] INNER JOIN [ACDATOXPSQL].[dbo].[Datos empresas y terceros] ON ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN ";
                            SqlFacturas = SqlFacturas + "[ACDATOXPSQL].[dbo].[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = ";
                            SqlFacturas = SqlFacturas + "[Datos cuentas de consumos].CuenNum LEFT OUTER JOIN BDADMINSIG.dbo.[Datos sedes de instalacion] ON ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].PrefiFac = BDADMINSIG.dbo.[Datos sedes de instalacion].PrefiFac ";


                            SqlFacturas = SqlFacturas + "WHERE ([Datos de las facturas realizadas].FechaFac >= CONVERT(DATETIME, '" + FecIni + "', 102)) AND ";
                            SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].FechaFac <= CONVERT(DATETIME, '" + FecFin + "', 102)) AND ";
                            SqlFacturas = SqlFacturas + "(ISNULL(BDADMINSIG.dbo.[Datos sedes de instalacion].TipSede, " + TipoSede + ") = N'" + TipoSede + "') AND ";
                            SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].AnuladaFac = 'False') AND ([Datos cuentas de consumos].DefiCuenta <> N'0') ";
                            SqlFacturas = SqlFacturas + "ORDER BY [Datos de las facturas realizadas].PrefiFac, [Datos de las facturas realizadas].FechaFac, ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].NumFactura";
                        }
                        else
                        {
                            //Se busca por el prefijo de factura de cada sede

                            SqlFacturas = "SELECT 1 as Estado,  [Datos de las facturas realizadas].NumFactura, Format([Datos de las facturas realizadas].FechaFac, 'dd-MM-yyyy') as FechaFac, ";
                            SqlFacturas = SqlFacturas + "[Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago ";


                            SqlFacturas = SqlFacturas + "FROM  [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] INNER JOIN [ACDATOXPSQL].[dbo].[Datos empresas y terceros] ON ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN ";
                            SqlFacturas = SqlFacturas + "[ACDATOXPSQL].[dbo].[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum ";


                            SqlFacturas = SqlFacturas + "WHERE ([Datos de las facturas realizadas].FechaFac >= CONVERT(DATETIME, '" + FecIni + "', 102)) AND ";
                            SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].FechaFac <= CONVERT(DATETIME, '" + FecFin + "', 102)) AND ";
                            SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].AnuladaFac = 'False') AND ";
                            SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].PrefiFac = N'" + PreSedBus + "')  AND ([Datos cuentas de consumos].DefiCuenta <> N'0') ";


                            SqlFacturas = SqlFacturas + "ORDER BY [Datos de las facturas realizadas].PrefiFac, [Datos de las facturas realizadas].FechaFac, ";
                            SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].NumFactura ";


                        }

                        Utils.Titulo01 = "Control de ejecución";
                        Utils.Informa = "¿Usted desea mostrar todas las facturas" + "\r";
                        Utils.Informa += "en la sede " + cboSedeVivi.Text + "\r";
                        Utils.Informa += "entre  " + FecIni + " y el " + FecFin + "?" + "\r";


                        break;
                    case 2:

                        switch (MarLimRegis)
                        {
                            case 1://Todas las facturas
                                SqlFacturas = "SELECT 1 as Estado,  [Datos de las facturas realizadas].NumFactura, Format([Datos de las facturas realizadas].FechaFac, 'dd-MM-yyyy') as FechaFac, ";
                                SqlFacturas = SqlFacturas + "[Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, ";
                                SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago ";


                                SqlFacturas = SqlFacturas + "FROM [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] INNER JOIN [ACDATOXPSQL].[dbo].[Datos empresas y terceros] ON ";
                                SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN ";
                                SqlFacturas = SqlFacturas + "[ACDATOXPSQL].[dbo].[Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum ";


                                SqlFacturas = SqlFacturas + "WHERE ([Datos de las facturas realizadas].FechaFac >= CONVERT(DATETIME, '" + FecIni + "', 102)) AND ";
                                SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].FechaFac <= CONVERT(DATETIME, '" + FecFin + "', 102)) AND ";
                                SqlFacturas = SqlFacturas + "([Datos de las facturas realizadas].AnuladaFac = 'False') AND ([Datos cuentas de consumos].DefiCuenta <> N'0') ";
                                SqlFacturas = SqlFacturas + "ORDER BY [Datos de las facturas realizadas].PrefiFac, [Datos de las facturas realizadas].FechaFac, ";
                                SqlFacturas = SqlFacturas + "[Datos de las facturas realizadas].NumFactura";

                                Utils.Titulo01 = "Control de ejecución";
                                Utils.Informa = "¿Usted desea mostrar todas las facturas" + "\r";
                                Utils.Informa += "realizadas a todos regimenes de salud" + "\r";
                                Utils.Informa += "entre  " + FecIni + " y el " + FecFin + "?" + "\r";

                                break;
                            case 2: //Por lista de Dx


                                if (CboGrupEspRegis.SelectedIndex == -1 || string.IsNullOrEmpty(CboGrupEspRegis.SelectedValue.ToString()))
                                {
                                    Utils.Titulo01 = "Control de errores de ejecución";
                                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                                    Utils.Informa += "el nombre de la lista de Dx a mostrar" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                GrupDx = CboGrupEspRegis.SelectedValue.ToString();

                                SqlFacturas = "SELECT 1 as Estado,  NumFactura, Format(FechaFac, 'dd-MM-yyyy') as FechaFac, NomAdmin, CodiMinSalud, AVG(ValorFac) AS ValorFac, AVG(Copago) AS Copago ";
                                SqlFacturas = SqlFacturas + "FROM [ACDATOXPSQL].[dbo].FacturasDxEspecial ";
                                SqlFacturas = SqlFacturas + "WHERE (CodDetaDx = N'" + GrupDx + "') ";
                                SqlFacturas = SqlFacturas + "GROUP BY NumFactura, FechaFac, NomAdmin, CodiMinSalud ";
                                SqlFacturas = SqlFacturas + "HAVING (FechaFac >= CONVERT(DATETIME, '" + FecIni + "', 102)) and ";
                                SqlFacturas = SqlFacturas + "(FechaFac <= CONVERT(DATETIME, '" + FecFin + "', 102)) ";
                                SqlFacturas = SqlFacturas + " ORDER BY NumFactura";


                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }





                res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    //'Proceda a mostrar

                    DataGridFacturas.Rows.Clear();

                    SqlDataReader TabFacturas;

                    using (SqlConnection connection3 = new SqlConnection(Conexion.conexionSQL))
                    {
                        SqlCommand command3 = new SqlCommand(SqlFacturas, connection3);
                        command3.Connection.Open();
                        TabFacturas = command3.ExecuteReader();

                        if (TabFacturas.HasRows == false)
                        {
                            Utils.Titulo01 = "Control de ejecución";
                            Utils.Informa = "Lo siento pero en el rango de fechas" + "\r";
                            Utils.Informa += "seleccionado no se encuentran facturas" + "\r";
                            Utils.Informa += "ealizadas con los parametros digitados." + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            Vr = 0;
                            while (TabFacturas.Read())
                            {
                                Vr += 1;
                                DataGridFacturas.Rows.Add(TabFacturas["Estado"], TabFacturas["NumFactura"].ToString(), TabFacturas["FechaFac"].ToString(), TabFacturas["NomAdmin"].ToString(), TabFacturas["CodiMinSalud"].ToString(), TabFacturas["ValorFac"], TabFacturas["Copago"]);
                            }


                            CalcularTotalFactura();

                            Utils.Titulo01 = "Control de ejecución";
                            Utils.Informa = "Se han seleccionado " + Vr + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                    TabFacturas.Close();
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
        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.Titulo01 = "Control para seleccionar datos";

                string SqlEmTer = null, HisBus = null, CuenBus = null, SqlConsumos1 = null, ConMinRips = null, MT = null, FacturError = null, FacRipsEs = null,SqlFacturas = null, Coenti02 = null, SqlConsumos = null, TDE = null, NCC = null, CodERP = null, UsSel = null, NEnti = null;
                int FunEli = 0;
                double ValdetaFac = 0, CanFacSel = 0;
                string TipUsSel = null;
                string NumDocSel = null;
                string CexterAten = null;
                string Sqlsuarios = null;
                bool SqlInsert;
                string CodInterIPs = null;
                string GBus = null;
                string ClaSerBus = null;
                string CodTomo = null;
                int TolD = 0;

                string NumFac, DxEntra, DxSalida, DxRelac01, DxRelac02, DxRelac03, DxComplica, DxMuerte;
                double RegisConsul;
                DateTime FecEnPer;
                string PoliFactu;

                if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)
                {
                    Utils.Informa = "Lo siento pero usted aún no ha seleccionado" + "\r";
                    Utils.Informa += "nombre de la entidad de los RIPS a reportar" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    Coenti02 = txtCardinal.Text;
                    //Cargamos los datos de la entidad
                    Utils.SqlDatos = "SELECT CarAdmin, ([NomAdmin] + ' ' + [ProgrAmin]) AS NP, TipoDocu, NumDocu , CodiMinSalud, ManualTari, RegimenAdmin, ActiReali, PerEmpre " +
                                            "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] WHERE ((([ACDATOXPSQL].[dbo].[Datos empresas y terceros].PerEmpre) = 1) AND(([ACDATOXPSQL].[dbo].[Datos empresas y terceros].HabilEmp) = 1)) " +
                                            "AND ([NomAdmin] + ' ' + [ProgrAmin]) is not null AND CarAdmin = '" + Coenti02 + "'";

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

                } //Fin  if (string.IsNullOrWhiteSpace(cboNameEntidades.SelectedValue.ToString()) == true || cboNameEntidades.SelectedIndex == -1)

                if (string.IsNullOrWhiteSpace(txtRips.Text))
                {
                    Utils.Informa = "Lo siento pero la entidad seleccionada no tiene" + "\r";
                    Utils.Informa += "definido el código RIPS para reportar los registros." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CodERP = txtRips.Text;

      

                if(Convert.ToInt32(TxtCanFacMar.Text) == 0)
                {
                    Utils.Informa = "Lo siento pero mientras no se tengan" + "\r";
                    Utils.Informa += "facturas seleccionadas para registrar" + "\r";
                    Utils.Informa += "los RIPS de todos los regimenes no se podra seleccionar" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsSel = lblCodigoUser.Text;

                Utils.Informa = "¿Usted desea seleccionar los datos necesarios " + "\r";
                Utils.Informa += "para realizar los RIPS de la entidad" + "\r";
                Utils.Informa += NEnti + "?" + "\r";
                Utils.Informa += "Son: " + TxtCanFacMar.Text + " Facturas para rips de " + TxtCanFacMos.Text + "\r";
                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if(res == DialogResult.Yes)
                {
                    FunEli = ElimdatosRIPS(UsSel, Coenti02); //'****************** Borrar lo que tenga la tabla Temporal de RIPS ***************************
                
                    if(FunEli == -1)
                    {
                        return;
                    }


                    //Corra el proceso de selección de datos por facturas

                    SqlConsumos = "SELECT [Datos cuentas de consumos].CuenNum, [Datos del Paciente].TipoIden, ";
                    SqlConsumos += "[Datos del Paciente].NumIden, [Datos registros de consumos].FechaCon, ";
                    SqlConsumos += "[Datos cuentas de consumos].NumRemi, [Datos registros de consumos].CodiSOAT, ";
                    SqlConsumos += "[Datos registros de consumos].CodiISS, [Datos registros de consumos].CodiCUPS, ";
                    SqlConsumos += "[Datos registros de consumos].CodInter, [Datos registros de consumos].CodInter, ";
                    SqlConsumos += "[Datos registros de consumos].FinalConsul, [Datos cuentas de consumos].CausaExterna, ";
                    SqlConsumos += "[Datos cuentas de consumos].DxSalida, [Datos cuentas de consumos].DxRelac01, [Datos cuentas de consumos].DxRelac02, ";
                    SqlConsumos += "[Datos cuentas de consumos].DxRelac03, [Datos cuentas de consumos].TipoDxPrin, [Datos registros de consumos].ValorUnitario, [Datos registros de consumos].SubValorUnita, ";
                    SqlConsumos += "[Datos registros de consumos].Copagos, ([ValorUnitario]-[Copagos]) AS VN, [Datos catalogo de servicios].GrupoServi, ";

                    SqlConsumos += "[Datos cuentas de consumos].DxComplica,";
                    SqlConsumos += "[Datos registros de consumos].Cantidad, ";
                    SqlConsumos += "[Datos registros de consumos].RealizadoEn, ";
                    SqlConsumos += "[Datos registros de consumos].FinalProce, ";
                    SqlConsumos += "[Datos registros de consumos].PerAtien, ";
                    SqlConsumos += "[Datos registros de consumos].FormaRealiza, ";
                    SqlConsumos += "[Datos registros de consumos].AutoriNum, ";
                    SqlConsumos += "[Datos catalogo de servicios].NomServicio, [Datos catalogo de servicios].ClasiSer, ";
                    SqlConsumos += "[Datos catalogo de servicios].CodiMedMin, ";
                    SqlConsumos += "[Datos catalogo de servicios].PosMedi, ";
                    SqlConsumos += "[Datos catalogo de servicios].CodInterno, ";

                    SqlConsumos += "[Datos del Paciente].HistorPaci, ";
                    SqlConsumos += "[Datos del Paciente].Apellido1, ";
                    SqlConsumos += "[Datos del Paciente].Apellido2, ";
                    SqlConsumos += "[Datos del Paciente].Nombre1, ";
                    SqlConsumos += "[Datos del Paciente].Nombre2, ";
                    SqlConsumos += "[Datos cuentas de consumos].TipoUsuario, [Datos cuentas de consumos].DxEntra,";
                    SqlConsumos += "[Datos cuentas de consumos].TipoCuenta, ";
                    SqlConsumos += "[Datos cuentas de consumos].DiasEstancias, ";
                    SqlConsumos += "[Datos cuentas de consumos].ServiRips,  [Datos cuentas de consumos].HorasObser,";
                    SqlConsumos += "[Datos cuentas de consumos].FecEntrada, [Datos cuentas de consumos].HorEntrada, ";

                    SqlConsumos += "[Datos cuentas de consumos].EstaSalida, [Datos cuentas de consumos].FecSalida, ";
                    SqlConsumos += "[Datos cuentas de consumos].Destino, ";
                    SqlConsumos += "[Datos cuentas de consumos].HorSalida, [Datos cuentas de consumos].DxMuerte, ";

                    SqlConsumos += "[Datos del Paciente].Sexo, [Datos del Paciente].ZonaResiden, ";
                    SqlConsumos += "[Datos del Paciente].CodDpto, [Datos del Paciente].CodMuni, ";

                    SqlConsumos += "[Datos cuentas de consumos].ValorEdad, ";
                    SqlConsumos += "[Datos cuentas de consumos].UnidadEdad, ";
                    SqlConsumos += "[Datos cuentas de consumos].NumPoliza ";



                   SqlConsumos += "FROM [ACDATOXPSQL].[dbo].[Datos catalogo de servicios] INNER JOIN ([ACDATOXPSQL].[dbo].[Datos del Paciente] INNER JOIN ";
                   SqlConsumos += "([ACDATOXPSQL].[dbo].[Datos cuentas de consumos] INNER JOIN [ACDATOXPSQL].[dbo].[Datos registros de consumos] ON ";
                   SqlConsumos += "[Datos cuentas de consumos].CuenNum = [Datos registros de consumos].CuenConsu) ON ";
                   SqlConsumos += "[Datos del Paciente].HistorPaci = [Datos cuentas de consumos].HistoNum) ON ";
                   SqlConsumos += "[Datos catalogo de servicios].CodInterno = [Datos registros de consumos].CodInter ";

                    BarraSeleccionar.Minimum = 1;
                    BarraSeleccionar.Maximum = Convert.ToInt32(TxtCanFacMar.Text);


                    foreach (DataGridViewRow Row in DataGridFacturas.Rows)
                    {

                        int Estado = Convert.ToInt32(Row.Cells["Estado"].Value);

                        if (Estado == 1)
                        {
                            //Tomamos el primer número de factura
                            FacRipsEs = Convert.ToString(Row.Cells["NumCuenFac"].Value);





                            SqlFacturas = "SELECT [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, ";
                            SqlFacturas += "[Datos de las facturas realizadas].NumCuenFac, [Datos empresas y terceros].NomAdmin, ";
                            SqlFacturas += "[Datos de las facturas realizadas].Cartercero, [Datos empresas y terceros].NomPlan, ";
                            SqlFacturas += "[Datos de las facturas realizadas].NumContra, [Datos de las facturas realizadas].ValorFac, ";
                            SqlFacturas += "[Datos de las facturas realizadas].Copago, [Datos empresas y terceros].CodiMinSalud, ";
                            SqlFacturas += "[Datos de las facturas realizadas].ValorTotal, [Datos empresas y terceros].ManualTari, ";
                            SqlFacturas += "[Datos de las facturas realizadas].AnuladaFac ";
                            SqlFacturas += "FROM [ACDATOXPSQL].[dbo].[Datos empresas y terceros] INNER JOIN [ACDATOXPSQL].[dbo].[Datos de las facturas realizadas] ON ";
                            SqlFacturas += "[Datos empresas y terceros].CarAdmin = [Datos de las facturas realizadas].Cartercero ";
                            SqlFacturas += "WHERE (([Datos de las facturas realizadas].NumFactura) = '" + FacRipsEs + "')";


                            SqlDataReader TabFacturas;

                            using (SqlConnection connection2 = new SqlConnection(Conexion.conexionSQL))
                            {
                                SqlCommand command2 = new SqlCommand(SqlFacturas, connection2);
                                command2.Connection.Open();
                                TabFacturas = command2.ExecuteReader();


                                if(TabFacturas.HasRows == false)
                                {
                                    Utils.Informa = "Lo siento pero en el número de factura " + FacRipsEs + "\r";
                                    Utils.Informa += "no se encontró en la base de datos de este sistema," + "\r";
                                    Utils.Informa += "por lo tanto queda excluida de la selección RIPS." + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    TabFacturas.Read();

                                    if(Convert.ToBoolean(TabFacturas["AnuladaFac"]) == true)
                                    {
                                        Utils.Informa = "Lo siento pero en el número de factura " + FacRipsEs + "\r";
                                        Utils.Informa += "se encuentra anulada en este sistema, por lo tanto" + "\r";
                                        Utils.Informa += "por lo tanto queda excluida de la selección RIPS." + "\r";
                                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        ValdetaFac = 0; //En esta variable se va ha registrar los valores de detalle de cada factura para después auditar qquien tiene descuadre
                                        CanFacSel += 1; //Cantidad de facturas seleccionadas

                                        CuenBus = TabFacturas["NumCuenFac"].ToString();
                                        ConMinRips = TabFacturas["CodiMinSalud"].ToString();
                                        FacturError = TabFacturas["NumFactura"].ToString(); //Si se presenta n error, el sistema muestra el número de la factura en la cual sucede el error
                                        MT = TabFacturas["ManualTari"].ToString(); //Tipo de manual


                                        SqlConsumos1 = SqlConsumos + "WHERE ((([Datos cuentas de consumos].CuenNum) = '" + CuenBus + "' ) and ";
                                        SqlConsumos1 += "(([Datos registros de consumos].SeRepRips) = 'True' ) And ";
                                        SqlConsumos1 += "(([Datos registros de consumos].PagaHoja) = 'True') AND (([Datos registros de consumos].Cantidad) > 0) ";
                                        SqlConsumos1 += "AND (([Datos registros de consumos].ValorUnitario + [Datos registros de consumos].SubValorUnita) > 0)) ";
                                        SqlConsumos1 += "ORDER BY [Datos cuentas de consumos].CuenNum, [Datos catalogo de servicios].GrupoServi ";


                                        SqlDataReader TabConsumos;

                                        using (SqlConnection connection = new SqlConnection(Conexion.conexionSQL))
                                        {
                                            SqlCommand command = new SqlCommand(SqlConsumos1, connection);
                                            command.Connection.Open();
                                            TabConsumos = command.ExecuteReader();

                                            if(TabConsumos.HasRows == false)
                                            {
                                                //Por integridad dificilmente entra aqui
                                            }
                                            else
                                            {
                                                TabConsumos.Read();

                                                //Agregamos los estaticos
                                                //usuarios

                                                 TipUsSel = TabConsumos["TipoIden"].ToString();
                                                 NumDocSel = TabConsumos["NumIden"].ToString();
                                                 CexterAten = TabConsumos["CausaExterna"].ToString(); //Causa externa de la atención

                                                 Sqlsuarios = "SELECT [Datos temporal usuarios RIPS].* " +
                                                            "FROM [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS] " +
                                                            "WHERE ((([Datos temporal usuarios RIPS].CodDigita) = '" + UsSel + "' ) And " +
                                                            "(([Datos temporal usuarios RIPS].NumRemi) = '" + Coenti02 + "' ) And " +
                                                            "(([Datos temporal usuarios RIPS].TipoDocum) = '" + TipUsSel + "' ) And " +
                                                            "(([Datos temporal usuarios RIPS].NumDocum) = '" + NumDocSel + "')) ";

                                                SqlDataReader TabUsuarios;

                                                using (SqlConnection connection3= new SqlConnection(Conexion.conexionSQL))
                                                {
                                                    SqlCommand command3 = new SqlCommand(Sqlsuarios, connection3);
                                                    command3.Connection.Open();
                                                    TabUsuarios = command3.ExecuteReader();

                                                    if (TabUsuarios.HasRows == false)
                                                    {
                                                        string codMuni = TabConsumos["CodMuni"].ToString();

                                                        if (codMuni.Length > 3 && codMuni.Length == 5)
                                                        {
                                                            codMuni = codMuni.Substring(2, 3);
                                                        }
                                                        else
                                                        {
                                                            Utils.Titulo01 = "Control de inserccion";
                                                            Utils.Informa = "No se pudo cortar el codigo del municipio" + "\r";
                                                            Utils.Informa += "en los ultimos 3 caracteres" + "\r";
                                                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            return;
                                                        }


                                                        string data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal usuarios RIPS]" +
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
                                                                    "ZonaResi)" +
                                                                     "VALUES(" +
                                                                     "'" + UsSel + "'," +
                                                                     "'" + Coenti02 + "'," +
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
                                                                     "'" + TabConsumos["ZonaResiden"].ToString() + "')";

                                                        SqlInsert = Conexion.SqlInsert(data);

                                                    }
                                                    TabUsuarios.Close();
                                                    TabUsuarios = null;
                                                }
                                                // '3. Hospitalizados

                                                if (TabConsumos["TipoCuenta"].ToString() == "04" || TabConsumos["TipoCuenta"].ToString() == "05")
                                                {
                                                    /*   'El paciente fue facturado como hospitalizado en este sistema
                                                          'A partir de hoy 02 de marzo, se incluye urgencias porque hay veces que no se hospitaliza en el sistema
                                                          'Miramos los d'ias de estancias u horas */


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
                                                        //El paciente si estuvo hospitalizado


                                                        Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal hospitalizacion RIPS]" +
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
                                                              "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                              "'" + TxtCodMinSalud.Text + "'," +
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

                                                            Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal observacion RIPS]" +
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
                                                                "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                                "'" + TxtCodMinSalud.Text + "'," +
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


                                                    DxEntra = (TabConsumos["DxEntra"].ToString() == "0000" ? "" : TabConsumos["DxEntra"].ToString());
                                                    DxSalida = (TabConsumos["DxSalida"].ToString() == "0000" ? "" : TabConsumos["DxSalida"].ToString());
                                                    DxRelac01 = (TabConsumos["DxRelac01"].ToString() == "0000" ? "" : TabConsumos["DxRelac01"].ToString());
                                                    DxRelac02 = (TabConsumos["DxRelac02"].ToString() == "0000" ? "" : TabConsumos["DxRelac02"].ToString());
                                                    DxRelac03 = (TabConsumos["DxRelac03"].ToString() == "0000" ? "" : TabConsumos["DxRelac03"].ToString());
                                                    DxComplica = (TabConsumos["DxComplica"].ToString() == "0000" ? "" : TabConsumos["DxComplica"].ToString());
                                                    DxMuerte = (TabConsumos["DxMuerte"].ToString() == "0000" ? "" : TabConsumos["DxMuerte"].ToString());

                                                    string SqlRecieNacidos = "SELECT [Datos de recien nacidos].* " +
                                                    "FROM [ACDATOXPSQL].[dbo].[Datos de recien nacidos] " +
                                                    "WHERE ((([Datos de recien nacidos].HistorMadre) = '" + HisBus + "') And " +
                                                    "(([Datos de recien nacidos].CuenParto) = '" + CuenBus + "') And " +
                                                    "(([Datos de recien nacidos].ParAnul) = 0)) " +
                                                    "ORDER BY [Datos de recien nacidos].HistorMadre, [Datos de recien nacidos].CuenParto; ";

                                                    SqlDataReader RecienNacidos;

                                                    using (SqlConnection connection4 = new SqlConnection(Conexion.conexionSQL))
                                                    {
                                                        SqlCommand command4 = new SqlCommand(SqlRecieNacidos, connection4);
                                                        command4.Connection.Open();
                                                        RecienNacidos = command4.ExecuteReader();

                                                        if (RecienNacidos.HasRows)
                                                        {
                                                            while (RecienNacidos.Read())
                                                            {
                                                                //   '***************  Las siguientes instrucciones se incluyen a partir del 27 de Noviembre de 2020, por HERNANDO ***********************

                                                                string data = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal recien nacidos RIPS]" +
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
                                                                if (string.IsNullOrWhiteSpace(RecienNacidos["FecMuerNaci"].ToString())) //si la fecha viene null termine el insert aqui
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
                                                                "'" + Coenti02 + "'," +
                                                                "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                                "'" + TxtCodMinSalud.Text + "'," +
                                                                "'" + TabConsumos["TipoIden"].ToString() + "'," +
                                                                "'" + TabConsumos["NumIden"].ToString() + "'," +
                                                                "'" + Convert.ToDateTime(RecienNacidos["FechaNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                                "'" + Convert.ToDateTime(RecienNacidos["HoraNaci"]).ToString("hh:mm:ss") + "'," +
                                                                "'" + RecienNacidos["EdadGesta"].ToString() + "'," +
                                                                "'" + RecienNacidos["ConPrena"].ToString() + "'," +
                                                                "'" + RecienNacidos["SexoNaci"].ToString() + "'," +
                                                                "'" + RecienNacidos["PesoNaci"].ToString() + "'," +
                                                                "'" + RecienNacidos["DxNaci"].ToString() + "'," +
                                                                "'" + RecienNacidos["DxMuerNaci"].ToString() + "'";
                                                                if (string.IsNullOrWhiteSpace(RecienNacidos["FecMuerNaci"].ToString()))
                                                                {
                                                                    data += ")";
                                                                }
                                                                else
                                                                {
                                                                    data += ",'" + Convert.ToDateTime(RecienNacidos["FecMuerNaci"]).ToString("yyyy-MM-dd") + "'," +
                                                                        "'" + Convert.ToDateTime(RecienNacidos["HorMuerNaci"]).ToString("hh:mm:ss") + "')";
                                                                }


                                                                Boolean RegisRecienNadido = Conexion.SqlInsert(data);


                                                            }
                                                            
                                                        } //HasRow

                                                        RecienNacidos.Close();

                                                    } //Final Using                                           

                                                }//Final de (TabConsumos![Sexo] = "F") Or (TabConsumos![Sexo] = "f")



                                                //   'Tomamos el posible numero de poliza, para grabarlo en la factura

                                                 PoliFactu = TabConsumos["NumPoliza"].ToString();
                                                 FecEnPer = Convert.ToDateTime(TabConsumos["FecEntrada"].ToString());

                                                SqlDataReader TabConsumos2;

                                                using (SqlConnection connection4 = new SqlConnection(Conexion.conexionSQL))
                                                {
                                                    SqlCommand command4 = new SqlCommand(SqlConsumos1, connection4);
                                                    command4.Connection.Open();
                                                    TabConsumos2 = command4.ExecuteReader();

                                                

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


                                                        CodTomo = TabConsumos2["CodiISS"].ToString(); //TOdos quedan como CUPS-CUM


                                                        int GBus2 = Convert.ToInt32(GBus);


                                                        switch (GBus2)
                                                        {
                                                            case 1: //Consultas
                                                                    //procede a agregar registro

                                                                    DxEntra = (TabConsumos2["DxEntra"].ToString() == "0000" ? "" : TabConsumos2["DxEntra"].ToString());
                                                                    DxSalida = (TabConsumos2["DxSalida"].ToString() == "0000" ? "" : TabConsumos2["DxSalida"].ToString());
                                                                    DxRelac01 = (TabConsumos2["DxRelac01"].ToString() == "0000" ? "" : TabConsumos2["DxRelac01"].ToString());
                                                                    DxRelac02 = (TabConsumos2["DxRelac02"].ToString() == "0000" ? "" : TabConsumos2["DxRelac02"].ToString());
                                                                    DxRelac03 = (TabConsumos2["DxRelac03"].ToString() == "0000" ? "" : TabConsumos2["DxRelac03"].ToString());
                                                                    DxComplica = (TabConsumos2["DxComplica"].ToString() == "0000" ? "" : TabConsumos2["DxComplica"].ToString());
                                                                    DxMuerte = (TabConsumos2["DxMuerte"].ToString() == "0000" ? "" : TabConsumos2["DxMuerte"].ToString());

                                                                    Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal consultas RIPS]" +
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
                                                                    "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                                    "'" + TxtCodMinSalud.Text + "'," +
                                                                    "'" + TabConsumos2["TipoIden"].ToString() + "'," +
                                                                    "'" + TabConsumos2["NumIden"].ToString() + "',"+
                                                                    // '****************** lo siguiente se coloca por los caprichos de COMFAMILIAR ******************* 01 de Agosto de 2020
                                                                    //if (DefFecTrans.Text == "Consultas")
                                                                    //{
                                                                    //    Utils.SqlDatos += "'" + Convert.ToDateTime(TabFacSele["FechaFac"]).ToString("yyyy-MM-dd") + "',";
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    Utils.SqlDatos += "'" + Convert.ToDateTime(TabConsumos2["FechaCon"]).ToString("yyyy-MM-dd") + "',";
                                                                    //}
                                                                    "'" + Convert.ToDateTime(TabConsumos2["FechaCon"]).ToString("yyyy-MM-dd") + "'," +
                                                                    "'" + TabConsumos2["AutoriNum"] + "'," +
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


                                                                    //  '******************** Lo sigueinte se cambia a partir del 01 Agosto de 2020 HERNANDO *******************
                                                                    Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal procedimientos RIPS] " +
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
                                                                    "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                                    "'" + TxtCodMinSalud.Text + "'," +
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

                                                                }//Fin For 

                                                                break;

                                                            case int G2 when GBus2 >= 6 && GBus2 <= 14: //'OTROS SERVICIOS


                                                                if (GBus2 == 12 || GBus2 == 13)
                                                                {
                                                                    //  'Son medicamentos, por lo tanto se deben tomar los datos complementarios

                                                                    string SqlMedicamentos = "SELECT [Datos productos farmaceuticos].CodigoPro, [Datos forma farmaceutica].CodForFar, " +
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

                                                                    using (SqlConnection connection6 = new SqlConnection(Conexion.conexionSQL))
                                                                    {
                                                                        try
                                                                        {
                                                                            SqlCommand command6 = new SqlCommand(SqlMedicamentos, connection6);

                                                                            command6.Connection.Open();

                                                                            TabMedicamentos = command6.ExecuteReader();

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



                                                                                Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal medicamentos RIPS]" +
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
                                                                                "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                                                "'" + TxtCodMinSalud.Text + "'," +
                                                                                "'" + TabConsumos2["TipoIden"].ToString() + "'," +
                                                                                "'" + TabConsumos2["NumIden"].ToString() + "'," +
                                                                                "'" + TabConsumos2["AutoriNum"].ToString() + "'," +
                                                                                "'" + NomForFar + "'," + //Se debe registrar el nombre de la forma
                                                                                "'" + Descripcion + "'," + // Se busca la unidad de medida
                                                                                "'" + Concentra + "',";

                                                                                //switch (MT)
                                                                                //{
                                                                                //    case "1": //'Manual SOAT
                                                                                //        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                                //        break;
                                                                                //    case "2": //'Manual CUPS-CUM
                                                                                //        CodTomo = TabConsumos2["CodiISS"].ToString();
                                                                                //        break;
                                                                                //    case "3": //'Manual CUPS
                                                                                //        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                                //        break;
                                                                                //    case "4": //'Manual IPS
                                                                                //        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                                //        break;
                                                                                //    case "5": //Manual SOAT-CUM
                                                                                //        CodTomo = TabConsumos2["CodiISS"].ToString();
                                                                                //        break;
                                                                                //    default: //'Utilice el manual IPS
                                                                                //        CodTomo = TabMedicamentos["CodiMinSa"].ToString();
                                                                                //        break;
                                                                                //}


                                                                                CodTomo = TabConsumos2["CodiISS"].ToString(); //'Manual CUPS-CUM

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

                                                                    Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal otros servicios RIPS]" +
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
                                                                     "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                                     "'" + TxtCodMinSalud.Text + "'," +
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

                                                    }//While TanConsumos 2

                                               }//USing Tancosumo 2

                                                // 2. copiamos las facturas, despues de registrar, datos de los archivos de movimientos,
                                                //para poder registrar el detalle individual de cada factura

                                                string TipoDocuIPS = txtTipoDocuIps.Text;

                                                if (TipoDocuIPS.Length > 2)
                                                {
                                                    TipoDocuIPS = TipoDocuIPS.Substring(0, 2);
                                                }



                                                string NomAdmin = TabFacturas["NomAdmin"].ToString();

                                                if (string.IsNullOrWhiteSpace(NomAdmin) == false && NomAdmin.Length > 30)
                                                {
                                                    NomAdmin = NomAdmin.Substring(0, 30);

                                                }

                                                string NumContra = TabFacturas["NumContra"].ToString();

                                                if (string.IsNullOrWhiteSpace(NumContra) == false && NumContra.Length > 15)
                                                {
                                                    NumContra = NumContra.Substring(0, 15);
                                                }

                                                string NomPlan = TabFacturas["NomPlan"].ToString();

                                                if (string.IsNullOrWhiteSpace(NomPlan) == false && NomPlan.Length > 30)
                                                {
                                                    NomPlan = NomPlan.Substring(0, 30);
                                                }



                                                Utils.SqlDatos = "INSERT INTO [DARIPSESSQL].[dbo].[Datos temporal transacciones RIPS]" +
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
                                                    "'" + TxtCodMinSalud.Text + "'," +
                                                    "'" + txtNombreIps.Text + "'," +
                                                    "'" + TipoDocuIPS + "'," +
                                                    "'" + txtDocuIps.Text + "'," +
                                                    "'" + TabFacturas["NumFactura"].ToString() + "'," +
                                                    "'" + Convert.ToDateTime(TabFacturas["FechaFac"]).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(DateInicial.Value).ToString("yyyy-MM-dd") + "'," +
                                                    "'" + Convert.ToDateTime(DateFinal.Value).ToString("yyyy-MM-dd") + "'," +                                           
                                                    "'" + ConMinRips + "'," +
                                                    "'" + NomAdmin + "',"+
                                                    "'" + NumContra + "',"+                                                 
                                                    "'" + NomPlan + "'," +
                                                    "'" + PoliFactu + "'," +
                                                    "'" + TabFacturas["Copago"].ToString() + "'," +
                                                    "'" + TabFacturas["ValorFac"].ToString() + "'," +
                                                    "'" + ValdetaFac + "'," +
                                                    "'" + CexterAten + "'" +
                                                    ")";


                                                Boolean Insertacturas = Conexion.SqlInsert(Utils.SqlDatos);


                                            } //TabConsumos

                                            TabConsumos.Close();

                                        } //USing

                                    }// if(Convert.ToBoolean(TabFacturas["AnuladaFac"]) == true)

                                }// if(TabFacturas.HasRows == false)

                                TabFacturas.Close();

                            } //Using

                            BarraSeleccionar.Increment(1);

                        } //Estado Grilla 1

                        TolD = TolD + 1;

                    }///Foreach Grillas

                    Utils.Informa = "El proceso de selección de datos para los archivos" + "\r";
                    Utils.Informa += "RIPS, ha concluido satisfactoriamente." + "\r";

                    BarraSeleccionar.Minimum = 0;
                    BarraSeleccionar.Maximum = 1;
                    BarraSeleccionar.Value = 0;

                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }// Dialogo Resul

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el evento click boton seleccionar" + "\r";
                BarraSeleccionar.Minimum = 0;
                BarraSeleccionar.Maximum = 1;
                BarraSeleccionar.Value = 0;
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region DataGrid


        private void DataGridFacturas_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (DataGridFacturas.SelectedCells.Count != 0)
                {
                    string CodArt = DataGridFacturas.SelectedCells[1].Value.ToString();
                    txtBusquedaFactura.Text = CodArt;
                }
                else
                {
                    txtBusquedaFactura.Text = null;
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
        private void DataGridFacturas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CalcularTotalFactura();
        }
        #endregion

        string TipoSede = null;
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

    }
}
