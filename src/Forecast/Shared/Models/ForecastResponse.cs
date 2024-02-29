using System.Text.Json.Serialization;


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

    [JsonPropertyName("referencing")]
    public Referencing[] Referencing { get; set; }
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

public partial class Referencing
{
    [JsonPropertyName("coordinates")]
    public string[] Coordinates { get; set; }

    // [JsonPropertyName("system")]
    // public SystemClass System { get; set; }
}

// public partial class SystemClass
// {
//     [JsonPropertyName("type")]
//     public string Type { get; set; }

//     [JsonPropertyName("id", NullValueHandling = NullValueHandling.Ignore)]
//     public Uri Id { get; set; }

//     [JsonPropertyName("calendar", NullValueHandling = NullValueHandling.Ignore)]
//     public string Calendar { get; set; }
// }

public partial class Parameters
{
    [JsonPropertyName("temperature-0m")]
    public ParametersTemperature0M Temperature0M { get; set; }

    [JsonPropertyName("total-precipitation")]
    public ParametersTemperature0M TotalPrecipitation { get; set; }
}

public partial class ParametersTemperature0M
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
    public RangesTemperature0M Temperature0M { get; set; }

    [JsonPropertyName("total-precipitation")]
    public RangesTemperature0M TotalPrecipitation { get; set; }
}

public partial class RangesTemperature0M
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("dataType")]
    public string DataType { get; set; }

    [JsonPropertyName("axisNames")]
    public string[] AxisNames { get; set; }

    [JsonPropertyName("shape")]
    public long[] Shape { get; set; }

    [JsonPropertyName("values")]
    public double[] Values { get; set; }
}