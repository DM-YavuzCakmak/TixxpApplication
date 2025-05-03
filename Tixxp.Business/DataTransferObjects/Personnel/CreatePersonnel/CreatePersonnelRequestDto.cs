namespace Tixxp.Business.DataTransferObjects.Personnel.CreatePersonnel;

public class CreatePersonnelRequestDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public string NationalIdNumber { get; set; }
}
