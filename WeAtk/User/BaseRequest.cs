using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeAtk.User
{
    class BaseRequest
    {
        public int type = 0; // 0:上分 1：下分
        public float num; //数量
        public Player player = null; //操作的玩家
    }
}
