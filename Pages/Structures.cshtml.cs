using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class StructuresModel : PageModel {

    private readonly Services.StructureServices _structureServices;

    public StructuresModel(Services.StructureServices structureServices) {
        _structureServices = structureServices;
    }

    public IList<ViewModels.StructureList> Structures { get; set; } = null!;

    public void OnGet() {
        Structures = _structureServices.GetStructures();
    }
    
}