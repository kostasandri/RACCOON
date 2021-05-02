using Antlr4.Runtime;
using GrammarAnalyzer.Info_Collection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GrammarAnalyzer
{
    class RaccoonFileGenerator : EBNFParserBaseVisitor<Int32>
    {
        private ParserRuleContext m_root;
        private string m_grammarID;

        private Dictionary<string, List<string>>
            m_nonTerminals_Contexts =
                new Dictionary<string, List<string>>(); // key: contains the non terminal nodes list: maps the contexts of the node

        private Dictionary<string, List<string>>
            m_override =
                new Dictionary<string, List<string>>(); //key:contains terminal node and maps the list: of overrided nodes

        Dictionary<string, List<string>> NT_CT_O = new Dictionary<string, List<string>>();
        List<int> key_pos = new List<int>();

        HashSet<string> leaf_nodes = new HashSet<string>();

        public RaccoonFileGenerator(ParserRuleContext root)
        {
            m_root = root;
        }

        /// <summary>
        /// initiates abstract syntax tree specification generation
        /// </summary>
        public void Generate()
        {
            PathCreator();
            GenerateAstComposite(); //done
            GenerateAstGeneration(); //done
            GenerateAstCompositeConcrete(); //done
            GenerateConfiguration(); //done
            GenerateAbstractConcreteIteratorFactory(); //done
            GenerateAstConcreteVisitor(); //done
            GenerateCAstAbstractIteratorEvents();
            GenerateAbstractGenericIteratorFactory();
            GenerateAbstractIterator();
            GenerateAbstractASTVistor();
            GenerateConcreteIterator();
            GenerateASTPrinter();
        }

        private void PathCreator()
        {
            string[] path =
            {
                "..\\..\\bin\\Debug\\Generated\\Composite",
                "..\\..\\bin\\Debug\\Generated\\Iterator",
                "..\\..\\bin\\Debug\\Generated\\Visitors",
                "..\\..\\bin\\Debug\\Generated\\Factory",
                "..\\..\\bin\\Debug\\Generated\\Configuration",
                "..\\..\\bin\\Debug\\Generated\\Events",
                "..\\..\\bin\\Debug\\Generated\\ASTGeneration",
                "..\\..\\bin\\Debug\\Generated\\ASTPrinter\\"
            };
            try
            {
                foreach (string pth in path)
                {
                    if (!Directory.Exists(pth))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pth);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        private void GenerateAstComposite()
        {
            // aquire grammar name
            ASTCompositeInfoCollectorVisitor info = new ASTCompositeInfoCollectorVisitor();
            info.Visit(m_root);

            // open template file
            StreamReader gen = new StreamReader("..\\..\\Templates\\Composite\\ASTCompositeTemplate.cs");
            StreamWriter newFile = new StreamWriter("Generated\\Composite\\ASTComposite_" + info.M_GrammarID + ".cs");
            string line;
            m_grammarID = info.M_GrammarID;
            Regex rgx = new Regex(@"\$GrammarName\$");

            //substitute occurencies of placeholder with grammarname
            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else
                {
                    string result = rgx.Replace(line, m_grammarID);
                    newFile.WriteLine(result);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAstCompositeConcrete()
        {
            ASTCompoisiteConcreteTerminalCollector info1 = new ASTCompoisiteConcreteTerminalCollector();
            info1.Visit(m_root);
            ASTCompoisiteConcreteTerminalCollector.setContexts();

            List<int> positions = new List<int>();
            //HashSet<string> leaf_nodes = new HashSet<string>();

            //eisagwgi stoixeiwn sto dictionary m_nonTerminal_Contexts
            foreach (var key in info1.M_Terminals.Reverse())
            {
                List<string> m_cont = new List<string>();

                for (int i = key.Value.Count - 1; i >= 0; i--)
                {
                    m_cont.Add(key.Value[i]);
                }

                m_nonTerminals_Contexts.Add(key.Key, m_cont);
            }

            foreach (var key1 in m_nonTerminals_Contexts) //find duplicate contexts
            {
                for (int i = key1.Value.Count - 1; i > 0; i--)
                {
                    string[] x = key1.Value[i].Split('_');

                    for (int k = key1.Value.Count - 1; k >= 0; k--)
                    {
                        if (key1.Value[k].Contains(key1.Value[i]) && i != k)
                        {
                            positions.Add(i);
                        }
                    }
                }

                positions = positions.Distinct().ToList();
                foreach (var pos in positions)
                {
                    key1.Value.RemoveAt(pos);
                }

                positions.Clear();
            }

            // open template file
            StreamReader gen = new StreamReader("..\\..\\Templates\\Composite\\ASTCompositeConcreteTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Composite\\ASTCompositeConcrete_" + m_grammarID + ".cs");
            string line;
            string lineoutput = m_grammarID;

            //substitute occurrences of placeholder with grammarname
            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                }
                else if (line.Contains("$GrammarName$")) //grammarname
                {
                    line = line.Replace("$GrammarName$", lineoutput);
                    newFile.WriteLine(line);
                }

                #region nodetype generation

                else if (line.Contains("$NodeType$"))
                {
                    newFile.WriteLine("\tpublic enum NodeType{");
                    newFile.WriteLine(@"
        /// <summary>
        /// This enumeration carries the different types of AST Nodes. The numbers indicate the index in the
        /// m_contextMappings table of tuples where each specific context starts
        /// </summary>
        // NON-TERMINAL NODES
                                    ");

                    int numOfContexts = 0;
                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                newFile.WriteLine("\t\tNT_{0} = {1}, ", key_value.ToUpper(), numOfContexts);
                                numOfContexts += var.Value.Count;
                            }
                        }
                    }

                    #region NT_leafs

                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                for (int k = 0; k < var.Value.Count; k++)
                                {
                                    string[] splitter = var.Value[k].Split(':');

                                    if (splitter[1].Equals("Terminal"))
                                    {
                                        if ((var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                             var.Value[k].StartsWith("*")) &&
                                            !leaf_nodes.Contains(splitter[0].Substring(1)))
                                        {
                                            newFile.WriteLine("\t\tNT_{0} = {1},", splitter[0].ToUpper(),
                                                numOfContexts);
                                            numOfContexts++;
                                            leaf_nodes.Add(splitter[0].Substring(1));
                                        }
                                        else if (!(var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                                   var.Value[k].StartsWith("*")) && !leaf_nodes.Contains(splitter[0]))
                                        {
                                            newFile.WriteLine("\t\tNT_{0} = {1},", splitter[0].ToUpper(),
                                                numOfContexts);
                                            numOfContexts++;
                                            leaf_nodes.Add(splitter[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    newFile.WriteLine("\t\tNT_NA = {0},\n\t\tNT_LEAF", numOfContexts);
                    newFile.WriteLine("\t}");
                }

                #endregion

                #region context type generation

                else if (line.Contains("$ContextType$"))
                {
                    newFile.WriteLine("\tpublic enum ContextType{");


                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                for (int k = 0; k < var.Value.Count; k++)
                                {
                                    if (var.Value[k].StartsWith("*") || var.Value[k].StartsWith("+") ||
                                        var.Value[k].StartsWith("?"))
                                    {
                                        string[] value = var.Value[k].Split(':');
                                        newFile.WriteLine("\t\tCT_{0}_{1},", key_value.ToUpper(),
                                            value[0].Substring(1).ToUpper());
                                    }
                                    else
                                    {
                                        string[] value = var.Value[k].Split(':');
                                        if (var.Value[k].StartsWith("?"))
                                        {
                                            newFile.WriteLine("\t\tCT_{0}_{1},", key_value.ToUpper(),
                                                value[0].Replace("?", "").ToUpper());
                                        }
                                        else
                                        {
                                            newFile.WriteLine("\t\tCT_{0}_{1},", key_value.ToUpper(),
                                                value[0].ToUpper());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    /*foreach (var key in m_nonTerminals_Contexts)
                    {

                        newFile.WriteLine();
                        for (int i = 0; i < key.Value.Count; i++)
                        {
                            var pieces = key.Value[i].Split(new[] { '_' }, 2);
                            newFile.WriteLine("\t\t{0},", pieces[0] + "_" + key.Key.Replace("NT_", "").ToUpper() + "_" + pieces[1].ToUpper());
                        }
                    }*/

                    newFile.WriteLine("\t\tCT_NA");
                    newFile.WriteLine("\t}");
                }

                #endregion

                #region Function generation

                else if (line.Contains("$CFunctions$"))
                {
                    int flag = 0;
                    foreach (var key in m_nonTerminals_Contexts)
                    {
                        String class_name = key.Key[3].ToString().ToUpper() + key.Key.Substring(4);
                        newFile.WriteLine("\tpublic class C" + class_name + " : CASTComposite\n\t{");
                        if (flag == 0)
                        {
                            newFile.WriteLine("\t\tpublic C" + class_name + "() : base(NodeType." + key.Key.ToUpper() +
                                              ", null, NodeType.NT_NA)\n\t\t{\n\t\t}");
                            flag = 1;
                        }
                        else
                        {
                            newFile.WriteLine("\t\tpublic C" + class_name + "() : base(NodeType." + key.Key.ToUpper() +
                                              ", parent, NodeType.NT_NA)\n\t\t{\n\t\t}");
                        }

                        newFile.WriteLine(
                            "\n\t\tpublic override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)\n\t\t{");
                        newFile.WriteLine(
                            "\t\t\tIASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;");
                        newFile.WriteLine("\t\t\tif (typedVisitor != null) return typedVisitor.Visit{0}(this);",
                            class_name);
                        newFile.WriteLine("\t\t\telse return visitor.VisitChildren(this);\n\t\t}\n");
                        newFile.WriteLine(
                            "\t\tpublic override CAbstractIterator<CASTElement> AcceptIterator(CASTAbstractConcreteIteratorFactory iteratorFactory)\n\t\t{");
                        newFile.WriteLine(
                            "\t\t\tIASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;");
                        newFile.WriteLine("\t\t\tif (typedFactory != null)\n\t\t\t{");
                        newFile.WriteLine("\t\t\t\t return iteratorFactory.Create{0}Iterator(this);", class_name);
                        newFile.WriteLine(
                            "\t\t\t}\n\t\t\treturn iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);\n\t\t}");
                        newFile.WriteLine(
                            "\t\tpublic override CAbstractIterator<CASTElement> AcceptEventIterator(CASTAbstractConcreteIteratorFactory iteratorFactory, CASTGenericIteratorEvents events, object info = null)\n\t\t{");
                        newFile.WriteLine(
                            "\t\t\tIASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;");
                        newFile.WriteLine("\t\t\tif (typedFactory != null)\n\t\t\t{");
                        newFile.WriteLine("\t\t\t\treturn iteratorFactory.Create{0}IteratorEvents(this, events, info);",
                            class_name);
                        newFile.WriteLine(
                            "\t\t\t}\n\t\t\treturn iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);\n\t\t}");
                        newFile.WriteLine("\t}\n");
                    }

                    foreach (string terminal_node in leaf_nodes)
                    {
                        newFile.WriteLine("\tpublic class C{0} : CASTLeaf<string>", terminal_node);
                        newFile.WriteLine("\t\t{");
                        newFile.WriteLine(
                            "\t\tpublic C{0}(string {1}Literal,CASTComposite parent) : base({1}Literal,NodeType.NT_{0},parent)",
                            terminal_node, terminal_node.ToLower());
                        newFile.WriteLine("\t\t{\n\t\t}");
                        newFile.WriteLine(
                            "\n\t\tprotected override void AddChild(CASTElement child, int context, int pos = -1)\n\t\t{");
                        newFile.WriteLine("\t\t\tthrow new NotImplementedException();\n\t\t}");
                        newFile.WriteLine(
                            "\t\tpublic override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor) \n\t\t{");
                        newFile.WriteLine(
                            "\t\t\tIASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;");
                        newFile.WriteLine("\t\t\tif (typedVisitor != null) return typedVisitor.Visit{0}(this);",
                            terminal_node);
                        newFile.WriteLine("\t\t\telse return visitor.VisitTerminal(this);\n\t\t}");
                        newFile.WriteLine(
                            "\n\t\t public override CAbstractIterator<CASTElement> AcceptIterator(CASTAbstractConcreteIteratorFactory iteratorFactory)\n\t\t{");
                        newFile.WriteLine("\t\t\tthrow new NotImplementedException();\n\t\t}");
                        newFile.WriteLine(
                            "\t\tpublic override CAbstractIterator<CASTElement> AcceptEventIterator(CASTAbstractConcreteIteratorFactory iteratorFactory, CASTGenericIteratorEvents events, object info = null)");
                        newFile.WriteLine("\t\t{\n\t\t\tthrow new NotImplementedException();\n\t\t}\n\t}");
                    }
                }

                #endregion

                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAstGeneration()
        {
            int flag = 0; //flag for the first node

            ASTGenerationInfoCollector info = new ASTGenerationInfoCollector();
            info.Visit(m_root);
            ASTGenerationInfoCollector.setAstGenerationInfo();

            ASTGenerationArrayCollector info1 = new ASTGenerationArrayCollector();
            info1.Visit(m_root);

            StreamReader gen = new StreamReader("..\\..\\Templates\\ASTGeneration\\ASTGenerationTemplate.cs");
            StreamWriter newFile = new StreamWriter("Generated\\ASTGeneration\\ASTGeneration_" + m_grammarID + ".cs");
            string line;
            string lineoutput = m_grammarID;

            ASTGenerationArrayCollector.DataFlow();
            Application.Run(new Form1());

            NT_CT_O = ASTGenerationArrayCollector.getN_TC_T;
            foreach (var var in Form1.checkd_positions)
            {
                key_pos.Add(var);
            }

            foreach (var key in ASTGenerationInfoCollector.getOverride.Reverse())
            {
                List<string> m_cont = new List<string>(); // contains RHS_override        #something
                for (int i = key.Value.Count - 1; i >= 0; i--)
                {
                    m_cont.Add(key.Value[i]);
                }

                m_override.Add(key.Key, m_cont);
            }

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using Antlr4.Runtime;");
                    newFile.WriteLine("using Antlr4.Runtime.Tree;");
                    newFile.WriteLine("using System.Collections.Generic;");
                    newFile.WriteLine("using {0};", m_grammarID);
                }
                else if (line.Contains("$GrammarName$"))
                {
                    line = line.Replace("$GrammarName$", lineoutput);
                    newFile.WriteLine(line);
                }
                else if (line.Contains("$ASTGenerationVisitors$"))
                {
                    if (line.Contains("$ASTGenerationVisitors$"))
                    {
                        for (int i = 0; i < key_pos.Count; i++)
                        {
                            int position = key_pos[i];
                            string key = NT_CT_O.Keys.ElementAt(position);
                            string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                                ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                                : NT_CT_O.Keys.ElementAt(position);

                            line = line.Replace("$ASTGenerationVisitors$", "\n");
                            newFile.Write(line);
                            newFile.WriteLine("\tpublic override int Visit{0} ({1}Parser.{0}Context context) ",
                                char.ToUpper(key_value[0]) + key_value.Substring(1), m_grammarID);
                            newFile.WriteLine("\t{");
                            if (flag == 0)
                            {
                                newFile.WriteLine("\t\tC{0} newNode = new C{0}();",
                                    char.ToUpper(key_value[0]) + key_value.Substring(1));
                                newFile.WriteLine("\t\tm_root = newNode;");
                                flag = 1;
                            }
                            else
                            {
                                newFile.WriteLine("\t\tC{0} newNode = new C{0}(m_parentsStack.Peek());",
                                    char.ToUpper(key_value[0]) + key_value.Substring(1));
                                newFile.WriteLine(
                                    "\t\tm_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());");
                            }

                            newFile.WriteLine("\t\tm_parentsStack.Push(newNode);");
                            foreach (var var in NT_CT_O)
                            {
                                if (var.Key.Equals(key))
                                {
                                    for (int k = 0; k < var.Value.Count; k++)
                                    {
                                        if (var.Value[k].StartsWith("*") || var.Value[k].StartsWith("+"))
                                        {
                                            string[] value = var.Value[k].Split(':');
                                            newFile.WriteLine(
                                                "\t\tVisitElementsInContext(context.{0}(), ContextType.CT_{1}_{2});",
                                                value[0].Substring(1), var.Key.ToUpper().Replace("#", ""),
                                                value[0].Substring(1).ToUpper());
                                        }
                                        else
                                        {
                                            string[] value = var.Value[k].Split(':');
                                            if (var.Value[k].StartsWith("?"))
                                            {
                                                newFile.WriteLine(
                                                    "\t\tVisitElementInContext(context.{0}(), ContextType.CT_{1}_{2});",
                                                    value[0].Substring(1), var.Key.ToUpper().Replace("#", ""),
                                                    value[0].Substring(1).ToUpper());
                                            }
                                            else
                                            {
                                                newFile.WriteLine(
                                                    "\t\tVisitElementInContext(context.{0}(), ContextType.CT_{1}_{2});",
                                                    value[0], var.Key.ToUpper().Replace("#", ""), value[0].ToUpper());
                                            }
                                        }
                                    }
                                }
                            }

                            newFile.WriteLine("\t\tm_parentsStack.Pop();");
                            newFile.WriteLine("\t\treturn 0;");
                            newFile.WriteLine("\t}");
                        }

                        #region VisitTerminal

                        newFile.WriteLine("\n\tpublic override int VisitTerminal(ITerminalNode node) {");
                        newFile.WriteLine("\n\t\tCASTComposite parent = m_parentsStack.Peek();");
                        newFile.WriteLine("\t\tswitch (node.Symbol.Type) {");

                        HashSet<string> leaf_nodes1 = new HashSet<string>();
                        flag = 0;
                        for (int i = 0; i < key_pos.Count; i++)
                        {
                            int position = key_pos[i];
                            string key = NT_CT_O.Keys.ElementAt(position);
                            string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                                ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                                : NT_CT_O.Keys.ElementAt(position);

                            foreach (var var in NT_CT_O)
                            {
                                if (var.Key.Equals(key))
                                {
                                    for (int k = 0; k < var.Value.Count; k++)
                                    {
                                        string[] splitter = var.Value[k].Split(':');

                                        if (splitter[1].Equals("Terminal"))
                                        {
                                            flag = 1;
                                            if ((var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                                 var.Value[k].StartsWith("*")) &&
                                                !leaf_nodes1.Contains(splitter[0].Substring(1)))
                                            {
                                                newFile.WriteLine("\t\t\tcase {0}Parser.{1}:", m_grammarID,
                                                    splitter[0].Substring(1).ToUpper());
                                                newFile.WriteLine(
                                                    "\t\t\tC{0} {1}Token = new C{0}(node.GetText(),parent);",
                                                    splitter[0].ToUpper(), splitter[0].Substring(1).ToLower());
                                                newFile.WriteLine(
                                                    "\t\t\tparent.AddChild({0}Token, m_currentContext.Peek());\n\t\t\t\tbreak;",
                                                    splitter[0].Substring(1).ToLower());
                                                leaf_nodes1.Add(splitter[0].Substring(1));
                                            }
                                            else if (!(var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                                       var.Value[k].StartsWith("*")) &&
                                                     !leaf_nodes1.Contains(splitter[0]))
                                            {
                                                newFile.WriteLine("\t\t\tcase {0}Parser.{1}:", m_grammarID,
                                                    splitter[0].ToUpper());
                                                newFile.WriteLine(
                                                    "\t\t\t\tC{0} {1}Token = new C{0}(node.GetText(),parent);",
                                                    splitter[0].ToUpper(), splitter[0].ToLower());
                                                newFile.WriteLine(
                                                    "\t\t\t\tparent.AddChild({0}Token, m_currentContext.Peek());\n\t\t\tbreak;",
                                                    splitter[0].ToLower());
                                                leaf_nodes1.Add(splitter[0]);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        newFile.WriteLine("\t\t\t\t}\n\t\t\treturn base.VisitTerminal(node);");

                        if (leaf_nodes1.Count() == 0 && flag == 0)
                        {
                            newFile.WriteLine("\t\t\t//No Terminal was found. Please specify yours.");
                            newFile.WriteLine("\t\t\tcase <GrammarName>Parser.<Terminal_name>:");
                            newFile.WriteLine(
                                "\t\t\t\tC<Terminal_name> <Terminal_name>Token = new C<Terminal_name>(node.GetText(),parent);");
                            newFile.WriteLine(
                                "\t\t\t\tparent.AddChild(<Terminal_name>Token, m_currentContext.Peek());\n\t\t\tbreak;\n\t\t}\n\t}");
                        }
                        else
                        {
                            newFile.WriteLine("\t\t}\n\t}");
                        }

                        #endregion
                    }
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateConfiguration()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\Configuration\\ConfigurationTemplate.cs");
            StreamWriter newFile = new StreamWriter("Generated\\Configuration\\Configuration_" + m_grammarID + ".cs");
            HashSet<string> leaf_nodes = new HashSet<string>();
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using {0};", m_grammarID);
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName$"))
                {
                    newFile.WriteLine("namespace " + m_grammarID);
                }
                else if (line.Contains("$InitNodeTypeConfigurationSettings$"))
                {
                    newFile.WriteLine("\t\t{0}", "private static void InitNodeTypeConfigurationSettings()");
                    newFile.WriteLine("\t\t{0}", "{");

                    #region non_terminal

                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                newFile.WriteLine(
                                    "\t\t\tCConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_{0}] = new CNodeTypeConfiguration()",
                                    key_value.ToUpper());
                                newFile.Write("\t\t\t\t{");
                                newFile.Write(
                                    @"M_NumberOfContexts = {0}, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = ""default"", M_Color = Color.C_DEFAULT ",
                                    var.Value.Count);
                                newFile.Write("};");
                                newFile.WriteLine();
                            }
                        }
                    }

                    #endregion

                    newFile.WriteLine();

                    #region leaf nodes

                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                for (int k = 0; k < var.Value.Count; k++)
                                {
                                    string[] splitter = var.Value[k].Split(':');

                                    if (splitter[1].Equals("Terminal"))
                                    {
                                        if ((var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                             var.Value[k].StartsWith("*")) &&
                                            !leaf_nodes.Contains(splitter[0].Substring(1)))
                                        {
                                            newFile.WriteLine(
                                                "\t\t\tCConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_{0}] = new CNodeTypeConfiguration()",
                                                splitter[0].Substring(1).ToUpper());
                                            newFile.Write("\t\t\t\t{");
                                            newFile.Write(
                                                @"M_NumberOfContexts = {0}, M_NodeTypeCategory = NodeType.NT_LEAF, M_ColorName = ""default"", M_Color = Color.C_DEFAULT ",
                                                "0");
                                            newFile.Write("};");
                                            newFile.WriteLine();
                                            leaf_nodes.Add(splitter[0].Substring(1));
                                        }
                                        else if (!(var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                                   var.Value[k].StartsWith("*")) && !leaf_nodes.Contains(splitter[0]))
                                        {
                                            newFile.WriteLine(
                                                "\t\t\tCConfigurationSettings.m_nodeTypeConfiguration[NodeType.NT_{0}] = new CNodeTypeConfiguration()",
                                                splitter[0].ToUpper());
                                            newFile.Write("\t\t\t\t{");
                                            newFile.Write(
                                                @"M_NumberOfContexts = {0}, M_NodeTypeCategory = NodeType.NT_LEAF, M_ColorName = ""default"", M_Color = Color.C_DEFAULT ",
                                                "0");
                                            newFile.Write("};");
                                            newFile.WriteLine();
                                            leaf_nodes.Add(splitter[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    newFile.WriteLine();

                    #region contexts

                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                for (int k = 0; k < var.Value.Count; k++)
                                {
                                    string[] splitter = var.Value[k].Split(':');

                                    if (var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                        var.Value[k].StartsWith("*"))
                                    {
                                        newFile.WriteLine(
                                            "\t\t\tCConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_{0}_{1}] = new CContextTypeConfiguration()",
                                            key_value.ToUpper(), splitter[0].Substring(1).ToUpper());
                                        newFile.Write("\t\t\t\t{");
                                        newFile.Write(" M_ContextIndex = {0}, M_HostNodeType = NodeType.NT_{1}", k,
                                            key_value.ToUpper());
                                        newFile.Write(" };");
                                        newFile.WriteLine();
                                    }
                                    else
                                    {
                                        newFile.WriteLine(
                                            "\t\t\tCConfigurationSettings.m_contextTypeConfiguration[ContextType.CT_{0}_{1}] = new CContextTypeConfiguration()",
                                            key_value.ToUpper(), splitter[0].ToUpper());
                                        newFile.Write("\t\t\t\t{");
                                        newFile.Write(" M_ContextIndex = {0}, M_HostNodeType = NodeType.NT_{1}", k,
                                            key_value.ToUpper());
                                        newFile.Write(" };");
                                        newFile.WriteLine();
                                    }
                                }

                                newFile.WriteLine();
                            }
                        }
                    }

                    #endregion

                    /*foreach (var key in m_nonTerminals_Contexts)
                    {
                        newFile.WriteLine("\t\t\tCConfigurationSettings.m_nodeTypeConfiguration[NodeType.{0}] = new CNodeTypeConfiguration()", key.Key.ToUpper());
                        newFile.Write("\t\t\t\t{");
                        newFile.Write(@" M_NumberOfContexts = {0}, M_NodeTypeCategory = NodeType.NT_NA, M_ColorName = ""default"", M_Color = Color.C_DEFAULT ", key.Value.Count);
                        newFile.Write(" };");
                        newFile.WriteLine();
                    }
                    newFile.WriteLine();
                    foreach (var key in m_nonTerminals_Contexts)
                    {
                        for (int i = 0; i < key.Value.Count; i++)
                        {
                            newFile.WriteLine("\t\t\tCConfigurationSettings.m_contextTypeConfiguration[ContextType.{0}] = new CContextTypeConfiguration()", key.Value[i].Replace("_", "_" + key.Key.Replace("NT_", "") + "_").ToUpper());
                            newFile.Write("\t\t\t\t{");
                            newFile.Write(" M_ContextIndex = {0}, M_HostNodeType = NodeType.{1}", i, key.Key.ToUpper());
                            newFile.Write(" };");
                            newFile.WriteLine();
                        }
                    }*/
                    newFile.WriteLine("\t\t{0}", "}");
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAbstractConcreteIteratorFactory()
        {
            StreamReader gen =
                new StreamReader("..\\..\\Templates\\Factory\\AbstractConcreteIteratorFactoryTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Factory\\AbstractConcreteIteratorFactory_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else if (line.Contains("$interfaces$") || line.Contains("$implementinterfaces$"))
                {
                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        if (line.Contains("$interfaces$"))
                        {
                            newFile.WriteLine(
                                "\t\tCAbstractIterator<CASTElement> Create{0}Iterator(CASTElement element);",
                                char.ToUpper(key_value[0]) + key_value.Substring(1));
                            newFile.WriteLine(
                                "\t\tCAbstractIterator<CASTElement> Create{0}IteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);\n",
                                char.ToUpper(key_value[0]) + key_value.Substring(1));
                        }
                        else
                        {
                            newFile.WriteLine(
                                "\t\tpublic virtual CAbstractIterator<CASTElement> Create{0}Iterator(CASTElement element)",
                                char.ToUpper(key_value[0]) + key_value.Substring(1));
                            newFile.WriteLine(
                                "\t\t{\n\t\t\treturn CreateIteratorASTElementDescentantsFlatten(element);\n\t\t}");

                            newFile.WriteLine(
                                "\t\tpublic virtual CAbstractIterator<CASTElement> Create{0}IteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null)",
                                char.ToUpper(key_value[0]) + key_value.Substring(1));
                            newFile.WriteLine(
                                "\t\t{\n\t\t\treturn CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);\n\t\t}\n");
                        }
                    }
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAstConcreteVisitor()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\Visitors\\ASTConcreteVisitorTemplate.cs");
            StreamWriter newFile = new StreamWriter("Generated\\Visitors\\ASTConcreteVisitor_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else if (line.Contains("$interface$") || line.Contains("$implementinterface$"))
                {
                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        if (line.Contains("$interface$"))
                        {
                            newFile.WriteLine("\t\tReturn Visit{0}(CASTElement currentNode);\n",
                                char.ToUpper(key_value[0]) + key_value.Substring(1));
                        }
                        else
                        {
                            newFile.WriteLine("\t\tpublic virtual Return Visit{0}(CASTElement currentNode)",
                                char.ToUpper(key_value[0]) + key_value.Substring(1));
                            newFile.WriteLine("\t\t{\n\t\t\treturn base.VisitChildren(currentNode);\n\t\t}\n");
                        }
                    }

                    foreach (string terminal_node in leaf_nodes)
                    {
                        if (line.Contains("$interface$"))
                            newFile.WriteLine("\t\tReturn Visit{0}(CASTElement currentNode);\n", terminal_node);
                        else if (line.Contains("$implementinterface$"))
                        {
                            newFile.WriteLine("\t\tpublic virtual Return Visit{0}(CASTElement currentNode)",
                                terminal_node);
                            newFile.WriteLine("\t\t{\n\t\t\treturn base.VisitTerminal(currentNode);\n\t\t}\n");
                        }
                    }
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateCAstAbstractIteratorEvents()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\Events\\CASTAbstractIteratorEventsTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Events\\CASTAbstractIteratorEvents_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAbstractGenericIteratorFactory()
        {
            StreamReader gen =
                new StreamReader("..\\..\\Templates\\Factory\\AbstractGenericIteratorFactoryTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Factory\\AbstractGenericIteratorFactory_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAbstractIterator()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\Iterator\\AbstractIteratorTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Iterator\\AbstractIterator_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateAbstractASTVistor()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\Visitors\\AbstractASTVisitorTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Visitors\\AbstractASTVisitor_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateConcreteIterator()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\Iterator\\ConcreteIteratorsTemplate.cs");
            StreamWriter newFile =
                new StreamWriter("Generated\\Iterator\\ConcreteIterators_" + m_grammarID + ".cs");
            string line;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System;");
                    newFile.WriteLine("using System.Collections.Generic;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }

        private void GenerateASTPrinter()
        {
            StreamReader gen = new StreamReader("..\\..\\Templates\\ASTPrinter\\ASTPrinterTemplate.cs");
            StreamWriter newFile = new StreamWriter("Generated\\ASTPrinter\\ASTPrinter_" + m_grammarID + ".cs");

            //HashSet<string> leaf_nodes = new HashSet<string>();
            string line;
            int flag = 0;
            string context_value;

            while ((line = gen.ReadLine()) != null)
            {
                if (line.Contains("$NameSpaces$"))
                {
                    newFile.WriteLine("using System.Diagnostics;");
                    newFile.WriteLine("using System.IO;");
                }
                else if (line.Contains("$GrammarName"))
                {
                    line = line.Replace("$GrammarName$", m_grammarID);
                    newFile.WriteLine(line);
                }
                else if (line.Contains("$functions$"))
                {
                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        newFile.WriteLine("\t\tpublic override int Visit{0}(CASTElement currentNode) ",
                            char.ToUpper(key_value[0]) + key_value.Substring(1));
                        newFile.WriteLine("\t\t{\n\t\t\tCASTComposite current = currentNode as CASTComposite;");
                        newFile.WriteLine("\t\t\tstring clusterName;");
                        newFile.WriteLine("\t\t\tstring contextName;");
                        if (flag == 0)
                        {
                            newFile.WriteLine("\n\t\t\tm_outputStream.WriteLine(\"digraph {\\n\");");
                        }
                        else
                        {
                            newFile.WriteLine(
                                "\n\t\t\tm_outputStream.WriteLine(\"\\\"{0}\\\"->\\\"{1}\\\"\", currentNode.M_Parent.M_Label, currentNode.M_Label);");
                        }

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                for (int k = 0; k < var.Value.Count; k++)
                                {
                                    if (var.Value[k].StartsWith("*") || var.Value[k].StartsWith("+") ||
                                        var.Value[k].StartsWith("?"))
                                    {
                                        string[] value = var.Value[k].Split(':');
                                        context_value = key_value.ToUpper() + "_" + value[0].Substring(1).ToUpper();
                                    }
                                    else
                                    {
                                        string[] value = var.Value[k].Split(':');
                                        context_value = key_value.ToUpper() + "_" + value[0].ToUpper();
                                    }

                                    newFile.WriteLine(
                                        "\n\t\t\tif (current.GetNumberOfContextElements(ContextType.CT_{0}) > 0) ",
                                        context_value);
                                    newFile.WriteLine(
                                        "\t\t\t{\n\t\t\t\tclusterName = \"cluster\" + ms_clusterCounter++;");
                                    newFile.WriteLine("\t\t\t\tcontextName = ContextType.CT_{0}.ToString();",
                                        context_value);
                                    newFile.WriteLine(
                                        "\t\t\t\tm_outputStream.WriteLine(\"subgraph {0} {{\\n node [style=filled, color=white];\\n style=filled;\\n color=lightgrey;\\n label = \\\"{1}\\\";\\n\", clusterName, contextName); ");
                                    newFile.WriteLine(
                                        $"\n\t\t\t\tforeach (CASTElement element in current.GetContextChildren(ContextType.CT_{context_value}))" +
                                        "{");
                                    newFile.WriteLine(
                                        "\t\t\t\t\tm_outputStream.WriteLine(\"\\\"{0}\\\"\", element.M_Label); ");
                                    newFile.WriteLine(
                                        "\n\t\t\t\t\tif (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT ) {");
                                    newFile.WriteLine(
                                        "\t\t\t\t\t\t m_outputStream.WriteLine(\"[fillcolor = \"+ CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_ColorName+ \"]\");\n\t\t\t\t\t}\n\t\t\t\t}");
                                    newFile.WriteLine("\t\t\t\tm_outputStream.WriteLine(\"}\");\n\t\t\t}");
                                }

                                newFile.WriteLine("\n\t\t\tbase.Visit{0}(currentNode);",
                                    char.ToUpper(key_value[0]) + key_value.Substring(1));
                                if (flag == 0)
                                {
                                    newFile.WriteLine("\n\t\t\tm_outputStream.WriteLine(\"}\");");
                                    newFile.WriteLine("\t\t\tm_outputStream.Close();");

                                    newFile.WriteLine("\n\t\t\tif (true)\n\t\t\t{");
                                    newFile.WriteLine("\t\t\t\tProcess process = new Process();");
                                    newFile.WriteLine("\t\t\t\tprocess.StartInfo.FileName = \"dot.exe\";");
                                    newFile.WriteLine(
                                        "\t\t\t\tprocess.StartInfo.Arguments = \" - Tgif \" + m_outputFile + \" - o\" + Path.GetFileNameWithoutExtension(m_outputFile) + \".gif\";");
                                    newFile.WriteLine(
                                        "\t\t\t\tprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;");
                                    newFile.WriteLine("\t\t\t\tprocess.Start();");
                                    newFile.WriteLine("\t\t\t\tprocess.WaitForExit();\n\t\t\t}");

                                    flag = 1;
                                }

                                newFile.WriteLine("\n \t\t\treturn 0;");
                            }
                        }

                        newFile.WriteLine("\t\t}\n");
                    }

                    foreach (string terminal_node in leaf_nodes)
                    {
                        newFile.WriteLine("\t\tpublic override int Visit{0}(CASTElement currentNode)", terminal_node);
                        newFile.WriteLine(
                            "\t\t{\n\t\t\tm_outputStream.WriteLine(\"\\\"{0}\\\"->\\\"{1}\\\"\", currentNode.M_Parent.M_Label, currentNode.M_Label);");
                        newFile.WriteLine("\t\t\treturn base.Visit{0}(currentNode);", terminal_node);
                        newFile.WriteLine("\t\t}");
                    }

                    int numOfContexts = 0;
                    for (int i = 0; i < key_pos.Count; i++)
                    {
                        int position = key_pos[i];
                        string key = NT_CT_O.Keys.ElementAt(position);
                        string key_value = NT_CT_O.Keys.ElementAt(position).StartsWith("#")
                            ? NT_CT_O.Keys.ElementAt(position).Replace("#", "")
                            : NT_CT_O.Keys.ElementAt(position);

                        foreach (var var in NT_CT_O)
                        {
                            if (var.Key.Equals(key))
                            {
                                for (int k = 0; k < var.Value.Count; k++)
                                {
                                    string[] splitter = var.Value[k].Split(':');

                                    if (splitter[1].Equals("Terminal"))
                                    {
                                        if ((var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                             var.Value[k].StartsWith("*")) &&
                                            !leaf_nodes.Contains(splitter[0].Substring(1)))
                                        {
                                            newFile.WriteLine("\t\tNT_{0} = {1},", splitter[0].ToUpper(),
                                                numOfContexts);
                                            numOfContexts++;
                                            leaf_nodes.Add(splitter[0].Substring(1));
                                        }
                                        else if (!(var.Value[k].StartsWith("?") || var.Value[k].StartsWith("+") ||
                                                   var.Value[k].StartsWith("*")) && !leaf_nodes.Contains(splitter[0]))
                                        {
                                            newFile.WriteLine("\t\tNT_{0} = {1},", splitter[0].ToUpper(),
                                                numOfContexts);
                                            numOfContexts++;
                                            leaf_nodes.Add(splitter[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    newFile.WriteLine(line);
                }
            }

            gen.Close();
            newFile.Close();
        }
    }
}