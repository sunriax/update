using System;
using System.Collections.Generic;
using System.Linq;
using RaGae.ArgumentLib;

namespace RaGae.UpdateLib
{
    namespace UpdateModelLib
    {
        public delegate void WriteMessage(object o);

        public abstract class UpdateModel : IDisposable
        {
            public abstract event WriteMessage UpdateMessage;

            protected Argument argument;

            // Necessary for reflector library, otherwise the class can not be found!
            protected UpdateModel() { }

            public UpdateModel(IEnumerable<string> args)
            {
                this.argument = new Argument(args.ElementAt(0), args.Skip(1));
            }

            public abstract void BeforeUpdate();
            public abstract void Update();
            public abstract void AfterUpdate();

            public abstract string Model { get; }

            public virtual void Dispose()
            {

            }
        }
    }
}
