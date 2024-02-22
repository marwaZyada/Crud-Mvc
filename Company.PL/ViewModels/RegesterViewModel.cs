using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
	public class RegesterViewModel
	{
	
		[Required(ErrorMessage ="Fname Is Required")]
	
		public string Fname { get; set; }
		[Required(ErrorMessage = "Lname Is Required")]
	
		public string Lname { get; set; }
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Compare("Password",ErrorMessage ="Password doesn't match")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Password Is Required")]
		
		public bool IsAgree { get; set; }
	}
}
