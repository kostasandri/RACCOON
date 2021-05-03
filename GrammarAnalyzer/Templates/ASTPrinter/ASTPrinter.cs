using GrammarAnalyzer.Info_Collection;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Prints the AST Tree
/// </summary> 

namespace GrammarAnalyzer.ASTVisitor.Visitors
{
    class ASTPrinter : CASTAbstractConcreteVisitor<int>
    {
        private StreamWriter m_outputStream;
        private string m_outputFile;
        private int ms_clusterCounter = 0;

        public ASTPrinter(string file)
        {
            m_outputFile = Path.GetFileNameWithoutExtension(file) + "_AST.dot";
            m_outputStream = new StreamWriter(m_outputFile);
        }

        public override int VisitCompileUnit(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            string clusterName;
            string contextName;


            m_outputStream.WriteLine("digraph G {\n");

            if (current.GetNumberOfContextElements(ContextType.CT_COMPILEUNIT_GRAMMAR_RULE) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_COMPILEUNIT_GRAMMAR_RULE.ToString();
                m_outputStream.WriteLine("subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n", clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_COMPILEUNIT_GRAMMAR_RULE))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " + CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }


            if (current.GetNumberOfContextElements(ContextType.CT_COMPILEUNIT_PROLOGUE) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_COMPILEUNIT_PROLOGUE.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_COMPILEUNIT_PROLOGUE))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            base.VisitCompileUnit(currentNode);

            m_outputStream.WriteLine("}");
            m_outputStream.Close();

            if (true)
            {
                Process process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = "dot.exe";
                process.StartInfo.Arguments = "-Tgif " + m_outputFile + " -o" + Path.GetFileNameWithoutExtension(m_outputFile) + ".gif";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit(); // Waits here for the process to exit.
            }

            return 0;
        }

        public override int VisitGram_Spec(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            string clusterName;
            string contextName;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            // Visit fundef context
            if (current.GetNumberOfContextElements(ContextType.CT_GRAMMARSPEC_TYPE) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_GRAMMARSPEC_TYPE.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_GRAMMARSPEC_TYPE))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            if (current.GetNumberOfContextElements(ContextType.CT_GRAMMARSPEC_ID) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_GRAMMARSPEC_ID.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_GRAMMARSPEC_ID))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            base.VisitGram_Spec(currentNode);
            return 0;
        }

        public override int VisitGrammar_Rule(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            string clusterName;
            string contextName;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            // Visit non terminal context
            if (current.GetNumberOfContextElements(ContextType.CT_GRAMMARRULE_NON_TERMINAL) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_GRAMMARRULE_NON_TERMINAL.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_GRAMMARRULE_NON_TERMINAL))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            // Visit grammar rule rhs override context
            if (current.GetNumberOfContextElements(ContextType.CT_GRAMMARRULE_GRAMMAR_RULE_ALTERNATIVES) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_GRAMMARRULE_GRAMMAR_RULE_ALTERNATIVES.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType
                    .CT_GRAMMARRULE_GRAMMAR_RULE_ALTERNATIVES))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            base.VisitGrammar_Rule(currentNode);

            return 0;
        }

        public override int VisitGrammar_Rule_Rhs(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            string clusterName;
            string contextName;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            // Visit non terminal context
            if (current.GetNumberOfContextElements(ContextType.CT_GRAMMARRULERHS_ALTERNATIVETERMS) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_GRAMMARRULERHS_ALTERNATIVETERMS.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType
                    .CT_GRAMMARRULERHS_ALTERNATIVETERMS))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            base.VisitGrammar_Rule_Rhs(currentNode);

            return 0;
        }

        public override int VisitClosure(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            string clusterName;
            string contextName;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            // Visit non terminal context
            if (current.GetNumberOfContextElements(ContextType.CT_CLOSURE_TERM) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_CLOSURE_TERM.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_CLOSURE_TERM))
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " + CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            base.VisitClosure(currentNode);

            return 0;
        }

        public override int VisitParenthesizedTerm(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            string clusterName;
            string contextName;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            // Visit non terminal context
            if (current.GetNumberOfContextElements(ContextType.CT_RHSPARENTHESIZEDTERM_OR_TERMS) > 0)
            {
                clusterName = "cluster" + ms_clusterCounter++;
                contextName = ContextType.CT_RHSPARENTHESIZEDTERM_OR_TERMS.ToString();
                m_outputStream.WriteLine(
                    "subgraph {0} {{\n node [style=filled,color=white];\n style=filled;\n color=lightgrey;\n label = \"{1}\";\n",
                    clusterName, contextName);
                foreach (CASTElement element in current.GetContextChildren(ContextType.CT_RHSPARENTHESIZEDTERM_OR_TERMS)
                )
                {
                    m_outputStream.WriteLine("\"{0}\"", element.M_Label);
                    if (CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType].M_Color != Color.C_DEFAULT)
                    {
                        m_outputStream.WriteLine(" [fillcolor = " +
                                                 CConfigurationSettings.m_nodeTypeConfiguration[element.M_NodeType]
                                                     .M_ColorName + "]");
                    }
                }

                m_outputStream.WriteLine("}");
            }

            base.VisitGrammar_Rule_Rhs(currentNode);

            return 0;
        }

        public override int VisitTERMINAL(CASTElement currentNode)
        {
            CASTLeaf<CASTElement> current = currentNode as CASTLeaf<CASTElement>;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            return 0;
        }

        public override int VisitNONTERMINAL(CASTElement currentNode)
        {
            CASTComposite current = currentNode as CASTComposite;
            m_outputStream.WriteLine("\"{0}\"->\"{1}\"", currentNode.M_Parent.M_Label, currentNode.M_Label);

            return 0;
        }
    }
}