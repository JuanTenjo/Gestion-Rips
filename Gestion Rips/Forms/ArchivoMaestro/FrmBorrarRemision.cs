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

namespace Gestion_Rips.Forms.ArchivoMaestro
{
    public partial class FrmBorrarRemision : Form
    {
        public FrmBorrarRemision()
        {
            InitializeComponent();
        }
        Int32 Seleccion = 0;
        private void FrmBorrarRemision_Load(object sender, EventArgs e)
        {
            try
            {
                CargaUsuario();
                CargaComboBox();
                CargarDatosRemi(Utils.NumRemi);
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al abrir formulario FrmBorrarRemision" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargaComboBox()
        {
            try
            {
                this.CboArchiBorrar.DataSource = null;
                this.CboArchiBorrar.Items.Clear();

                Utils.SqlDatos = "SELECT CodArchivo, NomArchivo, NomTabRemo FROM [Datos nombres de archivos] ";

                DataSet dataSet = Conexion.SQLDataSet(Utils.SqlDatos);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    this.CboArchiBorrar.DataSource = dataSet.Tables[0];
                    this.CboArchiBorrar.ValueMember = "NomTabRemo";
                    this.CboArchiBorrar.DisplayMember = "NomArchivo";
                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al cargar combobox" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargaUsuario()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Utils.codUsuario))
                {
                    LblCodigoUsaF.Text = "000";
                    LblNombreUsa.Text = "SOFTWARE PIRATA";
                    LblNivelPermi.Text = "0";

                }
                else
                {
                    LblCodigoUsaF.Text = Utils.codUsuario;
                    LblNombreUsa.Text = Utils.nomUsuario;
                    LblNivelPermi.Text = Utils.nivelPermiso;
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

        private void RdArSelect_CheckedChanged(object sender, EventArgs e)
        {
            CboArchiBorrar.Enabled = true;
            Seleccion = 1;
        }

        private void RdTodoSelect_CheckedChanged(object sender, EventArgs e)
        {
            CboArchiBorrar.Enabled = false;
            Seleccion = 2;
        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                string R, Estandatos, TB = null, T, NArch = null, USC, Rz, FRS;
                Double NR, TolEl;
                Int32 TRZ, FunFec;

                FRS = DateTime.Now.ToString("yyyy-MM-dd");

                Utils.tipoDocEmp = "Control para borrar datos de remisión";
                //Revisamos si se digito un numero valido de Remision
                if (string.IsNullOrWhiteSpace(TxtRemiNum.Text))
                {
                    Utils.Informa = "Lo siento pero usted no ha digitado" + "\r";
                    Utils.Informa += "el número de la remisión a la cual" + "\r";
                    Utils.Informa += "le piensa borrar los datos." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Tomamos el numero de Remision
                R = TxtRemiNum.Text;

                //'Revisamos si el usuario tiene privilegios, debe ser un administrador

                if(Convert.ToInt32(LblNivelPermi.Text) != 1)
                {
                    Utils.Informa = "Lo siento pero usted no tiene categoría" + "\r";
                    Utils.Informa += "de administrador del sistema, para poder" + "\r";
                    Utils.Informa += "ejecutar este proceso." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //'Buscamos el número de remisión

                Utils.SqlDatos = "SELECT * FROM [Datos archivo maestro] WHERE ConseArchivo = '" + R + "' ";

                SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if(TablaAux1.HasRows == false)
                {
                    Utils.Informa = "Lo siento pero el número de remisión" + "\r";
                    Utils.Informa += "digitado no existe en este sistema." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    TablaAux1.Read();
                    //Revisamos si la remision se encuentra abierta
                    if(Convert.ToBoolean(TablaAux1["CerraRemi"].ToString()) == true)
                    {
                        Utils.Informa = "Lo siento pero el número de remisión se encuentra" + "\r";
                        Utils.Informa += "cerrada, por tanto no se puede eleminar los datos" + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        TablaAux1.Close();
                    }


                }


                USC = LblCodigoUsaF.Text;

                switch (Seleccion)
                {
                    case 1: //Elomina por archivos
                            //Revisamos si se selecciono el archivo a eliminar
                        if(CboArchiBorrar.SelectedIndex == -1)
                        {
                            Utils.Informa = "Lo siento pero usted aún no ha" + "\r";
                            Utils.Informa += "seleccionado el nombre del archivo" + "\r";
                            Utils.Informa += "a borrar los datos de la remisión " + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Utils.Informa = "¿Usted desea BORRAR los datos contenidos en el" + "\r";
                        Utils.Informa += CboArchiBorrar.Text + "\r";
                        Utils.Informa += "de la remisión número " + R + "\r";
                        var res =  MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if(res == DialogResult.Yes)
                        {


                            Exportar.FrmAnularRemi frmAnularRemi = new Exportar.FrmAnularRemi();

                            frmAnularRemi.ShowDialog();

                            Rz = Utils.RazonAnul;

                            TB = CboArchiBorrar.SelectedValue.ToString();

                            Utils.SqlDatos = "SELECT COUNT(NumRemi) as TotalRemi FROM [" + TB + "] WHERE [NumRemi]  = '" + R + "'";

                            TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);


                            if(TablaAux1.HasRows == false)
                            { 
                                //NUNCA DEBERIA ENTRAR PORQUE SIEMPRE TRAE DATO
                                Utils.Informa = "Lo siento pero no existen registros para borrar del " + "\r";
                                Utils.Informa += CboArchiBorrar.Text + "\r";
                                Utils.Informa += "de la remisión número " + R + "\r";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                NR = 0;
                                TablaAux1.Read();

                                Int32  Contador = Convert.ToInt32(TablaAux1["TotalRemi"]);

                                if(Contador == 0)
                                {
                                    Utils.Informa = "Lo siento pero no existen registros para borrar del " + "\r";
                                    Utils.Informa += CboArchiBorrar.Text + "\r";
                                    Utils.Informa += "de la remisión número " + R + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                Utils.SqlDatos = "DELETE FROM [" + TB + "] WHERE [NumRemi]  = '" + R + "'";

                                Boolean EstaEli = Conexion.SQLDelete(Utils.SqlDatos);

                                if (EstaEli)
                                {
                                    // 'Registre la acción realizada, 5 = Eliminada parcialmente
                                

                                    FunFec = RegisAccion(R, "5", Rz, FRS, USC);

                                    Utils.Informa = "Se han BORRADO  de la Remisión No. " + R + "\r";
                                    Utils.Informa += "un total de " + Contador + "\r";
                                    Utils.Informa += "del " + CboArchiBorrar.Text + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }

                            }

                            TablaAux1.Close();

                        }

                        break;
                    case 2: //Eliminar todos los datos relacionados con la remisión
                            //Pregunte
                        Utils.Informa = "¿Usted desea BORRAR todos los datos" + "\r";
                        Utils.Informa += "contenidos en el la remisión No. " + R + "\r";
                        var Res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if(Res == DialogResult.Yes)
                        {
                            Exportar.FrmAnularRemi frmAnularRemi = new Exportar.FrmAnularRemi();

                            frmAnularRemi.ShowDialog();

                            Rz = Utils.RazonAnul;

                            if (string.IsNullOrWhiteSpace(Rz))
                            {
                                return;
                            }

                            //Hacemos el bucle por la tabla de nombres de archivos para eliminar los datos

                            Utils.SqlDatos = "SELECT * FROM [Datos nombres de archivos]";

                            SqlDataReader NombreDeArchivos = Conexion.SQLDataReader(Utils.SqlDatos);

                            if (NombreDeArchivos.HasRows == false)
                            {
                                Utils.Informa = "Lo siento, pero por un error fatal del" + "\r";
                                Utils.Informa += "sistema, los nombres de los archivos a" + "\r";
                                Utils.Informa += "borrar no se han encontrado" + "\r";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            Utils.Informa = "********* REGISTROS BORRADOS DE CADA ARCHIVO *********" + "\r";

                            while (NombreDeArchivos.Read()) //Contamos cuantos registros se eliminaras de las tablas para poder dar una clara informacion
                            {
                                TB = NombreDeArchivos["NomTabRemo"].ToString();
                                NArch = NombreDeArchivos["NomArchivo"].ToString();

                                Utils.SqlDatos = "SELECT Count(NumRemi) as TolRemi FROM [" + TB + "] WHERE [NumRemi]  = '" + R + "'";

                                SqlDataReader TolTablas = Conexion.SQLDataReader(Utils.SqlDatos);

                                if(TolTablas.HasRows == false)
                                {
                                    //Raro que entre
                                    NR = 0;
                                }
                                else
                                {
                                    TolTablas.Read();
                                    NR = Convert.ToDouble(TolTablas["TolRemi"].ToString());
                                    TolTablas.Close();
                                }

                                Utils.Informa += "Informa " + NArch + ". . ." + NR + "\r";

                            }

                            //Eliminamos Los usuarios junto con los demas archivos que tienen TD y NUMDOCU en cascada
                            Boolean EstadoEli;

                            Utils.SqlDatos = "DELETE FROM [Datos archivo usuarios] WHERE NumRemi = '" + R + "'";

                            EstadoEli = Conexion.SQLDelete(Utils.SqlDatos);

                            //Eliminamos control

                            Utils.SqlDatos = "DELETE FROM [Datos archivo de control] WHERE NumRemi = '" + R + "'";

                            EstadoEli = Conexion.SQLDelete(Utils.SqlDatos);

                            //  'Eliminamos Transacciones

                            Utils.SqlDatos = "DELETE FROM [Datos archivo de transacciones] WHERE NumRemi = '" + R + "'";

                            EstadoEli = Conexion.SQLDelete(Utils.SqlDatos);

                            //Eliminamos Agrupados

                            Utils.SqlDatos = "DELETE FROM [Datos archivo de servicios agrupados] WHERE NumRemi = '" + R + "'";

                            EstadoEli = Conexion.SQLDelete(Utils.SqlDatos);

                            Utils.Informa += "Datos eliminados satisfactoriamente ";

                            //Actualiza el campo de Numfacturas a 0 -- Juan Diego Pimentel 2021

                            Utils.SqlDatos = "UPDATE [Datos archivo maestro] SET NumFacturas = 0 WHERE ConseArchivo = '" + R + "' ";

                            Boolean EstaAct = Conexion.SQLUpdate(Utils.SqlDatos);

                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //Registre la acción realizada, 4 = Eliminada totalmente

                            FunFec = RegisAccion(R, "4", Rz, FRS, USC);

                        } //fINAL tABLAAux1.Hasrow

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de hacer click en borrar" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int RegisAccion(string Rm, string A, string Rz, string F, string Us)
        {
            try
            {
                Utils.SqlDatos = "SELECT * FROM [Datos control de remisiones] WHERE CodiMas = '" + Rm + "' and AcReal = '" + A + "'";

                SqlDataReader TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if(TablaAux2.HasRows == false)
                {
                    //Es la primer vez que se ejecuta la accion sobre la remision
                    Utils.SqlDatos = "INSERT INTO  [Datos control de remisiones] (" +
                                     "CodiMas," +
                                     "AcReal," +
                                     "RazReal," +
                                     "VezAccion," +
                                     "FecRegis," +
                                     "CodiRegis)" +
                                     "Values (" +
                                     "'" + Rm + "'," +
                                     "'" + A + "'," +
                                     "'" + Rz + "'," +
                                     "'" + 1 + "'," +
                                     "'" + F + "'," +
                                     "'" + Us + "')";

                    Boolean estaRegis = Conexion.SqlInsert(Utils.SqlDatos);

                    if (estaRegis)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    //La acci'on se ha ejecutado mas de una vez
                    TablaAux2.Read();
                    int V = Convert.ToInt32(TablaAux2["VezAccion"]) + 1;

                    Utils.SqlDatos = "UPDATE [Datos control de remisiones] SET " +
                        "RazReal = '" + Rz + "'," +
                        "VezAccion = '" + V + "'," +
                        "FecRegis = '" + F + "'," +
                        "CodiRegis = '" + Us + "'" +
                        "WHERE CodiMas = '" + Rm + "' and AcReal = '" + A + "'";

                    Boolean EstaAt = Conexion.SQLUpdate(Utils.SqlDatos);

                    if (EstaAt)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }

                TablaAux2.Close();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: RegisAccion" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private void TxtRemiNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (int)Keys.Enter)
                {
                    if (string.IsNullOrWhiteSpace(TxtRemiNum.Text) == false)
                    {
                        CargarDatosRemi(TxtRemiNum.Text);
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

        private void CargarDatosRemi(string NumRemi)
        {
            try
            {
                TxtCodMin.Clear();
                TxtNomEnti.Clear();

    

                Utils.SqlDatos = "SELECT * FROM [Datos archivo maestro] WHERE ConseArchivo = '" + NumRemi + "'";


                SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux1.HasRows)
                {
                    //Muestre algunos datos de la remision

                    TablaAux1.Read();


                    string CA = TablaAux1["CodInterAdmi"].ToString();
                    this.TxtRemiNum.Text = NumRemi;
                    this.DtFecRemi.Value = Convert.ToDateTime(TablaAux1["FecRemite"].ToString());
                    this.TxtCodMin.Text = TablaAux1["CodAdmin"].ToString();
                    this.DtPer01.Value = Convert.ToDateTime(TablaAux1["Periodo1"].ToString());
                    this.DtPer02.Value = Convert.ToDateTime(TablaAux1["Periodo2"].ToString());

                    string Data = "SELECT * FROM [Datos administradoras de planes] WHERE CodInterno = '" + CA + "'";

                    SqlDataReader TablaAux4 = Conexion.SQLDataReader(Data);

                    if (TablaAux4.HasRows)
                    {
                        TablaAux4.Read();
                        this.TxtNomEnti.Text = TablaAux4["NomAdmin"].ToString();
                        TablaAux4.Close();
                    }


                }
                else
                {
                    Utils.Titulo01 = "Control para mostrar datos";
                    Utils.Informa = "Lo siento pero el número de remisión" + "\r";
                    Utils.Informa += "digitado no existe en este sistema." + "\r";
                    Utils.Informa += "Por favor corrija el número o pulse" + "\r";
                    Utils.Informa += "La tecla [ESC] para continuar." + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                TablaAux1.Close();
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CargarDatosRemi " + "\r";
                Utils.Informa += "Mensaje del error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }

}
