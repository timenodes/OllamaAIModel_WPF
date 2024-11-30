using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAIModel.Commands
{
    internal class SwitchToViewCommand : ICommand
    {
        private Action<object> _execute;
        public SwitchToViewCommand(Action<object> execute)
        {
            _execute = execute;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return CanExecuteChanged != null;
        }

        public void Execute(object? parameter)
        {
            _execute.Invoke(parameter);
        }
    }
}
