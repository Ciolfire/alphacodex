using System;

namespace AlphaCodex.Events
{
    public class ActionEventArgs : EventArgs
    {
        private readonly string _action;

        public ActionEventArgs(string action)
        {
            _action = action;
        }

        public string Action
        {
            get { return _action; }
        }
    }
}
