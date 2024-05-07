using proyectoHospital.Models;

namespace proyectoHospital.Repositorio.Contrato
{
    public interface IArea
    {
        IEnumerable<Area> listadoAreas();
        string ingresarArea(Area ar);
        public Area buscar(int id);
        public string eliminarArea(int cod_area);
        public string actualizarArea(int cod_area, string nom_area);


    }
}
