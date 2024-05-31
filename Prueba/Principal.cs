using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prueba;
using static System.Net.Mime.MediaTypeNames;

namespace Prueba
{
   
    public partial class Examenes : Form
    {

        
        public Examenes()
        {
            InitializeComponent();
        }

        private void Examenes_Load(object sender, EventArgs e)
        {
            carga();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardaRegistro();
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            EliminarRegistro();

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {            
            ConsultaRegistro();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarRegistro();
        }

        private void cboxtipoOpe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboxtipoOpe.Text == "Guardar")
            {
                btnGuardar.Visible = true;            
                btneliminar.Visible = false;
                btnActualizar.Visible = false;
                btnConsultar.Visible = false;
                txtid.Text = "";
                txtnombre.Text = "";
                txtdescripcion.Text = "";
                txtdescripcion.Enabled = true;
                txtid.Enabled = true;
                txtnombre.Enabled = true;
            }
            else if(cboxtipoOpe.Text == "Eliminar")
            {
                btneliminar.Visible = true;
                btnGuardar.Visible = false;               
                btnActualizar.Visible = false;
                btnConsultar.Visible = false;
                txtdescripcion.Enabled=false;
                txtnombre.Enabled = false;
                txtid.Enabled=true;
                txtid.Text = "";
                txtnombre.Text = "";
                txtdescripcion.Text = "";
            }
            else if(cboxtipoOpe.Text == "Actualizar")
            {
                btnGuardar.Visible = false;
                btneliminar.Visible = false;
                btnActualizar.Visible = false;
                btnActualizar.Visible = true;
                txtid.Enabled = true;
                txtnombre.Enabled = true;
                txtdescripcion.Enabled = true;
                txtid.Text = "";
                txtnombre.Text = "";
                txtdescripcion.Text = "";
            }
            else if(cboxtipoOpe.Text == "Consultar")
            {
                btnGuardar.Visible = false;
                btneliminar.Visible = false;
                btnActualizar.Visible = false;
                btnConsultar.Visible = true;
                txtid.Enabled= false;
                txtnombre.Enabled = true;
                txtdescripcion.Enabled = true;
                txtid.Text = "";
                txtnombre.Text = "";
                txtdescripcion.Text = "";
            }
        }

        private void GuardaRegistro()
        {
            using (var db = new RECIBOSEntities1())
            {
                int id = Convert.ToInt32(txtid.Text);
                try
                {
                    var CodReturn = new SqlParameter("@CodigoRetorno", SqlDbType.Int);
                    CodReturn.Direction = ParameterDirection.ReturnValue;

                     db.Database.ExecuteSqlCommand("EXEC spIngresa @id, @Nombre, @Descripcion",
                        CodReturn,
                         new SqlParameter("@id", id),
                        new SqlParameter("@Nombre", txtnombre.Text),
                        new SqlParameter("@Descripcion", txtdescripcion.Text)
                        );
                   
                    int codigoRetorno = (int)CodReturn.Value;

                    if (codigoRetorno == 0)
                    {
                        MessageBox.Show("Se guardó el registro");
                    }
                    else
                    {
                        txtid.Text = "";
                        txtnombre.Text = "";
                        txtdescripcion.Text = "";
                        MessageBox.Show("Ocurrío un error al guardar el registro");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error : {ex.Message}");
                }
            }
        }
        private void EliminarRegistro()
        {
            using (var db = new RECIBOSEntities1())
            {
                int id = Convert.ToInt32(txtid.Text);
                try
                {
                    var CodReturn = new SqlParameter("@CodigoRetorno", SqlDbType.Int);
                    CodReturn.Direction = ParameterDirection.ReturnValue;

                    db.Database.ExecuteSqlCommand("EXEC spEliminar @id",
                       CodReturn,
                        new SqlParameter("@id", id));

                    int codigoRetorno = (int)CodReturn.Value;

                    if (codigoRetorno == 0)
                    {
                        MessageBox.Show("Se eliminó correctamente el registro");
                    }
                    else
                    {
                        txtid.Text = "";
                        txtnombre.Text = "";
                        txtdescripcion.Text = "";
                        MessageBox.Show("Ocurrío un error al  el registro");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error : {ex.Message}");
                }
            }
        }
        
        private void ActualizarRegistro()
        {
            using (var db = new RECIBOSEntities1())
            {
                int id = Convert.ToInt32(txtid.Text);
                try
                {
                    var CodReturn = new SqlParameter("@CodigoRetorno", SqlDbType.Int);
                    CodReturn.Direction = ParameterDirection.ReturnValue;

                    db.Database.ExecuteSqlCommand("EXEC spActualizar @id, @Nombre, @Descripcion",
                       CodReturn,
                        new SqlParameter("@id", id),
                       new SqlParameter("@Nombre", txtnombre.Text),
                       new SqlParameter("@Descripcion", txtdescripcion.Text)
                       );

                    int codigoRetorno = (int)CodReturn.Value;

                    if (codigoRetorno == 0)
                    {
                        MessageBox.Show("Se guardo correctamente el registro");
                    }
                    else
                    {
                        txtid.Text = "";
                        txtnombre.Text = "";
                        txtdescripcion.Text = "";
                        MessageBox.Show("Ocurrío un error al guardar el registro");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error : {ex.Message}");
                }
            }
        }

        private void ConsultaRegistro()
        {
            using(var db = new RECIBOSEntities1())
            {                
               var registros = db.spConsultar(txtnombre.Text, txtdescripcion.Text).ToList();
                if (registros.Count != 0)
                {
                    dgDatos.Visible = true;
                    dgDatos.DataSource = registros;
                }
                else
                {
                    MessageBox.Show("No existen registros con ese nombre: "+ txtnombre.Text+ " y esa descripción "+ txtdescripcion.Text);
                }
            }
        }
        
        private void carga()
        {
            btnGuardar.Visible = false;
            btneliminar.Visible = false;
            btnActualizar.Visible = false;
            btnConsultar.Visible = false;
            dgDatos.Visible = false;
            txtdescripcion.Enabled = false;
            txtid.Enabled = false;
            txtnombre.Enabled = false;
        }

        private void txtid_Leave(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtid.Text);
            using (var db = new RECIBOSEntities1())
            {
                var existeregistro = db.tblExamen.FirstOrDefault(x => x.idExamen == id);
                if (existeregistro != null)
                {
                    MessageBox.Show("El registro ya existe, intente con otro");
                    txtid.Text = "";
                }
            }
        }


    }
}

