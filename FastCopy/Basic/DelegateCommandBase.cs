using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FastCopy.Basic
{
    public abstract class DelegateCommandBase : ICommand
    {
        private readonly Action<object> _executeMethod;
        private readonly Func<object, bool> _canExecuteMethod;
        public event EventHandler CanExecuteChanged;

        public DelegateCommandBase(Action<object> executeMethod,Func<object,bool> canExecuteMethod) 
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod", "执行方法不能为null！");
            }
            this._executeMethod = executeMethod;
            this._canExecuteMethod = canExecuteMethod;
        }
        public bool CanExecute(object parameter)
        {
            return this._canExecuteMethod == null || this._canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            this._executeMethod(parameter);
        }
    }
}
