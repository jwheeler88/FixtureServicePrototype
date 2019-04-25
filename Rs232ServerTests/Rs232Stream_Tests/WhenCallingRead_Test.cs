using Moq;
using NUnit.Framework;
using RJCP.IO.Ports;

[Category("RS232Server")]
public class WhenCallingRead
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

public interface ITranslate
{
    string Translate(byte[] data);
}

public class Message
{
    public string Header { get; }
    
    public Message(string header) => Header = header;
}

public class Rs232Stream
{
    private readonly ITranslate _translator;
    private readonly ISerialPortStream _serialPort;

    public int BufferSize { get; set; }

    public Rs232Stream(ITranslate translator, ISerialPortStream serialPort)
    {
        _translator = translator;
        _serialPort = serialPort;
    }

    public Message Read()
    {
        var buffer = new byte[BufferSize];
        _serialPort.Read(buffer, 0, buffer.Length);

        string header = _translator.Translate(buffer);
        var msg = new Message(header);
            
        return msg;
    }
}