using System.Data;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;


IUserInteractor userInteractor = new ConsoleUserInteractor();
ICommentsSource commentsFromConsole = new CommentsFromConsole(userInteractor);

CommentApp commentApp = new CommentApp(userInteractor,commentsFromConsole);

commentApp.Run();
