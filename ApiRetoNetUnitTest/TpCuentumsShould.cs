using ApiRetoNet.Controllers;
using ApiRetoNet.Models;
using Moq;
using Xunit;

namespace ApiRetoNetUnitTest
{
    public class TpCuentumsShould
    {
        private const string MensajeGetMessageWhenCantFindAccount = "La información de la cuenta suministrada para la búsqueda no arrojo ningún resultado.";
        private const string MensajeGetMessageWhenCantDeleteAccount = "La información suministrada para la eliminación no arrojo ningún resultado.";

        Mock<DBRetoNetContext> _mockContext { get; set; }

        public TpCuentumsShould()
        {
            _mockContext = new Mock<DBRetoNetContext>();
        }

        [Fact]
        public void GetAccountInfo()
        {
            var TpCuentums = new TpCuentumsController(_mockContext.Object);

            _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(GetCuenta());

            var Result = TpCuentums.GetTpCuentum(1033763576, 123456789);

            Assert.NotNull(Result);
            Assert.IsType<TpCuentum>(Result.Result.Value);
        }

        [Fact]
        public void GetMessageWhenCantFindAccount()
        {
            var TpCuentums = new TpCuentumsController(_mockContext.Object);

            _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(null);

            var Result = TpCuentums.GetTpCuentum(1033763576, 123456789);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.True(Response.Value == MensajeGetMessageWhenCantFindAccount);
        }

        [Fact]
        public void GetExceptionWhenSomethingFailsOnGetMethod()
        {
            var TpCuentums = new TpCuentumsController(_mockContext.Object);

            _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            var Result = TpCuentums.GetTpCuentum(1033763576, 123456789);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.True(Response.StatusCode == 500);
            Assert.IsType<ExceptionModel>(Response.Value);
        }

        [Fact]
        public void GetMessageWhenCantDeleteAccount()
        {
            var TpCuentums = new TpCuentumsController(_mockContext.Object);

            _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(null);

            var Result = TpCuentums.DeleteTpCuentum(1033763576, 123456789);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.True(Response.Value == MensajeGetMessageWhenCantDeleteAccount);
        }


        private TpCuentum GetCuenta()
        {
            return new TpCuentum
            {
                IdentificacionPersona = 1033763576,
                NumeroCuenta = 123456789,
                IdTipoCuenta = 1,
                EstadoCuenta = true,
                Saldo = 1000000
            };
        }
    }
}
