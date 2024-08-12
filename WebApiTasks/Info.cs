namespace WebApiTasks
{
    public class Info
    {
        public string? Owner { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
    public class Image
    {
        public IFormFile? File { get; set; }
    }
}

