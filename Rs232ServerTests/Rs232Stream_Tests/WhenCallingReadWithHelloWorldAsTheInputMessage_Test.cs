using FixtureService.Streams;
using Moq;
using NUnit.Framework;
using RJCP.IO.Ports;

[Category("Rs232Stream")]
public class WhenCallingReadWithHelloWorldAsTheInputMessage
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ReturnsMessageWithCorrectHeader()
    {
        // Arrange
        var msg = "Hello, world!";
        
        var bufferSize = 256;
        var buffer = new byte[bufferSize];

        var mockSerialPort = new Mock<ISerialPortStream>(MockBehavior.Strict);
        mockSerialPort
            .Setup(sp => sp.Read(buffer, 0, buffer.Length))
            .Returns(It.IsAny<int>());
            
        var mockTranslator = new Mock<ITranslate>();
        mockTranslator.Setup(t => t.Translate(buffer)).Returns(msg);

        var sut = new Rs232Stream(mockTranslator.Object, mockSerialPort.Object)
        {
            BufferSize = bufferSize
        };

        // Act
        Message message = sut.Read();

        // Assert
        Assert.AreEqual(msg, message.Header);
    }
}