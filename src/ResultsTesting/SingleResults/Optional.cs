using System.Text.Json.Serialization;
namespace SingleResults;

public record Optional<TValue>(TValue? Value)
{
    [JsonIgnore]
    public bool HasValue => Value is not null;
    public TValue GetValue() => Value ?? throw new InvalidOperationException("no value");
}