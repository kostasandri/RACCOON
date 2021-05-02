$NameSpaces$

namespace $GrammarName$
{
    class ASTPrinter : CASTAbstractConcreteVisitor<int>
    {
        private StreamWriter m_outputStream;

        private string m_outputFile;

        private int ms_clusterCounter = 0;

        public ASTPrinter(string file)
        {
            m_outputFile = Path.GetFileNameWithoutExtension(file) + "AST.dot";
            m_outputStream = new StreamWriter(m_outputFile);
        }

        $functions$
    }
}