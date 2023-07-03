namespace BlankyBlankLibrary.Services;

public class StructureServices {

    private readonly Data.AppDbContext _db;

    public StructureServices(Data.AppDbContext db) {
        _db = db;
    }

    public IList<ViewModels.StructureList> GetStructures() {
        return _db.Structures.Select(x => new ViewModels.StructureList() {
            Id = x.Id,
            Category = x.Category,
            Structures = x.Structures.Select(y => y.Value).ToList()
        })
        .ToList();
    }

}