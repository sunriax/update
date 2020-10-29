using RaGae.UpdateLib.TemplateUpdateModelLib.Resource;
using RaGae.UpdateLib.UpdateModelLib;
using System;
using System.Collections.Generic;

namespace RaGae.UpdateLib.TemplateUpdateModelLib
{
    public class TemplateUpdateModel : UpdateModel
    {
        public override event WriteMessage UpdateMessage;

        private const string model = "Template";
        public override string Model { get => model.ToLower(); }

        private readonly bool emptyConstructor;
        private readonly TemplateUpdateConfig config;

        // Necessary for reflector library, otherwise the class can not be found!
        // If the constructor is not used it can be private
        // If no arguments are passed to the model use this constructor
        public TemplateUpdateModel()
        {
            this.emptyConstructor = true;
        }

        // If no arguments are passed to the model the constructor can be removed!
        public TemplateUpdateModel(IEnumerable<string> args) : base(args)
        {
            this.config = new TemplateUpdateConfig()
            {
                BoolValue = base.argument.GetValue<bool>("b"),
                BoolValue2 = base.argument.GetValue<bool>("b2"),
                StringValue = base.argument.GetValue<string>("s"),
                StringValue2 = base.argument.GetValue<string>("s2"),
                IntValue = base.argument.GetValue<int>("i"),
                IntValue2 = base.argument.GetValue<int>("i2"),
                DoubleValue = base.argument.GetValue<double>("d"),
                DoubleValue2 = base.argument.GetValue<double>("d2"),
            };
        }

        public override void BeforeUpdate()
        {
            this.UpdateMessage?.Invoke(TemplateResource.BeforeUpdate);
        }

        public override void Update()
        {
            this.UpdateMessage?.Invoke(TemplateResource.Update);
            if (!emptyConstructor)
            {
                this.UpdateMessage?.Invoke($"{TemplateResource.BoolValue} {this.config.BoolValue}");
                this.UpdateMessage?.Invoke($"{TemplateResource.BoolValue2} {this.config.BoolValue2}");
                this.UpdateMessage?.Invoke($"{TemplateResource.StringValue} {this.config.StringValue}");
                this.UpdateMessage?.Invoke($"{TemplateResource.StringValue} {this.config.StringValue2}");
                this.UpdateMessage?.Invoke($"{TemplateResource.IntValue} {this.config.IntValue}");
                this.UpdateMessage?.Invoke($"{TemplateResource.IntValue} {this.config.IntValue2}");
                this.UpdateMessage?.Invoke($"{TemplateResource.DoubleValue} {this.config.DoubleValue}");
                this.UpdateMessage?.Invoke($"{TemplateResource.DoubleValue} {this.config.DoubleValue2}");
            }
        }

        public override void AfterUpdate()
        {
            this.UpdateMessage?.Invoke(TemplateResource.AfterUpdate);
        }

        internal class TemplateUpdateConfig
        {
            public bool BoolValue { get; set; }
            public bool BoolValue2 { get; set; }
            public string StringValue { get; set; }
            public string StringValue2 { get; set; }
            public int IntValue { get; set; }
            public int IntValue2 { get; set; }
            public double DoubleValue { get; set; }
            public double DoubleValue2 { get; set; }
        }
    }
}