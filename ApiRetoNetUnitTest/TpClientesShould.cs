using ApiRetoNet.Controllers;
using ApiRetoNet.Models;
using Moq;
using Xunit;

namespace ApiRetoNetUnitTest
{
    public class TpClientesShould
    {
        private const string MensajeGetMessageWhenCantFindClient = "La identificación suministrada para la búsqueda no arrojo ningún resultado.";
        private const string MensajeGetMessageWhenCantDeleteClient = "La identificación suministrada para la eliminación no arrojo ningún resultado.";

        Mock<DBRetoNetContext> _mockContext { get; set; }

        public TpClientesShould()
        {
            _mockContext = new Mock<DBRetoNetContext>();
        }

        [Fact]
        public void GetClientInfo() {

            var TpClientes = new TpClientesController(_mockContext.Object);
            
            _mockContext.Setup(x => x.TpClientes.FindAsync(It.IsAny<int>())).ReturnsAsync(GetClient());
            _mockContext.Setup(x => x.TpPersonas.FindAsync(It.IsAny<int>())).ReturnsAsync(GetPersona());

            var Result = TpClientes.GetTpCliente(1033763576);

            Assert.NotNull(Result);
            Assert.IsType<TpCliente>(Result.Result.Value);
        }

        [Fact]
        public void GetExceptionWhenSomethingFailsOnGetMethod()
        {
            var TpClientes = new TpClientesController(_mockContext.Object);

            _mockContext.Setup(x => x.TpClientes.FindAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var Result = TpClientes.GetTpCliente(1033763576);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.True(Response.StatusCode == 500);
            Assert.IsType<ExceptionModel>(Response.Value);
        }

        [Fact]
        public void GetMessageWhenCantFindClient()
        {
            var TpClientes = new TpClientesController(_mockContext.Object);

            _mockContext.Setup(x => x.TpClientes.FindAsync(It.IsAny<int>())).Returns(null);
            _mockContext.Setup(x => x.TpPersonas.FindAsync(It.IsAny<int>())).Returns(null);

            var Result = TpClientes.GetTpCliente(1033763576);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.True(Response.Value == MensajeGetMessageWhenCantFindClient);
        }

        [Fact]
        public void GetMessageWhenCantDeleteClient()
        {
            var TpClientes = new TpClientesController(_mockContext.Object);

            _mockContext.Setup(x => x.TpClientes.FindAsync(It.IsAny<int>())).Returns(null);
            _mockContext.Setup(x => x.TpPersonas.FindAsync(It.IsAny<int>())).Returns(null);

            var Result = TpClientes.DeleteTpCliente(1033763576);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.True(Response.Value == MensajeGetMessageWhenCantDeleteClient);
        }

        private TpCliente GetClient()
        {
            return new TpCliente
            {
                IdentificacionPersona = 1033763576,
                EstadoCliente = true,
                Contraseña = "123456",
                IdentificacionPersonaNavigation = GetPersona()
            };
        }

        private TpPersona GetPersona()
        {
            return new TpPersona
            {
                Identificacion = 1033763576,
                IdGenero = 1,
                Edad = 30,
                Nombre = "Carlos Andres Jimenez Mora",
                Direccion = "Calle Falsa 123",
                Telefono = "3114329673"
            };
        }
    }
}
