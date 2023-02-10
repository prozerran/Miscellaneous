namespace WebApiNet7.Api.Modules.Reports.Models
{
    public sealed record Records
    {
        public int Id { get; init; } = 0;

        public string Name { get; init; } = string.Empty;
    }
}
