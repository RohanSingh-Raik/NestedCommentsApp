
using Microsoft.Data.Sqlite;

class CommentsFromConsoleToDb:ICommentsSource
{
    private string connectionString = "Data Source=comments.db";
    private readonly IUserInteractor _userInteractor;

    public CommentsFromConsoleToDb(IUserInteractor userInteractor)
    {
        InitializeDatabase();
        _userInteractor = userInteractor;
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Comments(
                    Id INTEGER PRIMARY KEY,
                    ParentId INTEGER NULL,
                    Content TEXT NOT NULL,
                    FOREIGN KEY (ParentId) REFERENCES Comments(Id)
                )
            ";
            command.ExecuteNonQuery();
        }
    }

    public void AddRootComment()
    {
        _userInteractor.ShowMessage("Enter your root comment: ");
        string? content = _userInteractor.ReadFromUser();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Comments (ParentId, Content)
                VALUES ($parentId, $content);
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$parentId", DBNull.Value);
            command.Parameters.AddWithValue("$content", content);

            long? newId = (long?)command.ExecuteScalar();
            _userInteractor.ShowMessage($"Root comment added successfully with ID: {newId}");
        }
    }

    public void ReplyToComment()
    {
        if (!AnyCommentsExist())
        {
            _userInteractor.ShowMessage("There are no comments to reply to. Please add a root comment first.");
            return;
        }

        ViewAllComments();

        _userInteractor.ShowMessage("Enter the ID of the comment you want to reply to: ");
        if (int.TryParse(_userInteractor.ReadFromUser(), out int parentId))
        {
         
            if (!CommentExists(parentId))
            {
                _userInteractor.ShowMessage($"Error: No comment found with ID {parentId}. Please enter a valid comment ID.");
                return;
            }

            _userInteractor.ShowMessage("Enter your reply: ");
            string? content = _userInteractor.ReadFromUser();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                INSERT INTO Comments (ParentId, Content)
                VALUES ($parentId, $content);
                SELECT last_insert_rowid();
            ";
                command.Parameters.AddWithValue("$parentId", parentId);
                command.Parameters.AddWithValue("$content", content);

                int newId = Convert.ToInt32(command.ExecuteScalar());
                _userInteractor.ShowMessage($"Reply added successfully with ID: {newId}");
            }
        }
        else
        {
            _userInteractor.ShowMessage("Invalid ID. Please enter a number.");
        }
    }

    private bool AnyCommentsExist()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Comments";

            long? count = (long?)command.ExecuteScalar();
            return count > 0;
        }
    }

    private bool CommentExists(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Comments WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }

    public void ViewAllComments()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, ParentId, Content FROM Comments ORDER BY ParentId, Id";

            using (var reader = command.ExecuteReader())
            {
                var comments = new List<Comment>();
                while (reader.Read())
                {
                    comments.Add(new Comment(
                        reader.GetInt32(0),
                        reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                        reader.GetString(2)
                    ));
                }

                if (comments.Count == 0)
                {
                    _userInteractor.ShowMessage("No comments yet.");
                    return;
                }

                DisplayComments(comments);
            }
        }
    }

    private void DisplayComments(List<Comment> comments, int? parentId = null, int depth = 0)
    {
        foreach (var comment in comments.Where(c => c.ParentId == parentId))
        {
            _userInteractor.ShowMessage($"{new string(' ', depth * 2)}[{comment.Id}] {comment.Content}");
            DisplayComments(comments, comment.Id, depth + 1);
        }
    }
}
