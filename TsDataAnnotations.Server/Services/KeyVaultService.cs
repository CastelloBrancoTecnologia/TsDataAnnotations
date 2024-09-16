using System.Text;

namespace TsDataAnnotations.Server.Services
{
    public static class KeyVaultService
    {
        public static string GetKey()
        {
            return "43e4dbf0-52ed-4203-895d-42b586496bd4";
        }

        public static byte[] GetKeyBytes()
        {
            return Encoding.ASCII.GetBytes(KeyVaultService.GetKey());
        }
    }
}
