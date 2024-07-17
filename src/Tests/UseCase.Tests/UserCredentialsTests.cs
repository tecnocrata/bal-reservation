using System.Threading.Tasks;
using Moq;
using Shared.Core.Bus.Query;
using Shared.Core.Results;
using SystemUsers.Core;
using SystemUsers.UseCases.Login;
using SytemUsers.UseCases.Login;
using Xunit;

namespace SystemUsers.Tests;
public class LoginQueryHandlerTests
{
  private readonly Mock<IUserRepository> _userRepositoryMock;
  private readonly Mock<ITokenProvider> _tokenProviderMock;
  private readonly LoginQueryHandler _handler;

  public LoginQueryHandlerTests()
  {
    _userRepositoryMock = new Mock<IUserRepository>();
    _tokenProviderMock = new Mock<ITokenProvider>();
    _handler = new LoginQueryHandler(_userRepositoryMock.Object, _tokenProviderMock.Object);
  }

  [Fact]
  public async Task Handle_ReturnsFailure_WhenUserNotFound()
  {
    // Arrange
    var user = new User("testuser", "password");
    var query = new LoginQuery { UserName = "testuser", Password = "password" };
    _userRepositoryMock.Setup(r => r.CheckUserCredentials(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync((User?)null);

    // Act
    var result = await _handler.Handle(query);

    // Assert
    _tokenProviderMock.Verify(t => t.GenerateToken(It.Is<User>(u => u == user)), Times.Never);
    Assert.True(result.IsFailure);
    Assert.Equal("user-not-found", result.Error.Code);
  }

  [Fact]
  public async Task Handle_ReturnsSuccess_WithToken_WhenUserIsFound()
  {
    // Arrange
    var user = new User("testuser", "password");
    var token = "generated-token";
    var query = new LoginQuery { UserName = "testuser", Password = "password" };

    _userRepositoryMock.Setup(r => r.CheckUserCredentials(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(user);
    _tokenProviderMock.Setup(t => t.GenerateToken(It.IsAny<User>()))
                      .Returns(token);

    // Act
    var result = await _handler.Handle(query);

    // Assert
    _tokenProviderMock.Verify(t => t.GenerateToken(It.Is<User>(u => u == user)), Times.Once);
    Assert.True(result.IsSuccess);
    Assert.Equal(token, result.Value.Token);
  }

  [Fact]
  public async Task Handle_CallsTokenProvider_WhenUserIsFound()
  {
    // Arrange
    var user = new User("testuser", "password");
    var query = new LoginQuery { UserName = "testuser", Password = "password" };

    _userRepositoryMock.Setup(r => r.CheckUserCredentials(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(user);

    // Act
    await _handler.Handle(query);

    // Assert
    _tokenProviderMock.Verify(t => t.GenerateToken(It.Is<User>(u => u == user)), Times.Once);
  }
}
