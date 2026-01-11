namespace CMCS.Services.Interfaces
{
    public interface IInvoiceService
    {
        string GenerateInvoicePdf(int claimId);
    }
}
