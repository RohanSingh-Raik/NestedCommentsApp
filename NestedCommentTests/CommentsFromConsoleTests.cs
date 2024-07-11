using Moq;
using NestedComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NestedCommentTests;

public class CommentsFromConsoleTests
{
    private Mock<IUserInteractor> _userCommunicationMock;
    private int _nextId;
    private List<Comment> rootComments;
    private CommentsFromConsole _cut;

    public CommentsFromConsoleTests()
    {
        _userCommunicationMock = new Mock<IUserInteractor>();
        _nextId = 1;
        rootComments = new List<Comment>();
        _cut = new CommentsFromConsole(_userCommunicationMock.Object);
    }
   
    [Fact]
    public void AddRootComment_ShallAddRootComment()
    {
        string expectedComment = "Rohan";
        _userCommunicationMock.Setup(mock => mock.ReadFromUser())
            .Returns(expectedComment);

        _cut.AddRootComment();

        var rootComments = _cut.GetRootComments();
        Assert.Equal(expectedComment, rootComments[0].Content);
    }

    [Fact]
    public void ReplyToComment_ShallShowNoCommentsMessage_IfThereAreNoComments()
    {
        _cut.ReplyToComment();

        _userCommunicationMock.Verify(mock => mock.ShowMessage(Resource.NoCommentsMessage)
                              ,Times.Once());
    }

    [Fact]
    public void ReplyToComment_ShallShowCommentNotFoundMessage_IfUserEntersInvalidId()
    {
        string expectedComment = "Rohan";
        _userCommunicationMock.Setup(mock => mock.ReadFromUser())
            .Returns(expectedComment);

        _cut.AddRootComment();

        _userCommunicationMock.Setup(mock => mock.ReadFromUser())
            .Returns("2");
        _cut.ReplyToComment();
        _userCommunicationMock.Verify(mock => mock.ShowMessage(Resource.CommentNotFoundMessage));

    }

    [Fact]
    public void ReplyToComment_ShallAddReply_IfUserEntersValidId()
    {
        string expectedComment = "Rohan";
        _userCommunicationMock.Setup(mock => mock.ReadFromUser())
            .Returns(expectedComment);

        _cut.AddRootComment();

        string expectedReply = "Singh";
        string expectedId = "1";
        _userCommunicationMock.SetupSequence(mock => mock.ReadFromUser())
            .Returns(expectedId)
            .Returns(expectedReply);

        _cut.ReplyToComment();

        var rootComments = _cut.GetRootComments();
        var reply = rootComments[0].Replies[0].Content;

        Assert.Equal(expectedReply, reply);

    }

    [Fact]
    public void ViewAllComments_ShallShowNoCommentsMessage_IfThereAreNoComments()
    {
        _cut.ViewAllComments();
        _userCommunicationMock.Verify(mock => mock.ShowMessage(Resource.NoCommentsMessage)
                              ,Times.Once());
    }
}



