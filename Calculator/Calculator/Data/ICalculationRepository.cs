using Calculator.Models;

namespace Calculator.Data
{
    public interface ICalculationRepository
    {
        public void Add(Calculation calculation);
        public IEnumerable<Calculation> GetAll();
        public IEnumerable<Calculation> GetLastN(int n);
    }
}
