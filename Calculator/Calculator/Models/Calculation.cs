using System.ComponentModel.DataAnnotations;

namespace Calculator.Models
{
    public class Calculation
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Výraz nemůže být prázdný.")]
        public string Expression { get; set; } = "";
        public string Result { get; set; } = "";
        public bool Decimal { get; set; } = true;
    }
}
