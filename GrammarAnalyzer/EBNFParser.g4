parser grammar EBNFParser;

@parser::header { } 

options { tokenVocab = EBNFLexer; }


compileUnit: prologue+  ( grammar_rule | FRAGMENT? reg_exp )+;						// ok

prologue :  gram_spec | action_content | toks | opts ;
       
gram_spec : tp=( PARSER | LEXER ) ? GRAMMAR ID SEMICOLON;							// ok 

toks : TOKENS LBRACE TERMINAL (COMMA TERMINAL)* RBRACE								//token declaration
	 ;

opts : OPT LBRACE (option)* RBRACE													// options
		; 

option :ID EQUAL (NON_TERMINAL | TERMINAL) SEMICOLON								 //options
	   ;

action_content : ACTION rule_action ;												// '@parser::header' { x; }		

		
grammar_rule : NON_TERMINAL COLON grammar_rule_rhs_overide (OR grammar_rule_rhs_overide )* SEMICOLON	// alternation		// ok // x : x #x | x ;
			 ;

grammar_rule_rhs_overide : grammar_rule_RHS? (HASH ID)?	;							// ok


// Y : A ( B C )*
//   |   ( B C )
//	 | ( A C ) ( B D )

//grammar_rule_RHS 
//grammar_rule_RHS LPAREN grammar_rule_RHS RPAREN
//LPAREN  grammar_rule_RHS RPAREN  LPAREN grammar_rule_RHS RPAREN

// Y : ( A B )*

//grammar_rule_RHS 
// grammar_rule_RHS  ASTERISK				
// LPAREN  grammar_rule_RHS RPAREN ASTERISK


// Y : A ( B C )* D

//grammar_rule_RHS 
// grammar_rule_RHS NON_TERMINAL
// grammar_rule_RHS  ASTERISK NON_TERMINAL			
// grammar_rule_RHS LPAREN grammar_rule_RHS RPAREN  ASTERISK NON_TERMINAL			
// grammar_rule_RHS LPAREN grammar_rule_RHS NON_TERMINAL RPAREN  ASTERISK NON_TERMINAL			
// grammar_rule_RHS LPAREN NON_TERMINAL NON_TERMINAL RPAREN  ASTERISK NON_TERMINAL			
// NON_TERMINAL LPAREN NON_TERMINAL NON_TERMINAL RPAREN  ASTERISK NON_TERMINAL			

// compileUnit	: fundefs+=fundef* stats+=stat*

// grammar_rule_RHS
// grammar_rule_RHS rhs_rule_terms
// grammar_rule_RHS  ID PLUSEQUAL NON_TERMINAL ASTERISK
// ID PLUSEQUAL NON_TERMINAL ASTERISK  ID PLUSEQUAL NON_TERMINAL ASTERISK

// Y : a c+= (~X)+ 

grammar_rule_RHS : term* ;      // concatenation

term :        factor ASTERISK		#closureAsterisk // closure
			| factor PLUS			#closurePlus	 // closure
			| factor QMARK			#closureQMark	 // closure
			| factor				#barefact		 // closure
			;

factor :    NOT? (ID EQUAL)? LPAREN term ( OR? term)* RPAREN	#parenthesizedTerm	
		|	NOT? (ID EQUAL)? rhs_rule_terms						#rhsTerm
		|	rule_action											#action
		|	ASSOCIATIVITY										#rhsassociativity
		|   PREDICATE											#rhspredicate
		 ;

rule_action : LBRACE extern_code? RBRACE;

extern_code : LBRACE extern_code RBRACE ;

rhs_rule_terms : (ID (EQUAL|PLUSEQUAL))? (terminal|NON_TERMINAL)		// ok		// x OR X OR 'x' OR x=X OR x+='X'+   ??????????????????????????????????????????????				
			   ;

terminal : TERMINAL								//ok	//	from capital
         | IMPLICIT_TERMINAL					//ok   //  ' everything inside ' 		         
		 ;

/*
grammar_rule_RHS : grammar_rule_RHS  LPAREN grammar_rule_RHS (OR grammar_rule_RHS)* RPAREN	#or_multiple			// x ( x ) OR x (x | x | x)
				 | grammar_rule_RHS  ASTERISK												#closureasterisk		// closure none or more
                 | grammar_rule_RHS  PLUS													#closureplus			// closure one or more
                 | grammar_rule_RHS  QMARK													#closureqmark			// closure none or one
				 | grammar_rule_RHS rhs_rule_terms											#concatenation			// conjuction
				 | grammar_rule_RHS ASSOCIATIVITY											#associate				// conjuction
				 | grammar_rule_RHS  PREDICATE												#multiple_predicate				 
                 | LPAREN  grammar_rule_RHS (OR grammar_rule_RHS)* RPAREN					#or						// ( A | B | x=C )
				 | rhs_rule_terms															#rhs_rule_term			// right hand side rule 
			     | PREDICATE																#predicate				// { conditional expression } ?
				 | rule_action																#ruleaction  			//  x:     { C# }  				 
				 | ASSOCIATIVITY															#associativity			// java grammar associativity in a rule line 543
				 ;
*/


reg_exp : TERMINAL COLON grammar_exp SEMICOLON ;

grammar_exp: grammar_exp grammar_exp					//concatenation	
           | LPAREN  grammar_exp RPAREN					// ( x )
           | LAGKIL grammar_exp RAGKIL					// [ x ]
           | grammar_exp OR grammar_exp					//  x | x
           | grammar_exp PLUS							// x+
           | grammar_exp ASTERISK						// x*
           | grammar_exp QMARK							// x?
		   | grammar_exp NOT? RANGE						// x ~[ everything inside ]
		   | grammar_exp DOT							// x.
		   | grammar_exp PREDICATE						// java 8 predicate in regular expression line 1742
           | NOT grammar_exp							// ~x , 'asm' ~'{'* '{' ~'}'* '}' 
           | grammar_exp DOTS grammar_exp               //'a'..'z'
           | reg_exp_terminal
           | grammar_exp arrow 
           | NOT? RANGE									// ~ [ everything inside ]
		   | CHAR_LITERAL								// '.\'  OR  '.'' OR '\' OR '''
		   | grammar_exp NONGREEDYCLOSURE  grammar_exp  // x 'everything inside' x      ???????????????????????????????????????????????
		   | DOT
		   | PREDICATE									// java 8 grammar predicate in regular expression line 1742
           ;

reg_exp_terminal: TERMINAL								//ok	//	from capital
				| IMPLICIT_TERMINAL						//ok   //  ' everything inside ' 		         
				;

arrow : ARROW ID (RPAREN TERMINAL LPAREN)? ;                // -> skip
