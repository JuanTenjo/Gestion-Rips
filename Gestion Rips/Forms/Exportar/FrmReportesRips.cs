﻿using Gestion_Rips.Clases;
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
    public partial class FrmReportesRips : Form
    {
        public FrmReportesRips()
        {
            InitializeComponent();
        }

        #region RadioButton
        private void rbConsultas_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 1;
        }

        private void rbHospi_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 2;
        }

        private void rbMedica_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 3;
        }

        private void rbObser_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 4;
        }

        private void rbOtrosServ_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 5;
        }

        private void rbRecienNaci_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 6;
        }

        private void rbProcedi_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 7;
        }

        private void rbTrasnsacciones_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 8;
        }

        private void rbUsuarios_CheckedChanged(object sender, EventArgs e)
        {
            MarArchiRips = 9;
        }

        #endregion

        #region Funciones
        private void CargarDatos()
        {
            try
            {
                string Coenti01, CR;
                string Coenti02 = null, NEnti = null, UsSel = null, TDE = null, NCC = null, Para02 = null, Para01 = null, TUReg = null, AcRe = null;
                int SiNoP = 0, FunAudi = 0, FunUs = 0, FunFac = 0, FunCon = 0, FunHos = 0, FunObs = 0, FunMedi = 0, FunOtros = 0, FunReN = 0, FunProce = 0, TolInco = 0;
                double TolUsa, TolOtrosSer, TolConsul, TolHos, TolMedi, TolObs, TolOtros, TolReN, TolProce, TolFac, ValTolTras = 0, ValTolDeta = 0;
                string Sqlsuarios, SqlFacturas, SqlHospitalizados, SqlUrgencias, SqlRNacidos, SqlConsultas, SqlMedica, SqlProcedimientos, SqlOtrosServi;


                txtCardinal.Text = Utils.CarAdmin;
                txtNombre.Text = Utils.NomTerc;
                txtCodigoSGSS.Text = Utils.CodRips;
                lblCodigoUser.Text = Utils.codUsuario;
                lblNombreUser.Text = Utils.nomUsuario;

                UsSel = Utils.codUsuario;
                Coenti02 = Utils.CarAdmin;

                Sqlsuarios = "SELECT COUNT(CodDigita) AS TolUsuarios ";
                Sqlsuarios += "FROM [DARIPSXPSQL].[dbo].[Datos temporal usuarios RIPS]";
                Sqlsuarios += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                Sqlsuarios += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabUsuarios = Conexion.SQLDataReader(Sqlsuarios);

                if (TabUsuarios.HasRows == false)
                {

                    TolUsa = 0;
                    lblTotalConsultas.Text = "0";
                    lblTotalConsultas.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabUsuarios.Read();

                    if (Convert.ToInt32(TabUsuarios["TolUsuarios"].ToString()) <= 0)
                    {
                        TolUsa = 0;
                        lbltTotalUser.Text = "0";
                        lbltTotalUser.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        lbltTotalUser.Text = TabUsuarios["TolUsuarios"].ToString();
                    }
                }

                TabUsuarios.Close();

                SqlFacturas = "SELECT COUNT(CodDigita) AS TolFacturas, SUM(Copago) AS ValCopaFac, SUM(ValorNeto) AS ValNetoFac ";
                SqlFacturas += "FROM [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] ";
                SqlFacturas += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlFacturas += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabFacturas = Conexion.SQLDataReader(SqlFacturas);


                if (TabFacturas.HasRows == false)
                {
                    TolFac = 0;
                    lblTotalTransacciones.Text = "0";   
                    lblTotalTransacciones.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabFacturas.Read();
                    if (Convert.ToInt32(TabFacturas["TolFacturas"].ToString()) <= 0)
                    {
                        TolFac = 0;
                        lblTotalTransacciones.Text = "0";
                        lblTotalTransacciones.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolFac = Convert.ToDouble(TabFacturas["TolFacturas"]);
                        lblTotalTransacciones.Text = TabFacturas["TolFacturas"].ToString();
                        ValTolTras = Convert.ToDouble(TabFacturas["ValCopaFac"]) + Convert.ToDouble(TabFacturas["ValNetoFac"]);

                    }
                }

                TabFacturas.Close();

                SqlHospitalizados = "SELECT COUNT(CodDigita) AS TolHospi ";
                SqlHospitalizados += "FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] ";
                SqlHospitalizados += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlHospitalizados += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabHospitalizados = Conexion.SQLDataReader(SqlHospitalizados);

                if (TabHospitalizados.HasRows == false)
                {
                    TolHos = 0;
                    lblTotalHospi.Text = "0";
                    lblTotalHospi.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabHospitalizados.Read();
                    if (Convert.ToInt32(TabHospitalizados["TolHospi"].ToString()) <= 0)
                    {
                        TolHos = 0;
                        lblTotalHospi.Text = "0";
                        lblTotalHospi.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolHos = Convert.ToDouble(TabHospitalizados["TolHospi"].ToString());
                        lblTotalHospi.Text = TabHospitalizados["TolHospi"].ToString();
                    }
                }

                TabHospitalizados.Close();


                SqlUrgencias = "SELECT COUNT(CodDigita) AS TolObserva ";
                SqlUrgencias += "FROM [DARIPSXPSQL].[dbo].[Datos temporal observacion RIPS] ";
                SqlUrgencias += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlUrgencias += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabUrgencias = Conexion.SQLDataReader(SqlUrgencias);

                if (TabUrgencias.HasRows == false)
                {
                    TolObs = 0;
                    lblTotalObser.Text = "0";
                    lblTotalObser.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabUrgencias.Read();
                    if (Convert.ToInt32(TabUrgencias["TolObserva"].ToString()) <= 0)
                    {
                        TolObs = 0;
                        lblTotalObser.Text = "0";
                        lblTotalObser.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolObs = Convert.ToDouble(TabUrgencias["TolObserva"]);
                        lblTotalObser.Text = TabUrgencias["TolObserva"].ToString();
                    }
                }

                TabUrgencias.Close();


                SqlRNacidos = "SELECT COUNT(CodDigita) AS TolNacido ";
                SqlRNacidos += "FROM [DARIPSXPSQL].[dbo].[Datos temporal recien nacidos RIPS]";
                SqlRNacidos += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlRNacidos += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabRNacidos = Conexion.SQLDataReader(SqlRNacidos);

                if (TabRNacidos.HasRows == false)
                {
                    TolReN = 0;
                    lblTotalRecien.Text = "0";
                    lblTotalRecien.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabRNacidos.Read();
                    if (Convert.ToInt32(TabRNacidos["TolNacido"].ToString()) <= 0)
                    {
                        TolReN = 0;
                        lblTotalRecien.Text = "0";
                        lblTotalRecien.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        lblTotalRecien.Text = TabRNacidos["TolNacido"].ToString();
                        TolReN = Convert.ToDouble(TabRNacidos["TolNacido"]);
                    }
                }

                TabRNacidos.Close();

                SqlConsultas = "SELECT COUNT(CodDigita) AS TolConsultas, SUM(ValorConsul) AS ValtolConsul ";
                SqlConsultas += "FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS]";
                SqlConsultas += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlConsultas += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabConsultas = Conexion.SQLDataReader(SqlConsultas);

                if (TabConsultas.HasRows == false)
                {
                    TolConsul = 0;
                    lblTotalConsultas.Text = "0";
                    lblTotalConsultas.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabConsultas.Read();
                    if (Convert.ToInt32(TabConsultas["TolConsultas"].ToString()) <= 0)
                    {
                        TolConsul = 0;
                        lblTotalConsultas.Text = "0";
                        lblTotalConsultas.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolConsul = Convert.ToDouble(TabConsultas["TolConsultas"]); //por aqui

                        lblTotalConsultas.Text = TabConsultas["TolConsultas"].ToString();

                        ValTolDeta = Convert.ToDouble(TabConsultas["ValtolConsul"]);

                    }
                }

                TabConsultas.Close();

                SqlMedica = "SELECT COUNT(CodDigita) AS TolMedicamentos, SUM(ValorTotal) AS ValtolMedi ";
                SqlMedica += "FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] ";
                SqlMedica += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlMedica += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabMedica = Conexion.SQLDataReader(SqlMedica);

                if (TabMedica.HasRows == false)
                {
                    TolMedi = 0;
                    lblTotalMedica.Text = "0";
                    lblTotalMedica.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabMedica.Read();
                    if (Convert.ToInt32(TabMedica["TolMedicamentos"].ToString()) <= 0)
                    {
                        TolMedi = 0;
                        lblTotalMedica.Text = "0";
                        lblTotalMedica.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolMedi = Convert.ToDouble(TabMedica["TolMedicamentos"]);
                        lblTotalMedica.Text = TabMedica["TolMedicamentos"].ToString();
                        ValTolDeta = ValTolDeta + Convert.ToDouble(TabMedica["ValtolMedi"]);
                    }
                }

                TabMedica.Close();

                SqlProcedimientos = "SELECT COUNT(CodDigita) AS TolProcedimientos, SUM(ValorProce) AS ValtolProce ";
                SqlProcedimientos += "FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] ";
                SqlProcedimientos += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlProcedimientos += "(NumRemi = N'" + Coenti02 + "')";

                SqlDataReader TabProcedimientos = Conexion.SQLDataReader(SqlProcedimientos);

                if (TabProcedimientos.HasRows == false)
                {
                    TolProce = 0;
                    lblTotalProce.Text = "0";
                    lblTotalProce.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    TabProcedimientos.Read();
                    if (Convert.ToInt32(TabProcedimientos["TolProcedimientos"].ToString()) <= 0)
                    {
                        TolProce = 0;
                        lblTotalProce.Text = "0";
                        lblTotalProce.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolProce = Convert.ToDouble(TabProcedimientos["TolProcedimientos"]);
                        lblTotalProce.Text = TabProcedimientos["TolProcedimientos"].ToString();
                        ValTolDeta = ValTolDeta + Convert.ToDouble(TabProcedimientos["ValtolProce"]);
                    }
                }

                TabProcedimientos.Close();

                SqlOtrosServi = "SELECT COUNT(CodDigita) AS TolOtrosSer, SUM(ValorTotal) AS ValtolOtros ";
                SqlOtrosServi += "FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] ";
                SqlOtrosServi += "WHERE (CodDigita = N'" + UsSel + "') AND ";
                SqlOtrosServi += "(NumRemi = N'" + Coenti02 + "')";



                SqlDataReader TabOtrosServi = Conexion.SQLDataReader(SqlOtrosServi);

                if (TabOtrosServi.HasRows == false)
                {
                    TolOtrosSer = 0;
                    lblTotalOtrosServi.Text = "0";
                    lblTotalOtrosServi.BackColor = Color.FromArgb(255, 0, 0);
                }

                else
                {
                    TabOtrosServi.Read();
                    if (Convert.ToInt32(TabOtrosServi["TolOtrosSer"].ToString()) <= 0)
                    {
                        TolOtrosSer = 0;
                        lblTotalOtrosServi.Text = "0";
                        lblTotalOtrosServi.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        TolOtrosSer = Convert.ToDouble(TabOtrosServi["TolOtrosSer"]);
                        lblTotalOtrosServi.Text = TabOtrosServi["TolOtrosSer"].ToString();
                        ValTolDeta = ValTolDeta + Convert.ToDouble(TabOtrosServi["ValtolOtros"]);
                    }
                }

                TabOtrosServi.Close();

                txtTotalTrans.Text = Convert.ToString(ValTolTras);

                txtSumDeta.Text = Convert.ToString(ValTolDeta);

                double difer = ValTolTras - ValTolDeta;

                txtTolDifer.Text = Convert.ToString(difer);

                if (difer != 0)
                {
                    txtTolDifer.BackColor = Color.FromArgb(255, 0, 0);
                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "al mostrar el formulario de reportes Rips " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private int AuditaDetaFacturas(string CI)
        {

            try
            {
                //'Permite registrar el valor de detalle de cada facturas, para definir cual está descuadrada

                string TemEnti, NF;
                double TolCon, TolMedi, TolProce, TolOtros;
                string SqlUpdate;
                string Cardinal = Utils.CarAdmin;
                string CodDigita = Utils.codUsuario;

                Utils.SqlDatos = "SELECT * FROM DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS] WHERE [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "'  ";

                SqlDataReader TabLocal5 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TabLocal5.HasRows)
                {

                    Utils.SqlDatos = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] SET [Datos temporal transacciones RIPS].[VaLorDeta] = 0 WHERE [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "'  ";

                    Boolean ActConsul = Conexion.SQLUpdate(Utils.SqlDatos);

                }

                TabLocal5.Close();

                //Auditamos cada una de las facturas de consultas


                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS] WHERE [Datos temporal consultas RIPS].[NumRemi] = '" + CI + "'  ";

                SqlDataReader TabLocal1 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TabLocal1.HasRows == false)
                {
                    TolCon = 0;
                }
                else
                {
                    while (TabLocal1.Read())
                    {
                        TemEnti = TabLocal1["NumRemi"].ToString();
                        NF = TabLocal1["NumFactur"].ToString();
                        TolCon = Convert.ToDouble(TabLocal1["ValorConsul"].ToString());

                        Utils.SqlDatos = "SELECT * FROM DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS] WHERE [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "' AND [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "'   ";

                        TabLocal5 = Conexion.SQLDataReader(Utils.SqlDatos);
            

                        if (TabLocal5.HasRows == false)
                        {
                            //NO COPIA NADA, DIFICILMENTE
                        }
                        else
                        {
                            TabLocal5.Read();
                            SqlUpdate = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] SET [Datos temporal transacciones RIPS].[VaLorDeta] = '" + (Convert.ToDouble(TabLocal5["VaLorDeta"]) + TolCon) + "'  WHERE [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "' AND [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "'  ";
                            Boolean ActuValor = Conexion.SQLUpdate(SqlUpdate);
                            TabLocal5.Close();
                        }

                    }
                } //Fianl TabLocal1

                TabLocal1.Close();

                //'Suma los medicamentos

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal medicamentos RIPS] WHERE [Datos temporal medicamentos RIPS].[NumRemi] = '" + CI + "'  ";

                SqlDataReader TabLocal2 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TabLocal2.HasRows == false)
                {
                    TolMedi = 0;
                }
                else
                {
                    while (TabLocal2.Read())
                    {
                        TemEnti = TabLocal2["NumRemi"].ToString();
                        NF = TabLocal2["NumFactur"].ToString();
                        TolMedi = Convert.ToDouble(TabLocal2["ValorTotal"].ToString());

                        Utils.SqlDatos = "SELECT * FROM DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS] WHERE [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "' AND [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "'   ";

                        TabLocal5 = Conexion.SQLDataReader(Utils.SqlDatos);

                        if (TabLocal5.HasRows == false)
                        {
                            //NO COPIA NADA, DIFICILMENTE
                        }
                        else
                        {
                            TabLocal5.Read();
                            SqlUpdate = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] SET [Datos temporal transacciones RIPS].[VaLorDeta] = '" + (Convert.ToDouble(TabLocal5["VaLorDeta"]) + TolMedi) + "'  WHERE [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "' AND [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "'  ";
                            Boolean ActuValor = Conexion.SQLUpdate(SqlUpdate);
                            TabLocal5.Close();
                        }

                    }

                } //Fianl TabLocal1

                TabLocal2.Close();

                //Proceda a sumar otros servicios

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal otros servicios RIPS] WHERE [Datos temporal otros servicios RIPS].[NumRemi] = '" + CI + "'  ";

                SqlDataReader TabLocal3 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TabLocal3.HasRows == false)
                {
                    TolOtros = 0;
                }
                else
                {
                    while (TabLocal3.Read())
                    {
                        TemEnti = TabLocal3["NumRemi"].ToString();
                        NF = TabLocal3["NumFactur"].ToString();
                        TolOtros = Convert.ToDouble(TabLocal3["ValorTotal"].ToString());

                        Utils.SqlDatos = "SELECT * FROM DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS] WHERE [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "' AND [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "'   ";

                        TabLocal5 = Conexion.SQLDataReader(Utils.SqlDatos);

                        if (TabLocal5.HasRows == false)
                        {
                            //NO COPIA NADA, DIFICILMENTE
                        }
                        else
                        {
                            TabLocal5.Read();
                            SqlUpdate = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] SET [Datos temporal transacciones RIPS].[VaLorDeta] = '" + (Convert.ToDouble(TabLocal5["VaLorDeta"]) + TolOtros) + "'  WHERE [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "' AND [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "'  ";
                            Boolean ActuValor = Conexion.SQLUpdate(SqlUpdate);
                            TabLocal5.Close();
                        }

                    }

                } //Fianl TabLocal1

                TabLocal3.Close();

                //'Proceda a sumar los procedimientos

                Utils.SqlDatos = "SELECT * FROM [DARIPSXPSQL].[dbo].[Datos temporal procedimientos RIPS] WHERE [Datos temporal procedimientos RIPS].[NumRemi] = '" + CI + "'  ";

                SqlDataReader TabLocal4 = Conexion.SQLDataReader(Utils.SqlDatos);

                if (TabLocal4.HasRows == false)
                {
                    TolProce = 0;
                }
                else
                {
                    while (TabLocal4.Read())
                    {
                        TemEnti = TabLocal4["NumRemi"].ToString();
                        NF = TabLocal4["NumFactur"].ToString();
                        TolProce = Convert.ToDouble(TabLocal4["ValorProce"].ToString());

                        Utils.SqlDatos = "SELECT * FROM DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS] WHERE [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "' AND [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "'   ";

                        TabLocal5 = Conexion.SQLDataReader(Utils.SqlDatos);

                        if (TabLocal5.HasRows == false)
                        {
                            //NO COPIA NADA, DIFICILMENTE
                        }
                        else
                        {
                            TabLocal5.Read();
                            SqlUpdate = "UPDATE [DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] SET [Datos temporal transacciones RIPS].[VaLorDeta] = '" + (Convert.ToDouble(TabLocal5["VaLorDeta"]) + TolProce) + "'  WHERE [Datos temporal transacciones RIPS].[NumFactur] = '" + NF + "' AND [Datos temporal transacciones RIPS].[NumRemi] = '" + CI + "'  ";
                            Boolean ActuValor = Conexion.SQLUpdate(SqlUpdate);
                            TabLocal5.Close();
                        }

                    }

                } //Fianl TabLocal1

                TabLocal4.Close();


                return 1;
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en la función: AuditaDetaFacturas del" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

        }

        #endregion

        #region botones
        private void btnAuditar_Click(object sender, EventArgs e)
        {
            try
            {
                string NE, NomInfo, Para01, Para02, Citer, UsSel;

                int SinAr, FunAudi;

                double TolFac;

                Utils.Titulo01 = "Control para mostrar auditar facturas";


                if (string.IsNullOrWhiteSpace(txtCardinal.Text))
                {
                    Utils.Informa = "Lo siento pero mientra no exista el cardinal" + "\r";
                    Utils.Informa += "de identificación de la entidad o convenio," + "\r";
                    Utils.Informa += "no se puede auditar las facturas" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Para01 = txtCardinal.Text;
                NE = txtNombre.Text;
                UsSel = Utils.codUsuario;

                if (Convert.ToDouble(txtTotalTrans.Text) <= 0)
                {
                    Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                    Utils.Informa += "no existen facturas para audita," + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    TolFac = Convert.ToDouble(lblTotalTransacciones.Text);
                    Utils.Informa = "¿Usted desea revisar las " + TolFac + " Facturas " + "\r";
                    Utils.Informa += "de la entidad " + NE;
                }

                var res = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    Citer = txtCardinal.Text;

                    FunAudi = AuditaDetaFacturas(Citer);

                    if(FunAudi == 1)
                    {
                        Utils.SqlDatos = "SELECT [Datos empresas y terceros].CodiMinSalud,  " +
                                        "Trim([Datos empresas y terceros].[NomAdmin] + ' ' + [ProgrAmin]) AS NE, [Datos empresas y terceros].TipoDocu, " +
                                        "[Datos empresas y terceros].NumDocu, [Datos temporal transacciones RIPS].NumFactur, [Datos temporal transacciones RIPS].NumRemi, " +
                                        "[Datos temporal transacciones RIPS].Copago, [Datos temporal transacciones RIPS].ValorNeto, " +
                                        "[Datos temporal transacciones RIPS].VaLorDeta, Abs(([ValorNeto] +[Copago]) -[VaLorDeta]) AS DT " +
                                        "FROM[Datos empresas y terceros] " +
                                        "INNER JOIN[DARIPSXPSQL].[dbo].[Datos temporal transacciones RIPS] ON[Datos empresas y terceros].CarAdmin = [Datos temporal transacciones RIPS].NumRemi " +
                                        "WHERE(((Abs(([ValorNeto] +[Copago]) -[VaLorDeta])) > 0)) AND [Datos temporal transacciones RIPS].NumRemi = '"+ Para01 +"'";

                        Utils.infNombreInforme = "dsInfInformeConsultaAuditar.rdlc";

                        Reportes.FrmInInformeAuditar frm = new Reportes.FrmInInformeAuditar();
                        frm.ShowDialog();

                    }

                }

            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "hacer click sobre el botón auditar " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {
                string NE, NomInfo, Citer, Citer1, Citer2, Para01, Para02;
                int SinAr = 0;


                Utils.Titulo01 = "Control para mostrar datos de RIPS";


                if (string.IsNullOrWhiteSpace(txtCardinal.Text))
                {
                    Utils.Informa = "Lo siento pero mientra no exista el cardinal" + "\r";
                    Utils.Informa += "de identificación de la entidad o convenio," + "\r";
                    Utils.Informa += "no se puede mostrar los datos" + "\r";
                    MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Para01 = Utils.codUsuario;
                Para02 = txtCardinal.Text;

                NE = txtNombre.Text;

                switch (MarArchiRips)
                {
                    case 1: //Mostrar el archivo de consultas
                        if (Convert.ToInt32(lblTotalConsultas.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen consultas para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de Consultas?";

                            Utils.SqlDatos = "SELECT [Datos temporal consultas RIPS].CodDigita, [Datos temporal consultas RIPS].NumRemi, [Datos temporal consultas RIPS].NumFactur, [Datos temporal consultas RIPS].CodIPS, [Datos temporal consultas RIPS].TipoDocum, " +
                                             "[Datos temporal consultas RIPS].NumDocum, [Datos temporal consultas RIPS].FecConsul, [Datos temporal consultas RIPS].AutoriNum, [Datos temporal consultas RIPS].CodConsul, [Datos temporal consultas RIPS].FinalConsul, " +
                                             "[Datos temporal consultas RIPS].CausExter, [Datos temporal consultas RIPS].DxPrincipal, [Datos temporal consultas RIPS].DxRelacion1, [Datos temporal consultas RIPS].DxRelacion2, " +
                                             "[Datos temporal consultas RIPS].DxRelacion3, [Datos temporal consultas RIPS].TipoDxPrin, [Datos temporal consultas RIPS].ValorConsul, [Datos temporal consultas RIPS].ValorCuota, [Datos temporal consultas RIPS].ValorNeto," +
                                             "[Datos temporal consultas RIPS].VezAno " +
                                             "FROM [DARIPSXPSQL].[dbo].[Datos temporal consultas RIPS] INNER JOIN " +
                                             "ACDATOXPSQL.dbo.[Datos empresas y terceros] ON [Datos temporal consultas RIPS].NumRemi = ACDATOXPSQL.dbo.[Datos empresas y terceros].CarAdmin INNER JOIN " +
                                             "BDADMINSIG.dbo.[Datos informacion de la empresa] ON[Datos temporal consultas RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                             "WHERE([Datos temporal consultas RIPS].CodDigita = '" + Para01 + "') AND ([Datos temporal consultas RIPS].NumRemi = '" + Para02 + "') ORDER BY [Datos temporal consultas RIPS].FecConsul ASC";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaRemision.rdlc";


                        }
                        break;
                    case 2: //Mostrar el archivo de Hospitalización
                        if (Convert.ToInt32(lblTotalHospi.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen hospitalizados para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de hospitalizados?";

                            Utils.SqlDatos = "SELECT [Datos temporal hospitalizacion RIPS].CodDigita, [Datos temporal hospitalizacion RIPS].NumRemi, [Datos temporal hospitalizacion RIPS].NumFactur, [Datos temporal hospitalizacion RIPS].CodIPS,  " +
                                            " [Datos temporal hospitalizacion RIPS].TipoDocum, [Datos temporal hospitalizacion RIPS].NumDocum, [Datos temporal hospitalizacion RIPS].ViaDIngreso, [Datos temporal hospitalizacion RIPS].FecIngresa,  " +
                                            " [Datos temporal hospitalizacion RIPS].HorIngresa, [Datos temporal hospitalizacion RIPS].AutoriNum, [Datos temporal hospitalizacion RIPS].CausExter, [Datos temporal hospitalizacion RIPS].DxPrincIngre,  " +
                                            " [Datos temporal hospitalizacion RIPS].DxPrincEgre, [Datos temporal hospitalizacion RIPS].DxRelacion1, [Datos temporal hospitalizacion RIPS].DxRelacion2, [Datos temporal hospitalizacion RIPS].DxRelacion3,  " +
                                            " [Datos temporal hospitalizacion RIPS].DxComplica, [Datos temporal hospitalizacion RIPS].EstadoSal, [Datos temporal hospitalizacion RIPS].DxMuerte, [Datos temporal hospitalizacion RIPS].FecSalida,  " +
                                            " [Datos temporal hospitalizacion RIPS].HorSalida " +
                                            " FROM [DARIPSXPSQL].[dbo].[Datos temporal hospitalizacion RIPS] INNER JOIN " +
                                            " ACDATOXPSQL.dbo.[Datos empresas y terceros] ON[Datos temporal hospitalizacion RIPS].NumRemi = ACDATOXPSQL.dbo.[Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            " BDADMINSIG.dbo.[Datos informacion de la empresa] ON[Datos temporal hospitalizacion RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            " WHERE([Datos temporal hospitalizacion RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal hospitalizacion RIPS].NumRemi = '" + Para02 + "') " +
                                            " ORDER BY[Datos temporal hospitalizacion RIPS].FecIngresa";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaHospitalizacion.rdlc";

                        }

                        break;
                    case 3: //Mostrar el archivo de medicamentos
                        if (Convert.ToInt32(lblTotalMedica.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen medicamentos para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de medicamentos?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NumFactur,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].CodIPS, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].TipoDocum, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NumDocum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].AutoriNum, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].CodMedica, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].TipoMedica,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NomGenerico, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].FormaFarma, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].ConcenMedi,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].UniMedida, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NumUnidad, DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].ValorUnita,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].ValorTotal " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NumRemi = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            "BDADMINSIG.dbo.[Datos informacion de la empresa] ON DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            "WHERE([Datos temporal medicamentos RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal medicamentos RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal medicamentos RIPS].NomGenerico";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaMedicamentos.rdlc";


                        }

                        break;
                    case 4: //Mostrar el RIPS archivo de Observación
                        if (Convert.ToInt32(lblTotalObser.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen Observación para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de Observación?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].NumFactur,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].CodIPS, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].TipoDocum, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].NumDocum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].FecIngresa, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].HorIngresa, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].AutoriNum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].CausExter, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].DxPrincIngre, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].DxRelacion1,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].DxRelacion2, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].DxRelacion3, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].Destino,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].EstadoSal, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].DxMuerte, DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].FecSalida,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].HorSalida " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal observacion RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].NumRemi = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            "BDADMINSIG.dbo.[Datos informacion de la empresa] ON DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            "WHERE([Datos temporal observacion RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal observacion RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal observacion RIPS].FecIngresa";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaObservacion.rdlc";



                        }

                        break;
                    case 5: //Mostrar el archivo de otros servicios
                        if (Convert.ToInt32(lblTotalOtrosServi.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen otros servicios para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de Otros servicios?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].NumFactur,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].CodIPS, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].TipoDocum, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].NumDocum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].AutoriNum, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].TipoServicio, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].CodiServi,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].NomServi, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].Cantidad, DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].ValorUnita,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].ValorTotal " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].NumRemi = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            "BDADMINSIG.dbo.[Datos informacion de la empresa] ON DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            "WHERE([Datos temporal otros servicios RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal otros servicios RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal otros servicios RIPS].NomServi";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaOtrosServi.rdlc";

                        }

                        break;

                    case 6: //Mostrar el archivo de recien nacidos
                        if (Convert.ToInt32(lblTotalRecien.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen recien nacidos para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de Recien Nacidos?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].NumFactur,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].CodIPS, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].TipoDocum, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].NumDocum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].FecNaci, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].HorIngresa, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].EdadGesta,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].ControlPrena, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].SexoRecien, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].PesoRecien,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].DxRecien, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].DxMuerte, DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].FecMuerte,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].HorMuerte " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].NumRemi = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            "BDADMINSIG.dbo.[Datos informacion de la empresa] ON DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            "WHERE([Datos temporal recien nacidos RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal recien nacidos RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal recien nacidos RIPS].FecNaci";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaRecienNaci.rdlc";

                        }

                        break;

                    case 7: //Mostrar el archivo de procedimiento
                        if (Convert.ToInt32(lblTotalProce.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen procedimiento para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de procedimientos?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].NumFactur,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].CodIPS, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].TipoDocum, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].NumDocum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].FecProce, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].AutoriNum, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].CodProce,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].AmbitoReal, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].FinalProce, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].PersonAten,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].DxPrincipal, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].DxRelacion, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].Complicacion,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].RealiActo, DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].ValorProce " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].NumRemi = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            "BDADMINSIG.dbo.[Datos informacion de la empresa] ON DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            "WHERE([Datos temporal procedimientos RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal procedimientos RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal procedimientos RIPS].FecProce";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaProcedimientos.rdlc";

                        }

                        break;

                    case 8: //Mostrar el archivo de transacciones
                        if (Convert.ToInt32(lblTotalTransacciones.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen transacciones para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de transacciones?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].CodIPS,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].RazonSocial, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].TipIdenti, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NumIdenti,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NumFactur, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].FecFactur, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].FecInicio,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].FecFinal, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].CodAdmin, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NomAdmin,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NumContra, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].PlanBene, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NumPoli,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].Copago, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].ValorComi, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].ValorDes,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].ValorNeto, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].VaLorDeta, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].CausExter " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].NumRemi = [Datos empresas y terceros].CarAdmin INNER JOIN " +
                                            "BDADMINSIG.dbo.[Datos informacion de la empresa] ON DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].CodIPS = BDADMINSIG.dbo.[Datos informacion de la empresa].CodiMinSalud " +
                                            "WHERE([Datos temporal transacciones RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal transacciones RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].FecFactur, DARIPSXPSQL.dbo.[Datos temporal transacciones RIPS].RazonSocial";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaTransacciones.rdlc";

                        }

                        break;
                    case 9: //Mostrar el archivo de usuarios
                        if (Convert.ToInt32(lblTotalTransacciones.Text) <= 0)
                        {
                            Utils.Informa = "Lo siento pero para entidad de nombre " + NE + "\r";
                            Utils.Informa += "no existen usuarios para mostrar" + "\r";
                            MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            Utils.Informa = "¿Usted desea mostrar el RIPS de usuarios?";

                            Utils.SqlDatos = "SELECT DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].CodDigita, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].NumRemi, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].TipoDocum,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].NumDocum, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].CodAdmin, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].TipUsuario,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Apellido1, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Apellido2, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Nombre1,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Nombre2, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Edad, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].EdadMedi,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Sexo, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].CodDpto, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].CodMuni,  " +
                                            "DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].ZonaResi, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Exportado, DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].CodDptoCity " +
                                            "FROM DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS] INNER JOIN " +
                                            "[Datos empresas y terceros] ON DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].NumRemi = [Datos empresas y terceros].CarAdmin " +
                                            "WHERE([Datos temporal usuarios RIPS].CodDigita = '" + Para01 + "') AND([Datos temporal usuarios RIPS].NumRemi = '" + Para02 + "') " +
                                            "ORDER BY DARIPSXPSQL.dbo.[Datos temporal usuarios RIPS].Apellido1";

                            Utils.CarAdmin = Para02;

                            Utils.infNombreInforme = "dsInfInformeConsultaUsuarios.rdlc";



                        }

                        break;
                    default:
                        Utils.Informa = "Selecciona un archivo a mostrar " + "\r";
                        MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                }

                var respuesta = MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (respuesta == DialogResult.Yes)
                {
                    Reportes.FrmInfReportesRIPS frm = new Reportes.FrmInfReportesRIPS();
                    frm.ShowDialog();
                }


            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "hacer click sobre el botón mostrar " + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        int MarArchiRips = 0;
        private void FrmReportesRips_Load(object sender, EventArgs e)
        {
            try
            {
                CargarDatos();
            }
            catch (Exception ex)
            {
                Utils.Titulo01 = "Control de errores de ejecución";
                Utils.Informa = "Lo siento pero se ha presentado un error" + "\r";
                Utils.Informa += "en el formulario Reportes Rips" + "\r";
                Utils.Informa += "Módulo gestión de RIPS" + "\r";
                Utils.Informa += "Error: " + ex.Message + " - " + ex.StackTrace;
                MessageBox.Show(Utils.Informa, Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



    }
}
