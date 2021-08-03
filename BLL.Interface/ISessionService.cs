using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface
{
    public interface ISessionService
    {
        void SetSession(string sessionKey);
        bool IsInited { get; }
    }
}
