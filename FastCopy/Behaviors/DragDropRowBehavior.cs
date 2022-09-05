using FastCopy.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FastCopy.Behaviors
{
    public static class DragDropRowBehavior
    {
        private static DataGrid m_DataGrid;
        private static Popup m_Popup;
        private static bool m_Enable;
        public static bool IsEditing { get; set; }
        public static bool IsDragging { get; set; }
        public static bool TextBoxIsEditing { get; set; }
        private static object m_DraggedItem;
        public static object DraggedItem
        {
            get
            {
                return m_DraggedItem;
            }
            set
            {
                m_DraggedItem = value;
            }
        }
        public static readonly DependencyProperty PopupControlProperty = DependencyProperty.RegisterAttached("PopupControl", typeof(Popup), typeof(DragDropRowBehavior), new UIPropertyMetadata(null, OnPopupControlChanged));

        public static Popup GetPopupControl(DependencyObject obj)
        {
            return (Popup)obj.GetValue(PopupControlProperty);
        }
        public static void SetPopupControl(DependencyObject obj, Popup value)
        {
            obj.SetValue(PopupControlProperty, value);
        }

        private static void OnPopupControlChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || !(e.NewValue is Popup))
            {
                throw new ArgumentException("Popup Control should be set", "PopupControl");
            }
            m_Popup = e.NewValue as Popup;
            if (depObject is DataGrid)
            {
                m_DataGrid = depObject as DataGrid;
                if (m_DataGrid == null)
                {
                    return;
                }
                if (m_Enable && m_Popup != null)
                {
                    m_DataGrid.BeginningEdit += new EventHandler<DataGridBeginningEditEventArgs>(OnBeginEdit);
                    m_DataGrid.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(OnEndEdit);
                    m_DataGrid.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(OnMouseLeftButtonUp);
                    m_DataGrid.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(OnMouseLeftButtonDown);
                    m_DataGrid.MouseMove += new System.Windows.Input.MouseEventHandler(OnMouseMove);
                }
                else
                {
                    m_DataGrid.BeginningEdit -= new EventHandler<DataGridBeginningEditEventArgs>(OnBeginEdit);
                    m_DataGrid.CellEditEnding -= new EventHandler<DataGridCellEditEndingEventArgs>(OnEndEdit);
                    m_DataGrid.MouseLeftButtonUp -= new System.Windows.Input.MouseButtonEventHandler(OnMouseLeftButtonUp);
                    m_DataGrid.PreviewMouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(OnMouseLeftButtonDown);
                    m_DataGrid.MouseMove -= new System.Windows.Input.MouseEventHandler(OnMouseMove);

                    m_DataGrid = null;
                    m_Popup = null;
                    DraggedItem = null;
                    IsEditing = false;
                    IsDragging = false;
                }
            }
            else if (depObject is TextBox)
            {
                TextBox textBox = depObject as TextBox;
                if (textBox == null)
                {
                    return;
                }

                textBox.GotFocus += TextBox_GotFocus;
                textBox.LostFocus += TextBox_LostFocus;
            } 
            else if (depObject is RichTextBox) 
            {
                RichTextBox textBox = depObject as RichTextBox;
                if (textBox == null)
                {
                    return;
                }

                textBox.GotFocus += TextBox_GotFocus;
                textBox.LostFocus += TextBox_LostFocus;
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBoxIsEditing = false;
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxIsEditing = true;
        }
        private static void OnBeginEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            IsEditing = true;
            if (IsDragging)
                ResetDragDrop();
        }
        private static void OnEndEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsEditing = false;
        }
        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging || IsEditing)
            {
                return;
            }
            m_DataGrid.Cursor = Cursors.Arrow;
            var targetItem = m_DataGrid.SelectedItem;
            if (targetItem == null || !ReferenceEquals(DraggedItem, targetItem))
            {
                var targetIndex = (m_DataGrid.ItemsSource as IList).IndexOf(targetItem);
                if (targetIndex == -1) return;
                (m_DataGrid.ItemsSource as IList).Remove(DraggedItem);
                (m_DataGrid.ItemsSource as IList).Insert(targetIndex, DraggedItem);
                m_DataGrid.SelectedItem = DraggedItem;
            }
            ResetDragDrop();
        }
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEditing) return;
            var row = UiHelper.TryFindFromPoint<DataGridRow>((UIElement)sender, e.GetPosition(m_DataGrid));
            if (row == null || row.IsEditing) return;
            IsDragging = true;
            DraggedItem = row.Item;
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging || e.LeftButton != MouseButtonState.Pressed || TextBoxIsEditing)
            {
                return;
            }
            if (m_DataGrid.Cursor != Cursors.SizeAll)
            {
                m_DataGrid.Cursor = Cursors.SizeAll;
            }
            if (!m_Popup.IsOpen)
            {
                m_DataGrid.IsReadOnly = true;
                m_Popup.IsOpen = true;
            }
            Size popupSize = new Size(m_Popup.ActualWidth, m_Popup.ActualHeight);
            m_Popup.PlacementRectangle = new Rect(e.GetPosition(m_DataGrid), popupSize);

            Point point = e.GetPosition(m_DataGrid);
            var row = UiHelper.TryFindFromPoint<DataGridRow>(m_DataGrid, point);
            if (row != null)
            {
                m_DataGrid.SelectedItem = row.Item;
            }
        }

        private static void ResetDragDrop()
        {
            IsDragging = false;
            m_Popup.IsOpen = false;
            m_DataGrid.IsReadOnly = false;
        }

        public static readonly DependencyProperty EnableProperty = DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(DragDropRowBehavior), new UIPropertyMetadata(false, OnEnabledChanged));

        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }
        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }
        private static void OnEnabledChanged(DependencyObject depObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool == false)
            {
                throw new ArgumentException("Value should be of bool type", "Enabled");
            }
            m_Enable = (bool)e.NewValue;
        }


    }
}
