using Moq;
using NestedComment;
using System.Reflection;
using Xunit;

namespace NestedCommentTests;

public class CommentAppTests
{
    private Mock<IUserInteractor> _userCommunicationMock;
    private Mock<ICommentsSource> _commentsSourceMock;
    private CommentApp _cut;

    public CommentAppTests()
    {
        _userCommunicationMock = new Mock<IUserInteractor>();
        _commentsSourceMock = new Mock<ICommentsSource>();
        _cut = new CommentApp(_userCommunicationMock.Object, _commentsSourceMock.Object);
    }

    [Fact]
    public void Run_ShallShowWelcomeMessage()
    {
        _userCommunicationMock.SetupSequence(mock => mock.ReadFromUser())
            .Returns("3")
            .Returns("4");
        _cut.Run();
        _userCommunicationMock.Verify(mock => mock.ShowMessage(Resource.WelcomeMessage),
            Times.Once());
    }

    [Fact]
    public void Run_ShallThrowArgumentNullException_IfUserChoiceIsNull()
    {
         String? input = null;
        _userCommunicationMock.Setup(mock => mock.ReadFromUser())
         .Returns(input);

        Assert.Throws<ArgumentNullException>(()=>_cut.Run());
    }

    [Fact]
    public void Run_ShallShowInvalidChoiceMessage_IfUserChoiceIsInvalid()
    {
        _userCommunicationMock.SetupSequence(mock => mock.ReadFromUser())
            .Returns("5")
            .Returns("4");

        _cut.Run();
        _userCommunicationMock.Verify(mock => mock.ShowMessage(Resource.InvalidChoiceMessage),
            Times.Once());
    }

    [Fact]
    public void Run_ShallShowChooseAnOptionMessageOnce_IfUserChoosesExitOptionOnFirstInput()
    {
        _userCommunicationMock.Setup(mock => mock.ReadFromUser())
            .Returns("4");
            
        _cut.Run();
        _userCommunicationMock.Verify(mock => mock.ShowMessage(Resource.ChooseOptionMessage),
            Times.Once());

    }
}
