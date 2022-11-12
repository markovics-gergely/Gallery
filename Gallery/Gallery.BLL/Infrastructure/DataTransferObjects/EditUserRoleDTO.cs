namespace Gallery.BLL.Infrastructure.DataTransferObjects
{
    public class EditUserRoleDTO
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
