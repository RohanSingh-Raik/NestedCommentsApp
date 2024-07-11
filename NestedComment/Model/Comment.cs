
public class Comment
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public List<Comment> Replies { get; set; } = new List<Comment>();

    public Comment(int id, string? content)
    {
        Id = id;
        Content = content;
    }
}
