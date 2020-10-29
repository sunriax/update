using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Globalization;
using RaGae.UpdateLib;
using RaGae.UpdateLib.TemplateUpdateModelLib.Resource;
using RaGae.UpdateLib.Resource;
using RaGae.UpdateLib.UpdateModelLib;

namespace UpdateLibTest
{
    public class UpdateTest
    {
        public static List<string> testArguments = new List<string>()
        {
            "UpdateLib.json",
            "TemplateUpdateModelLib.json",
            "-b",
            "-s",
            "Text 1",
            "-i",
            "1234",
            "-d",
            Convert.ToDouble("1234,1234", CultureInfo.CreateSpecificCulture("de-AT")).ToString(),
        };

        public static IEnumerable<object[]> GetArguments()
        {
            yield return new object[] {
                "TemplateUpdateModelLib.json",
                testArguments.Skip(2)
            };

            yield return new object[] {
                "TemplateUpdateModelLib.NoArguments.json",
                new List<string>()
            };
        }

        [Theory]
        [MemberData(nameof(GetArguments))]
        public void CreateReferenceWithTemplateUpdateModelEmptyNoArguments_Passing(string file, IEnumerable<string> arguments)
        {
            List<string> a = new List<string>();

            a.Add("UpdateLib.json");
            a.Add(file);

            arguments.ToList().ForEach(e => a.Add(e));

            Update u = new Update(a);
            Assert.NotNull(u);
        }

        public static IEnumerable<object[]> GetWrongConfig()
        {
            yield return new object[] {
                "UpdateLib.json",
                "NotFound.json",
                ErrorCode.GLOBAL,
                "Config <NotFound.json> not found!",
                "There was an ERROR with 'Config <NotFound.json> not found!'"
            };

            yield return new object[] {
                "UpdateLib.json",
                "UpdateModelLib.EmptyModel.json",
                ErrorCode.GLOBAL,
                $"Value cannot be null. (Parameter '{UpdateResource.ExceptionEmptyModel}')",
                $"There was an ERROR with 'Value cannot be null. (Parameter '{UpdateResource.ExceptionEmptyModel}')'"
            };

            yield return new object[] {
                "UpdateLib.WrongPath.json",
                "TemplateUpdateModelLib.json",
                ErrorCode.REFLECTION,
                "Directory <Wrong> not found!",
                "Directory <Wrong> not found!"
            };

            yield return new object[] {
                "UpdateLib.WrongSpecifier.json",
                "TemplateUpdateModelLib.json",
                ErrorCode.REFLECTION,
                "Directory <Model/*WrongModelLib.dll> contains no assemblies!",
                "Directory <Model/*WrongModelLib.dll> contains no assemblies!"
            };
        }

        [Theory]
        [MemberData(nameof(GetWrongConfig))]
        public void CreateReferenceWithTemplateUpdateModel_Failing(string updateConfig, string modelConfig, ErrorCode errorCode, string message, string errorMessage)
        {
            Update u = null;

            List<string> failArguments = new List<string>();

            failArguments.Add(updateConfig);
            failArguments.Add(modelConfig);

            testArguments.Skip(2).ToList().ForEach(e => failArguments.Add(e));

            BaseUpdateException ex = Assert.Throws<UpdateException>(() => u = new Update(failArguments));

            Assert.Null(u);
            Assert.Equal(errorCode, ex.ErrorCode);
            Assert.Equal(message, ex.Message);
            Assert.Equal(errorMessage, ex.ErrorMessage());
        }

        public static IEnumerable<object[]> GetModelData()
        {
            List<string> message = new List<string>()
                {
                    UpdateResource.LoadModel,
                    TemplateResource.BeforeUpdate,
                    UpdateResource.SkipBeforeUpdate,
                    TemplateResource.Update,
                    UpdateResource.SkipUpdate,
                    $"{TemplateResource.BoolValue} {true.ToString()}",
                    $"{TemplateResource.BoolValue2} {false.ToString()}",
                    $"{TemplateResource.StringValue} {testArguments.ElementAt(4)}",
                    $"{TemplateResource.StringValue} {string.Empty}",
                    $"{TemplateResource.IntValue} {testArguments.ElementAt(6)}",
                    $"{TemplateResource.IntValue} 0",
                    $"{TemplateResource.DoubleValue} {testArguments.ElementAt(8)}",
                    $"{TemplateResource.DoubleValue} 0",
                    TemplateResource.AfterUpdate,
                    UpdateResource.SkipAfterUpdate
                };

            yield return new object[] {
                "UpdateLib.json",
                "TemplateUpdateModelLib.json",
                testArguments.Skip(2),
                message.Where(e => e != UpdateResource.SkipBeforeUpdate && e != UpdateResource.SkipUpdate && e != UpdateResource.SkipAfterUpdate)
            };

            yield return new object[] {
                "UpdateLib.json",
                "TemplateUpdateModelLib.NoArguments.json",
                new List<string>(),
                message.Where(e => e == UpdateResource.LoadModel ||e == TemplateResource.BeforeUpdate || e == TemplateResource.Update || e == TemplateResource.AfterUpdate)
            };

            yield return new object[] {
                "UpdateLib.json",
                "TemplateUpdateModelLib.SkipBeforeUpdate.json",
                testArguments.Skip(2),
                message.Where(e => e != TemplateResource.BeforeUpdate && e != UpdateResource.SkipUpdate && e != UpdateResource.SkipAfterUpdate)
            };

            yield return new object[] {
                "UpdateLib.json",
                "TemplateUpdateModelLib.SkipAfterUpdate.json",
                testArguments.Skip(2),
                message.Where(e => e != UpdateResource.SkipBeforeUpdate && e != UpdateResource.SkipUpdate && e != TemplateResource.AfterUpdate)
            };

            List<string> temp = new List<string>();
            
            message.ForEach(e => temp.Add(e));
            temp.RemoveRange(5, 8);

            yield return new object[] {
                "UpdateLib.json",
                "TemplateUpdateModelLib.SkipUpdate.json",
                testArguments.Skip(2),
                temp.Where(e => e != UpdateResource.SkipBeforeUpdate && e != TemplateResource.Update && e != UpdateResource.SkipAfterUpdate)
            };
        }

        [Theory]
        [MemberData(nameof(GetModelData))]
        public void CreateReferenceWithTemplateUpdateModelAndExecuteUpdate_Passing(string updateConfig, string modelConfig, IEnumerable<string> arguments,  IEnumerable<string> message)
        {
            List<string> a = new List<string>();
            List<object> e = new List<object>();

            a.Add(updateConfig);
            a.Add(modelConfig);

            arguments.ToList().ForEach(e => a.Add(e));

            Update u = new Update(a);

            u.UpdateMessage += delegate (object o)
            {
                e.Add(o);
            };

            u.ExecuteUpdate();

            Assert.True(e.SequenceEqual(message.ToList()));
        }

        [Fact]
        public void CreateReferenceWithTemplateUpdateModelAndExecuteUpdate_Failing()
        {
            List<string> a = new List<string>();
            List<object> e = new List<object>();

            a.Add("UpdateLib.json");
            a.Add("UpdateModelLib.WrongModel.json");

            testArguments.Skip(2).ToList().ForEach(e => a.Add(e));

            Update u = new Update(a);

            u.UpdateMessage += delegate (object o)
            {
                e.Add(o);
            };

            BaseUpdateException ex = Assert.Throws<UpdateException>(() => u.ExecuteUpdate());

            Assert.NotNull(u);
            Assert.Equal(ErrorCode.REFLECTION, ex.ErrorCode);
            Assert.Equal("PropertyName <Model:wrong> not found!", ex.Message);
            Assert.Equal("PropertyName <Model:wrong> not found!", ex.ErrorMessage());
            Assert.Equal(UpdateResource.LoadModel, e.ElementAt(0));
        }
    }
}
