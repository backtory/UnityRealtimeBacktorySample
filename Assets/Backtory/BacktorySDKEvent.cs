using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Backtory
{
    public abstract class BacktorySDKEvent
    {
        internal static BacktorySDKEvent LogoutEvent()
        {
            return new LogoutEvent();
        }
    }

    public class LogoutEvent : BacktorySDKEvent { }
    
}
