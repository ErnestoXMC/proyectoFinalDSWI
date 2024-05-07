using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using proyectoHospital.Models;
using proyectoHospital.Repositorio.Contrato;

namespace proyectoHospital.Repositorio.Implementacion
{
    public class AreaRepositorio : IArea
    {
        private readonly string cadena;
        public AreaRepositorio()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cadenaSQL");
        }
        public IEnumerable<Area> listadoAreas()
        {
            List<Area> listar = new List<Area>();
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                cone.Open();
                SqlCommand cmd = new SqlCommand("exec sp_listarArea", cone);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listar.Add(new Area()
                    {
                        cod_area = dr.GetInt32(0),
                       nom_area = dr.GetString(1)
                    });
                }
                dr.Close();

            }
            return listar;
        }
        public Area buscar(int id)
        {
            return listadoAreas().Where(v => v.cod_area == id).FirstOrDefault();
        }

        public string eliminarArea(int cod_area)
        {
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminarArea", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_area", cod_area));

                    int i = cmd.ExecuteNonQuery(); // Ejecuta el procedimiento almacenado

                    if (i > 0)
                    {
                        return "Área eliminada con éxito.";
                    }
                    else
                    {
                        return "No se encontró el área con el código especificado.";
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de la excepción
                    return $"Error al eliminar el área: {ex.Message}";
                }
            }
        }
        public string ingresarArea(Area ar)
        {
            string mensaje = "";
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_insertarArea", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nom_area", ar.nom_area);
                    cone.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha insertado {i} Area exitosamente";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cone.Close(); }
            }
            return mensaje;
        }
        public string actualizarArea(int cod_area, string nom_area)
        {
            string mensaje = "";
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_actualizarArea", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_area", cod_area));
                    cmd.Parameters.Add(new SqlParameter("@nom_area", nom_area));
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} Área exitosamente";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cone.Close(); }
            }
            return mensaje;
        }

    }
}
