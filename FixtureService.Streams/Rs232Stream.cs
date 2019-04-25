using RJCP.IO.Ports;

namespace FixtureService.Streams
{
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
}