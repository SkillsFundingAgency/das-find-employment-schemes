
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Xunit;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Models
{
    public class ErrorViewModelTests
    {
        [Theory]
        [InlineData(true, "abc")]
        [InlineData(true, "xyz")]
        [InlineData(true, "   ")]
        [InlineData(false, "")]

        public void Constructor_ShowRequestId(bool expectedShowRequestId, string requestId)
        {
            ErrorViewModel evm = new ErrorViewModel(requestId);
            Assert.Equal(expectedShowRequestId, evm.ShowRequestId);
        }
    }
}