using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza
{
    public class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;
        #region конструкторы
        public RelayCommand(Action execute) => _execute = execute;
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            if (_execute != null)
                return true;

            return false;
        }

        public void Execute(object? parameter)
        {
            if (_execute != null) { _execute(); }
        }
        #endregion
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;
        #region конструкторы
        public RelayCommand(Action<T> execute) => _execute = execute;
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
            {
                T param = (T)parameter;
                return _canExecute(param);
            }
            if (_execute != null)
                return true;

            return false;
        }

        public void Execute(object? parameter)
        {
            if (_execute != null)
                _execute((T)parameter);
        }
        #endregion
    }
}
