using System;
using System.Threading.Tasks;
using api.Services;
using Xunit;

namespace apitests
{
    public class ConversionServiceTests
    {
        private IConversionService _sut;

        public ConversionServiceTests()
        {
            _sut = new ConversionService();
        }

        [Fact]
        public async Task
            Given_Account_And_ExchangeRate_When_Converting_A_Service_Then_It_Should_Convert_On_Found_Exchange_Rate()
        {
            var actual = await _sut.GetConvertedAccount("DKK");

            Assert.NotNull(actual);
            Assert.Equivalent("DKK", actual.Currency);
            Assert.Equivalent(110, actual.Balance);
        }

        [Fact]
        public async Task
            Given_Account_And_ExchangeRate_When_Converting_A_Service_With_Not_Found_Currency_It_Should_Throw()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetConvertedAccount("NOT_VALID"));
        }
    }
}