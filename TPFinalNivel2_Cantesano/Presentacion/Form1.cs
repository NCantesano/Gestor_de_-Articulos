using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;


namespace Presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Articulo> listaArticulo;
        
        public frmArticulos()
        {
            InitializeComponent();
        }
        
        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();
            cboBuscarPor.Items.Add("Categoria");
            cboBuscarPor.Items.Add("Marca");
            cboBuscarPor.Items.Add("Precio");
        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "$ 0.00";


            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
            
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                ocultarColumnas();
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulos.Load(" https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png ");
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();

            if (txtFiltro.Text.Length > 0)
            {
                MessageBox.Show("Debes limpiar el filtro antes de agregar el articulo.");
            }
            else
            {
                alta.ShowDialog();
                cargar();
            }
            
        }
        private void btnModificar_Click(object sender, EventArgs e)

        {
            if (!string.IsNullOrEmpty(txtFiltro.Text))
            {
                MessageBox.Show("Debes limpiar el filtro antes de modificar el articulo.");
                return;                                                                            
            }
            
            if (dgvArticulos.CurrentRow == null)
            {
                MessageBox.Show("Por favor, selecciona un artículo para modificar.");
                return;
            }

            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();

        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                if (!string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Debes limpiar el filtro antes de eliminar el articulo.");
                    return;
                }


                if (dgvArticulos.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un artículo para eleminar.");
                    return;
                }

                DialogResult respuesta = MessageBox.Show(" Esta seguro de eliminar el articulo ?" ,"Eliminando",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if( respuesta == DialogResult.Yes)
                {
                    
                   seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                   negocio.eliminar(seleccionado.Id);
                   cargar();

                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;
                string buscarPor= cboBuscarPor.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(buscarPor, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            ocultarColumnas();
        }
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)

            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x. Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }
        private void cboBuscarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboBuscarPor.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Igual a");
                cboCriterio.Items.Add("Menor a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con ");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
        private bool validarFiltro()
        {
            if (cboBuscarPor.SelectedIndex < 0)

            {
                MessageBox.Show(" Por favor, seleccione el campo para filtrar. ");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show(" Por favor, seleccione el criterio para filtrar. ");
                return true;
            }
            if (cboBuscarPor.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numericos....");
                    return true;
                }
                if (!(esNumeroValido(txtFiltroAvanzado.Text)))

                {
                    MessageBox.Show("Debes ingresas solo datos numericos");
                    return true;
                }

            }
            if (cboBuscarPor.SelectedItem.ToString() == "Categoria")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para letras....");
                    return true;
                }
                if (!(soloLetras(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Debes ingresar solo letras...");
                    return true;
                }
            }
            if (cboBuscarPor.SelectedItem.ToString() == "Marca")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para letras....");
                    return true;
                }
                if (!(soloLetras(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Debes ingresas solo letras");
                    return true;
                }
               
            }

            return false;
        }
        private bool soloLetras(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsLetter(caracter)))
                    return false;
            }
            return true;
        }
        private bool esNumeroValido(string cadena)

        {
            return decimal.TryParse(cadena, out _);
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Clear();
        }
        private void txtFiltroAvanzado_TextChanged(object sender, EventArgs e)
        {
            int cursorPosition = txtFiltroAvanzado.SelectionStart;
            string originalTex = txtFiltroAvanzado.Text;
            string newText = originalTex.Replace(',', '.');

            if (newText != originalTex)
            {
                txtFiltroAvanzado.Text = newText;
                txtFiltroAvanzado.SelectionStart = cursorPosition;
                txtFiltroAvanzado.SelectionLength = 0;
            }
        }

        
    }

        
}
