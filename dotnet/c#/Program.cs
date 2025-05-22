
DateTime ambiguousTime = new DateTime(2025, 3, 30, 1, 30, 0);
TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"); // Europe/Lodon
bool isInvalid = ukTimeZone.IsInvalidTime(ambiguousTime);
Console.WriteLine($"Time: {ambiguousTime}");
Console.WriteLine($"Is invalid time in UK timezone? {isInvalid}");

try
{
    DateTimeOffset dto = new DateTimeOffset(ambiguousTime, ukTimeZone.GetUtcOffset(ambiguousTime));
    Console.WriteLine($"DateTimeOffset: {dto}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Exception caught: {ex.Message}");
}