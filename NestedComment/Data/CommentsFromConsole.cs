using NestedComment;

public class CommentsFromConsole : ICommentsSource
{
    private List<Comment> rootComments = new List<Comment>();

    private readonly IUserInteractor _userInteractor;

    private int nextId = 1;

    public CommentsFromConsole(IUserInteractor userInteractor)
    {
        _userInteractor = userInteractor;
    }

    public void AddRootComment()
    {
        _userInteractor.ShowMessage("Enter your root comment: ");
        string? content = _userInteractor.ReadFromUser();
        rootComments.Add(new Comment(nextId++, content));
        _userInteractor.ShowMessage("Root comment added successfully.");
    }

    public void ReplyToComment()
    {
        if (rootComments.Count == 0)
        {
            _userInteractor.ShowMessage(Resource.NoCommentsMessage);
            return;
        }
        ViewAllComments();
        _userInteractor.ShowMessage("Enter the ID of the comment you want to reply to: ");
        if (int.TryParse(_userInteractor.ReadFromUser(), out int parentId))
        {
            Comment? parent = FindComment(rootComments, parentId);
            if (parent != null)
            {
                _userInteractor.ShowMessage("Enter your reply: ");
                string? content = _userInteractor.ReadFromUser();
                parent.Replies.Add(new Comment(nextId++, content));
                _userInteractor.ShowMessage("Reply added successfully.");
            }
            else
            {
                _userInteractor.ShowMessage(Resource.CommentNotFoundMessage);
            }
        }
        else
        {
            _userInteractor.ShowMessage("Invalid ID. Please enter a number.");
        }
    }

    private Comment? FindComment(List<Comment> comments, int id)
    {

        return comments.FirstOrDefault(c => c.Id == id)
         ?? comments.Select(c => FindComment(c.Replies, id))
                    .FirstOrDefault(found => found != null);

        //foreach (var comment in comments)
        //{
        //    if (comment.Id == id)
        //        return comment;

        //    var foundInReplies = FindComment(comment.Replies, id);
        //    if (foundInReplies != null)
        //        return foundInReplies;
        //}
        //return null;


    }

    public void ViewAllComments()
    {
        if (rootComments.Count == 0)
        {
            _userInteractor.ShowMessage(Resource.NoCommentsMessage);
            return;
        }

        rootComments.ForEach(rootComment => DisplayCommentTree(rootComment,0));

    }

    private void DisplayCommentTree(Comment comment, int depth)
    {
        _userInteractor.ShowMessage($"{new string(' ', depth * 2)}[{comment.Id}] {comment.Content}");
        
        comment.Replies.ForEach(reply => DisplayCommentTree(reply,depth + 1));

    }

    public List<Comment> GetRootComments()
    {
        return rootComments;
    }
}