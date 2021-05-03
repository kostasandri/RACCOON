grammar sample_grammar;

init: a | B;

a:  c   #override_c
  | d* #override_d
   ;
  
c: (X | Z)*
 | W
 ;

d: F;
