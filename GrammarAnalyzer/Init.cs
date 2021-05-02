using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GrammarAnalyzer;
using GrammarAnalyzer.ASTVisitor.Visitors;
using System;
using System.IO;


namespace ConsoleApplication1
{
    class Init
    {

        // Parses the given file
        static int Parse(string loc)
        {
            StreamReader reader = new StreamReader(loc);

            AntlrInputStream iStream = new AntlrInputStream(reader);
            EBNFLexer lexer = new EBNFLexer(iStream);
            /*ITokenSource lexer = new EBNFLexer(iStream);
            ITokenStream tokens = new CommonTokenStream(lexer);*/
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            EBNFParser parser = new EBNFParser(tokens);
            IParseTree tree = parser.compileUnit();

            //Console.WriteLine(tree.ToStringTree(parser));

            /*foreach (IToken tok in tokens.GetTokens())
            {
                Console.WriteLine(tok);
            }*/

            //STPrinterVisitor stPrinter = new STPrinterVisitor(loc);
            //stPrinter.Visit(tree);

            //ASTNodesLocatorVisitor vis1 = new ASTNodesLocatorVisitor();
            //vis1.Visit(tree);

            ASTGeneration astGeneration = new ASTGeneration();
            astGeneration.Visit(tree);

            ASTPrinter astPrinter = new ASTPrinter("Test");
            astPrinter.Visit(astGeneration.MRoot);

            RaccoonFileGenerator generator = new RaccoonFileGenerator((ParserRuleContext)tree);
            generator.Generate();

            return parser.NumberOfSyntaxErrors;
        }

        static int ParseSubDirectories(string directory)
        {

            int errors = 0; // Errors per file
            int fileerror;  // Total Errors


            Console.WriteLine("Processing directory {0}...", directory);
            foreach (string file in Directory.GetFiles(directory))
            {
                Console.WriteLine("Parsing {0}", Path.GetFileName(file));
                errors += fileerror = Parse(file);
                Console.WriteLine();
                Console.WriteLine(":{0} errors\n", fileerror);
            }
            foreach (string dir in Directory.GetDirectories(directory))
            {
                errors += ParseSubDirectories(dir);
            }
            return errors;

        }

        static void Main(string[] args)
        {
            int i = 0;
            int fileerror;
            int errors = 0;

            foreach (string loc in args)
            {
                if (File.Exists(loc))
                {
                    Console.WriteLine("Parsing {0}", args[i]);
                    errors += fileerror = Parse(loc);
                    Console.WriteLine();
                    Console.Write(":{0} errors", fileerror);
                }
                else if (Directory.Exists(loc))
                {
                    errors += ParseSubDirectories(loc);
                }
                else
                {
                    Console.WriteLine("File or Directory with the given name <{0}> doesn't exist", loc);
                }
                i++;
            }
            Console.WriteLine("\n\nTotal {0} errors", errors);

        }
    }
}
