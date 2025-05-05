using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Negocio
{
	public class ArticuloNegocio
	{
		public List<Articulo> listar()
		{
			List<Articulo> lista = new List<Articulo>();
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.setearConsulta("select Codigo,Nombre,A.Descripcion,Precio,ImagenUrl,C.Descripcion Categoria, M.Descripcion Marca,A.idCategoria,A.idMarca ,A.id from ARTICULOS A , CATEGORIAS C ,MARCAS M where C.Id = A.IdCategoria And M.Id = A.IdMarca");
				datos.ejecutarLectura();

                while (datos.Lector.Read())
				{

					Articulo aux = new Articulo();
					aux.Id = (int)datos.Lector["Id"];
					aux.Codigo = (string)datos.Lector["Codigo"];
					aux.Nombre = (string)datos.Lector["Nombre"];
					aux.Descripcion = (string)datos.Lector["Descripcion"];
					aux.Precio = (decimal)datos.Lector["Precio"];
					aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
					if (!(datos.Lector["ImagenUrl"] is DBNull))
					aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
					aux.Categoria = new Categoria();
					aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
					aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
					aux.Marca = new Marca();
					aux.Marca.Id = (int)datos.Lector["IdMarca"];
					aux.Marca.Descripcion = (string)datos.Lector["Marca"];

					lista.Add(aux);
					
				}
				return lista;

			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
             datos.cerrarConexion();

			}

		}
		public void agregar(Articulo nuevo)
        {
			AccesoDatos datos = new AccesoDatos();
			
            try
            {

			datos.setearConsulta(" insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio)Values(@Codigo,@Nombre,@Descripcion,@IdMarca,@IdCategoria,@ImagenUrl,@Precio)");
			datos.setearParametro("@Codigo", nuevo.Codigo);
			datos.setearParametro("@Nombre", nuevo.Nombre);
			datos.setearParametro("@Descripcion", nuevo.Descripcion);
			datos.setearParametro("@idMarca", nuevo.Marca.Id);
			datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
			datos.setearParametro("@ImagenUrl", nuevo.ImagenUrl);
			datos.setearParametro("@Precio", nuevo.Precio);


            datos.ejecutarAccion();

			}
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
				datos.cerrarConexion();

            }

        }
		public void modificar(Articulo art)
        {
			AccesoDatos datos = new AccesoDatos();
            try
            {
				datos.setearConsulta(" update ARTICULOS set Codigo = @Codigo,Nombre = @Nombre,Descripcion = @Descripcion,IdMarca = @IdMarca,IdCategoria = @IdCategoria,ImagenUrl = @ImagenUrl,Precio = @Precio where id = @id ");
				datos.setearParametro("@Codigo",art.Codigo);
				datos.setearParametro("@Nombre",art.Nombre);
				datos.setearParametro("@Descripcion", art.Descripcion);
				datos.setearParametro("@idMarca", art.Marca.Id);
				datos.setearParametro("@idCategoria", art.Categoria.Id);
				datos.setearParametro("@ImagenUrl", art.ImagenUrl);
				datos.setearParametro("@Precio",art.Precio);
				datos.setearParametro("@id", art.Id);
				
				datos.ejecutarAccion();

			}
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
				datos.cerrarConexion();
            }
        }
		public void eliminar(int id)
        {
            try
            {
				AccesoDatos datos = new AccesoDatos();
				datos.setearConsulta(" delete from ARTICULOS where id = @id");
				datos.setearParametro("@id", id);
				datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Articulo> filtrar(string BuscarPor, string criterio, string filtro)
		{
			List<Articulo> lista = new List<Articulo>();
			AccesoDatos datos = new AccesoDatos();

			try
			{
				string consulta = ("select Codigo,Nombre,A.Descripcion,Precio,ImagenUrl,C.Descripcion Categoria, M.Descripcion Marca,A.idCategoria,A.idMarca ,A.id from ARTICULOS A , CATEGORIAS C ,MARCAS M where C.Id = A.IdCategoria And M.Id = A.IdMarca And ");

				if (BuscarPor == "Precio")
				{
					switch (criterio)
					{
						case "Mayor a":
							consulta += "Precio>" + filtro;
							break;
						case "Menor a":
							consulta += "Precio <" + filtro;
							break;
						default:
							consulta += "Precio =" + filtro;
							break;

					}
				}
				else if (BuscarPor == "Marca")
				{
					switch (criterio)
					{
						case "Comienza con":
							consulta += "M.Descripcion Like '" + filtro + "%'";
							break;
						case "TFinaliza con":
							consulta += "M.Descripcion Like '%" + filtro + "' ";
							break;
						default:
						case "Contiene":
							consulta += "M.Descripcion Like '%" + filtro + "%' ";
						   break;

					}

				}
				else
				{
                    switch (criterio)
					{
						case "Comienza con":
							consulta += "C.Descripcion Like '" + filtro + "%'";
							break;
						case "Finaliza con":
							consulta += "C.Descripcion Like '%" + filtro + "' ";
							break;
						default:
						case "Contiene":
							consulta += "C.Descripcion Like  '%" + filtro + "%' ";
							break;

					}


				}

				datos.setearConsulta(consulta);
				datos.ejecutarLectura();
				while (datos.Lector.Read())
				{

					Articulo aux = new Articulo();
					aux.Id = (int)datos.Lector["Id"];
					aux.Codigo = (string)datos.Lector["Codigo"];
					aux.Nombre = (string)datos.Lector["Nombre"];
					aux.Descripcion = (string)datos.Lector["Descripcion"];
					aux.Precio = (decimal)datos.Lector["Precio"];
					aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
					if (!(datos.Lector["ImagenUrl"] is DBNull))
						aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
					aux.Categoria = new Categoria();
					aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
					aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
					aux.Marca = new Marca();
					aux.Marca.Id = (int)datos.Lector["IdMarca"];
					aux.Marca.Descripcion = (string)datos.Lector["Marca"];

					lista.Add(aux);
				}
				return lista;
			}
			 catch (Exception ex)
			{

             throw ex;
			}


		}

	}

}
