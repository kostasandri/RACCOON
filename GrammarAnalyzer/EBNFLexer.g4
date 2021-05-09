lexer grammar EBNFLexer;

@lexer::header {using System;}


@lexer::members { public static bool grammar_flag_TNT=true;
				  public static int nestinglevel = 0;
				  public static bool externalCodeFlag=false;
				  public static bool externCodeSwitch = false; }


PARSER : 'parser';
LEXER : 'lexer';

PREDICATE : '{' ~[}]* '}' '?' ;
OPT : 'options'; 
TOKENS : 'tokens' {									int x=1; 
												   while(_input.La(x)== ' '){
                                                     x++;
                                                   }
												   if ( _input.La(x) != '{' ){ 
													Type = NON_TERMINAL;}
												   };
ACTION :  '@parser::header'     { externalCodeFlag = true;}
		| '@lexer::header'		{ externalCodeFlag = true;}
		| '@parser::members'	{ externalCodeFlag = true;}
		| '@lexer::members'		{ externalCodeFlag = true;}
		| '@header'				{ externalCodeFlag = true;}
		| '@members'			{ externalCodeFlag = true;}
		;

FRAGMENT : 'fragment';
GRAMMAR : 'grammar' {grammar_flag_TNT = true;};
RANGE : '[' .*? ']';
QUOTE : ['] | ["];
LBRACE : '{'	{ nestinglevel++;
				  if ( externalCodeFlag ){
					externCodeSwitch=true;
					Mode(EXTERNALCODE);
				  }
			    } ;
RBRACE : '}'	{nestinglevel--;
					if ( nestinglevel == 0 && externalCodeFlag ){
						externalCodeFlag = false;
					    externCodeSwitch = false;
						Mode(EBNFLexer.DefaultMode);
					}
				};
LAGKIL : '[';
RAGKIL : ']';
LPAREN : '(';
RPAREN : ')' ;
QMARK : '?' ;
ASTERISK : '*' ;
OR : '|'   ;
PLUS : '+' ;
PLUSEQUAL : '+=' ;
SUBTRACT : '-';
COMMA : ',';										 //{grammar_flag_TNT= true; } ;
NOT : '~';
COLON : ':' ;
LABRACKET : '<';
RABRACKET : '>';
Override : 'Override';
DOT : '.';
DOTS : '..';
SEMICOLON : ';' ;
ARROW : '->' {grammar_flag_TNT= true; } ;
EQUAL : '=';
HASH : '#' {grammar_flag_TNT= true; };
IMPLICIT_TERMINAL : '\'' .*? '\''; 


CHAR_LITERAL : '\'' LITERAL_CHAR '\'';
fragment LITERAL_CHAR : .?[\\][\'];

ASSOCIATIVITY: '<' 'assoc=' ('left'|'right') '>';

NONGREEDYCLOSURE : '.*?';

TERMINAL: {!grammar_flag_TNT}? [A-Z][A-Za-z0-9_]* {int x=1;													
													//Console.WriteLine("###1: "+Text); 
													//Console.WriteLine("###1.1: " + (char) _input.La(x));
												   while(_input.La(x)== ' ' || _input.La(x)== '\t' ){
                                                     x++;
                                                   }

												   if ( _input.La(x) == '=' || (_input.La(x) == '+' && _input.La(x + 1) == '=')) { 
													Type = ID;}
												   };
NON_TERMINAL : {!grammar_flag_TNT}? [a-z][A-Za-z0-9_]*  { int x=1;														
														//Console.WriteLine("####2: " + Text); 
														//Console.WriteLine("####2.1: " + (char) _input.La(x)); 
												         while(_input.La(x)== ' ' || _input.La(x)== '\t' ){
                                                           x++;
                                                         }
												        if ( _input.La(x) == '=' || (_input.La(x) == '+' && _input.La(x + 1) == '=')) { 
													       Type = ID;}
												        };
ID : [a-zA-Z_][A-Za-z0-9_]*  { grammar_flag_TNT = false; };
NUMBER : [+-]?[0-9]+ ;
AT : '@' ;
BlockComment:   '/*' .*? '*/'-> skip ;
LineComment:'//' ~[\r\n]* -> skip;
WS : [ \r\n\t]+ -> skip;

mode EXTERNALCODE;
LBR : '{' { nestinglevel++;
				 if ( externalCodeFlag ){
					externCodeSwitch=true;
					Mode(EXTERNALCODE);
				 }
				Type=EBNFLexer.LBRACE; 
		    } ; 
RBR : '}' {nestinglevel--;
					if ( nestinglevel == 0 && externalCodeFlag ){
						externalCodeFlag = false;
					    externCodeSwitch = false;
						Mode(EBNFLexer.DefaultMode);
					}
					Type=EBNFLexer.RBRACE; 
				};
EXTERNCODE : ~[{}]*;

