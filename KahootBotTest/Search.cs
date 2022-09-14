namespace Katoot;

public class Search
{
    public entity[] entities { get; set; }
    
}

public class entity
{
    public card card { get; set; }
}



public class card
{
    public coverMetadata coverMetadata { get; set; }
    public Guid uuid { get; set; }
}

public class coverMetadata
{
    public Guid id { get; set; }
}