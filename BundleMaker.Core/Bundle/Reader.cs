using System.Text;
using BundleMaker.Core.Utils;

namespace BundleMaker.Core.Bundles;
public static class BundleReader {
    private enum State {
        None, Key, Value
    }

    public static async Task ReadBuilderIntoAsync(StringReader stream, IList<ILine> output) {
        var text = await stream.ReadToEndAsync();
        await Task.Run(() => { ReadBuilderInto(text, output); });
    }
    public static async Task ReadBundleIntoAsync(StringReader stream, IDictionary<string, string> output) {
        var text = await stream.ReadToEndAsync();
        await Task.Run(() => { ReadBundleInto(text, output); });
    }

    public static async Task ReadBuilderIntoAsync(string text, IList<ILine> output) {
        await Task.Run(() => { ReadBuilderInto(text, output); });
    }
    public static async Task ReadBundleIntoAsync(string text, IDictionary<string, string> output) {
        await Task.Run(() => { ReadBundleInto(text, output); });
    }

    public static void ReadBundleInto(string text, IDictionary<string, string> output) {
        var lines = text.SplitLines().ComposeMultilines();
        var key = new StringBuilder();
        var value = new StringBuilder();
        void Reset() {
            key.Clear();
            value.Clear();
        }
        foreach (var line in lines) {
            if (string.IsNullOrWhiteSpace(line)) continue;
            // Reset all state for current line
            Reset();
            var state = State.None;
            foreach (var c in line) {
                switch (c) {
                    case '=':
                        switch (state) {
                            case State.Key:
                                // If current is key, switch to value
                                state = State.Value;
                                break;
                            case State.Value:
                                // If current is a Value, just add into `value`
                                value.Append(c);
                                break;
                            case State.None:
                                // It could be start with `=`, invalid
                                Reset();
                                goto CUR_LINE_END;
                        }
                        break;
                    case '#':
                        switch (state) {
                            case State.Key:
                                // If current is key, just add `=` into value
                                value.Append(c);
                                break;
                            case State.Value:
                                // If current is a Value, just add `#` into `value`
                                value.Append(c);
                                break;
                            case State.None:
                                // It could be start with `#`, is a comment, omit it
                                Reset();
                                goto CUR_LINE_END;
                        }
                        break;
                    case ' ':
                        switch (state) {
                            case State.Key:
                                // If current is key, key mustn't contain a whitespace, invalid
                                Reset();
                                goto CUR_LINE_END;
                            case State.Value:
                                // If current is a Value, just add whitespace into `value`
                                value.Append(c);
                                break;
                            case State.None:
                                // It's ok, skip this
                                break;
                        }
                        break;
                    default:
                        switch (state) {
                            case State.Key:
                                // If current is key, just add anything into `key`
                                key.Append(c);
                                break;
                            case State.Value:
                                // If current is a Value, just add anything into `value`
                                value.Append(c);
                                break;
                            case State.None:
                                // Not `=`, `whitespace` or `#`, it's a key
                                state = State.Key;
                                key.Append(c);
                                break;
                        }
                        break;
                }
            }
            CUR_LINE_END: ;
            if (key.Length > 0 && value.Length > 0) {
                output[key.ToString().Processed()] = value.ToString().Processed();
            }
            Reset();
        }
    }

    public static void ReadBuilderInto(string text, IList<ILine> output) {
        var key = new StringBuilder();
        var value = new StringBuilder();
        void Reset() {
            key.Clear();
            value.Clear();
        }
        var lines = text.SplitLines().ComposeMultilinesUnresolved();
        // No multiline
        foreach (var (i, line) in lines.Select(it => it.Content).WithIndex()) {
            if (string.IsNullOrWhiteSpace(line)) {// If this line is totally empty
                output.Add(new EmptyLine());
            } else {
                // Reset all state for current line
                Reset();
                var state = State.None;
                foreach (var c in line) {
                    switch (c) {
                        case '=':
                            switch (state) {
                                case State.Key:
                                    // If current is key, switch to value
                                    state = State.Value;
                                    break;
                                case State.Value:
                                    // If current is a Value, just add into `value`
                                    value.Append(c);
                                    break;
                                case State.None:
                                    // It could be start with `=`, invalid
                                    Reset();
                                    goto CUR_LINE_END;
                            }
                            break;
                        case '#':
                            switch (state) {
                                case State.Key:
                                    // If current is key, just add `=` into value
                                    value.Append(c);
                                    break;
                                case State.Value:
                                    // If current is a Value, just add `#` into `value`
                                    value.Append(c);
                                    break;
                                case State.None:
                                    // It could be start with `#`, is a comment
                                    output.Add(new Comment {
                                        Content = i + 1 < line.Length - 1 ? line[(i + 1)..].Processed() : string.Empty
                                    });
                                    Reset();
                                    goto CUR_LINE_END;
                            }
                            break;
                        case ' ':
                            switch (state) {
                                case State.Key:
                                    // If current is key, key mustn't contain a whitespace, invalid
                                    Reset();
                                    goto CUR_LINE_END;
                                case State.Value:
                                    // If current is a Value, just add whitespace into `value`
                                    value.Append(c);
                                    break;
                                case State.None:
                                    // It's ok, skip this
                                    break;
                            }
                            break;
                        default:
                            switch (state) {
                                case State.Key:
                                    // If current is key, just add anything into `key`
                                    key.Append(c);
                                    break;
                                case State.Value:
                                    // If current is a Value, just add anything into `value`
                                    value.Append(c);
                                    break;
                                case State.None:
                                    // Not `=`, `whitespace` or `#`, it's a key
                                    state = State.Key;
                                    key.Append(c);
                                    break;
                            }
                            break;
                    }
                }
                CUR_LINE_END: ;
                if (key.Length > 0 && value.Length > 0) {
                    output.Add(new Pair {
                        Key = key.ToString().Processed(),
                        Value = value.ToString().Processed()
                    });
                }
                Reset();
            }
        }
    }

    private static string Processed(this string text) => text.Trim();

    private static string[] SplitLines(this string text) =>
        text.Replace("\r\n", "\n")
            .Split('\n');

    private class UnresolvedLine {
        public UnresolvedLine() { }
        public UnresolvedLine(string oneLine) {
            LinesContained = new List<string> { oneLine };
        }
        private bool IsMultiline {
            get => LinesContained.Count > 1;
        }
        public bool HasLine {
            get => LinesContained.Count > 0;
        }
        public string Content {
            get => IsMultiline ? ToString() : LinesContained[0];
        }
        public IList<string> LinesContained {
            get;
            init;
        }
        public override string ToString() {
            return string.Join("", LinesContained);
        }
    }

    private static IEnumerable<UnresolvedLine> ComposeMultilinesUnresolved(this IEnumerable<string> lines) {
        var res = new LinkedList<UnresolvedLine>();
        var cache = new List<string>();
        var isMultiline = false;
        foreach (var line in lines) {
            if (line.EndsWith('\\')) {
                cache.Add(line.TrimEnd('\\'));
                isMultiline = true;
            } else {
                if (isMultiline) {
                    cache.Add(" " + line.TrimStart());
                    isMultiline = line.EndsWith('\\');
                    if (!isMultiline) {//If it's no longer multiline
                        res.AddLast(new UnresolvedLine { LinesContained = cache });
                        cache = new();
                    }
                } else {
                    res.AddLast(new UnresolvedLine(line));
                    cache.Clear();// Just clear, prevent any exception
                }
            }
        }
        // If the last line is still a multiline, it's valid and would be omitted.
        return res;
    }
    private static IEnumerable<string> ComposeMultilines(this IEnumerable<string> lines) {
        var res = new LinkedList<string>();
        var cache = new StringBuilder();
        var isMultiline = false;
        foreach (var line in lines) {
            if (line.EndsWith('\\')) {
                cache.Append(line.TrimEnd('\\'));
                isMultiline = true;
            } else {
                if (isMultiline) {
                    cache.Append(' ');
                    cache.Append(line.TrimStart());
                    isMultiline = line.EndsWith('\\');
                    if (!isMultiline) {//If it's no longer multiline
                        res.AddLast(cache.ToString());
                        cache.Clear();
                    }
                } else {
                    res.AddLast(line);
                    cache.Clear();// Just clear, prevent any exception
                }
            }
        }
        // If the last line is still a multiline, it's valid and would be omitted.
        return res;
    }
}