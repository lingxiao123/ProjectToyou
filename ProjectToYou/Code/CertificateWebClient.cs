using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ProjectToYou.Code
{
    /// <summary>
    /// 
    /// </summary>
    public class CertificateWebClient : WebClient
    {
        private readonly X509Certificate2 certificate;
        public CertificateWebClient()
        {
            X509Certificate2 cert = new X509Certificate2();
            Convert.ToBase64String(cert.GetPublicKey());

            X509Certificate2 Cert = new X509Certificate2("client.p12", "1234", X509KeyStorageFlags.MachineKeySet);
        }
        public CertificateWebClient(X509Certificate2 cert)
        {
            certificate = cert;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate X509certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
            {
                return true;
            };

            request.ClientCertificates.Add(certificate);
            return request;
        }
    }
}