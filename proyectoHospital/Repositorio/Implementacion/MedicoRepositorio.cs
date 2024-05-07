using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using proyectoHospital.Models;
using proyectoHospital.Repositorio.Contrato;

namespace proyectoHospital.Repositorio.Implementacion
{
    public class MedicoRepositorio : IMedico
    {
        private readonly string cadena;
        public MedicoRepositorio()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cadenaSQL");
        }
        public IEnumerable<Medico> listadoMedicos()
        {
            List<Medico> listar = new List<Medico>();
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                cone.Open();
                SqlCommand cmd = new SqlCommand("exec sp_listarMedico", cone);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listar.Add(new Medico()
                    {
                        cod_medico = dr.GetInt32(0),
                        nom_med = dr.GetString(1),
                        edad_med = dr.GetInt32(2),
                        sexo_med = dr.GetString(3),
                        cod_area = dr.GetString(4)
                    });
                }
                dr.Close();

            }
            return listar;

        }
        public Medico buscar(int id)
        {
            return listadoMedicos().Where(v => v.cod_medico == id).FirstOrDefault();
        }

        public string ingresarMedico(Medico med)
        {
            string mensaje = "";
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_insertarMedico", cone);
                    cmd.Parameters.AddWithValue("@nom_med", med.nom_med);
                    cmd.Parameters.AddWithValue("@edad_med", med.edad_med);
                    cmd.Parameters.AddWithValue("@sexo_med", med.sexo_med);
                    cmd.Parameters.AddWithValue("@cod_area", med.cod_area);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha insertado {i} Medico exitosamente";
                }

                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cone.Close(); }
            }
            return mensaje;
        
        }
        public string eliminarMedico(int cod_medico)
        {
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminarMedico", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_medico", cod_medico));

                    int i = cmd.ExecuteNonQuery(); 

                    if (i > 0)
                    {
                        return "Médico eliminado con éxito.";
                    }
                    else
                    {
                        return "No se encontró el Médico con el código especificado.";
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de la excepción
                    return $"Error al eliminar al médico: {ex.Message}";
                }
            }
        }
        public string actualizarMedico(int cod_medico, string nom_medico, int edad_medico, string sexo_medico, string cod_area)
        {
            string mensaje = "";
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_actualizarMedico", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_medico", cod_medico));
                    cmd.Parameters.Add(new SqlParameter("@nom_med", nom_medico));
                    cmd.Parameters.Add(new SqlParameter("@edad_med", edad_medico));
                    cmd.Parameters.Add(new SqlParameter("@sexo_med", sexo_medico));
                    cmd.Parameters.Add(new SqlParameter("@cod_area", cod_area));
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} Medico exitosamente";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cone.Close(); }
            }
                return mensaje;
            

        }


    }
}
