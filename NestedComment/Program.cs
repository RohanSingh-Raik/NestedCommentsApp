using System.Data;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;


IUserInteractor userInteractor = new ConsoleUserInteractor();
ICommentsSource commentsFromConsole = new CommentsFromConsoleToDb(userInteractor);

CommentApp commentApp = new CommentApp(userInteractor,commentsFromConsole);

commentApp.Run();
