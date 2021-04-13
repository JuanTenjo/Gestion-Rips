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
namespace Gestion_Rips.Forms.Exportar
{
    public partial class FrmAnularRemi : Form
    {
        public FrmAnularRemi()
        {
            InitializeComponent();
        }

        private void FrmAnularRemi_Load(object sender, EventArgs e)
        {
            lblTexto.Text = "Por favor registre las razones por las cuales anula la remisión: " + Utils.RemiAnular;

        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtRazonAnul.Text) == false)
            {
                Utils.RazonAnul = txtRazonAnul.Text;
                this.Close();
            }
            else
            {
                Utils.Informa = "Lo siento pero no puedes anular una remision sin una razon" + "\r";

                MessageBox.Show("Control de anulacion", Utils.Titulo01, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
