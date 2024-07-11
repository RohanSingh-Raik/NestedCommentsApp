
using NestedComment;

public class CommentApp
{
    private readonly IUserInteractor _userInteractor;
    private readonly ICommentsSource _commentsSource;

    private Dictionary<string, Action> choiceMappingDictionary;

    public CommentApp(IUserInteractor userInteractor , ICommentsSource commentsSource)
    {
        _userInteractor = userInteractor;
        _commentsSource = commentsSource;
        choiceMappingDictionary = new Dictionary<string, Action>
        {
            ["1"] = () => _commentsSource.AddRootComment(),
            ["2"] = () => _commentsSource.ReplyToComment(),
            ["3"] = () => _commentsSource.ViewAllComments(),
        };  
    }

    public void Run()
    {
        _userInteractor.ShowMessage(Resource.WelcomeMessage);


        while (true)
        {
            _userInteractor.ShowMessage("\n1. Add root comment");
            _userInteractor.ShowMessage("2. Reply to comment");
            _userInteractor.ShowMessage("3. View all comments");
            _userInteractor.ShowMessage("4. Exit");
            _userInteractor.ShowMessage(Resource.ChooseOptionMessage);

            string? choice = _userInteractor.ReadFromUser();

            if(choice is null)
            {
                throw new ArgumentNullException("choice cannot be null");
            }

            if(choice == "4")
            {
                return;
            }

            if (choiceMappingDictionary.ContainsKey(choice))
            {
                choiceMappingDictionary[choice]();
            }
            else
            {
                _userInteractor.ShowMessage(Resource.InvalidChoiceMessage);
            }
        }
    }

}
