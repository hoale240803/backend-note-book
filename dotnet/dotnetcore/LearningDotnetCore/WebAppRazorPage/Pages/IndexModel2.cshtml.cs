using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazorPage.Pages;

public class IndexModel2 : PageModel, IDisposable
{
    private readonly ILogger _logger;
    private readonly IOperationTransient _transientOperation;
    private readonly IOperationScoped _scopedOperation;
    private readonly IOperationSingleton _singletonOperation;

    public IndexModel2(ILogger<IndexModel2> logger,
        IOperationTransient transientOperation,
        IOperationScoped scopedOperation,
        IOperationSingleton singletonOperation)
    {
        _logger = logger;
        _transientOperation = transientOperation;
        _scopedOperation = scopedOperation;
        _singletonOperation = singletonOperation;
        
        _logger.LogInformation("IndexModel2 Constructor - Transient: {TransientId}, Scoped: {ScopedId}, Singleton: {SingletonId}",
            _transientOperation.OperationId,
            _scopedOperation.OperationId,
            _singletonOperation.OperationId);
    }

    public void OnGet()
    {
        _logger.LogInformation("OnGet - Transient: {TransientId}, Scoped: {ScopedId}, Singleton: {SingletonId}",
            _transientOperation.OperationId,
            _scopedOperation.OperationId,
            _singletonOperation.OperationId);
    }

    public void Dispose()
    {
        Debugger.Break();
        _logger.LogInformation("IndexModel2 Disposing - Transient: {TransientId}, Scoped: {ScopedId}, Singleton: {SingletonId}",
            _transientOperation.OperationId,
            _scopedOperation.OperationId,
            _singletonOperation.OperationId);
    }
}