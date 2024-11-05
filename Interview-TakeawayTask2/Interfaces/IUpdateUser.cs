namespace Interview_TakeawayTask2.Interfaces
{
    public interface IUpdateUser : IUser
    {
        public string? NewPassword { get; set; }
        public string? NewUsername { get; set; }
        public int? Age { get; set; }
    }
}
