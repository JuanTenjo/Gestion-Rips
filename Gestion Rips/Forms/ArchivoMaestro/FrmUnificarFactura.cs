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
    public partial class FrmUnificarFactura : Form
    {
        public FrmUnificarFactura()
        {
            InitializeComponent();
        }

        private void FrmUnificarFactura_Load(object sender, EventArgs e)
        {
            try
            {
                CargaUsuario();
                txtRemi.Text = Utils.NumRemi;
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "Al abrir formulario UnificarFactura" + "\r";
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
                    LblNivelPermitido.Text = "0";

                }
                else
                {
                    LblCodigoUsaF.Text = Utils.codUsuario;
                    LblNombreUsa.Text = Utils.nomUsuario;
                    LblNivelPermitido.Text = Utils.nivelPermiso;
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

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string RB, FB;
                Int32 F;

                Utils.Titulo01 = "Control para ejecutar procesos";

                if (string.IsNullOrWhiteSpace(this.txtRemi.Text))
                {

                    Utils.Informa = "Debe digitar el número de la remisión" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (string.IsNullOrWhiteSpace(this.txtFacUnica.Text))
                {

                    Utils.Informa = "Debe digitar el número único de la factura" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RB = txtRemi.Text;
                FB = txtFacUnica.Text;

                Utils.Informa = "¿Usted desea convertir todas las facturas de la remisión " + RB + "\r";
                Utils.Informa += "en una unica factura de número " + FB + "\r";
                var res =  MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);


                if(res == DialogResult.Yes)
                {
                    NumFactUnico(RB, FB);
                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "despues de hacer click en el boton actualizar" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NumFactUnico(string R, string F)
        {
            try
            {
                double TC = 0, TM = 0, TH = 0, TU = 0, TOS = 0, TP = 0, TN = 0, TA = 0, TF = 0, TCN = 0, CP = 0, Comi = 0, DesC = 0, VN = 0;
                string TRC, TRM, TRH, TRU, TRO, TRP, TRN, TRA, TRF, TRCN, Para01, Para02, SqlDatos, Estandantos;
                Boolean EstadoAct;
                SqlDataReader TablaAux2;

                Utils.SqlDatos = "Convertir en única factura";

                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de consulta] WHERE NumRemi = '" + R + "' GROUP BY NumRemi";

                //Consulta

                 TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TC = 0;
                }
                else
                {
                    TablaAux2.Read();

                    TC = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if (TC != 0 && TC > 0)
                    {
                        TRC = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de consulta] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }

                }

                TablaAux2.Close();

                //Hospitalizacion

                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de hospitalizacion] WHERE NumRemi = '" + R + "'  GROUP BY NumRemi";


                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TH = 0;
                }
                else
                {
                    TablaAux2.Read();

                    TH = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if (TH != 0 && TH > 0)
                    {
                        TRH = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de hospitalizacion] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }

                }

                TablaAux2.Close();

                //Medicamentos


                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de medicamentos] WHERE NumRemi = '" + R + "' GROUP BY NumRemi";


                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TM = 0;
                }
                else
                {
                    TablaAux2.Read();

                    TM = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if (TM != 0 && TM > 0)
                    {
                        TRM = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de medicamentos] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }

                }

                TablaAux2.Close();

                //Datos archivo de observacion urgencias

                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de observacion urgencias] WHERE NumRemi = '" + R + "'  GROUP BY NumRemi";


                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TU = 0;
                }
                else
                {
                    TablaAux2.Read();

                    TU = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if (TU != 0 && TU > 0)
                    {
                        TRU = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de observacion urgencias] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }



                }

                TablaAux2.Close();

                //Datos archivo de otros servicios


                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de otros servicios] WHERE NumRemi = '" + R + "'  GROUP BY NumRemi";


                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TOS = 0;
                }
                else
                {
                    TablaAux2.Read();

                    TOS = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if (TOS != 0 && TOS > 0)
                    {
                        TRO = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de otros servicios] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }


                }

                TablaAux2.Close();

                //Datos archivo de procedimientos


                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de procedimientos] WHERE NumRemi = '" + R + "'  GROUP BY NumRemi";


                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TP = 0;
                }
                else
                {

                    TablaAux2.Read();

                    TP = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if(TP != 0 && TP > 0)
                    {
                        TRP = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de procedimientos] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }
          


                }

                TablaAux2.Close();

                //archivo de recien nacido

                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi, NumRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de recien nacido] WHERE NumRemi = '" + R + "'  GROUP BY NumRemi";


                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TN = 0;
                }
                else
                {

                    TablaAux2.Read();

                    TN = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());

                    if (TN != 0 && TN > 0)
                    {
                        TRN = TablaAux2["NumRemi"].ToString();

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[archivo de recien nacido] SET NumFactur = '" + F + "' WHERE NumRemi = '" + R + "'";

                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                    }



                }

                TablaAux2.Close();


                //Datos archivo de transacciones

                Utils.SqlDatos = "SELECT Count(NumRemi) as TotalRemi,NumRemi, SUM(Copago) as TolCopago, SUM(ValorComi) as TolValComi, Sum(ValorDes) as TolValDes, Sum(ValorNeto) as TolValNeto" +
                    " FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] " +
                    " WHERE NumRemi = '" + R + "' GROUP BY NumRemi ";

                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TF = 0;
                }
                else
                {

                    TablaAux2.Read();

                    TF = Convert.ToDouble(TablaAux2["TotalRemi"].ToString());



                    if (TF != 0 && TF > 0)
                    {
                        TRF = TablaAux2["NumRemi"].ToString();

                        CP = Convert.ToDouble(TablaAux2["TolCopago"].ToString());
                        Comi = Convert.ToDouble(TablaAux2["TolValComi"].ToString());
                        DesC = Convert.ToDouble(TablaAux2["TolValDes"].ToString());

                        VN = Convert.ToDouble(TablaAux2["TolValNeto"].ToString());

                    }

                    TablaAux2.Close();

                    TF = 0;

                    //Modificamos el numero de factura del primer registro de la consulta a la remision

                    Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] WHERE NumRemi = '" + R  + "' order by NumRemi, NumFactur ";

                    TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                    if (TablaAux2.HasRows)
                    {
                        TablaAux2.Read();
                        string NumFac = TablaAux2["NumFactur"].ToString();
                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] " +
                                        "SET NumFactur = '" + F + "' " +
                                        "WHERE NumRemi = '" + R + "' " +
                                        "and NumFactur = '" + NumFac + "'"; //Se modificara la factura del primer registro de la consulta
                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                        if (EstadoAct)
                        {
                            Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] " +
                                        "WHERE NumRemi = '" + R + "' " +
                                        "and NumFactur <> '" + F + "'"; //Se eliminaran los registros de esa remision y que sean diferentes a al numero de factura ingresado
                            EstadoAct = Conexion.SQLDelete(Utils.SqlDatos);
                        }

                        TablaAux2.Close();
                    }
                }

                //Buscanos nuevamente el número de factura unico para agregarle los totales


                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] WHERE NumRemi = '" + R + "' and NumFactur = '" + F + "'";

                SqlDataReader Validacion = Conexion.SQLDataReader(Utils.SqlDatos);

                if(Validacion.HasRows)
                {
                    Utils.SqlDatos =" UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de transacciones] SET " +
                                    " Copago = "+ CP +", ValorComi = "+ Comi +", ValorDes = "+ DesC +", ValorNeto = "+ VN +" " +
                                    " WHERE NumRemi = '" + R + "' and NumFactur = '" + F + "'";

                    EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                }
                else
                {
                    Utils.Titulo01 = "Control de errores de ejecución";
                    Utils.Informa = "Error fatal no se encontró la factura unica" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                Validacion.Close();


                Utils.SqlDatos = "INSERT INTO [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] (NumRemi, NumFactur, CodIPS, CodConcepto, CantiGrupo, ValUnita, Valtotal) " +
                " SELECT [Datos archivo de servicios agrupados].NumRemi, '" + F + "' AS NFV, [Datos archivo de servicios agrupados].CodIPS, " +
                " [Datos archivo de servicios agrupados].CodConcepto, Sum([Datos archivo de servicios agrupados].CantiGrupo) " +
                " AS SumaDeCantiGrupo, (Sum([Valtotal])/ Sum([CantiGrupo])) AS VU, Sum([Datos archivo de servicios agrupados].Valtotal) AS SumaDeValtotal " +
                " FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                " GROUP BY [Datos archivo de servicios agrupados].NumRemi, [Datos archivo de servicios agrupados].CodIPS, " +
                " [Datos archivo de servicios agrupados].CodConcepto" +
                " HAVING ((([Datos archivo de servicios agrupados].NumRemi) = '" + R + "'  )); ";

                EstadoAct = Conexion.SqlInsert(Utils.SqlDatos);

                //'Proceda a eliminar las que sean distintas a la nueva factura

                Utils.SqlDatos = "DELETE FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] " +
                                 "WHERE ((([Datos archivo de servicios agrupados].NumRemi)= '" + R + "' ) AND " +
                                 "(([Datos archivo de servicios agrupados].NumFactur) <> '" + F + "' ));";

                EstadoAct = Conexion.SQLDelete(Utils.SqlDatos);

                //Contamos cuantos registros de agrupados quedan

                Utils.SqlDatos = "SELECT Count(NumRemi) as TolRemi FROM [DARIPSXPSQL].[dbo].[Datos archivo de servicios agrupados] WHERE NumRemi = '" + R + "'";

                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TablaAux2.HasRows == false)
                {
                    TA = 0;
                }
                else
                {
                    TablaAux2.Read();
    
                    TA = Convert.ToDouble(TablaAux2["TolRemi"].ToString());
                }

                TablaAux2.Close();

                //'Actualizamos el archivo de control


                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos archivo de control] WHERE NumRemi = '" + R + "'";

                TablaAux2 = Conexion.SQLDataReader(Utils.SqlDatos); //Se hace con un ciclo

                if (TablaAux2.HasRows == false)
                {
                    TCN = 0;
                }
                else
                {
                    TablaAux2.Read();
                    
                        TRCN = TablaAux2["NumRemi"].ToString();
                        string op1 = "AD" + TRCN;
                        string op2 = "AF" + TRCN;
              
                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de control] SET TotalRegis = '" + TA + "'  WHERE NumRemi = '" + R + "' and CodArchivo = '"+ op1 + "' ";
                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);

                        Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos archivo de control] SET TotalRegis = '" + 1 + "'  WHERE NumRemi = '" + R + "' and CodArchivo = '" + op2 + "'";
                        EstadoAct = Conexion.SQLUpdate(Utils.SqlDatos);
                   
                }

                TablaAux2.Close();

                Utils.Titulo01 = "Control de ejecución";
                Utils.Informa = "Total facturas de CONSULTAS actualizadas " + TC + "\r";
                Utils.Informa += "Total facturas de HOSPITALIZACION actualizadas " + TH + "\r";
                Utils.Informa += "Total facturas de MEDICAMENTOS actualizadas " + TM + "\r";
                Utils.Informa += "Total facturas de OBSERVACION actualizadas " + TU + "\r";
                Utils.Informa += "Total facturas de OTROS SERVICIOS  actualizadas " + TOS + "\r";
                Utils.Informa += "Total facturas de PROCEDIMIENTOS actualizadas " + TP + "\r";
                Utils.Informa += "Total facturas de RECIEN NACIDOS actualizadas " + TN + "\r";
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la funcion NumFacUnico" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
