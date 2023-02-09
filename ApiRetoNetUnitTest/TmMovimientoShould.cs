using ApiRetoNet.Controllers;
using ApiRetoNet.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ApiRetoNetUnitTest
{
    public class TmMovimientoShould
    {
        private const string MensajeAccountDoesntExists = "La informacion suministrada de la cuenta no existe";
        private const string MensajeMovementTypeDoesntExists = "El tipo de movimiento especificado no existe";

        Mock<DBRetoNetContext> _mockContext { get; set; }

        public TmMovimientoShould()
        {
            _mockContext = new Mock<DBRetoNetContext>();
        }

        [Fact]
        public void ReturnAMessageWhenTheAccountDoesntExistsOnPostTmMovimientoMethod()
        {
            var TmMovimiento = new TmMovimientoController(_mockContext.Object);

            _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(null);
            _mockContext.Setup(x => x.TpTipoMovimientos.FindAsync(It.IsAny<int>())).Returns(null);

            var Result = TmMovimiento.PostTmMovimiento(1000000, 1033763576, 123456789, 1);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.NotNull(Result);
            Assert.True(Response.Value == MensajeAccountDoesntExists);
        }

        [Fact]
        public void ReturnAMessageWhenTheMovementTypeDoesntExistsOnPostTmMovimientoMethod()
        {
            var TmMovimiento = new TmMovimientoController(_mockContext.Object);

            _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(GetCuenta());
            _mockContext.Setup(x => x.TpTipoMovimientos.FindAsync(It.IsAny<int>())).Returns(null);

            var Result = TmMovimiento.PostTmMovimiento(1000000, 1033763576, 123456789, 1);

            Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

            Assert.NotNull(Result);
            Assert.True(Response.Value == MensajeMovementTypeDoesntExists);
        }

        //[Fact]
        //public void MakeTheMovementWithSuccessResult()
        //{
        //    var TmMovimiento = new TmMovimientoController(_mockContext.Object);

        //    var Cuenta = GetCuenta();

        //    _mockContext.Setup(x => x.TpCuenta.FindAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(Cuenta);
        //    _mockContext.Setup(x => x.TpTipoMovimientos.FindAsync(It.IsAny<int>())).ReturnsAsync(GetTipoMovimiento());

        //    _mockContext.Setup(x => x.Entry(Cuenta));
            
        //    var Result = TmMovimiento.PostTmMovimiento(1000000, 1033763576, 123456789, 1);

        //    Microsoft.AspNetCore.Mvc.ObjectResult? Response = Result.Result as Microsoft.AspNetCore.Mvc.ObjectResult;

        //    Assert.NotNull(Result);
        //    Assert.True(Response.Value == MensajeMovementTypeDoesntExists);
        //}

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

        private TpTipoMovimiento GetTipoMovimiento()
        {
            return new TpTipoMovimiento
            {
                Id = 1,
                TipoMovimiento = "Ingreso"
            };
        }
    }
}
