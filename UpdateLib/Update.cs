using RaGae.BootstrapLib.Loader;
using RaGae.ExceptionLib;
using RaGae.ReflectionLib;
using RaGae.UpdateLib.Resource;
using RaGae.UpdateLib.UpdateModelLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErrorCode = RaGae.UpdateLib.UpdateModelLib.ErrorCode;

namespace RaGae.UpdateLib
{
    public class Update
    {
        public event WriteMessage UpdateMessage;

        private IEnumerable<string> arguments;
        private Reflection reflection;
        private UpdateConfig updateConfig;

        public Update(IEnumerable<string> args)
        {
            this.arguments = args;

            LoadConfig();
            LoadLibraries();
        }

        private void LoadConfig()
        {
            try
            {
                this.updateConfig = Loader.LoadConfigSection<UpdateConfig>(this.arguments.ElementAt(1));
            }
            catch (Exception ex)
            {
                if (ex.InnerException is Exception)
                    throw new UpdateException(ErrorCode.GLOBAL, ex.InnerException.Message);

                throw new UpdateException(ErrorCode.GLOBAL, string.Format(UpdateResource.ConfigNotFound, this.arguments.ElementAt(1)));
            }
        }

        private void LoadLibraries()
        {
            try
            {
                this.reflection = new Reflection(this.arguments.ElementAt(0), 0);
            }
            catch (ReflectionException ex)
            {
                throw new UpdateException(ErrorCode.REFLECTION, ex.ErrorMessage());
            }
        }

        public void ExecuteUpdate()
        {
            this.UpdateMessage?.Invoke(UpdateResource.LoadModel);

            try
            {
                using (UpdateModel updateModel = reflection.GetInstanceByProperty<UpdateModel>(nameof(updateModel.Model), updateConfig.Model.ToLower(), this.arguments.Count() == 2 ? null : new object[] { this.arguments.Skip(1) }))
                {
                    updateModel.UpdateMessage += this.UpdateMessage;

                    if (!updateConfig.SkipBeforeUpdate)
                        updateModel.BeforeUpdate();
                    else
                        this.UpdateMessage(UpdateResource.SkipBeforeUpdate);

                    if (!updateConfig.SkipUpdate)
                        updateModel.Update();
                    else
                        this.UpdateMessage(UpdateResource.SkipUpdate);

                    if (!updateConfig.SkipAfterUpdate)
                        updateModel.AfterUpdate();
                    else
                        this.UpdateMessage(UpdateResource.SkipAfterUpdate);

                    updateModel.UpdateMessage -= this.UpdateMessage;
                }
            }
            catch (ReflectionException ex)
            {
                throw new UpdateException(ErrorCode.REFLECTION, ex.ErrorMessage());
            }
        }
    }
}
