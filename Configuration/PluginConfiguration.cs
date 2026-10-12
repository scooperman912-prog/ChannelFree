using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.QuasiTV.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>Secret that must be present (?token=) on the M3U, guide, stream and remote URLs.</summary>
    public string StreamToken { get; set; } = string.Empty;

    /// <summary>How Jellyfin reaches itself (from inside its own container).</summary>
    public string InternalUrl { get; set; } = "http://localhost:8096";

    /// <summary>Video bitrate for channel streams, in kbps.</summary>
    public int VideoBitrateKbps { get; set; } = 4000;

    /// <summary>Max output height (720 keeps the server + Roku happy).</summary>
    public int MaxHeight { get; set; } = 720;

    /// <summary>Encode channel streams on the GPU: VA-API when /dev/dri is available, VideoToolbox on macOS.</summary>
    public bool UseHardwareEncoding { get; set; } = true;

    /// <summary>VA-API render device.</summary>
    public string VaapiDevice { get; set; } = "/dev/dri/renderD128";

    /// <summary>After a Record-jump, leaving the channel within this many minutes reverts it.</summary>
    public int RevertWindowMinutes { get; set; } = 10;

    /// <summary>Per-channel schedule shifts created by Restart / Skip.</summary>
    public List<ChannelShift> Shifts { get; set; } = new();

    /// <summary>Editor changes per channel (enable, rename, renumber, added/removed titles).</summary>
    public List<ChannelOverride> Overrides { get; set; } = new();

    /// <summary>1 = built-in channel names migrated (0.9.2 generic defaults; older installs keep their old names).</summary>
    public int NamesVersion { get; set; }

    /// <summary>False on a fresh install until the starter lineup has been picked from the library.</summary>
    public bool StarterLineupDone { get; set; }

    /// <summary>Channels you created yourself.</summary>
    public List<CustomChannel> CustomChannels { get; set; } = new();

    /// <summary>Genre channels appear when a movie genre has at least this many titles.</summary>
    public int MinGenreTitles { get; set; } = 15;

    /// <summary>Network channels appear when a network has at least this many shows.</summary>
    public int MinNetworkShows { get; set; } = 4;

    /// <summary>Cable-box style info banner (channel, title, times, progress) when tuning in.</summary>
    public bool ShowBanner { get; set; } = true;

    public int BannerSeconds { get; set; } = 5;

    /// <summary>Time zone for times shown in the banner.</summary>
    public string TimeZoneId { get; set; } = string.Empty; // empty = the server's own time zone

    /// <summary>Preferred audio languages for channel streams, in order (ISO codes, comma-separated). Files whose first track is another language switch to these.</summary>
    public string PreferredAudioLanguages { get; set; } = "eng,en";

    // ---- credits (0.8)

    /// <summary>Cut end credits short when a credits marker exists (Intro Skipper / "Credits" chapter).</summary>
    public bool TrimCredits { get; set; } = true;

    /// <summary>Seconds of TV end credits kept before fading out.</summary>
    public int CreditsKeepSecondsTv { get; set; } = 10;

    /// <summary>Seconds of movie end credits kept before fading out.</summary>
    public int CreditsKeepSecondsMovie { get; set; } = 60;

    /// <summary>Shows / movies whose credits are always played in full (series id or movie id).</summary>
    public List<Guid> KeepCredits { get; set; } = new();

    // ---- bumpers (0.8)

    /// <summary>"Up Next" bumper between programs (not between episodes of the same show).</summary>
    public bool ShowBumpers { get; set; } = true;

    public int BumperSeconds { get; set; } = 5;

    /// <summary>Stinger volume change in dB (negative = quieter).</summary>
    public int BumperVolumeDb { get; set; } = -14;

    // ---- web / LG remote (0.8)

    /// <summary>Web + LG: first Down press on a channel shows a mini info banner; the next press opens the normal controls.</summary>
    public bool DownArrowBanner { get; set; } = true;
}

/// <summary>Editor overrides for one channel (by key).</summary>
public class ChannelOverride
{
    public string Key { get; set; } = string.Empty;

    /// <summary>null = default (core/custom on, auto channels off).</summary>
    public bool? Enabled { get; set; }

    public string? Name { get; set; }

    public int? Number { get; set; }

    /// <summary>"shuffle" (default) or "inorder" (release date, looping).</summary>
    public string? Order { get; set; }

    /// <summary>Episodes played back-to-back when a show comes up (default 1).</summary>
    public int? EpisodesInARow { get; set; }

    /// <summary>Per-show overrides of <see cref="EpisodesInARow"/>.</summary>
    public List<ShowBlock> ShowBlocks { get; set; } = new();

    /// <summary>Weighted mix: titles earlier in <see cref="Ranking"/> come up more often.</summary>
    public bool Weighted { get; set; }

    /// <summary>Your left-to-right order for the weighted mix (titles not listed go last).</summary>
    public List<Guid> Ranking { get; set; } = new();

    /// <summary>Titles added by hand (movie, show or episode ids).</summary>
    public List<Guid> Added { get; set; } = new();

    /// <summary>Titles removed from this channel (survive rebuilds).</summary>
    public List<Guid> Removed { get; set; } = new();

    /// <summary>Whole genres added, as "scope:Genre" where scope = movies | shows | all.</summary>
    public List<string> IncludeGenres { get; set; } = new();

    /// <summary>Genres kept off this channel (by genre name). Titles added one by one still stay.</summary>
    public List<string> ExcludeGenres { get; set; } = new();

    /// <summary>Whole keyword tags added (TMDB keywords / your own tags), as "scope:tag".</summary>
    public List<string> IncludeTags { get; set; } = new();

    /// <summary>Keyword tags kept off this channel.</summary>
    public List<string> ExcludeTags { get; set; } = new();

    /// <summary>Whole studios / networks added, as "scope:Studio".</summary>
    public List<string> IncludeStudios { get; set; } = new();

    /// <summary>Studios / networks kept off this channel.</summary>
    public List<string> ExcludeStudios { get; set; } = new();
}

/// <summary>How many episodes of one show to play in a row on a channel.</summary>
public class ShowBlock
{
    public Guid SeriesId { get; set; }

    public int Count { get; set; }
}

/// <summary>A hand-made channel.</summary>
public class CustomChannel
{
    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Number { get; set; }
}

/// <summary>How far a channel's schedule has been moved (ticks; positive = later).</summary>
public class ChannelShift
{
    public string ChannelId { get; set; } = string.Empty;

    public long ShiftTicks { get; set; }
}
