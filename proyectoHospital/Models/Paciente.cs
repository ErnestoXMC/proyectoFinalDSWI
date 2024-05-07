using System.ComponentModel.DataAnnotations;

namespace proyectoHospital.Models
{
    public class Paciente
    {
        [Display(Name = "Código Del Paciente")]
        public int cod_paciente { get; set; }
        [Display(Name = "Nombres")]
        public string nom_pac { get; set; }
        [Display(Name = "Apellidos")]
        public string ape_pac { get; set; }
        [Display(Name = "Edad")]
        public int edad_pac { get; set; }
        [Display(Name = "DNI")]
        public int dni { get; set; }
        [Display(Name = "Sexo")]
        public string sexo_pac { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime fec_nac { get; set; }
        [Display(Name = "Contacto de Emergencia")]
        public int contac_emer { get; set; }

        [Display(Name = "Medico")]
        public string cod_medico { get; set; }

        [Display(Name = "Área")]
        public string cod_area { get; set; }

    }
}
