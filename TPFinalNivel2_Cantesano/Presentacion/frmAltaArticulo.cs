using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;



namespace Presentacion
{

    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        
        public frmAltaArticulo()
        {

        InitializeComponent();

        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (validarAceptar())
                    return;

                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Precio = Decimal.Parse(txtPrecio.Text.ToString());

                if (articulo.Id != 0)
                {

                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");

                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

                if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))

                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["image-folder"] + archivo.SafeFileName);


                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }

        }
        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            
          CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

           try
           {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "id";
                cboCategoria.DisplayMember = "Descripcion";


                MarcaNegocio marcaNegocio = new MarcaNegocio();

                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "id";
                cboMarca.DisplayMember = "Descripcion";



                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    txtPrecio.Text = articulo.Precio.ToString();
                    cargarImagen(articulo.ImagenUrl);
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }

           }
           catch (Exception ex)

           {

              MessageBox.Show(ex.ToString());
           }
            

        }
        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
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
        private void btnAgregarImagen_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|jpeg|*.jpeg";

            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }

        }
        
        private bool validarAceptar()
        {

            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                MessageBox.Show("Debes cargar el precio del producto....");
                return true;
            }

            if (!esNumeroValido(txtPrecio.Text))
            {
                MessageBox.Show("Debes ingresar solo datos numéricos.");
                return true;
            }

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Nombre , Campo obligatorio....");
                return true;
            }

            return false;
        }
        private bool esNumeroValido(string cadena)

        {
           
           return decimal.TryParse(cadena, out _);
            
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
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            
            int cursorPosition = txtPrecio.SelectionStart;
            string originalTex = txtPrecio.Text;
            string newText = originalTex.Replace('.', ',');

            if(newText != originalTex)
            {
                txtPrecio.Text = newText;
                txtPrecio.SelectionStart = cursorPosition;
                txtPrecio.SelectionLength = 0;
            }

        }

    }


}
   
            
        
    



