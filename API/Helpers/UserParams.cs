namespace API.Helpers;

public class UserParams : PaginationParams
{
    // Parameter for Filter
    public string CurrentUsername { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;

    // Parameter for Filtering
    public string OrderBy { get; set; } = "lastActive";
}
