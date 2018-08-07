namespace SSH.Core.Enum
{
    public enum JobStatus
    {
        None = 0,
        Created = 1,
        Accepted = 2,
        Orphan = 3,
        Abandoned = 4,
        Completed = 5,
        InProgress = 6,
        Exception = 7,
        Closed = 8
    }
}
