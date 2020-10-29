using RaGae.UpdateLib.UpdateModelLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaGae.UpdateLib
{
    public class UpdateException : BaseUpdateException
    {
        public UpdateException(ErrorCode errorCode) : base(errorCode) { }

        public UpdateException(ErrorCode errorCode, string errorMessage) : base(errorCode, errorMessage) { }

        public override string ErrorMessage()
        {
            switch (ErrorCode)
            {
                case ErrorCode.OK:
                    return "TILT: Should not be reached!";
                case ErrorCode.GLOBAL:
                    return $"There was an ERROR with '{base.Message}'";
                case ErrorCode.REFLECTION:
                    return base.Message;
                default:
                    return string.Empty;
            }
        }
    }
}
