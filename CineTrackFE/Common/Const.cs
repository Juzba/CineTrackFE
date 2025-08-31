using System.Text.RegularExpressions;

namespace CineTrackFE.Common;

public static class Const
{
    public const string MainRegion = "MainRegion";
    public const string AdminRegion = "AdminRegion";


    public const string FilmId = "FilmId";

    public static readonly Regex OnlyLettersRegex = new("^[a-zA-Z]+$");





}
