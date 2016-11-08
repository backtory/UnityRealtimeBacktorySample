using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Backtory.core
{
    public interface IGlobalEventListener
    {
        void OnEvent(BacktorySDKEvent logoutEvent);
    }

}
