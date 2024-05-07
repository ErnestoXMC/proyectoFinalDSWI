using proyectoHospital.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using proyectoHospital.Repositorio.Contrato;

namespace proyectoHospital.Repositorio.Implementacion
{
    public class PacienteRepositorio : IPaciente
    {
    
    private readonly string cadena;
    public PacienteRepositorio()
    {
        cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cadenaSQL");
    }
    public IEnumerable<Paciente> listadoPacientes()
    {
        List<Paciente> listar = new List<Paciente>();
        using (SqlConnection cone = new SqlConnection(cadena))
        {
            cone.Open();
            SqlCommand cmd = new SqlCommand("exec sp_listarPacientes", cone);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listar.Add(new Paciente()
                {
                    cod_paciente = dr.GetInt32(0),
                    nom_pac = dr.GetString(1),
                    ape_pac = dr.GetString(2),
                    edad_pac = dr.GetInt32(3),
                    dni = dr.GetInt32(4),
                    sexo_pac = dr.GetString(5),
                    fec_nac = dr.GetDateTime(6),
                    contac_emer = dr.GetInt32(7),
                    cod_medico = dr.GetString(8),
                    cod_area = dr.GetString(9)
                });
            }
            dr.Close();

        }
        return listar;

    }

        public Paciente buscar(int id)
        {
            return listadoPacientes().Where(v => v.cod_paciente == id).FirstOrDefault();
        }

        public string ingresarPaciente(Paciente pac)
        {
            string mensaje = "";
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_ingresarPacientes", cone);
                    cmd.Parameters.AddWithValue("@nom_pac", pac.nom_pac);
                    cmd.Parameters.AddWithValue("@ape_pac", pac.ape_pac);
                    cmd.Parameters.AddWithValue("@edad_pac", pac.edad_pac);
                    cmd.Parameters.AddWithValue("@dni", pac.dni);
                    cmd.Parameters.AddWithValue("@sexo_pac", pac.sexo_pac);
                    cmd.Parameters.AddWithValue("@fec_nac", pac.fec_nac);
                    cmd.Parameters.AddWithValue("@contac_emer", pac.contac_emer);
                    cmd.Parameters.AddWithValue("@cod_medico", pac.cod_medico);
                    cmd.Parameters.AddWithValue("@cod_area", pac.cod_area);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha insertado {i} Paciente exitosamente";
                }

                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cone.Close(); }
            }
            return mensaje;

        }

        public string eliminarPaciente(int cod_paciente)
        {
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminarPaciente", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_paciente", cod_paciente));

                    int i = cmd.ExecuteNonQuery(); // Ejecuta el procedimiento almacenado

                    if (i > 0)
                    {
                        return "Paciente eliminado con éxito.";
                    }
                    else
                    {
                        return "No se encontró el Paciente con el código especificado.";
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de la excepción
                    return $"Error al eliminar al Paciente: {ex.Message}";
                }
            }
        }
        public string actualizarPaciente(int cod_pac, string nom_pac, string ape_pac,int edad_pac, int dni, 
            string sexo_pac, DateTime fec_nac, int contac_emer,string cod_med, string cod_area)
        {
            string mensaje = "";
            using (SqlConnection cone = new SqlConnection(cadena))
            {
                try
                {
                    cone.Open();
                    SqlCommand cmd = new SqlCommand("sp_actualizarPaciente", cone);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_paciente", cod_pac));
                    cmd.Parameters.Add(new SqlParameter("@nom_pac", nom_pac));
                    cmd.Parameters.Add(new SqlParameter("@ape_pac", ape_pac));
                    cmd.Parameters.Add(new SqlParameter("@edad_pac", edad_pac));
                    cmd.Parameters.Add(new SqlParameter("@dni", dni));
                    cmd.Parameters.Add(new SqlParameter("@sexo_pac", sexo_pac));
                    cmd.Parameters.Add(new SqlParameter("@fec_nac", fec_nac));
                    cmd.Parameters.Add(new SqlParameter("@contac_emer", contac_emer));
                    cmd.Parameters.Add(new SqlParameter("@cod_medico", cod_med));
                    cmd.Parameters.Add(new SqlParameter("@cod_area", cod_area));
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} Paciente exitosamente";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cone.Close(); }
            }
            return mensaje;

        }


    }
}
