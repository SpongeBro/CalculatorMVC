using Calculator.Models;

namespace Calculator.Data
{
    public class CalculationRepository : ICalculationRepository
    {
        private AppDbContext m_context;

        public CalculationRepository(AppDbContext context)
        {
            m_context = context;
        }
        public void Add(Calculation calculation)
        {
            m_context.Add(calculation);
            m_context.SaveChanges();
        }
        public IEnumerable<Calculation> GetAll()
        {
            return m_context.Calculations;
        }
        public IEnumerable<Calculation> GetLastN(int n)
        {
            return m_context.Calculations.ToList().TakeLast(n);
        }
    }
}
