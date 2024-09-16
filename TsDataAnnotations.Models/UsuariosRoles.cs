using CommunityToolkit;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using TsModelGeneratorLib.Attributes;

namespace TsDataAnnotations.Models;

[GenerateTypeScript]
public partial class UsuariosRole : BaseViewModel
{
    [ObservableProperty]
    private long _idUsuario = 0;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(maximumLength: 64, ErrorMessage = "A role deve ter no maximo 64 caracteres")]
    private string _role = "";
}
