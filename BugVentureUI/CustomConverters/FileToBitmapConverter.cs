using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BugVentureUI.CustomConverters
{
	public class FileToBitmapConverter : IValueConverter
	{
		// 用于当作缓存。
		private static readonly Dictionary<string, BitmapImage> _locations = new Dictionary<string, BitmapImage>();

		// The “value” parameter is the value of the bound property in the UI’s Image control, for this program.
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// 尝试转换 value 参数然后将其储存为名为 filename 的变量
			if (!(value is string filename))
			{
				return null; // 如果转换失败
			}

			// 当玩家走过这个地点时，先检查字典里有没有。没有的话再增加
			if (!_locations.ContainsKey(filename))
			{
				_locations.Add(filename, new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}{filename}", UriKind.Absolute)));
			}

			return _locations[filename];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
