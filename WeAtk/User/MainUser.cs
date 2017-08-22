using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeAtk.User
{
    class MainUser : BaseUser
    {
        private static MainUser _Instance = null;
        public static MainUser Instance
        {
            get {
                if (_Instance == null) {
                    _Instance = new MainUser();
                }
                return _Instance;
            }
        }

    }
}
