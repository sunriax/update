using RaGae.UpdateLib;
using RaGae.UpdateLib.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Xunit;

namespace UpdateLibTest
{
    public class UpdateConfigTest
    {
        public static string testModel = "TestModel";

        public static IEnumerable<object[]> CreateTestListPassing()
        {
            yield return new object[] { testModel, null, null, null };
            yield return new object[] { testModel, null, null, true };
            yield return new object[] { testModel, null, true, null };
            yield return new object[] { testModel, null, true, true };
            yield return new object[] { testModel, true, null, null };
            yield return new object[] { testModel, true, null, true };
            yield return new object[] { testModel, true, true, null };
            yield return new object[] { testModel, false, false, false };
            yield return new object[] { testModel, false, false, true };
            yield return new object[] { testModel, false, true, false };
            yield return new object[] { testModel, false, true, true };
            yield return new object[] { testModel, true, false, false };
            yield return new object[] { testModel, true, false, true };
            yield return new object[] { testModel, true, true, false };
            yield return new object[] { testModel, true, true, true };
        }

        [Theory]
        [MemberData(nameof(CreateTestListPassing))]
        public void CreateReferenceWithProperties_Passing(string model, bool skipBeforeUpdate, bool skipUpdate, bool skipAfterUpdate)
        {
            UpdateConfig config = new UpdateConfig()
            {
                Model = model,
                SkipBeforeUpdate = skipBeforeUpdate,
                SkipUpdate = skipUpdate,
                SkipAfterUpdate = skipAfterUpdate
            };
        }
        public static IEnumerable<object[]> CreateTestListFailing()
        {
            yield return new object[] { null };
            yield return new object[] { string.Empty };
            yield return new object[] { "   " };
        }

        [Theory]
        [MemberData(nameof(CreateTestListFailing))]
        public void CreateReferenceWithProperties_Failing(string model)
        {
            UpdateConfig config = null;

            Exception ex = Assert.Throws<ArgumentNullException>(() => config = new UpdateConfig()
            {
                Model = model,
                SkipBeforeUpdate = false,
                SkipUpdate = false,
                SkipAfterUpdate = false
            });

            Assert.Null(config);
            Assert.Equal($"Value cannot be null. (Parameter '{UpdateResource.ExceptionEmptyModel}')", ex.Message);

        }


    }
}
