class Comment
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Content { get; set; }

    public Comment(int id, int? parentId, string content)
    {
        Id = id;
        ParentId = parentId;
        Content = content;
    }
}
