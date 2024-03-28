using System.ComponentModel;
using System.Globalization;

namespace AjaxTest.Shared;

public class ShortGuidTypeConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) 
		=> sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) 
		=> value is string casted
			? new ShortGuid(casted)
			: base.ConvertFrom(context, culture, value);

	public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) 
		=> destinationType == typeof (string) && value is ShortGuid casted
			? string.Join("", casted)
			: base.ConvertTo(context, culture, value, destinationType);
}