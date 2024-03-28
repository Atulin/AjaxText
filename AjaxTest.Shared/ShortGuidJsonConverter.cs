using System.Text.Json;
using System.Text.Json.Serialization;

namespace AjaxTest.Shared;

public class ShortGuidJsonConverter : JsonConverter<ShortGuid>
{
	public override ShortGuid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
		=> ShortGuid.TryParse(reader.GetString(), out var guid) ? guid : null;

	public override void Write(Utf8JsonWriter writer, ShortGuid value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString());
}