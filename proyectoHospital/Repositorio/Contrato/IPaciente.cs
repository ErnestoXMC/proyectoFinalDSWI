using proyectoHospital.Models;
namespace proyectoHospital.Repositorio.Contrato
{
    public interface IPaciente
    {
        IEnumerable<Paciente> listadoPacientes();
        public Paciente buscar(int id);
        public string ingresarPaciente(Paciente pac);
        public string eliminarPaciente(int cod_paciente);
        public string actualizarPaciente(int cod_pac, string nom_pac, string ape_pac, int edad_pac, int dni,
                    string sexo_pac, DateTime fec_nac, int contac_emer, string cod_med, string cod_area);


    }
}
