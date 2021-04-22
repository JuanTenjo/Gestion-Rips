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
    }
}
