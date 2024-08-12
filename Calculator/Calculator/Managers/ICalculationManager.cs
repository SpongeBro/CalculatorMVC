using Calculator.Models;

namespace Calculator.Managers
{
    public interface ICalculationManager
    {
        public void ProcessCalculation(Calculation calculation);
        public void InsertToDb(Calculation calculation);
        public List<string> HistoryOfLastN(int n);
    }
}
