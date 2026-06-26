using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using Microsoft.ServiceModel.Samples;

namespace Sats.Utils
{
    public static class CommonUtils
    {
        public static Binding CreateHttpBinding()
        {
            var binding = new BasicHttpBinding
                              {
                                  MaxBufferPoolSize = int.MaxValue,
                                  //MaxBufferSize = int.MaxValue,
                                  MaxReceivedMessageSize = int.MaxValue,
                                  ReaderQuotas =
                                      {
                                          MaxArrayLength = int.MaxValue,
                                          MaxStringContentLength = int.MaxValue
                                      }
                              };

            binding.ReceiveTimeout = TimeSpan.FromHours(12);
            binding.SendTimeout = TimeSpan.FromHours(12);
            binding.OpenTimeout = TimeSpan.FromHours(12);
            binding.CloseTimeout = TimeSpan.FromHours(12);

            return binding;
        }

        public static Binding CreateTcpBinding()
        {
            return CreateTcpBinding(false);
        }

        public static Binding CreateTcpBinding(bool reliableSession)
        {
            var binMessageElement = new BinaryMessageEncodingBindingElement
            {
                ReaderQuotas =
                {
                    MaxArrayLength = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                }
            };

            var gzipElement = new GZipMessageEncodingBindingElement
            {
                InnerMessageEncodingBindingElement = binMessageElement
            };

            var tcpTransport = new TcpTransportBindingElement
            {
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ChannelInitializationTimeout = TimeSpan.FromHours(12),
                PortSharingEnabled = true
            };

            CustomBinding binding;
            if(reliableSession)
            {
                var reliableElement = new ReliableSessionBindingElement
                {
                    InactivityTimeout = TimeSpan.FromHours(12),
                    MaxPendingChannels = 16384,
                    ReliableMessagingVersion = ReliableMessagingVersion.WSReliableMessagingFebruary2005
                };
                binding = new CustomBinding(gzipElement, reliableElement, tcpTransport);
            }
            else
            {
                binding = new CustomBinding(gzipElement, tcpTransport);
            }
            
            binding.ReceiveTimeout = TimeSpan.FromHours(12);
            binding.SendTimeout = TimeSpan.FromHours(12);
            binding.OpenTimeout = TimeSpan.FromHours(12);
            binding.CloseTimeout = TimeSpan.FromHours(12);

            return binding;
        }

        public static Binding CreateUpdateTcpBinding()
        {
            var binMessageElement = new BinaryMessageEncodingBindingElement
            {
                ReaderQuotas =
                {
                    MaxArrayLength = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                }
            };

            var gzipElement = new GZipMessageEncodingBindingElement
            {
                InnerMessageEncodingBindingElement = binMessageElement
            };

            var tcpBinding = new NetTcpBinding
            {
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ListenBacklog = int.MaxValue,
                TransactionFlow = false,
                Security = { Mode = SecurityMode.None }
            };
            tcpBinding.CreateBindingElements().Add(gzipElement);

            //tcpTransport.ReaderQuotas.MaxArrayLength = int.MaxValue;
            //tcpTransport.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            //binding.ReliableSession.Enabled = false;


            //binding.Security.Mode = SecurityMode.None;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            return tcpBinding;
        }

        public static string MD5Standard(byte[] buffer)
        {
            var md5Encrypt = new MD5CryptoServiceProvider();
            var hashData = md5Encrypt.ComputeHash(buffer);

            return hashData.Aggregate("", (current, t) => current + Convert.ToString(t, 16));
        }

        static public string EncodeTo64(byte[] buffers)
        {
            var returnValue = Convert.ToBase64String(buffers);
            return returnValue;
        }

        static public byte[] DecodeFrom64(string encodedData)
        {
            return Convert.FromBase64String(encodedData);
        }
        
        public static string MD5Standard(string source)
        {
            var utf8Encoding = new UTF8Encoding();
            byte[] buffer = utf8Encoding.GetBytes(source);
            return MD5Standard(buffer);
        }

        public static string PrivateMD5(string source)
        {
            return MD5Standard(string.Format("SATS{0}HASH", source));
        }

        public static string MakeUTF8String(string value)
        {
            var res = "";
            foreach (var c in value)
            {
                if (c < 255)
                {
                    res = res + c;
                }
                else
                {
                    res = res + "\\" + string.Format("{0:X4}", Convert.ToUInt16(c));
                }
            }

            return res;
        }

        public static void CacheObject(object serializeObject, string fileName)
        {
            Directory.CreateDirectory(new FileInfo(fileName).Directory.FullName);
            var binaryFormater = new BinaryFormatter();
            var stream = File.Open(fileName, FileMode.Create);
            binaryFormater.Serialize(stream, serializeObject);
            stream.Close();
        }

        public static bool IsCached(string fileName)
        {
#if DEBUG
            return false;
#else
            return File.Exists(fileName);
#endif
        }

        public static object GetCache(string fileName)
        {
            var binaryFormater = new BinaryFormatter();
            var stream = File.Open(fileName, FileMode.Open);
            var cacheResult = binaryFormater.Deserialize(stream);
            stream.Close();

            return cacheResult;
        }

        public static string MD5File(string fileName)
        {
            using (var f = File.OpenRead(fileName))
            {
                var buffer = new byte[f.Length];
                f.Read(buffer, 0, buffer.Length);
                return MD5Standard(buffer);
            }
        }
    }
}
