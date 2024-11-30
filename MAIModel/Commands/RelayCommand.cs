
using System.Windows.Input;

namespace MAIModel.Commands
{
    public class RelayCommand : ICommand
    {
        private Action _execute;
        public RelayCommand(Action execute)
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
            _execute.Invoke();
        }
    }
}
