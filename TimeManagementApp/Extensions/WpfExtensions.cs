using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TimeManagementApp.Extensions
{
    public static class WpfExtensions
    {
        public static void BeginInvoke<T>(this T element, Action<T> action, DispatcherPriority priority = DispatcherPriority.ApplicationIdle) where T : UIElement
        {
            element.Dispatcher.BeginInvoke(priority, action);
        }

        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(row);
            if (gridRow == null)
                return null;
            return GetCell(grid, gridRow, column);
        }

        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
