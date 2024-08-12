using Calculator.Data;
using Calculator.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Calculator.Managers
{
    public class CalculationManager : ICalculationManager
    {
        private ICalculationRepository m_repository;
        private Action<Exception> m_errorHandler;
        public CalculationManager(ICalculationRepository repository, Action<Exception> errorHandler)
        {
            m_repository = repository;
            m_errorHandler = errorHandler;
        }
        public void InsertToDb(Calculation calculation)
        {
            // add spaces before/after operation
            calculation.Expression = Regex.Replace(calculation.Expression, @"([\+\-\*\/])", " $1 ");
            m_repository.Add(calculation);
        }
        public void ProcessCalculation(Calculation calculation)
        {
            try {
                calculation.Result = EvaluateExpression(calculation.Expression, calculation.Decimal);
            }
            catch (Exception ex) {
                m_errorHandler?.Invoke(ex);
                throw;
            }
        }
        public List<string> HistoryOfLastN(int n)
        {
            List<string> history = new List<string>();
            foreach (var row in m_repository.GetLastN(n)) {
                history.Add(string.Format("{0} = {1}", row.Expression, row.Result));
            }
            return history;
        }

        private string EvaluateExpression(string expression, bool returnDecimal)
        {
            if (string.IsNullOrEmpty(expression)) {
                throw new ArgumentException("Výraz nemůže být prázdný.");
            }
            Regex regex = new Regex(@"^(-?\d+\.\d+|-?\d+)([\+\-\*\/])(\d+\.\d+|\d+)$");
            var match = regex.Match(expression.Replace(',', '.'));

            if (!match.Success) {
                throw new ArgumentException("Výraz musí mít dva operandy a jeden operátor.");
            }

            string strOp1 = match.Groups[1].Value;
            string strOp2 = match.Groups[3].Value;
            string strOperation = match.Groups[2].Value;

            double operand1 = double.Parse(strOp1, System.Globalization.CultureInfo.InvariantCulture);
            double operand2 = double.Parse(strOp2, System.Globalization.CultureInfo.InvariantCulture);
            double result = 0;

            switch (strOperation[0]) {
                case '+':
                    result = operand1 + operand2;
                    break;
                case '-':
                    result = operand1 - operand2;
                    break;
                case '*':
                    result = operand1 * operand2;
                    break;
                case '/':
                    if (operand2 == 0) {
                        throw new DivideByZeroException("Chyba - dělení nulou.");
                    }
                    result = operand1 / operand2;
                    break;
                default: 
                    throw new ArgumentException("Chybný operátor.");
            }
            return Math.Round(result, returnDecimal ? 3 : 0).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}