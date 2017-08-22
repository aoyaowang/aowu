using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeAtk.User
{
    class SubUser : BaseUser
    {
        private static SubUser _Instance = null;

        public static SubUser Instance
        {
            get {
                if (_Instance == null) {
                    _Instance = new SubUser();
                }
                return _Instance;
            }
        }

    }
}
