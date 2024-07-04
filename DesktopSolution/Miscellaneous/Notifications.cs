using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSolution.Miscellaneous
{
    public static class Notifications
    {
        public static event Action UsersChanged;

        public static void CallUsersChanged()
            => UsersChanged?.Invoke();
    }
}
