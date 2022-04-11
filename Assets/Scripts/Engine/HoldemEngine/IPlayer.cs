using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemEngine
{
    public interface IPlayer
    {
        void GetAction(HandHistory history, out Action.ActionTypes action, out double amount);
    }
}
