using CommunityToolkit;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TsModelGeneratorLib.Attributes;

namespace TsDataAnnotations.Models;

[GenerateTypeScript]
public partial class LoginModel : BaseViewModel
{
    [Required]
    [ObservableProperty]
    [Length(4, 64, ErrorMessage = "O nome de usuario deve ter no minimo 4 e no maximo 32 letras ou numeros, sem espaços")]
    [NotifyDataErrorInfo]
    private string _username = "";

    [Required]
    [Length (8, 16, ErrorMessage = "A senha deve ter no minimo 8 e no maximo 16 caracteres")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    private string _senha = "";

    [Required]
    [Length(8, 8, ErrorMessage = "A senha otp deve ter 8 caracteres")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    private string _otp = "";
}
