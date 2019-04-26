using FixtureService.Streams;
using Moq;
using NUnit.Framework;
using RJCP.IO.Ports;

[TestFixture("Hello, world!")]
[Category("Rs232Stream")]
public class WhenCallingReadWithHelloWorldAsTheInputMessage
{
    private readonly string _msgHeader;
    private byte[] _buffer;
    private ISerialPortStream _serialPort;
    private ITranslate _translator;
    private int _bufferSize;

    public WhenCallingReadWithHelloWorldAsTheInputMessage(string msgHeader)
    {
        _msgHeader = msgHeader;
    }
    
    [SetUp]
    public void Setup()
    {
        _bufferSize = 256;
        _buffer = new byte[_bufferSize];
        
        _serialPort = GetMockSerialPort();
        _translator = GetMockTranslator();
    }

    [Test]
    public void ReturnsMessageWithCorrectHeader()
    {
        // Arrange
        var sut = new Rs232Stream(_translator, _serialPort) { BufferSize = _bufferSize };
        
        // Act
        Message message = sut.Read();

        // Assert
        StringAssert.AreEqualIgnoringCase(_msgHeader, message.Header);
    }

    private ITranslate GetMockTranslator()
    {
        var mockTranslator = new Mock<ITranslate>();
        mockTranslator.Setup(t => t.Translate(_buffer)).Returns(_msgHeader);
        
        return mockTranslator.Object;
    }

    private ISerialPortStream GetMockSerialPort()
    {
        var mockSerialPort = new Mock<ISerialPortStream>(MockBehavior.Strict);
        mockSerialPort
            .Setup(sp => sp.Read(_buffer, 0, _buffer.Length))
            .Returns(It.IsAny<int>());
        
        return mockSerialPort.Object;
    }
}