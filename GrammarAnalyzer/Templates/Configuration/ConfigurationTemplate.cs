$NameSpaces$

namespace $GrammarName$
{
    //Enumerable which contains the colors we may want to have in our graph
    public enum Color
{
    C_DEFAULT, C_antiquewhite, C_coral, C_darkgoldenrod,
    C_azure, C_crimson, C_gold, C_bisque, C_darksalmon,
    C_goldenrod, C_aliceblue, C_blanchedalmond, C_deeppink,
    C_blue, C_cornsilk, C_firebrick, C_lightgoldenrod,
    C_floralwhite, C_hotpink, C_lightgoldenrodyellow, C_cadetblue,
    C_gainsboro, C_indianred, C_lightyellow, C_cornflowerblue, C_ghostwhite,
    C_lightpink, C_palegoldenrod, C_darkslateblue, C_honeydew, C_lightsalmon,
    C_yellow, C_deepskyblue, C_ivory, C_maroon, C_dodgerblue,
    C_lavender, C_indigo, C_lavenderblush,
    C_lightblue, C_lemonchiffon, C_chartreuse, C_lightskyblue,
    C_linen, C_pink, C_darkgreen, C_lightslateblue, C_red, C_darkolivegreen,
    C_mediumblue, C_mistyrose, C_salmon, C_darkseagreen, C_mediumslateblue,
    C_moccasin, C_tomato, C_forestgreen, C_midnightblue, C_navajowhite,
    C_green, C_navy, C_oldlace, C_greenyellow, C_navyblue,
    C_papayawhip, C_lawngreen, C_powderblue, C_peachpuff, C_beige, C_lightseagreen,
    C_royalblue, C_seashell, C_brown, C_limegreen, C_skyblue, C_snow, C_burlywood,
    C_mediumseagreen, C_slateblue, C_thistle, C_chocolate, C_mediumspringgreen, C_steelblue,
    C_wheat, C_darkkhaki, C_mintcream, C_white, C_khaki, C_olivedrab, C_whitesmoke, C_peru,
    C_palegreen, C_blueviolet, C_rosybrown, C_seagreen, C_darkorchid, C_saddlebrown, C_springgreen,
    C_darkviolet, C_darkslategray, C_sandybrown, C_yellowgreen, C_magenta, C_dimgray, C_sienna,
    C_mediumorchid, C_tan, C_mediumpurple, C_gray, C_aquamarine, C_mediumvioletred, C_lightgray,
    C_cyan, C_orchid, C_lightslategray, C_darkorange, C_darkturquoise, C_palevioletred, C_slategray,
    C_orange, C_lightcyan, C_plum, C_orangered, C_mediumaquamarine, C_purple, C_mediumturquoise, C_violet,
    C_black, C_paleturquoise, C_violetred
};
/// <summary>
///Configuration of the node's type
/// provides specific information according to the node's category
/// the number of contexts that each node has
/// its color and the color's name
/// </summary>

public partial class CNodeTypeConfiguration
{
    public NodeType M_NodeTypeCategory { get; set; }
    public int M_NumberOfContexts { get; set; }

    public Color M_Color { get; set; }
    public string M_ColorName { get; set; }
}
/// <summary>
/// Context type configuration
/// specifies the context's index in addition to the the host nodetype.
/// </summary>
public partial class CContextTypeConfiguration
{

    public int M_ContextIndex { get; set; }

    public NodeType M_HostNodeType { get; set; }

}
/// <summary>
/// Configuration of the node's specifications
/// </summary>
internal static partial class CConfigurationSettings
{
    /// <summary>
    /// All the desired color names we may need 
    /// </summary>
    public static readonly string[] m_colorNames = new[] { "default", "antiquewhite", "coral", "darkgoldenrod",
            "azure", "crimson", "gold", "bisque", "darksalmon",
            "goldenrod", "aliceblue","blanchedalmond", "deeppink",
            "blue","cornsilk", "firebrick", "lightgoldenrod",
            "floralwhite", "hotpink","lightgoldenrodyellow", "cadetblue",
            "gainsboro", "indianred", "lightyellow", "cornflowerblue","ghostwhite",
            "lightpink", "palegoldenrod", "darkslateblue","honeydew", "lightsalmon",
            "yellow", "deepskyblue","ivory", "maroon", "dodgerblue",
            "lavender", "indigo","lavenderblush",
            "lightblue","lemonchiffon",  "chartreuse", "lightskyblue",
            "linen", "pink", "darkgreen", "lightslateblue", "red","darkolivegreen",
            "mediumblue","mistyrose", "salmon", "darkseagreen", "mediumslateblue",
            "moccasin", "tomato", "forestgreen", "midnightblue","navajowhite",
            "green", "navy","oldlace", "greenyellow", "navyblue",
            "papayawhip", "lawngreen", "powderblue", "peachpuff", "beige", "lightseagreen",
            "royalblue","seashell", "brown", "limegreen", "skyblue","snow", "burlywood",
            "mediumseagreen", "slateblue","thistle", "chocolate", "mediumspringgreen", "steelblue",
            "wheat", "darkkhaki", "mintcream", "white", "khaki", "olivedrab", "whitesmoke", "peru",
            "palegreen", "blueviolet", "rosybrown", "seagreen", "darkorchid", "saddlebrown", "springgreen",
            "darkviolet", "darkslategray", "sandybrown", "yellowgreen", "magenta","dimgray", "sienna",
            "mediumorchid","tan","mediumpurple","gray","aquamarine","mediumvioletred","lightgray",
            "cyan","orchid","lightslategray","darkorange","darkturquoise","palevioletred","slategray",
            "orange","lightcyan","plum","orangered","mediumaquamarine","purple","mediumturquoise","violet",
            "black","paleturquoise","violetred"};

    internal static Dictionary<NodeType, CNodeTypeConfiguration> m_nodeTypeConfiguration = new Dictionary<NodeType, CNodeTypeConfiguration>();

    internal static Dictionary<ContextType, CContextTypeConfiguration> m_contextTypeConfiguration = new Dictionary<ContextType, CContextTypeConfiguration>();

    static CConfigurationSettings()
    {
        InitNodeTypeConfigurationSettings();
    }
        
        $InitNodeTypeConfigurationSettings$       
    }
}

