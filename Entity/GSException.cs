using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class GSException : Exception
    {
        #region Variables
        ErrorCode errorCode;
        string message;
        #endregion

        #region Constructors
        public GSException()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_errorCode"></param>
        /// <param name="_message"></param>
        public GSException(ErrorCode _errorCode, string _message)
        {
            errorCode = _errorCode;
            message = _message;
        }
        #endregion

        #region Properties
        public override string Message
        {
            get { return message; }
        }
        public ErrorCode ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        #endregion
    }
}
