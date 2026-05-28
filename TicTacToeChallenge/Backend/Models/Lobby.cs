namespace Backend.Models
{
    public class Lobby
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> Users { get; set; }
        public string Status { get; set; }
    }

    public class CreateLobbyDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
    }

    public class JoinLobbyDTO
    {
        public string Code { get; set; }
        public string Username { get; set; }
    }
}
