using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public static class NavigationManager
{
    private static Dictionary<Type, Window> _openWindows = new Dictionary<Type, Window>();

    public static void Navigate<T>() where T : Window, new()
    {
        Type windowType = typeof(T);

        // Kiểm tra xem cửa sổ đã tồn tại chưa
        if (_openWindows.TryGetValue(windowType, out Window existingWindow))
        {
            // Nếu cửa sổ đã tồn tại, đưa lên phía trước
            existingWindow.Activate();
        }
        else
        {
            // Tạo cửa sổ mới
            var newWindow = new T();
            newWindow.Closed += (s, e) => {
                // Loại bỏ khỏi danh sách khi đóng
                _openWindows.Remove(windowType);
            };

            newWindow.Show();
            _openWindows[windowType] = newWindow;
        }
    }

    public static void CloseCurrentWindow(Window currentWindow)
    {
        // Đóng cửa sổ hiện tại nếu không phải là cửa sổ mới
        if (!_openWindows.ContainsValue(currentWindow))
        {
            currentWindow.Close();
        }
    }
}
