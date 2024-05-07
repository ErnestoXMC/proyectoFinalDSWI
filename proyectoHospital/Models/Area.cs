using System.ComponentModel.DataAnnotations;

namespace proyectoHospital.Models
{
    public class Area
    {
        [Display(Name = "Código del Área")]
        public int cod_area { get; set; }
        [Display(Name = "Área")]
        public string nom_area { get; set; }
    }
}
