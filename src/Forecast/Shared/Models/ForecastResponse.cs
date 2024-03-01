using System.Text.Json.Serialization;

namespace Forecast.Shared.Models;


public partial class ForecastResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("title")]
    public Title Title { get; set; }

    [JsonPropertyName("domain")]
    public Domain Domain { get; set; }

    [JsonPropertyName("parameters")]
    public Parameters Parameters { get; set; }

    [JsonPropertyName("ranges")]
    public Ranges Ranges { get; set; }
}

public partial class Domain
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("domainType")]
    public string DomainType { get; set; }

    [JsonPropertyName("axes")]
    public Axes Axes { get; set; }
}

public partial class Axes
{
    [JsonPropertyName("t")]
    public T T { get; set; }

    [JsonPropertyName("x")]
    public X X { get; set; }

    [JsonPropertyName("y")]
    public X Y { get; set; }
}

public partial class T
{
    [JsonPropertyName("values")]
    public DateTimeOffset[] Values { get; set; }
}

public partial class X
{
    [JsonPropertyName("values")]
    public double[] Values { get; set; }

    [JsonPropertyName("bounds")]
    public double[] Bounds { get; set; }
}



public partial class Parameters
{
    [JsonPropertyName("temperature-0m")]
    public ParametersGustWindSpeed10M Temperature0M { get; set; }

    [JsonPropertyName("gust-wind-speed-10m")]
    public ParametersGustWindSpeed10M GustWindSpeed10M { get; set; }

    [JsonPropertyName("wind-speed-10m")]
    public ParametersGustWindSpeed10M WindSpeed10M { get; set; }

    [JsonPropertyName("total-precipitation")]
    public ParametersGustWindSpeed10M TotalPrecipitation { get; set; }
}

public partial class ParametersGustWindSpeed10M
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public Title Description { get; set; }

    [JsonPropertyName("observedProperty")]
    public ObservedProperty ObservedProperty { get; set; }
}

public partial class Title
{
    [JsonPropertyName("en")]
    public string En { get; set; }
}

public partial class ObservedProperty
{
    [JsonPropertyName("label")]
    public Title Label { get; set; }
}

public partial class Ranges
{
    [JsonPropertyName("temperature-0m")]
    public RangesGustWindSpeed10M Temperature0M { get; set; }

    [JsonPropertyName("gust-wind-speed-10m")]
    public RangesGustWindSpeed10M GustWindSpeed10M { get; set; }

    [JsonPropertyName("wind-speed-10m")]
    public RangesGustWindSpeed10M WindSpeed10M { get; set; }

    [JsonPropertyName("total-precipitation")]
    public RangesGustWindSpeed10M TotalPrecipitation { get; set; }
}

public partial class RangesGustWindSpeed10M
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("dataType")]
    public string DataType { get; set; }


    [JsonPropertyName("shape")]
    public long[] Shape { get; set; }

    [JsonPropertyName("values")]
    public double[] Values { get; set; }
}
