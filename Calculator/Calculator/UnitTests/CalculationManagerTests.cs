using Calculator.Data;
using Calculator.Managers;
using Calculator.Models;
using Moq;
using Xunit;

namespace Calculator.Tests
{
    public class CalculationManagerTests
    {
        private Mock<ICalculationRepository> m_repository;
        private Mock<Action<Exception>> m_errorHandler;
        private CalculationManager m_manager;

        public CalculationManagerTests()
        {
            m_repository = new Mock<ICalculationRepository>();
            m_errorHandler = new Mock<Action<Exception>>();
            m_manager = new CalculationManager(m_repository.Object, m_errorHandler.Object);
        }

        [Theory]
        [InlineData("5+3", "8")]
        [InlineData("20-5", "15")]
        [InlineData("8*5", "40")]
        [InlineData("5/2", "2.5")]
        public void ProcessCalculation_ValidOperation(string expression, string expected)
        {
            Calculation calc = new Calculation() { Expression = expression};
            m_manager.ProcessCalculation(calc);
            Assert.Equal(calc.Result, expected);
            m_errorHandler.Verify(x => x(It.IsAny<Exception>()), Times.Never);
        }
        [Theory]
        [InlineData("2.1+1", "3", false)]
        [InlineData("4.7+2", "7", false)]
        [InlineData("60+1.4", "61.4", true)]
        [InlineData("9.1+2", "11.1", true)]
        public void ProcessCalculation_ValidOperationRoundOnOff(string expression, string expected, bool returnDecimal)
        {
            Calculation calc = new Calculation() { Expression = expression, Decimal = returnDecimal };
            m_manager.ProcessCalculation(calc);
            Assert.Equal(calc.Result, expected);
            m_errorHandler.Verify(x => x(It.IsAny<Exception>()), Times.Never);
        }

        [Theory]
        [InlineData("2%3")]
        [InlineData("31")]
        [InlineData("2+6-3")]
        [InlineData("30++5")]
        public void ProcessCalculation_InvalidOperator(string expression)
        {
            Calculation calc = new Calculation() { Expression = expression };
            var exception = Assert.Throws<ArgumentException>(() => m_manager.ProcessCalculation(calc));
            Assert.Equal("Výraz musí mít dva operandy a jeden operátor.", exception.Message);
            m_errorHandler.Verify(x => x(It.Is<Exception>(ex => ex is ArgumentException)), Times.Once);
        }

        [Fact]
        public void ProcessCalculation_DivideByZero()
        {
            Calculation calc = new Calculation() { Expression = "8/0" };
            var exception = Assert.Throws<DivideByZeroException>(() => m_manager.ProcessCalculation(calc));
            Assert.Equal("Chyba - dělení nulou.", exception.Message);
            m_errorHandler.Verify(x => x(It.Is<Exception>(ex => ex is DivideByZeroException)), Times.Once);
        }
    }
}
