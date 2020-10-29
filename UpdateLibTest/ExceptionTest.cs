using RaGae.UpdateLib;
using RaGae.UpdateLib.UpdateModelLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UpdateLibTest
{
    public class ExceptionTest
    {
        private const string testMessage = "parameter";

        public static IEnumerable<object[]> GetExceptionType()
        {
            yield return new object[] {
                ErrorCode.OK,
                null,
                "TILT: Should not be reached!"
            };

            yield return new object[] {
                ErrorCode.GLOBAL,
                testMessage,
                $"There was an ERROR with '{testMessage}'"
            };

            yield return new object[] {
                ErrorCode.REFLECTION,
                testMessage,
                null
            };

            yield return new object[] {
                ErrorCode.TEST,
                null,
                string.Empty
            };
        }

        [Theory]
        [MemberData(nameof(GetExceptionType))]
        public void CreateExceptionWithErrorCodeAndParameter_Passing(ErrorCode code, string parameter, string message)
        {
            BaseUpdateException ex = null;

            if (string.IsNullOrEmpty(parameter))
                ex = new UpdateException(code);
            else
                ex = new UpdateException(code, parameter);

            Assert.Equal(code, ex.ErrorCode);

            if (string.IsNullOrEmpty(parameter))
                Assert.Equal($"Exception of type 'RaGae.UpdateLib.{nameof(UpdateException)}' was thrown.", ex.Message);
            else
                Assert.Equal(parameter, ex.Message);

            if (message == null)
                Assert.Equal(parameter, ex.ErrorMessage());
            else
                Assert.Equal(message, ex.ErrorMessage());
        }
    }
}
