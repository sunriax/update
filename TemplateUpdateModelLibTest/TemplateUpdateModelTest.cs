using RaGae.UpdateLib.TemplateUpdateModelLib;
using RaGae.UpdateLib.TemplateUpdateModelLib.Resource;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using ArgumentException = RaGae.ArgumentLib.ArgumentException;

namespace TemplateUpdateModelLibTest
{
    public class TemplateUpdateModelTest
    {
        public static readonly List<string> testArguments = new List<string>()
        {
            "-b",
            "-s",
            "Text 1",
            "-i",
            "1234",
            "-d",
            Convert.ToDouble("1234,1234", CultureInfo.CreateSpecificCulture("de-AT")).ToString(),
        };

        public static readonly List<string> extendedArguments = new List<string>()
        {
            "-b2",
            "-s2",
            "Text 2",
            "-i2",
            "4321",
            "-d2",
            Convert.ToDouble("4321,4321", CultureInfo.CreateSpecificCulture("de-AT")).ToString(),
        };

        public static IEnumerable<object[]> GetArguments()
        {
            yield return new object[] {
                "TemplateUpdateModelLib.json",
                testArguments
            };

            yield return new object[] {
                "TemplateUpdateModelLib.NoArguments.json",
                new List<string>()
            };
        }

        [Theory]
        [MemberData(nameof(GetArguments))]
        public void CreateReference_Passing(string file, IEnumerable<string> arguments)
        {
            List<string> a = new List<string>()
            {
                file
            };

            arguments.ToList().ForEach(e => a.Add(e));

            TemplateUpdateModel u = null;

            if (a.Count() == 1)
                u = new TemplateUpdateModel();
            else
                u = new TemplateUpdateModel(a);

            Assert.NotNull(u);
        }
        public static IEnumerable<object[]> GetWrongArguments()
        {
            yield return new object[] {
                "Wrong.json",
                testArguments,
                RaGae.ArgumentLib.MarshalerLib.ErrorCode.GLOBAL,
                $"There was an ERROR with 'Config <Wrong.json> not found!'"
            };

            yield return new object[] {
                "TemplateUpdateModelLib.json",
                testArguments.Take(5),
                RaGae.ArgumentLib.MarshalerLib.ErrorCode.MISSING,
                "Missing parameter: ('d, double')"
            };

            yield return new object[] {
                "TemplateUpdateModelLib.WrongPath.json",
                testArguments,
                RaGae.ArgumentLib.MarshalerLib.ErrorCode.REFLECTION,
                "Directory <Wrong> not found!"
            };

            yield return new object[] {
                "TemplateUpdateModelLib.WrongFile.json",
                testArguments,
                RaGae.ArgumentLib.MarshalerLib.ErrorCode.REFLECTION,
                "Directory <Marshaler/*WrongLib.dll> contains no assemblies!"
            };
        }

        [Theory]
        [MemberData(nameof(GetWrongArguments))]
        public void CreateReference_Failing(string file, IEnumerable<string> arguments, RaGae.ArgumentLib.MarshalerLib.ErrorCode errorCode, string message)
        {
            List<string> a = new List<string>()
            {
                file
            };

            arguments.ToList().ForEach(e => a.Add(e));

            TemplateUpdateModel u = null;

            ArgumentException ex = Assert.Throws<ArgumentException>(() => u = new TemplateUpdateModel(a));

            Assert.Null(u);
            Assert.Equal(errorCode, ex.ErrorCode);
            Assert.Equal(message, ex.Message);
            Assert.Equal(message, ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndGetModel_Passing()
        {
            List<string> a = new List<string>()
            {
                "TemplateUpdateModelLib.json"
            };

            testArguments.ForEach(e => a.Add(e));

            TemplateUpdateModel u = new TemplateUpdateModel(a);

            Assert.NotNull(u);
            Assert.Equal("Template".ToLower(), u.Model);
        }

        [Fact]
        public void CreateReferenceAndExecuteBeforeUpdate_Passing()
        {
            List<object> e = new List<object>();
            List<string> a = new List<string>()
            {
                "TemplateUpdateModelLib.json"
            };

            testArguments.ForEach(e => a.Add(e));

            TemplateUpdateModel u = new TemplateUpdateModel(a);

            u.UpdateMessage += delegate (object o)
            {
                e.Add(o);
            };

            u.BeforeUpdate();

            Assert.NotNull(u);
            Assert.True(e.Count == 1);
            Assert.Equal(TemplateResource.BeforeUpdate, e.ElementAt(0));
        }

        public static IEnumerable<object[]> GetArgumentsAndMessages()
        {
            List<string> a = new List<string>()
            {
                "TemplateUpdateModelLib.json"
            };
            testArguments.ForEach(e => a.Add(e));
            extendedArguments.ForEach(e => a.Add(e));

            yield return new object[] {
                new List<string>(),
                new List<object>()
                {
                    TemplateResource.Update
                }
            };

            yield return new object[] {
                a.Take(8),
                new List<object>()
                {
                    TemplateResource.Update,
                    $"{TemplateResource.BoolValue} {true.ToString()}",
                    $"{TemplateResource.BoolValue2} {false.ToString()}",
                    $"{TemplateResource.StringValue} {a.ElementAt(3)}",
                    $"{TemplateResource.StringValue} {string.Empty}",
                    $"{TemplateResource.IntValue} {a.ElementAt(5)}",
                    $"{TemplateResource.IntValue} 0",
                    $"{TemplateResource.DoubleValue} {a.ElementAt(7)}",
                    $"{TemplateResource.DoubleValue} 0"
                }
            };

            yield return new object[] {
                a,
                new List<object>()
                {
                    TemplateResource.Update,
                    $"{TemplateResource.BoolValue} {true.ToString()}",
                    $"{TemplateResource.BoolValue2} {true.ToString()}",
                    $"{TemplateResource.StringValue} {a.ElementAt(3)}",
                    $"{TemplateResource.StringValue} {a.ElementAt(10)}",
                    $"{TemplateResource.IntValue} {a.ElementAt(5)}",
                    $"{TemplateResource.IntValue} {a.ElementAt(12)}",
                    $"{TemplateResource.DoubleValue} {a.ElementAt(7)}",
                    $"{TemplateResource.DoubleValue} {a.ElementAt(14)}"
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetArgumentsAndMessages))]
        public void CreateReferenceAndExecuteUpdate_Passing(IEnumerable<string> arguments, IEnumerable<object> message)
        {
            List<object> e = new List<object>();

            TemplateUpdateModel u = null;

            if (arguments.Count() == 0)
                u = new TemplateUpdateModel();
            else
                u = new TemplateUpdateModel(arguments);

            u.UpdateMessage += delegate (object o)
            {
                e.Add(o);
            };

            u.Update();

            Assert.NotNull(u);
            Assert.True(e.SequenceEqual(message));
        }

        [Fact]
        public void CreateReferenceAndExecuteAfterUpdate_Passing()
        {
            List<object> e = new List<object>();
            List<string> a = new List<string>()
            {
                "TemplateUpdateModelLib.json"
            };

            testArguments.ForEach(e => a.Add(e));

            TemplateUpdateModel u = new TemplateUpdateModel(a);

            u.UpdateMessage += delegate (object o)
            {
                e.Add(o);
            };

            u.AfterUpdate();

            Assert.NotNull(u);
            Assert.True(e.Count == 1);
            Assert.Equal(TemplateResource.AfterUpdate, e.ElementAt(0));
        }
    }
}
