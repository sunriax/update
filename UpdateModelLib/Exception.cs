using RaGae.ExceptionLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaGae.UpdateLib
{
    namespace UpdateModelLib
    {
        public enum ErrorCode
        {
            OK,
            GLOBAL,
            REFLECTION,
            TEST
        }

        public abstract class BaseUpdateException : BaseException<ErrorCode>
        {
            public BaseUpdateException(ErrorCode errorCode)
            {
                base.ErrorCode = errorCode;
            }

            public BaseUpdateException(ErrorCode errorCode, string errorMessage) : base(errorMessage)
            {
                base.ErrorCode = errorCode;
            }
        }
    }
}