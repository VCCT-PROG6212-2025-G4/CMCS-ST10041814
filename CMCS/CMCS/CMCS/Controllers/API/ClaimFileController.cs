using CMCS.Controllers;
using CMCS.Repositories.Interfaces;
using CMCS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/claims")]
public class ClaimFileController : BaseController
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    // activate all the server and repository
    private readonly IClaimFileService _fileService;
    private readonly IInvoiceService _invoiceService;
    private readonly IAuthorizationService _auth;
    private readonly IClaimRepository _claimRepo;
    private readonly IWebHostEnvironment _env;

    public ClaimFileController(
    IUserRepository userRepository,
    IClaimFileService fileService,
    IInvoiceService invoiceService,
    IAuthorizationService auth,
    IClaimRepository claimRepo,
    IWebHostEnvironment env)
    : base(userRepository)
    {
        _fileService = fileService;
        _invoiceService = invoiceService;
        _auth = auth;
        _claimRepo = claimRepo;
        _env = env;
    }


    // upload files
    [HttpPost("{id}/upload")]
    public IActionResult Upload(int id, IFormFile file)
    {
        var claim = _claimRepo.GetById(id);
        // check the authorized user to upload files
        if (!_auth.CanUploadFile(claim, CurrentUser))
            return Forbid();
        //upload file by name of the claim ID
        _fileService.Upload(id, file);
        return Ok();
    }
    // Delete
    [HttpDelete("{id}/{file}")]
    public IActionResult Delete(int id, string file)
    {
        var claim = _claimRepo.GetById(id);
        // check who can delete the file
        if (!_auth.CanDeleteFile(claim, CurrentUser))
            return Forbid();

        _fileService.Delete(id, file);
        return Ok();
    }
    // create the invoice
    [HttpPost("{id}/invoice")]
    public IActionResult GenerateInvoice(int id)
    {
        if (!_auth.CanGenerateInvoice(CurrentUser))
            return Forbid();

        // ensure one invoice only
        foreach (var f in _fileService.GetFiles(id)
            .Where(f => f.EndsWith(".pdf")))
        {
            _fileService.Delete(id, f);
        }

        _invoiceService.GenerateInvoicePdf(id);
        return Ok();
    }
    // download files
    [HttpGet("{id}/download/{file}")]
    public IActionResult Download(int id, string file)
    {
        // ensure the file exist
        var claim = _claimRepo.GetById(id);
        if (claim == null)
            return NotFound();
        // reject unauthorized user download
        if (!_auth.CanViewFile(claim, CurrentUser))
            return Forbid();

        // get the correct file  path 
        var path = Path.Combine(
            _env.WebRootPath,
            "Claims",
            id.ToString(),
            file
        );

        if (!System.IO.File.Exists(path))
            return NotFound();

        return PhysicalFile(//Sends the file from disk
            path,
            "application/octet-stream",
            file
        );
    }

}
