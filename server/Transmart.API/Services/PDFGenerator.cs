
using iText.Html2pdf;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using TranSmart.API.Models;

namespace TranSmart.API.Services
{
    public class PdfGenerator
	{
		private const string Path = @"D:\Watermark.png";
		readonly string XsltPath = @"D:\XSLData.xsl";
        string Password = "";
		readonly string AdminPassword = "TDS@AVONTIX";

        public MemoryStream ConvertClassToPdf(PaySlip paySlip)
        {
            try
            {
                Password = paySlip.EmpCode.ToUpper();
                var xs = new XmlSerializer(typeof(PaySlip));
                var memStream = new MemoryStream();

                //Local save
                //System.IO.TextWriter txtWriter = new System.IO.StreamWriter(XmlPath);
                //xs.Serialize(txtWriter, paySlip);
                //txtWriter.Close();

                xs.Serialize(memStream, paySlip);

                MemoryStream htmlstream = ConvertXMltoXSLT(memStream);
                MemoryStream PdfStream = ConvertHTMltoPdf(htmlstream);
                return PdfStream;
            }
            catch (Exception)
            { throw; }
        }
        public MemoryStream ConvertXMltoXSLT(MemoryStream ms)
        {
            try
            {
				// Create a resolver with default credentials.
				var resolver = new XmlUrlResolver
				{
					Credentials = System.Net.CredentialCache.DefaultCredentials
				};
				// transform the personnel.xml file to HTML
				var transform = new XslTransform();
                // load up the stylesheet
                transform.Load(XsltPath, resolver);
                ms.Position = 0;
                var doc = new XPathDocument(ms);
                //Local save
                // transform.Transform(XmlPath, HtmlPath, resolver);
                var mstream = new MemoryStream();
                transform.Transform(doc, null, mstream);
                return mstream;
               
            }
            catch (Exception)
            {
				throw;
			}
        }

        public MemoryStream ConvertHTMltoPdf(MemoryStream mstream)
        {
            try
            {
                var pdfstream = new MemoryStream();
                PdfDocument pdf;
                if (!string.IsNullOrEmpty(Password))
                {
                    var userPassword = System.Text.Encoding.ASCII.GetBytes(Password);
                    var ownerPassword = System.Text.Encoding.ASCII.GetBytes(AdminPassword);

                    WriterProperties props = new WriterProperties()
                    .SetStandardEncryption(userPassword, ownerPassword, EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_128 | EncryptionConstants.DO_NOT_ENCRYPT_METADATA);

                    pdf = new PdfDocument(new PdfWriter(pdfstream, props));
                }
                else
                {
                   pdf = new PdfDocument(new PdfWriter(pdfstream));
                }
                var doc = new Document(pdf);
                if (System.IO.File.Exists(Path))
                {
              
                    ImageData imageData = ImageDataFactory.Create(@"D:\Watermark.png");
                    var image = new Image(imageData);

                    image.SetMargins(-doc.GetTopMargin(), -doc.GetRightMargin(), -doc.GetBottomMargin(), -doc.GetLeftMargin());
                    // This adds the image to the page
                    doc.Add(image);
                }

                mstream.Position = 0;
                var converterProperties = new ConverterProperties();
                mstream.Position = 0;
                HtmlConverter.ConvertToPdf(mstream,pdf,converterProperties);

                return pdfstream;
            }
            catch (Exception Ex)
            {
				throw;
			}
        }

    }
}
