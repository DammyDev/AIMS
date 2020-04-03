using CyberArk.AIM.NetPasswordSDK;
using CyberArk.AIM.NetPasswordSDK.Exceptions;
using Serilog;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ProjectAPI
{
	public class Cyber_Ark
	{
		public static string GetConnectionString()
		{
            PSDKPassword password = null;
            string C_Str = "";
            try
            {
                PSDKPasswordRequest passRequest = new PSDKPasswordRequest
                {
                    AppID = "AMS",
                    ConnectionPort = 18923,
                    ConnectionTimeout = 30,
                    Safe = "UAT-AMS-DATABASE",
                    Folder = "root",
                    Object = "Database-UAT-OracleDatabase-172.27.4.135-batappuser",
                    Reason = "AMS Requesting Database Password"
                };

                passRequest.RequiredProperties.Add("PolicyId");
                passRequest.RequiredProperties.Add("userName");
                passRequest.RequiredProperties.Add("Address");

                // Sending the request to get the password
                Log.Information("...retrieving password from CyberArk Vault");
                password = PasswordSDK.GetPassword(passRequest);

                // Analyzing the response
                SecureString secret = password.SecureContent;

                //Build the connectionstring
                Log.Information("...building ConnectionString.");
                C_Str = $"Server={password.Address};Database=dbAppManager2;uid={password.UserName};password={ConvertToUnSecureString(secret)};";
               
            }
            catch (PSDKException ex)
            {
                Log.Error(ex.Reason);
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
            }
            finally
            {
                if (password != null)
                {
                    password.SecureContent.Dispose();
                    Log.Information("CyberArk Password used and disposed successfully.");
                }
            }
            return C_Str;
        }

        private static string ConvertToUnSecureString(SecureString secstrPassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secstrPassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}