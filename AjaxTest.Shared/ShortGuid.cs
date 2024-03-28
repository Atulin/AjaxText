using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AjaxTest.Shared;

[JsonConverter(typeof(ShortGuidJsonConverter))]
[TypeConverter(typeof(ShortGuidTypeConverter))]
public record ShortGuid
{
	private readonly string _id;
	private ShortGuid(Guid guid) => _id = Convert.ToBase64String(guid.ToByteArray());
	public ShortGuid() => _id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
	internal ShortGuid(string s) => _id = s;

	public override string ToString() => _id;
	public static implicit operator string(ShortGuid guid) => guid.ToString();

	public static ShortGuid Parse(string? s)
	{
		ArgumentException.ThrowIfNullOrEmpty(s);
		
		var bytes = Convert.FromBase64String(s);
		var guid = new Guid(bytes);
		return new ShortGuid(guid);
	}

	public static bool TryParse(string? s, [NotNullWhen(true)]out ShortGuid? result)
	{
		if (s is null)
		{
			result = null;
			return false;
		}

		byte[] bytes;
		try {
			bytes = Convert.FromBase64String(s);
		}
		catch{
			result = null;
			return false;
		}
		
		if (bytes.Length != 16)
		{
			result = null;
			return false;
		}

		var guid = new Guid(bytes);
		result = new ShortGuid(guid);
		return true;
	}
}