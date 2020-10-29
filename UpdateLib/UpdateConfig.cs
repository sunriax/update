using RaGae.UpdateLib.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaGae.UpdateLib
{
    public class UpdateConfig
    {
        private string model;

        public string Model
        {
            get => this.model;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(UpdateResource.ExceptionEmptyModel);

                this.model = value;
            }
        }

        public bool SkipBeforeUpdate { get; set; }
        public bool SkipUpdate { get; set; }
        public bool SkipAfterUpdate { get; set; }

    }
}
