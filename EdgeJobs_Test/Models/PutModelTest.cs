using System.ComponentModel.DataAnnotations;

namespace EdgeJobs_Test.Models
{
    public class PutModelTest
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")] //reglas de validacion por atributo
        public int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string title { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string body { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int userId { get; set; }
    }
}
