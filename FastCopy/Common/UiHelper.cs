using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FastCopy.Common
{
    public static class UiHelper
    {
        public static T TryFindParent<T>(DependencyObject child) where T:DependencyObject
        {
            DependencyObject parentObject = GetParentObject(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent !=null)
            {
                return parent;
            }
            else
            {
                return TryFindParent<T>(parentObject);
            }
        }
        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null) return null;
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;
                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }
            return VisualTreeHelper.GetParent(child);
        }
        public static void UpdateBindingSources(DependencyObject obj,params DependencyProperty[] properties)
        {
            foreach(DependencyProperty depProperty in properties)
            {
                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(obj, depProperty);
                if (bindingExpression != null)
                {
                    bindingExpression.UpdateSource();
                }
            }
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for(int i = 0; i < count; i++)
            {
                DependencyObject childObject = VisualTreeHelper.GetChild(obj,i);
                UpdateBindingSources(childObject, properties);
            }
        }
        public static T TryFindFromPoint<T>(UIElement reference,Point point) where T:DependencyObject
        {
            DependencyObject element = reference.InputHitTest(point) as DependencyObject;
            if (element == null) return null;
            else if (element is T) return (T)element;
            else return TryFindParent<T>(element);
        }
    }
}
