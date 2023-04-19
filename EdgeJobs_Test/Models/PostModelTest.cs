using System.ComponentModel.DataAnnotations;

namespace EdgeJobs_Test.Models
{
    public class PostModelTest
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")] 
        [StringLength(maximumLength: 200, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres.")]
        public string title { get; set; }
        [Required(ErrorMessage = "El campo body es obligatorio.")]
        public string body { get; set; }
        [Required(ErrorMessage = "El campo userId es obligatorio.")]
        public int userId { get; set; }

    }
}
