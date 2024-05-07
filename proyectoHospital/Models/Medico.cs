using System.ComponentModel.DataAnnotations;
namespace proyectoHospital.Models
{
    public class Medico
    {
        [Display(Name = "Código del Médico")]
        public int cod_medico { get; set; }

        [Display(Name = "Médico")]
        public string nom_med { get; set; }

        [Display(Name = "Edad")]
        public int edad_med { get; set; }

        [Display(Name = "Sexo")]
        public string sexo_med { get; set; }

        [Display(Name = "Área")]
        public string cod_area { get; set; }

    }
}
