using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeArk
{
    class VBScriptCode
    {
        public string WHO()
        {
            dynamic obj = Activator.CreateInstance(Type.GetTypeFromCLSID(Guid.Parse("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC")), false);
            obj.Language = "vbscript";
            string vbscript = "CreateObject(\"ADSystemInfo\").UserName";
            return obj.Eval(vbscript).Substring(3, (obj.Eval(vbscript).IndexOf(",") - 3));
        }
    }
}
