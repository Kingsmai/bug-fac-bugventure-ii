// 图片路径：/assemblyName;component/path/to/image.png; 

```c#
loc.ImageName = $"/BugVentureEngine;component/Images/Locations/{imageName}";
```

## ? 和 ?? 运算符

参考资料：https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-

### ? 运算符

可以为null的类型

在处理数据库和其他包含不可赋值的元素的数据类型时，将 null 赋值给**数值类型**或**布尔型**以及**日期类型**的功能特别有用。例如，数据库中的布尔型字段可以存储值 true 或 false，或者，该字段也可以未定义。

### ?? 运算符

```c#
x = y ?? z;

// 等价于
x = (y != null ? y : z);
```

