using System.Text;
using System.Text.RegularExpressions;
using BlankyBlankLibrary.Extensions;
using Newtonsoft.Json;

namespace BlankyBlankLibrary.Services;

public class WordServices {

    private readonly Data.AppDbContext _db;

    public WordServices (Data.AppDbContext db) => _db = db;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="wordListFile"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ImportWordList (IFormFile wordListFile) {

        // Deserialize into WordList
        ViewModels.WordListsContainer incomingWordLists = JsonConvert.DeserializeObject<ViewModels.WordListsContainer>(await wordListFile.ReadFileContents()) ?? throw new Exception("What're you feeding me?");

        // Regex formats for incoming data. Expecting
        // word-values-like-this
        var wordsWordFormat = new Regex(@"^[^<>]+$");
        var wordsListFormat = new Regex(@"^<[a-z0-9-\s]*>$");

        // Check for unknown formats in incoming word lists
        // Anything with words not matching either of the above
        // regexes will be here.
        {
            var wordsWithUnknownFormats = incomingWordLists.WordLists
                .Where(wl => wl.Words.Any(w => !wordsWordFormat.IsMatch(w.WordName) && !wordsListFormat.IsMatch(w.WordName)));
            if (wordsWithUnknownFormats.Any()) throw new Exception("Unknown format(s) found in list words.");
        }

        var trackedDbWordLists = new Dictionary<string, Data.Models.WordLists.List>();
        
        foreach (var wordList in incomingWordLists.WordLists) {

            Data.Models.WordLists.List? dbWordList;
            if (!trackedDbWordLists.TryGetValue(wordList.Name.ToLower(), out dbWordList)) {
                dbWordList = _db.WordLists.SingleOrDefault(wl => wl.Name == wordList.Name);
                if (dbWordList == null) {
                    dbWordList = new Data.Models.WordLists.List() {
                        LegacyId = Convert.ToInt32(wordList.Id),
                        Name = wordList.Name,
                        Amount = string.IsNullOrEmpty(wordList.Amount) ? null : Convert.ToInt32(wordList.Amount),
                        MaxChoices = string.IsNullOrEmpty(wordList.MaxChoices) ? null : Convert.ToInt32(wordList.MaxChoices),
                        Optional = wordList.Optional,
                        Placeholder = string.IsNullOrEmpty(wordList.Placeholder) ? null : wordList.Placeholder,
                        Words = wordList.Words
                            .Where(w => wordsWordFormat.IsMatch(w.WordName))
                            .Select(w => new Data.Models.WordLists.ListWord() {
                                WordName = w.WordName,
                                AlwaysChoose = w.AlwaysChoose
                            })
                            .ToList()
                    };
                    _db.WordLists.Add(dbWordList);
                }
                trackedDbWordLists.Add(wordList.Name, dbWordList);
            }

        }

        // Populate the list's lists now that we've tracked all the lists
        foreach (var wordList in trackedDbWordLists.Values) {

            var incomingWordList = incomingWordLists.WordLists.Single(list => list.Name == wordList.Name);
            wordList.Lists = incomingWordList.Words
                // <gadget> is referenced as a list but the list is never defined so ignore it
                .Where(word => wordsListFormat.IsMatch(word.WordName) && word.WordName != "<gadget>")
                .Select(word => new Data.Models.WordLists.ListList() {
                    // There's an <is in relation to> that I assume is supposed to be
                    // referencing list with name "is-in-relation-to" ... maybe the
                    // game doesn't care about dashes but dangit! I do so replace
                    // spaces with dashes in list references to match formatting.
                    List = trackedDbWordLists[word.WordName.Substring(1, word.WordName.Length - 2).Replace(' ', '-')],
                    AlwaysChoose = word.AlwaysChoose
                })
                .ToList();

        }

        await _db.SaveChangesAsync();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ViewModels.FileContainer ExportWordList () {

        ViewModels.WordListsContainer container = new ViewModels.WordListsContainer
        {
            WordLists = _db.WordLists
                .OrderBy(word_list => word_list.LegacyId)
                .Select(word_list => new ViewModels.WordListsContainer.WordList()
                {
                    Id = word_list.Id.ToString()!,
                    Name = word_list.Name,
                    Amount = word_list.Amount == null ? "" : word_list.Amount.ToString(),
                    MaxChoices = word_list.MaxChoices.ToString()!,
                    Optional = word_list.Optional,
                    Placeholder = word_list.Placeholder ?? ""
                })
                .ToList()
        };

        foreach (var wordList in container.WordLists) {

            var words = _db.WordLists
                .Where(word_list => word_list.Name == wordList.Name)
                .SelectMany(word_list => word_list.Words)
                .Select(word => new ViewModels.WordListsContainer.WordList.Word() {
                    WordName = word.WordName,
                    AlwaysChoose = word.AlwaysChoose
                })
                .ToList();

            var lists = _db.WordLists
                .Where(word_list => word_list.Name == wordList.Name)
                .SelectMany(word_list => word_list.Lists)
                .Select(list => new ViewModels.WordListsContainer.WordList.Word() {
                    WordName = $"<{list.List.Name}>",
                    AlwaysChoose = list.AlwaysChoose
                })
                .ToList();

            wordList.Words = words.Concat(lists);

        }

        var json = JsonConvert.SerializeObject(container);

        return new ViewModels.FileContainer() {
            FileContents = Encoding.UTF8.GetBytes(json),
            FileContentType = "application/json",
            FileName = "BlankyBlankWordLists.jet"
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetWordListCount () => _db.WordLists.Count();

}