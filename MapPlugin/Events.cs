using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroDrive
{
    internal class Events
    {
        public delegate void StopEventHandler(object source, EventArgs args);
        public static event StopEventHandler Stop;
        protected static void OnAIsFive()
        {
            Stop?.Invoke(null, EventArgs.Empty);
        }
    }
}
