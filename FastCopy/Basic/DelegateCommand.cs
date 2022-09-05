using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Basic
{
    public class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod):this(executeMethod,()=>true)
        {

        }
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : base(
            (object o) => executeMethod(), 
            (object o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null) 
            {
                throw new ArgumentNullException("executeMethod", "执行方法不能为空！");
            }
        }
        public bool CanExecute()
        {
            return base.CanExecute(null);
        }
        public void Execute()
        {
            base.Execute(null);
        }
    }
    public class DelegateCommand<T> : DelegateCommandBase
    {
        public readonly Action<object> _executeMethod;
        public readonly Func<object, bool> _canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod) : this(executeMethod, (T o) => true)
        {

        }
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : base(
            (object o) => executeMethod((T)((object)o)),
            (object o) => canExecuteMethod((T)((object)o)))
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod", "执行方法不能为空！");
            }
            Type typeFromHandle = typeof(T);
            if (typeFromHandle.IsValueType && (!typeFromHandle.IsGenericType || !typeof(Nullable<>).IsAssignableFrom(typeFromHandle.GetGenericTypeDefinition())))
            {
                throw new InvalidCastException("无效通用类型");
            }
        }
        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }
        public void Execute(T parameter)
        {
            base.Execute(parameter);
        }
    }
}
