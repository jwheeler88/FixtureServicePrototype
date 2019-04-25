namespace FixtureService.Streams
{
    public class Message
    {
        public string Header { get; }
    
        public Message(string header) => Header = header;
    }
}