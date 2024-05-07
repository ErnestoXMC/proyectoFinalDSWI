using proyectoHospital.Models;

namespace proyectoHospital.Repositorio.Contrato
{
    public interface IMedico
    {
        IEnumerable<Medico> listadoMedicos();
        public Medico buscar(int id);
        public string ingresarMedico(Medico med);
        public string eliminarMedico(int cod_medico);
        public string actualizarMedico(int cod_medico, string nom_medico, int edad_medico, string sexo_medico, string cod_area);


    }
}
