using Microsoft.EntityFrameworkCore;
using proyectoHospital.Models;

namespace proyectoHospital.Repositorio.Contrato
{
    public interface IUsuario
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario modelo);
    }
}
