using CommunityToolkit;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using TsModelGeneratorLib.Attributes;

namespace TsDataAnnotations.Models;

[GenerateTypeScript]
public partial class Usuario : BaseViewModel
{
    [ObservableProperty]
    private long _id = 0;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(maximumLength: 32, ErrorMessage = "O username deve ter no maximo 32 caracteres")]
    private string _username = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(maximumLength:64, ErrorMessage = "O Nome deve ter no maximo 64 caracteres")]
    private string _nome = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength( maximumLength:80, ErrorMessage = "O Email deve ter no maximo 128 caracteres")]
    [Email]
    private string _email = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(maximumLength: 32, ErrorMessage = "O celular deve ter no maximo 128 caracteres")]
    private string _celular = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string _hashSenha = "";

    public static bool VerificaSenha(Usuario u, string senha_descriptografada)
    {
        byte[] salt_and_hash = Convert.FromBase64String(u.HashSenha);
        byte[] salt = new byte[16];

        Array.Copy(salt_and_hash, 0, salt, 0,  16);

        var pbkdf2 = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(senha_descriptografada), salt, 1000, HashAlgorithmName.SHA512);

        byte[] hash = pbkdf2.GetBytes(64);

        byte[] hashBytes = new byte[80];

        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 64);

        return Convert.ToBase64String(hashBytes) == u.HashSenha;
    }

    public static string Hash (string senha) 
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        var pbkdf2 = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(senha), salt, 1000, HashAlgorithmName.SHA512);

        byte[] hash = pbkdf2.GetBytes(64);

        byte[] hashBytes = new byte[80];

        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 64);

        return Convert.ToBase64String(hashBytes);
    }
}
