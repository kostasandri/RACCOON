/*
MIT License

Copyright(c) [2016] [Grigoris Dimitroulakos]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;

namespace GrammarAnalyzer
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
        /// <summary>
        /// sets the node's color
        /// </summary>
        /// <param name="nodeType">Type of node</param>
        /// <param name="color">The color that the node will have</param>
/*        public static void SetNodeCategoryColor(NodeType nodeType, Color color) {
            switch (nodeType) {
                case NodeType.CAT_REGEXP_OPERATORS:
                case NodeType.CAT_REGEXP_BASIC:
                case NodeType.CAT_ASSERTIONS:
                case NodeType.CAT_LEAFS:
                    foreach (KeyValuePair<NodeType, CNodeTypeConfiguration> record in m_nodeTypeConfiguration) {
                        if (record.Value.M_NodeTypeCategory == nodeType) {
                            record.Value.M_Color = color;
                            record.Value.M_ColorName = CConfigurationSettings.m_colorNames[(int)color];
                        }
                    }
                    break;
                default:
                    if (color != Color.C_DEFAULT) {
                        m_nodeTypeConfiguration[nodeType].M_Color = color;
                        m_nodeTypeConfiguration[nodeType].M_ColorName = CConfigurationSettings.m_colorNames[(int)color];
                    }
                    break;
            }
        }*/
        /// <summary>
        /// Configures each node category according to the specifications we declared
        /// First configures the node's category and secondly the node's context type specifications
        /// </summary>
        private static void InitNodeTypeConfigurationSettings()
        {
            Console.Write("Initializing AST configuration...");
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_COMPILEUNIT] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 2, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = "default", M_Color = Color.C_DEFAULT };
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_GRAMMAR_SPEC] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 2, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = "default", M_Color = Color.C_DEFAULT };
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_GRAMMAR_RULE] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 2, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = "green", M_Color = Color.C_green };
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_CLOSURE] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 1, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = "blue", M_Color = Color.C_blue };
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_GRAMMAR_RULE_RHS] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 1, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = "default", M_Color = Color.C_DEFAULT };
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_RHS_PARENTHESIZED_TERM] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 1, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = "green", M_Color = Color.C_green };

            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_TERMINAL] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 0, M_NodeTypeCategory = NodeType.NT_LEAF, M_ColorName = "orange", M_Color = Color.C_orange };
            CConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_NONTERMINAL] = new CNodeTypeConfiguration()
            { M_NumberOfContexts = 1, M_NodeTypeCategory = NodeType.NT_LEAF, M_ColorName = "red", M_Color = Color.C_red };

            //regexpstatement configuration
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_COMPILEUNIT_PROLOGUE] = new CContextTypeConfiguration()
            { M_ContextIndex = 0, M_HostNodeType = NodeType.NT_COMPILEUNIT };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_COMPILEUNIT_GRAMMAR_RULE] = new CContextTypeConfiguration()
            { M_ContextIndex = 1, M_HostNodeType = NodeType.NT_COMPILEUNIT };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_GRAMMARSPEC_ID] = new CContextTypeConfiguration()
            { M_ContextIndex = 1, M_HostNodeType = NodeType.NT_GRAMMAR_SPEC };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_GRAMMARSPEC_TYPE] = new CContextTypeConfiguration()
            { M_ContextIndex = 0, M_HostNodeType = NodeType.NT_GRAMMAR_SPEC };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_GRAMMARRULE_NON_TERMINAL] = new CContextTypeConfiguration()
            { M_ContextIndex = 0, M_HostNodeType = NodeType.NT_GRAMMAR_RULE };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_GRAMMARRULE_GRAMMAR_RULE_ALTERNATIVES] = new CContextTypeConfiguration()
            { M_ContextIndex = 1, M_HostNodeType = NodeType.NT_GRAMMAR_RULE };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_GRAMMARRULERHS_ALTERNATIVETERMS] = new CContextTypeConfiguration()
            { M_ContextIndex = 0, M_HostNodeType = NodeType.NT_GRAMMAR_RULE_RHS };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_CLOSURE_TERM] = new CContextTypeConfiguration()
            { M_ContextIndex = 0, M_HostNodeType = NodeType.NT_CLOSURE };
            CConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_RHSPARENTHESIZEDTERM_OR_TERMS] = new CContextTypeConfiguration()
            { M_ContextIndex = 0, M_HostNodeType = NodeType.NT_RHS_PARENTHESIZED_TERM };


            Console.WriteLine("OK");
            /* using System.Diagnostics;

             Trace.Write("Hello World");
             Trace.WriteLine("Hello World");
             Debug.Write("Hello World2");
             Debug.WriteLine("Hello World2");
             */
            //---------mexri edw------------------------//

        }

    }
}
