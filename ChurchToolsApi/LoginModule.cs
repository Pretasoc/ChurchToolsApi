using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchToolsApi
{
    public class HomeModule : ChurchtoolsModule
    {
        public HomeModule(ChurchToolsSession session) : base(session)
        {

        }

        protected override string Path => "churchhome";
    }
}
