using PKHeX.Core;

//Most of functions are taken from pkhex
//https://github.com/kwsch/PKHeX/blob/master/PKHeX.WinForms/Util/WinFormsTranslator.cs

namespace TeraFinder.Plugins;

public static class WinFormsTranslator
{
    private static readonly Dictionary<string, TranslationContext> Context = [];
    public static void TranslateInterface(this Control form, string lang) => TranslateForm(form, GetContext(lang));
    public static Dictionary<string, string> TranslateInnerStrings(this Dictionary<string, string> strings, string lang) => TranslateStrings(strings, GetContext(lang));

    private static string GetTranslationFileNameInternal(string lang) => $"lang_{lang}";
    private static string GetTranslationFileNameExternal(string lang) => $"lang_{lang}.txt";

    public static IReadOnlyDictionary<string, string> GetDictionary(string lang) => GetContext(lang).Lookup;

    private static TranslationContext GetContext(string lang)
    {
        if (Context.TryGetValue(lang, out var context))
            return context;

        var lines = GetTranslationFile(lang);
        Context.Add(lang, context = new TranslationContext(lines));
        return context;
    }

    private static Dictionary<string, string> TranslateStrings(Dictionary<string, string> strings, TranslationContext context)
    {
        var dic = new Dictionary<string, string>();
        foreach(var strs in strings)
        {
            var val = context.GetTranslatedText(strs.Key, strs.Value);
            dic.Add(strs.Key, val ?? strs.Value);
        }
        return dic;
    }

    private static void TranslateForm(Control form, TranslationContext context)
    {
        form.SuspendLayout();

        // Translate Title
        var formName = form.Name;
        formName = GetSaneFormName(formName);
        form.Text = context.GetTranslatedText(formName, form.Text);

        // Translate Controls
        var translatable = GetTranslatableControls(form);
        foreach (var c in translatable)
            TranslateControl(c, context, formName);

        form.ResumeLayout();
    }

    internal static void TranslateControls(IEnumerable<Control> controls)
    {
        foreach (var c in controls)
        {
            foreach (var context in Context.Values)
                context.GetTranslatedText(c.Name, c.Text);
        }
    }

    private static string GetSaneFormName(string formName)
    {
        // Strip out generic form names
        var degen = formName.IndexOf('`');
        if (degen != -1)
            formName = formName[..degen];

        return formName;
    }

    private static void TranslateControl(object c, TranslationContext context, string formname)
    {
        if (c is Control r)
        {
            var current = r.Text;
            var updated = context.GetTranslatedText($"{formname}.{r.Name}", current);
            if (!ReferenceEquals(current, updated))
                r.Text = updated;
        }
        else if (c is ToolStripItem t)
        {
            var current = t.Text;
            var updated = context.GetTranslatedText($"{formname}.{t.Name}", current);
            if (!ReferenceEquals(current, updated))
                t.Text = updated;
        }
    }

    private static ReadOnlySpan<string> GetTranslationFile(string lang)
    {
        var file = GetTranslationFileNameInternal(lang);
        // Check to see if a the translation file exists in the same folder as the executable
        string externalLangPath = GetTranslationFileNameExternal(file);
        if (File.Exists(externalLangPath))
        {
            try { return File.ReadAllLines(externalLangPath); }
            catch { /* In use? Just return the internal resource. */ }
        }

        var txt = Core.ResourcesUtil.GetTextResource(file);
        return Util.GetStringList(file, txt);
    }

    private static IEnumerable<object> GetTranslatableControls(Control f)
    {
        foreach (var z in f.GetChildrenOfType<Control>())
        {
            switch (z)
            {
                case ToolStrip menu:
                    foreach (var obj in GetToolStripMenuItems(menu))
                        yield return obj;

                    break;
                default:
                    if (string.IsNullOrWhiteSpace(z.Name))
                        break;

                    if (z.ContextMenuStrip != null) // control has attached MenuStrip
                    {
                        foreach (var obj in GetToolStripMenuItems(z.ContextMenuStrip))
                            yield return obj;
                    }

                    if (z is ListControl or TextBoxBase or LinkLabel or NumericUpDown or ContainerControl)
                        break; // undesirable to modify, ignore

                    if (!string.IsNullOrWhiteSpace(z.Text))
                        yield return z;
                    break;
            }
        }
    }

    private static IEnumerable<T> GetChildrenOfType<T>(this Control control) where T : class
    {
        foreach (var child in control.Controls.OfType<Control>())
        {
            if (child is T childOfT)
                yield return childOfT;

            if (!child.HasChildren) continue;
            foreach (var descendant in GetChildrenOfType<T>(child))
                yield return descendant;
        }
    }

    private static IEnumerable<object> GetToolStripMenuItems(ToolStrip menu)
    {
        foreach (var i in menu.Items.OfType<ToolStripMenuItem>())
        {
            if (!string.IsNullOrWhiteSpace(i.Text))
                yield return i;
            foreach (var sub in GetToolsStripDropDownItems(i).Where(z => !string.IsNullOrWhiteSpace(z.Text)))
                yield return sub;
        }
    }

    private static IEnumerable<ToolStripMenuItem> GetToolsStripDropDownItems(ToolStripDropDownItem item)
    {
        foreach (var dropDownItem in item.DropDownItems.OfType<ToolStripMenuItem>())
        {
            yield return dropDownItem;
            if (!dropDownItem.HasDropDownItems)
                continue;
            foreach (ToolStripMenuItem subItem in GetToolsStripDropDownItems(dropDownItem))
                yield return subItem;
        }
    }
}

public sealed class TranslationContext
{
    public bool AddNew { private get; set; }
    public bool RemoveUsedKeys { private get; set; }
    public const char Separator = '=';
    private readonly Dictionary<string, string> Translation = [];
    public IReadOnlyDictionary<string, string> Lookup => Translation;

    public TranslationContext(ReadOnlySpan<string> content, char separator = Separator)
    {
        foreach (var line in content)
            LoadLine(line, separator);
    }

    private void LoadLine(ReadOnlySpan<char> line, char separator = Separator)
    {
        var split = line.IndexOf(separator);
        if (split < 0)
            return; // ignore
        var key = line[..split].ToString();
        var value = line[(split + 1)..].ToString();
        Translation.TryAdd(key, value);
    }

    public string? GetTranslatedText(string val, string? fallback)
    {
        if (RemoveUsedKeys)
            Translation.Remove(val);

        if (Translation.TryGetValue(val, out var translated))
            return translated;

        if (fallback != null && AddNew)
            Translation.Add(val, fallback);
        return fallback;
    }
}
