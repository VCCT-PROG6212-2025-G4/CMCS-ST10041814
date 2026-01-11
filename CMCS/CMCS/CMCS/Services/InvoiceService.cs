using CMCS.Models;
using CMCS.Repositories.Interfaces;
using CMCS.Services.Interfaces;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace CMCS.Services
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class InvoiceService : IInvoiceService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IWebHostEnvironment _env;

        public InvoiceService(
            IClaimRepository claimRepository,
            IWebHostEnvironment env)
        {
            _claimRepository = claimRepository;
            _env = env;
        }
        // print the pdf
        public string GenerateInvoicePdf(int claimId)
        {
            var claim = _claimRepository.GetById(claimId)
                ?? throw new Exception("Claim not found");

            // wwwroot/Claims/{id}
            var claimFolder = Path.Combine(
                _env.WebRootPath,
                "Claims",
                claimId.ToString());

            if (!Directory.Exists(claimFolder))
                Directory.CreateDirectory(claimFolder);

            var fileName = $"{claimId}-Invoice.pdf";
            var filePath = Path.Combine(claimFolder, fileName);

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var titleFont = new XFont("Arial", 18, XFontStyle.Bold);
            var textFont = new XFont("Arial", 12);

            int y = 40;
            // context
            gfx.DrawString("Invoice",
                titleFont,
                XBrushes.Black,
                new XRect(0, y, page.Width, 40),
                XStringFormats.TopCenter);

            y += 60;

            gfx.DrawString($"Claim ID: {claim.ClaimID}", textFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Lecturer: {claim.Lecturer.Username}", textFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Student: {claim.User.Username}", textFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Month / Year: {claim.Month} {claim.Year}", textFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Hours Worked: {claim.HoursWorked}", textFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Rate: {claim.HourRate:C}", textFont, XBrushes.Black, 40, y);
            y += 25;

            gfx.DrawString($"Total: {(claim.HoursWorked * claim.HourRate):C}",
                textFont,
                XBrushes.Black,
                40,
                y);

            document.Save(filePath);

            return filePath;
        }
    }
}
