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

namespace Gestion_Rips.Forms.ArchivoMaestro
{
    public partial class FrmCrearModificarMaestro : Form
    {
        public FrmCrearModificarMaestro()
        {
            InitializeComponent();
        }

        private void FrmCrearModificarMaestro_Load(object sender, EventArgs e)
        {
            try
            {

                CargaUsuario();

                if (string.IsNullOrWhiteSpace(Utils.NumRemi) == false)
                {

                    txtRemiG.ReadOnly = true;

                    txtRemiG.Text = Utils.NumRemi;
                    DtFecEnvio.Value = Convert.ToDateTime(Utils.FecEnvio);
                    txtResponEnvia.Text = Utils.ResponEnvia;
                    txtTeleRespon.Text = Utils.TeleRespon;
                    txtCantifact.Text = Utils.Cantifact;
                    DpFecInicial.Value = Convert.ToDateTime(Utils.FecInicial);
                    DpFecFinal.Value = Convert.ToDateTime(Utils.FecFinal);


                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el formulario FrmCrearModificarMaestro " + "\r";
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
                    lblCodigoUsaF.Text = "000";
                    lbNombreUsa.Text = "SOFTWARE PIRATA";
                    lblNivelPermitido.Text = "0";

                }
                else
                {
                    lblCodigoUsaF.Text = Utils.codUsuario;
                    lbNombreUsa.Text = Utils.nomUsuario;
                    lblNivelPermitido.Text = Utils.nivelPermiso;

                }
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion CargaUsuario" + "\r";
                Utils.Informa += "Módulo FrmCrearMofificarMaestro" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                string NAP, RENum, EnRutardatos, FunCon , UsGra;
                Int32 FunNivel, FunDer;
                Boolean CarFor;

                Utils.Titulo01 = "Control para crear remisiones";

                if (string.IsNullOrWhiteSpace(txtResponEnvia.Text))
                {
                    Utils.Informa = "Lo siento pero grabar los datos debe" + "\r";
                    Utils.Informa += "existir el nombre del responsable";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                NAP = Utils.NomAdmin;

                RENum = txtRemiG.Text;
                UsGra = lblCodigoUsaF.Text;

                //'proceda a verificar si el usuario está creando o modificando

                Utils.SqlDatos = "SELECT * FROM [Datos archivo maestro] WHERE ConseArchivo = '" + RENum + "' ";

                SqlDataReader TablaAux7 = Conexion.SQLDataReader(Utils.SqlDatos);

                if(TablaAux7.HasRows == false)
                {
                    //No existem, por tanto se va ha agregar
                    //Veridicamos si tiene permiso

                 FunNivel = Convert.ToInt32(lblNivelPermitido.Text);

                    switch (FunNivel)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        default:
                            Utils.Informa = "Lo siento pero usted no es un usuario" + "\r";
                            Utils.Informa += "autorizado para agregar datos.";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                    }

                    Utils.Informa = "¿Usted desea agregar un nuevo número";
                    Utils.Informa += "de remisión de envío a la entidad@";
                    Utils.Informa += NAP + "?";
                    var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if(res == DialogResult.Yes)
                    {
                        //Procesa a buscar el consecutivo
                        FunCon = ConseRemisiones(true, UsGra);

                        switch (FunCon)
                        {
                            case "-3": //limite
                                Utils.Informa = "Error de administración de datos." + "\r";
                                Utils.Informa += "El registro único de contadores";
                                Utils.Informa += "no fué posible encontrarlo.";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                                break;
                            case "-2":
                                Utils.Informa = "Lo siento pero la fecha del sistema es" + "\r";
                                Utils.Informa += "menor a la de la última remisión generada";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                                break;
                            case "-1":
                                //Error en la funcion
                                break;
                            case "0":
                                Utils.Informa = "Error de administración de datos." + "\r";
                                Utils.Informa += "El registro único de contadores";
                                Utils.Informa += "no fué posible encontrarlo.";
                                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                                break;

                            default:
                                string Date = DateTime.Now.ToString("yyyy-MM-dd");
                                //'Proceda a registrar la nueva remisi'on
                                Utils.SqlDatos = "INSERT INTO [Datos archivo maestro]" +
                                       "(" +
                                       "ConseArchivo," +
                                       "CodInterAdmi," +
                                       "CodIps," +
                                       "CodAdmin," +
                                       "FecRemite," +
                                       "NomRespon," +
                                       "TelResponsa," +
                                       "Periodo1," +
                                       "Periodo2," +
                                       "NumFacturas," +
                                       "CodiRegis," +
                                       "FecRegis" +
                                       ")" +
                                       "VALUES" +
                                       "(" +
                                       "'" + FunCon + "'," +
                                       "'" + Utils.NomAdmin + "'," +
                                       "'" + Utils.CodiIPS + "'," +
                                       "'" + Utils.CodigAdmin + "'," +
                                       "CONVERT(DATETIME,'" + Convert.ToDateTime(DtFecEnvio.Value).ToString("yyyy-MM-dd") + "',102)," +
                                       "'" + txtResponEnvia.Text + "'," +
                                       "'" + txtTeleRespon.Text + "'," +
                                       "CONVERT(DATETIME,'" + Convert.ToDateTime(DpFecInicial.Value).ToString("yyyy-MM-dd") + "',102)," +        
                                       "CONVERT(DATETIME,'" + Convert.ToDateTime(DpFecFinal.Value).ToString("yyyy-MM-dd") + "',102)," +
                                       "'" + txtCantifact.Text + "'," +
                                       "'" + UsGra + "'," +
                                       "CONVERT(DATETIME,'" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "',102)" +
                                       ")";

                                    Boolean RegistraRemision = Conexion.SqlInsert(Utils.SqlDatos);

                                 break;
                        } //Final Swich


                    }//Final Pregunta
                    else
                    {
                        //No quiere
                    }


                }
                else
                {
                    //Revisamos si tiene permisos para modificar

                    switch (Convert.ToInt32(lblNivelPermitido.Text))
                    {
                        case 1:
                            break;
                        case 2:
                            break;

                        default:
                            Utils.Informa = "Lo siento pero usted no es un usuario" + "\r";
                            Utils.Informa += "autorizado para modificar datos.";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            break;
                    }

                    Utils.Informa = "¿Usted desea modificar los datos de";
                    Utils.Informa += "remisión de envío de la entidad";
                    Utils.Informa += NAP + "?";
                    var Resp = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (Resp == DialogResult.Yes)
                    {
                        //Procede a mofificar

                        Utils.SqlDatos = "UPDATE [Datos archivo maestro] SET " +
                        "FecRemite = '" + DtFecEnvio.Value.ToString("yyyy-MM-dd") + "'," +
                        "NomRespon = '" + txtResponEnvia.Text + "'," +
                        "TelResponsa = '" + txtTeleRespon.Text + "'," +
                        "Periodo1 = '" + DpFecInicial.Value.ToString("yyyy-MM-dd") + "'," +
                        "Periodo2 = '" + DpFecFinal.Value.ToString("yyyy-MM-dd") + "'," +
                        "NumFacturas = '" + UsGra + "' " +
                        "WHERE ConseArchivo = '" + RENum + "' ";

                        Boolean EstadoActualizacion = Conexion.SQLUpdate(Utils.SqlDatos);


                    }

                }

                TablaAux7.Close();

                this.Close();
           }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de dar click en el boton Grabar" + "\r";
                Utils.Informa += "Módulo FrmCrearMofificarMaestro" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConseRemisiones(Boolean A, string Us)
        {
            try
            {

                DateTime FechaUltima;
                double Fac = 0;
                string Date2, Convertido;
                Utils.SqlDatos = "SELECT * FROM [Datos contadores sedas] WHERE CodiUNico = 1";

                SqlDataReader TablaAux1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if(TablaAux1.HasRows == false)
                {
                    //NO existe registro unico
                    return "0";
                }
                else
                {
                    TablaAux1.Read();
                    FechaUltima = Convert.ToDateTime(TablaAux1["FecRemi"].ToString());

                    //Revisamos que esta no sea mayor a la del sistema

                    DateTime Date = DateTime.Now; 

                    if(FechaUltima > Date)
                    {
                        return "-2";
                    }
                    else
                    {

                        if (Convert.ToInt32(TablaAux1["UlConRemi"].ToString()) == 0)
                        {
                            //no existe remisiones perdidas
                            Fac = Convert.ToInt32(TablaAux1["ConsRemi"].ToString());
                            Fac += 1;

                            if (A)
                            {
                                //Procesa a actualizar el campo de concecutivos

                                Date2 = DateTime.Now.ToString("yyyy-MM-dd");

                                Utils.SqlDatos = "UPDATE [Datos contadores sedas] SET [ConsRemi] = '" + Fac + "', [UsarRemi] = '" + Us + "', FecRemi =  CONVERT(DATETIME,'" + Date2 + "',102)";

                                Boolean EstaActConce = Conexion.SQLUpdate(Utils.SqlDatos);

                                if (EstaActConce == false)
                                {
                                    Utils.Informa = "Error de administración de datos. ";
                                    Utils.Informa += "al actualizar el concecutivo" + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return "0";
                                }
                            }
                        }
                        else
                        {
                            Fac = Convert.ToDouble(TablaAux1["UlConRemi"].ToString());
                            if (A)
                            {
                                Utils.SqlDatos = "UPDATE [Datos contadores sedas] SET [UlConRemi] = '" + 0 + "'";

                                Boolean EstaActConce = Conexion.SQLUpdate(Utils.SqlDatos);

                                if (EstaActConce == false)
                                {
                                    Utils.Informa = "Error de administración de datos. ";
                                    Utils.Informa += "al actualizar el campo UlConRemi " + "\r";
                                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
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

                        return Convertido;


                    } //FIn fecha ultuima > Date

                }// Final TablaAux

                TablaAux1.Close();

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion ConseRemisiones" + "\r";
                Utils.Informa += "Módulo FrmCrearMofificarMaestro" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "-1";
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
