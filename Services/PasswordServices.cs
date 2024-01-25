using System.Text;
using System.Text.RegularExpressions;
using BlankyBlankLibrary.Extensions;
using Newtonsoft.Json;

namespace BlankyBlankLibrary.Services;

public class PasswordServices {

    private readonly Data.AppDbContext _db;

    public PasswordServices (Data.AppDbContext db) => _db = db;

    public async Task ImportPasswords (IFormFile passwordsFile) {

        ViewModels.PasswordsContainer passwordsContainer = JsonConvert.DeserializeObject<ViewModels.PasswordsContainer>(await passwordsFile.ReadFileContents()) ?? throw new Exception("What're you feeding me?");

        var trackedDbPasswords = new Dictionary<string, Data.Models.Passwords.Password>();
        
        foreach (var password in passwordsContainer.Passwords) {

            Data.Models.Passwords.Password? dbPassword;
            
            if (!trackedDbPasswords.TryGetValue(password.Name, out dbPassword)) {
                dbPassword = _db.Passwords.SingleOrDefault(pw => pw.Name == password.Name);
                if (dbPassword == null) {
                    dbPassword = new Data.Models.Passwords.Password() {
                        LegacyId = Convert.ToInt32(password.Id),
                        Name = password.Name
                    };
                    _db.Passwords.Add(dbPassword);
                }
                trackedDbPasswords.Add(password.Name, dbPassword);
            }

        }

        await _db.SaveChangesAsync();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ViewModels.FileContainer ExportPasswords () {

        ViewModels.PasswordsContainer container = new ViewModels.PasswordsContainer {
            Passwords = _db.Passwords
                .Select(password => new ViewModels.PasswordsContainer.Password() {
                    Id = password.Id.ToString(),
                    Name = password.Name
                }).ToList()
        };

        var json = JsonConvert.SerializeObject(container);

        return new ViewModels.FileContainer() {
            FileContents = Encoding.UTF8.GetBytes(json),
            FileContentType = "application/json",
            FileName = "BlankyBlankPasswords.jet"
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetPasswordsCount () => _db.Passwords.Count();

}