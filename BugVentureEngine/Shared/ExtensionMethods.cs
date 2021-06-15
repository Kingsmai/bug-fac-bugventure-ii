using System;
using System.Xml;

namespace BugVentureEngine.Shared
{
	// 这个类是专门用来拓展已有的对象，向其添加更多自定义的方法
	public static class ExtensionMethods
	{
		// 必须是 static 函数
		// 第一个参数必须是：this 对象名 变量名。
		public static int AttributeAsInt(this XmlNode node, string attributeName)
		{
			return Convert.ToInt32(node.AttributeAsString(attributeName));
		}

		public static string AttributeAsString(this XmlNode node, string attributeName)
		{
			XmlAttribute attribute = node.Attributes?[attributeName];

			if (attribute == null)
			{
				throw new ArgumentException($"The attribute '{attributeName}' does not exists");
			}

			return attribute.Value;
		}
	}
}
