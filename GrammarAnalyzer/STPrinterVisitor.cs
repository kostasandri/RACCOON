using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GrammarAnalyzer
{
    class STPrinterVisitor : EBNFParserBaseVisitor<int>
    {
        private Stack<string> m_PTPath;

        private static int ms_ASTElementCounter = 0;

        private StreamWriter m_outputStream;

        private string m_outputFile;

        public STPrinterVisitor(string file, bool callGraphViz = true)
        {
            m_PTPath = new Stack<string>();
            m_outputFile = Path.GetFileNameWithoutExtension(file) + ".dot";
            m_outputStream = new StreamWriter(m_outputFile);
        }

        public override int VisitCompileUnit(EBNFParser.CompileUnitContext context)
        {
            // PREORDER ACTIONS
            m_outputStream.WriteLine("digraph " + Path.GetFileNameWithoutExtension(m_outputFile) + "{\n");
            string label = "CompileUnit_" + ms_ASTElementCounter.ToString();
            //m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitCompileUnit(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();


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
                process.WaitForExit();// Waits here for the process to exit.
            }


            return 0;
        }

        public override int VisitPrologue(EBNFParser.PrologueContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitPrologue(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitGram_spec(EBNFParser.Gram_specContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitGram_spec(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitToks(EBNFParser.ToksContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitToks(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitOpts(EBNFParser.OptsContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitOpts(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitOption(EBNFParser.OptionContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitOption(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitAction_content(EBNFParser.Action_contentContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitAction_content(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitExtern_code(EBNFParser.Extern_codeContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitExtern_code(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitGrammar_rule(EBNFParser.Grammar_ruleContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitGrammar_rule(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitGrammar_rule_RHS(EBNFParser.Grammar_rule_RHSContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitGrammar_rule_RHS(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitRule_action(EBNFParser.Rule_actionContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitRule_action(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitRhs_rule_terms(EBNFParser.Rhs_rule_termsContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitRhs_rule_terms(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitTerminal(EBNFParser.TerminalContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitTerminal(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitReg_exp(EBNFParser.Reg_expContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitReg_exp(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitGrammar_exp(EBNFParser.Grammar_expContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitGrammar_exp(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }

        public override int VisitArrow(EBNFParser.ArrowContext context)
        {
            // PREORDER ACTIONS
            string label = "Grammar_rule_RHS" + ms_ASTElementCounter.ToString();
            m_outputStream.WriteLine("\"{0}\"->\"{1}\";", m_PTPath.Peek(), label);
            ms_ASTElementCounter++;
            m_PTPath.Push(label);

            base.VisitArrow(context);

            // POSTORDER ACTIONS
            m_PTPath.Pop();

            return 0;
        }
    }
}
