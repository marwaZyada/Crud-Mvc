using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage ="Password is Required")]
		[DataType(DataType.Password)]
        public string NewPassword { get; set; }

		[Required(ErrorMessage = "ConfirmPassword is Required")]
		[DataType(DataType.Password)]
		[Compare("NewPassword",ErrorMessage ="password doesn't match")]
		public string ConfirmPassword { get; set; }

	}
}
